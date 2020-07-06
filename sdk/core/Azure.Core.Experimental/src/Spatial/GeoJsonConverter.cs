﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Azure.Core.Spatial
{
    /// <summary>
    /// Converts a <see cref="Geometry"/> value from and to JSON in GeoJSON format.
    /// </summary>
    public class GeoJsonConverter : JsonConverter<Geometry>
    {
        private const string PointType = "Point";
        private const string LineStringType = "LineString";
        private const string MultiPointType = "MultiPoint";
        private const string PolygonType = "Polygon";
        private const string MultiLineStringType = "MultiLineString";
        private const string MultiPolygonType = "MultiPolygon";
        private const string GeometryCollectionType = "GeometryCollection";
        private const string TypeProperty = "type";
        private const string GeometriesProperty = "geometries";
        private const string CoordinatesProperty = "coordinates";
        private const string BBoxProperty = "bbox";

        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(Geometry).IsAssignableFrom(typeToConvert);
        }

        /// <inheritdoc />
        public override Geometry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var document = JsonDocument.ParseValue(ref reader);
            return Read(document.RootElement);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Geometry value, JsonSerializerOptions options)
        {
            Write(writer, value);
        }

        internal static Geometry Read(JsonElement element)
        {
            JsonElement typeProperty = GetRequiredProperty(element, TypeProperty);

            string type = typeProperty.GetString();

            GeoBoundingBox? boundingBox = ReadBoundingBox(element);

            if (type == GeometryCollectionType)
            {
                var geometries = new List<Geometry>();
                foreach (var geometry in GetRequiredProperty(element, GeometriesProperty).EnumerateArray())
                {
                    geometries.Add(Read(geometry));
                }

                return new GeoCollection(geometries, boundingBox, ReadAdditionalProperties(element, GeometriesProperty));
            }

            IReadOnlyDictionary<string, object?> additionalProperties = ReadAdditionalProperties(element);
            JsonElement coordinates = GetRequiredProperty(element, CoordinatesProperty);

            switch (type)
            {
                case PointType:
                    return new GeoPoint(ReadCoordinate(coordinates), boundingBox, additionalProperties);
                case LineStringType:
                    return new GeoLine(ReadCoordinates(coordinates), boundingBox, additionalProperties);
                case MultiPointType:
                    var points = new List<GeoPoint>();
                    foreach (GeoCoordinate coordinate in ReadCoordinates(coordinates))
                    {
                        points.Add(new GeoPoint(coordinate, null, Geometry.DefaultProperties));
                    }

                    return new GeoMultiPoint(points, boundingBox, additionalProperties);

                case PolygonType:
                    var rings = new List<GeoLine>();
                    foreach (JsonElement ringArray in coordinates.EnumerateArray())
                    {
                        rings.Add(new GeoLine(ReadCoordinates(ringArray), null, Geometry.DefaultProperties));
                    }

                    return new GeoPolygon(rings, boundingBox, additionalProperties);

                case MultiLineStringType:
                    var lineStrings = new List<GeoLine>();
                    foreach (JsonElement ringArray in coordinates.EnumerateArray())
                    {
                        lineStrings.Add(new GeoLine(ReadCoordinates(ringArray), null, Geometry.DefaultProperties));
                    }

                    return new GeoMultiLine(lineStrings, boundingBox, additionalProperties);

                case MultiPolygonType:

                    var polygons = new List<GeoPolygon>();
                    foreach (JsonElement polygon in coordinates.EnumerateArray())
                    {
                        var polygonRings = new List<GeoLine>();
                        foreach (JsonElement ringArray in polygon.EnumerateArray())
                        {
                            polygonRings.Add(new GeoLine(ReadCoordinates(ringArray), null, Geometry.DefaultProperties));
                        }

                        polygons.Add(new GeoPolygon(polygonRings));
                    }

                    return new GeoMultiPolygon(polygons, boundingBox, additionalProperties);

                default:
                    throw new NotSupportedException($"Unsupported geometry type '{type}' ");
            }
        }

        private static GeoBoundingBox? ReadBoundingBox(in JsonElement element)
        {
            GeoBoundingBox? bbox = null;

            if (element.TryGetProperty(BBoxProperty, out JsonElement bboxElement))
            {
                var arrayLength = bboxElement.GetArrayLength();

                switch (arrayLength)
                {
                    case 4:
                        bbox = new GeoBoundingBox(
                            bboxElement[0].GetDouble(),
                            bboxElement[1].GetDouble(),
                            bboxElement[2].GetDouble(),
                            bboxElement[3].GetDouble()
                        );
                        break;
                    case 6:
                        bbox = new GeoBoundingBox(
                            bboxElement[0].GetDouble(),
                            bboxElement[1].GetDouble(),
                            bboxElement[3].GetDouble(),
                            bboxElement[4].GetDouble(),
                            bboxElement[2].GetDouble(),
                            bboxElement[5].GetDouble()
                        );
                        break;
                    default:
                        throw new JsonException("Only 2 or 3 element coordinates supported");
                }
            }

            return bbox;
        }

        private static IReadOnlyDictionary<string, object?> ReadAdditionalProperties(in JsonElement element, string knownProperty = CoordinatesProperty)
        {
            Dictionary<string, object?>? additionalProperties = null;
            foreach (var property in element.EnumerateObject())
            {
                var propertyName = property.Name;
                if (propertyName.Equals(TypeProperty, StringComparison.Ordinal) ||
                    propertyName.Equals(BBoxProperty, StringComparison.Ordinal) ||
                    propertyName.Equals(knownProperty, StringComparison.Ordinal))
                {
                    continue;
                }

                additionalProperties ??= new Dictionary<string, object?>();
                additionalProperties.Add(propertyName, ReadAdditionalPropertyValue(property.Value));
            }

            return additionalProperties ?? Geometry.DefaultProperties;
        }

        private static object? ReadAdditionalPropertyValue(in JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.String:
                    return element.GetString();
                case JsonValueKind.Number:
                    if (element.TryGetInt32(out int intValue))
                    {
                        return intValue;
                    }
                    if (element.TryGetInt64(out long longValue))
                    {
                        return longValue;
                    }
                    return element.GetDouble();
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Undefined:
                case JsonValueKind.Null:
                    return null;
                case JsonValueKind.Object:
                    var dictionary = new Dictionary<string, object?>();
                    foreach (JsonProperty jsonProperty in element.EnumerateObject())
                    {
                        dictionary.Add(jsonProperty.Name, ReadAdditionalPropertyValue(jsonProperty.Value));
                    }
                    return dictionary;
                case JsonValueKind.Array:
                    var list = new List<object?>();
                    foreach (JsonElement item in element.EnumerateArray())
                    {
                        list.Add(ReadAdditionalPropertyValue(item));
                    }
                    return list.ToArray();
                default:
                    throw new NotSupportedException("Not supported value kind " + element.ValueKind);
            }
        }

        private static IReadOnlyList<GeoCoordinate> ReadCoordinates(JsonElement coordinatesElement)
        {
            GeoCoordinate[] coordinates = new GeoCoordinate[coordinatesElement.GetArrayLength()];

            int i = 0;
            foreach (JsonElement coordinate in coordinatesElement.EnumerateArray())
            {
                 coordinates[i] = ReadCoordinate(coordinate);
                 i++;
            }

            return coordinates;
        }

        private static GeoCoordinate ReadCoordinate(JsonElement coordinate)
        {
            var arrayLength = coordinate.GetArrayLength();
            if (arrayLength < 2 || arrayLength > 3)
            {
                throw new JsonException("Only 2 or 3 element coordinates supported");
            }

            var lon = coordinate[0].GetDouble();
            var lat = coordinate[1].GetDouble();
            double? altitude = null;

            if (arrayLength > 2)
            {
                altitude = coordinate[2].GetDouble();
            }

            return new GeoCoordinate(lon, lat, altitude);
        }

        internal static void Write(Utf8JsonWriter writer, Geometry value)
        {
            void WritePositionValues(GeoCoordinate type)
            {
                writer.WriteNumberValue(type.C1);
                writer.WriteNumberValue(type.C2);
                if (type.C3 != null)
                {
                    writer.WriteNumberValue(type.C3.Value);
                }
            }

            void WriteType(string type)
            {
                writer.WriteString(TypeProperty, type);
            }

            void WritePosition(GeoCoordinate type)
            {
                writer.WriteStartArray();
                WritePositionValues(type);

                writer.WriteEndArray();
            }

            void WritePositions(IEnumerable<GeoCoordinate> positions)
            {
                writer.WriteStartArray();
                foreach (var position in positions)
                {
                    WritePosition(position);
                }

                writer.WriteEndArray();
            }

            writer.WriteStartObject();
            switch (value)
            {
                case GeoPoint point:
                    WriteType(PointType);
                    writer.WritePropertyName(CoordinatesProperty);
                    WritePosition(point.Coordinate);
                    break;

                case GeoLine lineString:
                    WriteType(LineStringType);
                    writer.WritePropertyName(CoordinatesProperty);
                    WritePositions(lineString.Coordinates);
                    break;

                case GeoPolygon polygon:
                    WriteType(PolygonType);
                    writer.WritePropertyName(CoordinatesProperty);
                    writer.WriteStartArray();
                    foreach (var ring in polygon.Rings)
                    {
                        WritePositions(ring.Coordinates);
                    }

                    writer.WriteEndArray();
                    break;

                case GeoMultiPoint multiPoint:
                    WriteType(MultiPointType);
                    writer.WritePropertyName(CoordinatesProperty);
                    writer.WriteStartArray();
                    foreach (var point in multiPoint.Points)
                    {
                        WritePosition(point.Coordinate);
                    }

                    writer.WriteEndArray();
                    break;

                case GeoMultiLine multiLineString:
                    WriteType(MultiLineStringType);
                    writer.WritePropertyName(CoordinatesProperty);
                    writer.WriteStartArray();
                    foreach (var lineString in multiLineString.Lines)
                    {
                        WritePositions(lineString.Coordinates);
                    }

                    writer.WriteEndArray();
                    break;

                case GeoMultiPolygon multiPolygon:
                    WriteType(MultiPolygonType);
                    writer.WritePropertyName(CoordinatesProperty);
                    writer.WriteStartArray();
                    foreach (var polygon in multiPolygon.Polygons)
                    {
                        writer.WriteStartArray();
                        foreach (var polygonRing in polygon.Rings)
                        {
                            WritePositions(polygonRing.Coordinates);
                        }
                        writer.WriteEndArray();
                    }

                    writer.WriteEndArray();
                    break;

                case GeoCollection geometryCollection:
                    WriteType(GeometryCollectionType);
                    writer.WritePropertyName(GeometriesProperty);
                    writer.WriteStartArray();
                    foreach (var geometry in geometryCollection.Geometries)
                    {
                        Write(writer, geometry);
                    }

                    writer.WriteEndArray();
                    break;

                default:
                    throw new NotSupportedException($"Geometry type '{value?.GetType()}' not supported");
            }

            if (value.BoundingBox is GeoBoundingBox bbox)
            {
                writer.WritePropertyName(BBoxProperty);
                writer.WriteStartArray();
                writer.WriteNumberValue(bbox.West);
                writer.WriteNumberValue(bbox.South);
                if (bbox.MinAltitude != null)
                {
                    writer.WriteNumberValue(bbox.MinAltitude.Value);
                }
                writer.WriteNumberValue(bbox.East);
                writer.WriteNumberValue(bbox.North);
                if (bbox.MaxAltitude != null)
                {
                    writer.WriteNumberValue(bbox.MaxAltitude.Value);
                }
                writer.WriteEndArray();
            }

            foreach (var additionalProperty in value.AdditionalProperties)
            {
                writer.WritePropertyName(additionalProperty.Key);
                WriteAdditionalPropertyValue(writer, additionalProperty.Value);
            }

            writer.WriteEndObject();
        }
        private static void WriteAdditionalPropertyValue(Utf8JsonWriter writer, object? value)
        {
            switch (value)
            {
                case null:
                    writer.WriteNullValue();
                    break;
                case int i:
                    writer.WriteNumberValue(i);
                    break;
                case double d:
                    writer.WriteNumberValue(d);
                    break;
                case float f:
                    writer.WriteNumberValue(f);
                    break;
                case long l:
                    writer.WriteNumberValue(l);
                    break;
                case string s:
                    writer.WriteStringValue(s);
                    break;
                case bool b:
                    writer.WriteBooleanValue(b);
                    break;
                case IEnumerable<KeyValuePair<string, object?>> enumerable:
                    writer.WriteStartObject();
                    foreach (KeyValuePair<string, object?> pair in enumerable)
                    {
                        writer.WritePropertyName(pair.Key);
                        WriteAdditionalPropertyValue(writer, pair.Value);
                    }
                    writer.WriteEndObject();
                    break;
                case IEnumerable<object?> objectEnumerable:
                    writer.WriteStartArray();
                    foreach (object? item in objectEnumerable)
                    {
                        WriteAdditionalPropertyValue(writer, item);
                    }
                    writer.WriteEndArray();
                    break;

                default:
                    throw new NotSupportedException("Not supported type " + value.GetType());
            }
        }

        private static JsonElement GetRequiredProperty(JsonElement element, string name)
        {
            if (!element.TryGetProperty(name, out JsonElement property))
            {
                throw new JsonException($"GeoJSON object expected to have '{name}' property.");
            }

            return property;
        }
    }
}
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Azure.Core.Spatial
{
    /// <summary>
    /// Represents a geometry that is composed of multiple <see cref="GeoPolygon"/>.
    /// </summary>
    public sealed class GeoMultiPolygon : Geometry
    {
        /// <summary>
        /// Initializes new instance of <see cref="GeoMultiPolygon"/>.
        /// </summary>
        /// <param name="polygons">The collection of inner polygons.</param>
        public GeoMultiPolygon(IEnumerable<GeoPolygon> polygons): this(polygons, null, DefaultProperties)
        {
        }

        /// <summary>
        /// Initializes new instance of <see cref="GeoMultiPolygon"/>.
        /// </summary>
        /// <param name="polygons">The collection of inner geometries.</param>
        /// <param name="boundingBox">The <see cref="GeoBoundingBox"/> to use.</param>
        /// <param name="additionalProperties">The set of additional properties associated with the <see cref="Geometry"/>.</param>
        public GeoMultiPolygon(IEnumerable<GeoPolygon> polygons, GeoBoundingBox? boundingBox, IReadOnlyDictionary<string, object?> additionalProperties): base(boundingBox, additionalProperties)
        {
            Argument.AssertNotNull(polygons, nameof(polygons));

            Polygons = polygons.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        public IReadOnlyList<GeoPolygon> Polygons { get; }
    }
}
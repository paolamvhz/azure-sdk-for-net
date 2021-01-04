// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Azure.Communication.Administration.Models
{
    /// <summary> The location type of the phone plan. </summary>
    [ExcludeFromCodeCoverage]
    public readonly partial struct LocationType : IEquatable<LocationType>
    {
        private readonly string _value;

        /// <summary> Determines if two <see cref="LocationType"/> values are the same. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public LocationType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string CivicAddressValue = "CivicAddress";
        private const string NotRequiredValue = "NotRequired";
        private const string SelectionValue = "Selection";

        /// <summary> CivicAddress. </summary>
        public static LocationType CivicAddress { get; } = new LocationType(CivicAddressValue);
        /// <summary> NotRequired. </summary>
        public static LocationType NotRequired { get; } = new LocationType(NotRequiredValue);
        /// <summary> Selection. </summary>
        public static LocationType Selection { get; } = new LocationType(SelectionValue);
        /// <summary> Determines if two <see cref="LocationType"/> values are the same. </summary>
        public static bool operator ==(LocationType left, LocationType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="LocationType"/> values are not the same. </summary>
        public static bool operator !=(LocationType left, LocationType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="LocationType"/>. </summary>
        public static implicit operator LocationType(string value) => new LocationType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is LocationType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(LocationType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.Management.Network.Models
{
    /// <summary> Status code of the application gateway customer error. </summary>
    public readonly partial struct ApplicationGatewayCustomErrorStatusCode : IEquatable<ApplicationGatewayCustomErrorStatusCode>
    {
        private readonly string _value;

        /// <summary> Determines if two <see cref="ApplicationGatewayCustomErrorStatusCode"/> values are the same. </summary>
        public ApplicationGatewayCustomErrorStatusCode(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string HttpStatus403Value = "HttpStatus403";
        private const string HttpStatus502Value = "HttpStatus502";

        /// <summary> HttpStatus403. </summary>
        public static ApplicationGatewayCustomErrorStatusCode HttpStatus403 { get; } = new ApplicationGatewayCustomErrorStatusCode(HttpStatus403Value);
        /// <summary> HttpStatus502. </summary>
        public static ApplicationGatewayCustomErrorStatusCode HttpStatus502 { get; } = new ApplicationGatewayCustomErrorStatusCode(HttpStatus502Value);
        /// <summary> Determines if two <see cref="ApplicationGatewayCustomErrorStatusCode"/> values are the same. </summary>
        public static bool operator ==(ApplicationGatewayCustomErrorStatusCode left, ApplicationGatewayCustomErrorStatusCode right) => left.Equals(right);
        /// <summary> Determines if two <see cref="ApplicationGatewayCustomErrorStatusCode"/> values are not the same. </summary>
        public static bool operator !=(ApplicationGatewayCustomErrorStatusCode left, ApplicationGatewayCustomErrorStatusCode right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="ApplicationGatewayCustomErrorStatusCode"/>. </summary>
        public static implicit operator ApplicationGatewayCustomErrorStatusCode(string value) => new ApplicationGatewayCustomErrorStatusCode(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is ApplicationGatewayCustomErrorStatusCode other && Equals(other);
        /// <inheritdoc />
        public bool Equals(ApplicationGatewayCustomErrorStatusCode other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.AI.Inference
{
    /// <summary> A representation of the possible image detail levels for image-based chat completions message content. </summary>
    public readonly partial struct ChatMessageImageDetailLevel : IEquatable<ChatMessageImageDetailLevel>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="ChatMessageImageDetailLevel"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public ChatMessageImageDetailLevel(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string AutoValue = "auto";
        private const string LowValue = "low";
        private const string HighValue = "high";

        /// <summary> Specifies that the model should determine which detail level to apply using heuristics like image size. </summary>
        public static ChatMessageImageDetailLevel Auto { get; } = new ChatMessageImageDetailLevel(AutoValue);
        /// <summary>
        /// Specifies that image evaluation should be constrained to the 'low-res' model that may be faster and consume fewer
        /// tokens but may also be less accurate for highly detailed images.
        /// </summary>
        public static ChatMessageImageDetailLevel Low { get; } = new ChatMessageImageDetailLevel(LowValue);
        /// <summary>
        /// Specifies that image evaluation should enable the 'high-res' model that may be more accurate for highly detailed
        /// images but may also be slower and consume more tokens.
        /// </summary>
        public static ChatMessageImageDetailLevel High { get; } = new ChatMessageImageDetailLevel(HighValue);
        /// <summary> Determines if two <see cref="ChatMessageImageDetailLevel"/> values are the same. </summary>
        public static bool operator ==(ChatMessageImageDetailLevel left, ChatMessageImageDetailLevel right) => left.Equals(right);
        /// <summary> Determines if two <see cref="ChatMessageImageDetailLevel"/> values are not the same. </summary>
        public static bool operator !=(ChatMessageImageDetailLevel left, ChatMessageImageDetailLevel right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="ChatMessageImageDetailLevel"/>. </summary>
        public static implicit operator ChatMessageImageDetailLevel(string value) => new ChatMessageImageDetailLevel(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is ChatMessageImageDetailLevel other && Equals(other);
        /// <inheritdoc />
        public bool Equals(ChatMessageImageDetailLevel other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Azure.Core;

namespace Azure.Communication.Administration.Models
{
    /// <summary> Represents a list of location option queries, used for fetching area codes. </summary>
    [ExcludeFromCodeCoverage]
    internal partial class LocationOptionsQueries
    {
        /// <summary> Initializes a new instance of LocationOptionsQueries. </summary>
        public LocationOptionsQueries()
        {
            LocationOptions = new ChangeTrackingList<LocationOptionsQuery>();
        }

        /// <summary> Represents the underlying list of countries. </summary>
        public IList<LocationOptionsQuery> LocationOptions { get; }
    }
}

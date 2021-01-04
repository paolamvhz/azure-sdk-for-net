// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Azure.Core;

namespace Azure.Communication.Administration.Models
{
    /// <summary> Represents a phone number search. </summary>
    [ExcludeFromCodeCoverage]
    public partial class PhoneNumberReservation
    {
        /// <summary> Initializes a new instance of PhoneNumberReservation. </summary>
        internal PhoneNumberReservation()
        {
            PhonePlanIds = new ChangeTrackingList<string>();
            LocationOptions = new ChangeTrackingList<LocationOptionsDetails>();
            PhoneNumbers = new ChangeTrackingList<string>();
        }

        /// <summary> Initializes a new instance of PhoneNumberReservation. </summary>
        /// <param name="reservationId"> The id of the search. </param>
        /// <param name="displayName"> The name of the search. </param>
        /// <param name="createdAt"> The creation time of the search. </param>
        /// <param name="description"> The description of the search. </param>
        /// <param name="phonePlanIds"> The phone plan ids of the search. </param>
        /// <param name="areaCode"> The area code of the search. </param>
        /// <param name="quantity"> The quantity of phone numbers in the search. </param>
        /// <param name="locationOptions"> The location options of the search. </param>
        /// <param name="status"> The status of the search. </param>
        /// <param name="phoneNumbers"> The list of phone numbers in the search, in the case the status is reserved or success. </param>
        /// <param name="reservationExpiryDate"> The date that search expires and the numbers become available. </param>
        /// <param name="errorCode"> The error code of the search. </param>
        internal PhoneNumberReservation(string reservationId, string displayName, DateTimeOffset? createdAt, string description, IReadOnlyList<string> phonePlanIds, string areaCode, int? quantity, IReadOnlyList<LocationOptionsDetails> locationOptions, ReservationStatus? status, IReadOnlyList<string> phoneNumbers, DateTimeOffset? reservationExpiryDate, int? errorCode)
        {
            ReservationId = reservationId;
            DisplayName = displayName;
            CreatedAt = createdAt;
            Description = description;
            PhonePlanIds = phonePlanIds;
            AreaCode = areaCode;
            Quantity = quantity;
            LocationOptions = locationOptions;
            Status = status;
            PhoneNumbers = phoneNumbers;
            ReservationExpiryDate = reservationExpiryDate;
            ErrorCode = errorCode;
        }
        /// <summary> The name of the search. </summary>
        public string DisplayName { get; }
        /// <summary> The creation time of the search. </summary>
        public DateTimeOffset? CreatedAt { get; }
        /// <summary> The description of the search. </summary>
        public string Description { get; }
        /// <summary> The phone plan ids of the search. </summary>
        public IReadOnlyList<string> PhonePlanIds { get; }
        /// <summary> The area code of the search. </summary>
        public string AreaCode { get; }
        /// <summary> The quantity of phone numbers in the search. </summary>
        public int? Quantity { get; }
        /// <summary> The location options of the search. </summary>
        public IReadOnlyList<LocationOptionsDetails> LocationOptions { get; }
        /// <summary> The status of the search. </summary>
        public ReservationStatus? Status { get; }
        /// <summary> The list of phone numbers in the search, in the case the status is reserved or success. </summary>
        public IReadOnlyList<string> PhoneNumbers { get; }
        /// <summary> The date that search expires and the numbers become available. </summary>
        public DateTimeOffset? ReservationExpiryDate { get; }
        /// <summary> The error code of the search. </summary>
        public int? ErrorCode { get; }
    }
}

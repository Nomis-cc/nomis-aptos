// ------------------------------------------------------------------------------------------------------
// <copyright file="AptoslabsExplorerTokensRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.AptoslabsExplorer.Interfaces.Requests
{
    /// <summary>
    /// Aptoslabs Explorer tokens request.
    /// </summary>
    public class AptoslabsExplorerTokensRequest
    {
        /// <summary>
        /// Owner address.
        /// </summary>
        /// <example>0x8fbb89b979c0a6976e77be646a421a4d40107c20c896b4d11af40d7a44ee1e01</example>
        [JsonPropertyName("owner_address")]
        public string? OwnerAddress { get; set; }

        /// <summary>
        /// Limit.
        /// </summary>
        /// <example>100</example>
        public int Limit { get; set; } = 100;

        /// <summary>
        /// Offset.
        /// </summary>
        /// <example>0</example>
        public int Offset { get; set; } = 0;
    }
}
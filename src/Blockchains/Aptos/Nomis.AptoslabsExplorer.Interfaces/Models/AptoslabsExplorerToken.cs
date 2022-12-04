// ------------------------------------------------------------------------------------------------------
// <copyright file="AptoslabsExplorerToken.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.AptoslabsExplorer.Interfaces.Models
{
    /// <summary>
    /// Aptoslabs Explorer token data.
    /// </summary>
    public class AptoslabsExplorerToken
    {
        /// <summary>
        /// Token data id hash.
        /// </summary>
        [JsonPropertyName("token_data_id_hash")]
        public string? TokenDataIdHash { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Collection name.
        /// </summary>
        [JsonPropertyName("collection_name")]
        public string? CollectionName { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public ulong Amount { get; set; }

        /// <summary>
        /// Last transaction version.
        /// </summary>
        [JsonPropertyName("last_transaction_version")]
        public ulong LastTransactionVersion { get; set; }
    }
}
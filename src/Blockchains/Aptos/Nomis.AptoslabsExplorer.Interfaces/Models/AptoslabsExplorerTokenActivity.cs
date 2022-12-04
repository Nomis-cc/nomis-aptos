// ------------------------------------------------------------------------------------------------------
// <copyright file="AptoslabsExplorerTokenActivity.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.AptoslabsExplorer.Interfaces.Models
{
    /// <summary>
    /// Aptoslabs Explorer token activity data.
    /// </summary>
    public class AptoslabsExplorerTokenActivity
    {
        /// <summary>
        /// Transaction version.
        /// </summary>
        [JsonPropertyName("transaction_version")]
        public ulong TransactionVersion { get; set; }

        /// <summary>
        /// From address.
        /// </summary>
        [JsonPropertyName("from_address")]
        public string? FromAddress { get; set; }

        /// <summary>
        /// To address.
        /// </summary>
        [JsonPropertyName("to_address")]
        public string? ToAddress { get; set; }

        /// <summary>
        /// Token amount.
        /// </summary>
        [JsonPropertyName("token_amount")]
        public ulong TokenAmount { get; set; }

        /// <summary>
        /// Transfer type.
        /// </summary>
        [JsonPropertyName("transfer_type")]
        public string? TransferType { get; set; }

        /// <summary>
        /// Token data id hash.
        /// </summary>
        [JsonPropertyName("token_data_id_hash")]
        public string? TokenDataIdHash { get; set; }
    }
}
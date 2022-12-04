// ------------------------------------------------------------------------------------------------------
// <copyright file="AptoslabsExplorerCoinActivity.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.AptoslabsExplorer.Interfaces.Models
{
    /// <summary>
    /// Aptoslabs Explorer coin activity data.
    /// </summary>
    public class AptoslabsExplorerCoinActivity
    {
        /// <summary>
        /// Activity type.
        /// </summary>
        [JsonPropertyName("activity_type")]
        public string? ActivityType { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public ulong Amount { get; set; }

        /// <summary>
        /// Coin type.
        /// </summary>
        [JsonPropertyName("coin_type")]
        public string? CoinType { get; set; }

        /// <summary>
        /// Entry function id string.
        /// </summary>
        [JsonPropertyName("entry_function_id_str")]
        public string? EntryFunctionIdStr { get; set; }

        /// <summary>
        /// Transaction version.
        /// </summary>
        [JsonPropertyName("transaction_version")]
        public ulong TransactionVersion { get; set; }

        /// <summary>
        /// Transaction timestamp.
        /// </summary>
        [JsonPropertyName("transaction_timestamp")]
        public string? Timestamp { get; set; }

        /// <summary>
        /// Is transaction success.
        /// </summary>
        [JsonPropertyName("is_transaction_success")]
        public bool IsTransactionSuccess { get; set; }
    }
}
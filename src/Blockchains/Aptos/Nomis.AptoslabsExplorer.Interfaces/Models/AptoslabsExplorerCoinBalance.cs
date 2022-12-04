// ------------------------------------------------------------------------------------------------------
// <copyright file="AptoslabsExplorerCoinBalance.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Nomis.AptoslabsExplorer.Interfaces.Models
{
    /// <summary>
    /// Aptoslabs Explorer coin balance.
    /// </summary>
    public class AptoslabsExplorerCoinBalance
    {
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
        /// Coin info.
        /// </summary>
        [JsonPropertyName("coin_info")]
        public AptoslabsExplorerCoinBalanceCoinInfo? CoinInfo { get; set; }
    }
}
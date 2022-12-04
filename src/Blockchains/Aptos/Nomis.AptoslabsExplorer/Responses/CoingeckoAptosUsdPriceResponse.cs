// ------------------------------------------------------------------------------------------------------
// <copyright file="CoingeckoAptosUsdPriceResponse.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json.Serialization;

using Nomis.Coingecko.Interfaces.Models;

namespace Nomis.AptoslabsExplorer.Responses
{
    /// <summary>
    /// Coingecko APT USD price response.
    /// </summary>
    public class CoingeckoAptosUsdPriceResponse :
        ICoingeckoUsdPriceResponse
    {
        /// <inheritdoc cref="CoingeckoUsdPriceData"/>
        [JsonPropertyName("aptos")]
        public CoingeckoUsdPriceData? Data { get; set; }
    }
}
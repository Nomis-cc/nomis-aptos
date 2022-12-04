// ------------------------------------------------------------------------------------------------------
// <copyright file="AptoslabsExplorerService.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Nodes;

using GraphQL;
using Nomis.AptoslabsExplorer.Calculators;
using Nomis.AptoslabsExplorer.Interfaces;
using Nomis.AptoslabsExplorer.Interfaces.Extensions;
using Nomis.AptoslabsExplorer.Interfaces.Models;
using Nomis.AptoslabsExplorer.Interfaces.Requests;
using Nomis.AptoslabsExplorer.Responses;
using Nomis.Blockchain.Abstractions.Converters;
using Nomis.Coingecko.Interfaces;
using Nomis.Domain.Scoring.Entities;
using Nomis.ScoringService.Interfaces;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Wrapper;

// ReSharper disable InconsistentNaming

namespace Nomis.AptoslabsExplorer
{
    /// <inheritdoc cref="IAptosScoringService"/>
    internal sealed class AptoslabsExplorerService :
        IAptosScoringService,
        ITransientService
    {
        private readonly IAptoslabsExplorerGraphQLClient _client;
        private readonly ICoingeckoService _coingeckoService;
        private readonly IScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="AptoslabsExplorerService"/>.
        /// </summary>
        /// <param name="client"><see cref="IAptoslabsExplorerGraphQLClient"/>.</param>
        /// <param name="coingeckoService"><see cref="ICoingeckoService"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        public AptoslabsExplorerService(
            IAptoslabsExplorerGraphQLClient client,
            ICoingeckoService coingeckoService,
            IScoringService scoringService)
        {
            _client = client;
            _coingeckoService = coingeckoService;
            _scoringService = scoringService;
        }

        /// <inheritdoc />
        public ulong ChainId => 111117;

        /// <inheritdoc/>
        public async Task<Result<AptosWalletScore>> GetWalletStatsAsync(string address, CancellationToken cancellationToken = default)
        {
            var coinBalances = (await GetAptoslabsExplorerCoinBalancesAsync(new AptoslabsExplorerCoinBalancesRequest
            {
                OwnerAddress = address
            })).Data;
            ulong balanceWei = coinBalances
                .FirstOrDefault(x => x.CoinType?.Equals("0x1::aptos_coin::AptosCoin", StringComparison.InvariantCultureIgnoreCase) == true)?
                .Amount ?? 0;
            decimal usdBalance = await _coingeckoService.GetUsdBalanceAsync<CoingeckoAptosUsdPriceResponse>(balanceWei.ToAptos(), "aptos");
            var coinActivities = (await GetAptoslabsExplorerCoinActivitiesAsync(new AptoslabsExplorerCoinActivitiesRequest
            {
                OwnerAddress = address
            })).Data;
            var tokens = (await GetAptoslabsExplorerTokensAsync(new AptoslabsExplorerTokensRequest
            {
                OwnerAddress = address
            })).Data;
            var tokenActivities = (await GetAptoslabsExplorerTokenActivitiesAsync(new AptoslabsExplorerTokenActivitiesRequest
            {
                Address = address
            })).Data;

            var walletStats = new AptosStatCalculator(
                    address,
                    decimal.TryParse(balanceWei.ToString(), out decimal wei) ? wei : 0,
                    usdBalance,
                    coinBalances,
                    coinActivities,
                    tokens,
                    tokenActivities)
                .GetStats();

            double score = walletStats.GetScore();
            var scoringData = new ScoringData(address, address, ChainId, score, JsonSerializer.Serialize(walletStats));
            await _scoringService.SaveScoringDataToDatabaseAsync(scoringData, cancellationToken);

            return await Result<AptosWalletScore>.SuccessAsync(
                new()
                {
                    Address = address,
                    Stats = walletStats,
                    Score = score
                }, "Got aptos wallet score.");
        }

        private async Task<List<TResult>> GetDataAsync<TResult>(GraphQLRequest query, string responseAlias)
        {
            var response = await _client.SendQueryAsync<JsonObject>(query);
            var data = JsonSerializer.Deserialize<List<TResult>>(response.Data[responseAlias]?.ToJsonString(new()
            {
                Converters = { new BigIntegerConverter() }
            }) !) ?? new List<TResult>();

            return data;
        }

        private async Task<Result<List<AptoslabsExplorerCoinBalance>>> GetAptoslabsExplorerCoinBalancesAsync(
            AptoslabsExplorerCoinBalancesRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query CoinsData($owner_address: String, $limit: Int, $offset: Int) {
                  current_coin_balances(
                    where: {owner_address: {_eq: $owner_address}, amount: {_gt: ""0""}}
                    order_by: {last_transaction_version: desc}
                    limit: $limit
                    offset: $offset
                  ) {
                    amount
                    coin_type
                    coin_info {
                      name
                      decimals
                      symbol
                    }
                  }
                }
                ",
                Variables = request
            };

            var result = new List<AptoslabsExplorerCoinBalance>();
            var data = await GetDataAsync<AptoslabsExplorerCoinBalance>(query, "current_coin_balances");
            result.AddRange(data);
            while (data.Count > 0 && data.Count == request.Limit)
            {
                request = new AptoslabsExplorerCoinBalancesRequest
                {
                    OwnerAddress = request.OwnerAddress,
                    Limit = request.Limit,
                    Offset = request.Offset + request.Limit
                };
                query.Variables = request;
                data = await GetDataAsync<AptoslabsExplorerCoinBalance>(query, "current_coin_balances");
                result.AddRange(data);
            }

            return await Result<List<AptoslabsExplorerCoinBalance>>.SuccessAsync(result, "Coin balances received.");
        }

        private async Task<Result<List<AptoslabsExplorerCoinActivity>>> GetAptoslabsExplorerCoinActivitiesAsync(
            AptoslabsExplorerCoinActivitiesRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query CoinActivity($owner_address: String, $offset: Int, $limit: Int) {
                  coin_activities(
                    where: {owner_address: {_eq: $owner_address}}
                    order_by: {transaction_version: desc}
                    offset: $offset
                    limit: $limit
                  ) {
                    activity_type
                    amount
                    coin_type
                    entry_function_id_str
                    transaction_version
                    transaction_timestamp
                    is_transaction_success
                  }
                }
                ",
                Variables = request
            };

            var result = new List<AptoslabsExplorerCoinActivity>();
            var data = await GetDataAsync<AptoslabsExplorerCoinActivity>(query, "coin_activities");
            result.AddRange(data);
            while (data.Count > 0 && data.Count == request.Limit)
            {
                request = new AptoslabsExplorerCoinActivitiesRequest
                {
                    OwnerAddress = request.OwnerAddress,
                    Limit = request.Limit,
                    Offset = request.Offset + request.Limit
                };
                query.Variables = request;
                data = await GetDataAsync<AptoslabsExplorerCoinActivity>(query, "coin_activities");
                result.AddRange(data);
            }

            return await Result<List<AptoslabsExplorerCoinActivity>>.SuccessAsync(result, "Coin coin activities received.");
        }

        private async Task<Result<List<AptoslabsExplorerToken>>> GetAptoslabsExplorerTokensAsync(
            AptoslabsExplorerTokensRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query CurrentTokens($owner_address: String, $offset: Int, $limit: Int) {
                  current_token_ownerships(
                    where: {owner_address: {_eq: $owner_address}, amount: {_gt: ""0""}, table_type: {_eq: ""0x3::token::TokenStore""}}
                    order_by: {last_transaction_version: desc}
                    offset: $offset
                    limit: $limit
                  ) {
                    token_data_id_hash
                    name
                    collection_name
                    property_version
                    amount
                    last_transaction_version
                  }
                }
                ",
                Variables = request
            };

            var result = new List<AptoslabsExplorerToken>();
            var data = await GetDataAsync<AptoslabsExplorerToken>(query, "current_token_ownerships");
            result.AddRange(data);
            while (data.Count > 0 && data.Count == request.Limit)
            {
                request = new AptoslabsExplorerTokensRequest
                {
                    OwnerAddress = request.OwnerAddress,
                    Limit = request.Limit,
                    Offset = request.Offset + request.Limit
                };
                query.Variables = request;
                data = await GetDataAsync<AptoslabsExplorerToken>(query, "current_token_ownerships");
                result.AddRange(data);
            }

            return await Result<List<AptoslabsExplorerToken>>.SuccessAsync(result, "Coin tokens received.");
        }

        private async Task<Result<List<AptoslabsExplorerTokenActivity>>> GetAptoslabsExplorerTokenActivitiesAsync(
            AptoslabsExplorerTokenActivitiesRequest request)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                        query TokenActivities($address: String, $offset: Int, $limit: Int) {
                          token_activities(
                            where: {_or: [{to_address: {_eq: $address}}, {from_address: {_eq: $address}}]}
                            order_by: {transaction_version: desc}
                            offset: $offset
                            limit: $limit
                          ) {
                            transaction_version
                            from_address
                            to_address
                            token_amount
                            transfer_type
                            token_data_id_hash
                          }
                        }
                ",
                Variables = request
            };

            var result = new List<AptoslabsExplorerTokenActivity>();
            var data = await GetDataAsync<AptoslabsExplorerTokenActivity>(query, "token_activities");
            result.AddRange(data);
            while (data.Count > 0 && data.Count == request.Limit)
            {
                request = new AptoslabsExplorerTokenActivitiesRequest
                {
                    Address = request.Address,
                    Limit = request.Limit,
                    Offset = request.Offset + request.Limit
                };
                query.Variables = request;
                data = await GetDataAsync<AptoslabsExplorerTokenActivity>(query, "token_activities");
                result.AddRange(data);
            }

            return await Result<List<AptoslabsExplorerTokenActivity>>.SuccessAsync(result, "Coin token activities received.");
        }
    }
}
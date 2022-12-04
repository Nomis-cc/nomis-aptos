// ------------------------------------------------------------------------------------------------------
// <copyright file="AptosStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.AptoslabsExplorer.Extensions;
using Nomis.AptoslabsExplorer.Interfaces.Extensions;
using Nomis.AptoslabsExplorer.Interfaces.Models;
using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;

namespace Nomis.AptoslabsExplorer.Calculators
{
    /// <summary>
    /// Aptos wallet stats calculator.
    /// </summary>
    internal sealed class AptosStatCalculator :
        IStatCalculator<AptosWalletStats, AptosTransactionIntervalData>
    {
        private readonly string _address;
        private readonly decimal _balance;
        private readonly decimal _usdBalance;
        private readonly IEnumerable<AptoslabsExplorerCoinBalance> _coinBalances;
        private readonly IEnumerable<AptoslabsExplorerCoinActivity> _coinActivities;
        private readonly IEnumerable<AptoslabsExplorerToken> _tokens;
        private readonly IEnumerable<AptoslabsExplorerTokenActivity> _tokenActivities;

        public AptosStatCalculator(
            string address,
            decimal balance,
            decimal usdBalance,
            IEnumerable<AptoslabsExplorerCoinBalance> coinBalances,
            IEnumerable<AptoslabsExplorerCoinActivity> coinActivities,
            IEnumerable<AptoslabsExplorerToken> tokens,
            IEnumerable<AptoslabsExplorerTokenActivity> tokenActivities)
        {
            _address = address;
            _balance = balance;
            _usdBalance = usdBalance;
            _coinBalances = coinBalances;
            _coinActivities = coinActivities;
            _tokens = tokens;
            _tokenActivities = tokenActivities;
        }

        public AptosWalletStats GetStats()
        {
            if (!_coinActivities.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator<AptosWalletStats, AptosTransactionIntervalData>
                .GetTransactionsIntervals(_coinActivities.Select(x => x.Timestamp!.ToAptosDateTime())).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            var soldTokens = _tokenActivities.Where(x => x.FromAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true).ToList();
            var soldSum = IStatCalculator<AptosWalletStats, AptosTransactionIntervalData>
                .GetTokensSum(soldTokens.Select(x => x.TransactionVersion.ToString()), _coinActivities.Select(x => (x.TransactionVersion.ToString(), new BigInteger(x.Amount))));

            var soldTokensIds = soldTokens.Select(x => x.TokenDataIdHash);
            var buyTokens = _tokenActivities.Where(x => x.ToAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && soldTokensIds.Contains(x.TokenDataIdHash));
            var buySum = IStatCalculator<AptosWalletStats, AptosTransactionIntervalData>
                .GetTokensSum(buyTokens.Select(x => x.TransactionVersion.ToString()), _coinActivities.Select(x => (x.TransactionVersion.ToString(), new BigInteger(x.Amount))));

            var buyNotSoldTokens = _tokenActivities.Where(x => x.ToAddress?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true && !soldTokensIds.Contains(x.TokenDataIdHash));
            var buyNotSoldSum = IStatCalculator<AptosWalletStats, AptosTransactionIntervalData>
                .GetTokensSum(buyNotSoldTokens.Select(x => x.TransactionVersion.ToString()), _coinActivities.Select(x => (x.TransactionVersion.ToString(), new BigInteger(x.Amount))));

            int holdingTokens = _tokens.Count();
            decimal nftWorth = buySum == 0 ? 0 : (decimal)soldSum / (decimal)buySum * (decimal)buyNotSoldSum;
            int contractsCreated = _coinActivities.Count(x => x.EntryFunctionIdStr?.Equals("0x1::code::publish_package_txn", StringComparison.InvariantCultureIgnoreCase) == true);
            var totalTokens = _coinBalances.Select(x => x.CoinInfo?.Symbol).Distinct();

            var turnoverIntervalsDataList =
                _coinActivities.Select(x => new TurnoverIntervalsData(
                    x.Timestamp!.ToAptosDateTime(),
                    new BigInteger(x.Amount),
                    x.ActivityType?.Equals("0x1::coin::DepositEvent", StringComparison.InvariantCultureIgnoreCase) == true));
            var turnoverIntervals = IStatCalculator<AptosWalletStats, AptosTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _coinActivities.Min(x => x.Timestamp!.ToAptosDateTime())).ToList();

            return new()
            {
                Balance = _balance.ToAptos(),
                BalanceUSD = _usdBalance,
                WalletAge = IStatCalculator<AptosWalletStats, AptosTransactionIntervalData>
                    .GetWalletAge(_coinActivities.Select(x => x.Timestamp!.ToAptosDateTime())),
                TotalTransactions = _coinActivities.Count(),
                TotalRejectedTransactions = _coinActivities.Count(t => !t.IsTransactionSuccess),
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = _coinActivities.Sum(x => (decimal)x.Amount).ToAptos(),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<AptosWalletStats, AptosTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<AptosWalletStats, AptosTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = _coinActivities.Count(x => x.Timestamp!.ToAptosDateTime() > monthAgo),
                LastYearTransactions = _coinActivities.Count(x => x.Timestamp!.ToAptosDateTime() > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _coinActivities.OrderBy(x => x.Timestamp).Last().Timestamp!.ToAptosDateTime()).TotalDays / 30),
                NftHolding = holdingTokens,
                NftTrading = (soldSum - buySum).ToAptos(),
                NftWorth = nftWorth.ToAptos(),
                DeployedContracts = contractsCreated,
                TokensHolding = totalTokens.Count()
            };
        }
    }
}
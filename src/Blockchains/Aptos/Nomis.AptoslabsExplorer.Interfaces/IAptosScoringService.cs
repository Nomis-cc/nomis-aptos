// ------------------------------------------------------------------------------------------------------
// <copyright file="IAptosScoringService.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.AptoslabsExplorer.Interfaces.Models;
using Nomis.Blockchain.Abstractions;
using Nomis.Utils.Contracts.Services;

namespace Nomis.AptoslabsExplorer.Interfaces
{
    /// <summary>
    /// Aptos scoring service.
    /// </summary>
    public interface IAptosScoringService :
        IBlockchainScoringService<AptosWalletScore>,
        IBlockchainDescriptor,
        IInfrastructureService
    {
    }
}
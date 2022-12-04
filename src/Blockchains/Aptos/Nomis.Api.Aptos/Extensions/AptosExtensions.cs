// ------------------------------------------------------------------------------------------------------
// <copyright file="AptosExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Api.Aptos.Settings;
using Nomis.Api.Common.Extensions;
using Nomis.AptoslabsExplorer.Interfaces;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Aptos.Extensions
{
    /// <summary>
    /// Aptos extension methods.
    /// </summary>
    public static class AptosExtensions
    {
        /// <summary>
        /// Add Aptos blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithAptosBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IAptosServiceRegistrar, new()
        {
            return optionsBuilder
                .With<AptosAPISettings, TServiceRegistrar>();
        }
    }
}
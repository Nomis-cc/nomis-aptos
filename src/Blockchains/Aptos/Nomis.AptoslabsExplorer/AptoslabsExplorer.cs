// ------------------------------------------------------------------------------------------------------
// <copyright file="AptoslabsExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.AptoslabsExplorer.Extensions;
using Nomis.AptoslabsExplorer.Interfaces;

namespace Nomis.AptoslabsExplorer
{
    /// <summary>
    /// Aptoslabs Explorer service registrar.
    /// </summary>
    public sealed class AptoslabsExplorer :
        IAptosServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddAptoslabsExplorerService();
        }
    }
}
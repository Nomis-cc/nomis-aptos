// ------------------------------------------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomis.AptoslabsExplorer.Interfaces;
using Nomis.AptoslabsExplorer.Settings;
using Nomis.Coingecko.Interfaces;
using Nomis.ScoringService.Interfaces;
using Nomis.Utils.Extensions;

namespace Nomis.AptoslabsExplorer.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add Aptoslabs Explorer service.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        /// <returns>Returns <see cref="IServiceCollection"/>.</returns>
        internal static IServiceCollection AddAptoslabsExplorerService(
            this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var coingeckoService = serviceProvider.GetRequiredService<ICoingeckoService>();
            services.AddSettings<AptoslabsExplorerSettings>(configuration);
            var settings = configuration.GetSettings<AptoslabsExplorerSettings>();
            var scoringService = serviceProvider.GetRequiredService<IScoringService>();
            services.AddSingleton<IAptoslabsExplorerGraphQLClient>(_ =>
            {
                var graphQlOptions = new GraphQLHttpClientOptions
                {
                    EndPoint = new(settings.ApiBaseUrl!)
                };
                return new AptoslabsExplorerGraphQLClient(graphQlOptions, new SystemTextJsonSerializer());
            });

            return services.AddTransient<IAptosScoringService>(ctx =>
            {
                var graphQlClient = ctx.GetRequiredService<IAptoslabsExplorerGraphQLClient>();
                return new AptoslabsExplorerService(graphQlClient, coingeckoService, scoringService);
            });
        }
    }
}
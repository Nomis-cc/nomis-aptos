// ------------------------------------------------------------------------------------------------------
// <copyright file="PingResponseExample.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Swashbuckle.AspNetCore.Filters;

namespace Nomis.Api.Common.Swagger.Examples
{
    /// <summary>
    /// Example response for Ping action.
    /// </summary>
    public class PingResponseExample : IExamplesProvider<object>
    {
        /// <inheritdoc/>
        public object GetExamples()
        {
            return "Ok";
        }
    }
}
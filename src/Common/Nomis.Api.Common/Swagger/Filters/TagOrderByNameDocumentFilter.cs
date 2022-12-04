// ------------------------------------------------------------------------------------------------------
// <copyright file="TagOrderByNameDocumentFilter.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Nomis.Api.Common.Swagger.Filters
{
    /// <summary>
    /// Filter for ordering tags by name in OpenApi document.
    /// </summary>
    public class TagOrderByNameDocumentFilter :
        IDocumentFilter
    {
        /// <summary>
        /// Apply filter.
        /// </summary>
        /// <param name="swaggerDoc"><see cref="OpenApiDocument" />.</param>
        /// <param name="context"><see cref="DocumentFilterContext" />.</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context) => swaggerDoc.Tags = swaggerDoc.Tags.OrderBy(t => t.Name).ToList();
    }
}
// ------------------------------------------------------------------------------------------------------
// <copyright file="AptosController.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nomis.AptoslabsExplorer.Interfaces;
using Nomis.AptoslabsExplorer.Interfaces.Models;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Aptos
{
    /// <summary>
    /// A controller to aggregate all Aptos-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Aptos blockchain.")]
    public sealed class AptosController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/aptos";

        /// <summary>
        /// Common tag for Aptos actions.
        /// </summary>
        internal const string AptosTag = "Aptos";

        private readonly ILogger<AptosController> _logger;
        private readonly IAptosScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="AptosController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IAptosScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public AptosController(
            IAptosScoringService scoringService,
            ILogger<AptosController> logger)
        {
            _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get Nomis Score for given wallet address.
        /// </summary>
        /// <param name="address" example="0x8fbb89b979c0a6976e77be646a421a4d40107c20c896b4d11af40d7a44ee1e01">Aptos wallet address to get Nomis Score.</param>
        /// <returns>An Nomis Score value and corresponding statistical data.</returns>
        /// <remarks>
        /// Sample request:
        ///     GET /api/v1/aptos/wallet/0x8fbb89b979c0a6976e77be646a421a4d40107c20c896b4d11af40d7a44ee1e01/score
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetAptosWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetAptosWalletScore",
            Tags = new[] { AptosTag })]
        [ProducesResponseType(typeof(Result<AptosWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAptosWalletScoreAsync(
            [Required(ErrorMessage = "Wallet address should be set")] string address)
        {
            var result = await _scoringService.GetWalletStatsAsync(address);
            return Ok(result);
        }
    }
}
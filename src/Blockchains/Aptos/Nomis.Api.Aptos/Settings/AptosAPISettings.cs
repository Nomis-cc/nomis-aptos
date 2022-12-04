// ------------------------------------------------------------------------------------------------------
// <copyright file="AptosAPISettings.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Utils.Contracts.Common;

namespace Nomis.Api.Aptos.Settings
{
    /// <summary>
    /// Aptos API settings.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal class AptosAPISettings :
        IAPISettings
    {
        /// <inheritdoc/>
        public bool APIEnabled { get; set; }

        /// <inheritdoc/>
        public string APIName => AptosController.AptosTag;

        /// <inheritdoc/>
        public string ControllerName => nameof(AptosController);
    }
}
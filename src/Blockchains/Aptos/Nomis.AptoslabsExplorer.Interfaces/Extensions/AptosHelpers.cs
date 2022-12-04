// ------------------------------------------------------------------------------------------------------
// <copyright file="AptosHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

namespace Nomis.AptoslabsExplorer.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for Aptos.
    /// </summary>
    public static class AptosHelpers
    {
        /// <summary>
        /// Convert Wei value to Aptos.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total Aptos.</returns>
        public static decimal ToAptos(this ulong valueInWei)
        {
            return valueInWei * 0.00_000_001M;
        }

        /// <summary>
        /// Convert Wei value to Aptos.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total Aptos.</returns>
        public static decimal ToAptos(this in BigInteger valueInWei)
        {
            return (decimal)valueInWei * 0.00_000_001M;
        }

        /// <summary>
        /// Convert Wei value to Aptos.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total Aptos.</returns>
        public static decimal ToAptos(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToAptos();
        }
    }
}
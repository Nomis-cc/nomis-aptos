// ------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2022. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Globalization;

namespace Nomis.AptoslabsExplorer.Extensions
{
    /// <summary>
    /// Extension methods for converting DateTime.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert TimeStamp to DateTime.
        /// </summary>
        /// <param name="timeStamp">TimeStamp in string.</param>
        /// <returns><see cref="DateTime"/>.</returns>
        public static DateTime ToAptosDateTime(this string timeStamp)
        {
            return DateTime.ParseExact(timeStamp, "s", CultureInfo.InvariantCulture);
        }
    }
}
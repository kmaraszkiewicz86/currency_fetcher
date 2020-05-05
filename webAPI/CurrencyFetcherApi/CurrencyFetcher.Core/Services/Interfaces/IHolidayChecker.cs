using System;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    /// <summary>
    /// Change date when is set at holiday or weekend day
    /// </summary>
    public interface IHolidayChecker
    {
        /// <summary>
        /// Change date when is set at holiday or weekend day
        /// </summary>
        /// <param name="value">Date to check if is holiday and change it</param>
        /// <returns>Date that is not holiday date</returns>
        DateTime ReturnDateBeforeDayOff(DateTime value);
    }
}

using System;
using CurrencyFetcher.Core.Services.Interfaces;
using Nager.Date;

namespace CurrencyFetcher.Core.Services.Implementations
{
    /// <summary>
    /// Change date when is set at holiday or weekend day
    /// </summary>
    public class HolidayChecker: IHolidayChecker
    {
        /// <summary>
        /// Change date when is set at holiday or weekend day
        /// </summary>
        /// <param name="value">Date to check if is holiday and change it</param>
        /// <returns>Date that is not holiday date</returns>
        public DateTime ReturnDateBeforeDayOff(DateTime value)
        {
            while (true)
            {
                if (!DateSystem.IsPublicHoliday(value, CountryCode.PL) && !DateSystem.IsWeekend(value, CountryCode.PL))
                {
                    break;
                }

                value = value.AddDays(-1);
            }

            return value;
        }
    }
}

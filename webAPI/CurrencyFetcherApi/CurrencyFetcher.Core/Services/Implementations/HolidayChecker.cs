using System;
using CurrencyFetcher.Core.Services.Interfaces;
using Nager.Date;

namespace CurrencyFetcher.Core.Services.Implementations
{
    public class HolidayChecker: IHolidayChecker
    {
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

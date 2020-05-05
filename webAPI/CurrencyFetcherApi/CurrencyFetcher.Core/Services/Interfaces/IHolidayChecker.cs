using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    public interface IHolidayChecker
    {
        DateTime ReturnDateBeforeDayOff(DateTime value);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    public interface IDateChecker
    {
        (DateTime StartDate, DateTime EndDate) SetCorrectDate(DateTime startDate, DateTime? endDate);

        void ValidateDate(DateTime startDate, DateTime? endDate);
    }
}

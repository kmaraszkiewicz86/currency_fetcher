using System;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Services.Interfaces;
using Nager.Date;

namespace CurrencyFetcher.Core.Services.Implementations
{
    public class DateChecker: IDateChecker
    {
        public (DateTime StartDate, DateTime EndDate) SetCurrentDate(DateTime startDate, DateTime? endDate)
        {
            var startDateTmp = startDate;
            var endDateTmp = endDate ?? startDate;

            while (true)
            {
                if (!DateSystem.IsPublicHoliday(startDateTmp, CountryCode.PL) && !DateSystem.IsWeekend(startDateTmp, CountryCode.PL))
                {
                    break;
                }

                startDateTmp = startDateTmp.AddDays(-1);
            }

            return (startDateTmp, endDateTmp);
        }

        public void ValidateDate(DateTime startDate, DateTime? endDate)
        {
            var currentDate = DateTime.UtcNow;

            if (currentDate < startDate)
            {
                throw new NotFoundException("The startDate could not be after current date");
            }
            
            if (endDate.HasValue && currentDate < endDate)
            {
                throw new NotFoundException("The endDate could not be after current date");
            }

            if (endDate.HasValue && startDate > endDate)
            {
                throw new BadRequestException("The endDate could not be after startDate date");
            }
        }
    }
}

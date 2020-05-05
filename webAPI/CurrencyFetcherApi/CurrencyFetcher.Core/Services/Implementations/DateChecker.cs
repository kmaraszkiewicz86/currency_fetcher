using System;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Services.Interfaces;

namespace CurrencyFetcher.Core.Services.Implementations
{
    public class DateChecker: IDateChecker
    {
        private readonly IHolidayChecker _holidayChecker;

        public DateChecker(IHolidayChecker holidayChecker)
        {
            _holidayChecker = holidayChecker;
        }

        public (DateTime StartDate, DateTime EndDate) SetCorrectDate(DateTime startDate, DateTime? endDate)
        {
            var startDateTmp = _holidayChecker.ReturnDateBeforeDayOff(startDate);
            var endDateTmp = endDate ?? startDateTmp;

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

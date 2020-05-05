using System;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Services.Interfaces;

namespace CurrencyFetcher.Core.Services.Implementations
{
    /// <summary>
    /// Helps to handles dates in system
    /// </summary>
    public class DateService: IDateService
    {
        /// <summary>
        /// <see cref="IHolidayChecker"/>
        /// </summary>
        private readonly IHolidayChecker _holidayChecker;

        /// <summary>
        /// Creates instance of class
        /// </summary>
        /// <param name="holidayChecker"><see cref="IHolidayChecker"/></param>
        public DateService(IHolidayChecker holidayChecker)
        {
            _holidayChecker = holidayChecker;
        }

        /// <summary>
        /// Changes date that date is not a holiday day and check if endDate is null
        /// If yes then set value as startDate
        /// </summary>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <returns>Tuple of start and end date</returns>
        public (DateTime StartDate, DateTime EndDate) SetCorrectDate(DateTime startDate, DateTime? endDate)
        {
            var startDateTmp = _holidayChecker.ReturnDateBeforeDayOff(startDate);
            var endDateTmp = endDate ?? startDateTmp;

            return (startDateTmp, endDateTmp);
        }

        /// <summary>
        /// Validate dates
        /// </summary>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
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
                throw new BadRequestException("The endDate could not be before startDate date");
            }
        }
    }
}

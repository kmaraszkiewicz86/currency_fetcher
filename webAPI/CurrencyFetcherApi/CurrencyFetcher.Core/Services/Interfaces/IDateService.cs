using System;

namespace CurrencyFetcher.Core.Services.Interfaces
{
    /// <summary>
    /// Helps to handles dates in system
    /// </summary>
    public interface IDateService
    {
        /// <summary>
        /// Changes date that date is not a holiday day and check if endDate is null
        /// If yes then set value as startDate
        /// </summary>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <returns>Tuple of start and end date</returns>
        (DateTime StartDate, DateTime EndDate) SetCorrectDate(DateTime startDate, DateTime? endDate);

        /// <summary>
        /// Validate dates
        /// </summary>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        void ValidateDate(DateTime startDate, DateTime? endDate);
    }
}

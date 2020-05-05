using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Core;
using CurrencyFetcher.Core.Entities;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrencyFetcher.Core.Services.Implementations
{
    /// <summary>
    /// Caches currency information in database
    /// </summary>
    public class CacheDatabase : ICacheDatabase
    {
        /// <summary>
        /// <see cref="CurrencyDbContext"/>
        /// </summary>
        private readonly CurrencyDbContext _dbContext;

        /// <summary>
        /// <see cref="IHolidayChecker"/>
        /// </summary>
        private readonly IHolidayChecker _holidayChecker;

        /// <summary>
        /// Creates instance of class
        /// </summary>
        /// <param name="dbContext"><see cref="CurrencyDbContext"/></param>
        /// <param name="holidayChecker"><see cref="IHolidayChecker"/></param>
        public CacheDatabase(CurrencyDbContext dbContext, IHolidayChecker holidayChecker)
        {
            _dbContext = dbContext;
            _holidayChecker = holidayChecker;
        }

        /// <summary>
        /// Saves <see cref="CurrencyModel"/> into database
        /// </summary>
        /// <param name="result"><see cref="CurrencyModel"/></param>
        /// <returns></returns>
        public async Task SaveAsync(CurrencyResultResponse result)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var currencyValue = await _dbContext.CurrencyValues.Include(c => c.Currency)
                        .FirstOrDefaultAsync(c =>
                            c.DailyDataOfCurrency == result.DailyDataOfCurrency &&
                            c.Currency.CurrencyBeingMeasured == result.CurrencyBeingMeasured &&
                            c.Currency.CurrencyMatched == result.CurrencyMatched);

                    if (currencyValue != null)
                    {
                        return;
                    }

                    var currency = await _dbContext.Currencies.FirstOrDefaultAsync(c =>
                        c.CurrencyBeingMeasured == result.CurrencyBeingMeasured && c.CurrencyMatched == result.CurrencyMatched);

                    if (currency == null)
                    {
                        currency = new Currency
                        {
                            CurrencyBeingMeasured = result.CurrencyBeingMeasured,
                            CurrencyMatched = result.CurrencyMatched
                        };

                        var currencyEntity = await _dbContext.Currencies.AddAsync(currency);

                        currency = currencyEntity.Entity;
                    }

                    currencyValue = new CurrencyValue
                    {
                        Value = result.CurrencyValue,
                        DailyDataOfCurrency = result.DailyDataOfCurrency,
                        Currency = currency
                    };

                    await _dbContext.CurrencyValues.AddAsync(currencyValue);

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        /// <summary>
        /// Get caches data from database
        /// </summary>
        /// <param name="model"><see cref="CurrencyModel"/></param>
        /// <returns><see cref="IEnumerable{CurrencyValue}"/></returns>
        public IEnumerable<CurrencyValue> GetAsync(CurrencyModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CurrencyBeingMeasured) ||
                string.IsNullOrWhiteSpace(model.CurrencyMatched))
            {
                return new List<CurrencyValue>();
            }

            var endDate = _holidayChecker.ReturnDateBeforeDayOff(model.EndDate ?? model.StartDate);

            var results = _dbContext.CurrencyValues.Include(c => c.Currency)
                .Where(c =>
                    (c.DailyDataOfCurrency >= model.StartDate && c.DailyDataOfCurrency <= model.EndDate) &&
                    c.Currency.CurrencyBeingMeasured == model.CurrencyBeingMeasured &&
                    c.Currency.CurrencyMatched == model.CurrencyMatched);

            if (!results.Any() || results.Max(r => r.DailyDataOfCurrency) != endDate)
            {
                return new List<CurrencyValue>();
            }

            return results;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Models.Requests;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Services.Interfaces;

namespace CurrencyFetcher.Core.Services.Implementations
{
    /// <summary>
    /// Currency service (Facade) to split all services in one place
    /// </summary>
    public class CurrencyService: ICurrencyService
    {
        /// <summary>
        /// <see cref="ICurrencyGetterService"/>
        /// </summary>
        private readonly ICurrencyGetterService _currencyGetterService;

        /// <summary>
        /// <see cref="IXmlReader"/>
        /// </summary>
        private readonly IXmlReader _xmlReader;

        /// <summary>
        /// <see cref="IDateService"/>
        /// </summary>
        private readonly IDateService _dateChecker;

        /// <summary>
        /// <see cref="ICacheDatabase"/>
        /// </summary>
        private readonly ICacheDatabase _cacheDatabase;

        /// <summary>
        /// Creates instance of class
        /// </summary>
        /// <param name="currencyGetterService"><see cref="ICurrencyGetterService"/></param>
        /// <param name="xmlReader"><see cref="IXmlReader"/></param>
        /// <param name="dateChecker"><see cref="IDateService"/></param>
        /// <param name="cacheDatabase"><see cref="ICacheDatabase"/></param>
        public CurrencyService(ICurrencyGetterService currencyGetterService, IXmlReader xmlReader, IDateService dateChecker, 
            ICacheDatabase cacheDatabase)
        {
            _currencyGetterService = currencyGetterService;
            _xmlReader = xmlReader;
            _dateChecker = dateChecker;
            _cacheDatabase = cacheDatabase;
        }

        /// <summary>
        /// Get currency information
        /// </summary>
        /// <param name="currencyCollectionModel"><see cref="CurrencyCollectionRequest"/></param>
        /// <returns>returns collection of <see cref="CurrencyResultResponse"/> items</returns>
        public async Task<IEnumerable<CurrencyResultResponse>> GetCurrencyResultsAsync(CurrencyCollectionRequest currencyCollectionModel)
        {
            var currencyResult = new List<CurrencyResultResponse>();
            var currencyModel = new CurrencyModel();

            _dateChecker.ValidateDate(currencyCollectionModel.StartDate, currencyCollectionModel.EndDate);
            (DateTime StartDate, DateTime EndDate) dateItems = _dateChecker.SetCorrectDate(currencyCollectionModel.StartDate, currencyCollectionModel.EndDate);

            currencyModel.StartDate = dateItems.StartDate;
            currencyModel.EndDate = dateItems.EndDate;

            foreach (KeyValuePair<string, string> currencyCode in currencyCollectionModel.CurrencyCodes)
            {
                currencyModel.CurrencyBeingMeasured = currencyCode.Key;
                currencyModel.CurrencyMatched = currencyCode.Value;

                List<CurrencyResultResponse> currencyResults = _cacheDatabase.GetAsync(currencyModel).Select(c => new CurrencyResultResponse
                {
                    CurrencyBeingMeasured = c.Currency.CurrencyBeingMeasured,
                    CurrencyMatched = c.Currency.CurrencyMatched,
                    CurrencyValue = c.Value,
                    DailyDataOfCurrency = c.DailyDataOfCurrency
                }).ToList();

                if (currencyResults.Count == 0)
                {
                    var xmlBody = await _currencyGetterService.FetchDataAsync(currencyModel);
                    currencyResults = _xmlReader.GetCurrencyResults(currencyModel, xmlBody).ToList();

                    foreach (CurrencyResultResponse c in currencyResults)
                    {
                        await _cacheDatabase.SaveAsync(c);
                    }
                }

                currencyResult.AddRange(currencyResults);
            }

            return currencyResult;
        }
    }
}
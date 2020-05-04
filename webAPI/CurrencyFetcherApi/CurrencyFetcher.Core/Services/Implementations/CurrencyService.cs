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
    public class CurrencyService: ICurrencyService
    {
        private readonly ICurrencyGetterService _currencyGetterService;
        private readonly IXmlReader _xmlReader;
        private readonly IDateChecker _dateChecker;
        private readonly ICacheDatabase _cacheDatabase;

        public CurrencyService(ICurrencyGetterService currencyGetterService, IXmlReader xmlReader, IDateChecker dateChecker, ICacheDatabase cacheDatabase)
        {
            _currencyGetterService = currencyGetterService;
            _xmlReader = xmlReader;
            _dateChecker = dateChecker;
            _cacheDatabase = cacheDatabase;
        }

        public async Task<IEnumerable<CurrencyResult>> GetCurrencyResults(CurrencyCollectionModel collectionModel)
        {
            var currencyModels = new List<CurrencyResult>();
            var currencyModel = new CurrencyModel();

            _dateChecker.ValidateDate(collectionModel.StartDate, collectionModel.EndDate);
            (DateTime StartDate, DateTime EndDate) dateItems = _dateChecker.SetCurrentDate(collectionModel.StartDate, collectionModel.EndDate);

            currencyModel.StartDate = dateItems.StartDate;
            currencyModel.EndDate = dateItems.EndDate;

            foreach (KeyValuePair<string, string> currencyCode in collectionModel.CurrencyCodes)
            {
                currencyModel.CurrencyBeingMeasured = currencyCode.Key;
                currencyModel.CurrencyMatched = currencyCode.Value;

                List<CurrencyResult> currencyResults = _cacheDatabase.GetAsync(currencyModel).Select(c => new CurrencyResult
                {
                    CurrencyBeingMeasured = c.Currency.CurrencyBeingMeasured,
                    CurrencyMatched = c.Currency.CurrencyMatched,
                    CurrencyValue = c.Value,
                    DailyDataOfCurrency = c.DailyDataOfCurrency
                }).ToList();

                if (currencyResults.Count == 0)
                {
                    var xmlBody = await _currencyGetterService.FetchData(currencyModel);
                    currencyResults = _xmlReader.GetCurrencyResults(currencyModel, xmlBody).ToList();

                    foreach (var currencyResult in currencyResults)
                    {
                        await _cacheDatabase.SaveAsync(currencyResult);
                    }
                }

                currencyModels.AddRange(currencyResults);
            }

            return currencyModels;
        }
    }
}

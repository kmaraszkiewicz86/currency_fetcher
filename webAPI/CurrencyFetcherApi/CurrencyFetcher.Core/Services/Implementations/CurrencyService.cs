using System;
using System.Collections.Generic;
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

        public CurrencyService(ICurrencyGetterService currencyGetterService, IXmlReader xmlReader, IDateChecker dateChecker)
        {
            _currencyGetterService = currencyGetterService;
            _xmlReader = xmlReader;
            _dateChecker = dateChecker;
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
                currencyModel.Currency = currencyCode.Key;
                currencyModel.CurrencyToMatch = currencyCode.Value;

                var xmlBody = await _currencyGetterService.FetchData(currencyModel);
                currencyModels.AddRange(_xmlReader.GetCurrencyResults(currencyModel, xmlBody));
            }

            return currencyModels;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyFetcher.Core.Models;
using CurrencyFetcher.Core.Models.Responses;
using CurrencyFetcher.Core.Services.Implementations;
using FluentAssertions;
using NUnit.Framework;

namespace CurrencyFetcher.Core.Tests.Services.Implementations
{
    public class XmlReaderTests
    {
        public const string XmlBody =
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?><message:GenericData xmlns:message=\"http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message\" xmlns:common=\"http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:generic=\"http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic\" xsi:schemaLocation=\"http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message https://sdw-wsrest.ecb.europa.eu:443/vocabulary/sdmx/2_1/SDMXMessage.xsd http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common https://sdw-wsrest.ecb.europa.eu:443/vocabulary/sdmx/2_1/SDMXCommon.xsd http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic https://sdw-wsrest.ecb.europa.eu:443/vocabulary/sdmx/2_1/SDMXDataGeneric.xsd\">\n<message:Header>\n<message:ID>0e402194-43ad-47bf-9e85-bfca85219a76</message:ID>\n<message:Test>false</message:Test>\n<message:Prepared>2020-05-05T12:29:47.140+02:00</message:Prepared>\n<message:Sender id=\"ECB\"/>\n<message:Structure structureID=\"ECB_EXR1\" dimensionAtObservation=\"TIME_PERIOD\">\n<common:Structure>\n<URN>urn:sdmx:org.sdmx.infomodel.datastructure.DataStructure=ECB:ECB_EXR1(1.0)</URN>\n</common:Structure>\n</message:Structure>\n</message:Header>\n<message:DataSet action=\"Replace\" validFromDate=\"2020-05-05T12:29:47.140+02:00\" structureRef=\"ECB_EXR1\">\n<generic:Series>\n<generic:SeriesKey>\n<generic:Value id=\"FREQ\" value=\"D\"/>\n<generic:Value id=\"CURRENCY\" value=\"PLN\"/>\n<generic:Value id=\"CURRENCY_DENOM\" value=\"EUR\"/>\n<generic:Value id=\"EXR_TYPE\" value=\"SP00\"/>\n<generic:Value id=\"EXR_SUFFIX\" value=\"A\"/>\n</generic:SeriesKey>\n<generic:Attributes>\n<generic:Value id=\"UNIT_MULT\" value=\"0\"/>\n<generic:Value id=\"TITLE\" value=\"Polish zloty/Euro\"/>\n<generic:Value id=\"DECIMALS\" value=\"4\"/>\n<generic:Value id=\"TIME_FORMAT\" value=\"P1D\"/>\n<generic:Value id=\"UNIT\" value=\"PLN\"/>\n<generic:Value id=\"TITLE_COMPL\" value=\"ECB reference exchange rate, Polish zloty/Euro, 2:15 pm (C.E.T.)\"/>\n<generic:Value id=\"COLLECTION\" value=\"A\"/>\n<generic:Value id=\"SOURCE_AGENCY\" value=\"4F0\"/>\n</generic:Attributes>\n<generic:Obs>\n<generic:ObsDimension value=\"2009-01-01\"/>\n<generic:ObsValue value=\"NaN\"/>\n<generic:Attributes>\n<generic:Value id=\"OBS_STATUS\" value=\"H\"/>\n</generic:Attributes>\n</generic:Obs>\n<generic:Obs>\n<generic:ObsDimension value=\"2009-01-02\"/>\n<generic:ObsValue value=\"4.1638\"/>\n<generic:Attributes>\n<generic:Value id=\"OBS_STATUS\" value=\"A\"/>\n</generic:Attributes>\n</generic:Obs>\n<generic:Obs>\n<generic:ObsDimension value=\"2009-01-05\"/>\n<generic:ObsValue value=\"4.136\"/>\n<generic:Attributes>\n<generic:Value id=\"OBS_STATUS\" value=\"A\"/>\n</generic:Attributes>\n</generic:Obs>\n</generic:Series>\n</message:DataSet>\n</message:GenericData>";

        private XmlReader _xmlReader;

        [SetUp]
        public void SetUp()
        {
            _xmlReader = new XmlReader();
        }

        [Test]
        public void GetCurrencyResults_WithNoEmptyResultFromApi_ReturnListOfItems()
        {
            var expectedResults = new List<CurrencyResultResponse>
            {
                new CurrencyResultResponse
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "EUR",
                    DailyDataOfCurrency = new DateTime(2009,1,2),
                    CurrencyValue = 4.1638m
                },
                new CurrencyResultResponse
                {
                    CurrencyBeingMeasured = "PLN",
                    CurrencyMatched = "EUR",
                    DailyDataOfCurrency = new DateTime(2009,1,5),
                    CurrencyValue = 4.136m
                }
            };

            var currencyResults = _xmlReader.GetCurrencyResults(new CurrencyModel
            {
                CurrencyBeingMeasured = "PLN",
                CurrencyMatched = "EUR",
                StartDate = new DateTime(2009, 1, 1),
                EndDate = new DateTime(2009, 1, 5)
            }, XmlBody).ToList();

            currencyResults.Should().NotBeEmpty();
            currencyResults.Should().BeEquivalentTo(expectedResults);
        }

        [Test]
        public void GetCurrencyResults_WithEmptyResultFromApi_ReturnEmptyList()
        {
            List<CurrencyResultResponse> currencyResults = _xmlReader.GetCurrencyResults(new CurrencyModel
            {
                CurrencyBeingMeasured = "PLN",
                CurrencyMatched = "EUR",
                StartDate = new DateTime(1980, 1, 1),
                EndDate = new DateTime(1980, 1, 5)
            }, string.Empty).ToList();

            currencyResults.Should().BeEmpty();
        }

        [TearDown]
        public void TearDown()
        {
            _xmlReader = null;
        }
    }
}

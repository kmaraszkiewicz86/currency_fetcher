using System;
using CurrencyFetcher.Core.Services.Implementations;
using FluentAssertions;
using NUnit.Framework;

namespace CurrencyFetcher.Core.Tests.Services.Implementations
{
    public class HolidayCheckerTests
    {
        private HolidayChecker _holidayChecker;

        [SetUp]
        public void SetUp()
        {
            _holidayChecker = new HolidayChecker();
        }

        [Test]
        public void ReturnDateBeforeDayOff_HolidayDate_ReturnChangedDate()
        {
            DateTime date = _holidayChecker.ReturnDateBeforeDayOff(new DateTime(2020, 5, 3));

            date.Should().Be(new DateTime(2020, 4, 30));
        }

        [Test]
        public void ReturnDateBeforeDayOff_NoHolidayDate_ReturnNotChangedDate()
        {
            var expectedDate = new DateTime(2020, 5, 6);

            DateTime date = _holidayChecker.ReturnDateBeforeDayOff(expectedDate);

            date.Should().Be(expectedDate);
        }
    }
}
using System;
using CurrencyFetcher.Core.Exceptions;
using CurrencyFetcher.Core.Services.Implementations;
using CurrencyFetcher.Core.Services.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CurrencyFetcher.Core.Tests.Services.Implementations
{
    public class DateCheckerTests
    {
        private Mock<IHolidayChecker> _holidayChecker;
        private DateService _dateChecker;

        [SetUp]
        public void SetUp()
        {
            _holidayChecker = new Mock<IHolidayChecker>();
            _dateChecker = new DateService(_holidayChecker.Object);
        }

        [Test]
        public void SetCurrentDate_IsHolidayDate_ReturnsChangedDate()
        {
            var expectedDate = new DateTime(2020, 4, 30);

            _holidayChecker.Setup(h => h.ReturnDateBeforeDayOff(It.IsAny<DateTime>()))
                .Returns(expectedDate);

            (DateTime StartDate, DateTime EndDate) dates =
                _dateChecker.SetCorrectDate(new DateTime(2020, 5, 3), new DateTime(2020, 5, 3));

            dates.StartDate.Should().Be(expectedDate);
            dates.EndDate.Should().Be(new DateTime(2020, 5, 3));
        }

        [Test]
        public void SetCurrentDate_IsHolidayDateWhenEndDateIsNull_ReturnsChangedDate()
        {
            var expectedDate = new DateTime(2020, 4, 30);

            _holidayChecker.Setup(h => h.ReturnDateBeforeDayOff(It.IsAny<DateTime>()))
                .Returns(expectedDate);

            (DateTime StartDate, DateTime EndDate) dates =
                _dateChecker.SetCorrectDate(new DateTime(2020, 5, 3), null);

            dates.StartDate.Should().Be(expectedDate);
            dates.EndDate.Should().Be(expectedDate);
        }

        [Test]
        public void ValidateDate_StartDateIsFromFuture_ThrowsNotFoundException()
        {
            CheckValidationOfValidateDateMethod<NotFoundException>(DateTime.UtcNow.AddMonths(1), DateTime.UtcNow.AddMonths(1),
                "The startDate could not be after current date");
        }

        [Test]
        public void ValidateDate_EndDateIsFromFuture_ThrowsNotFoundException()
        {
            CheckValidationOfValidateDateMethod<NotFoundException>(new DateTime(2010, 2, 1), DateTime.UtcNow.AddMonths(1),
                "The startDate could not be after current date");
        }

        [Test]
        public void ValidateDate_EndDateIsBeforeStartDate_ThrowsBadRequestException()
        {
            CheckValidationOfValidateDateMethod<BadRequestException>(new DateTime(2010,2,1), new DateTime(2010, 1, 1),
                "The endDate could not be after current date");
        }

        private void CheckValidationOfValidateDateMethod<TExpectedException>(DateTime startDate, DateTime endDate, string errorMessage)
            where TExpectedException: Exception
        {
            Action action = () => _dateChecker.ValidateDate(startDate, endDate);

            action.Should().Throw<TExpectedException>(errorMessage);
        }
    }
}

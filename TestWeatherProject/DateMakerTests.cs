using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.NetworkInformation;
using WeatherApplication;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using static WeatherApplication.HuntingModel;
using static WeatherApplication.UVModel;
using Newtonsoft.Json;
using Moq;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using static System.Net.Mime.MediaTypeNames;

namespace WeatherApplication.Tests
{
    [TestFixture]
    public class DateMakerTests
    {
        DateMaker dateMaker = new DateMaker();

        [Test]
        public void ValidDateInputNoException()
        {
            DateTime expectedDate = new DateTime(2024, 5, 28);
            Assert.DoesNotThrow(() => dateMaker.MakeAndValidateDate("2024", "05", "28"));
            Assert.That(dateMaker.MakeAndValidateDate("2024", "05", "28"), Is.EqualTo(expectedDate));
        }
        [TestCase("1500", "05", "28", "Check the year.")]
        [TestCase("2024", "05", "day", "Wrong date format.")]
        [TestCase("", "05", "28", "Day, month, year can not be empty.")]
        [TestCase("2024", "", "28", "Day, month, year can not be empty.")]
        [TestCase("2024", "05", "", "Day, month, year can not be empty.")]
        [TestCase("", "", "", "Day, month, year can not be empty.")]

        [Test]
        public void InvalidDateInputException(string year, string month, string day, string message)
        {
            var exception = Assert.Throws<ArgumentException>(() => dateMaker.MakeAndValidateDate(year, month, day));
            Assert.That(exception.Message, Is.EqualTo(message));
        }
    }
}


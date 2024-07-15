using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication;

namespace WeatherApplication.Tests
{
    [TestFixture]
    public class TideDataTests
    {
        double lon = 174.7667;//valid coordinates
        double lat = -36.8667; //valid coordinates

        [Test]
        public void DeserializeTideData_ReturnsTideDataObject()
        {
            // Arrange
            string json = "{\"metadata\":{\"latitude\":-37.045,\"longitude\":174.846,\"datum\":\"LAT\",\"start\":\"2024-04-28T12:00:00.000Z\",\"days\":7,\"interval\":0,\"height\":\"LAT = 2.241m below MSL\"},\"values\":[{\"time\":\"2024-04-28T13:30:00Z\",\"value\":3.6},{\"time\":\"2024-04-28T19:38:00Z\",\"value\":1},{\"time\":\"2024-04-29T01:46:00Z\",\"value\":3.54},{\"time\":\"2024-04-29T08:03:00Z\",\"value\":0.87},{\"time\":\"2024-04-29T14:21:00Z\",\"value\":3.51},{\"time\":\"2024-04-29T20:32:00Z\",\"value\":1.1},{\"time\":\"2024-04-30T02:41:00Z\",\"value\":3.42},{\"time\":\"2024-04-30T09:01:00Z\",\"value\":0.97},{\"time\":\"2024-04-30T15:23:00Z\",\"value\":3.43},{\"time\":\"2024-04-30T21:38:00Z\",\"value\":1.18},{\"time\":\"2024-05-01T03:49:00Z\",\"value\":3.34},{\"time\":\"2024-05-01T10:10:00Z\",\"value\":1.03},{\"time\":\"2024-05-01T16:35:00Z\",\"value\":3.42},{\"time\":\"2024-05-01T22:52:00Z\",\"value\":1.17},{\"time\":\"2024-05-02T05:05:00Z\",\"value\":3.35},{\"time\":\"2024-05-02T11:25:00Z\",\"value\":1},{\"time\":\"2024-05-02T17:49:00Z\",\"value\":3.51},{\"time\":\"2024-05-03T00:07:00Z\",\"value\":1.04},{\"time\":\"2024-05-03T06:20:00Z\",\"value\":3.47},{\"time\":\"2024-05-03T12:36:00Z\",\"value\":0.86},{\"time\":\"2024-05-03T18:57:00Z\",\"value\":3.69},{\"time\":\"2024-05-04T01:14:00Z\",\"value\":0.83},{\"time\":\"2024-05-04T07:26:00Z\",\"value\":3.67},{\"time\":\"2024-05-04T13:40:00Z\",\"value\":0.68},{\"time\":\"2024-05-04T19:57:00Z\",\"value\":3.89},{\"time\":\"2024-05-05T02:12:00Z\",\"value\":0.6},{\"time\":\"2024-05-05T08:24:00Z\",\"value\":3.89}]}";

            // Act
            var tideData = JsonConvert.DeserializeObject<TidesModel.TidesData>(json);

            // Assert
            Assert.IsNotNull(tideData);
            Assert.IsNotNull(tideData.Metadata);
            Assert.IsNotNull(tideData.Values);
            Assert.That(tideData.Metadata.Latitude, Is.EqualTo(-37.045));
            Assert.That(tideData.Metadata.Longitude, Is.EqualTo(174.846));
        }
        [TestCase("2022-02-02", "2021-02-02", "Start date can not be after end date.", "NIWA_Tides_API_Key")]
        [TestCase("2022-02-02", "2022-02-02", "API key cannot be null or empty.", "")]
        [Test]
        public void InvalidInputsException(string date1, string date2, string message, string key)
        {
            DateTime startDate = DateTime.Parse(date1);
            DateTime endDate = DateTime.Parse(date2);
            TidesModel tidesModel = new TidesModel(key);
            var exception = Assert.Throws<ArgumentException>(() => tidesModel.GetTidesData(lat, lon, key, startDate, endDate).GetAwaiter().GetResult());
            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [Test]
        public void DatesInTheFutureException()
        {
            DateTime startDate = DateTime.Today.AddDays(32);
            DateTime endDate = DateTime.Today.AddDays(32);
            TestDateRangeInFuture(startDate, endDate);
        }
        [Test]
        public void EndDateInTheFutureException()
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(32);
            TestDateRangeInFuture(startDate, endDate);
        }
        private void TestDateRangeInFuture(DateTime startDate, DateTime endDate)
        {
            string key = "NIWA_Tides_API_Key";
            string message = "The data is limited by 31 days from today.";
            TidesModel tidesModel = new TidesModel(key);
            var exception = Assert.Throws<ArgumentException>(() =>
                tidesModel.GetTidesData(lat, lon, key, startDate, endDate).GetAwaiter().GetResult());
            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [TestCase("2022-02-02", "2022-02-02", "NIWA_Tides_API_Key")]
        [Test]
        public void ValidInputNoException(string date1, string date2, string key)
        {
            DateTime startDate = DateTime.Parse(date1);
            DateTime endDate = DateTime.Parse(date2);
            TidesModel tidesModel = new TidesModel(key);
            Assert.DoesNotThrow(() => tidesModel.GetTidesData(lat, lon, key, startDate, endDate).GetAwaiter().GetResult());
        }
    }
}

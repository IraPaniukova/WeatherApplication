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
    public class Co2DataTests
    {

        [Test]
        public void DeserialiseCo2Data()
        {
            string json = "{\"queryCost\":24,\"latitude\":-36.8523,\"longitude\":174.764,\"resolvedAddress\":\"Auckland, New Zealand\",\"address\":\"auckland\",\"timezone\":\"Pacific/Auckland\",\"tzoffset\":13.0,\"days\":[{\"datetime\":\"2024-02-02\",\"pm2p5\":7.0,\"co\":59.0,\"aqieur\":1.0,\"hours\":[{\"datetime\":\"00:00:00\",\"pm2p5\":7.0,\"co\":56.0,\"aqieur\":1.0},{\"datetime\":\"01:00:00\",\"pm2p5\":6.0,\"co\":61.0,\"aqieur\":1.0},{\"datetime\":\"02:00:00\",\"pm2p5\":6.0,\"co\":21.0,\"aqieur\":1.0},{\"datetime\":\"03:00:00\",\"pm2p5\":7.0,\"co\":42.0,\"aqieur\":1.0},{\"datetime\":\"04:00:00\",\"pm2p5\":8.0,\"co\":63.0,\"aqieur\":1.0},{\"datetime\":\"05:00:00\",\"pm2p5\":8.0,\"co\":64.0,\"aqieur\":1.0},{\"datetime\":\"06:00:00\",\"pm2p5\":8.0,\"co\":64.0,\"aqieur\":1.0},{\"datetime\":\"07:00:00\",\"pm2p5\":7.0,\"co\":64.0,\"aqieur\":1.0},{\"datetime\":\"08:00:00\",\"pm2p5\":8.0,\"co\":64.0,\"aqieur\":1.0},{\"datetime\":\"09:00:00\",\"pm2p5\":7.0,\"co\":64.0,\"aqieur\":1.0},{\"datetime\":\"10:00:00\",\"pm2p5\":6.0,\"co\":64.0,\"aqieur\":1.0},{\"datetime\":\"11:00:00\",\"pm2p5\":6.0,\"co\":64.0,\"aqieur\":1.0},{\"datetime\":\"12:00:00\",\"pm2p5\":5.0,\"co\":63.0,\"aqieur\":1.0},{\"datetime\":\"13:00:00\",\"pm2p5\":5.0,\"co\":62.0,\"aqieur\":1.0},{\"datetime\":\"14:00:00\",\"pm2p5\":5.0,\"co\":23.0,\"aqieur\":1.0},{\"datetime\":\"15:00:00\",\"pm2p5\":6.0,\"co\":45.0,\"aqieur\":1.0},{\"datetime\":\"16:00:00\",\"pm2p5\":7.0,\"co\":68.0,\"aqieur\":1.0},{\"datetime\":\"17:00:00\",\"pm2p5\":7.0,\"co\":67.0,\"aqieur\":1.0},{\"datetime\":\"18:00:00\",\"pm2p5\":7.0,\"co\":67.0,\"aqieur\":1.0},{\"datetime\":\"19:00:00\",\"pm2p5\":7.0,\"co\":66.0,\"aqieur\":1.0},{\"datetime\":\"20:00:00\",\"pm2p5\":8.0,\"co\":65.0,\"aqieur\":1.0},{\"datetime\":\"21:00:00\",\"pm2p5\":7.0,\"co\":65.0,\"aqieur\":1.0},{\"datetime\":\"22:00:00\",\"pm2p5\":7.0,\"co\":64.0,\"aqieur\":1.0},{\"datetime\":\"23:00:00\",\"pm2p5\":8.0,\"co\":63.0,\"aqieur\":1.0}]}]}";
            AirPollution? actualAirPollution = JsonConvert.DeserializeObject<AirPollution>(json);

            // Expected data
            AirPollution expectedAirPollution = new AirPollution
            {
                queryCost = 24,
                latitude = -36.8523,
                longitude = 174.764,
                resolvedAddress = "Auckland, New Zealand",
                address = "Auckland",
                timezone = "Pacific/Auckland",
                tzoffset = 13.0,
                days = new List<DayData>
                {
                    new DayData
                    {
                        datetime = "2024-02-02",
                        pm2p5 = 7.0,
                        co = 59.0,
                        aqieur = 1.0,
                        hours = new List<HourData>
                        {
                            new HourData { datetime = "00:00:00", pm2p5 = 7.0, co = 56.0, aqieur = 1.0 },
                            new HourData { datetime = "01:00:00", pm2p5 = 6.0, co = 61.0, aqieur = 1.0 }
                        }
                    }
                }
            };
            for (int i = 0; i < actualAirPollution?.days?.Count && i < expectedAirPollution?.days?.Count; i++)
            {
                Assert.That(actualAirPollution?.days[i].datetime, Is.EqualTo(expectedAirPollution?.days[i].datetime));
                Assert.That(actualAirPollution?.days[i].pm2p5, Is.EqualTo(expectedAirPollution?.days[i].pm2p5));
                Assert.That(actualAirPollution?.days[i].co, Is.EqualTo(expectedAirPollution?.days[i].co));
                Assert.That(actualAirPollution?.days[i].aqieur, Is.EqualTo(expectedAirPollution?.days[i].aqieur));
                for (int j = 0; j < actualAirPollution?.days[i].hours?.Count && j < expectedAirPollution?.days[i].hours?.Count; j++)
                {
                    Assert.That(actualAirPollution?.days[i].hours?[j].datetime, Is.EqualTo(expectedAirPollution?.days[i].hours?[j].datetime));
                    Assert.That(actualAirPollution?.days[i].hours?[j].pm2p5, Is.EqualTo(expectedAirPollution?.days[i].hours?[j].pm2p5));
                    Assert.That(actualAirPollution?.days[i].hours?[j].co, Is.EqualTo(expectedAirPollution?.days[i].hours?[j].co));
                    Assert.That(actualAirPollution?.days[i].hours?[j].aqieur, Is.EqualTo(expectedAirPollution?.days[i].hours?[j].aqieur));
                }
            }
        }


        [Test]
        public void InvalidCityNameShouldThrowArgumentException()
        {
            Co2Model co2Model = new Co2Model("CO2_API_Key");
            string invalidCityName = "";
            string message = "City name cannot be null or empty.";

            var exception = Assert.Throws<ArgumentException>(() => co2Model.GetCo2Async(invalidCityName, DateTime.Now).GetAwaiter().GetResult());
            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [Test]
        public void FutureDateShouldThrowArgumentException()
        {
            Co2Model co2Model = new Co2Model("CO2_API_Key");
            DateTime futureDate = DateTime.Now.AddDays(1);
            string message = "Date can not be in the future.";
            var exception = Assert.Throws<ArgumentException>(() => co2Model.GetCo2Async("Auckland", futureDate).GetAwaiter().GetResult());
            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [Test]
        public void PastDateShouldThrowArgumentException()
        {
            Co2Model co2Model = new Co2Model("CO2_API_Key");
            DateTime pastDate = DateTime.Now.AddMonths(-6).AddDays(-1);
            string message = "Date can not be more then half year in the past.";
            var exception = Assert.Throws<ArgumentException>(() => co2Model.GetCo2Async("Auckland", pastDate).GetAwaiter().GetResult());
            Assert.That(exception.Message, Is.EqualTo(message));
        }

    }

}

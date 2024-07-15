using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication;

namespace WeatherApplication.Tests
{
    [TestFixture]
    public class ApiInfoTests
    {
        [TestCase(ApiInfo.OpenWeatherMapApi, "Weather")]
        [TestCase(ApiInfo.NIWA_UV_Api, "NIWA UV")]
        [TestCase(ApiInfo.NIWA_Tides_Api, "NIWA Tides")]
        [TestCase(ApiInfo.NASA_Api, "NASA")]
        [TestCase(ApiInfo.CO2_Api, "CO2")]
        public void GetNameTest(string apiString, string expectedName)
        {
            // Act
            string actualName = ApiInfo.GetName(apiString);

            // Assert
            Assert.That(expectedName, Is.EqualTo(actualName));
        }

        [TestCase(ApiInfo.OpenWeatherMapApi, "https://api.openweathermap.org/data/2.5/weather")]
        [TestCase(ApiInfo.NIWA_UV_Api, "https://api.niwa.co.nz/uv/data")]
        [TestCase(ApiInfo.NIWA_Tides_Api, "https://api.niwa.co.nz/tides/data")]
        [TestCase(ApiInfo.NASA_Api, "https://api.nasa.gov/planetary/earth/imagery")]
        [TestCase(ApiInfo.CO2_Api, "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline")]
        public void GetLinkTest(string apiString, string expectedLink)
        {
            // Act
            string actualLink = ApiInfo.GetLink(apiString);

            // Assert
            Assert.That(expectedLink, Is.EqualTo(actualLink));
        }
    }

}

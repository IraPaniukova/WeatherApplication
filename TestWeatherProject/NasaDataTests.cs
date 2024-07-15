using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication;

namespace WeatherApplication.Tests
{
    [TestFixture]
    public class NasaDataTests
    {
        double lon = 174.7667;//valid coordinates
        double lat = -36.8667; //valid coordinates
        [Test]
        public void InvalidInputException()
        {
            NasaDataModel nasaModel = new NasaDataModel("NASA_API_Key");
            DateTime date = new DateTime(2024, 5, 15); //there is no images for this date
            var exception = Assert.Throws<CustomException>(() => nasaModel.GetImageAsync(lon, lat, date).GetAwaiter().GetResult());
            Assert.That(exception.Message, Is.EqualTo("No imagery for specified date."));
        }

    }

}

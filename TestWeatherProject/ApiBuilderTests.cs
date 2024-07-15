using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication;

namespace TestWeatherProject
{
    [TestFixture]
    internal class ApiBuilderTests
    {
        [Test]
        public void ValidCodesCorrectBuilderTest()
        {
            // Arrange
            Dictionary<string, Type> codeToBuilderType = new Dictionary<string, Type>
            {
                { "1", typeof(WeatherApiBuilder) },
                { "2", typeof(UVApiBuilder) },
                { "3", typeof(TidesApiBuilder) },
                { "4", typeof(NasaApiBuilder) },
                { "5", typeof(Co2ApiBuilder) },
                { "6", typeof(HuntingApiBuilder) }
            };

            foreach (var kvp in codeToBuilderType)
            {
                IAPIBuilder builder = UIApiBuilderFactory.getApi(kvp.Key);
                Assert.IsInstanceOf(kvp.Value, builder);
            }
        }
        [Test]
        public void InvalidCodeExceptionTest()
        {
            string invalidCode = "b99";
            var ex = Assert.Throws<Exception>(() => UIApiBuilderFactory.getApi(invalidCode));
            Assert.That(ex.Message, Is.EqualTo("Invalid number"));
        }
    }
}

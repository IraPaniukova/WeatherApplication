using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication;

namespace TestWeatherProject
{
    [TestFixture]
    public class UVDataTests
    {// Arrange
        string json = @"
        {
          ""products"": [
            {
              ""values"": [
                {
                  ""time"": ""2024-04-22T18:00:00.000Z"",
                  ""value"": 0
                },
                {
                  ""time"": ""2024-04-22T19:00:00.000Z"",
                  ""value"": 0
                },
                {
                  ""time"": ""2024-04-22T20:00:00.000Z"",
                  ""value"": 0.261
                }
              ],
              ""name"": ""cloudy_sky_uv_index""
            },
            {
              ""values"": [
                {
                  ""time"": ""2024-04-22T18:00:00.000Z"",
                  ""value"": 0
                },
                {
                  ""time"": ""2024-04-22T19:00:00.000Z"",
                  ""value"": 0
                },
                {
                  ""time"": ""2024-04-22T20:00:00.000Z"",
                  ""value"": 0.382
                }
              ],
              ""name"": ""clear_sky_uv_index""
            }
          ],
          ""coord"": ""EPSG:4326,-36.993,174.88""
        }";


        [Test]
        public void DeserializeUVData_ReturnsUVDataObject()
        {
            // Act
            var uvDataDeserializer = new UVModel.UVDataDeserializer();
            UVModel.UVData uvData = uvDataDeserializer.Deserialize(json);

            // Assert
            Assert.That(uvData, Is.Not.Null);
            Assert.That(uvData.Products, Has.Count.EqualTo(2));

            // Assert product 1
            Assert.That(uvData.Products[0].Name, Is.EqualTo("cloudy_sky_uv_index"));
            Assert.That(uvData.Products[0].Values, Has.Count.EqualTo(3));
            AssertValuesInRange(uvData.Products[0].Values);

            // Assert product 2
            Assert.That(uvData.Products[1].Name, Is.EqualTo("clear_sky_uv_index"));
            Assert.That(uvData.Products[1].Values, Has.Count.EqualTo(3));
            AssertValuesInRange(uvData.Products[1].Values);
        }

        [Test]
        public void DataIsFilteredTest()
        {
            // Act
            var uvDataDeserializer = new UVModel.UVDataDeserializer();
            UVModel.UVData uvData = uvDataDeserializer.Deserialize(json);
            UVModel model = new UVModel("");
            var actualUVData = model.FilterUVDataByHour(uvData, new DateTime(2024, 04, 22, 20, 00, 00));

            for (int i = 0; i > uvData?.Products?.Count; i++)
            {
                for (int j = 0; j < uvData?.Products[i].Values?.Count;)
                {
                    Assert.That(uvData?.Products[i].Values, Has.Count.EqualTo(1));
                    AssertValuesInRange(uvData?.Products[i].Values);

                }
            }
        }

        private void AssertValuesInRange(List<UVModel.UVDataEntry> values)
        {
            AssertValuesInRange(values, v => v.Value, v => v.Time);
        }

        private void AssertValuesInRange(List<UVModel.UVDataEntry> values, Func<UVModel.UVDataEntry, double> valueAccessor, Func<UVModel.UVDataEntry, DateTime> timeAccessor)
        {
            foreach (var value in values)
            {
                Assert.That(valueAccessor(value), Is.GreaterThanOrEqualTo(0));
                Assert.That(valueAccessor(value), Is.LessThanOrEqualTo(double.MaxValue));
            }
        }
    }
}

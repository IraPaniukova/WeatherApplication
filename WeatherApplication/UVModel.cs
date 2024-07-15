using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static WeatherApplication.UVModel;

namespace WeatherApplication
{
    public class UVModel
    {
        private readonly HttpClientWrapper m_httpClient = HttpClientWrapper.Instance;
        private string m_apiKey;
        public UVModel(string apiKey)
        {
            m_apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));

        }

        public class UVData
        {
            [Required]
            public List<UVProduct>? Products { get; set; }

            [Required]
            public string? Coord { get; set; }
        }
        
        public class UVProduct
        {
            [Required]
            public string? Name { get; set; }

            [Required]
            public List<UVDataEntry>? Values { get; set; }
        }

        public class UVDataEntry
        {
            [Required]
            public DateTime Time { get; set; }

            [Required]
            [Range(0, double.MaxValue)]
            public double Value { get; set; }
        }

        public class UVDataDeserializer
        {
            public UVData Deserialize(string json)
            {
                var uvData = new UVData { Products = new List<UVProduct>() };

                var jsonObject = JObject.Parse(json);
                if (jsonObject == null)
                {
                    throw new ArgumentNullException(nameof(json), "JSON object is null.");
                }

                // Check for the "coord" property
                var coordToken = jsonObject["coord"];
                if (coordToken == null)
                {
                    throw new JsonSerializationException("Missing 'coord' property in JSON object.");
                }
                uvData.Coord = coordToken.Value<string>();

                // Check for the "products" array
                var productsToken = jsonObject["products"];
                if (productsToken == null || !productsToken.HasValues)
                {
                    throw new JsonSerializationException("Missing 'products' array in JSON object.");
                }

                foreach (var productJson in productsToken)
                {
                    // Check for the "name" property
                    var nameToken = productJson["name"];
                    if (nameToken == null)
                    {
                        throw new JsonSerializationException("Missing 'name' property in product JSON object.");
                    }

                    var uvProduct = new UVProduct
                    {
                        Name = nameToken.Value<string>(),
                        Values = new List<UVDataEntry>()
                    };

                    // Check for the "values" array
                    var valuesToken = productJson["values"];
                    if (valuesToken == null || !valuesToken.HasValues)
                    {
                        throw new JsonSerializationException("Missing 'values' array in product JSON object.");
                    }

                    foreach (var valueJson in valuesToken)
                    {
                        // Check for the "time" property
                        var timeToken = valueJson["time"];
                        if (timeToken == null)
                        {
                            throw new JsonSerializationException("Missing 'time' property in value JSON object.");
                        }

                        // Check for the "value" property
                        var valueToken = valueJson["value"];
                        if (valueToken == null)
                        {
                            throw new JsonSerializationException("Missing 'value' property in value JSON object.");
                        }

                        var uvDataEntry = new UVDataEntry
                        {
                            Time = DateTime.Parse(timeToken.ToString()),
                            Value = double.Parse(valueToken.ToString())
                        };
                        uvProduct.Values.Add(uvDataEntry);
                    }
                    uvData.Products.Add(uvProduct);
                }
                return uvData;
            }
        }
        public UVData FilteredUvData(UVData uvData)
        {
            if (uvData == null) { new ArgumentException("There is no data."); }
            if (uvData?.Products == null) { new ArgumentException("There is no data for this coordinates."); }
            DateTime currentHour = DateTime.UtcNow;

            return FilterUVDataByHour(uvData ?? throw new Exception("No UV Data"), currentHour);
          
        }
        public UVData FilterUVDataByHour(UVData uvData, DateTime currentHour)
        {
              foreach (var product in uvData?.Products ?? throw new Exception("No Data"))
            {
                if (product.Values != null)
                {
                    product.Values = product.Values
                        .Where(entry => entry.Time.Year == currentHour.Year &&
                                    entry.Time.Month == currentHour.Month &&
                                    entry.Time.Day == currentHour.Day &&
                                    entry.Time.Hour == currentHour.Hour)
                        .ToList();
                }
            }
            return uvData;
        }

        internal async Task<UVData> GetUVDataAsync(double lon, double lat)
        {
            if (lon == 0 || lat == 0)
            {
                throw new ArgumentException("The coordinates do not exist.");
            }
            try {
                ApiDataFetcherFactory json = new ApiDataFetcherFactory(m_apiKey);
                UVDataDeserializer uVDataDeserializer = new UVDataDeserializer();
                var allData = uVDataDeserializer.Deserialize(await json.FetchUVData(lon, lat)) ?? throw new Exception("Weather data deserialization failed.");
                return FilteredUvData(allData);
            }
            catch (HttpRequestException ex)
            {
                throw new CustomException("HTTP request error.", ex);
            }
            catch (JsonException ex)
            {
                throw new CustomException("Error occurred while parsing JSON response.", ex);
            }
            catch (Exception ex)
            {
                throw new CustomException("An error occurred during the asynchronous operation.", ex);
            }
        }
    }
}

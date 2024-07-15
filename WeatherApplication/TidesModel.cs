using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static WeatherApplication.UVModel;

namespace WeatherApplication
{
    public class TidesModel
    { //it was missing these lines:
        private readonly HttpClientWrapper m_httpClient = HttpClientWrapper.Instance;
        private string m_apiKey;

        public TidesModel(string apiKey) //the constractor added to ensure that API Key
        {
            m_apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));

        }
        public class TidesData
        {
            public Metadata? Metadata { get; set; }
            public List<TideValue>? Values { get; set; }
        }

        public class Metadata
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string? Datum { get; set; }
            public DateTime Start { get; set; }
            public int Days { get; set; }
            public int Interval { get; set; }
            public string? Height { get; set; }
        }

        public class TideValue
        {
            public DateTime Time { get; set; }
            public double Value { get; set; }
        }


        public async Task<TidesData> GetTidesData(double lat, double lon, string apiKey, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(apiKey)) //checking first is the key is ok
                {
                    throw new ArgumentException("API key cannot be null or empty.");
                }             
                if (startDate > endDate) //checking that the date are in correct order
                {
                    throw new ArgumentException("Start date can not be after end date.");
                }
                if (startDate> DateTime.Today.AddDays(31)|| endDate> DateTime.Today.AddDays(31)) //NASA gives such limitation for their data
                {
                    throw new ArgumentException("The data is limited by 31 days from today.");
                }
                int numberOfDays = (int)(endDate - startDate).TotalDays + 1;
                TidesData tidesData = new TidesData
                {
                    Metadata = new Metadata
                    {
                        Latitude = lat,
                        Longitude = lon,
                        Datum = "MSL", // Assuming default datum is MSL (Mean Sea Level)
                        Start = startDate,
                        Days = numberOfDays, // Total number of days including start and end dates
                        Interval = 0, // Assuming no interval between tide measurements
                        Height = "MSL = 0m" // Assuming default height is at Mean Sea Level
                    },
                };
                ApiDataFetcherFactory json = new ApiDataFetcherFactory(m_apiKey);
                return JsonConvert.DeserializeObject<TidesData>(await json.FetchTidesData(lat, lon, apiKey, startDate, endDate)) ?? throw new Exception("Tides data deserialization failed.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                throw new CustomException("Error occurred while parsing JSON response.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}

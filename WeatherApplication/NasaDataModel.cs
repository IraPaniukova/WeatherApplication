using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class NasaDataModel
    {
        
        //https://api.nasa.gov/planetary/earth/imagery?lon=100.75&lat=1.5&date=2014-02-01&api_key=NASA_API_KEY

        private readonly HttpClientWrapper m_httpClient = HttpClientWrapper.Instance; //Singleton pattern
        private readonly string m_apiKey;

        public NasaDataModel(string apiKey)
        {
            m_apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        // NASA data is excluded from the factory, because it is fetching just an image and has a different pattern to the other APIs
        public async Task<byte[]> GetImageAsync(double lon, double lat, DateTime date)
        {
            if (date > DateTime.Today)
            {
                throw new ArgumentException("Date can not be in the future.");
            }
            string encodedApiKey = Uri.EscapeDataString(m_apiKey);
            string urlBase = ApiInfo.GetLink(ApiInfo.NASA_Api);
            
            try 
            {
                string apiUrl = $"{urlBase}?lon={lon}&lat={lat}&date={date:yyyy-MM-dd}&api_key={encodedApiKey}";

                HttpResponseMessage response = await m_httpClient.GetAsync(apiUrl);
                if (response.Content.Headers.ContentType?.MediaType == "text/plain" ||
                    response.Content.Headers.ContentType?.MediaType == "application/json")
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(jsonResponse);
                    string? message = (string?)jsonObject["msg"];
                    //Console.WriteLine(message);
                    throw new CustomException(message ?? "No message");
                }
                    response.EnsureSuccessStatusCode();
                byte[] imageData = await response.Content.ReadAsByteArrayAsync();
                return imageData; 
            }
            catch (Exception ex)
            {
                throw new CustomException($"{ex.Message}", ex);
            }
        }
    }
}

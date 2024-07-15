using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static WeatherApplication.UVModel;

namespace WeatherApplication
{
    public class ApiDataFetcherFactory
    //The factory class is responsible for creating instances of other classes. The reasons to use it:
    //Encapsulation: The creation logic is centralized, so if it changes, you only need to update it in one place.
    //Abstraction: The client code does not need to know the specifics of object creation.
    //Maintenance: It simplifies the code by reducing the number of places where object creation logic is implemented.

    {
        private readonly HttpClientWrapper _httpClient;
        private readonly string _apiKey;

        public ApiDataFetcherFactory(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = HttpClientWrapper.Instance;
        }
        protected async Task<string> FetchDataAsync(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        internal async Task<String> FetchWeatherData(string cityName)
        {
            string encodedCityName = Uri.EscapeDataString(cityName);
            string encodedApiKey = Uri.EscapeDataString(_apiKey);
            string urlBase = ApiInfo.GetLink(ApiInfo.OpenWeatherMapApi);
            string url = $"{urlBase}?q={encodedCityName}&appid={encodedApiKey}&units=metric";
            return await FetchDataAsync(url);
        }

        internal async Task<String> FetchUVData(double lon, double lat)
        {
            string urlBase = ApiInfo.GetLink(ApiInfo.NIWA_UV_Api);
            string url = $"{urlBase}?lat={lat}&long={lon}&apikey={_apiKey}";
            return await FetchDataAsync(url);
        }
        internal async Task<String> FetchTidesData(double lat, double lon, string apiKey, DateTime startDate, DateTime endDate)
        {
            string startDateString = startDate.ToString("yyy-MM-dd");
            int numberOfDays = (int)(endDate - startDate).TotalDays + 1;
            string urlBase = ApiInfo.GetLink(ApiInfo.NIWA_Tides_Api);
            string url = $"{urlBase}?lat={lat}&long={lon}&datum=MSL&numberOfDays={numberOfDays}&apikey={apiKey}&startDate={startDateString}";
            return await FetchDataAsync(url);
        }

        internal async Task<String> FetchCo2Data(string cityName, DateTime date)
        {
            string encodedCityName = Uri.EscapeDataString(cityName);
            string encodedApiKey = Uri.EscapeDataString(_apiKey);
            string encodedDate = date.ToString("yyyy-MM-dd");
            string urlBase = ApiInfo.GetLink(ApiInfo.CO2_Api);
            string url = $"{urlBase}/{encodedCityName}/{encodedDate}?unitGroup=metric&key={encodedApiKey}&contentType=json&elements=datetime,pm2p5,co,aqieur";
            return await FetchDataAsync(url);
        }
    }
}

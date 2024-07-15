using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class WeatherDataService
    {
        private WeatherController? controller;
        private WeatherModel InitializeModel()
        {
            string weatherAPIKey = new ApiKeyManager(ApiInfo.GetName(ApiInfo.OpenWeatherMapApi)).ApiKey ?? throw new Exception("No key");
            return new WeatherModel(weatherAPIKey);
        }
        public string UserInput()
        {
            Console.Write("Enter the name of a city: ");
            string cityName = Console.ReadLine() ?? throw new Exception("Can not be empty city");
            return cityName;
        }
        public async Task BuildApiAsync()
        {
            WeatherModel weatherService = InitializeModel();
            WeatherView view = new WeatherView();
            controller = new WeatherController(weatherService, view);
            string cityName = UserInput();
            await controller.RefreshWeatherData(cityName);
            controller.RefreshPanelView();
        }
        public async Task<(double lon, double lat)> GetCityCoordinates()
        {
            string cityName = UserInput();
            if (string.IsNullOrWhiteSpace(cityName))
            {
                throw new ArgumentException("City name cannot be null or empty.");
            }
            WeatherData? weatherData = await InitializeModel().GetWeatherAsync(cityName);
            double lon = 0;
            double lat = 0;

            if (weatherData != null)
            {
                lon = weatherData.Coord?.Lon ?? 0;
                lat = weatherData.Coord?.Lat ?? 0;
            }
            return (lon, lat);
        }
    }
}

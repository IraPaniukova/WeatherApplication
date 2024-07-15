using System;
using System.Threading.Tasks;
using WeatherApplication;

namespace WeatherApplication
{
    public class WeatherController
    {
        private readonly WeatherView m_weatherView;
        private readonly WeatherModel m_weatherModel;
        private WeatherData? m_weatherData;
        public WeatherController(WeatherModel model, WeatherView view)
        {
            m_weatherModel = model ?? throw new ArgumentNullException(nameof(model));
            m_weatherView = view ?? throw new ArgumentNullException(nameof(view));
        }
        public async Task RefreshWeatherData(string city)
        {
            try
            {
                // Call GetWeatherAsync method to retrieve weather data
                m_weatherData = await m_weatherModel.GetWeatherAsync(city);
            }

            catch (ArgumentException ex)
            {
                Console.WriteLine($"{ex.Message}");
                ErrorLogger.Instance.LogError($" {ex.Message}");
            }
            catch (CustomException ex)
            {
                Console.WriteLine($"{ex.Message}");
                ErrorLogger.Instance.LogError($"An error occurred while retrieving weather data: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                ErrorLogger.Instance.LogError($"An unexpected error occurred: {ex.Message}");
            }
        }
        public void RefreshPanelView()
        {
            // Render the weather data only if it is available.
            if (null != m_weatherData)
            {
                m_weatherView.Render(m_weatherData);
            }
        }

        public WeatherData? WeatherData => m_weatherData;
    }
}

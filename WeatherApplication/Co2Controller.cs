using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class Co2Controller
    {
        private readonly Co2View co2View;
        private readonly Co2Model co2Model;
        private AirPollution? airPolutionData;
        public Co2Controller(Co2Model model, Co2View view)
        {
            co2Model = model ?? throw new ArgumentNullException(nameof(model));
            co2View = view ?? throw new ArgumentNullException(nameof(view));
        }
        public async Task RefreshCo2Data(string city, DateTime date)
        {

            try
            {
                // Call GetWeatherAsync method to retrieve weather data
                airPolutionData = await co2Model.GetCo2Async(city, date);         
            }
            catch (CustomException ex)
            {
                Console.WriteLine(ex.Message);
                ErrorLogger.Instance.LogError($"An error occurred while retrieving co2 data: {ex.Message}");
            }
        }
        public void RefreshPanelView()
        {
            // Render the weather data only if it is available.
            if (null != airPolutionData)
            {
                co2View.Render(airPolutionData);
            }
        }

        public AirPollution? AirPolutionData => airPolutionData;
    }








}

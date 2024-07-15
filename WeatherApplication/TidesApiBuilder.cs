using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class TidesApiBuilder : IAPIBuilder
    {
        DateMaker dateMaker = new DateMaker();       
        private readonly WeatherDataService weatherDataService;
        public TidesApiBuilder()
        {
            this.weatherDataService = new WeatherDataService();
        }
        public async Task BuildApiAsync()
        {                            
            //retrieving coordinates from WeatherAPI
            double lon = 0; //we can use 0, because there are no cities at such coordinates
            double lat = 0;
            try
            {
                Console.WriteLine("(Only for New Zealand)");
                (lon, lat) = await weatherDataService.GetCityCoordinates();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if ((lat >= -47.65 && lat <= -33.89) && (lon >= 165.87 && lon <= 179.27))
            {
                Console.WriteLine("\n\n*****************************TIDES DATA***************************************\n");
                Console.WriteLine($"Enter period for tides. Any dates from year 1830 and up to 31 days from today ({DateTime.Today.AddDays(31).ToShortDateString()})");
                try
                {
                    DateTime startDate = dateMaker.CreateDate("start date");
                    DateTime endDate = dateMaker.CreateDate("end date");

                    string tidesApiKey = new ApiKeyManager(ApiInfo.GetName(ApiInfo.NIWA_Tides_Api)).ApiKey ?? throw new Exception("No key");

                    TidesModel tidesModel = new TidesModel(tidesApiKey);
                    TidesView tidesView = new TidesView();
                    TidesController tidesController = new TidesController(tidesModel, tidesView);
                    await tidesController.RefreshTidesData(lat, lon, tidesApiKey, startDate, endDate);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else { Console.WriteLine("NIWA has data only for New Zealand."); }
        }
    }
}

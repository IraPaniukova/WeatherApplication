using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class UVApiBuilder : IAPIBuilder
    {
        private readonly WeatherDataService weatherDataService;
        public UVApiBuilder()
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
                Console.WriteLine("\n\n*****************************UV DATA***************************************\n");
                UVView uvView = new UVView();
                string uvApiKey = new ApiKeyManager(ApiInfo.GetName(ApiInfo.NIWA_UV_Api)).ApiKey ?? throw new Exception("No key");
                UVModel uVModel = new UVModel(uvApiKey);
                UVController uVController = new UVController(uVModel, uvView);
                await uVController.RefreshUVData(lon, lat);
            }
            else { Console.WriteLine("NIWA has data only for New Zealand."); }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherApplication
{
    public class NasaApiBuilder : IAPIBuilder
    {
        DateMaker dateMaker = new DateMaker();
        private readonly WeatherDataService weatherDataService;
        public NasaApiBuilder()
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
            if (lon != 0 && lat != 0)
            {
                Console.WriteLine("\n\n*****************************NASA satellite image data string (truncated)*********\n");
                Console.WriteLine("Please, enter a date. NASA API takes time, please be patient, thank you.\n(Try year 2014, NASA doesn't have images for current year or future.)");
                //unfortunately NASA doesn't have images for the current year, but definitely has for 2014.
                try
                {
                    DateTime dateNasa = dateMaker.CreateDate("a");
                    Console.WriteLine("Date: " + dateNasa);
                    string nasaApiKey = new ApiKeyManager(ApiInfo.GetName(ApiInfo.NASA_Api)).ApiKey ?? throw new Exception("No key");
                    NasaDataModel nasaDataModel = new NasaDataModel(nasaApiKey);
                    NasaDataView nasaDataView = new NasaDataView();
                    NasaDataController nasaDataController = new NasaDataController(nasaDataModel, nasaDataView);
                    await nasaDataController.FetchAndDisplayImageAsync(lon, lat, dateNasa);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else { Console.WriteLine("The site is not defined."); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class WeatherApiBuilder : IAPIBuilder

    { 
         private readonly WeatherDataService coordinatesService;

    public WeatherApiBuilder()
    {
        this.coordinatesService = new WeatherDataService(); 
    }

    public async Task BuildApiAsync()
    {
        // Use coordinatesService for building Weather API
        try
        {
            await coordinatesService.BuildApiAsync();
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class Co2ApiBuilder : IAPIBuilder
    {
        DateMaker dateMaker = new DateMaker();
        public async Task BuildApiAsync()
        {
            string co2APIKey = new ApiKeyManager(ApiInfo.GetName(ApiInfo.CO2_Api)).ApiKey ?? throw new Exception("No key");
            Console.Write("Enter city ");
            string co2city = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(co2city))
            {
                Console.WriteLine("\n\n***************************** CO2******************************");
                Console.WriteLine("Check city air pollution:\n");
                Console.WriteLine($"Enter date starting from {DateTime.Today.AddMonths(-6).ToShortDateString()} to today ({DateTime.Today.ToShortDateString()})");

                try
                {
                    DateTime co2date = dateMaker.CreateDate("a");
                    Console.WriteLine("Date: " + co2date);
                    Console.WriteLine("\n\nAir pollution level for " + co2city + " on " + co2date.ToString("yyyy-MM-dd"));
                    Co2Model co2Model = new Co2Model(co2APIKey);
                    Co2View co2View = new Co2View();
                    Co2Controller co2Controller = new Co2Controller(co2Model, co2View);
                    await co2Controller.RefreshCo2Data(co2city, co2date);
                    co2Controller.RefreshPanelView();
                    Console.WriteLine("\n\n\n");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            else { Console.WriteLine("City name can not be null or empty."); }
        }
    }
}

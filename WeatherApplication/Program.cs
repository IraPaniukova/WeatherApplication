using Microsoft.VisualBasic;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WeatherApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("\nHello.\nNice to see you here today");
                bool count = true;
                while (count)
                {
                    Console.WriteLine("\nPlease choose an Api:\nWeather[1] UV[2] Tides[3] NASA[4] Air Polution[5] Hunting Season[6] Exit[7]");
                    string code = (Console.ReadLine()?.Trim()) ?? throw new Exception("Nothing entered"); if (code == "1" || code == "2" || code == "3" || code == "4" || code == "5" || code == "6" || code == "7")
                    {
                        if (code == "7")
                        {
                            count = false;
                            Console.WriteLine("Thank you for using our app.\nSee you next time. *_*");
                            break;
                        }
                        else
                        {
                            IAPIBuilder apiBuilder = UIApiBuilderFactory.getApi(code);
                            await apiBuilder.BuildApiAsync();
                        }
                    }
                    else { Console.WriteLine("Please enter a number between 1 and 7."); }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

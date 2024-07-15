using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class Co2View
    {
        public void Render(AirPollution ap)
        {
            if (null == ap)
            {
                Console.WriteLine("air polution data is null.");
            }
            else
            {
                Console.WriteLine();

                if (ap.days?.Count > 0 && ap.days[0].hours?.Count > 0)
                {
                    Console.WriteLine("Average PM2.5, CO, and Air Quality Index for the day:");
                    Console.WriteLine(ap.days[0]);
                    Console.WriteLine("\nHourly PM2.5, CO, and Air Quality Index for the day:");
                    Console.WriteLine();
                    
                    foreach (var hour in ap.days[0].hours ?? throw new Exception("No data"))
                    {
                        Console.WriteLine(hour);
                    }
                }
            }
        }
    }
}

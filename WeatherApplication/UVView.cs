using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class UVView //The class was created to maintain the MVC architecture and SOLID principles
    {
        public void Render(UVModel.UVData uvData)
        {
            if (null == uvData)
            {
                Console.WriteLine("UV data is null.");
            }
            else
            {               
                Console.WriteLine($"Coordinates: {uvData.Coord}");
                Console.WriteLine($"UV index for current time  {DateTime.Now}");
                foreach (UVModel.UVProduct uVProduct in uvData.Products ?? throw new Exception("No data"))
                {
                    Console.WriteLine($"If conditions are \"{uVProduct?.Name?.Replace("_uv_index", "").Replace("_"," ")}\"");
                    
                    foreach (UVModel.UVDataEntry uVDataEntry in uVProduct?.Values ?? throw new Exception("No data"))
                    {
                        Console.WriteLine($"\tValue is {uVDataEntry.Value}");                                         
                    }
                }
            }
        }
    }
}
    

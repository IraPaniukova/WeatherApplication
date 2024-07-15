using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class UIApiBuilderFactory
    {
        public static IAPIBuilder getApi(string code)
        {
            switch (code)
            {
                case "1":
                    return new WeatherApiBuilder();
                case "2":
                    return new UVApiBuilder();
                case "3":
                    return new TidesApiBuilder();
                case "4":
                    return new NasaApiBuilder();
                case "5":
                    return new Co2ApiBuilder();
                case "6":
                    return new HuntingApiBuilder();
                default:
                    throw new Exception("Invalid number");
            }
        }
    }
}

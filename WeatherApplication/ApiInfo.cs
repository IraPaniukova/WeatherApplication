using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public static class ApiInfo
    {//the pattern inside of the quote marks should contain the name and the link with a coma inbetween)
        public const string OpenWeatherMapApi = "Weather, https://api.openweathermap.org/data/2.5/weather";
        public const string NIWA_UV_Api = "NIWA UV, https://api.niwa.co.nz/uv/data";
        public const string NIWA_Tides_Api = "NIWA Tides, https://api.niwa.co.nz/tides/data";
        public const string NASA_Api = "NASA, https://api.nasa.gov/planetary/earth/imagery";
        public const string CO2_Api = "CO2, https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline";

        public static string GetName(string apiString)
        {
            return apiString.Split(',')[0].Trim();
        }

        public static string GetLink(string apiString)
        {
            return apiString.Split(',')[1].Trim() ?? apiString.Split(',')[1].Trim();
        }
    }
}

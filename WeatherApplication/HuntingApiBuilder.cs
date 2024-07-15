using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class HuntingApiBuilder : IAPIBuilder
    {
        public async Task BuildApiAsync()
        {
            Console.WriteLine("\n\n*****************************HUNTING SEASONS***********************************\n");
            Console.Write("Would you like to see Hunting data for a months or all year? Month name/All:  ");
            string month = Console.ReadLine() ?? "";
            HuntingController huntingController = new HuntingController();
            huntingController.RefreshHuntingSeasonData("hunting_season_data.txt", month.ToLower());
        }
    }
}

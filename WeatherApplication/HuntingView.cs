using System;
using System.Collections.Generic;

namespace WeatherApplication
{
    public class HuntingView
    {
        public void Render(List<HuntingModel.HuntingSeason> seasons)
        {
            foreach (var season in seasons)
            {
                Console.WriteLine($"Species: {season.Species}");
                if (season.HuntingEnds != "")
                {
                    Console.WriteLine($"Hunting Starts: {season.HuntingStarts}");
                    Console.WriteLine($"Hunting Ends: {season.HuntingEnds}");
                }
                else { Console.WriteLine($"Hunting: {season.HuntingStarts}"); }
                Console.WriteLine($"Notes: {season.Notes}");               
                Console.WriteLine();
            }
        }
    }
}

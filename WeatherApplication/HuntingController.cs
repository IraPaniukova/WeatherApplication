using System;
using System.Collections.Generic;
using System.IO;

namespace WeatherApplication
{
    public class HuntingController
    {
        private readonly HuntingView m_huntingView = new HuntingView();

        public void RefreshHuntingSeasonData(string filePath, string month)
        {
            try
            {
                if (string.IsNullOrEmpty(month))
                { Console.WriteLine("The input was empty"); }
                else if (month == "all")
                {
                    m_huntingView.Render(HuntingModel.ParseHuntingSeasonData(filePath));
                }
                else
                {
                    m_huntingView.Render(HuntingModel.FilterHuntingSeasons(HuntingModel.ParseHuntingSeasonData(filePath), month));
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException("File not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while parsing hunting season data.", ex);
            }
        }
    }
}

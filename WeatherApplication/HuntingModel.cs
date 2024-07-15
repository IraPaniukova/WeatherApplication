using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace WeatherApplication
{
    public class HuntingModel
    {
        public class HuntingSeason
        {
            public string Species { get; }
            public string HuntingStarts { get; }
            public string HuntingEnds { get; }
            public string Notes { get; }


            public HuntingSeason(string species, string huntingStarts, string huntingEnds, string notes)
            {
                Species = species ?? throw new ArgumentNullException(nameof(species));
                HuntingStarts = huntingStarts ?? throw new ArgumentNullException(nameof(huntingStarts));
                HuntingEnds = huntingEnds ?? throw new ArgumentNullException(nameof(huntingEnds));
                Notes = notes ?? throw new ArgumentNullException(nameof(notes));

            }
        }

        public static List<HuntingSeason> ParseHuntingSeasonData(string filePath)
        {
            var huntingSeasons = new List<HuntingSeason>();

            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Skip the header line (first line)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                // Split each line 
                string[] parts = line.Split(',');
                string[] monthsAndNote = parts[1].Split(new string[] { " to ", " through ", " and " }, 2, StringSplitOptions.None);
                string Species = parts[0].Trim();
                string HuntingStarts = monthsAndNote[0].Trim();
                string HuntingEnds = monthsAndNote.Length > 1 ? monthsAndNote[1].Trim() : "";
                string Notes = parts.Length > 2 ? parts[2] : ""; // Handle cases where Notes field is empty

                var season = new HuntingSeason(Species, HuntingStarts, HuntingEnds, Notes);
                huntingSeasons.Add(season);

            }
            return huntingSeasons;

        }

        //filter data for current month (this function is related to the particular set up of the txt file)
        public static List<HuntingSeason> FilterHuntingSeasons(List<HuntingSeason> allSeasons, string month)
        {
            List<HuntingSeason> filtereduHuntingSeasons = new List<HuntingSeason>();
            try
            {
                string[] allMonths = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                string monthProperCase = char.ToUpper(month[0]) + month.Substring(1).ToLower();
                int currentMonthN = Array.IndexOf(allMonths, monthProperCase) + 1;  //it gives a number of a month
                if (currentMonthN == 0) { Console.WriteLine("The input was misspelt"); }
                else
                {
                    Console.WriteLine("Hunting data for month " + allMonths[currentMonthN - 1]);
                    foreach (var s in allSeasons)
                    {
                        string startMonth = ExtractMonth(s.HuntingStarts);
                        string endMonth = ExtractMonth(s.HuntingEnds);
                        if (startMonth == "" || endMonth == "") //for AllYear
                        {
                            filtereduHuntingSeasons.Add(s);
                            continue;
                        }
                        int startMonthN = Array.IndexOf(allMonths, startMonth) + 1;
                        int endMonthN = Array.IndexOf(allMonths, endMonth) + 1;
                        if (currentMonthN <= endMonthN && currentMonthN >= startMonthN)
                        {
                            filtereduHuntingSeasons.Add(s);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            return filtereduHuntingSeasons;
        }
        public static string ExtractMonth(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "";
            }
            string[] words = input.Split(' ');
            // the list of valid month names
            string[] validMonths = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;

            // the first word that is a valid month name
            string month = words.FirstOrDefault(word => validMonths.Contains(word, StringComparer.CurrentCultureIgnoreCase)) ?? "";
            return month;
        }
    }
}

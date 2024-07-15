using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WeatherApplication
{
    public class DateMaker //creates and validates dates
    {
        
        public DateTime CreateDate(string dateDefinition)
        {
            Console.Write($"Enter {dateDefinition} year, yyyy: ");
            string year = Console.ReadLine() ?? "";
            Console.Write($"Enter {dateDefinition} month, mm: ");
            string month = Console.ReadLine() ?? "";
            Console.Write($"Enter {dateDefinition} day, dd: ");
            string day = Console.ReadLine() ?? "";
            Console.WriteLine("- - - -");
            return MakeAndValidateDate(year, month, day);  
        }
        public DateTime MakeAndValidateDate(string year, string month, string day)
        {
            string inputDate = year + "-" + month + "-" + day;
            if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month) || string.IsNullOrEmpty(day))
            { throw new ArgumentException("Day, month, year can not be empty."); }
            if (!DateTime.TryParse(inputDate, out _))
            { throw new ArgumentException("Wrong date format."); }

            DateTime date = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            if (date < new DateTime(1830, 01, 01))
            { throw new ArgumentException("Check the year."); }
            return date;
        }
    }
}

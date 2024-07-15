using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    internal class NasaDataView
    {
        public void DisplayImage(byte[] imageData)
        {
            if (imageData == null)
            { 
                Console.WriteLine("NASA data is null."); 
            }
            else
            {
                string imgString = Convert.ToBase64String(imageData);

                // the string for an image is too long, that is why it is truncated in our program
                int maxLength = 50; // Maximum length of the string to display
                string truncatedImgString = imgString.Length <= maxLength ? imgString : imgString.Substring(0, maxLength);
                Console.WriteLine($"Received image data: {truncatedImgString}...(truncated)");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class CustomException :Exception
    {//The custom exception was moved here to maintain SOLID principles
     //the repetitive code was in 3 different classes
        public CustomException() { }
        public CustomException(string message) : base(message) { }
        public CustomException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

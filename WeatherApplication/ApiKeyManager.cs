using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WeatherApplication
{
    public class ApiKeyManager
    {
        private static readonly FileEncoder encoder = FileEncoder.GetInstance();
        public string? ApiKey { get; private set; }
        public string ApiName { get; private set; }
        public ApiKeyManager(string apiName)
        {
            ApiName = apiName ?? throw new ArgumentNullException(nameof(apiName));
            SetApiKey();
        }

        private void SetApiKey()
        {
            if (string.IsNullOrEmpty(encoder.Read(ApiName)))
            {
                Console.Write("Enter API key for " + ApiName + ": ");
                ApiKey = Console.ReadLine() ?? throw new Exception("Can not be empty key");
                encoder.Write(ApiName, ApiKey);
            }
            else
            {
                ApiKey = encoder.Read(ApiName);
            }           
        }
    }
}

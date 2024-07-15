using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class Co2Model
    {
        private readonly HttpClientWrapper m_httpClient = HttpClientWrapper.Instance;
        private readonly string m_apiKey;

        public Co2Model(string apiKey)
        {
            m_apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        public async Task<AirPollution> GetCo2Async(string cityName, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(cityName))
            {
                throw new ArgumentException("City name cannot be null or empty.");
            }

            if (date > DateTime.Today)
            {
                throw new ArgumentException("Date can not be in the future.");
            }
            if (date < DateTime.Today.AddMonths(-6))
            {
                throw new ArgumentException("Date can not be more then half year in the past.");
            }
            try
            {
                ApiDataFetcherFactory json = new ApiDataFetcherFactory(m_apiKey);
                AirPollution? co2data = JsonConvert.DeserializeObject<AirPollution>(await json.FetchCo2Data(cityName, date));
                return co2data ?? throw new Exception("No data");
            }
            catch (HttpRequestException ex)
            { //Error occurred while sending the HTTP request.
                throw new CustomException("City not found or misspelt / HTTP error", ex);
            }
            catch (JsonException ex)
            {
                throw new CustomException("Error occurred while parsing JSON response.", ex);
            }
            catch (Exception ex)
            {
                throw new CustomException("An error occurred during the asynchronous operation.", ex);
            }
        }

    }
    public class HourData
    {
        public string? datetime { get; set; }
        public double pm2p5 { get; set; }
        public double co { get; set; }
        public double aqieur { get; set; }

        public string GetTimeDescription()
        {
            DateTime time = DateTime.ParseExact(datetime ?? "", "HH:mm:ss", null); // Parse the datetime string to DateTime object
            switch (time.Hour)
            {
                case 0:
                    return "12am";
                case 1:
                    return "1am";
                case 2:
                    return "2am";
                case 3:
                    return "3am";
                case 4:
                    return "4am";
                case 5:
                    return "5am";
                case 6:
                    return "6am";
                case 7:
                    return "7am";
                case 8:
                    return "8am";
                case 9:
                    return "9am";
                case 10:
                    return "10am";
                case 11:
                    return "11am";
                case 12:
                    return "12pm";
                case 13:
                    return "1pm";
                case 14:
                    return "2pm";
                case 15:
                    return "3pm";
                case 16:
                    return "4pm";
                case 17:
                    return "5pm";
                case 18:
                    return "6pm";
                case 19:
                    return "7pm";
                case 20:
                    return "8pm";
                case 21:
                    return "9pm";
                case 22:
                    return "10pm";
                case 23:
                    return "11pm";
                default:
                    return "Unknown";
            }
        }


        public string GetAQIDescription()
        {
            int aqi = (int)aqieur;

            switch (aqi)
            {
                case 1:
                    return "Very Good";
                case 2:
                    return "Good";
                case 3:
                    return "Medium";
                case 4:
                    return "Poor";
                case 5:
                    return "Very Poor";
                case 6:
                    return "Extremely Poor";
                default:
                    return "Unknown";
            }
        }

        public override string ToString()
        {
            return $"Time: {GetTimeDescription()}, PM2.5: {pm2p5}, CO: {co}, Air Quality: {GetAQIDescription()}";
        }
    }

    public class DayData
    {
        public string? datetime { get; set; }
        public double pm2p5 { get; set; }
        public double co { get; set; }
        public double aqieur { get; set; }
        public List<HourData>? hours { get; set; }

        public string GetAQIDescription()
        {
            int aqi = (int)aqieur;

            switch (aqi)
            {
                case 1:
                    return "Very Good";
                case 2:
                    return "Good";
                case 3:
                    return "Medium";
                case 4:
                    return "Poor";
                case 5:
                    return "Very Poor";
                case 6:
                    return "Extremely Poor";
                default:
                    return "Unknown";
            }
        }

        public override string ToString()
        {
            return $"Date: {datetime}, PM2.5: {pm2p5}, CO: {co}, Air Quality: {GetAQIDescription()}";
        }

    }

    public class AirPollution
    {
        public double queryCost { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string? resolvedAddress { get; set; }
        public string? address { get; set; }
        public string? timezone { get; set; }
        public double tzoffset { get; set; }
        public List<DayData>? days { get; set; }
    }
}

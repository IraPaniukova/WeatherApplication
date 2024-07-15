using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication;

namespace WeatherApplication.Tests
{
    [TestFixture]
    public class ApiManagerTests
    {
        [Test]
        public void ApiManager_GettingCorrectKey()
        {
            string testApiName = GenerateRandom(10);
            string expectedTestApiKey = GenerateRandom(32);

            FileEncoder encoder = FileEncoder.GetInstance();
            encoder.Write(testApiName, expectedTestApiKey);

            string actualTestAPIKey = new ApiKeyManager(testApiName).ApiKey ?? "No key";

            Assert.That(actualTestAPIKey, Is.EqualTo(expectedTestApiKey));
        }

        private string GenerateRandom(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
        }

        private static readonly Random random = new Random();
    }

}

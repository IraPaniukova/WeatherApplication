using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication;

namespace WeatherApplication.Tests
{
    [TestFixture]
    public class FileEncoderTests
    {
        private string testFilePath = "testApikeys.sys";

        [SetUp]
        public void Setup()
        {
            // Clear the contents of the test file before each test
            File.WriteAllText(testFilePath, string.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            // Remove the test file after each test
            File.Delete(testFilePath);
        }

        [Test]
        public void FileEncoder_AddNameValuePair()
        {
            // Get an instance of FileEncoder for testing
            FileEncoder encoder = FileEncoder.GetTestInstance(testFilePath);
            // Write key-value pairs for testing
            encoder.Write("ApiKey", "a173994356f879bb3e422754bfdde559");
            // Read values by key for testing
            string actualAPIKey = encoder.Read("ApiKey");
            Assert.That(actualAPIKey, Is.EqualTo("a173994356f879bb3e422754bfdde559"));
        }
    }

}

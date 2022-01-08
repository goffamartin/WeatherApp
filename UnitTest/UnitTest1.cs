using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using WeatherApp;
using Newtonsoft.Json;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task APICallTest()
        {
            // Arrange
            string latitude = "50";
            string longitude = "50";
            string units = "metric";
            string ApiKey = "f4cdb9a4d3badec1ff1423c4a5fba527";
            string url = $"https://api.openweathermap.org/data/2.5/onecall?lat={latitude}&lon={longitude}&exclude=minutely&units={units}&appid={ApiKey}";
            // Act
            API_Response response = await API_Caller.Get(url);
            // Assert
            Assert.IsTrue(response.Successuful);
        }
        [TestMethod]
        public async Task DeserializeIntoObject()
        {
            // Arrange
            string latitude = "50";
            string longitude = "50";
            string units = "metric";
            string ApiKey = "f4cdb9a4d3badec1ff1423c4a5fba527";
            string url = $"https://api.openweathermap.org/data/2.5/onecall?lat={latitude}&lon={longitude}&exclude=minutely&units={units}&appid={ApiKey}";
            // Act
            API_Response response = await API_Caller.Get(url);
            var info = JsonConvert.DeserializeObject<Rootobject>(response.Response);
            // Assert
            Assert.IsNotNull(info);
        }
    }
}
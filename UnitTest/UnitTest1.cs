using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using WeatherApp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task APICallTest() //Test volání API
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
        public async Task DeserializeIntoObject1() //Test, zda je objekt správnì strukturován pro deserializaci (Weather)
        {
            // Arrange
            string ApiKey = "f4cdb9a4d3badec1ff1423c4a5fba527";
            string url = $"https://api.openweathermap.org/data/2.5/onecall?lat=50&lon=50&exclude=minutely&units=minutes&appid={ApiKey}";
            // Act
            API_Response response = await API_Caller.Get(url);
            var info = JsonConvert.DeserializeObject<Rootobject>(response.Response);
            // Assert
            Assert.IsNotNull(info);
        }
        [TestMethod]
        public async Task DeserializeIntoObject2() //Test, zda je objekt správnì strukturován pro deserializaci (GooglePlaceAutoComplete)
        {
            // Arrange
            string searchtext = "Praha";
            string ApiKey = "AIzaSyAtS_ahApNcoB7BoJdDopnScf4CWySsp3I";
            string url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={searchtext}&key={ApiKey}";
            // Act
            API_Response response = await API_Caller.Get(url);
            var info = JsonConvert.DeserializeObject<GooglePlaceAutoCompleteResult>(response.Response);
            // Assert
            Assert.IsNotNull(info);
        }
        [TestMethod]
        public async Task DeserializeIntoObject3() //Test, zda je objekt správnì strukturován pro deserializaci (GooglePlace)
        {
            // Arrange
            string placeId = "ChIJi3lwCZyTC0cRkEAWZg-vAAQ"; //Praha
            string ApiKey = "AIzaSyAtS_ahApNcoB7BoJdDopnScf4CWySsp3I";
            string url = $"https://maps.googleapis.com/maps/api/place/details/json?placeid={placeId}&key={ApiKey}";
            // Act
            API_Response response = await API_Caller.Get(url);
            var info = new GooglePlace(JObject.Parse(response.Response));
            // Assert
            Assert.IsNotNull(info);
        }
    }
}
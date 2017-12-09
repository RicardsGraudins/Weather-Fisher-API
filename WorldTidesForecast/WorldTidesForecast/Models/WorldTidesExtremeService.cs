using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using static WorldTidesForecast.Data.WorldTidesExtremes;

namespace WorldTidesForecast.Models
{
    public class WorldTidesExtremeService
    {
        public async static Task<RootObjectExtreme> GetMaxMinTides()
        {
            var http = new HttpClient();
            var url = String.Format("https://www.worldtides.info/api?extremes&lat=53&lon=-9&key=fc4813c4-07c1-4437-bb20-2a10f8c4fba0");
            //var url = String.Format("https://www.worldtides.info/api?extremes&lat={0}&lon={1}&key=fc4813c4-07c1-4437-bb20-2a10f8c4fba0", lat, lon);
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObjectExtreme));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObjectExtreme)serializer.ReadObject(ms);

            return data;
        }//GetMaxMinTides
    }//WorldTidesExtremeService
}//WorldTidesForecast
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
        public async static Task<RootObjectExtreme> GetMaxMinTides(double lat, double lon)
        {
            var http = new HttpClient();
            var url = String.Format("https://www.worldtides.info/api?extremes&lat={0}&lon={1}&key=834b4e65-cfde-40f8-aa73-00f3ea7fbc40", lat, lon);
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObjectExtreme));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObjectExtreme)serializer.ReadObject(ms);

            return data;
        }//GetMaxMinTides
    }//WorldTidesExtremeService
}//WorldTidesForecast
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static WorldTidesForecast.Data.WorldTidesExtremes;
using WorldTidesForecast.Models;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WorldTidesForecast.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index(string county)
        {
            WorldTidesExtremeDBService dbService = new WorldTidesExtremeDBService();

            //Retrieve county data from DB
            RootObjectExtreme data = dbService.ExecuteQueryReturnObject("TidesDB", "Tides", county);

            //Convert to JSON
            /*
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObjectExtreme));
            ser.WriteObject(stream1, data);

            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            */

            MemoryStream ms = new MemoryStream();

            //Ref https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-serialize-and-deserialize-json-data  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObjectExtreme));
            ser.WriteObject(ms, data);
            byte[] json = ms.ToArray();
            ms.Close();

            //Display JSON
            //ViewBag.json = sr.ReadToEnd();
            ViewBag.json = Encoding.UTF8.GetString(json, 0, json.Length);
            return View();
        }//Index
    }//HomeController
}//Controllers
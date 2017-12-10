using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static WorldTidesForecast.Data.WorldTidesExtremes;
using System.Web.Script.Serialization;
using WorldTidesForecast.Models;

namespace WorldTidesForecast.Controllers
{
    public class HomeController : Controller
    {
        WorldTidesExtremeDBService dbService = new WorldTidesExtremeDBService();

        public async Task<IActionResult> Index()
        {
            //RootObjectExtreme myTides = await Models.WorldTidesExtremeService.GetMaxMinTides(53, -9);

            //var time0 = myTides.extremes[0].dt;
            //var height0 = myTides.extremes[0].height;
            //var type0 = myTides.extremes[0].type;

            //ViewBag.test1 = time0;


            //var json = new JavaScriptSerializer().Serialize(myTides);
            //ViewBag.test1 = json;

            return View();
        }

        /*
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        */
    }
}
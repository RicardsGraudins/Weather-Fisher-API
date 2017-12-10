using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Timers;
using WorldTidesForecast.Models;

namespace WorldTidesForecast
{
    public class Program
    {
        public static WorldTidesExtremeDBService dbService = new WorldTidesExtremeDBService();

        public static void Main(string[] args)
        {
            //Ref https://stackoverflow.com/questions/10954859/run-function-every-second-visual-c-sharp
            //Create a timer
            Timer myTimer = new System.Timers.Timer();
            //Tell the timer what to do when it elapses
            myTimer.Elapsed += new ElapsedEventHandler(myEvent);
            //Set it to go off every 3 hours
            myTimer.Interval = 10800000;
            //And start it        
            myTimer.Enabled = true;
            BuildWebHost(args).Run();
        }//Main

        public static async void myEvent(object source, ElapsedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Updating...");
            //Delete the db
            dbService.DeleteDatabase("TidesDB");
            //Create the db & documents for each county
            await dbService.CreateDocuments();
        }//myEvent

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }//Program
}//WorldTidesForecast
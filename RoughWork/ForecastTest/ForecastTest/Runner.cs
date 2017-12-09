using System;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.IO;
using static ForecastTest.WorldTidesExtremes;
namespace ForecastTest
{
    public class Runner
    {
        //Replace EndpointURL and PrimaryKey
        private const string EndpointUrl = "PlaceholderURL";
        private const string PrimaryKey = "PlaceholderKey";
        private DocumentClient client;

        static void Main(string[] args)
        {
            try
            {
                Runner p = new Runner();
                p.GetStarted().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("Reached the end, press any key to exit.");
                Console.ReadKey();
            }
        }//Main

        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }//WriteToConsoleAndPromptToContinue

        private async Task GetStarted()
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            //Create the database and document collection
            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "TidesDB" });
            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("TidesDB"), new DocumentCollection { Id = "Tides" });

            //Get JSON for Galway
            double lat = 53, lon = -9;
            RootObjectExtreme myTides = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);

            //Setting county to Galway since its not included in the JSON
            myTides.county = "Galway";
            Console.WriteLine(myTides.county);
            Console.WriteLine("======");

            //Create & save document into the TideDB database, Tides collection
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", myTides);
            Console.WriteLine("==========================\n");

            //Look for the Galway document and execute query
            this.ExecuteQuery("TidesDB", "Tides", "Galway");

            //Executing query and returning RootObjectExtreme object
            RootObjectExtreme tide = this.ExecuteQueryReturnObject("TidesDB", "Tides", "Galway");
            Console.WriteLine("\nCounty: " + tide.county);
            Console.WriteLine("Extreme[0] type: " + tide.extremes[0].type);

            //Convert the object back into JSON
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObjectExtreme));
            ser.WriteObject(stream1, tide);

            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            Console.Write("JSON form of RootObjectExtreme object: \n");
            Console.Write("====================================== \n");
            Console.WriteLine(sr.ReadToEnd());

            DeleteDatabase("TidesDB");
        }//GetStarted

        //Get JSON, return RootObjectExtreme object
        public class WorldTidesExtremeService
        {
            public async static Task<RootObjectExtreme> GetMaxMinTides(double lat, double lon)
            {
                var http = new HttpClient();
                var url = String.Format("https://www.worldtides.info/api?extremes&lat={0}&lon={1}&key=fc4813c4-07c1-4437-bb20-2a10f8c4fba0", lat, lon);
                var response = await http.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(RootObjectExtreme));

                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                var data = (RootObjectExtreme)serializer.ReadObject(ms);

                return data;
            }//GetMaxMinTides
        }//WorldTidesExtremeService

        //Creating document
        private async Task CreateTideEntryIfNotExists(string databaseName, string collectionName, RootObjectExtreme tideObject)
        {
            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, tideObject.county));
                this.WriteToConsoleAndPromptToContinue("Found {0}", tideObject.county);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), tideObject);
                    this.WriteToConsoleAndPromptToContinue("Created document for {0}", tideObject.county);
                }
                else
                {
                    throw;
                }
            }
        }//CreateTideEntryIfNotExists

        //Delete the database
        private async void DeleteDatabase(string databaseName)
        {
            Console.WriteLine("\nDeleting the following database: " + databaseName);
            await this.client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
        }

        //Executing query - checking if the document exists in the database
        private void ExecuteQuery(string databaseName, string collectionName, string county)
        {
            //Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<RootObjectExtreme> tideQuery = this.client.CreateDocumentQuery<RootObjectExtreme>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                    .Where(f => f.county == county);

            //The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Console.WriteLine("Running LINQ query...");
            foreach (RootObjectExtreme tide in tideQuery)
            {
                Console.WriteLine("\tRead {0}", tide);
            }

            /*
            //Execute the same query via direct SQL
            IQueryable<RootObjectExtreme> tideQueryInSql = this.client.CreateDocumentQuery<RootObjectExtreme>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    "Select * from Tides where Tides.county = 'Galway'",
                    queryOptions);

            Console.WriteLine("Running direct SQL query...");
            foreach (RootObjectExtreme tide in tideQueryInSql)
            {
                Console.WriteLine("\tRead {0}", tide);
            }

            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
            */
        }//ExecuteQuery


        //Executing query & returning RootObjectExtreme object
        private RootObjectExtreme ExecuteQueryReturnObject(string databaseName, string collectionName, string county)
        {
            //Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<RootObjectExtreme> tideQuery = this.client.CreateDocumentQuery<RootObjectExtreme>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                    .Where(f => f.county == county);

            //The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            Console.WriteLine("Running LINQ query...");
            foreach (RootObjectExtreme tide in tideQuery)
            {
                Console.WriteLine("\tRead {0}", tide);
                return tide;
            }
            
            return null;
        }//ExecuteQueryReturnObject
    }//Runner
}//ForecastTest
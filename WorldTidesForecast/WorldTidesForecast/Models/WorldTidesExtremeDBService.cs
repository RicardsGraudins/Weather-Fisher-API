using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using static WorldTidesForecast.Data.WorldTidesExtremes;

namespace WorldTidesForecast.Models
{
    public class WorldTidesExtremeDBService
    {
        //Replace EndpointURL and PrimaryKey
        private const string EndpointUrl = "PlaceholderURL";
        private const string PrimaryKey = "PlaceholderKey";
        private DocumentClient client;

        //Creating document
        public async Task CreateTideEntryIfNotExists(string databaseName, string collectionName, RootObjectExtreme tideObject)
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, tideObject.county));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), tideObject);
                }
                else
                {
                    throw;
                }
            }
        }//CreateTideEntryIfNotExists

        //Delete the database
        public async void DeleteDatabase(string databaseName)
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            await this.client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
        }//DeleteDatabase

        //Executing query - checking if the document exists in the database
        public void ExecuteQuery(string databaseName, string collectionName, string county)
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            //Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<RootObjectExtreme> tideQuery = this.client.CreateDocumentQuery<RootObjectExtreme>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                    .Where(f => f.county == county);

            //The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            foreach (RootObjectExtreme tide in tideQuery)
            {
                System.Diagnostics.Debug.WriteLine("\tRead {0}", tide);
            }

            /*
            //Execute the same query via direct SQL
            IQueryable<RootObjectExtreme> tideQueryInSql = this.client.CreateDocumentQuery<RootObjectExtreme>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    "Select * from Tides where Tides.county = 'Galway'",
                    queryOptions);

            foreach (RootObjectExtreme tide in tideQueryInSql)
            {
                System.Diagnostics.Debug.WriteLine("\tRead {0}", tide);
            }
            */
        }//ExecuteQuery


        //Executing query & returning RootObjectExtreme object
        public RootObjectExtreme ExecuteQueryReturnObject(string databaseName, string collectionName, string county)
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            //Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<RootObjectExtreme> tideQuery = this.client.CreateDocumentQuery<RootObjectExtreme>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions)
                    .Where(f => f.county == county);

            //The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
            foreach (RootObjectExtreme tide in tideQuery)
            {
                System.Diagnostics.Debug.WriteLine("\tRead {0}", tide);
                return tide;
            }

            return null;
        }//ExecuteQueryReturnObject

        //Create documents for each county
        public async Task CreateDocuments()
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            //Create the database and document collection
            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "TidesDB" });
            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("TidesDB"), new DocumentCollection { Id = "Tides" });

            double lat = 0, lon = 0;

            //Creating RootObjectExreme objects for each county
            //Carlow
            lat = 52.83;
            lon = -6.93;
            RootObjectExtreme Carlow = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Cavan
            lat = 53.97;
            lon = -7.29;
            RootObjectExtreme Cavan = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Clare
            lat = 52.90;
            lon = -8.98;
            RootObjectExtreme Clare = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Cork
            lat = 51.89;
            lon = -8.48;
            RootObjectExtreme Cork = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Donegal
            lat = 54.65;
            lon = -8.10;
            RootObjectExtreme Donegal = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Dublin
            lat = 53.34;
            lon = -6.26;
            RootObjectExtreme Dublin = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Galway
            lat = 53.27;
            lon = -9.06;
            RootObjectExtreme Galway = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Kerry
            lat = 52.15;
            lon = -9.56;
            RootObjectExtreme Kerry = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Kildare
            lat = 53.15;
            lon = -6.60;
            RootObjectExtreme Kildare = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Kilkenny
            lat = 52.65;
            lon = -7.24;
            RootObjectExtreme Kilkenny = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Laois
            lat = 52.99;
            lon = -7.33;
            RootObjectExtreme Laois = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Leitrim
            lat = 53.94;
            lon = -8.08;
            RootObjectExtreme Leitrim = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Limerick
            lat = 52.66;
            lon = -8.63;
            RootObjectExtreme Limerick = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Longford
            lat = 53.72;
            lon = -7.79;
            RootObjectExtreme Longford = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Louth
            lat = 53.92;
            lon = -6.48;
            RootObjectExtreme Louth = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Mayo
            lat = 54.01;
            lon = -9.42;
            RootObjectExtreme Mayo = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Meath
            lat = 53.60;
            lon = -6.65;
            RootObjectExtreme Meath = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Monaghan
            lat = 54.24;
            lon = -6.96;
            RootObjectExtreme Monaghan = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Offaly
            lat = 53.09;
            lon = -7.90;
            RootObjectExtreme Offaly = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Roscommon
            lat = 53.75;
            lon = -8.26;
            RootObjectExtreme Roscommon = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Sligo
            lat = 54.27;
            lon = -8.47;
            RootObjectExtreme Sligo = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Tipperary
            lat = 52.47;
            lon = -8.16;
            RootObjectExtreme Tipperary = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Waterford
            lat = 52.25;
            lon = -7.11;
            RootObjectExtreme Waterford = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Westmeath
            lat = 53.53;
            lon = -7.46;
            RootObjectExtreme Westmeath = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Wexford
            lat = 52.33;
            lon = -6.46;
            RootObjectExtreme Wexford = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);
            //Wicklow
            lat = 52.98;
            lon = -6.36;
            RootObjectExtreme Wicklow = await WorldTidesExtremeService.GetMaxMinTides(lat, lon);

            //Creating documents for each county
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Carlow);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Cavan);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Clare);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Cork);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Donegal);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Dublin);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Galway);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Kerry);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Kildare);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Kilkenny);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Laois);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Leitrim);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Limerick);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Longford);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Louth);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Mayo);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Meath);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Monaghan);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Offaly);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Roscommon);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Sligo);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Tipperary);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Waterford);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Westmeath);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Wexford);
            await this.CreateTideEntryIfNotExists("TidesDB", "Tides", Wicklow);
        }//CreateDocuments
    }//WorldTidesExtremeDBService
}//Models
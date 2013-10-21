using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SteamLibraryIntersecter.Models;
using SteamLibraryIntersecter.Steam.Entities;

namespace SteamLibraryIntersecter.DAL
{
    public class Dal : IDisposable
    {
        private MongoServer mongoServer = null;
        private bool disposed = false;


        private string connectionString = System.Environment.GetEnvironmentVariable("CUSTOMCONNSTR_MONGOLAB_URI");


        private string dbName = "MongoLab-ec";
        private string collectionName = "Games";


        // Creates a Note and inserts it into the collection in MongoDB.
        public void InsertGames(params SteamGame[] game)
        {
            var collection = GetGamesCollection();
            try
            {
                collection.InsertBatch(game, WriteConcern.Acknowledged);
            }
            catch (MongoCommandException ex)
            {
                var x = ex;
            }
        }

        public IEnumerable<SteamGame> RetrieveGames(params string[] appIds)
        {
            var collection = GetGamesCollection();

            return collection.FindAs<SteamGame>(Query<SteamGame>.In(x => x.AppId, appIds.Select(x => new BsonString(x))));
        }

        private MongoCollection<SteamGame> GetGamesCollection()
        {
            var client = (ConfigurationManager.AppSettings["onAzure"] == "true") ? new MongoClient(connectionString) : new MongoClient();

            var database = client.GetServer()[dbName];
            return database.GetCollection<SteamGame>(collectionName);
        }


        # region IDisposable


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (mongoServer != null)
                    {
                        this.mongoServer.Disconnect();
                    }
                }
            }


            this.disposed = true;
        }


        # endregion
    }
}
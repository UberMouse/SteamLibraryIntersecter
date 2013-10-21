using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using EasyHttp.Http;
using Newtonsoft.Json;
using SteamLibraryIntersecter.DAL;
using SteamLibraryIntersecter.Models;
using SteamLibraryIntersecter.Steam.Entities;

namespace SteamLibraryIntersecter.Steam
{
    public class Store
    {
        private static readonly string API_ENDPOINT = ConfigurationManager.AppSettings["Steam.StoreApiEndpoint"];
        private readonly HttpClient _httpClient;

        public Store(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Game GetAppInfo(string appId)
        {
            return GetAppInfos(appId).First();    
        }

        public IEnumerable<Game> GetAppInfos(params string[] appIds)
        {
            if(appIds == null) throw new ArgumentNullException();
            if(appIds.Any(x => !x.IsNumerical())) throw new ArgumentException(string.Format("The following AppIds are malformed: {0}", 
                                                                                            string.Join(",", 
                                                                                                        appIds.Where(x => !x.IsNumerical()))));
            var existingGames = new List<Game>();
            using (var dal = new Dal())
            {
                var dbGames = dal.RetrieveGames(appIds);
                existingGames.AddRange(dbGames.Select(SteamGame.ToGame));

                appIds = appIds.Where(x => existingGames.All(y => y.AppId != x)).OrderBy(int.Parse).ToArray();
            }

            var groupedIds = appIds.Select((i, index) => new
                                                         {
                                                             i,
                                                             index
               
                                                         }).GroupBy(group => group.index / 50, element => element.i);
            var retrievedGames = new List<Game>(200);
            
            foreach (var group in groupedIds)
            {
                var ids = group.ToList();
                var response = _httpClient.Get(API_ENDPOINT + string.Join(",", ids)).RawText;
                var parsedJson = JsonConvert.DeserializeObject<Dictionary<string, StoreApiResponse>>(response);

                foreach (var id in ids)
                    retrievedGames.Add(_parseGame(parsedJson[id], id));
            }
            using (var dal = new Dal())
            {
                dal.InsertGames(retrievedGames.Where(x => x.Success).Select(SteamGame.FromGame).ToArray());
            }
            existingGames.AddRange(retrievedGames);
            return existingGames;
        }

        private Game _parseGame(StoreApiResponse appInfo, string appId)
        {
            if (!appInfo.Success || appInfo.Data.Type != "game" || appInfo.Data.Categories == null) return new Game {Success = false};
            const string MultiPlayerId = "1";
            const string CoopId = "9";

            var categories = appInfo.Data.Categories;
            var coop = categories.Any(x => x.Id == CoopId);
            var mp = categories.Any(x => x.Id == MultiPlayerId);

            return new Game {AppId = appId, Coop = coop, Multiplayer = mp, Name = appInfo.Data.Name};
        }

        class StoreApiResponse
        {
            public bool Success { get; set; }
            public StoreApiData Data { get; set; }
        }

        class StoreApiData
        {
            public List<StoreApiCategory> Categories { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
        }

        class StoreApiCategory
        {
            public string Id { get; set; }
        }
    }
}
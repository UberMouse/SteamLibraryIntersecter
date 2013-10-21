using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using EasyHttp.Http;
using SteamLibraryIntersecter.Ninject;
using SteamLibraryIntersecter.Steam.Entities;
using HttpResponse = EasyHttp.Http.HttpResponse;

namespace SteamLibraryIntersecter.Steam
{
    public class Community
    {
        private static readonly string API_ENDPOINT = string.Format(ConfigurationManager.AppSettings["Steam.WebApiEndpoint"],
                                                                    ConfigurationManager.AppSettings["Steam.ApiKey"]);

        private readonly HttpClient _httpClient;

        public Community(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Player GetUserInfo(string steamId)
        {
            if(steamId == null) throw new ArgumentNullException();
            if(!steamId.IsNumerical()) throw new ArgumentException("steamId is malformed, must be numerical");

            var store = NinjectCore.Get<Store>();

            var resp = _httpClient.Get(API_ENDPOINT + steamId).StaticBody<WebApiResponse>();
            var games = store.GetAppInfos(resp.response.games.Select(x => x.appid).ToArray());
            return new Player {Games = games.ToList(), SteamId = steamId};
        }

        class WebApiResponse
        {
            public WebApiGames response { get; set; }
            public class WebApiGames
            {
                public List<WebApiGame> games
                {
                    get; set;
                } 
            }
        }

        class WebApiGame
        {
            public string appid { get; set; }
        }
    }
}
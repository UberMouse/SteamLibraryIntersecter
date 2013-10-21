using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using SteamLibraryIntersecter.DAL;
using SteamLibraryIntersecter.Models;
using SteamLibraryIntersecter.Ninject;
using SteamLibraryIntersecter.Steam;
using SteamLibraryIntersecter.Steam.Entities;

namespace SteamLibraryIntersecter.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Index/

        public ActionResult Index()
        {
            return View();
        }

        public ViewResult DisplayIntersection(string steamIds)
        {
            var community = NinjectCore.Get<Community>();

            var players = steamIds.Split(new[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries)
                                  .Select(x => (x.IsNumerical()) ? x : community.ResolveVanityUrl(x))
                                  .Select(community.GetUserInfo);

            var coopGames = players.First()
                                   .IntersectLibrary(players.Skip(1).ToArray())
                                   .Where(x => x.Coop)
                                   .GroupBy(x => x.Name)
                                   .Select(x => x.First());

            return View("DisplayIntersection", coopGames);
        }

    }
}

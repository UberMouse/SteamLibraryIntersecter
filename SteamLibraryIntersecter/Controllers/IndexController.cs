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

        public ViewResult DisplayIntersection(string firstSteamId, string secondSteamId)
        {
            var community = NinjectCore.Get<Community>();

            if (!firstSteamId.IsNumerical()) firstSteamId = community.ResolveVanityUrl(firstSteamId);
            if (!secondSteamId.IsNumerical()) secondSteamId = community.ResolveVanityUrl(secondSteamId);

            var firstPlayer = community.GetUserInfo(firstSteamId);
            var secondPlayer = community.GetUserInfo(secondSteamId);

            var coopGames = firstPlayer.IntersectLibrary(secondPlayer)
                                       .Where(x => x.Coop)
                                       .GroupBy(x => x.Name)
                                       .Select(x => x.First());

            return View("DisplayIntersection", coopGames);
        }

    }
}

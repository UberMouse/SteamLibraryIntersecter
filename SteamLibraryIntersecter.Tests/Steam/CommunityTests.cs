using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamLibraryIntersecter.Ninject;
using SteamLibraryIntersecter.Steam;
using SteamLibraryIntersecter.Steam.Entities;

namespace SteamLibraryIntersecter.Tests.Steam
{
    [TestClass]
    public class SteamCommunityTests
    {
        private Community community;

        [TestInitialize]
        public void Setup()
        {
            community = NinjectCore.Get<Community>();
        }

        [TestMethod]
        public void GetUserInfoReturnsCorrectPlayerObject()
        {
            var expectedPlayer = new Player
            {
                Games = new List<Game>()
                {
                    new Game {AppId = "72850", Coop = false, Multiplayer = false, Name = "The Elder Scrolls V: Skyrim"},
                    new Game {AppId = "105450", Coop = false, Multiplayer = true, Name = "Age of Empires® III: Complete Collection"}
                },
                SteamId = "76561198052179741"
            };
            var actualPlayer = community.GetUserInfo("76561198052179741");

            Assert.AreEqual(expectedPlayer.SteamId, actualPlayer.SteamId);
            
            foreach (var game in expectedPlayer.Games)
                Assert.IsTrue(actualPlayer.Games.Contains(game), string.Format("Expected to find {0}", game));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetUserInfoThrowsArgumentNullExceptionWhenCalledWithNullId()
        {
            community.GetUserInfo(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUserInfoThrowsArgumentExceptionWhenCalledWithNullId()
        {
            community.GetUserInfo("1234a");
        }

        [TestMethod]
        public void ResolveVanityUrlReturnsCorrectSteamId()
        {
            Assert.AreEqual(community.ResolveVanityUrl("ubermouse"), "76561197994157624");
            Assert.AreEqual(community.ResolveVanityUrl("flyboy95"), "76561197996719044");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ResolveVanityUrlThrowsArgumentExceptionOnInvalidVanityUrl()
        {
            community.ResolveVanityUrl("fakevanityurl");
        }
    }
}

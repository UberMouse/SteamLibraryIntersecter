using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamLibraryIntersecter.Ninject;
using SteamLibraryIntersecter.Steam;
using SteamLibraryIntersecter.Steam.Entities;

namespace SteamLibraryIntersecter.Tests.Steam
{
    [TestClass]
    public class StoreTests
    {
        private Store store;

        [TestInitialize]
        public void Setup()
        {
            store = NinjectCore.Get<Store>();        
        }

        [TestMethod]
        public void GetAppInfoReturnsCorrectInfoForAppId()
        {
            var expectedAudioSurf = new Game {AppId = "12900", Coop = true, Multiplayer = false, Name = "AudioSurf"};
            var expectedRedOrchestra = new Game { AppId = "1200", Coop = false, Multiplayer = true, Name = "Red Orchestra: Ostfront 41-45" };
            var actualAudioSurf = store.GetAppInfo("12900");
            var actualRedOrchestra = store.GetAppInfo("1200");

            Assert.AreEqual(expectedAudioSurf, actualAudioSurf);
            Assert.AreEqual(expectedRedOrchestra, actualRedOrchestra);
            Assert.IsTrue(actualAudioSurf.Success && actualRedOrchestra.Success, "Success is false");
        }
    
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAppInfosThrowsArgumentNullExceptionWhenGivenNullAppIds()
        {
            store.GetAppInfos(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAppInfosThrowsArgumentExceptionWhenGivenMalformedAppIds()
        {
            store.GetAppInfos("12", "1a2", "14", "aa", "a1a");
        }
    }
}

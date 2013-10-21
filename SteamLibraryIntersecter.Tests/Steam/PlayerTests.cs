using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteamLibraryIntersecter.Steam.Entities;

namespace SteamLibraryIntersecter.Tests.Steam
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void IntersectLibrary()
        {
            var expectedGames = new List<Game>
            {
                new Game()
                {
                    AppId = "1",
                    Coop = true,
                    Multiplayer = false,
                    Name = "TestApp1"
                },
                new Game()
                {
                    AppId = "3",
                    Coop = true,
                    Multiplayer = true,
                    Name = "TestApp3"
                }
            };

            var dummyPlayer1 = new Player
            {
                Games = new List<Game>
                {
                    new Game()
                    {
                        AppId = "1",
                        Coop = true,
                        Multiplayer = false,
                        Name = "TestApp1"
                    },
                    new Game()
                    {
                        AppId = "2",
                        Coop = false,
                        Multiplayer = true,
                        Name = "TestApp2"
                    },
                    new Game()
                    {
                        AppId = "3",
                        Coop = true,
                        Multiplayer = true,
                        Name = "TestApp3"
                    }
                },
                SteamId = "1"
            };

            var dummyPlayer2 = new Player
            {
                Games = new List<Game>
                {
                    new Game()
                    {
                        AppId = "1",
                        Coop = true,
                        Multiplayer = false,
                        Name = "TestApp1"
                    },
                    new Game()
                    {
                        AppId = "3",
                        Coop = true,
                        Multiplayer = true,
                        Name = "TestApp3"
                    }
                },
                SteamId = "2"
            };


            var actualGames = dummyPlayer1.IntersectLibrary(dummyPlayer2).ToList();
            Assert.IsTrue(actualGames.Count == expectedGames.Count, (actualGames.Count < expectedGames.Count) ? "Intersection is missing games" : "Intersection has too many games");
            foreach (var game in actualGames)
                Assert.IsTrue(expectedGames.Contains(game), string.Format("Intersected library is missing {0}", game.Name));
        }
    }
}

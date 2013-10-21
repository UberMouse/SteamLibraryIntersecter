using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using SteamLibraryIntersecter.Steam.Entities;

namespace SteamLibraryIntersecter.Models
{
    public class SteamGame
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        [BsonElement("AppId")]
        public string AppId { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Coop")]
        public bool Coop { get; set; }
        [BsonElement("Multiplayer")]
        public bool Multiplayer { get; set; }

        public static SteamGame FromGame(Game game)
        {
            return new SteamGame
            {
                AppId = game.AppId,
                Coop = game.Coop,
                Multiplayer = game.Multiplayer,
                Name = game.Name
            };
        }

        public static Game ToGame(SteamGame game)
        {
            return new Game
            {
                AppId = game.AppId,
                Coop = game.Coop,
                Multiplayer = game.Multiplayer,
                Name = game.Name
            };
        }
    }
}
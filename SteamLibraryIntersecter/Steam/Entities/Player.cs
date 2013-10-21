using System.Collections.Generic;
using System.Linq;

namespace SteamLibraryIntersecter.Steam.Entities
{
    public class Player
    {
        public string SteamId { get; set; }
        public List<Game> Games { get; set; }

        public override string ToString()
        {
            return string.Format("SteamId: {0}", SteamId);
        }

        public IEnumerable<Game> IntersectLibrary(params Player[] players)
        {
            return players.Aggregate(Games as IEnumerable<Game>, (current, player) => current.Where(x => player.Games.Contains(x)));
        }

        public bool Equals(Player other)
        {
            return string.Equals(SteamId, other.SteamId) && Equals(Games, other.Games);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Player && Equals((Player) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((SteamId != null ? SteamId.GetHashCode() : 0)*397) ^ (Games != null ? Games.GetHashCode() : 0);
            }
        }
    }
}
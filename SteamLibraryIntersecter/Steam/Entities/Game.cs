namespace SteamLibraryIntersecter.Steam.Entities
{
    public class Game
    {
        public string AppId { get; set; }
        public string Name { get; set; }
        public bool Coop { get; set; }
        public bool Multiplayer { get; set; }
        public bool Success { get; set; }

        public Game()
        {
            Success = true;
        }

        public override string ToString()
        {
            return string.Format("AppId: {0}, Name: {1}, Coop: {2}, Multiplayer: {3}", AppId, Name, Coop, Multiplayer);
        }

        protected bool Equals(Game other)
        {
            return Multiplayer.Equals(other.Multiplayer) && string.Equals(Name, other.Name) && string.Equals(AppId, other.AppId) && Coop.Equals(other.Coop);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Game) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Multiplayer.GetHashCode();
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AppId != null ? AppId.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Coop.GetHashCode();
                return hashCode;
            }
        }
    }
}
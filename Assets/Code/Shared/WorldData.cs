using Assets.Code.Components.Containers;
using System.Collections.Generic;

namespace Assets.Code.Shared
{
    public class WorldData
    {
        public List<Character> Players { get; set; }

        public WorldData()
        {
            Players = new List<Character>();
        }

        public void AddPlayer(Character player)
        {
            Players.Add(player);
        }
    }
}

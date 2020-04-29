using System;
using System.Collections.Generic;
using System.Text;

namespace TextGame
{
    public class Player
    {
        public string Name { get; set; }
        public int NumberOfMonsters { get; set; }
        public Monster[] Party { get; set; }

        public Player(string name, Monster[] party)
        {
            this.Party = party;
            this.Name = name;
            NumberOfMonsters = Party.Length;
        }
    }
}

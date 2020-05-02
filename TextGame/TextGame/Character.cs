using System;
using System.Collections.Generic;
using System.Text;

namespace TextGame
{
    public class Character
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfMonsters { get; set; }
        public int UsableMonsters { get; set; }
        public Monster[] Party { get; set; } = new Monster[5];
        public int Reward { get; set; } = 0;

        public Character(string name, Monster[] party)
        {
            this.Party = party;
            this.Name = name;
            NumberOfMonsters = Party.Length;
            UsableMonsters = GetUsableMonsters(this);
        }

        public static int GetUsableMonsters(Character x)
        {
            int usableMonsters = 0;
            for (int i = 0; i < x.Party.Length; i++)
            {
                if (x.Party[i].Status != "FNT")
                {
                    usableMonsters++;
                }
            }
            return usableMonsters;
        }
    }
}

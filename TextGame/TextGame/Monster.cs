using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TextGame
{
    public class Monster
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int LV { get; private set; }
        public int XP { get; set; }
        public int XPToNextLV { get; set; }
        // Stats [0,1,2,3,4] = MaxHP, Atk, Def, Special, Speed
        public float[] BaseStats { get; private set; } = new float[5];
        public float[] Stats { get; private set; } = new float[5]; 
        public int HP { get; set; }
        public int[] StatsDisplay { get; private set; } = new int[5];
        public Move[] Moves { get; set; }
        public string Status { get; set; }

        public Monster(int id, string name, int lv)
        {
            this.ID = id;
            this.Name = name;
            this.LV = lv;
            this.BaseStats = GetBaseStats();
            this.Stats = StatsBuilder();
        }

        public void CheckIfLvlUp()
        {
            if (XP > XPToNextLV)
            {
                Console.WriteLine("{0} leveled up!", Name);
                LevelUp();
            }
        }

        public void LevelUp()
        {
            while (XP > XPToNextLV)
            {
                Stats = StatsBuilder();
                LV++;
                XP -= XPToNextLV;
                float fXPToNextLV = XPToNextLV;
                fXPToNextLV *= LevelScaler();
                XPToNextLV = (int)Math.Round(fXPToNextLV);
            }
        }

        public float LevelScaler()
        {
            float[] EXPDifficultyGroups = { 1.56f, 1.3f, 1.15f };
            return EXPDifficultyGroups[ID];
        }

        public float[] StatsBuilder()
        {
            float[] newStats = new float[5];
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    newStats[i] = ((2f * BaseStats[i]) * LV / 100f + LV + 10f);
                    StatsDisplay[i] = (int)Math.Round(newStats[i]);
                }
                newStats[i] = ((2f * BaseStats[i]) * LV / 100f + 5f);
                StatsDisplay[i] = (int)Math.Round(newStats[i]);
            }

            return newStats;
        }

        public float[] GetBaseStats()
        {
            return ID switch
            {
                1 => new float[] { 100, 49, 49, 65, 45 },
                2 => new float[] { 100, 52, 43, 50, 65 },
                3 => new float[] { 100, 48, 65, 50, 43 },
                _ => new float[] { },
            };
        }
    }
}

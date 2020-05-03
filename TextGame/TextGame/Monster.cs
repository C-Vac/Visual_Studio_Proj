using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TextGame
{
    public class Monster
    {
        public int ID { get; private set; } = 0;
        public string Name { get; private set; } = "";
        public string Nickname { get; set; } = "";
        public string Type1 { get; set; } = "normal";
        public string Type2 { get; set; } = null;
        public int LV { get; private set; } = 1;
        public int XP { get; set; } = 0;
        public int XPToNextLV { get; set; } = 0;
        // Stats [0,1,2,3,4] = MaxHP, Atk, Def, Special, Speed
        public float[] BaseStats { get; private set; } = new float[5];
        public float[] Stats { get; set; } = new float[5];
        public int HP { get; set; } = 0;
        public int[] StatsDisplay { get; private set; } = new int[5];
        public int[] IV { get; private set; } = new int[5];
        public Move[] Moves { get; set; } = new Move[4];
        public string Status { get; set; } = "FNT";

        public Monster(int id, int lv)
        {
            this.ID = id;
            string[] nameAndType = GetNameAndType().Split('`');
            this.Name = nameAndType[0];
            this.Nickname = Name;
            this.Type1 = nameAndType[1];
            if (nameAndType.Length == 3)
            {
                this.Type2 = nameAndType[2];
            }
            this.LV = lv;

            if (Name != "")
            {
                this.BaseStats = GetBaseStats();
                this.IV = IVBuilder();
                this.Stats = StatsBuilder();
                this.HP = (int)Math.Round(Stats[0]);
                if (HP > 0)
                {
                    Status = "OK";
                }
            }
        }

        public int[] IVBuilder()
        {
            Random r = new Random();
            int[] ivs = new int[5];
            for (int i = 1; i < 5; i++)
            {
                ivs[i] = r.Next(0, 15);
            }

            ivs[0] = 0;
            if (ivs[1] % 2 != 0)
            {
                ivs[0] += 8;
            }
            if (ivs[2] % 2 != 0)
            {
                ivs[0] += 4;
            }
            if (ivs[3] % 2 != 0)
            {
                ivs[0] += 2;
            }
            if (ivs[4] % 2 != 0)
            {
                ivs[0] += 1;
            }
            return ivs;
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
            
            newStats[0] = 2f * (BaseStats[0] + IV[0]) * LV / 100f + LV + 10;
            this.StatsDisplay[0] = (int)Math.Round(newStats[0]);
            
            for (int i = 1; i < 5; i++)
            {
                newStats[i] = 2f*(BaseStats[i] + IV[i])*LV/100f + 5;
                this.StatsDisplay[i] = (int)Math.Round(newStats[i]);
            }
            return newStats;
        }

        public float[] GetBaseStats()
        {
            return ID switch
            {
                1 => new float[] { 45, 49, 49, 65, 45 },
                2 => new float[] { 39, 52, 43, 50, 65 },
                3 => new float[] { 44, 48, 65, 50, 43 },
                _ => new float[] { },
            };
        }
        public string GetNameAndType()
        {
            return ID switch
            {
                1 => "PLANTFROG`GRASS",
                2 => "LIZARDFIRE`FIRE",
                3 => "TURTLEGUY`WATER",
                _ => "`NORMAL",
            };
        }
        
    }

    
}

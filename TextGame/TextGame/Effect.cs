using System;
using System.Collections.Generic;
using System.Text;

namespace TextGame
{
    public class Effect
    {
        public string StatModified;
        public int StatModifiedInt;
        public int StagesModified;

        public Effect(string statModified, int stagesModified)
        {
            StatModified = statModified;
            StatModifiedInt = StatStringTable(statModified);
            StagesModified = stagesModified;
        }

        public int StatStringTable(string statString)
        {
            switch (statString.ToLower())
            {
                case "attack":
                    return 1;
                case "defense":
                    return 2;
                case "special":
                    return 3;
                case "speed":
                    return 4;
                case "accuracy":
                    return 5;
                case "evasion":
                    return 6;
                default:
                    break;
            }
            return 0;
        }

        public float MultTable(int stagesModified)
        {
            int index = 7 + stagesModified;
            float[] multTable =
                {
                    .25f, .285f, .33f, .4f, .5f, .66f,
                    1, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f
                };

            return multTable[index];
        }
    }

    
}

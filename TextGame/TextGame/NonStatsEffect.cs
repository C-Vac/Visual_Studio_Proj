using System;
using System.Collections.Generic;
using System.Text;

namespace TextGame
{
    public class NonStatsEffect
    {
        public bool HasEffect = false;
        public NonStatsEffect(bool hasEffect)
        {
            HasEffect = hasEffect;
        }

        public static int Absorb(float damage, int userHP)
        {
            return userHP += (int)Math.Round(damage * .5f);
        }

        public static string Burn(float burnChance)
        {
            Random r = new Random();
            if (r.NextDouble() < burnChance)
            {
                return "BRN";
            }
            else
            {
                return "OK";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TextGame
{
    public class Move
    {
        public string Name { get; private set; } = "--------";
        public byte Power { get; private set; } = 0;
        public float Accuracy { get; private set; } = 1f;
        public byte PP { get; set; } = 0;
        public byte MaxPP { get; set; } = 0;
        public string Type { get; private set; } = "NORMAL";
        public bool IsPhysical { get; private set; } = true;
        public bool IsStatus { get; private set; } = false;
        public bool TargetSelf { get; set; } = false;
        public Effect Effect { get; set; } = new Effect("", 0);
        public NonStatsEffect NSEffect { get; set; } = null;

        public Move(string name, string type, byte power, float accuracy, 
            byte pp, bool isPhys, bool isStatus, bool targetSelf)
        {
            Name = name;
            Power = power;
            Type = type;
            Accuracy = accuracy;
            MaxPP = pp;
            PP = MaxPP;
            IsPhysical = isPhys;
            IsStatus = isStatus;
            TargetSelf = targetSelf;
        }
    }
}

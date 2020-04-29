using System;
using System.Collections.Generic;
using System.Text;

namespace TextGame
{
    public class Move
    {
        public string Name { get; private set; }
        public byte Power { get; private set; }
        public float Accuracy { get; private set; }
        public byte PP { get; private set; }
        public object Effect { get; private set; }
        public string Type { get; private set; }
        public bool IsPhysical { get; private set; }
        public bool IsStatus { get; private set; }

        public Move(string name, byte power, float accuracy, byte pp, bool isPhys, bool isStatus)
        {
            Name = name;
            Power = power;
            Accuracy = accuracy;
            PP = pp;
            IsPhysical = isPhys;
            IsStatus = isStatus;
        }

        
    }
}

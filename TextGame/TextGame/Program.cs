using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Timers;

namespace TextGame
{
    public class Program
    {
        public static void Main()
        {
            Move[] moveList = MoveList();
            
            Monster bulbasaur = new Monster(1, "Bulbasaur", 5);
            bulbasaur.Moves = new Move[] { moveList[0], moveList[2] };
            Monster squirtle = new Monster(3, "Squirtle", 5);
            squirtle.Moves = new Move[] { moveList[0], moveList[1] };

            Player player = new Player("C-Vac", new Monster[] { bulbasaur , null, null,
             null, null, null,});
            Player rival = new Player("Angery Liberal", new Monster[] { squirtle , null, null,
             null, null, null,});

            Battle test = new Battle(player, rival);
            test.Go();

        }

        public static Move[] MoveList()
        {
            Move Tackle = new Move("Tackle", 35, .95f, 35, true, false);
            Move TailWhip = new Move("Tail Whip", 0, 1f, 30, false, true);
            Move Growl = new Move("Growl", 0, 1f, 40, false, true);
            
            Move[] moveList = new Move[] { Tackle, TailWhip, Growl };

            return moveList;
        }

    }
}

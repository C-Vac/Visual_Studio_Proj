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
            Random r = new Random();
            Move[] moveList = MoveList();

            Monster squirt2 = new Monster(3, 15);
            squirt2.Moves = new Move[] { moveList[6], moveList[3], moveList[1], moveList[0] };
            squirt2.Nickname = "Retard";
            Monster bulba2 = new Monster(1, 22);
            bulba2.Nickname = "Bulbie <3";
            bulba2.Moves = new Move[] { moveList[4], moveList[3], moveList[1], moveList[0] };


            Monster squirt = new Monster(3, 18);
            squirt.Moves = new Move[] { moveList[6], moveList[5], moveList[1], moveList[0] };
            Monster charm = new Monster(2, 25);
            charm.Moves = new Move[] { moveList[5], moveList[5], moveList[1], moveList[0] };

            Monster empty = new Monster(0, 0);

            Character player = new Character("C-Vac", new Monster[] { squirt2, bulba2, empty,
                 empty, empty, empty,});
            Character rival = new Character("GARY", new Monster[] { charm , squirt, empty,
                 empty, empty, empty,});
            rival.Reward = 5;
            rival.Description = "Rival";

            while (0 == 0)
            {
                Battle test = new Battle(player, rival);
                test.Go();

                player.Party = MonsterCenter(player.Party);
                rival.Party = MonsterCenter(rival.Party);
            }
            /* TEST INPUT
                ConsoleKeyInfo cki;
                Console.TreatControlCAsInput = true; 
                Console.WriteLine("Press any combination of CTL, ALT, and SHIFT, and a console key.");
                Console.WriteLine("Press the Escape (Esc) key to quit: \n");
                do
                {
                    cki = Console.ReadKey();
                    Console.Write(" --- You pressed ");
                    if ((cki.Modifiers & ConsoleModifiers.Alt) != 0) Console.Write("ALT+");
                    if ((cki.Modifiers & ConsoleModifiers.Shift) != 0) Console.Write("SHIFT+");
                    if ((cki.Modifiers & ConsoleModifiers.Control) != 0) Console.Write("CTL+");
                    Console.WriteLine(cki.Key.ToString());

                } while (cki.Key != ConsoleKey.Escape);
                END TEST */
        }

        public static Monster[] MonsterCenter(Monster[] p)
        {
            foreach (Monster m in p)
            {
                if (m.Name != "")
                {
                    m.HP = m.StatsDisplay[0];
                    m.Status = "OK";
                }
            }
            return p;
        }

        public static Move[] MoveList()
        {
            Move Empty = new Move("--------", "NORMAL", 0, 1f, 0, false, false, false);

            Move Tackle = new Move("TACKLE", "NORMAL", 35, .95f, 35, true, false, false);

            Move TailWhip = new Move("TAIL WHIP", "NORMAL", 0, 1f, 30, false, true, false);
            TailWhip.Effect = new Effect("defense", -1);

            Move Growl = new Move("GROWL", "NORMAL", 0, 1f, 40, false, true, false);
            Growl.Effect = new Effect("attack", -1);

            Move Ember = new Move("EMBER", "FIRE", 40, 1f, 25, false, false, false);
            Ember.NSEffect = new NonStatsEffect(true);

            Move WaterGun = new Move("WATER GUN", "WATER", 40, 1f, 25, false, false, false);

            Move Absorb = new Move("ABSORB", "GRASS", 20, 1f, 25, false, false, false);
            Absorb.NSEffect = new NonStatsEffect(true);

            Move[] moveList = new Move[] { Empty,  Tackle, TailWhip, Growl, Absorb, Ember, WaterGun};

            return moveList;
        }

    }
}

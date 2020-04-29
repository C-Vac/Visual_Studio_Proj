using System;
using System.Collections.Generic;
using System.Text;

namespace TextGame
{
    public class Battle
    {
        Player Player { get; set; }
        Player Opponent { get; set; }
        Monster PlayerMonster { get; set; }
        public Monster OpponentMonster { get; set; }
        public bool PlayerInitiative { get; set; }

        public Battle(Player player1, Player player2)
        {
            Player = player1;
            Opponent = player2;
            PlayerMonster = Player.Party[0];
            OpponentMonster = Opponent.Party[0];

            PlayerMonster.HP = Convert.ToInt32(PlayerMonster.StatsDisplay[0]);
            OpponentMonster.HP = Convert.ToInt32(PlayerMonster.StatsDisplay[0]);
        }

        public void Go()
        {
            DisplayNewWindow(Opponent.Name + " would like to battle...");
            Console.ReadKey();
            DisplayOnlyOpponent(Opponent.Name + " sends out " + OpponentMonster.Name);
            Console.ReadKey();
            DisplayOnlyOpponent("Go, " + PlayerMonster.Name + "!");
            Console.ReadKey();

            while (!Update())
            {
                PlayerMove();
            }
            if (OpponentMonster.HP <= 0)
            {
                Console.WriteLine("{0}'s {1} fainted.",
                    Opponent.Name, OpponentMonster.Name);
            } else if (PlayerMonster.HP <= 0)
            {
                Console.WriteLine("{0}'s {1} fainted.",
                    Player.Name, PlayerMonster.Name);
            }
        }
        public Random r = new Random();

        public static string HPVisualizer(int hp, int maxHP)
        {
            char[] output = { '|', ' ', ' ', ' ', ' ', ' ', 
                ' ', ' ', ' ', ' ', ' ', '|' };

            float fraction = (float)hp*10 / maxHP;
            int digits = (int)Math.Round(fraction);
            for (int i = 1; i <= digits && i <=10; i++)
            {
                output[i] = '=';
            }
            return new string(output);
        }

        public string[] FormatMessage(string message)
        {
            string[] msg = new string[24];

            if (message.Length > 2)
            {
                msg = message.Split(' ');
                for (int i = 1; i < msg.Length; i++)
                {
                    if (msg[0].Length + msg[i].Length <= 26)
                    {
                        msg[0] = String.Concat(msg[0], " ", msg[i]);
                        msg[1] = "";
                    }
                    else if (msg[1].Length + msg[i].Length <= 26)
                    {
                        msg[1] += " " + msg[i];
                    }
                }
                if (msg[1] == null) { msg[1] = " "; }
            } else
            {
                msg = new string[] { " " , " " };
            }
            
            return msg;
        }

        public bool Update()
        {
            if (OpponentMonster.HP <= 0 || PlayerMonster.HP <= 0) 
            {
                return true;
            }

            return false;
        }

        public string GetMoves()
        {
            string moves = "Moves: ";
            foreach (Move m in PlayerMonster.Moves)
            {
              moves = String.Concat(moves, m.Name + " ");
            }
            return moves.TrimEnd();
        }

        public void PlayerMove()
        {
            Move selectedMove = default;

            // Take action
            int action = 0;
            while (action == 0)
            {
                action = BattleMenu();
                if (action == 2)
                {
                    ShowStats(PlayerMonster);
                    action = 0;
                }

                if (action == 1)
                {
                    DisplayFull(GetMoves());
                    selectedMove = MoveSelect();
                    if (selectedMove == null) { action = 0; }
                }
            }

            Move opponentSelectedMove = OpponentMoveSelect();
            GetInitiative();
            if (PlayerInitiative)
            {
                if (r.Next(0,1) < selectedMove.Accuracy)
                {
                    if (selectedMove.IsStatus)
                    {
                        StatusCalc(PlayerMonster, OpponentMonster, selectedMove);
                    }
                    else
                    {
                        DamageCalc(PlayerMonster, OpponentMonster, selectedMove);
                    }
                }
                else
                {
                    DisplayFull(PlayerMonster.Name + "'s attack missed!");
                    Console.ReadKey();
                }

                if (r.Next(0, 1) < opponentSelectedMove.Accuracy)
                {
                    if (OpponentMonster.HP > 0)
                    {
                        if (opponentSelectedMove.IsStatus)
                        {
                            StatusCalc(OpponentMonster, PlayerMonster, opponentSelectedMove);
                        }
                        else
                        {
                            DamageCalc(OpponentMonster, PlayerMonster, opponentSelectedMove);
                        }
                    }
                }
                else
                {
                    DisplayFull(OpponentMonster.Name + "'s attack missed!");
                    Console.ReadKey();
                }

            }

            else
            {
                if (r.Next(0, 1) < opponentSelectedMove.Accuracy)
                {
                    if (OpponentMonster.HP > 0)
                    {
                        if (opponentSelectedMove.IsStatus)
                        {
                            StatusCalc(OpponentMonster, PlayerMonster, opponentSelectedMove);
                        }
                        else
                        {
                            DamageCalc(OpponentMonster, PlayerMonster, opponentSelectedMove);
                        }
                    }
                }
                else
                {
                    DisplayFull(OpponentMonster.Name + "'s attack missed!");
                    Console.ReadKey();
                }

                if (PlayerMonster.HP > 0)
                {
                    if (r.Next(0, 1) < selectedMove.Accuracy)
                    {
                        if (selectedMove.IsStatus)
                        {
                            StatusCalc(PlayerMonster, OpponentMonster, selectedMove);
                        }
                        else
                        {
                            DamageCalc(PlayerMonster, OpponentMonster, selectedMove);
                        }
                    }
                    else
                    {
                        DisplayFull(PlayerMonster.Name + "'s attack missed!");
                        Console.ReadKey();
                    }
                }
            }
        }

        public void StatusCalc(Monster attacker, Monster defender, Move m)
        {
            DisplayFull(attacker.Name + " used a STATUS MOVE LOL.");
            Console.ReadKey();
        }

        public void DamageCalc(Monster attacker, Monster defender, Move m)
        {
            DisplayFull(attacker.Name + " used " + m.Name.ToUpper() );
            Console.ReadKey();

            int atkStatIndex;
            int defStatIndex;

            if (m.IsPhysical)
            {
                atkStatIndex = 1;
                defStatIndex = 2;
            } else
            {
                atkStatIndex = defStatIndex = 3;
            }

            byte critThreshold = (byte)(attacker.Stats[4] / 2);
            bool criticalHit = critThreshold > r.Next(0, 255);
            float critMult = 1;
            if (criticalHit) { critMult = (2 * attacker.LV + 5) / (attacker.LV + 5); }

            int damage = (int)Math.Round((((2f*attacker.LV/5 + 2f) * m.Power * 
                attacker.Stats[atkStatIndex] / defender.Stats[defStatIndex])/50f + 2f)*critMult);

            defender.HP -= damage;

            if (criticalHit)
            {
                DisplayFull("A critical hit!");
                Console.ReadKey();
            }
        }

        public void GetInitiative()
        {
            if (PlayerMonster.Stats[4] <= OpponentMonster.Stats[4])
            {
                PlayerInitiative = true;
            } else
            {
                PlayerInitiative = false;
            }
        }

        public Move MoveSelect()
        {
            switch (Console.ReadLine())
            {
                case "1":
                    return PlayerMonster.Moves[0];
                case "2":
                    return PlayerMonster.Moves[1];
                default:
                    return null;
            }
        }

        public Move OpponentMoveSelect()
        {
            
            if (r.Next() % 2 == 0)
            {
                return OpponentMonster.Moves[0];
            }
            return OpponentMonster.Moves[1];
        } 
        public int BattleMenu()
        {
            DisplayFull("1. FIGHT! | 2. STATS");
            
            switch (Console.ReadLine())
            {
                case "1":
                    return 1;
                case "2":
                    return 2;
                default:
                    return 0;
            }
        }

        public void ShowStats(Monster x)
        {
            Console.Clear();
            Console.WriteLine("{0}\nLV {1}", x.Name, x.LV +
                "\nHP, Atk, Def, Spc, Spd");
            Console.Write("HP: {0}/{1}, ", PlayerMonster.HP, PlayerMonster.Stats[0]);
            for (int i = 1; i < 5; i++)
            {
                Console.Write(x.StatsDisplay[i] + " ");
            }
            Console.WriteLine("\nCurrent XP: {0}/{1}. To next LV: {2}", x.XP, x.XPToNextLV, (x.XPToNextLV - x.XP));
            Console.WriteLine(GetMoves());
            Console.ReadKey(true);
        }

        public void DisplayNewWindow(string message)
        {
            var msg = FormatMessage(message);

            Console.Clear();
            Console.WriteLine(" {0,20}", "\\ /");
            Console.WriteLine(" {0,20}", "O O");
            Console.WriteLine(" {0,20}", " < ");
            Console.WriteLine(" {0,20}", "REEEEEEEE      A ");
            Console.WriteLine(" {0,23}", "F====[ [  . ] ]");
            Console.WriteLine(" {0,23}", "[ [  . ] ]");
            Console.WriteLine(" {0,23}", "[ [  . ] ]");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("o____________________________o");
            Console.WriteLine("| {0,-26} |", msg[0].Trim());
            Console.WriteLine("| {0,-26} |", msg[1].Trim());
            Console.WriteLine("o____________________________o");
        }

        public void DisplayOnlyPlayer(string message)
        {
            var msg = FormatMessage(message);

            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("{0,28}", PlayerMonster.Name);
            Console.WriteLine("{0,28}", ":L" + PlayerMonster.LV);
            Console.WriteLine("{0,30}", "HP: " + HPVisualizer(PlayerMonster.HP, (int)PlayerMonster.Stats[0]));
            Console.WriteLine("{0,27}{1}\n", PlayerMonster.HP + "/", PlayerMonster.Stats[0]); ;
            Console.WriteLine(" ");
            Console.WriteLine("o____________________________o");
            Console.WriteLine("| {0,-26} |", msg[0].Trim());
            Console.WriteLine("| {0,-26} |", msg[1].Trim());
            Console.WriteLine("o____________________________o");
        }
        public void DisplayOnlyOpponent(string message)
        {
            var msg = FormatMessage(message);

            Console.Clear();
            Console.WriteLine(" {0,-10}", OpponentMonster.Name);
            Console.WriteLine("     :L{0,-15}", OpponentMonster.LV);
            Console.WriteLine("  HP: {0,-15}", HPVisualizer(OpponentMonster.HP, (int)OpponentMonster.Stats[0]));
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("o____________________________o");
            Console.WriteLine("| {0,-26} |", msg[0].Trim());
            Console.WriteLine("| {0,-26} |", msg[1].Trim());
            Console.WriteLine("o____________________________o");
        }
        public void DisplayFull(string message)
        {
            var msg = FormatMessage(message);

            Console.Clear();
            Console.WriteLine(" {0,-10}", OpponentMonster.Name);
            Console.WriteLine("     :L{0,-15}", OpponentMonster.LV);
            Console.WriteLine("  HP: {0,-15}", HPVisualizer(OpponentMonster.HP, (int)OpponentMonster.Stats[0]));
            Console.WriteLine(" ");
            Console.WriteLine("{0,28}", PlayerMonster.Name);
            Console.WriteLine("{0,28}", ":L" + PlayerMonster.LV);
            Console.WriteLine("{0,30}", "HP: " + HPVisualizer(PlayerMonster.HP, (int)PlayerMonster.Stats[0]));
            Console.WriteLine("{0,27}{1}", PlayerMonster.HP + "/", PlayerMonster.Stats[0]); ;
            Console.WriteLine(" ");
            Console.WriteLine("o____________________________o");
            Console.WriteLine("| {0,-26} |", msg[0].Trim());
            Console.WriteLine("| {0,-26} |", msg[1].Trim());
            Console.WriteLine("o____________________________o");

        }
    }
}

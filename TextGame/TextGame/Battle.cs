using System;
using System.Collections.Generic;
using System.Text;

namespace TextGame
{
    public class Battle
    {
        readonly Character Player;
        readonly Character Opponent;
        Monster PlayerMonster;
        Monster OpponentMonster;
        public bool PlayerInitiative;
        public BattleStat PlayerMonsterStatValue = new BattleStat();
        public BattleStat OpponentMonsterStatValue = new BattleStat();
        public Random r = new Random();
        public bool PlayerWon = false;
        public Move SwitchMonster;
        public int damage;
        public bool superEffective = false;
        public bool notVeryEffective = false;
        public bool notEffective = false;
        readonly float[] multTable =
                {
                    .25f, .285f, .33f, .4f, .5f, .66f,
                    1, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f
                };


        public Battle(Character player1, Character player2)
        {
            Player = player1;
            Opponent = player2;
            PlayerMonster = Player.Party[0];
            OpponentMonster = Opponent.Party[0];
            InitMonster(true);
            InitMonster(false);
        }

        public void Go()
        {
            bool battleEnded = false;

            DisplayNewWindow(Opponent.Description + " " + Opponent.Name + " wants to fight!");
            Console.ReadKey();
            DisplayNewWindow(Opponent.Name + " sent out " + OpponentMonster.Nickname + "!");
            Console.ReadKey();
            DisplayOnlyOpponent("");
            Console.ReadKey();
            DisplayOnlyOpponent("Go! " + PlayerMonster.Nickname + "!");
            Console.ReadKey();

            while (!battleEnded)
            {
                PlayerMove();
                if (AMonsterFainted())
                {
                    SetNewMonster();
                }
            }

            void SetNewMonster()
            {
                if (OpponentMonster.HP <= 0)
                {
                    // if opponent still has monsters, continue - else, end battle
                    DisplayFull(Opponent.Name + "'s " + OpponentMonster.Nickname + " fainted!");
                    OpponentMonster.Status = "FNT";
                    Opponent.UsableMonsters = Character.GetUsableMonsters(Opponent);
                    Console.ReadKey();

                    if (Opponent.UsableMonsters > 0)
                    {
                        for (int i = 0; i < Opponent.Party.Length; i++)
                        {
                            if (Opponent.Party[i].Status != "FNT")
                            {
                                OpponentMonster = Opponent.Party[i];
                                break;
                            }
                        }
                        DisplayOnlyPlayer("");
                        Console.ReadKey();
                        DisplayOnlyPlayer(Opponent.Name + " sent out " + OpponentMonster.Nickname + "!");
                        InitMonster(false);
                        Console.ReadKey();

                    }
                    else
                    {
                        battleEnded = true;
                        PlayerWon = true;
                        DisplayOnlyPlayer(Player.Name + " defeated " + Opponent.Name);
                        Console.ReadKey();
                        DisplayOnlyPlayer(Player.Name + " got $" + Opponent.Reward);
                        Console.ReadKey();
                    }

                }
                else if (PlayerMonster.HP <= 0)
                {
                    // if player still has monsters, continue - else, end battle
                    DisplayFull(Player.Name + "'s " + PlayerMonster.Nickname + " FAINTED!");
                    PlayerMonster.Status = "FNT";
                    Player.UsableMonsters = Character.GetUsableMonsters(Player);
                    Console.ReadKey();

                    if (Player.UsableMonsters > 0)
                    {
                        string selection = "";
                        int s = 0;
                        while (selection == "")
                        {
                            DisplayParty();
                            DisplayTextBox("Choose a MONSTER.");
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    s = 0;
                                    break;
                                case "2":
                                    s = 1;
                                    break;
                                default:
                                    selection = "";
                                    break;
                            }
                            if (Player.Party[s].Status == "FNT")
                            {
                                DisplayParty();
                                DisplayTextBox(Player.Party[s].Nickname + " can't fight!");
                                Console.ReadKey();
                                selection = "";
                            } else
                            {
                                selection = Player.Party[s].Nickname;
                            }
                        }

                        PlayerMonster = Player.Party[s];
                        DisplayOnlyOpponent("");
                        Console.ReadKey();
                        DisplayOnlyOpponent("Go! " + PlayerMonster.Nickname + "!");
                        InitMonster(true);
                        Console.ReadKey();
                    }


                    else
                    {
                        battleEnded = true;
                        PlayerWon = false;
                        DisplayOnlyOpponent(Player.Name + " is out of usable MONSTERS");
                        Console.ReadKey();
                        DisplayOnlyOpponent(Player.Name + " blacked out!");
                        Console.ReadKey();
                    }
                }
            }
        }

        public void InitMonster(bool isPlayer)
        {
            if (isPlayer)
            {
                PlayerMonsterStatValue.Atk = PlayerMonster.Stats[1];
                PlayerMonsterStatValue.Def = PlayerMonster.Stats[2];
                PlayerMonsterStatValue.Spec = PlayerMonster.Stats[3];
                PlayerMonsterStatValue.Spd = PlayerMonster.Stats[4];
                PlayerMonsterStatValue.Stages = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                PlayerMonster.HP = Convert.ToUInt16(PlayerMonster.HP);
            }
            else
            {
                OpponentMonsterStatValue.Atk = OpponentMonster.Stats[1];
                OpponentMonsterStatValue.Def = OpponentMonster.Stats[2];
                OpponentMonsterStatValue.Spec = OpponentMonster.Stats[3];
                OpponentMonsterStatValue.Spd = OpponentMonster.Stats[4];
                OpponentMonsterStatValue.Stages = new int[] { 0, 0, 0, 0, 0, 0, 0 };
                OpponentMonster.HP = Convert.ToUInt16(OpponentMonster.HP);
            }
        }

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

        public string PartyVisualizer(Character c)
        {
            string output = "";

            if (c != Player)
            {
                for (int i = 5; i >= 0; i--)
                {
                    Logic(i);
                }
            } else
            {
                for (int i = 0; i <= 5; i++)
                {
                    Logic(i);
                }
            }
            

            return output;

            void Logic(int i)
            {
                if (c.Party[i].Name == "")
                {
                    output = String.Concat(output, "o");
                }
                else if (c.Party[i].Status == "FNT")
                {
                    output = String.Concat(output, "x");
                }
                else if (c.Party[i].Status == "OK")
                {
                    output = String.Concat(output, "O");
                }
                else
                {
                    output = String.Concat(output, "0");
                }

                if (Opponent.Name == "Wild ") { output = " "; }
            }
        }

        public string[] FormatMessage(string message)
        {
            string[] msg;

            if (message.Length > 26)
            {
                bool firstLineDone = false;

                msg = message.Split(' ');
                for (int i = 1; i < msg.Length; i++)
                {
                    if (!firstLineDone && msg[0].Length + msg[i].Length < 26)
                    {
                        msg[0] = String.Concat(msg[0], " ", msg[i]);
                        msg[1] = "";
                    } else 
                    {
                        firstLineDone = true;
                    }

                    if (msg[1] == "")
                    { 
                        msg[1] = " "; 
                    } else if (msg[1].Length + msg[i].Length < 26)
                    {
                        msg[1] = String.Concat(msg[1], " ", msg[i]);
                    } 
                }
            } else
            {
                msg = new string[] { message , " " };
            }
            
            return msg;
        }

        public bool AMonsterFainted()
        {
            if (OpponentMonster.HP <= 0 || PlayerMonster.HP <= 0) 
            {
                if (OpponentMonster.HP < 0)
                {
                    OpponentMonster.HP = 0;
                }
                if (PlayerMonster.HP < 0)
                {
                    PlayerMonster.HP = 0;
                }
                return true;
            }

            return false;
        }

        public void PlayerMove()
        {
            Move selectedMove = default;

            // Take action
            int action = 0;
            while (action == 0)
            {
                action = BattleMenu();
                switch (action)
                {
                    case 1:
                        DisplayMoveMenu();
                        selectedMove = MoveSelect();
                        if (selectedMove == null)
                        {
                            action = 0;
                        }
                        break;
                    case 2:
                        if (ShowParty())
                        {
                            selectedMove = SwitchMonster;
                            DisplayFull(" ");
                            Console.ReadKey();
                            break;
                        }
                        action = 0;
                        break;
                    case 3:
                        DisplayFull("You have no items; you are BROKE");
                        Console.ReadKey();
                        action = 0;
                        break;
                    case 4:
                        DisplayFull("You would rather DIE than run from this");
                        Console.ReadKey();
                        action = 0;
                        break;
                    default:
                        action = 0;
                        break;
                }

            }
            Move opponentSelectedMove = OpponentMoveSelect();
            GetInitiative();
            BeforeTurnEffects();

            if (selectedMove == SwitchMonster)
            {
                ExecuteTurn(OpponentMonster, opponentSelectedMove);

            } else
            {
                selectedMove.PP--;
                if (PlayerInitiative)
                {
                    ExecuteTurn(PlayerMonster, selectedMove);
                    ExecuteTurn(OpponentMonster, opponentSelectedMove);
                } else
                {
                    ExecuteTurn(OpponentMonster, opponentSelectedMove);
                    ExecuteTurn(PlayerMonster, selectedMove);
                }
            }
            EndOfTurnEffects();
        }

        public void BeforeTurnEffects()
        {
            
            switch (PlayerMonster.Status)
            {
                case "OK":
                    break;
                case "PAR":
                    Paralyzed(PlayerMonster);
                    break;
            }
            switch (OpponentMonster.Status)
            {
                case "OK":
                    break;
                case "PAR":
                    Paralyzed(OpponentMonster);
                    break;
            }

            

            void Paralyzed(Monster m)
            {
                if (m == PlayerMonster)
                {
                    PlayerMonsterStatValue.Spd = (int)Math.Round(PlayerMonster.Stats[4] * 
                        multTable[PlayerMonsterStatValue.Stages[4]] * .25f);
                }
            }
        }

        public void EndOfTurnEffects()
        {
            if (!AMonsterFainted())
            {
                switch (PlayerMonster.Status)
                {
                    case "OK":
                        break;
                    case "BRN":
                        Burn(PlayerMonster);
                        break;
                }
                switch (OpponentMonster.Status)
                {
                    case "OK":
                        break;
                    case "BRN":
                        Burn(OpponentMonster);
                        break;
                }
            }

            void Burn(Monster m)
            {
                m.HP -= (int)Math.Round(m.StatsDisplay[0] * (1 / 16f));
                if (!AMonsterFainted())
                {
                    DisplayFull("");
                    Console.ReadKey();
                    DisplayFull(m.Nickname + " took damage from its BURN!");
                    Console.ReadKey();
                }
            }
        }

        void ExecuteTurn(Monster u, Move m)
        {
            BattleStat s = OpponentMonsterStatValue;
            Monster o = PlayerMonster;
            if (u == PlayerMonster)
            {
                s = PlayerMonsterStatValue;
                o = OpponentMonster;
            }

            if (u.HP > 0)
            {
                if (r.NextDouble() < m.Accuracy * s.Acc)
                {
                    if (m.IsStatus)
                    {
                        StatusCalc(u, m);
                    }
                    else
                    {
                        DamageCalc(u, o, m);
                    }
                    SpecialEffects(u, o, m, damage);
                }
                else
                {
                    DisplayFull(u.Nickname + " used " + m.Name + "!");
                    Console.ReadKey();
                    DisplayFull(u.Nickname + "'s attack missed!");
                    Console.ReadKey();
                }
            }
            AMonsterFainted();
        }

        public void SpecialEffects(Monster x, Monster o, Move m, int d)
        {
            string message = " ";

            if (m.NSEffect != null)
            {
                switch (m.Name)
                {
                    case "ABSORB":
                        x.HP = NonStatsEffect.Absorb(d, x.HP);
                        if (x.HP > x.StatsDisplay[0])
                        {
                            x.HP = x.StatsDisplay[0];
                        }
                        message = x.Nickname + " absorbed health from " + o.Nickname +  "!" ;
                        break;
                    case "EMBER":
                        if (o.Status == "OK" && o.HP > 0)
                        {
                            o.Status = NonStatsEffect.Burn(.1f);
                            if (o.Status == "BRN")
                            {
                                message = o.Nickname + " was BURNED!";
                            }
                        }
                        break;
                    default:
                        break;
                }

                if (x.Nickname == PlayerMonster.Nickname)
                {
                    PlayerMonster = x;
                    OpponentMonster = o;
                } else
                {
                    OpponentMonster = x;
                    PlayerMonster = o;
                }
                if (message != " ")
                {
                    DisplayFull(message);
                    Console.ReadKey();
                }
            }
        }

        public bool ShowParty()
        {
            bool done = false;
            while (!done)
            {
                DisplayParty();
                DisplayTextBox("Choose a MONSTER.");
                int s = -1;
                string selection;
                switch (Console.ReadLine())
                {
                    case "1":
                        s = 0;
                        break;
                    case "2":
                        s = 1;
                        break;
                    default:
                        done = true;
                        break;
                }
                if (done)
                { break; }
                DisplayParty();
                DisplayTextBoxLong(new string[] {
                "Do what with " + Player.Party[s].Nickname + "?", 
                "1.SHIFT", "2.STATS"
                    } );
                selection = Console.ReadLine();
                if (selection == "1")
                {
                    if (SwapMonster(s))
                    {
                        return true;
                    }
                }
                else if (selection == "2")
                {
                    ShowStats(Player.Party[s]);
                }
            }
            return false;

            bool SwapMonster(int x)
            {
                if (PlayerMonster == Player.Party[x])
                {
                    DisplayParty();
                    DisplayTextBox(Player.Party[x].Nickname + " is already in battle!");
                    Console.ReadKey();
                    return false;
                } else if (Player.Party[x].Status == "FNT")
                {
                    DisplayParty();
                    DisplayTextBox(Player.Party[x].Nickname + " can't fight!");
                    Console.ReadKey();
                    return false;
                }
                else
                {
                    DisplayFull(PlayerMonster.Nickname + ", come back!");
                    Console.ReadKey();
                    PlayerMonster = Player.Party[x];
                    InitMonster(true);
                    DisplayOnlyOpponent("Go! " + PlayerMonster.Nickname + "!");
                    Console.ReadKey();
                    return true;
                }
            }

        }

        public void DisplayParty()
        {
            Console.Clear();
            foreach (Monster m in Player.Party)
            {
                if (m.Nickname != "MISSINGNO")
                {
                    Console.WriteLine("{0,-15}   :L{1}",
                        m.Nickname, m.LV);
                    Console.WriteLine("  HP: {2} {0,3}/{1,3}", m.HP, m.StatsDisplay[0], HPVisualizer(m.HP, m.StatsDisplay[0]));
                }
            }
        }

        public void StatusCalc(Monster attacker, Move m)
        {
            DisplayFull(attacker.Nickname + " used " + m.Name + "!");
            Console.ReadKey();
            m.PP--;

            if (attacker == PlayerMonster)
            {
                DoEffect(true, m, m.Effect);
            } else
            {
                DoEffect(false, m, m.Effect);
            }
        }

        public void DoEffect(bool playerAttacking, Move m, Effect e)
        {
            BattleStat targetStats = new BattleStat();
            Monster targetMonster;

            float[] multTable =
                {
                    .25f, .285f, .33f, .4f, .5f, .66f,
                    1, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f
                };
            string roseOrFell = "";
            switch (e.StagesModified) 
            {
                case 1:
                    roseOrFell = " rose!";
                    break;
                case 2:
                    roseOrFell = " greatly rose!";
                    break;
                case -1:
                    roseOrFell = " fell!";
                    break;
                case -2:
                    roseOrFell = " greatly fell!";
                    break;
                default:
                    break;
            }

            if (playerAttacking)
            {
                if (m.TargetSelf)
                {
                    targetStats = PlayerMonsterStatValue;
                    targetMonster = PlayerMonster;
                    SetStat();
                    PlayerMonsterStatValue = targetStats;
                } else
                {
                    targetStats = OpponentMonsterStatValue;
                    targetMonster = OpponentMonster;
                    SetStat();
                    OpponentMonsterStatValue = targetStats;
                }
            } else
            {
                if (m.TargetSelf)
                {
                    targetStats = OpponentMonsterStatValue;
                    targetMonster = OpponentMonster;
                    SetStat();
                    OpponentMonsterStatValue = targetStats;
                } else
                {
                    targetStats = PlayerMonsterStatValue;
                    targetMonster = PlayerMonster;
                    SetStat();
                    PlayerMonsterStatValue = targetStats;
                }
            }

            void SetStat()
            {
                if (targetStats.Stages[e.StatModifiedInt] + e.StagesModified > -7 &&
                    targetStats.Stages[e.StatModifiedInt] + e.StagesModified < 7) 
                {
                    targetStats.Stages[e.StatModifiedInt] += e.StagesModified;
                    switch (e.StatModifiedInt)
                    {
                        case 1:
                            targetStats.Atk = multTable[targetStats.Stages[e.StatModifiedInt] + 6] * targetMonster.Stats[e.StatModifiedInt];
                            break;
                        case 2:
                            targetStats.Def = multTable[targetStats.Stages[e.StatModifiedInt] + 6] * targetMonster.Stats[e.StatModifiedInt];
                            break;
                        case 3:
                            targetStats.Spec = multTable[(targetStats.Stages[e.StatModifiedInt] + 6)] * targetMonster.Stats[e.StatModifiedInt];
                            break;
                        case 4:
                            targetStats.Spd = multTable[targetStats.Stages[e.StatModifiedInt] + 6] * targetMonster.Stats[e.StatModifiedInt];
                            break;
                        case 5:
                            // targetStats.Acc;
                            break;
                        case 6:
                            // targetStats.Evas;
                            break;
                        default:
                            break;
                    }
                }

                if (targetStats.Stages[e.StatModifiedInt] + e.StagesModified > 6)
                {
                    DisplayFull(targetMonster.Nickname + "'s " + e.StatModified + " won't go any higher!");
                    Console.ReadKey();
                }
                else if (targetStats.Stages[e.StatModifiedInt] + e.StagesModified < -6)
                {
                    DisplayFull(targetMonster.Nickname + "'s " + e.StatModified + " won't go any lower!");
                    Console.ReadKey();
                }
                else
                {
                    DisplayFull(targetMonster.Nickname + "'s " + e.StatModified + roseOrFell);
                    Console.ReadKey();
                }
            }
        }


        public void DamageCalc(Monster attacker, Monster defender, Move m)
        {
            DisplayFull(attacker.Nickname + " used " + m.Name + "!");
            Console.ReadKey();
            float atkStat;
            float defStat;
            superEffective = false;
            notVeryEffective = false;
            notEffective = false;

        // crit calc
        double chanceMult = r.Next(85, 100) / 100f;
            byte critThreshold = (byte)(attacker.BaseStats[4] / 2);
            bool criticalHit = critThreshold > r.Next(0, 255);
            float critMult = 1;
            if (criticalHit) { critMult = (2 * attacker.LV + 5) / (attacker.LV + 5); }

            if (attacker == PlayerMonster)
            {
                if (m.IsPhysical)
                {
                    atkStat = PlayerMonsterStatValue.Atk;
                    defStat = OpponentMonsterStatValue.Def;
                }
                else
                {
                    atkStat = PlayerMonsterStatValue.Spec;
                    defStat = OpponentMonsterStatValue.Spec;
                }
                
            } else
            {
                if (m.IsPhysical)
                {
                    atkStat = OpponentMonsterStatValue.Atk;
                    defStat = PlayerMonsterStatValue.Def;
                }
                else
                {
                    atkStat = OpponentMonsterStatValue.Spec;
                    defStat = PlayerMonsterStatValue.Spec;
                }
            }

            damage = (int)Math.Round(((2f * attacker.LV / 5f + 2f) * m.Power *
                atkStat / defStat / 50f + 2f) * critMult * chanceMult);
            defender.HP -= MultiplierCalc(attacker, defender, m);

            AMonsterFainted();
            if (criticalHit)
            {
                DisplayFull("A critical hit!");
                Console.ReadKey();
            }
            if (superEffective)
            {
                DisplayFull("It's super effective!");
                Console.ReadKey();
            }
            else if (notVeryEffective)
            {
                DisplayFull("It's not very effective...");
                Console.ReadKey();
            }
            else if (notEffective)
            {
                if (attacker == PlayerMonster)
                {
                    DisplayFull("It doesn't affect foe " + defender.Nickname + "...");
                    Console.ReadKey();
                } else
                {
                    DisplayFull("It doesn't affect " + defender.Nickname + "...");
                    Console.ReadKey();
                }
            }
        }

        public int MultiplierCalc(Monster u, Monster t, Move m)
        {
            
            float stab = 1f;
            if (u.Type1 == m.Type || u.Type2 == m.Type)
            {
                stab = 1.5f;
            }
            float type = 1f;
            type = TypeEffectiveness(m.Type, t.Type1);
            if (t.Type2 != null)
            {
                type *= TypeEffectiveness(m.Type, t.Type2);
            }
            if (type <= 0.1f) { notEffective = true; }
            else if (type < 0.9f) { notVeryEffective = true; }
            else if (type >= 1.9f) { superEffective = true; }

            return (int)Math.Round(damage * stab * type);
        }

        public float TypeEffectiveness(string moveType, string targetType)
        {
            //               n   f   w   e   g   i   f   p   g   f   p   b   r   g   d
            float[] norm = { 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1, .5f, 0,  1  };
            float[] fire = { 1, .5f,.5f, 1,  2,  2,  1,  1,  1,  1,  1,  2, .5f, 1, .5f };
            float[] wter = { 1,  2, .5f, 1, .5f, 1,  1,  1,  2,  1,  1,  1,  2,  1, .5f };
            float[] elec = { 1,  1,  2, .5f,.5f, 1,  1,  1,  0,  2,  1,  1,  1,  1, .5f };
            float[] gras = { 1, .5f, 2,  1, .5f, 1,  1, .5f, 2, .5f, 1, .5f, 2,  1, .5f };
            float[] _ice = { 1,  1, .5f, 1,  2, .5f, 1,  1,  2,  2,  1,  1,  1,  1,  2  };
            float[] fght = { 2,  1,  1,  1,  1,  2,  1, .5f, 1, .5f,.5f,.5f, 2,  0,  1  };
            float[] pois = { 1,  1,  1,  1,  2,  1,  1, .5f,.5f, 1,  1,  2, .5f,.5f, 1  };
            float[] grnd = { 1,  2,  1,  2, .5f, 1,  1,  2,  1,  0,  1, .5f, 2,  1,  1  };
            float[] _fly = { 1,  1,  1, .5f, 2,  1,  2,  1,  1,  1,  1,  2, .5f, 1,  1  };
            float[] psyc = { 1,  1,  1,  1,  1,  1,  2,  2,  1,  1, .5f, 1,  1,  1,  1  };
            float[] _bug = { 1, .5f, 1,  1,  2,  1, .5f, 2,  1,  1,  2,  1,  1, .5f, 1  };
            float[] rock = { 1,  2,  1,  1,  1,  2, .5f, 1, .5f, 2,  1,  2,  1,  1,  1  };
            float[] ghst = { 0,  1,  1,  1,  1,  1,  1,  1,  1,  1,  0,  1,  1,  2,  1  };
            float[] drag = { 1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  2  };
            float[][] typeChart = { norm, fire, wter, elec, gras, _ice, fght, pois, grnd, _fly, psyc, _bug, rock, ghst, drag };
            
            return typeChart[GetTypeIndex(moveType)][GetTypeIndex(targetType)];

            static int GetTypeIndex(string input)
            {
                string type = input;
                return type switch
                {
                    "NORMAL" => 0,
                    "FIRE" => 1,
                    "WATER" => 2,
                    "ELECTRIC" => 3,
                    "GRASS" => 4,
                    "ICE" => 5,
                    "FIGHTING" => 6,
                    "POISON" => 7,
                    "GROUND" => 8,
                    "FLYING" => 9,
                    "PSYCHIC" => 10,
                    "BUG" => 11,
                    "ROCK" => 12,
                    "GHOST" => 13,
                    "DRAGON" => 14,
                    _ => 0,
                };
            }
        }

        public void GetInitiative()
        {
            if (PlayerMonsterStatValue.Spd > OpponentMonsterStatValue.Spd)
            {
                PlayerInitiative = true;
            } else if (PlayerMonsterStatValue.Spd == OpponentMonsterStatValue.Spd)
            {
                if (r.Next(1,100) > 50)
                {
                    PlayerInitiative = true;
                } else
                {
                    PlayerInitiative = false;
                }
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
                case "3":
                    return PlayerMonster.Moves[2];
                case "4":
                    return PlayerMonster.Moves[3];
                default:
                    return null;
            }
        }

        public Move OpponentMoveSelect()
        {
            List<Move> possibleMoves = new List<Move>();
            possibleMoves.Clear();

            foreach (Move m in OpponentMonster.Moves)
            {
                if (PlayerMonster.Type2 == null)
                {
                    if (TypeEffectiveness(m.Type, PlayerMonster.Type1) > 1.9f)
                    {
                        possibleMoves.Add(m);
                    }
                } 
                else if (TypeEffectiveness(m.Type, PlayerMonster.Type1) *
                    TypeEffectiveness(m.Type, PlayerMonster.Type2) > 1.9f)
                {
                    possibleMoves.Add(m);
                }
            }

            if (possibleMoves.Count != 0)
            {
                return possibleMoves[r.Next(0, possibleMoves.Count)];
            }
            else {
                Move trythismove = OpponentMonster.Moves[r.Next(0, OpponentMonster.Moves.Length)];
                while (trythismove.Name == "--------")
                {
                    trythismove = OpponentMonster.Moves[r.Next(0, OpponentMonster.Moves.Length)];
                }
                return trythismove;
            }
        } 
        public int BattleMenu()
        {
            DisplayFull("1.FIGHT    2.PKMN            3.ITEM     4.RUN");

            switch (Console.ReadLine())
            {
                case "1":
                    return 1;
                case "2":
                    return 2;
                case "3":
                    return 3;
                case "4":
                    return 4;
                default:
                    return 0;
            }
        }

        public void ShowStats(Monster x)
        {
            Console.Clear();
            Console.WriteLine("{0,-15}LV {1}", (x.Nickname), x.LV);
            Console.WriteLine(x.Name);
            Console.WriteLine("{0} {1}", x.Type1, x.Type2);
            Console.WriteLine("HP: {0,5}/{1}", PlayerMonster.HP, Math.Round((decimal)PlayerMonster.StatsDisplay[0]));
            Console.WriteLine("Attack: {0,4}", x.StatsDisplay[1]);
            Console.WriteLine("Defend: {0,4}", x.StatsDisplay[2]);
            Console.WriteLine("Special: {0,3}", x.StatsDisplay[3]);
            Console.WriteLine("Speed: {0,5}", x.StatsDisplay[4]);
            Console.WriteLine("\nCurrent XP: {0}/{1}. To next LV: {2}\n", x.XP, x.XPToNextLV, (x.XPToNextLV - x.XP));
            Console.WriteLine("{0,-1}{1,13}\n{2,-1}{3,13}",
                x.Moves[0].Name, x.Moves[1].Name, x.Moves[2].Name, x.Moves[3].Name);
            Console.ReadKey();
        }
        public void DisplayTextBoxLong(string[] msg)
        {

            Console.WriteLine("o____________________________o");
            Console.WriteLine("| {0,-26} |", msg[0].Trim());
            Console.WriteLine("| {0,-26} |", " ");
            Console.WriteLine("o____________________________o");

            Console.WriteLine("");
            Console.WriteLine("o_________________o");
            Console.WriteLine("| {0,-15} |", msg[1].Trim());
            Console.WriteLine("| {0,-15} |", msg[2].Trim());
            if (msg.Length > 3)
            {
                Console.WriteLine("| {0,-15} |", msg[3].Trim());
            }
            if (msg.Length > 4)
            {
                Console.WriteLine("| {0,-15} |", msg[4].Trim());
            }
            Console.WriteLine("o_________________o"); ;
        }

        public void DisplayTextBox(string message)
        {
            var msg = FormatMessage(message);
            Console.WriteLine("o____________________________o");
            Console.WriteLine("| {0,-26} |", msg[0].Trim());
            Console.WriteLine("| {0,-26} |", msg[1].Trim());
            Console.WriteLine("o____________________________o");
        }

        public void DisplayNewWindow(string message)
        {
            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("   {0,-15}", PartyVisualizer(Opponent));
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" {0,27}", PartyVisualizer(Player));
            Console.WriteLine(" ");
            DisplayTextBox(message);
        }

        public void DisplayOnlyPlayer(string message)
        {
            string pStatus = DisplayStatusOrLevel(PlayerMonster);
            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" {0,-15}", PartyVisualizer(Opponent));
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("{0,28}", PlayerMonster.Nickname);
            Console.WriteLine("{0,28}", pStatus);
            Console.WriteLine("{0,30}", "HP: " + HPVisualizer(PlayerMonster.HP, (int)PlayerMonster.StatsDisplay[0]));
            Console.WriteLine("{0,27}{1}", PlayerMonster.HP + "/", Convert.ToUInt16(PlayerMonster.StatsDisplay[0]));
            Console.WriteLine(" ");
            DisplayTextBox(message);
        }
        public void DisplayOnlyOpponent(string message)
        {
            string oStatus = DisplayStatusOrLevel(OpponentMonster);
            Console.Clear();
            Console.WriteLine(" {0,-10}", OpponentMonster.Nickname);
            Console.WriteLine("     {0,-15}", oStatus);
            Console.WriteLine("  HP: {0,-15}", HPVisualizer(OpponentMonster.HP, OpponentMonster.StatsDisplay[0]));
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine(" {0,26}", PartyVisualizer(Player));
            Console.WriteLine(" ");
            DisplayTextBox(message);
        }
        public void DisplayFull(string message)
        {
            string pStatus = DisplayStatusOrLevel(PlayerMonster);
            string oStatus = DisplayStatusOrLevel(OpponentMonster);
            Console.Clear();
            Console.WriteLine(" {0,-10}", OpponentMonster.Nickname);
            Console.WriteLine("     {0,-15}", oStatus);
            Console.WriteLine("  HP: {0,-15}", HPVisualizer(OpponentMonster.HP, (int)OpponentMonster.StatsDisplay[0]));
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("{0,28}", PlayerMonster.Nickname);
            Console.WriteLine("{0,28}", pStatus);
            Console.WriteLine("{0,30}", "HP: " + HPVisualizer(PlayerMonster.HP, (int)PlayerMonster.StatsDisplay[0]));
            Console.WriteLine("{0,27}{1}", PlayerMonster.HP + "/", Convert.ToUInt16(PlayerMonster.StatsDisplay[0]));
            Console.WriteLine(" ");
            DisplayTextBox(message);
        }
        public void DisplayMoveMenu()
        {
            string pStatus = DisplayStatusOrLevel(PlayerMonster);
            string oStatus = DisplayStatusOrLevel(OpponentMonster);
            Console.Clear();
            Console.WriteLine(" {0,-10}", OpponentMonster.Nickname);
            Console.WriteLine("     {0,-15}", oStatus);
            Console.WriteLine("  HP: {0,-15}", HPVisualizer(OpponentMonster.HP, (int)OpponentMonster.StatsDisplay[0]));
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("{0,28}", PlayerMonster.Nickname);
            Console.WriteLine("{0,28}", pStatus);
            Console.WriteLine("{0,30}", "HP: " + HPVisualizer(PlayerMonster.HP, (int)PlayerMonster.StatsDisplay[0]));
            Console.WriteLine("{0,27}{1}", PlayerMonster.HP + "/", PlayerMonster.StatsDisplay[0]); ;
            Console.WriteLine(" ");
            Console.WriteLine("o____________________________o");
            Console.WriteLine("| {0,-13} {1,-12} |", PlayerMonster.Moves[0].Name, PlayerMonster.Moves[1].Name);
            Console.WriteLine("| {0,-13} {1,-12} |", PlayerMonster.Moves[2].Name, PlayerMonster.Moves[3].Name);
            Console.WriteLine("o____________________________o");
        }

        public string DisplayStatusOrLevel(Monster x)
        {
            if (x.Status == "OK")
            {
                return ":L" + x.LV.ToString();
            } else
            {
                return x.Status;
            }
        }
    }
}
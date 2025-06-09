using System;
using System.Linq;

namespace pokemontcg
{
    /*Name: Kai Andrews
     * Problem:We need to implement the pokemon trading card game in C#.
     * Solution/Pseudocode: We will use a list of set decks for the player to use, then cause the AI to pick a random deck too. 
     * We will pick a random number each turn that is the card from the deck the player draws, then remove it from the list.
     * We will use my very simple knowledge of machine logic to make a basic algorithm for the enemy to follow, doing things like playing cards, retreating,
     * attaching energy, and attacking. 
     * 
     * Access Log
     * Date:       Log:
     * 5/13        excited to get started! i have some plans and a general outline and I hope this does not turn out like wordle.
     * 5/15        made deck list and added three decks for player/computer to use, took me a while but wasnt hard
     *             i also added the part where the computer picks a deck from the ones left over
     * 5/16        working on making a deck to hold cards and to draw a card
     * 5/19        making draw card method
     * 5/20        finished draw card, working on making the hand and checking whether the draw card is valid and the hand contains a basic pokemon
                   also, i converted over to VS code because I can.
       5/22        worked on printing the screen
       5/26        working on the first step logic. it is seperate from the rest of the game loop because they are different. 
                   write screen logic and core game loop (turn) logic
       5/28        worked on select logic for player (selecting a bench, active, hand card)
       5/29        finished the player turn core loop, working on enemy core turn loop as well as some more hand and bench logic (confusing ugh)
                   working on evolving as well
       5/30        ironing out bugs, glitches, and exceptions
       6/2         working on bugs, adding points, and improving computer smartness
       6/4         evolve and energy attach are done, working on death mechanic
       6/5         check and reset logic, just flattening things out, i should be done pretty soon
       6/7         so it did end up like wordle, this is taking forever.. flattening bugs, specifically with enemy behavior
                   because it is on the fritz.
       6/8         fixing some last index bugs, (theres been a lot) i should be done today.
                   IM DONE YES OMG THIS TOOK SO LONG!! I'M FREE!!! im never doing a computer enemy again..

     */
    static internal class Program
    {
        static void check( //this method handles death and player win/loss
        ref Pokemon pactivepokemon, List<Pokemon> pbench, ref int ppoints, ref int epoints,
        ref Pokemon eactivepokemon, List<Pokemon> ebench, ref bool gamend, ref bool playerwins,
        List<Pokemon> hand, List<Pokemon> enemyhand, List<Pokemon> decklist, List<Pokemon> enemylist,
        int pcardsleft, int ecardsleft, bool decklocked, ref bool playerturn)
        { //player logic
            if (pactivepokemon.hp <= 0 && pbench.Count >0)
            {
                epoints++;
                if (pbench.Count > 0)
                {
                    while (pactivepokemon.hp <= 0)
                    {
                        Console.WriteLine("your pokemon has died. please select a pokemon from your bench.");
                        Console.Write("Bench: ");
                        for (int i = 0; i < 3; i++)
                        {
                            if (i < pbench.Count && pbench[i] != null)
                                Console.Write(pbench[i].name);
                            else
                                Console.Write("---");
                            if (i < 2) Console.Write(", ");
                        }
                        Console.WriteLine();
                        string input = Console.ReadLine();
                        if (input == "0" && pbench.Count >= 1)
                        {
                            pactivepokemon = pbench[0];
                            pbench.RemoveAt(0);
                        }
                        else if (input == "1" && pbench.Count >= 2)
                        {
                            pactivepokemon = pbench[1];
                            pbench.RemoveAt(1);
                        }
                        else if (input == "2" && pbench.Count >= 3)
                        {
                            pactivepokemon = pbench[2];
                            pbench.RemoveAt(2);
                        }
                    }
                }
                else
                {
                        pactivepokemon = null;
                        playerturn = false;
                        gamend = true;
                        playerwins = false;
                }
            }

            if (ppoints >= 3)
            {
                while (ppoints >= 3)
                {
                    Console.WriteLine("you win! Restart? Y/N");
                    string yn = Console.ReadLine();
                    if (yn == "Y" || yn == "y")
                    {
                        ResetGame(ref ppoints, ref epoints, ref pcardsleft, ref ecardsleft, ref decklocked, ref playerwins, ref gamend,
                            hand, enemyhand, pbench, ebench, decklist, enemylist, ref pactivepokemon, ref eactivepokemon);
                    }
                    else if (yn == "N" || yn == "N")
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("only put in Y or N.");
                    }
                }
            }
            //enemy logic
            if (eactivepokemon != null && eactivepokemon.hp <= 0 && ebench.Count > 0)
            {
                //active pokemon is dead,so we need to remove him and award points
                ppoints++;
                if (ebench.Count > 0)
                {
                    Pokemon mosthponbench = Mosthpfinder(ebench);
                    eactivepokemon = mosthponbench;
                    if (mosthponbench != null && ebench.Count > 0)
                    {
                        for (int i = ebench.Count - 1; i >= 0; i--)
                        {
                            if (mosthponbench == ebench[i])
                            {
                                ebench.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    eactivepokemon = null;
                    playerturn = true;
                    gamend = true;
                    playerwins = true;
                }
            }
            else if (eactivepokemon == null)
            {
                playerturn = true;
                gamend = true;
                playerwins = true;
            }
            if (epoints >= 3)
            {
                while (epoints >= 3)
                {
                    Console.WriteLine("you lose! Restart? Y/N");
                    string yn = Console.ReadLine();
                    if (yn == "Y" || yn == "y")
                    {
                        ResetGame(ref ppoints, ref epoints, ref pcardsleft, ref ecardsleft, ref decklocked, ref playerwins, ref gamend,
                            hand, enemyhand, pbench, ebench, decklist, enemylist, ref pactivepokemon, ref eactivepokemon);
                    }
                    else if (yn == "N" || yn == "N")
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("only put in Y or N.");
                    }
                }
            }
            if (eactivepokemon == null)
            {
                playerturn = true;
                gamend = true;
                playerwins = true;
            }
            else if (pactivepokemon == null)
            {
                playerturn = true;
                gamend = true;
                playerwins = false;
            }
        }
        static void ResetGame(ref int ppoints,ref int epoints,ref int pcardsleft,ref int ecardsleft,ref bool decklocked,ref bool playerwins,
        ref bool gamend,List<Pokemon> hand,List<Pokemon> enemyhand,List<Pokemon> pbench,List<Pokemon> ebench,List<Pokemon> decklist,
        List<Pokemon> enemylist,ref Pokemon pactivepokemon,ref Pokemon eactivepokemon) //we will use this to reset the game state (not sure whether i actually need this)
        {
            ppoints = 0;
            epoints = 0;
            pcardsleft = 20;
            ecardsleft = 20;
            decklocked = false;
            playerwins = false;
            gamend = false;
            hand.Clear();
            enemyhand.Clear();
            pbench.Clear();
            ebench.Clear();
            decklist.Clear();
            enemylist.Clear();
            pactivepokemon = null;
            eactivepokemon = null;

        }
        static Pokemon Mosthpfinder(List<Pokemon> ebench) //this finds the most hp on the enemy bench and returns it, to be used for enemy logic 
        {
            Pokemon maxhpmon = null;
            int maxhp = int.MinValue;
            for (int i = 0; i < ebench.Count; i++)
            {
                if (ebench[i].hp > maxhp)
                {
                    maxhp = ebench[i].hp;
                    maxhpmon = ebench[i];
                }
            }
            return maxhpmon;
        }
        static void removenulls(List<Pokemon> ehand, List<Pokemon>phand) //this ensures we don't draw nulls,
        //as draw card can return (which adds to hands) a null, which would break the computer.
        {
            for (int i = phand.Count - 1; i >= 0; i--)
            {
                if (phand[i] == null)
                {
                    phand.RemoveAt(i);
                }
            }
            for (int i = ehand.Count - 1; i >= 0; i--)
            {
                if (ehand[i] == null)
                {
                    ehand.RemoveAt(i);
                }
            }
            for (int i = ehand.Count - 1; i >= 0; i--)
            {
                if (ehand[i] == null)
                {
                    ehand.RemoveAt(i);
                }
            }
        }
        static void writescreen(List<Pokemon> enemyhand, int epoints, List<Pokemon> ebench, Pokemon eactivepokemon,
            Pokemon pactivepoemon, List<Pokemon> hand, List<Pokemon> pbench, int ppoints, int energyleft) 
            //this will write our board state, effectively this is the player's visible UI.
        {
            Console.Clear();
            Console.WriteLine("Enemy Hand: " + enemyhand.Count + "    Points:" + epoints);
            // enemy bench
            Console.Write("Enemy Bench: ");
            for (int i = 0; i < 3; i++)
            {
                if (i < ebench.Count && ebench[i] != null)
                    Console.Write(ebench[i].name);
                else
                    Console.Write("---");
                if (i < 2) Console.Write(", ");
            }
            Console.WriteLine();

            Console.WriteLine("Enemy Active: " + (eactivepokemon != null ? eactivepokemon.name : "---"));
            Console.WriteLine();

            Console.WriteLine("Your Active: " + (pactivepoemon != null ? pactivepoemon.name : "---"));

            // player bench
            Console.Write("Your Bench: ");
            for (int i = 0; i < 3; i++)
            {
                if (i < pbench.Count && pbench[i] != null)
                    Console.Write(pbench[i].name);
                else
                    Console.Write("---");
                if (i < 2) Console.Write(", ");
            }
            Console.WriteLine();
            Console.WriteLine("Your Hand: " + hand.Count + "    Points:" + ppoints + " energy to attach: " + energyleft);
            Console.WriteLine("Hand     Bench     Active     End Turn     Energy");
            Console.WriteLine("1        2         3          4            5");
        }
        static void DoAttack(Pokemon attackingpokemon, Pokemon enemyactive) // this handles attacking 
        {
            enemyactive.hp -= attackingpokemon.atkdata.damage;
            if (attackingpokemon.type == "grass" && enemyactive.type == "water")
            {
                enemyactive.hp -= 20;
            }
            if (attackingpokemon.type == "water" && enemyactive.type == "fire")
            {
                enemyactive.hp -= 20;
            }
            if (attackingpokemon.type == "fire" && enemyactive.type == "grass")
            {
                enemyactive.hp -= 20;
            }
            if (attackingpokemon.atkdata.effect == "if the active enemy is an EX, this does 70 more damage")
            {
                if (enemyactive.name.Contains("EX"))
                {
                    enemyactive.hp -= 70;
                }
            }
            else if (attackingpokemon.atkdata.effect == "heal 10 damage")
            {
                attackingpokemon.hp += 10;
            }
        }
        private static Pokemon DrawCard(List<Pokemon> refdeck, int cardrawer, Random rng)
        //this method draws a card from the remaining cards in the deck
        {
            if (refdeck.Count > 0)
            {
                rng = new Random();
                cardrawer = rng.Next(0, refdeck.Count);
                Pokemon returnmon = refdeck[cardrawer];
                refdeck.RemoveAt(cardrawer);
                return returnmon;
            }
            else
            {
                return null; //just trust me on this part.
            }
        }
        static void Main(string[] args)
        {
            while (true)
            {
                /*name type hp retreatcost weakness stage energy attackdata base evolve 
                  attack name damage effect cost
                  pokemon pikachu = new pokemon("pikachu", "electric", 60, 1, "fighting", 0, jolt
                  jolt, 20, paralyze enemy 1); */
                Random rng = new Random();
                List<Pokemon> grassdeck = new List<Pokemon>();
                List<Pokemon> firedeck = new List<Pokemon>();
                List<Pokemon> waterdeck = new List<Pokemon>();
                string deckelemental = "";
                bool decklocked = false;
                bool playerwins = false;
                int pcardsleft = 20;
                int ecardsleft = 20;
                int deckchooser = rng.Next(3);
                int cardrawer = rng.Next(pcardsleft);
                int deckelement = -1;
                int enemydeck;
                int inputchecker = -1;
                List<Pokemon> decklist = new List<Pokemon>();
                List<Pokemon> enemylist = new List<Pokemon>();

                attackdata ram = new attackdata("ram", 20, "none", 1);
                attackdata blot = new attackdata("blot", 10, "heal 10 damage", 1);
                attackdata sharp_scythe = new attackdata("sharp scythe", 30, "none", 1);
                attackdata slash = new attackdata("slash", 40, "none", 2);
                attackdata leaf_step = new attackdata("leaf step", 80, "none", 2);
                attackdata fighting_claws = new attackdata("fighting claws", 60, "if the active enemy is an EX, this does 70 more damage", 2);
                attackdata anchor = new attackdata("anchor", 80, "none", 2);
                attackdata horn_throw = new attackdata("horn throw", 90, "none", 3);
                attackdata claws = new attackdata("claws", 60, "none", 3);
                attackdata crimson_storm = new attackdata("crimson storm", 200, "none", 4);
                attackdata corner = new attackdata("corner", 70, "none", 2);
                attackdata smoke_bomb = new attackdata("smoke bomb", 70, "none", 3);
                attackdata pound = new attackdata("pound", 40, "none", 2);
                attackdata splash = new attackdata("splash", 40, "none", 1);
                attackdata hydro_splash = new attackdata("hydro splash", 90, "none", 2);
                attackdata blizzard = new attackdata("blizzard", 100, "none", 3);
                attackdata whirlpool = new attackdata("whirlpool", 140, "none", 4);

                Pokemon scyther = new Pokemon("scyther", "grass", 70, 70, 1, "fire", 0, 0, sharp_scythe, "none");
                Pokemon oddish1 = new Pokemon("oddish", "grass", 60, 60, 1, "fire", 0, 0, ram, "none");
                Pokemon oddish2 = new Pokemon("oddish", "grass", 60, 60, 1, "fire", 0, 0, blot, "none");
                Pokemon gloom = new Pokemon("gloom", "grass", 80, 80, 2, "fire", 1, 0, sharp_scythe, "oddish");
                Pokemon gloom2 = new Pokemon("gloom", "grass", 80, 80, 2, "fire", 1, 0, sharp_scythe, "oddish");
                Pokemon bellossom = new Pokemon("bellossom", "grass", 130, 130, 1, "fire", 2, 0, anchor, "gloom");
                Pokemon bellossom2 = new Pokemon("bellossom", "grass", 130, 130, 1, "fire", 2, 0, anchor, "gloom");
                Pokemon combee = new Pokemon("combee", "grass", 50, 50, 1, "fire", 0, 0, ram, "none");
                Pokemon combee2 = new Pokemon("combee", "grass", 50, 50, 1, "fire", 0, 0, ram, "none");
                Pokemon vespiquen = new Pokemon("vespiquen", "grass", 100, 100, 2, "fire", 1, 0, leaf_step, "combee");
                Pokemon sprigatito = new Pokemon("sprigatito", "grass", 60, 60, 1, "fire", 0, 0, ram, "none");
                Pokemon sprigatito2 = new Pokemon("sprigatito", "grass", 60, 60, 1, "fire", 0, 0, ram, "none");
                Pokemon floragato = new Pokemon("floragato", "grass", 90, 90, 1, "fire", 1, 0, slash, "sprigatito");
                Pokemon floragato2 = new Pokemon("floragato", "grass", 90, 90, 1, "fire", 1, 0, slash, "sprigatito");
                Pokemon meowscarada = new Pokemon("meowscarada", "grass", 140, 140, 1, "fire", 2, 0, fighting_claws, "floragato");
                Pokemon meowscarada2 = new Pokemon("meowscarada", "grass", 140, 140, 1, "fire", 2, 0, fighting_claws, "floragato");
                Pokemon dhelmise_EX = new Pokemon("dhelmise EX", "grass", 140, 140, 2, "fire", 0, 0, anchor, "none");
                Pokemon dhelmise_EX2 = new Pokemon("dhelmise EX", "grass", 140, 140, 2, "fire", 0, 0, anchor, "none");
                Pokemon heracross = new Pokemon("heracross", "grass", 100, 100, 2, "fire", 0, 0, horn_throw, "none");
                Pokemon heracross2 = new Pokemon("heracross", "grass", 100, 100, 2, "fire", 0, 0, horn_throw, "none");
                grassdeck.Add(scyther);
                grassdeck.Add(oddish1);
                grassdeck.Add(oddish2);
                grassdeck.Add(gloom);
                grassdeck.Add(gloom2);
                grassdeck.Add(bellossom);
                grassdeck.Add(bellossom2);
                grassdeck.Add(combee);
                grassdeck.Add(combee2);
                grassdeck.Add(vespiquen);
                grassdeck.Add(sprigatito);
                grassdeck.Add(sprigatito2);
                grassdeck.Add(floragato);
                grassdeck.Add(floragato2);
                grassdeck.Add(meowscarada);
                grassdeck.Add(meowscarada2);
                grassdeck.Add(dhelmise_EX);
                grassdeck.Add(dhelmise_EX2);
                grassdeck.Add(heracross);
                grassdeck.Add(heracross2);

                Pokemon charmander1 = new Pokemon("charmander", "fire", 60, 60, 1, "water", 0, 0, ram, "none");
                Pokemon charmander2 = new Pokemon("charmander", "fire", 60, 60, 1, "water", 0, 0, ram, "none");
                Pokemon charmeleon1 = new Pokemon("charmeleon", "fire", 90, 90, 2, "water", 1, 0, claws, "charmander");
                Pokemon charmeleon2 = new Pokemon("charmeleon", "fire", 90, 90, 2, "water", 1, 0, claws, "charmander");
                Pokemon charizard_EX1 = new Pokemon("charizard EX", "fire", 180, 180, 2, "water", 2, 0, crimson_storm, "charmeleon");
                Pokemon charizard_EX2 = new Pokemon("charizard EX", "fire", 180, 180, 2, "water", 2, 0, crimson_storm, "charmeleon");
                Pokemon heatmor1 = new Pokemon("heatmor", "fire", 80, 80, 1, "water", 0, 0, sharp_scythe, "none");
                Pokemon heatmor2 = new Pokemon("heatmor", "fire", 80, 80, 1, "water", 0, 0, sharp_scythe, "none");
                Pokemon houndour1 = new Pokemon("houndour", "fire", 60, 60, 1, "water", 0, 0, ram, "none");
                Pokemon houndour2 = new Pokemon("houndour", "fire", 60, 60, 1, "water", 0, 0, ram, "none");
                Pokemon houndoom1 = new Pokemon("houndoom", "fire", 100, 100, 2, "water", 1, 0, corner, "houndour");
                Pokemon houndoom2 = new Pokemon("houndoom", "fire", 100, 100, 2, "water", 1, 0, corner, "houndour");
                Pokemon magmar1 = new Pokemon("magmar", "fire", 70, 70, 1, "water", 0, 0, ram, "none");
                Pokemon magmar2 = new Pokemon("magmar", "fire", 70, 70, 1, "water", 0, 0, ram, "none");
                Pokemon magmortar1 = new Pokemon("magmortar", "fire", 120, 120, 3, "water", 1, 0, smoke_bomb, "magmar");
                Pokemon magmortar2 = new Pokemon("magmortar", "fire", 120, 120, 3, "water", 1, 0, smoke_bomb, "magmar");
                Pokemon fire_tauros1 = new Pokemon("fire tauros", "fire", 110, 110, 2, "water", 0, 0, horn_throw, "none");
                Pokemon fire_tauros2 = new Pokemon("fire tauros", "fire", 110, 110, 2, "water", 0, 0, horn_throw, "none");
                Pokemon oricorio = new Pokemon("oricorio", "fire", 70, 70, 1, "water", 0, 0, pound, "none");
                Pokemon oricorio2 = new Pokemon("oricorio", "fire", 70, 70, 1, "water", 0, 0, pound, "none");
                firedeck.Add(charmander1);
                firedeck.Add(charmander2);
                firedeck.Add(charmeleon1);
                firedeck.Add(charmeleon2);
                firedeck.Add(charizard_EX1);
                firedeck.Add(charizard_EX2);
                firedeck.Add(heatmor1);
                firedeck.Add(heatmor2);
                firedeck.Add(houndour1);
                firedeck.Add(houndour2);
                firedeck.Add(houndoom1);
                firedeck.Add(houndoom2);
                firedeck.Add(magmar1);
                firedeck.Add(magmar2);
                firedeck.Add(magmortar1);
                firedeck.Add(magmortar2);
                firedeck.Add(fire_tauros1);
                firedeck.Add(fire_tauros2);
                firedeck.Add(oricorio);
                firedeck.Add(oricorio2);

                Pokemon staryu1 = new Pokemon("staryu", "water", 50, 50, 1, "grass", 0, 0, ram, "none");
                Pokemon staryu2 = new Pokemon("staryu", "water", 50, 50, 1, "grass", 0, 0, ram, "none");
                Pokemon starmie = new Pokemon("starmie", "water", 90, 90, 1, "grass", 1, 0, splash, "staryu");
                Pokemon starmie_EX = new Pokemon("starmie EX", "water", 130, 130, 0, "grass", 1, 0, hydro_splash, "staryu");
                Pokemon articuno_EX1 = new Pokemon("articuno EX", "water", 150, 150, 2, "grass", 0, 0, blizzard, "none");
                Pokemon articuno_EX2 = new Pokemon("articuno EX", "water", 150, 150, 2, "grass", 0, 0, blizzard, "none");
                Pokemon magikarp = new Pokemon("magikarp", "water", 30, 30, 1, "grass", 0, 0, ram, "none");
                Pokemon gyarados_EX = new Pokemon("gyarados EX", "water", 180, 180, 3, "grass", 1, 0, whirlpool, "magikarp");
                Pokemon buizel1 = new Pokemon("buizel", "water", 60, 60, 1, "grass", 0, 0, ram, "none");
                Pokemon buizel2 = new Pokemon("buizel", "water", 60, 60, 1, "grass", 0, 0, ram, "none");
                Pokemon floatzel1 = new Pokemon("floatzel", "water", 90, 90, 1, "grass", 1, 0, sharp_scythe, "buizel");
                Pokemon floatzel2 = new Pokemon("floatzel", "water", 90, 90, 1, "grass", 1, 0, sharp_scythe, "buizel");
                Pokemon tentacool1 = new Pokemon("tentacool", "water", 70, 70, 1, "grass", 0, 0, ram, "none");
                Pokemon tentacool2 = new Pokemon("tentacool", "water", 70, 70, 1, "grass", 0, 0, ram, "none");
                Pokemon tentacruel1 = new Pokemon("tentacruel", "water", 100, 100, 1, "grass", 1, 0, splash, "tentacool");
                Pokemon tentacruel2 = new Pokemon("tentacruel", "water", 100, 100, 1, "grass", 1, 0, splash, "tentacool");
                Pokemon bruxish1 = new Pokemon("bruxish", "water", 80, 80, 1, "grass", 0, 0, pound, "none");
                Pokemon bruxish2 = new Pokemon("bruxish", "water", 80, 80, 1, "grass", 0, 0, pound, "none");
                Pokemon lapras1 = new Pokemon("lapras", "water", 110, 110, 2, "grass", 0, 0, smoke_bomb, "none");
                Pokemon lapras2 = new Pokemon("lapras", "water", 110, 110, 2, "grass", 0, 0, smoke_bomb, "none");
                waterdeck.Add(staryu1);
                waterdeck.Add(staryu2);
                waterdeck.Add(starmie);
                waterdeck.Add(starmie_EX);
                waterdeck.Add(articuno_EX1);
                waterdeck.Add(articuno_EX2);
                waterdeck.Add(magikarp);
                waterdeck.Add(gyarados_EX);
                waterdeck.Add(buizel1);
                waterdeck.Add(buizel2);
                waterdeck.Add(floatzel1);
                waterdeck.Add(floatzel2);
                waterdeck.Add(tentacool1);
                waterdeck.Add(tentacool2);
                waterdeck.Add(tentacruel1);
                waterdeck.Add(tentacruel2);
                waterdeck.Add(bruxish1);
                waterdeck.Add(bruxish2);
                waterdeck.Add(lapras1);
                waterdeck.Add(lapras2);

                bool deckchosen = false;
                while (deckchosen == false)
                {
                    Console.WriteLine("Choose your deck.");
                    Console.WriteLine("Grass Deck       Fire Deck       Water Deck");
                    Console.WriteLine("'grass'          'fire'          'water'");
                    string input = Console.ReadLine();
                    if (input == "grass")
                    {
                        deckelement = 0;
                        decklist.Add(scyther);
                        decklist.Add(oddish1);
                        decklist.Add(oddish2);
                        decklist.Add(gloom);
                        decklist.Add(gloom2);
                        decklist.Add(bellossom);
                        decklist.Add(bellossom2);
                        decklist.Add(combee);
                        decklist.Add(combee2);
                        decklist.Add(vespiquen);
                        decklist.Add(sprigatito);
                        decklist.Add(sprigatito2);
                        decklist.Add(floragato);
                        decklist.Add(floragato2);
                        decklist.Add(meowscarada);
                        decklist.Add(meowscarada2);
                        decklist.Add(dhelmise_EX);
                        decklist.Add(dhelmise_EX2);
                        decklist.Add(heracross);
                        decklist.Add(heracross2);
                        deckchosen = true;
                        deckelemental = "grass";
                        deckelement = 1;
                    }
                    else if (input == "fire")
                    {
                        deckelement = 1;
                        decklist.Add(charmander1);
                        decklist.Add(charmander2);
                        decklist.Add(charmeleon1);
                        decklist.Add(charmeleon2);
                        decklist.Add(charizard_EX1);
                        decklist.Add(charizard_EX2);
                        decklist.Add(heatmor1);
                        decklist.Add(heatmor2);
                        decklist.Add(houndour1);
                        decklist.Add(houndour2);
                        decklist.Add(houndoom1);
                        decklist.Add(houndoom2);
                        decklist.Add(magmar1);
                        decklist.Add(magmar2);
                        decklist.Add(magmortar1);
                        decklist.Add(magmortar2);
                        decklist.Add(fire_tauros1);
                        decklist.Add(fire_tauros2);
                        decklist.Add(oricorio);
                        decklist.Add(oricorio2);
                        deckchosen = true;
                        deckelemental = "fire";
                        deckelement = 2;
                    }
                    else if (input == "water")
                    {
                        deckelement = 2;
                        decklist.Add(staryu1);
                        decklist.Add(staryu2);
                        decklist.Add(starmie);
                        decklist.Add(starmie_EX);
                        decklist.Add(articuno_EX1);
                        decklist.Add(articuno_EX2);
                        decklist.Add(magikarp);
                        decklist.Add(gyarados_EX);
                        decklist.Add(buizel1);
                        decklist.Add(buizel2);
                        decklist.Add(floatzel1);
                        decklist.Add(floatzel2);
                        decklist.Add(tentacool1);
                        decklist.Add(tentacool2);
                        decklist.Add(tentacruel1);
                        decklist.Add(tentacruel2);
                        decklist.Add(bruxish1);
                        decklist.Add(bruxish2);
                        decklist.Add(lapras1);
                        decklist.Add(lapras2);
                        deckchosen = true;
                        deckelemental = "water";
                        deckelement = 3;
                    }
                    else
                    {
                        Console.WriteLine("please input either grass, fire, or water. ");
                    }
                }
                //Console.WriteLine("your deck contains:");
                /*
                for (int i = 0; i < decklist.Count; i++)
                {
                    if (i != 19)
                    {
                        Console.Write(decklist[i].name + ", ");
                    }
                    else
                    {
                        Console.Write(decklist[i].name);
                    }
                    Console.WriteLine("");
                }
                */
                while (decklocked == false)
                {
                    if (deckchooser == deckelement)
                    {
                        rng = new Random();
                        deckchooser = rng.Next(3);
                    }
                    else
                    {
                        enemydeck = deckchooser;
                        decklocked = true;
                    }
                }
                if (deckchooser == 0)
                {
                    enemylist.Add(scyther);
                    enemylist.Add(oddish1);
                    enemylist.Add(oddish2);
                    enemylist.Add(gloom);
                    enemylist.Add(gloom2);
                    enemylist.Add(bellossom);
                    enemylist.Add(combee);
                    enemylist.Add(combee2);
                    enemylist.Add(vespiquen);
                    enemylist.Add(sprigatito);
                    enemylist.Add(sprigatito2);
                    enemylist.Add(floragato);
                    enemylist.Add(floragato2);
                    enemylist.Add(meowscarada);
                    enemylist.Add(meowscarada2);
                    enemylist.Add(dhelmise_EX);
                    enemylist.Add(dhelmise_EX2);
                    enemylist.Add(heracross);
                    enemylist.Add(heracross2);
                }
                else if (deckchooser == 1)
                {
                    enemylist.Add(charmander1);
                    enemylist.Add(charmander2);
                    enemylist.Add(charmeleon1);
                    enemylist.Add(charmeleon2);
                    enemylist.Add(charizard_EX1);
                    enemylist.Add(charizard_EX2);
                    enemylist.Add(heatmor1);
                    enemylist.Add(heatmor2);
                    enemylist.Add(houndour1);
                    enemylist.Add(houndour2);
                    enemylist.Add(houndoom1);
                    enemylist.Add(houndoom2);
                    enemylist.Add(magmar1);
                    enemylist.Add(magmar2);
                    enemylist.Add(magmortar1);
                    enemylist.Add(magmortar2);
                    enemylist.Add(fire_tauros1);
                    enemylist.Add(fire_tauros2);
                    enemylist.Add(oricorio);
                    enemylist.Add(oricorio2);
                }
                else if (deckchooser == 2)
                {
                    enemylist.Add(staryu1);
                    enemylist.Add(staryu2);
                    enemylist.Add(starmie);
                    enemylist.Add(starmie_EX);
                    enemylist.Add(articuno_EX1);
                    enemylist.Add(articuno_EX2);
                    enemylist.Add(magikarp);
                    enemylist.Add(gyarados_EX);
                    enemylist.Add(buizel1);
                    enemylist.Add(buizel2);
                    enemylist.Add(floatzel1);
                    enemylist.Add(floatzel2);
                    enemylist.Add(tentacool1);
                    enemylist.Add(tentacool2);
                    enemylist.Add(tentacruel1);
                    enemylist.Add(tentacruel2);
                    enemylist.Add(bruxish1);
                    enemylist.Add(bruxish2);
                    enemylist.Add(lapras1);
                    enemylist.Add(lapras2);
                }
                List<Pokemon> hand = new List<Pokemon>();
                List<Pokemon> enemyhand = new List<Pokemon>();
                bool gtg = false;
                while (gtg == false)
                {               //player loop
                    do
                    {
                        hand.Clear();
                        if (deckelemental == "grass")
                        {
                            decklist = grassdeck;
                        }
                        if (deckelemental == "fire")
                        {
                            decklist = firedeck;
                        }
                        if (deckelemental == "water")
                        {
                            decklist = waterdeck;
                        }
                        cardrawer = rng.Next(pcardsleft);
                        hand.Add(DrawCard(decklist, cardrawer, rng));
                        cardrawer = rng.Next(pcardsleft);
                        hand.Add(DrawCard(decklist, cardrawer, rng));
                        cardrawer = rng.Next(pcardsleft);
                        hand.Add(DrawCard(decklist, cardrawer, rng));
                        cardrawer = rng.Next(pcardsleft);
                        hand.Add(DrawCard(decklist, cardrawer, rng));
                        cardrawer = rng.Next(pcardsleft);
                        removenulls(enemyhand, hand);
                    }
                    while (!hand.Any(p => p.stage == 0));
                    if (hand.Any(p => p.stage == 0))
                    {
                        gtg = true;
                    }
                    /*for (int i = 0; i < hand.Count; i++)
                    {
                        if (hand[i] == null)
                        {
                            hand.RemoveAt(i);
                        }
                    }*/
                }
                gtg = false;
                while (gtg == false)
                {               //enemy loop
                    do
                    {
                        enemyhand.Clear();
                        if (deckchooser == 0)
                        {
                            enemylist = grassdeck;
                        }
                        if (deckchooser == 1)
                        {
                            enemylist = firedeck;
                        }
                        if (deckchooser == 2)
                        {
                            enemylist = waterdeck;
                        }
                        cardrawer = rng.Next(ecardsleft);
                        enemyhand.Add(DrawCard(enemylist, cardrawer, rng));
                        cardrawer = rng.Next(ecardsleft);
                        enemyhand.Add(DrawCard(enemylist, cardrawer, rng));
                        cardrawer = rng.Next(ecardsleft);
                        enemyhand.Add(DrawCard(enemylist, cardrawer, rng));
                        cardrawer = rng.Next(ecardsleft);
                        enemyhand.Add(DrawCard(enemylist, cardrawer, rng));
                        cardrawer = rng.Next(ecardsleft);
                        removenulls(enemyhand, hand);
                    }
                    while (!enemyhand.Any(p => p.stage == 0));
                    if (enemyhand.Any(p => p.stage == 0))
                    {
                        gtg = true;
                    }
                    /*for (int i = 0; i < enemyhand.Count; i++)
                    {
                        if (enemyhand[i] == null)
                        {
                            enemyhand.RemoveAt(i);
                        }
                    }*/
                }
                List<Pokemon> ebench = new List<Pokemon>();
                List<Pokemon> pbench = new List<Pokemon>();
                ecardsleft = 20;
                pcardsleft = 20;
                int epoints = 0;
                int ppoints = 0;
                int chosenactive;  //player turn
                Pokemon pactivepokemon = null;
                Pokemon eactivepokemon = null;
                bool gamend = false;
                Console.WriteLine("play a card for your active spot.");
                Console.Write("Hand: ");
                for (int i = 0; i < hand.Count; i++)
                {
                    Console.Write(hand[i].name + ", ");
                }
                Console.WriteLine();
                Console.WriteLine("select 0 through 4 to choose your active pokemon, left to right 0 to 3.");
                while (pactivepokemon == null)
                {
                    string input = Console.ReadLine();
                    if (input == "0" || input == "1" || input == "2" || input == "3")
                    {
                        chosenactive = int.Parse(input);
                        if (hand[chosenactive].stage == 0)
                        {
                            pactivepokemon = hand[chosenactive];
                            hand.RemoveAt(chosenactive);
                            Console.WriteLine(pactivepokemon.name);
                        }
                        else
                        {
                            Console.WriteLine("please input a number that is a basic pokemon 0 through 3.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("please input a number 0 through 3");
                    }
                }


                //enemy turn
                Pokemon ecardplaced = null;
                for (int i = 0;  i < enemyhand.Count; i++)
                {
                    if (enemyhand[i].stage == 0)
                    {
                        eactivepokemon = enemyhand[i];
                    }
                    ecardplaced = enemyhand[i];
                }
                enemyhand.Remove(ecardplaced);
                cardrawer = rng.Next(pcardsleft);
                hand.Add(DrawCard(decklist, cardrawer, rng));
                cardrawer = rng.Next(ecardsleft);
                enemyhand.Add(DrawCard(enemylist, cardrawer, rng));
                cardrawer = rng.Next(ecardsleft);


                while (!gamend)
                {
                    bool playerturn = true;
                    cardrawer = rng.Next(pcardsleft);
                    hand.Add(DrawCard(decklist, cardrawer, rng));
                    cardrawer = rng.Next(pcardsleft); //draws card for player
                    removenulls(enemyhand, hand);
                    int energyleft = 1; // we use this to control energy flow per turn


                    while (playerturn == true && eactivepokemon != null) //player turn
                    {
                        if (gamend || eactivepokemon == null)
                        {
                            gamend = true;
                            playerwins = true;
                            break;
                        }
                        writescreen(enemyhand, epoints, ebench, eactivepokemon, pactivepokemon, hand, pbench, ppoints, energyleft);
                        string input = Console.ReadLine();
                        if (input == "1") //hand
                        {
                            Console.WriteLine("Hand: ");
                            for (int i = 0; i < hand.Count; i++)
                            {
                                Console.Write(hand[i].name + ", ");
                            }
                            Console.WriteLine();
                            Console.WriteLine("select any card in your hand, or press a letter to go back.");
                            input = Console.ReadLine();
                            if (int.TryParse(input, out inputchecker) && int.Parse(input) < hand.Count)
                            {
                                int iinput = int.Parse(input);
                                Console.WriteLine(hand[iinput].name + ": " + hand[iinput].energy + " energy " + hand[iinput].hp + " hp " + hand[iinput].type + " "
                                + "retreat cost " + hand[iinput].retreatcost + " stage " + hand[iinput].stage + " evolves from: " + hand[iinput].evolvesfrom);
                                Console.WriteLine("press 1 to play to bench or evolve. press anything else to go back.");
                                input = Console.ReadLine();
                                if (int.TryParse(input, out inputchecker))
                                {
                                    if (inputchecker == 1)
                                    {
                                        if (pbench.Count < 3 && hand[iinput].stage == 0) //play a card
                                        {
                                            pbench.Add(hand[iinput]);
                                            hand.RemoveAt(iinput);
                                            iinput = -1;
                                        }
                                        else if (hand[iinput].stage == 1) //evolve logic
                                        {
                                            if (pactivepokemon.stage == 0 && hand[iinput].evolvesfrom == pactivepokemon.name)
                                            {
                                                int energyon = pactivepokemon.energy;
                                                int damage = pactivepokemon.maxhp - pactivepokemon.hp;
                                                pactivepokemon = hand[iinput];
                                                hand.RemoveAt(iinput);
                                                pactivepokemon.energy = energyon;
                                                pactivepokemon.hp = pactivepokemon.hp - damage;
                                                break; // keeps from evolving 2 of the same card with one higher stage
                                            }
                                            for (int j = 0; j < pbench.Count; j++)
                                            {
                                                if (pbench[j].stage == 0 && hand[iinput].evolvesfrom == pbench[j].name)
                                                {
                                                    int energyon = pbench[j].energy;
                                                    int damage = pbench[j].maxhp - pbench[j].hp;
                                                    pbench[j] = hand[iinput];
                                                    hand.RemoveAt(iinput);
                                                    pbench[j].energy = energyon;
                                                    pbench[j].hp = pbench[j].hp - damage;
                                                    break; // keeps from evolving 2 of the same card with one higher stage
                                                }
                                            }
                                        }
                                        else if (hand[iinput].stage == 2) //evolve logic
                                        {
                                            if (pactivepokemon.stage == 1 && hand[iinput].evolvesfrom == pactivepokemon.name)
                                            {
                                                int energyon = pactivepokemon.energy;
                                                int damage = pactivepokemon.maxhp - pactivepokemon.hp;
                                                pactivepokemon = hand[iinput];
                                                hand.RemoveAt(iinput);
                                                pactivepokemon.energy = energyon;
                                                pactivepokemon.hp = pactivepokemon.hp - damage;
                                                break; // keeps from evolving 2 of the same card with one higher stage
                                            }
                                            for (int j = 0; j < pbench.Count; j++)
                                            {
                                                if (pbench[j].stage == 1 && hand[iinput].evolvesfrom == pbench[j].name)
                                                {
                                                    int energyon = pbench[j].energy;
                                                    int damage = pbench[j].maxhp - pbench[j].hp;
                                                    pbench[j] = hand[iinput];
                                                    hand.RemoveAt(iinput);
                                                    pbench[j].energy = energyon;
                                                    pbench[j].hp = pbench[j].hp - damage;
                                                    break; // keeps from evolving 2 of the same card with one higher stage
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("hand is full or card cannot be played. Press a key to continue.");
                                            char whyexistatall = Console.ReadKey().KeyChar;
                                        }
                                    }
                                }
                            }
                        }
                        else if (input == "2") //bench
                        {
                            Console.WriteLine("select a benched pokemon to turn to active. Press anything else to go back.");
                            Console.WriteLine("Your Bench: ");
                            for (int i = 0; i < pbench.Count; i++)
                            {
                                Console.Write(pbench[i].name.PadRight(15));
                            }
                            Console.WriteLine();
                            for (int i = 0; i < pbench.Count; i++)
                            {
                                Console.Write(i.ToString().PadRight(15));
                            }
                            Console.WriteLine();
                            Console.WriteLine("Enemy Bench:");
                            for (int i = 0; i < ebench.Count; i++)
                            {
                                Console.Write(ebench[i].name.PadRight(15));
                            }
                            Console.WriteLine();
                            for (int i = 0; i < ebench.Count; i++)
                            {
                                Console.Write(i.ToString().PadRight(15));
                            }
                            Console.WriteLine();
                            input = Console.ReadLine();
                            if (input == "0")
                            {
                                if (pactivepokemon.energy >= pactivepokemon.retreatcost)
                                {
                                    if (pbench.Count >= 1)
                                    {
                                        pactivepokemon.energy -= pactivepokemon.retreatcost;
                                        pbench.Add(pactivepokemon);
                                        pactivepokemon = pbench[0];
                                        pbench.RemoveAt(0);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("active pokemon does not have enough energy to retreat, or there is nothing there.");
                                    char whyevenexistatall = Console.ReadKey().KeyChar;
                                }
                            }
                            else if (input == "1")
                            {
                                if (pactivepokemon.energy >= pactivepokemon.retreatcost)
                                {
                                    if (pbench.Count >= 2)
                                    {
                                        pactivepokemon.energy -= pactivepokemon.retreatcost;
                                        pbench.Add(pactivepokemon);
                                        pactivepokemon = pbench[1];
                                        pbench.RemoveAt(1);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("active pokemon does not have enough energy to retreat, or there is nothing there.");
                                    char whyevenexistatall = Console.ReadKey().KeyChar;
                                }
                            }
                            else if (input == "2")
                            {
                                if (pactivepokemon.energy >= pactivepokemon.retreatcost)
                                {
                                    if (pbench.Count >= 3)
                                    {
                                        pactivepokemon.energy -= pactivepokemon.retreatcost;
                                        pbench.Add(pactivepokemon);
                                        pactivepokemon = pbench[2];
                                        pbench.RemoveAt(2);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("active pokemon does not have enough energy to retreat, or there is nothing there.");
                                    char whyevenexistatall = Console.ReadKey().KeyChar;
                                }
                            }
                        }
                        else if (input == "3")
                        {
                            Console.WriteLine($"Enemy Active Pokémon: {eactivepokemon.name}, Type: {eactivepokemon.type}, HP: {eactivepokemon.hp}, " +
                                              $"Retreat Cost: {eactivepokemon.retreatcost}, Stage: {eactivepokemon.stage}, Energy: {eactivepokemon.energy}" +
                                              $" Attack: {eactivepokemon.atkdata.attackname}, Damage: {eactivepokemon.atkdata.damage} " +
                                              $"         Cost: {eactivepokemon.atkdata.energycost}       Effect: {eactivepokemon.atkdata.effect}");
                            Console.WriteLine();
                            Console.WriteLine($"Active Pokémon: {pactivepokemon.name}, Type: {pactivepokemon.type}, HP: {pactivepokemon.hp}, " +
                                              $"Retreat Cost: {pactivepokemon.retreatcost}, Stage: {pactivepokemon.stage}, Energy: {pactivepokemon.energy}" +
                                              $" Attack: {pactivepokemon.atkdata.attackname}, Damage: {pactivepokemon.atkdata.damage} " +
                                              $"         Cost: {pactivepokemon.atkdata.energycost}       Effect: {pactivepokemon.atkdata.effect}");
                            Console.WriteLine("press a letter to go back, press 1 to attack");
                            input = Console.ReadLine();
                            if (input == "1")
                            {
                                if (pactivepokemon.energy >= pactivepokemon.atkdata.energycost)
                                {
                                    DoAttack(pactivepokemon, eactivepokemon);
                                    playerturn = false;
                                }
                                else
                                {
                                    Console.WriteLine("cannot attack");
                                    char whyevenexistatall = Console.ReadKey().KeyChar;
                                }
                            }
                        }
                        else if (input == "4")
                        {
                            playerturn = false;
                        }
                        else if (input == "5")
                        {
                            if (energyleft == 1)
                            {
                                Console.WriteLine("select a pokemon to attach. 1 for active, 2 to select farther into bench.");
                                Console.WriteLine($"Active Pokémon: {pactivepokemon.name}, Type: {pactivepokemon.type}, HP: {pactivepokemon.hp}, " +
                                                  $"Retreat Cost: {pactivepokemon.retreatcost}, Stage: {pactivepokemon.stage}, Energy: {pactivepokemon.energy}" +
                                                  $" Attack: {pactivepokemon.atkdata.attackname}, Damage: {pactivepokemon.atkdata.damage} " +
                                                  $"         Cost: {pactivepokemon.atkdata.energycost}       Effect: {pactivepokemon.atkdata.effect}");
                                Console.WriteLine();
                                Console.WriteLine();
                                for (int i = 0; i < pbench.Count; i++)
                                {
                                    Console.Write(i.ToString().PadRight(15));
                                }
                                Console.WriteLine();
                                input = Console.ReadLine();
                                if (input == "1")
                                {
                                    pactivepokemon.energy++;
                                    energyleft = 0;
                                }
                                else if (input == "2")
                                {
                                    Console.WriteLine("Your Bench: ");
                                    for (int i = 0; i < pbench.Count; i++)
                                    {
                                        Console.Write(pbench[i].name.PadRight(15));
                                    }
                                    Console.WriteLine();
                                    int pbenchcountminusone = pbench.Count - 1;
                                    Console.WriteLine("Select a character, 0 to " + pbenchcountminusone + ".");
                                    input = Console.ReadLine();
                                    int intput = -1;
                                    if (int.TryParse(input, out intput))
                                    {
                                        if (intput == 0 || intput == 1 || intput == 2)
                                        {
                                            if (intput < pbench.Count)
                                            {
                                                Console.WriteLine("attach energy to this pokemon? Y/N");
                                                Console.Write(pbench[intput].name + "    damage: " + pbench[intput].atkdata.damage + ", cost: " + pbench[intput].atkdata.energycost + ", energy: " + pbench[intput].energy + ", effect:" + pbench[intput].atkdata.effect );
                                            }
                                            string attachoice = Console.ReadLine();
                                            if (attachoice == "Y" || attachoice == "y")
                                            {
                                                pbench[intput].energy++;
                                                energyleft = 0;
                                            }
                                            else if (attachoice == "N" || attachoice == "n")
                                            {}
                                            else
                                            {
                                                Console.WriteLine("please only put in Y or N.");
                                                char whyevenexistatall = Console.ReadKey().KeyChar;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("please only input a benched pokemon's number.");
                                            char whyevenexistatall = Console.ReadKey().KeyChar;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("you have already attached energy this turn");
                                char whyevenexistatall = Console.ReadKey().KeyChar;
                            }
                        }
                        else
                        {
                            Console.WriteLine("please put in 1 through 5.");
                            char whyexistatall = Console.ReadKey().KeyChar;
                        }
                        check(ref pactivepokemon, pbench, ref ppoints, ref epoints, ref eactivepokemon, ebench, ref gamend, ref playerwins, hand, enemyhand, decklist, enemylist, pcardsleft, ecardsleft, decklocked, ref playerturn);
                    }
                    check(ref pactivepokemon, pbench, ref ppoints, ref epoints, ref eactivepokemon, ebench, ref gamend, ref playerwins, hand, enemyhand, decklist, enemylist, pcardsleft, ecardsleft, decklocked, ref playerturn);
                    while (!playerturn && eactivepokemon != null) //enemy turn
                    {
                        if (gamend || eactivepokemon == null)
                        {
                            gamend = true;
                            playerwins = true;
                            break;
                        }
                        check(ref pactivepokemon, pbench, ref ppoints, ref epoints, ref eactivepokemon, ebench, ref gamend, ref playerwins, hand, enemyhand, decklist, enemylist, pcardsleft, ecardsleft, decklocked, ref playerturn);
                        cardrawer = rng.Next(ecardsleft);
                        enemyhand.Add(DrawCard(enemylist, cardrawer, rng));
                        cardrawer = rng.Next(ecardsleft);
                        removenulls(enemyhand, hand);
                        for (int i = enemyhand.Count - 1; i >= 0; i--) // hand procedure to play basics
                        {

                            if (enemyhand.Count > 0)
                            {
                                if (i < enemyhand.Count && enemyhand[i].stage == 0)
                                {
                                    if (ebench.Count < 3) //if the bench has room, add a card
                                    {
                                        ebench.Add(enemyhand[i]);
                                        enemyhand.RemoveAt(i);
                                    }
                                }
                                else if (i < enemyhand.Count && enemyhand[i].stage == 1) //evolve logic basic to 1
                                {
                                    bool cardplayed= false;
                                    for (int j = 0; j < ebench.Count; j++)
                                    {
                                        for (int h = 0; h < enemyhand.Count; h++)
                                        {
                                            if (eactivepokemon.stage == 0 && enemyhand[h].evolvesfrom == eactivepokemon.name)
                                            {
                                                int energyon = eactivepokemon.energy;
                                                int damage = eactivepokemon.maxhp - eactivepokemon.hp;
                                                eactivepokemon = enemyhand[h];
                                                enemyhand.RemoveAt(h);
                                                eactivepokemon.energy = energyon;
                                                eactivepokemon.hp = eactivepokemon.hp - damage;
                                                cardplayed = true;
                                                break;
                                            }
                                        }
                                        if (cardplayed)
                                        {
                                            break;
                                        }
                                        for (int h = 0; h < enemyhand.Count; h++)
                                        {
                                            if (ebench[j].stage == 0 && enemyhand[h].evolvesfrom == ebench[j].name)
                                            {
                                                int energyon = ebench[j].energy;
                                                int damage = ebench[j].maxhp - ebench[j].hp;
                                                ebench[j] = enemyhand[h];
                                                enemyhand.RemoveAt(h);
                                                ebench[j].energy = energyon;
                                                ebench[j].hp = ebench[j].hp - damage;
                                                break;
                                            }
                                        }
                                        for (int h = 0; h < enemyhand.Count; h++)
                                        {
                                            if (eactivepokemon.stage == 1 && enemyhand[h].evolvesfrom == eactivepokemon.name)
                                            {
                                                int energyon = eactivepokemon.energy;
                                                int damage = eactivepokemon.maxhp - eactivepokemon.hp;
                                                eactivepokemon = enemyhand[h];
                                                enemyhand.RemoveAt(h);
                                                eactivepokemon.energy = energyon;
                                                eactivepokemon.hp = eactivepokemon.hp - damage;
                                                break; // keeps from evolving 2 of the same card with one higher stage
                                            }
                                        }
                                        for (int h = 0; h < enemyhand.Count; h++)
                                        {
                                            if (ebench[j].stage == 1 && enemyhand[h].evolvesfrom == ebench[j].name)
                                            {
                                                int energyon = ebench[j].energy;
                                                int damage = ebench[j].maxhp - ebench[j].hp;
                                                ebench[j] = enemyhand[h];
                                                enemyhand.RemoveAt(h);
                                                ebench[j].energy = energyon;
                                                ebench[j].hp = ebench[j].hp - damage;
                                                break; // keeps from evolving 2 of the same card with one higher stage
                                            }
                                        }
                                    }
                                }
                                else if (i < enemyhand.Count && enemyhand[i].stage == 2) // evolve logic 1 to 2
                                {
                                    for (int j = 0; j < ebench.Count; j++)
                                    {
                                        if (ebench[j].stage == 1 && enemyhand[i].evolvesfrom == ebench[j].name)
                                        {
                                            int energyon = ebench[j].energy;
                                            int damage = ebench[j].maxhp - ebench[j].hp;
                                            ebench[j] = enemyhand[i];
                                            enemyhand.RemoveAt(i);
                                            ebench[j].energy = energyon;
                                            ebench[j].hp = ebench[j].hp - damage;
                                            break; // keeps from evolving 2 of the same card with one higher stage
                                        }
                                    }
                                }
                            }
                        }
                        if (eactivepokemon.energy >= eactivepokemon.atkdata.energycost + eactivepokemon.retreatcost)//energy attach
                        {
                            Pokemon mosthp = Mosthpfinder(ebench);
                            for (int i = ebench.Count - 1; i >= 0; i--)
                            {
                                if (ebench[i].energy < ebench[i].atkdata.energycost + ebench[i].retreatcost && ebench[i] == mosthp)
                                {
                                    ebench[i].energy++;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            eactivepokemon.energy++;
                        }
                        if (eactivepokemon.hp <= pactivepokemon.atkdata.damage && pactivepokemon.energy >= pactivepokemon.atkdata.energycost - 1 && 
                            eactivepokemon.energy >= eactivepokemon.retreatcost && eactivepokemon.energy <= eactivepokemon.atkdata.energycost && 
                            eactivepokemon.atkdata.damage <= pactivepokemon.hp) // retreat if hp is low, and we can't secure a point.
                        {
                            eactivepokemon.energy -= eactivepokemon.retreatcost;
                            Pokemon mosthponbench = Mosthpfinder(ebench);
                            ebench.Add(eactivepokemon);
                            for (int finder = ebench.Count-1; finder >= 0; finder--)
                            {
                                if (mosthponbench == ebench[finder])
                                {
                                    ebench.RemoveAt(finder);
                                    break;
                                }
                            }
                            eactivepokemon = mosthponbench;
                            Console.WriteLine("retreating");
                            char whyevenexistatall = Console.ReadKey().KeyChar;
                        }
                        check(ref pactivepokemon, pbench, ref ppoints, ref epoints, ref eactivepokemon, ebench, ref gamend, ref playerwins, hand, enemyhand, decklist, enemylist, pcardsleft, ecardsleft, decklocked, ref playerturn);
                        if (eactivepokemon != null && eactivepokemon.energy >= eactivepokemon.atkdata.energycost ) // attack
                        {
                            DoAttack(eactivepokemon, pactivepokemon);
                            Console.WriteLine("Enemy attacks and does " + eactivepokemon.atkdata.damage + " damage!");
                            char whyevenexistatall = Console.ReadKey().KeyChar;
                            playerturn = true;
                        }
                        else //if you cant attack end turn
                        {
                            playerturn = true;
                        }
                        check(ref pactivepokemon, pbench, ref ppoints, ref epoints, ref eactivepokemon, ebench, ref gamend, ref playerwins, hand, enemyhand, decklist, enemylist, pcardsleft, ecardsleft, decklocked, ref playerturn);
                    }
                    check(ref pactivepokemon, pbench, ref ppoints, ref epoints, ref eactivepokemon, ebench, ref gamend, ref playerwins, hand, enemyhand, decklist, enemylist, pcardsleft, ecardsleft, decklocked, ref playerturn);
                }
                if (playerwins)
                {
                    Console.WriteLine("you win! restart? y/n");
                    char yn = Console.ReadKey().KeyChar;
                    while (yn != 'y' && yn != 'n')
                    {
                        Console.WriteLine("y or n?");
                        yn = Console.ReadKey().KeyChar;
                    }
                    if (yn == 'n')
                    {
                        Environment.Exit(0);
                    }
                    else if (yn == 'y')
                    {
                    }
                }
                else
                {
                    Console.WriteLine("you lose! restart? y/n");
                    char yn = Console.ReadKey().KeyChar;
                    while (yn != 'y' && yn != 'n')
                    {
                        Console.WriteLine("y or n?");
                        yn = Console.ReadKey().KeyChar;
                    }
                    if (yn == 'n')
                    {
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}
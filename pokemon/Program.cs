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
    5/23        
  */
    static internal class Program
    {
        static void writescreen(List<Pokemon> enemyhand, int epoints ) // todo make screenwrite
        {
            Console.Clear();
            Console.WriteLine("Enemy Hand: " + enemyhand.Count + "   Points:" + epoints );
            //Console.WriteLine("Bench: " + );
        }
        private static Pokemon DrawCard(List<Pokemon> refdeck, int cardrawer, Random rng)
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
                return null;
            }
        }
        static void Main(string[] args)
        {
            /*
            name type hp retreatcost weakness stage attackdata
            name hp retreatcost stage attackdata
            attack name damage effect cost*/
            //pokemon pikachu = new pokemon("pikachu", "electric", 60, 1, "fighting", 0, jolt
            //jolt, 20, paralyze enemy 1);
            Random rng = new Random();
            bool decklocked = false;
            int pcardsleft = 20;
            int ecardsleft = 20;
            int deckchooser = rng.Next(3);
            int cardrawer = rng.Next(pcardsleft);
            int enemydeck;
            int deckelement = -1;
            string cardisplay = "";
            int epoints = 0;
            int ppoints = 0;
            List<Pokemon> decklist = new List<Pokemon>();
            List<Pokemon> enemylist = new List<Pokemon>();
            attackdata ram = new attackdata("ram", 20, "", 1);
            attackdata blot = new attackdata("blot", 10, "heal 10 damage", 1);
            attackdata sharp_scythe = new attackdata("sharp scythe", 30, "", 1);
            attackdata slash = new attackdata("slash", 40, "", 2);
            attackdata leaf_step = new attackdata("leaf step", 80, "", 2);
            attackdata fighting_claws = new attackdata("fighting claws", 60, "if the active enemy is an EX, this does 70 more damage", 2);
            attackdata anchor = new attackdata("anchor", 80, "enemy cannot retreat next turn", 3);
            attackdata horn_throw = new attackdata("anchor", 90, "", 3);
            attackdata claws = new attackdata("claws", 60, "", 3);
            attackdata crimson_storm = new attackdata("crimson storm", 200, "", 4);
            attackdata corner = new attackdata("corner", 70, "", 2);
            attackdata smoke_bomb = new attackdata("smoke bomb", 70, "", 3);
            attackdata pound = new attackdata("pound", 40, "", 2);
            attackdata splash = new attackdata("splash", 40, "", 1);
            attackdata hydro_splash = new attackdata("hydro splash", 90, "", 2);
            attackdata blizzard = new attackdata("blizzard", 100, "", 3);
            attackdata whirlpool = new attackdata("whirlpool", 140, "", 4);

            Pokemon scyther = new Pokemon("scyther", "grass", 70, 1, "fire", 0, sharp_scythe);
            Pokemon oddish1 = new Pokemon("oddish", "grass", 60, 1, "fire", 0, ram);
            Pokemon oddish2 = new Pokemon("oddish", "grass", 60, 1, "fire", 0, blot);
            Pokemon gloom = new Pokemon("gloom", "grass", 80, 2, "fire", 1, sharp_scythe);
            Pokemon gloom2 = new Pokemon("gloom", "grass", 80, 2, "fire", 1, sharp_scythe);
            Pokemon bellossom = new Pokemon("bellossom", "grass", 130, 1, "fire", 2, sharp_scythe);
            Pokemon combee = new Pokemon("combee", "grass", 50, 1, "fire", 0, ram);
            Pokemon combee2 = new Pokemon("combee", "grass", 50, 1, "fire", 0, ram);
            Pokemon vespiquen = new Pokemon("vespiquen", "grass", 100, 2, "fire", 1, leaf_step);
            Pokemon sprigatito = new Pokemon("sprigatito", "grass", 60, 1, "fire", 0, ram);
            Pokemon sprigatito2 = new Pokemon("sprigatito", "grass", 60, 1, "fire", 0, ram);
            Pokemon floragato = new Pokemon("floragato", "grass", 90, 1, "fire", 1, slash);
            Pokemon floragato2 = new Pokemon("floragato", "grass", 90, 1, "fire", 1, slash);
            Pokemon meowscarada = new Pokemon("meowscarada", "grass", 140, 1, "fire", 2, fighting_claws);
            Pokemon meowscarada2 = new Pokemon("meowscarada", "grass", 140, 1, "fire", 2, fighting_claws);
            Pokemon dhelmise_EX = new Pokemon("dhelmise EX", "grass", 140, 2, "fire", 0, anchor);
            Pokemon dhelmise_EX2 = new Pokemon("dhelmise EX", "grass", 140, 2, "fire", 0, anchor);
            Pokemon heracross = new Pokemon("heracross", "grass", 100, 2, "fire", 0, horn_throw);
            Pokemon heracross2 = new Pokemon("heracross", "grass", 100, 2, "fire", 0, horn_throw);

            Pokemon charmander1 = new Pokemon("charmander", "fire", 60, 1, "water", 0, ram);
            Pokemon charmander2 = new Pokemon("charmander", "fire", 60, 1, "water", 0, ram);
            Pokemon charmeleon1 = new Pokemon("charmeleon", "fire", 90, 2, "water", 1, claws);
            Pokemon charmeleon2 = new Pokemon("charmeleon", "fire", 90, 2, "water", 1, claws);
            Pokemon charizard_EX1 = new Pokemon("charizard EX", "fire", 180, 2, "water", 2, crimson_storm);
            Pokemon charizard_EX2 = new Pokemon("charizard EX", "fire", 180, 2, "water", 2, crimson_storm);
            Pokemon heatmor1 = new Pokemon("heatmor", "fire", 80, 1, "water", 0, sharp_scythe);
            Pokemon heatmor2 = new Pokemon("heatmor", "fire", 80, 1, "water", 0, sharp_scythe);
            Pokemon houndour1 = new Pokemon("houndour", "fire", 60, 1, "water", 0, ram);
            Pokemon houndour2 = new Pokemon("houndour", "fire", 60, 1, "water", 0, ram);
            Pokemon houndoom1 = new Pokemon("houndoom", "fire", 100, 2, "water", 1, corner);
            Pokemon houndoom2 = new Pokemon("houndoom", "fire", 100, 2, "water", 1, corner);
            Pokemon magmar1 = new Pokemon("magmar", "fire", 70, 1, "water", 0, ram);
            Pokemon magmar2 = new Pokemon("magmar", "fire", 70, 1, "water", 0, ram);
            Pokemon magmortar1 = new Pokemon("magmortar", "fire", 120, 3, "water", 1, smoke_bomb);
            Pokemon magmortar2 = new Pokemon("magmortar", "fire", 120, 3, "water", 1, smoke_bomb);
            Pokemon fire_tauros1 = new Pokemon("fire tauros", "fire", 110, 2, "water", 0, horn_throw);
            Pokemon fire_tauros2 = new Pokemon("fire tauros", "fire", 110, 2, "water", 0, horn_throw);
            Pokemon oricorio = new Pokemon("oricorio", "fire", 70, 1, "water", 0, pound);
            Pokemon oricorio2 = new Pokemon("oricorio", "fire", 70, 1, "water", 0, pound);

            Pokemon staryu1 = new Pokemon("staryu", "water", 50, 1, "grass", 0, ram);
            Pokemon staryu2 = new Pokemon("staryu", "water", 50, 1, "grass", 0, ram);
            Pokemon starmie = new Pokemon("starmie", "water", 90, 1, "grass", 1, splash);
            Pokemon starmie_EX = new Pokemon("starmie EX", "water", 130, 0, "grass", 1, hydro_splash);
            Pokemon articuno_EX1 = new Pokemon("articuno EX", "water", 150, 2, "grass", 0, blizzard);
            Pokemon articuno_EX2 = new Pokemon("articuno EX", "water", 150, 2, "grass", 0, blizzard);
            Pokemon magikarp = new Pokemon("magikarp", "water", 30, 1, "grass", 0, ram);
            Pokemon gyarados_EX = new Pokemon("gyarados EX", "water", 180, 3, "grass", 1, whirlpool);
            Pokemon buizel1 = new Pokemon("buizel", "water", 60, 1, "grass", 0, ram);
            Pokemon buizel2 = new Pokemon("buizel", "water", 60, 1, "grass", 0, ram);
            Pokemon floatzel1 = new Pokemon("floatzel", "water", 90, 1, "grass", 1, sharp_scythe);
            Pokemon floatzel2 = new Pokemon("floatzel", "water", 90, 1, "grass", 1, sharp_scythe);
            Pokemon tentacool1 = new Pokemon("tentacool", "water", 70, 1, "grass", 0, ram);
            Pokemon tentacool2 = new Pokemon("tentacool", "water", 70, 1, "grass", 0, ram);
            Pokemon tentacruel1 = new Pokemon("tentacruel", "water", 100, 1, "grass", 1, splash);
            Pokemon tentacruel2 = new Pokemon("tentacruel", "water", 100, 1, "grass", 1, splash);
            Pokemon bruxish1 = new Pokemon("bruxish", "water", 80, 1, "grass", 0, pound);
            Pokemon bruxish2 = new Pokemon("bruxish", "water", 80, 1, "grass", 0, pound);
            Pokemon lapras1 = new Pokemon("lapras", "water", 110, 2, "grass", 0, smoke_bomb);
            Pokemon lapras2 = new Pokemon("lapras", "water", 110, 2, "grass", 0, smoke_bomb);

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
                    deckchooser = rng.Next(deckchooser);
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
            List<Pokemon> enemyhand = new List <Pokemon>();
            string turn = "player";
            bool gtg = false;
            while (gtg == false)
            {               //player loop
                hand.Clear();
                cardrawer = rng.Next(pcardsleft);
                hand.Add(DrawCard(decklist, cardrawer, rng));
                cardrawer = rng.Next(pcardsleft);
                hand.Add(DrawCard(decklist, cardrawer, rng));
                cardrawer = rng.Next(pcardsleft);
                hand.Add(DrawCard(decklist, cardrawer, rng));
                cardrawer = rng.Next(pcardsleft);
                hand.Add(DrawCard(decklist, cardrawer, rng));
                cardrawer = rng.Next(pcardsleft);
                hand.Add(DrawCard(decklist, cardrawer, rng));
                cardrawer = rng.Next(pcardsleft); 
                if (hand.Any(p => p.stage == 0))
                {
                    gtg = true;
                }
                for (int i = 0; i < hand.Count; i++)
                {
                    if (hand[i] == null)
                    {
                        hand.RemoveAt(i);
                    }
                }
            }
            gtg = false;
            while (gtg == false)
            {               //enemy loop
                enemyhand.Clear();
                cardrawer = rng.Next(ecardsleft);
                rng = new Random();
                enemyhand.Add(DrawCard(enemylist, cardrawer, rng));
                cardrawer = rng.Next(ecardsleft);
                rng = new Random();
                enemyhand.Add(DrawCard(enemylist, cardrawer, rng));
                cardrawer = rng.Next(ecardsleft);
                rng = new Random();
                enemyhand.Add(DrawCard(enemylist, cardrawer, rng));
                cardrawer = rng.Next(ecardsleft);
                rng = new Random();
                enemyhand.Add(DrawCard(enemylist, cardrawer, rng));
                cardrawer = rng.Next(ecardsleft);
                rng = new Random();
                enemyhand.Add(DrawCard(enemylist, cardrawer, rng));
                cardrawer = rng.Next(ecardsleft);
                rng = new Random();
                if (enemyhand.Any(p => p.stage == 0))
                {
                    gtg = true;
                }
                for (int i = 0; i < enemyhand.Count; i++)
                {
                    if (enemyhand[i] == null)
                    {
                        enemyhand.RemoveAt(i);
                    }
                }
            }
            bool gamend = false;
            while(gamend == false)
            {
                writescreen(enemyhand, epoints);
                if (turn =="player")
                {
                    Console.WriteLine("play a card for your active spot.");
                    Console.Write("Hand: ");
                    for (int i = 0; i <=hand.Count; i++)
                    {
                        Console.Write(hand[i]+ ", ");
                    }
                }
            }
        }
    }
}
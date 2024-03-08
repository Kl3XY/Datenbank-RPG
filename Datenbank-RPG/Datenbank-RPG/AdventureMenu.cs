using ConsoleTables;
using Datenbank;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datenbank_RPG
{
    public class AdventureMenu
    {
        public static int menuSelect = 0;
        public static int turns = 0;
        public static bool loopSection = true;

        public static void Menu()
        {
            while (loopSection)
            {
                switch (menuSelect)
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }

                Console.WriteLine("You are at a camp, rest for now.");
                SQL.drawPlayerList();

                Console.WriteLine(" .. ........... .............  ........... . ..... ........ .......\r\n ......  ....................%.... .... ..... .........%............\r\n .@@@ ........ @@.... @@@@  . ............................  *  .....\r\n ....@@ ..... @ .... @ .............   ....... .....; .... *** .....\r\n .....\\@\\....@ .... @ ............................. #  .. *****  ...\r\n  @@@.. @@@@@  @@@@@@___.. ....... ...%..... ...  {###}  *******\r\n ....@-@..@ ..@......@@@\\...... %...... ....... <## ####>********\r\n   @@@@\\...@ @ ........\\@@@@ ..... ...... ....... {###}***********\r\n ....%..@  @@ /@@@@@ . ....... ...............<###########> *******\r\n ...... .@-@@@@ ...V......     .... %.......... {#######}******* ***\r\n ...... .  @@ .. ..v.. .. . { } ............<###############>*******\r\n ......... @@ .... ........ {^^,     .......   {## ######}***** ****\r\n ..%..... @@ .. .%.... . .. (   `-;   ... <###################> ****\r\n . .... . @@ . .... .. _  .. `;;~~ ......... {#############}********\r\n .... ... @@ ... ..   /(______); .. ....<################  #####>***\r\n . .... ..@@@ ...... (         (  .........{##################}*****\r\n ......... @@@  ....  |:------( )  .. <##########################>**\r\n  @@@@ ....@@@  ... _// ...... \\\\ ...... {###   ##############}*****\r\n @@@@@@@  @@@@@ .. / /@@@@@@@@@ vv  <##############################>\r\n @@@@@@@ @@@@@@@ @@@@@@@@@@@@@@@@@@@ ..... @@@@@@  @@@@@@@  @@@@\r\n @@@@@@###@@@@@### @@@@@@@@@@@@@@@@@@ @@@@@@@@@@@@@@@@@@@@@@@@@@@@@\r\n @@@@@@@@###@##@@ @@@@@@@@@@@@@@@@@@@@@ @@@@@   @@@@@@@@@@@@@@@@@@@\r\n @@@@@@@@@@@### @@@@@@@@@@@@@@@@@@@@@@@@@@ @@@@@@@@@@@@@@@@@@@@@@@@\r\n -@@@@@@@@@#####@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    

                ConsoleTable menu = new ConsoleTable();
                menu.Options.EnableCount = false;

                var menuOptions = new string[] { "Easy (25 Turns)", "Normal (50 Turns)", "Hard (100 Turns)", "Brutal (250 Turns)" };

                for (var i = 0; i < 4; i++)
                {
                    if (i == menuSelect)
                    {
                        menuOptions[i] = ">" + menuOptions[i] + "<";
                    }
                }
                menu.AddColumn(menuOptions);
                menu.Write();
                Console.WriteLine("Use the left and right arrow key to traverse the menu. (Press ESC to exit)");

                var key = Console.ReadKey().Key;

                if (key.ToString() == "RightArrow")
                {
                    if (menuSelect++ > 2) { menuSelect = 0; }
                }
                if (key.ToString() == "LeftArrow")
                {
                    if (menuSelect-- < 1) { menuSelect = 2; }
                }
                if (key.ToString() == "Escape")
                {
                    loopSection = false;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (key.ToString() == "Spacebar")
                {
                    switch (menuSelect)
                    {
                        case 0:
                            turns = 25;
                            break;
                        case 1:
                            turns = 50;
                            break;
                        case 2:
                            turns = 100;
                            break;
                        case 3:
                            turns = 250;
                            break;
                    }
                    loopSection = false;
                    Game();
                }
                Console.Clear();
                Program.players.Clear();
            }
        }

        public static void Game()
        {
            var scenery = "";
            scenery = "";
            for (var i = 0; i < turns; i++)
            {
                var sym = ".";
                var rnd = new Random();
                if (rnd.Next(100) < 5) { sym = "¥"; }
                if (rnd.Next(100) < 10) { sym = "¶"; }
                scenery += sym;
            }


            while (turns > 0)
            {
                Console.WriteLine("Your party is traveling! (Turns Left: {0})", turns);
                SQL.drawPlayerList();

                scenery = scenery.Substring(1);

                if (scenery.Length > 0)
                {
                    switch (scenery[0])
                    {
                        case '¥':
                            Console.WriteLine("You've entered a camp. Do you want to check your inventory? [y/n]");
                            if (Console.ReadKey().Key.ToString() == "Y")
                            {
                                Console.Clear();
                                Inventory.loopSection = true;
                                Inventory.Menu();
                            };

                            break;
                        case '¶':
                            var choose = new Random().Next(0, Program.players.Count-1);
                            Combat_Screen.idOfChosenPlayer = Program.players[choose].Id;
                            Combat_Screen.loopSection = true;
                            Console.Clear();
                            SQL.chooseEnemy();

                            Console.WriteLine("{0} has encountered a {1}!", Program.players[choose].Name, Combat_Screen.enemies.Find(d => d.Id == Combat_Screen.idOfChosenEnemy).Name);
                            Combat_Screen.Menu();
                            break;
                    }
                }

                var partyMember = "";
                
                foreach (Player plr in Program.players)
                {
                    partyMember += plr.Name[0] + ".";
                }
                Console.WriteLine(partyMember + scenery);

                Thread.Sleep(500);
                turns--;
                Program.players.Clear();
                Console.Clear();
            }
        }
    }
}

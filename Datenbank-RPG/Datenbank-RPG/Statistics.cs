using ConsoleTables;
using Datenbank;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Datenbank_RPG
{
    public class Statistics
    {
        public static int menuSelect = 0;
        public static int turns = 0;
        public static bool loopSection = true;
        public static int generalGoldAmount = 0;
        public static int selectedItemId = -1;
        public static int selectedItemList = -1;
        public static bool in_submenu = true;
        public static string search = "";
        public static string lastKey = "";



        public static void Menu()
        {
            while (loopSection)
            {
                Console.WriteLine("The Statistics of all current and past heroes.");
                Console.WriteLine("Enter the name of the character you want to find!");
                Console.WriteLine("Use the up and down keys to traverse the menu. And enter to Confirm. (Press ESC to exit)");
                SQL.drawPlayerListSelectStatistics(search);

                Console.WriteLine("Searching: {0}", search);
                var searchString = "";
                var key = Console.ReadKey().Key;

                if (key.ToString() == "DownArrow")
                {
                    if (menuSelect++ > Program.players.Count - 2) { menuSelect = 0; }
                }
                if (key.ToString() == "UpArrow")
                {
                    if (menuSelect-- < 1) { menuSelect = Program.players.Count - 1; }
                }

                if (key.ToString() == "Enter")
                {
                    var chosenPerson = Program.players[menuSelect];
                    Console.WriteLine("You have chosen {0}(Id: {1})", chosenPerson.Name, chosenPerson.Id);
                    Console.Clear();
                    in_submenu = true;
                    Submenu(chosenPerson.Id);
                }

                if (key.ToString() == "Escape")
                {
                    loopSection = false;
                }

                switch(key.ToString())
                {
                    case "Spacebar":
                        search += " ";
                        if (menuSelect > Program.players.Count - 2) { menuSelect = 0; }
                        break;
                    case "LeftArrow":
                        break;
                    case "DownArrow":
                        break;
                    case "Enter":
                        break;
                    case "UpArrow":
                        break;
                    case "Tab":
                        break;
                    case "RightArrow":
                        break;
                    case "D1":
                        search += "1";
                        break;
                    case "D2":
                        search += "2";
                        break;
                    case "D3":
                        search += "3";
                        break;
                    case "D4":
                        search += "4";
                        break;
                    case "D5":
                        search += "5";
                        break;
                    case "D6":
                        search += "6";
                        break;
                    case "D7":
                        search += "7";
                        break;
                    case "D8":
                        search += "8";
                        break;
                    case "D9":
                        search += "9";
                        break;
                    case "Backspace":
                        if (search.Length > 0)
                            search = search.Substring(0, search.Length - 1);
                        break;
                    default:
                        search += key.ToString()[0];
                        if (menuSelect > Program.players.Count - 2) { menuSelect = 0; }
                        break;
                }
                lastKey = key.ToString();
                Console.Clear();
                Program.players.Clear();
                Inventory.items.Clear();
            }
        }

        public static void Submenu(int chosenPerson)
        {
            while (in_submenu)
            {
                Console.WriteLine("are or were they the one to save the world? who knows.");
                SQL.drawPlayerList(chosenPerson);

                ConsoleTable menu = new ConsoleTable();
                menu.Options.EnableCount = false;

                var menuOptions = new string[] { "Kills", "Arch Nemesis", "Hall of Riches (Global)", "Hall of Belongings (Global)", "Hall of Shame (Enemies)", "Hall of Shame (Players)"};

                for (var i = 0; i < menuOptions.Length; i++)
                {
                    if (i == menuSelect)
                    {
                        menuOptions[i] = ">" + menuOptions[i] + "<";
                    }
                }
                menu.AddColumn(menuOptions);
                menu.Write();
                Console.WriteLine("Use the left and right arrow key to traverse the menu. And enter to confirm (Press ESC to exit.)");

                var key = Console.ReadKey().Key;

                if (key.ToString() == "RightArrow")
                {
                    if (menuSelect++ > menuOptions.Length - 2) { menuSelect = 0; }
                }
                if (key.ToString() == "LeftArrow")
                {
                    if (menuSelect-- < 1) { menuSelect = menuOptions.Length - 1; }
                }

                if (key.ToString() == "Enter")
                {
                    switch(menuSelect)
                    {
                        case 0:
                            var enemyGraveCMD = prepared_statement.getStatement("displayEnemyGraveyard");
                            enemyGraveCMD.Parameters[0].Value = chosenPerson;
                            SQL.queryDraw(enemyGraveCMD);
                            Console.ReadKey();
                            break;
                        case 1:
                            var playerGraveCMD = prepared_statement.getStatement("displayPlayerGraveyard");
                            playerGraveCMD.Parameters[0].Value = chosenPerson;
                            SQL.queryDraw(playerGraveCMD);
                            Console.ReadKey();
                            break;
                        case 2:
                            SQL.displayInventorySortedByAmount();
                            Console.ReadKey();
                            break;
                        case 3:
                            SQL.drawPlayerListSortedByGold();
                            Console.ReadKey();
                            break;
                        case 4:
                            SQL.queryDraw(new SqlCommand("exec mostKilledEnemy", Program.connection));
                            Console.ReadKey();
                            break;
                        case 5:
                            SQL.queryDraw(new SqlCommand("exec mostKilledPlayer", Program.connection));
                            Console.ReadKey();
                            break;
                    }
                }

                if (key.ToString() == "Escape")
                {
                    in_submenu = false;
                }

                lastKey = key.ToString();
                Console.Clear();
                Program.players.Clear();
                Inventory.items.Clear();
            }
        }
    }
}

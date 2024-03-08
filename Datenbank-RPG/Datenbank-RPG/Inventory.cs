using Datenbank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Datenbank_RPG
{
    public class Inventory
    {
        public static int menuSelect = 0;
        public static int turns = 0;
        public static bool loopSection = true;
        public static List<Item> items = new();
        public static int generalGoldAmount = 0;
        public static int selectedItemId = -1;
        public static int selectedItemList = -1;
        public static bool in_submenu = true;


        public static void Menu()
        {
            while (loopSection)
            {
                Console.WriteLine("Your Inventory. You know all its secrets.");
                SQL.drawPlayerList();
                SQL.displayInventory();

                Console.WriteLine("Use the up and down to traverse the menu.  (Press ESC to exit)");
                Console.WriteLine("You currently have {0} Gold in your group.", generalGoldAmount);

                var key = Console.ReadKey().Key;

                if (key.ToString() == "DownArrow")
                {
                    if (menuSelect++ > items.Count - 2) { menuSelect = 0; }
                }
                if (key.ToString() == "UpArrow")
                {
                    if (menuSelect-- < 1) { menuSelect = items.Count - 1; }
                }
                if (key.ToString() == "Spacebar")
                {
                    in_submenu = true;
                    selectedItemId = items[menuSelect].Id;
                    selectedItemList = menuSelect;
                    menuSelect = 0;


                    while (in_submenu)
                    {
                        Console.Clear();
                        Console.WriteLine("Select who gets the {0}", items[menuSelect].Name);
                        SQL.drawPlayerListSelectInventory();

                        key = Console.ReadKey().Key;

                        if (key.ToString() == "DownArrow")
                        {
                            if (menuSelect++ > items.Count - 2) { menuSelect = 0; }
                        }
                        if (key.ToString() == "UpArrow")
                        {
                            if (menuSelect-- < 1) { menuSelect = items.Count - 1; }
                        }

                        if (key.ToString() == "Spacebar")
                        {
                            switch (items[menuSelect].ItemType)
                            {
                                case "Health Potion":
                                    if (Program.players[menuSelect].Life > 0)
                                    {
                                        var cmd = prepared_statement.getStatement("setPlayerHealth");
                                        cmd.Parameters[0].Value = Program.players[menuSelect].Id;
                                        cmd.Parameters[1].Value = Math.Min(Program.players[menuSelect].Life + items[menuSelect].ItemPower/3, Program.players[menuSelect].MaxLife);
                                        cmd.ExecuteNonQuery();

                                        var cmdUseItem = prepared_statement.getStatement("useItem");
                                        cmdUseItem.Parameters[0].Value = selectedItemId;
                                        cmdUseItem.ExecuteNonQuery();
                                        in_submenu = false;
                                    } else
                                    {
                                        Console.WriteLine("Character is dead");
                                        Console.ReadKey();
                                    }
                                    break;
                                case "Revive Item":
                                    if (Program.players[menuSelect].Life <= 0)
                                    {
                                        var cmd = prepared_statement.getStatement("setPlayerHealth");
                                        cmd.Parameters[0].Value = Program.players[menuSelect].Id;
                                        cmd.Parameters[1].Value = Program.players[menuSelect].MaxLife;
                                        cmd.ExecuteNonQuery();

                                        var cmdUseItem = prepared_statement.getStatement("useItem");
                                        cmdUseItem.Parameters[0].Value = selectedItemId;
                                        cmdUseItem.ExecuteNonQuery();
                                        in_submenu = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Character doesn't need");
                                        Console.ReadKey();
                                    }
                                    break;
                            }
                        }

                        if (key.ToString() == "Escape")
                        {
                            in_submenu = false;
                        }
                    }
                }
                if (key.ToString() == "Escape")
                {
                    loopSection = false;
                }

                Console.Clear();
                Program.players.Clear();
                Inventory.items.Clear();
            }
        }
    }
}

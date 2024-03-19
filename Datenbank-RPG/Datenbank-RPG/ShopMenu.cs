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
    public class ShopMenu
    {
        public static int menuSelect = 0;
        public static int turns = 0;
        public static bool loopSection = true;
        public static List<sql.Item> items = new();
        public static int generalGoldAmount = 0;

        public static void Menu()
        {
            while (loopSection)
            {
                Console.WriteLine("A Shop! A place where you can buy questionably sourced items for a questionably high price!");
                SQL.drawPlayerList();
                SQL.initShop();

                Console.WriteLine("Use the up and down to traverse the menu. And enter to Confirm. (Press ESC to exit)");
                Console.WriteLine("You currently have {0} Gold in your group.", generalGoldAmount);

                var key = Console.ReadKey().Key;

                if (key.ToString() == "DownArrow")
                {
                    if (menuSelect++ > items.Count - 2) { menuSelect = 0; }
                }
                if (key.ToString() == "UpArrow")
                {
                    if (menuSelect-- < 1) { menuSelect = items.Count-1; }
                }
                if (key.ToString() == "Enter")
                {
                    if (items[menuSelect].Gold <= generalGoldAmount)
                    {
                        updateMoney(items[menuSelect].Id, items[menuSelect].Gold);

                        var cmd = prepared_statement.getStatement("addItem");
                        cmd.Parameters[0].Value = items[menuSelect].Id;
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("Added {0} to the inventory!", items[menuSelect].Name);
                        Console.ReadKey();
                    } else
                    {
                        Console.WriteLine("Sorry, i don't give credit. Come back when you are a little bit... Richer.");
                        Console.ReadKey();
                    }
                }
                if (key.ToString() == "Escape")
                {
                    loopSection = false;
                }

                Console.Clear();
                Program.players.Clear();
                ShopMenu.items.Clear();
            }
        }
        public static void updateMoney(int itemID, int cost)
        {
            var cmd = prepared_statement.getStatement("buyItem");
            while (cost > 0)
            {
                foreach (sql.Player player in Program.players)
                {
                    cmd.Parameters[0].Value = player.Id;
                    cmd.Parameters[1].Value = itemID;
                    cmd.Parameters[2].Value = player.Gold - Math.Min(cost, player.Gold);
                    cmd.ExecuteNonQuery();

                    cost -= Math.Min(cost, player.Gold);
                }
            }
        }
    }
}

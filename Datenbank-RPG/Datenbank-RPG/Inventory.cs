using Datenbank;
using System;
using System.Collections.Generic;
using System.Linq;
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

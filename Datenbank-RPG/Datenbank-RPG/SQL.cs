using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Datenbank;

namespace Datenbank_RPG
{
    public static class SQL
    {
        public static DataTable drawPlayerList()
        {
            Program.players.Clear();
            ConsoleTable table = new ConsoleTable("Name", "Life", "Defense", "Gold");
            table.Options.EnableCount = false;
            DataTable data = new DataTable();
            using (SqlCommand cmd = new SqlCommand("select * from player", Program.connection))
            {
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    var basePlayer = new Player(row["name"].ToString(), Convert.ToInt32(row["life"]), Convert.ToInt32(row["defense"]), Convert.ToInt32(row["id"]), Convert.ToInt32(row["classId"]), Convert.ToInt32(row["gold"]), Convert.ToInt32(row["maxlife"]));
                    Program.players.Add(basePlayer);
                }
            }
            ShopMenu.generalGoldAmount = 0;
            foreach (Player player in Program.players)
            {
                ShopMenu.generalGoldAmount += player.Gold;
                table.AddRow(player.Name, $"{player.Life}/{player.MaxLife}", player.Defense, player.Gold);
            }

            table.Write();

            return data;
        }

        public static DataTable initShop()
        {
            ConsoleTable table = new ConsoleTable("Name", "ItemType", "Power", "Cost");
            table.Options.EnableCount = false;
            DataTable data = new DataTable();
            using (var cmd = prepared_statement.getStatement("listItems"))
            {
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    var baseItem = new Item(Convert.ToInt32(row["id"]), row["name"].ToString(), row["itemTypeName"].ToString(), Convert.ToInt32(row["itemPower"]), Convert.ToInt32(row["gold"]));
                    ShopMenu.items.Add(baseItem);
                }
            }
            
            for(var i = 0; i < ShopMenu.items.Count; i++)
            {
                var item = ShopMenu.items[i];
                if (i == ShopMenu.menuSelect)
                {
                    table.AddRow(">" + item.Name, item.ItemType, item.ItemPower, item.Gold + "<");
                } else
                {
                    table.AddRow(item.Name, item.ItemType, item.ItemPower, item.Gold);
                }
            }

            table.Write();

            return data;
        }

        public static DataTable displayInventory()
        {
            ConsoleTable table = new ConsoleTable("Name", "ItemType", "Power", "Amount");
            table.Options.EnableCount = false;
            DataTable data = new DataTable();
            using (var cmd = prepared_statement.getStatement("displayInventory"))
            {
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    var baseItem = new Item(Convert.ToInt32(row["id"]), row["name"].ToString(), row["itemname"].ToString(), Convert.ToInt32(row["itemPower"]), Convert.ToInt32(row["gold"]), Convert.ToInt32(row["amount"]));
                    Inventory.items.Add(baseItem);
                }
            }

            for (var i = 0; i < Inventory.items.Count; i++)
            {
                var item = Inventory.items[i];
                if (i == Inventory.menuSelect)
                {
                    table.AddRow(">" + item.Name, item.ItemType, item.ItemPower, item.amount + "<");
                }
                else
                {
                    table.AddRow(item.Name, item.ItemType, item.ItemPower, item.amount);
                }
            }

            table.Write();

            return data;
        }
    }
}

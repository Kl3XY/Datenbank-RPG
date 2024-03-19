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
using System.Numerics;
using sql;

namespace Datenbank_RPG
{
    public static class SQL
    {
        public static void drawPlayerList(int id = -1)
        {
            Program.players.Clear();
            ConsoleTable table = new ConsoleTable("Name", "Class", "Life", "Attack", "Defense", "Gold");
            table.Options.EnableCount = false;
            SqlCommand cmd = prepared_statement.getStatement("displayPlayers");
            cmd.Parameters[0].Value = id;
            Program.players = cmds.GetPlayers(cmd);

            ShopMenu.generalGoldAmount = 0;
            foreach (Player player in Program.players)
            {
                ShopMenu.generalGoldAmount += player.Gold;
                table.AddRow(player.Name, player.playerClassName, $"{player.Life}/{player.MaxLife}", player.Attack, player.Defense, player.Gold);
            }

            table.Write();
        }

        public static void drawPlayerListSortedByGold()
        {
            Program.players.Clear();
            ConsoleTable table = new ConsoleTable("Name", "Class", "Life", "Attack", "Defense", "Gold");
            table.Options.EnableCount = false;
            SqlCommand cmd = prepared_statement.getStatement("sortPlayerByGold");
            Program.players = cmds.GetPlayers(cmd);

            ShopMenu.generalGoldAmount = 0;
            foreach (Player player in Program.players)
            {
                ShopMenu.generalGoldAmount += player.Gold;
                table.AddRow(player.Name, player.playerClassName, $"{player.Life}/{player.MaxLife}", player.Attack, player.Defense, player.Gold);
            }

            table.Write();
        }

        public static void drawPlayerListSelectInventory(int id = -1)
        {
            Program.players.Clear();
            ConsoleTable table = new ConsoleTable("Name", "Class", "Life", "Attack", "Defense", "Gold");
            table.Options.EnableCount = false;
            SqlCommand cmd = prepared_statement.getStatement("displayPlayers");
            cmd.Parameters[0].Value = id;

            Program.players = cmds.GetPlayers(cmd);

            for (var i = 0; i < Program.players.Count; i++)
            {
                var item = Program.players[i];
                if (i == Inventory.menuSelect)
                {
                    table.AddRow(">" + item.Name + "<", ">" + item.playerClassName + "<", ">" + $"{item.Life}/{item.MaxLife}" + "<", ">" + item.Attack + "<", ">" + item.Defense + "<", item.Gold + "<");
                }
                else
                {
                    table.AddRow(item.Name, item.playerClassName, $"{item.Life}/{item.MaxLife}", item.Attack, item.Defense, item.Gold);
                }
            }

            table.Write();
        }

        public static void drawPlayerListSelectStatistics(string search = "")
        {
            Program.players.Clear();
            ConsoleTable table = new ConsoleTable("Name", "Class", "Life", "Attack", "Defense", "Gold");
            table.Options.EnableCount = false;
            SqlCommand cmd = prepared_statement.getStatement("searchPlayer");
            cmd.Parameters[0].Value = search;
            Program.players = cmds.GetPlayers(cmd);

            for (var i = 0; i < Program.players.Count; i++)
            {
                var item = Program.players[i];
                if (i == Statistics.menuSelect)
                {
                    table.AddRow(">" + item.Name + "<", ">" + item.playerClassName + "<", ">" + $"{item.Life}/{item.MaxLife}" + "<", ">" + item.Attack + "<", ">" + item.Defense + "<", ">" + item.Gold + "<");
                }
                else
                {
                    table.AddRow(item.Name, item.playerClassName, $"{item.Life}/{item.MaxLife}", item.Attack, item.Defense, item.Gold);
                }
            }

            table.Write();
        }
        public static void initShop()
        {
            ConsoleTable table = new ConsoleTable("Name", "ItemType", "Power", "Cost");
            table.Options.EnableCount = false;
            var cmd = prepared_statement.getStatement("listItems");
            ShopMenu.items = sql.cmds.GetItems(cmd);
            
            
            for(var i = 0; i < ShopMenu.items.Count; i++)
            {
                var item = ShopMenu.items[i];
                if (i == ShopMenu.menuSelect)
                {
                    table.AddRow(">" + item.Name, ">" + item.ItemType + "<", ">" + item.ItemPower + "<", ">" + item.Gold + "<");
                } else
                {
                    table.AddRow(item.Name, item.ItemType, item.ItemPower, item.Gold);
                }
            }

            table.Write();
        }

        public static void drawEnemyList()
        {
            Combat_Screen.enemies.Clear();
            ConsoleTable table = new ConsoleTable("Name", "Life");
            table.Options.EnableCount = false;
            SqlCommand cmd = prepared_statement.getStatement("displayEnemy");
            cmd.Parameters[0].Value = Combat_Screen.idOfChosenEnemy;

            Combat_Screen.enemies = cmds.GetEnemies(cmd);

            for (var i = 0; i < Combat_Screen.enemies.Count; i++)
            {
                var item = Combat_Screen.enemies[i];
                table.AddRow(item.Name, $"{item.Life}/{item.maxLife}");
            }

            table.Write();
        }

        public static void chooseEnemy()
        {
            Combat_Screen.enemies.Clear();
            SqlCommand cmd = prepared_statement.getStatement("displayEnemy");
            cmd.Parameters[0].Value = -1;

            Combat_Screen.enemies = cmds.GetEnemies(cmd);
            var rnd = Program.rng.Next(0, Combat_Screen.enemies.Count-1);
            Combat_Screen.idOfChosenEnemy = Combat_Screen.enemies[rnd].Id;
        }

        public static void displayInventory()
        {
            ConsoleTable table = new ConsoleTable("Name", "ItemType", "Power", "Amount");
            table.Options.EnableCount = false;
            var cmd = prepared_statement.getStatement("displayInventory");
            Inventory.items = cmds.GetItems(cmd);
 
            for (var i = 0; i < Inventory.items.Count; i++)
            {
                var item = Inventory.items[i];
                if (i == Inventory.menuSelect)
                {
                    table.AddRow(">" + item.Name + "<", ">" + item.ItemType + "<", ">" + item.ItemPower + "<", item.amount + "<");
                }
                else
                {
                    table.AddRow(item.Name, item.ItemType, item.ItemPower, item.amount);
                }
            }

            table.Write();

        }

        public static void displayInventorySortedByAmount()
        {
            ConsoleTable table = new ConsoleTable("Name", "ItemType", "Amount");
            table.Options.EnableCount = false;
            var cmd = prepared_statement.getStatement("sortInventoryByAmount");
            Inventory.items = cmds.GetItems(cmd);

            for (var i = 0; i < Inventory.items.Count; i++)
            {
                var item = Inventory.items[i];
                table.AddRow(item.Name, item.ItemType, item.amount);
            }

            table.Write();
        }

        public static DataSet queryDraw(SqlCommand cmd)
        {
            using (cmd)
            {
                var Dt = new DataSet();
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(Dt);
                foreach (DataTable dt in Dt.Tables)
                {
                    var resultTable = new ConsoleTable();
                    resultTable.Options.EnableCount = false;

                    var collist = new List<string>();
                    var rowlist = new string[dt.Columns.Count];

                    for (var i = 0; i < rowlist.Length; i++)
                        rowlist[i] = "";

                    foreach (DataColumn col in dt.Columns) { collist.Add(col.ColumnName); } //Get & Set Column Names
                    resultTable.AddColumn(collist);

                    foreach (DataRow row in dt.Rows)
                    {
                        rowlist = new string[dt.Columns.Count];
                        for (var i = 0; i < rowlist.Length; i++)
                            rowlist[i] = "";

                        foreach (DataColumn col in dt.Columns)
                        {
                            for (var i = 0; i < rowlist.Length; i++)
                                if (rowlist[i] == "") { rowlist[i] = row[col].ToString(); break; };
                        }

                        resultTable.AddRow(rowlist);
                    }

                    resultTable.Write();
                    Console.WriteLine();
                }
                return Dt;
            }
        }
    }
}

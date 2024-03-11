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

namespace Datenbank_RPG
{
    public static class SQL
    {
        public static DataTable drawPlayerList(int id = -1)
        {
            Program.players.Clear();
            ConsoleTable table = new ConsoleTable("Name", "Class", "Life", "Attack", "Defense", "Gold");
            table.Options.EnableCount = false;
            DataTable data = new DataTable();
            SqlCommand cmd = prepared_statement.getStatement("displayPlayers");
            cmd.Parameters[0].Value = id;

            using (cmd)
            {
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    var basePlayer = new Player(row["name"].ToString(), Convert.ToInt32(row["life"]), Convert.ToInt32(row["defense"]), Convert.ToInt32(row["id"]), Convert.ToInt32(row["classId"]), Convert.ToInt32(row["gold"]), Convert.ToInt32(row["maxlife"]), Convert.ToInt32(row["attack"]), Convert.ToInt32(row["attackDelay"]), row["className"].ToString());
                    Program.players.Add(basePlayer);
                }
            }
            ShopMenu.generalGoldAmount = 0;
            foreach (Player player in Program.players)
            {
                ShopMenu.generalGoldAmount += player.Gold;
                table.AddRow(player.Name, player.playerClassName, $"{player.Life}/{player.MaxLife}", player.Attack, player.Defense, player.Gold);
            }

            table.Write();

            return data;
        }

        public static DataTable drawPlayerListSortedByGold()
        {
            Program.players.Clear();
            ConsoleTable table = new ConsoleTable("Name", "Class", "Life", "Attack", "Defense", "Gold");
            table.Options.EnableCount = false;
            DataTable data = new DataTable();
            SqlCommand cmd = prepared_statement.getStatement("sortPlayerByGold");
            using (cmd)
            {
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    var basePlayer = new Player(row["name"].ToString(), Convert.ToInt32(row["life"]), Convert.ToInt32(row["defense"]), Convert.ToInt32(row["id"]), Convert.ToInt32(row["classId"]), Convert.ToInt32(row["gold"]), Convert.ToInt32(row["maxlife"]), Convert.ToInt32(row["attack"]), Convert.ToInt32(row["attackDelay"]), row["className"].ToString());
                    Program.players.Add(basePlayer);
                }
            }
            ShopMenu.generalGoldAmount = 0;
            foreach (Player player in Program.players)
            {
                ShopMenu.generalGoldAmount += player.Gold;
                table.AddRow(player.Name, player.playerClassName, $"{player.Life}/{player.MaxLife}", player.Attack, player.Defense, player.Gold);
            }

            table.Write();

            return data;
        }

        public static DataTable drawPlayerListSelectInventory(int id = -1)
        {
            Program.players.Clear();
            ConsoleTable table = new ConsoleTable("Name", "Class", "Life", "Attack", "Defense", "Gold");
            table.Options.EnableCount = false;
            DataTable data = new DataTable();
            SqlCommand cmd = prepared_statement.getStatement("displayPlayers");
            cmd.Parameters[0].Value = id;

            using (cmd)
            {
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    var basePlayer = new Player(row["name"].ToString(), Convert.ToInt32(row["life"]), Convert.ToInt32(row["defense"]), Convert.ToInt32(row["id"]), Convert.ToInt32(row["classId"]), Convert.ToInt32(row["gold"]), Convert.ToInt32(row["maxlife"]), Convert.ToInt32(row["attack"]), Convert.ToInt32(row["attackDelay"]), row["className"].ToString());
                    Program.players.Add(basePlayer);
                }
            }

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

            
            return data;
        }

        public static DataTable drawPlayerListSelectStatistics(string search = "")
        {
            Program.players.Clear();
            ConsoleTable table = new ConsoleTable("Name", "Class", "Life", "Attack", "Defense", "Gold");
            table.Options.EnableCount = false;
            DataTable data = new DataTable();
            SqlCommand cmd = prepared_statement.getStatement("searchPlayer");
            cmd.Parameters[0].Value = search;

            using (cmd)
            {
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    var basePlayer = new Player(row["name"].ToString(), Convert.ToInt32(row["life"]), Convert.ToInt32(row["defense"]), Convert.ToInt32(row["id"]), Convert.ToInt32(row["classId"]), Convert.ToInt32(row["gold"]), Convert.ToInt32(row["maxlife"]), Convert.ToInt32(row["attack"]), Convert.ToInt32(row["attackDelay"]), row["className"].ToString());
                    Program.players.Add(basePlayer);
                }
            }

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
                    table.AddRow(">" + item.Name, ">" + item.ItemType + "<", ">" + item.ItemPower + "<", ">" + item.Gold + "<");
                } else
                {
                    table.AddRow(item.Name, item.ItemType, item.ItemPower, item.Gold);
                }
            }

            table.Write();

            return data;
        }

        public static DataTable drawEnemyList()
        {
            Combat_Screen.enemies.Clear();
            ConsoleTable table = new ConsoleTable("Name", "Life");
            table.Options.EnableCount = false;
            DataTable data = new DataTable();
            SqlCommand cmd = prepared_statement.getStatement("displayEnemy");
            cmd.Parameters[0].Value = Combat_Screen.idOfChosenEnemy;

            using (cmd)
            {
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    var baseEnemy = new Enemy(Convert.ToInt32(row["id"]), row["name"].ToString(), Convert.ToInt32(row["life"]), Convert.ToInt32(row["defense"]), Convert.ToInt32(row["attack"]), Convert.ToInt32(row["attackDelay"]), Convert.ToInt32(row["maxLife"]));
                    Combat_Screen.enemies.Add(baseEnemy);
                }
            }

            for (var i = 0; i < Combat_Screen.enemies.Count; i++)
            {
                var item = Combat_Screen.enemies[i];
                table.AddRow(item.Name, $"{item.Life}/{item.maxLife}");
            }

            table.Write();

            return data;
        }

        public static DataTable chooseEnemy()
        {
            Combat_Screen.enemies.Clear();
            DataTable data = new DataTable();
            SqlCommand cmd = prepared_statement.getStatement("displayEnemy");
            cmd.Parameters[0].Value = -1;
            var roll = 20;

            using (cmd)
            {
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    var baseEnemy = new Enemy(Convert.ToInt32(row["id"]), row["name"].ToString(), Convert.ToInt32(row["life"]), Convert.ToInt32(row["defense"]), Convert.ToInt32(row["attack"]), Convert.ToInt32(row["attackDelay"]), Convert.ToInt32(row["maxLife"]));
                    Combat_Screen.enemies.Add(baseEnemy);
                }
            }
            var rnd = Program.rng.Next(1, Combat_Screen.enemies.Count);
            Combat_Screen.idOfChosenEnemy = Combat_Screen.enemies[rnd].Id;

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
                    table.AddRow(">" + item.Name + "<", ">" + item.ItemType + "<", ">" + item.ItemPower + "<", item.amount + "<");
                }
                else
                {
                    table.AddRow(item.Name, item.ItemType, item.ItemPower, item.amount);
                }
            }

            table.Write();

            return data;
        }

        public static DataTable displayInventorySortedByAmount()
        {
            ConsoleTable table = new ConsoleTable("Name", "ItemType", "Amount");
            table.Options.EnableCount = false;
            DataTable data = new DataTable();
            using (var cmd = prepared_statement.getStatement("sortInventoryByAmount"))
            {
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    var baseItem = new Item(Convert.ToInt32(row["id"]), row["name"].ToString(), "", 0, 0, Convert.ToInt32(row["amount"]));
                    Inventory.items.Add(baseItem);
                }
            }

            for (var i = 0; i < Inventory.items.Count; i++)
            {
                var item = Inventory.items[i];
                table.AddRow(item.Name, item.ItemType, item.amount);
            }

            table.Write();

            return data;
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

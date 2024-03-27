using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using System.Data.SqlClient;

namespace sql
{
    public static class cmds
    {
        public static DataTable GetDataTable(SqlCommand sqlCommand)
        {
            using (sqlCommand)
            {
                var data = new DataTable();
                var adapter = new SqlDataAdapter(sqlCommand);
                adapter.Fill(data);

                return data;
            }
        }

        public static List<Player> GetPlayers(SqlCommand sqlCommand)
        {
            var data = GetDataTable(sqlCommand);
            var list = new List<Player>();

            foreach (DataRow row in data.Rows)
            {
                var basePlayer = new Player(row["name"].ToString(), Convert.ToInt32(row["life"]), Convert.ToInt32(row["defense"]), Convert.ToInt32(row["id"]), Convert.ToInt32(row["classId"]), Convert.ToInt32(row["gold"]), Convert.ToInt32(row["maxlife"]), Convert.ToInt32(row["attack"]), Convert.ToInt32(row["attackDelay"]), row["className"].ToString());
                list.Add(basePlayer);
            }

            return list;
        }

        public static List<Enemy> GetEnemies(SqlCommand sqlCommand)
        {
            var data = GetDataTable(sqlCommand);
            var list = new List<Enemy>();

            foreach (DataRow row in data.Rows)
            {
                var baseEnemy = new Enemy(Convert.ToInt32(row["id"]), row["name"].ToString(), Convert.ToInt32(row["life"]), Convert.ToInt32(row["defense"]), Convert.ToInt32(row["attack"]), Convert.ToInt32(row["attackDelay"]), Convert.ToInt32(row["maxLife"]), row["type"].ToString());
                list.Add(baseEnemy);
            }

            return list;
        }

        public static List<Item> GetItems(SqlCommand sqlCommand)
        {
            var data = GetDataTable(sqlCommand);
            var list = new List<Item>();

            foreach (DataRow row in data.Rows)
            {
                var baseItem = new Item(Convert.ToInt32(row["id"]), row["name"].ToString(), row["itemname"].ToString(), Convert.ToInt32(row["itemPower"]), Convert.ToInt32(row["gold"]), Convert.ToInt32(row["amount"]));
                list.Add(baseItem);
            }

            return list;
        }

        public static List<(string, string, string)> GetPlayerGraveyard(SqlCommand sqlCommand)
        {
            var data = GetDataTable(sqlCommand);
            var list = new List<(string, string ,string)>();

            foreach (DataRow row in data.Rows)
            {
                var baseItem = (row["Hero"].ToString(), row["Has been slain by"].ToString(), row["amount"].ToString());
                list.Add(baseItem);
            }

            return list;
        }

        public static List<(string, string, int)> GetEnemyGraveyard(SqlCommand sqlCommand)
        {
            var data = GetDataTable(sqlCommand);
            var list = new List<(string, string, int)>();

            foreach (DataRow row in data.Rows)
            {
                var baseItem = (row["Enemy"].ToString(), row["Has been slain by"].ToString(), Convert.ToInt32(row["Amount of times Slain"]));
                list.Add(baseItem);
            }

            return list;
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Datenbank_RPG.Controllers
{
    enum classID
    {
        Undefined = 0,
        Warrior = 1,
        Mage = 2,
        Thief = 3,
        Demon_Hunter = 4
    }
    public class PlayerController : Controller
    {

        public IActionResult Index()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                var displayPlayersCommand = new SqlCommand("exec displayPlayers @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = -1;

                var list = sql.cmds.GetPlayers(displayPlayersCommand);

                ViewData["listOfPlayers"] = list;
                ViewData["listSize"] = list.Count;

                return View();
            }
        }

        public IActionResult showPlayer(int id)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                var displayPlayersCommand = new SqlCommand("exec displayPlayers @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                var listAllItemsCommand = new SqlCommand("exec displayInventory", connection);

                var playerGraveyardCommand = new SqlCommand("exec displayPlayerGraveyard @id = @i", connection);
                playerGraveyardCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                playerGraveyardCommand.Parameters[0].Value = id;

                var enemyGraveyardCommand = new SqlCommand("exec displayEnemyGraveyard @id = @i", connection);
                enemyGraveyardCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                enemyGraveyardCommand.Parameters[0].Value = id;

                var listPlayers = sql.cmds.GetPlayers(displayPlayersCommand);
                var listItems = sql.cmds.GetItems(listAllItemsCommand);
                var listPlayerGraveyard = sql.cmds.GetPlayerGraveyard(playerGraveyardCommand);
                var listEnemyGraveyard = sql.cmds.GetEnemyGraveyard(enemyGraveyardCommand);

                ViewData["Player"] = listPlayers[0];
                ViewData["listItems"] = listItems;
                ViewData["listPlayerGraveyard"] = listPlayerGraveyard;
                ViewData["listEnemyGraveyard"] = listEnemyGraveyard;

                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var player = new sql.Player();
            return View(player);
        }
        [HttpPost]
        public IActionResult Create(sql.Player player)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            using (var connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                connection.Open();

                var addPlayerCommand = new SqlCommand("exec add_player @name = @n, @life = @l, @defense = @d, @classId = @ci", connection);
                addPlayerCommand.Parameters.Add(new SqlParameter("@n", System.Data.SqlDbType.VarChar, 64));
                addPlayerCommand.Parameters[0].Value = player.Name;

                addPlayerCommand.Parameters.Add(new SqlParameter("@l", System.Data.SqlDbType.Int));
                addPlayerCommand.Parameters[1].Value = player.MaxLife;

                addPlayerCommand.Parameters.Add(new SqlParameter("@d", System.Data.SqlDbType.Int));
                addPlayerCommand.Parameters[2].Value = player.Defense;

                addPlayerCommand.Parameters.Add(new SqlParameter("@ci", System.Data.SqlDbType.Int));
                addPlayerCommand.Parameters[3].Value = player.classId;

                addPlayerCommand.ExecuteNonQuery();

                connection.Close();

                return Redirect("/Player");
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                var displayPlayersCommand = new SqlCommand("exec displayPlayers @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                var listEnemy = sql.cmds.GetPlayers(displayPlayersCommand);

                Console.WriteLine();

                return View(listEnemy[0]);
            }
        }

        [HttpPost]
        public IActionResult Edit(sql.Player player)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                connection.Open();

                var displayPlayersCommand = new SqlCommand("exec updatePlayer @id = @i, @name = @n, @life = @l, @maxlife = @ml, @defense = @d, @classId = @e", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = player.Id;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@n", System.Data.SqlDbType.VarChar, 64));
                displayPlayersCommand.Parameters[1].Value = player.Name;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@l", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[2].Value = player.Life;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@ml", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[3].Value = player.MaxLife;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@d", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[4].Value = player.Defense;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@e", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[5].Value = player.classId;

                displayPlayersCommand.ExecuteNonQuery();

                connection.Close();

                return Redirect("/Player");
            }
        }

        public IActionResult Delete(int id)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                var displayPlayersCommand = new SqlCommand("exec removePlayer @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                connection.Open();

                displayPlayersCommand.ExecuteNonQuery();

                connection.Close();

                Console.WriteLine();

                return Redirect("/Player");
            }
        }
    }
}

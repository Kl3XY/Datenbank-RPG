using Microsoft.AspNetCore.Mvc;
using sql;
using System.Data.SqlClient;

namespace Datenbank_RPG.Controllers
{
    enum enemyType
    {
        smallFoe = 1
    }
    public class EnemiesController : Controller
    {
        public IActionResult Index()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                var displayPlayersCommand = new SqlCommand("exec displayEnemies @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = -1;

                var listPlayers = sql.cmds.GetEnemies(displayPlayersCommand);


                ViewData["Enemies"] = listPlayers;

                return View();
            }
        }

        public IActionResult showEnemy(int id)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                var displayPlayersCommand = new SqlCommand("exec displayEnemies @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                var playerGraveyardCommand = new SqlCommand("exec displayPlayerGraveyard_enemyID @id = @i", connection);
                playerGraveyardCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                playerGraveyardCommand.Parameters[0].Value = id;

                var enemyGraveyardCommand = new SqlCommand("exec displayEnemyGraveyard_enemyID @id = @i", connection);
                enemyGraveyardCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                enemyGraveyardCommand.Parameters[0].Value = id;

                var listEnemy = sql.cmds.GetEnemies(displayPlayersCommand);
                var listPlayerGraveyard = sql.cmds.GetPlayerGraveyard(playerGraveyardCommand);
                var listEnemyGraveyard = sql.cmds.GetEnemyGraveyard(enemyGraveyardCommand);

                ViewData["Enemy"] = listEnemy[0];
                ViewData["listPlayerGraveyard"] = listPlayerGraveyard;
                ViewData["listEnemyGraveyard"] = listEnemyGraveyard;

                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var enemy = new sql.Enemy();
            enemy = new sql.Enemy();
            return View(enemy);
        }


        [HttpPost]
        public IActionResult Create(sql.Enemy enemy)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            using (var connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                connection.Open();

                var addEnemyCommand = new SqlCommand("exec addEnemy @name = @n, @life = @l, @defense = @d, @enemyType = @e", connection);
                addEnemyCommand.Parameters.Add(new SqlParameter("@n", System.Data.SqlDbType.VarChar, 64));
                addEnemyCommand.Parameters[0].Value = enemy.Name;

                addEnemyCommand.Parameters.Add(new SqlParameter("@l", System.Data.SqlDbType.Int));
                addEnemyCommand.Parameters[1].Value = enemy.Defense;

                addEnemyCommand.Parameters.Add(new SqlParameter("@d", System.Data.SqlDbType.Int));
                addEnemyCommand.Parameters[2].Value = enemy.Defense;

                addEnemyCommand.Parameters.Add(new SqlParameter("@e", System.Data.SqlDbType.Int));
                addEnemyCommand.Parameters[3].Value = enemy.type;

                addEnemyCommand.ExecuteNonQuery();

                connection.Close();

                return Redirect("/Enemies");
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
                var displayPlayersCommand = new SqlCommand("exec displayEnemies @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                var listEnemy = sql.cmds.GetEnemies(displayPlayersCommand);

                Console.WriteLine();

                return View(listEnemy[0]);
            }
        }

        [HttpPost]
        public IActionResult Edit(sql.Enemy enemy)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                connection.Open();

                var displayPlayersCommand = new SqlCommand("exec updateEnemy @id = @i, @name = @n, @life = @l, @maxlife = @ml, @defense = @d, @enemyType = @e", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = enemy.Id;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@n", System.Data.SqlDbType.VarChar, 64));
                displayPlayersCommand.Parameters[1].Value = enemy.Name;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@l", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[2].Value = enemy.Life;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@ml", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[3].Value = enemy.maxLife;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@d", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[4].Value = enemy.Defense;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@e", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[5].Value = enemy.type;

                displayPlayersCommand.ExecuteNonQuery();

                connection.Close();

                return Redirect("/Enemies");
            }
        }

        public IActionResult Delete(int id)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                var displayPlayersCommand = new SqlCommand("exec removeEnemy @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                connection.Open();

                displayPlayersCommand.ExecuteNonQuery();

                connection.Close();

                Console.WriteLine();

                return Redirect("/Enemies");
            }
        }

    }
}

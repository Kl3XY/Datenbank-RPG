using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
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
        [Breadcrumb("/All Enemies")]
        [HttpGet]
        public IActionResult Index()
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var displayEnemiesCommand = new SqlCommand("exec searchEnemy @search = @d", connection);
                displayEnemiesCommand.Parameters.Add(new SqlParameter("@d", System.Data.SqlDbType.VarChar, 64));
                displayEnemiesCommand.Parameters[0].Value = sql.cmds.search;

                var list = sql.cmds.GetEnemies(displayEnemiesCommand);

                ViewData["Enemies"] = list;
                ViewData["listSize"] = list.Count;
                ViewData["route"] = RouteData.Values;
                sql.cmds.search = "";
                return View();
            }
        }


        [HttpPost]
        public IActionResult Index(sql.search search)
        {
            if (search.searchTerm == null) { search.searchTerm = ""; }
            sql.cmds.search = search.searchTerm;
            ViewData["route"] = RouteData.Values;
            return Redirect($"Index");
        }

        [Breadcrumb("/Show Enemy")]
        public IActionResult showEnemy(int id)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
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
                ViewData["route"] = RouteData.Values;


                return View();
            }
        }

        [Breadcrumb("/Create Enemy")]
        [HttpGet]
        public IActionResult Create()
        {
            var enemy = new sql.Enemy();
            enemy = new sql.Enemy();
            ViewData["route"] = RouteData.Values;
            return View(enemy);
        }

        [HttpPost]
        public IActionResult Create(sql.Enemy enemy)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
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
                ViewData["route"] = RouteData.Values;
                connection.Close();

                return Redirect("/Enemies/Index");
            }
        }
        [Breadcrumb("/Edit Enemy")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var displayPlayersCommand = new SqlCommand("exec displayEnemies @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                var listEnemy = sql.cmds.GetEnemies(displayPlayersCommand);
                ViewData["route"] = RouteData.Values;
                Console.WriteLine();

                return View(listEnemy[0]);
            }
        }

        [HttpPost]
        public IActionResult Edit(sql.Enemy enemy)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
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
                ViewData["route"] = RouteData.Values;
                connection.Close();

                return Redirect("/Enemies/Index");
            }
        }

        public IActionResult Delete(int id)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var displayPlayersCommand = new SqlCommand("exec removeEnemy @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                connection.Open();

                displayPlayersCommand.ExecuteNonQuery();

                connection.Close();
                ViewData["route"] = RouteData.Values;
                Console.WriteLine();

                return Redirect("/Enemies/Index");
            }
        }

    }
}

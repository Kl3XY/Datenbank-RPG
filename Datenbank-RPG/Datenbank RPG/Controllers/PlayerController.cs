using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
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
        [DefaultBreadcrumb]
        [HttpGet]
        public IActionResult Index()
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var displayPlayersCommand = new SqlCommand("exec searchPlayer @search = @d", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@d", System.Data.SqlDbType.VarChar, 64));
                displayPlayersCommand.Parameters[0].Value = sql.cmds.search;

                var list = sql.cmds.GetPlayers(displayPlayersCommand);

                ViewData["listOfPlayers"] = list;
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

        [Breadcrumb("/Show Player")]
        public IActionResult showPlayer(int id)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
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
                ViewData["route"] = RouteData.Values;

                return View();
            }
        }
        [Breadcrumb("/Create Player")]
        [HttpGet]
        public IActionResult Create()
        {
            var player = new sql.Player();
            ViewData["route"] = RouteData.Values;
            return View(player);
        }
        [HttpPost]
        public IActionResult Create(sql.Player player)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
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
                ViewData["route"] = RouteData.Values;
                connection.Close();

                return Redirect("/Player/Index");
            }
        }
        [Breadcrumb("/Edit Player")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var displayPlayersCommand = new SqlCommand("exec displayPlayers @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                var listEnemy = sql.cmds.GetPlayers(displayPlayersCommand);
                ViewData["route"] = RouteData.Values;
                Console.WriteLine();

                return View(listEnemy[0]);
            }
        }

        [HttpPost]
        public IActionResult Edit(sql.Player player)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
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
                ViewData["route"] = RouteData.Values;
                return Redirect("/Player/Index");
            }
        }

        public IActionResult Delete(int id)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var displayPlayersCommand = new SqlCommand("exec removePlayer @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                connection.Open();

                displayPlayersCommand.ExecuteNonQuery();

                connection.Close();
                ViewData["route"] = RouteData.Values;
                Console.WriteLine();

                return Redirect("/Player");
            }
        }
    }
}

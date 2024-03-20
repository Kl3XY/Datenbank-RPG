using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

using System.Data.SqlClient;

namespace Datenbank_RPG.Controllers
{
    public class DatabaseController : Controller
    {
        //
        // GET: /Database/
        public IActionResult Index(int age)
        {
            ViewData["age"] = age;
            return View();
        }

        //
        // GET: /Database/Welcome
        public IActionResult players()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=(localDB)\\MSSQLLocalDB;Database=game;Integrated Security=True;TrustServerCertificate=true";

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

            sqlBuilder.ConnectionString = $"Server=(localDB)\\MSSQLLocalDB;Database=game;Integrated Security=True;TrustServerCertificate=true";

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
    }
}

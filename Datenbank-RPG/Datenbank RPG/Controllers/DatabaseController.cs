using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

using System.Data.SqlClient;

namespace Datenbank_RPG.Controllers
{
    public class DatabaseController : Controller
    {
        //
        // GET: /Database/
        public IActionResult Index()
        {
            return View();
        }

        //
        // GET: /Database/Welcome
        public IActionResult welcome(string name, int age)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=(localDB)\\MSSQLLocalDB;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using(connection = new SqlConnection(sqlBuilder.ConnectionString))
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
    }
}

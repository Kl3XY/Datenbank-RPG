using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

using System.Data.SqlClient;
using sql;
using Datenbank_RPG.Models;

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

        public IActionResult items()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                var getInventoryCommand = new SqlCommand("exec displayInventory", connection);

                var listItem = sql.cmds.GetItems(getInventoryCommand);


                ViewData["Items"] = listItem;

                return View();
            }
        }

        public IActionResult showitem(int id)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                var getInventoryCommand = new SqlCommand("exec list_item @id = @i", connection);
                getInventoryCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                getInventoryCommand.Parameters[0].Value = id;

                var listItem = sql.cmds.GetItems(getInventoryCommand);


                ViewData["Item"] = listItem[0];

                return View();
            }
        }

        public IActionResult allItems()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                var getInventoryCommand = new SqlCommand("exec list_all_items", connection);

                var listItem = sql.cmds.GetItems(getInventoryCommand);


                ViewData["Items"] = listItem;

                return View();
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
                var displayPlayersCommand = new SqlCommand("exec list_item @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                var listEnemy = sql.cmds.GetItems(displayPlayersCommand);

                Console.WriteLine();

                return View(listEnemy[0]);
            }
        }

        [HttpPost]
        public IActionResult Edit(sql.Item item)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                connection.Open();

                var displayPlayersCommand = new SqlCommand("exec updateItem @id = @i, @name = @n, @itemPower = @l, @itemType = @ml, @Gold = @d", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = item.Id;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@l", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[1].Value = item.ItemPower;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@ml", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[2].Value = item.ItemType;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@d", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[3].Value = item.Gold;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@n", System.Data.SqlDbType.VarChar, 64));
                displayPlayersCommand.Parameters[4].Value = item.Name;


                displayPlayersCommand.ExecuteNonQuery();

                connection.Close();

                return Redirect("/Database/allitems");
            }
        }

        public IActionResult Delete(int id)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                var displayPlayersCommand = new SqlCommand("exec deleteItem @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                connection.Open();

                displayPlayersCommand.ExecuteNonQuery();

                connection.Close();

                Console.WriteLine();

                return Redirect("/Database/allitems");
            }
        }
    }
}

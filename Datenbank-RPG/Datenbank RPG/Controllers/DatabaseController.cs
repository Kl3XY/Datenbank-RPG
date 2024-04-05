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
        [HttpGet]
        public IActionResult allItems()
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var displayItemsCommand = new SqlCommand("exec searchItem @search = @d", connection);
                displayItemsCommand.Parameters.Add(new SqlParameter("@d", System.Data.SqlDbType.VarChar, 64));
                displayItemsCommand.Parameters[0].Value = sql.cmds.search;

                var list = sql.cmds.GetItems(displayItemsCommand);

                ViewData["Items"] = list;
                ViewData["listSize"] = list.Count;
                sql.cmds.search = "";
                return View();
            }
        }

        [HttpPost]
        public IActionResult allItems(sql.search search)
        {
            if (search.searchTerm == null) { search.searchTerm = ""; }
            sql.cmds.search = search.searchTerm;
            return Redirect($"allItems");
        }

        public IActionResult items()
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var getInventoryCommand = new SqlCommand("exec displayInventory", connection);

                var listItem = sql.cmds.GetItems(getInventoryCommand);


                ViewData["Items"] = listItem;

                return View();
            }
        }

        public IActionResult showitem(int id)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var getInventoryCommand = new SqlCommand("exec list_item @id = @i", connection);
                getInventoryCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                getInventoryCommand.Parameters[0].Value = id;

                var listItem = sql.cmds.GetItems(getInventoryCommand);


                ViewData["Item"] = listItem[0];

                return View();
            }
        }

        [HttpGet]
        public IActionResult initServer()
        {
            return View();
        }


        [HttpPost]
        public IActionResult initServer(stringBuilder sB)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server={sB.connectionString};Database=game;Integrated Security=True;TrustServerCertificate=true";

            sql.cmds.connection = sqlBuilder.ConnectionString;

            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                //Testing if connection works.
                connection.Open();
                connection.Close();
            }
                
                



            return Redirect("/Player/Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
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
            using (var connection = new SqlConnection(sql.cmds.connection))
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
            using (var connection = new SqlConnection(sql.cmds.connection))
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

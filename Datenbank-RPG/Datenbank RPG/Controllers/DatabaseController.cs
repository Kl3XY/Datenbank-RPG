using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

using System.Data.SqlClient;
using sql;
using Datenbank_RPG.Models;
using Microsoft.AspNetCore.Http;
using SmartBreadcrumbs.Attributes;

namespace Datenbank_RPG.Controllers
{
    public class DatabaseController : Controller
    {
        //
        // GET: /Database/
        [Breadcrumb("/All Items")]
        [HttpGet]
        public IActionResult Index()
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var displayItemsCommand = new SqlCommand("exec searchItem @search = @d", connection);
                displayItemsCommand.Parameters.Add(new SqlParameter("@d", System.Data.SqlDbType.VarChar, 64));
                displayItemsCommand.Parameters[0].Value = sql.cmds.search;

                var list = sql.cmds.GetItems(displayItemsCommand);
                ViewData["Items"] = list;
                ViewData["listSize"] = list.Count;
                ViewData["route"] = RouteData.Values;
                sql.cmds.search = "";
                return View();
            }
        }

        [HttpPost]
        public IActionResult allItems(sql.search search)
        {
            if (search.searchTerm == null) { search.searchTerm = ""; }
            sql.cmds.search = search.searchTerm;
            ViewData["route"] = RouteData.Values;
            return Redirect($"Index");
        }

        public IActionResult items()
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var getInventoryCommand = new SqlCommand("exec displayInventory", connection);

                var listItem = sql.cmds.GetItems(getInventoryCommand);

                ViewData["route"] = RouteData.Values;
                ViewData["Items"] = listItem;

                return View();
            }
        }
        [Breadcrumb("/Show Item")]
        public IActionResult showitem(int id)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var getInventoryCommand = new SqlCommand("exec list_item @id = @i", connection);
                getInventoryCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                getInventoryCommand.Parameters[0].Value = id;

                var listItem = sql.cmds.GetItems(getInventoryCommand);

                ViewData["route"] = RouteData.Values;
                ViewData["Item"] = listItem[0];

                return View();
            }
        }
        [Breadcrumb("/Init Server")]
        [HttpGet]
        public IActionResult initServer()
        {
            ViewData["route"] = RouteData.Values;
            if (HttpContext.Session.GetInt32("_DarkMode") == null)
            {
                HttpContext.Session.SetInt32("_DarkMode", 0);
            }
            
            return View();
        }
        [Breadcrumb("/Toggle dark mode")]
        [HttpGet]
        public IActionResult toggleDarkMode(string url)
        {
            bool updatedSetting = !Convert.ToBoolean(HttpContext.Session.GetInt32("_DarkMode"));
            HttpContext.Session.SetInt32("_DarkMode", Convert.ToInt32(updatedSetting));
            sql.cmds.darkMode = updatedSetting;
            return Redirect(url);
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

            ViewData["route"] = RouteData.Values;



            return Redirect("/Player/Index");
        }
        [Breadcrumb("/Edit Item")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var displayPlayersCommand = new SqlCommand("exec list_item @id = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                var listEnemy = sql.cmds.GetItems(displayPlayersCommand);
                ViewData["route"] = RouteData.Values;
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
                ViewData["route"] = RouteData.Values;
                return Redirect("/Database/Index");
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
                ViewData["route"] = RouteData.Values;
                return Redirect("/Database/Index");
            }
        }
    }
}

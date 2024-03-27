using Datenbank_RPG.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Datenbank_RPG.Controllers
{
    public enum itemType
    {
        HealthPotion = 1,
        Revive = 2
    }
    public class ItemController : Controller
    {
        public string Name { get; set; } = "Item";
        public IActionResult Index()
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
        [HttpGet]
        public IActionResult Create()
        {
            var item = new sql.Item();
            return View( item );
        }
        [HttpPost]
        public IActionResult Create(sql.Item item)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            using (var connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                connection.Open();

                var createItemCommand = new SqlCommand("exec createItem @name = @i, @itemtype = @it, @itemPower = @fr, @gold = @g", connection);
                createItemCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.VarChar, 64));
                createItemCommand.Parameters[0].Value = item.Name;

                createItemCommand.Parameters.Add(new SqlParameter("@it", System.Data.SqlDbType.VarChar, 64));
                createItemCommand.Parameters[1].Value = item.ItemType;

                createItemCommand.Parameters.Add(new SqlParameter("@fr", System.Data.SqlDbType.VarChar, 64));
                createItemCommand.Parameters[2].Value = item.ItemPower;

                createItemCommand.Parameters.Add(new SqlParameter("@g", System.Data.SqlDbType.VarChar, 64));
                createItemCommand.Parameters[3].Value = item.Gold;

                createItemCommand.ExecuteNonQuery();

                connection.Close();

                return Redirect("/");
            }
        }

        public IActionResult Delete(int id)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

            SqlConnection connection;

            using (connection = new SqlConnection(sqlBuilder.ConnectionString))
            {
                var displayPlayersCommand = new SqlCommand("exec removeItemFromInventory @itemId = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                connection.Open();

                displayPlayersCommand.ExecuteNonQuery();

                connection.Close();

                Console.WriteLine();

                return Redirect("/Item");
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

                var displayPlayersCommand = new SqlCommand("exec updateAmountItem @itemId = @i, @amount = @n", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = item.Id;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@n", System.Data.SqlDbType.VarChar, 64));
                displayPlayersCommand.Parameters[1].Value = item.amount;


                displayPlayersCommand.ExecuteNonQuery();

                connection.Close();

                return Redirect("/Item");
            }
        }
    }
}

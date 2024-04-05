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
        [HttpGet]
        public IActionResult Index()
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var displayItemsCommand = new SqlCommand("exec searchInventory @search = @d", connection);
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
        public IActionResult Index(sql.search search)
        {
            if (search.searchTerm == null) { search.searchTerm = ""; }
            sql.cmds.search = search.searchTerm;
            return Redirect($"Index");
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
        public IActionResult Create()
        {
            var item = new sql.Item();
            return View( item );
        }
        [HttpPost]
        public IActionResult Create(sql.Item item)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
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

                return Redirect("/Database/Index");
            }
        }

        public IActionResult Delete(int id)
        {
            using (var connection = new SqlConnection(sql.cmds.connection))
            {
                var displayPlayersCommand = new SqlCommand("exec removeItemFromInventory @itemId = @i", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = id;

                connection.Open();

                displayPlayersCommand.ExecuteNonQuery();

                connection.Close();

                Console.WriteLine();

                return Redirect("/Item/Index");
            }
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

                var displayPlayersCommand = new SqlCommand("exec updateAmountItem @itemId = @i, @amount = @n", connection);
                displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
                displayPlayersCommand.Parameters[0].Value = item.Id;

                displayPlayersCommand.Parameters.Add(new SqlParameter("@n", System.Data.SqlDbType.VarChar, 64));
                displayPlayersCommand.Parameters[1].Value = item.amount;


                displayPlayersCommand.ExecuteNonQuery();

                connection.Close();

                return Redirect("/Item/Index");
            }
        }
    }
}

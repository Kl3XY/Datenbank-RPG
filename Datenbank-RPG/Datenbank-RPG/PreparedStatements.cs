﻿using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Datenbank
{
    internal static class prepared_statement
    {
        public static List<(string,SqlCommand)> statements = new();
        public static void prepareStatements()
        {
            statements.Clear();

            /* ADD PLAYER */

            var addPlayerCommand = new SqlCommand("exec add_player @name = @n, @life = @l, @defense = @d, @classId = @cid", Program.connection);
            addPlayerCommand.Parameters.Add(new SqlParameter("@n", System.Data.SqlDbType.VarChar, 64));
            addPlayerCommand.Parameters[0].Value = "";

            addPlayerCommand.Parameters.Add(new SqlParameter("@l", System.Data.SqlDbType.Int));
            addPlayerCommand.Parameters[1].Value = 0;

            addPlayerCommand.Parameters.Add(new SqlParameter("@d", System.Data.SqlDbType.Int));
            addPlayerCommand.Parameters[2].Value = 0;

            addPlayerCommand.Parameters.Add(new SqlParameter("@cid", System.Data.SqlDbType.Int));
            addPlayerCommand.Parameters[3].Value = 1;
            addPlayerCommand.Prepare();

            statements.Add(("addPlayer", addPlayerCommand));

            /* LIST ALL ITEMS */

            var listAllItemsCommand = new SqlCommand("exec list_all_items", Program.connection);

            statements.Add(("listItems", listAllItemsCommand));

            /* BUY ITEM */

            var updateMoneyCommand = new SqlCommand("exec buyItem @playerID = @id, @itemId = @itemID, @gold = @g", Program.connection);
            updateMoneyCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int));
            updateMoneyCommand.Parameters[0].Value = 0;

            updateMoneyCommand.Parameters.Add(new SqlParameter("@itemID", System.Data.SqlDbType.Int));
            updateMoneyCommand.Parameters[1].Value = 0;

            updateMoneyCommand.Parameters.Add(new SqlParameter("@g", System.Data.SqlDbType.Int));
            updateMoneyCommand.Parameters[2].Value = 0;

            statements.Add(("buyItem", updateMoneyCommand));

            /* ADD ITEM */

            var addItemCommand = new SqlCommand("exec addItem @itemId = @id", Program.connection);
            addItemCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int));
            addItemCommand.Parameters[0].Value = 0;

            statements.Add(("addItem", addItemCommand));

            /* DISPLAY INVENTORY */

            var displayInventoryCommand = new SqlCommand("exec displayInventory", Program.connection);

            statements.Add(("displayInventory", displayInventoryCommand));

        }

        public static SqlCommand getStatement(string searchTerm)
        {
            var find = statements.FindIndex(n => n.Item1 == searchTerm);

            return statements[find].Item2;
        }
    }
}
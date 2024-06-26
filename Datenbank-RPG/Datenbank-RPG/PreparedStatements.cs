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

            /* sort inventory by amount */

            var sortInventoryCommand = new SqlCommand("exec sortInventoryByAmount", Program.connection);

            statements.Add(("sortInventoryByAmount", sortInventoryCommand));

            /* give Gold */

            var giveGoldCommand = new SqlCommand("exec giveGold @id = @i, @amount = @a", Program.connection);
            giveGoldCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.VarChar, 64));
            giveGoldCommand.Parameters[0].Value = 1;

            giveGoldCommand.Parameters.Add(new SqlParameter("@a", System.Data.SqlDbType.VarChar, 64));
            giveGoldCommand.Parameters[1].Value = 99999;

            statements.Add(("giveGold", giveGoldCommand));

            /* sort Player By Gold */

            var sortPlayerByGoldCommand = new SqlCommand("exec sortPlayerByGold", Program.connection);

            statements.Add(("sortPlayerByGold", sortPlayerByGoldCommand));

            /* SEARCH PLAYER */

            var searchPlayerCommand = new SqlCommand("exec searchPlayer @search = @s", Program.connection);
            searchPlayerCommand.Parameters.Add(new SqlParameter("@s", System.Data.SqlDbType.VarChar, 64));
            searchPlayerCommand.Parameters[0].Value = "";

            statements.Add(("searchPlayer", searchPlayerCommand));

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

            /* PLAYER DEAD */

            var playerDeadCommand = new SqlCommand("exec playerDead @playerId = @pid, @enemyid = @eid", Program.connection);
            playerDeadCommand.Parameters.Add(new SqlParameter("@pid", System.Data.SqlDbType.Int));
            playerDeadCommand.Parameters[0].Value = 0;

            playerDeadCommand.Parameters.Add(new SqlParameter("@eid", System.Data.SqlDbType.Int));
            playerDeadCommand.Parameters[1].Value = 0;

            statements.Add(("playerDead", playerDeadCommand));

            /* ENEMY DEAD */

            var enemyDeadCommand = new SqlCommand("exec enemyDead @playerId = @pid, @enemyid = @eid", Program.connection);
            enemyDeadCommand.Parameters.Add(new SqlParameter("@pid", System.Data.SqlDbType.Int));
            enemyDeadCommand.Parameters[0].Value = 0;

            enemyDeadCommand.Parameters.Add(new SqlParameter("@eid", System.Data.SqlDbType.Int));
            enemyDeadCommand.Parameters[1].Value = 0;

            statements.Add(("enemyDead", enemyDeadCommand));

            /* DISPLAY PLAYER GRAVEYARD */

            var displayPlayerGraveyardCommand = new SqlCommand("exec displayPlayerGraveyard @id = @idd", Program.connection);
            displayPlayerGraveyardCommand.Parameters.Add(new SqlParameter("@idd", System.Data.SqlDbType.Int));
            displayPlayerGraveyardCommand.Parameters[0].Value = 0;

            statements.Add(("displayPlayerGraveyard", displayPlayerGraveyardCommand));

            /* DISPLAY ENEMY GRAVEYARD */

            var displayEnemyGraveyardCommand = new SqlCommand("exec displayEnemyGraveyard @id = @idd", Program.connection);
            displayEnemyGraveyardCommand.Parameters.Add(new SqlParameter("@idd", System.Data.SqlDbType.Int));
            displayEnemyGraveyardCommand.Parameters[0].Value = 0;

            statements.Add(("displayEnemyGraveyard", displayEnemyGraveyardCommand));

            /* DISPLAY INVENTORY */

            var displayInventoryCommand = new SqlCommand("exec displayInventory", Program.connection);

            statements.Add(("displayInventory", displayInventoryCommand));

            /* displayPlayers */

            var displayPlayersCommand = new SqlCommand("exec displayPlayers @id = @i", Program.connection);
            displayPlayersCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
            displayPlayersCommand.Parameters[0].Value = -1;

            statements.Add(("displayPlayers", displayPlayersCommand));

            /* displayEnemy */

            var displayEnemyCommand = new SqlCommand("exec displayEnemies @id = @i", Program.connection);
            displayEnemyCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
            displayEnemyCommand.Parameters[0].Value = -1;

            statements.Add(("displayEnemy", displayEnemyCommand));

            /* SET PLAYER HEALTH */

            var setPlayerHealthCommand = new SqlCommand("exec setPlayerHealth @playerid = @i, @health = @h", Program.connection);
            setPlayerHealthCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
            setPlayerHealthCommand.Parameters[0].Value = -1;
            setPlayerHealthCommand.Parameters.Add(new SqlParameter("@h", System.Data.SqlDbType.Int));
            setPlayerHealthCommand.Parameters[0].Value = -1;

            statements.Add(("setPlayerHealth", setPlayerHealthCommand));

            /* SET ENEMY HEALTH */

            var setEnemyHealthCommand = new SqlCommand("exec setEnemyHealth @enemyid = @i, @health = @h", Program.connection);
            setEnemyHealthCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
            setEnemyHealthCommand.Parameters[0].Value = -1;
            setEnemyHealthCommand.Parameters.Add(new SqlParameter("@h", System.Data.SqlDbType.Int));
            setEnemyHealthCommand.Parameters[0].Value = -1;

            statements.Add(("setEnemyHealth", setEnemyHealthCommand));

            /* USE ITEM */

            var useItemHealthCommand = new SqlCommand("exec useItem @itemId = @i", Program.connection);
            useItemHealthCommand.Parameters.Add(new SqlParameter("@i", System.Data.SqlDbType.Int));
            useItemHealthCommand.Parameters[0].Value = -1;

            statements.Add(("useItem", useItemHealthCommand));

        }

        public static SqlCommand getStatement(string searchTerm)
        {
            var find = statements.FindIndex(n => n.Item1 == searchTerm);

            return statements[find].Item2;
        }
    }
}

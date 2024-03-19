using Datenbank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Datenbank_RPG
{
    public class Combat_Screen
    {
        public static int menuSelect = 0;
        public static int turns = 0;
        public static bool loopSection = true;
        public static List<sql.Enemy> enemies = new();
        public static List<string> combatLog = new();
        public static int generalGoldAmount = 0;
        public static int idOfChosenPlayer = 0;
        public static int idOfChosenEnemy = 0;

        public static int playerAttackDelay = 0;
        public static int playerCurrentAttackDelay = 0;

        public static int enemyAttackDelay = 0;
        public static int enemyCurrentAttackDelay = 0;

        public static void Menu()
        {
            while (loopSection)
            {
                Console.Clear();
                Console.WriteLine("A Bout! This will be interesting.");
                SQL.drawPlayerList(idOfChosenPlayer);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("v.s.");
                SQL.drawEnemyList();

                var player = Program.players.Find(d => d.Id == idOfChosenPlayer);
                var enemy = enemies.Find(d => d.Id == idOfChosenEnemy);
                playerAttackDelay = player.attackDelay;
                enemyAttackDelay = enemy.atkDelay;
                playerCurrentAttackDelay -= 100;
                enemyCurrentAttackDelay -= 100;

                var rnd = Program.rng;

                if (playerCurrentAttackDelay < 0)
                {
                    if (rnd.Next(99) > 40)
                    {
                        var cmd = prepared_statement.getStatement("setEnemyHealth");
                        cmd.Parameters[0].Value = enemy.Id;
                        cmd.Parameters[1].Value = enemy.Life - player.attack / 10;
                        combatLog.Add($"{player.Name} Damaged {enemy.Name} for {player.attack / 10}");
                        cmd.ExecuteNonQuery();
                        playerCurrentAttackDelay = playerAttackDelay;
                    } else
                    {
                        combatLog.Add($"{player.Name} Missed!");
                        playerCurrentAttackDelay = playerAttackDelay;
                    }
                }

                if (enemyCurrentAttackDelay < 0)
                {
                    if (rnd.Next(99) > 40)
                    {
                        var cmd = prepared_statement.getStatement("setPlayerHealth");
                        cmd.Parameters[0].Value = player.Id;
                        cmd.Parameters[1].Value = player.Life - enemy.atk / 10;
                        combatLog.Add($"{enemy.Name} Damaged {player.Name} for {enemy.atk / 10}");
                        cmd.ExecuteNonQuery();
                        enemyCurrentAttackDelay = enemyAttackDelay;
                    } else
                    {
                        combatLog.Add($"{enemy.Name} Missed!");
                        enemyCurrentAttackDelay = enemyAttackDelay;
                    }
                }

                if (player.Life <= 0)
                {
                    loopSection = false;
                    Console.WriteLine("{0} Has Died...", player.Name);
                    Console.ReadKey();
                    combatLog.Clear();

                    var cmd = prepared_statement.getStatement("playerDead");
                    cmd.Parameters[0].Value = player.Id;
                    cmd.Parameters[1].Value = enemy.Id;
                    cmd.ExecuteNonQuery();
                }
                if (enemy.Life <= 0)
                {
                    var cmd = prepared_statement.getStatement("setEnemyHealth");
                    cmd.Parameters[0].Value = enemy.Id;
                    cmd.Parameters[1].Value = enemy.maxLife;

                    var cmd2 = prepared_statement.getStatement("enemyDead");
                    cmd2.Parameters[0].Value = player.Id;
                    cmd2.Parameters[1].Value = enemy.Id;
                    cmd2.ExecuteNonQuery();

                    var goldObtained = Program.rng.Next(50);
                    var cmd3 = prepared_statement.getStatement("giveGold");
                    cmd3.Parameters[0].Value = player.Id;
                    cmd3.Parameters[1].Value = goldObtained;
                    cmd3.ExecuteNonQuery();

                    loopSection = false;
                    Console.WriteLine("{1} has killed {0}!", enemy.Name, player.Name);
                    Console.WriteLine("{1} Obtained {0} Gold!", goldObtained, player.Name);

                    if (Program.rng.Next(100) < 5) { 
                        Console.WriteLine("Found Minor Health Potion!"); 
                        var cmd4 = prepared_statement.getStatement("addItem");
                        cmd4.Parameters[0].Value = 1;
                    }

                    Console.ReadKey();
                    cmd.ExecuteNonQuery();
                    combatLog.Clear();
                }


                for (var i = 0; i < Math.Min(combatLog.Count, 10); i++)
                {
                    Console.WriteLine(combatLog[^(i+1)]);
                }

                Program.players.Clear();
                Inventory.items.Clear();
                Thread.Sleep(250);
            }
            Console.Clear();
        }
    }
}

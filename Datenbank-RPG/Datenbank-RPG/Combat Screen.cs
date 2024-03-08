using Datenbank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datenbank_RPG
{
    public class Combat_Screen
    {
        public static int menuSelect = 0;
        public static int turns = 0;
        public static bool loopSection = true;
        public static List<Enemy> enemies = new();
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
                playerAttackDelay = player.atkDelay;
                enemyAttackDelay = enemy.atkDelay;
                playerCurrentAttackDelay -= 10;
                enemyCurrentAttackDelay -= 10;
                
                var rnd = new Random();

                if (playerCurrentAttackDelay < 0)
                {
                    if (rnd.Next(99) > 40)
                    {
                        var cmd = prepared_statement.getStatement("setEnemyHealth");
                        cmd.Parameters[0].Value = enemy.Id;
                        cmd.Parameters[1].Value = enemy.Life - player.Attack / 10;
                        combatLog.Add($"{player.Name} Damaged {enemy.Name} for {player.Attack / 10}");
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

                if (player.Life < 0)
                {
                    loopSection = false;
                    Console.WriteLine("{0} Has Died...", player.Name);
                    Console.ReadKey();
                }
                if (enemy.Life < 0)
                {
                    var cmd = prepared_statement.getStatement("setEnemyHealth");
                    cmd.Parameters[0].Value = enemy.Id;
                    cmd.Parameters[1].Value = enemy.maxLife;

                    loopSection = false;
                    Console.WriteLine("{1} has killed {0}!", enemy.Name, player.Name);
                    Console.ReadKey();
                    cmd.ExecuteNonQuery();
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

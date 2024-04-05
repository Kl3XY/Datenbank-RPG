using ConsoleTables;
using Datenbank;
using Datenbank_RPG;
using RandomNameGenerator;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

internal class Program
{
    public static List<sql.Player> players = new();
    public static List<sql.Enemy> enemies = new();
    public static SqlConnection connection = null;
    public static Random rng = new();

    public static int menuSelect = 0;

    private static void Main(string[] args)
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        Console.WriteLine("Enter your connection string: ");
        var connectString = Console.ReadLine();

        builder.ConnectionString = $"Server={connectString};Database=game;Integrated Security=True;TrustServerCertificate=true";

        using (connection = new SqlConnection(builder.ConnectionString))
        {
            Console.WriteLine("Connecting...");

            connection.Open();

            prepared_statement.prepareStatements();

            Console.Clear();

            while (true)
            {
                Console.WriteLine("You are at a camp, rest for now.");

                SQL.drawPlayerList();

                Console.WriteLine("        ______\r\n       /     /\\\r\n      /     /  \\\r\n     /_____/----\\_    (  \r\n    \"     \"          ).  \r\n   _ ___          o (:') o   \r\n  (@))_))        o ~/~~\\~ o   \r\n                  o  o  o");
                Console.WriteLine();
                ConsoleTable menu = new ConsoleTable();
                menu.Options.EnableCount = false;

                var menuOptions = new string[] {"Adventure", "Inventory", "Shop", "Statistics", "Trash Fill" };

                for(var i = 0; i < menuOptions.Length; i++)
                {
                    if (i == menuSelect) {
                        menuOptions[i] = ">" + menuOptions[i] + "<";
                    } 
                }
                menu.AddColumn(menuOptions);
                menu.Write();
                Console.WriteLine("Use the left and right arrow key to traverse the menu. And enter to Confirm.");
                
                var key = Console.ReadKey().Key;

                if (key.ToString() == "RightArrow")
                {
                    if (menuSelect++ > menuOptions.Length-2) { menuSelect = 0; }
                }
                if (key.ToString() == "LeftArrow")
                {
                    if (menuSelect-- < 1) { menuSelect = menuOptions.Length - 1; }
                }
                if (key.ToString() == "Enter")
                {
                    switch (menuSelect) {
                        case 0:
                            Console.Clear();
                            players.Clear();
                            AdventureMenu.loopSection = true;
                            AdventureMenu.Menu();
                            break;
                        case 1:
                            Console.Clear();
                            players.Clear();
                            Inventory.loopSection = true;
                            Inventory.Menu();
                            break;
                        case 2:
                            Console.Clear();
                            players.Clear();
                            ShopMenu.loopSection = true;
                            ShopMenu.Menu();
                            break;
                        case 3:
                            Console.Clear();
                            players.Clear();
                            Statistics.loopSection = true;
                            Statistics.Menu();
                            break;
                        case 4:
                            Console.Clear();
                            players.Clear();
                            for (var i = 0; i < 1000; i++) {
                                int rnd = Program.rng.Next(1);
                                var GenerateName = NameGenerator.Generate((Gender)rnd);

                                Console.WriteLine("{0} has entered the party!\nPress any key to enter.", GenerateName);
                                var cmd = prepared_statement.getStatement("addPlayer");
                                cmd.Parameters[0].Value = GenerateName;
                                cmd.Parameters[1].Value = (int)Program.rng.Next(100);
                                cmd.Parameters[2].Value = 20;
                                cmd.Parameters[3].Value = (int)Program.rng.Next(1, 4);

                                cmd.ExecuteNonQuery();
                            }
                            break;
                    } 
                }
                Console.Clear();
                players.Clear();
            }
        }
    }
}
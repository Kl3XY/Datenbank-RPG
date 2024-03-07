﻿using ConsoleTables;
using Datenbank;
using Datenbank_RPG;
using RandomNameGenerator;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;

internal class Program
{
    public static List<Player> players = new();
    public static List<Enemy> enemies = new();
    public static SqlConnection connection = null;

    public static int menuSelect = 0;

    private static void Main(string[] args)
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.ConnectionString = $"Server=DESKTOP-PR3K8AO\\SQLEXPRESS;Database=game;Integrated Security=True;TrustServerCertificate=true";

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

                Console.WriteLine("        ______\r\njgs    /     /\\\r\n      /     /  \\\r\n     /_____/----\\_    (  \r\n    \"     \"          ).  \r\n   _ ___          o (:') o   \r\n  (@))_))        o ~/~~\\~ o   \r\n                  o  o  o");
                Console.WriteLine();

                ConsoleTable menu = new ConsoleTable();
                menu.Options.EnableCount = false;

                var menuOptions = new string[] {"Adventure", "Inventory", "Shop"};

                for(var i = 0; i < 3; i++)
                {
                    if (i == menuSelect) {
                        menuOptions[i] = ">" + menuOptions[i] + "<";
                    } 
                }
                menu.AddColumn(menuOptions);
                menu.Write();
                Console.WriteLine("Use the left and right arrow key to traverse the menu.");
                
                var key = Console.ReadKey().Key;

                if (key.ToString() == "RightArrow")
                {
                    if (menuSelect++ > 1) { menuSelect = 0; }
                }
                if (key.ToString() == "LeftArrow")
                {
                    if (menuSelect-- < 1) { menuSelect = 2; }
                }
                if (key.ToString() == "Spacebar")
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
                    } 
                }
                Console.Clear();
                players.Clear();
            }
        }
    }
}
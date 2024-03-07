using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datenbank_RPG
{
    internal class Player
    {
        public string Name { get; set; }
        public int Life { get; set; }
        public int MaxLife { get; set; }
        public int Defense { get; set; }
        public int Id { get; set; }
        public int playerClass { get; set; }
        public int Attack { get; set; }
        public int Gold { get; set; }

        public Player(string nm, int l, int d, int id, int pClass, int g, int ml) { 
            Name = nm;
            Life = l;
            Defense = d;
            Id = id;
            playerClass = pClass;
            Gold = g;
            MaxLife = ml;
        }
    }

    internal class Enemy
    {
        public string Name { get; set; }
        public int Life { get; set; }
        public int Defense { get; set; }
        public int Id { get; set; }
        public int playerClass { get; set; }

        public Enemy(string nm, int l, int d, int id, int pClass)
        {
            Name = nm;
            Life = l;
            Defense = d;
            Id = id;
            playerClass = pClass;
        }
    }
}

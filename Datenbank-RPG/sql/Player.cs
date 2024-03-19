using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql
{
    public class Player
    {
        public string Name { get; set; }
        public int Life { get; set; }
        public int MaxLife { get; set; }
        public int Defense { get; set; }
        public int Id { get; set; }
        public int classId { get; set; }
        public string className { get; set; }
        public int attack { get; set; }
        public int attackDelay { get; set; }
        public int currentAtkDelay { get; set; }
        public int gold { get; set; }

        public Player() { }
        public Player(string nm, int l, int d, int id, int pClass, int g, int ml, int attack, int attackDelay, string pClassName) { 
            Name = nm;
            Life = l;
            Defense = d;
            Id = id;
            classId = pClass;
            gold = g;
            MaxLife = ml;
            this.attack = attack;
            this.attackDelay = attackDelay;
            currentAtkDelay = attackDelay;
            className = pClassName;
        }
    }

    public class Enemy
    {
        public string Name { get; set; }
        public int Life { get; set; }
        public int maxLife { get; set; }
        public int Defense { get; set; }
        public int Id { get; set; }
        public int atk { get; set; }
        public int atkDelay { get; set; }
        public string type { get; set; }

        public Enemy(int id, string nm, int l, int d, int atk, int atkDelay, int maxLife, string type)
        {
            Name = nm;
            Life = l;
            this.maxLife = maxLife;
            Defense = d;
            Id = id;
            this.atk = atk;
            this.atkDelay = atkDelay;
            this.type = type;
        }
    }
}

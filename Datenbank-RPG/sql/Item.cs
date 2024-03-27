using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public int ItemPower { get; set; }
        public int Gold { get; set; }
        public int amount { get; set; }
        public Item() { }
        public Item(int Id, string Name, string ItemType, int ItemPower, int Gold, int amount = 1)
        {
            this.Id = Id;
            this.Name = Name;
            this.ItemType = ItemType;
            this.ItemPower = ItemPower;
            this.Gold = Gold;
            this.amount = amount;
        }
    }
}

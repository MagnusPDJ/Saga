using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Items.Loot
{
    public class LootEntry
    {
        public string ItemId { get; set; } = string.Empty;
        public double DropChance { get; set; } // 0.0 - 1.0
    }
}

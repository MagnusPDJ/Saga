using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Dungeon
{
    [Serializable]
    internal class Act1Quest : Quest
    {
        public Act1Quest(string name, Type type, string giver, string target, int amount) { 
            Name = name;
            QuestType = type;
            Giver = giver;
            type.SetRequirements(target, amount);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Dungeon
{
    [Serializable]
    internal class Act1Quest : Quest
    {
        public Act1Quest(string name, Type type, string giver, string target, int amount=1) { 
            Name = name;
            QuestType = type;
            Giver = giver;
            TypeExtensions.Requirements = new Dictionary<string, int>();
            type.SetRequirements(target, amount);
        }
    }
}

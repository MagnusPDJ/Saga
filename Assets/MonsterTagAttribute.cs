using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Assets
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MonsterTagAttribute(string tag) : Attribute
    {
        public string Tag { get; } = tag;
    }
}

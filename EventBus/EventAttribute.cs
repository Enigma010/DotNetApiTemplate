using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventBus
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class EventAttribute : Attribute
    {
        public EventAttribute() : base() { }
    }
}

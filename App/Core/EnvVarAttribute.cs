using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class EnvVarAttribute : Attribute
    {
        public string Name { get; }
        public EnvVarAttribute(string name)
        {
            Name = name;
        }
    }
}

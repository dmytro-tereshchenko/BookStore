using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple =false)]
    public class SqlDefaultValueAttribute: Attribute
    {
        public string DefaultValue { get; set; }
    }
}

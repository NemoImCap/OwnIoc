using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnIoc.Model
{
    public class ContainerAdapter
    {
        public string Name { get; set; }

        private int Age { get; set; }

        public long Life
        {
            get { return (long) Age; }
        }

        public ContainerAdapter(string name)
        {
            Name = name;
        }
    }
}

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

        public ContainerAdapter(string name)
        {
            Name = name;
        }
    }
}

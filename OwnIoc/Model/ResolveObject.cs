using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OwnIoc.Model
{
    public class ResolveObject <T>
    {
        public object Resolve { get; set; }
        public ResolveObject(T type, Func<T,bool> expression)
        {
            Resolve = type;
        }

        public void ResolveArguments()
        {
            
        }
    }
}

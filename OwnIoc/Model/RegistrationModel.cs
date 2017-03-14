using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OwnIoc.Enumiration;

namespace OwnIoc.Model
{
    internal class RegistrationModel
    {
        internal Type ObjectType { get; set; }
        internal Reg_Type RegType { get; set; }
    }
}

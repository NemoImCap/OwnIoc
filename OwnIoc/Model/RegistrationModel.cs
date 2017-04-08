using System;
using OwnIoc.Enumiration;

namespace OwnIoc.Model
{
    internal class RegistrationModel
    {
        internal Type ObjectType { get; set; }
        internal RegType RegType { get; set; }
    }
}
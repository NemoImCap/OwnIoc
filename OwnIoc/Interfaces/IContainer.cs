using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnIoc.Interfaces
{
    public interface IContainer
    {

        void RegisterInstanceType<I, T>() where I : class where T : class;
        void RegisterSingletonType<I, T>() where I : class where T : class;


        IContainer SelfRegisterInstanceType<I, T>() where I : class where T : class;
        T Resolve<T>();

        IContainer SetContructorParams<T>(string name, object value, Action<T, bool> expression = null) where T : class;

    }
}

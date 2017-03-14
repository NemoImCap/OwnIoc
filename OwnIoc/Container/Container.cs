using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OwnIoc.Enumiration;
using OwnIoc.Interfaces;
using OwnIoc.Model;

namespace OwnIoc.Container
{
    public class Container : IContainer
    {
        Dictionary<Type, RegistrationModel> _instanceRegistration = new Dictionary<Type, RegistrationModel>(); 
        public void RegisterInstanceType<I, T>() where I : class where T : class
        {
            RegisterType<I, T>(Reg_Type.Instance);
        }

        public void RegisterSingletonType<I, T>() where I : class where T : class
        {
              RegisterType< I, T >(Reg_Type.Singleton);
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));    
        }


        private void RegisterType<I, C>(Reg_Type regType)
        {
            if (_instanceRegistration.ContainsKey(typeof (I)))
            {
                _instanceRegistration.Remove(typeof (I));
            }
            _instanceRegistration.Add(typeof(I), new RegistrationModel{RegType = regType, ObjectType = typeof(C)});
        }

        private object Resolve(Type t)
        {
            object obj = null;
            if (_instanceRegistration.ContainsKey(t))
            {
                RegistrationModel model = _instanceRegistration[t];
                if (model != null)
                {
                    Type typeToCreate = model.ObjectType;
                    ConstructorInfo[] cons = typeToCreate.GetConstructors();

                    var test = cons[0].CustomAttributes;
                    var test2 = cons[0].Attributes;
                    var dependentCtor =
                        cons.FirstOrDefault(
                            x =>
                                x.CustomAttributes.FirstOrDefault(
                                    att => att.AttributeType == typeof (DependencyAttribute)) != null);
                    var contrField = cons.FirstOrDefault(x => x.CustomAttributes != null);

                    if (contrField == null)
                    {
                        obj = CreateInstance(model);
                    }
                    else
                    {
                        ParameterInfo [] _params = contrField.GetParameters();
                        if (!_params.Any())
                        {
                            obj = CreateInstance(model);
                        }
                        else
                        {
                            List<object> defindedParams = new List<object>();
                            foreach (var info in _params)
                            {
                                Type type = info.ParameterType;
                                defindedParams.Add(this.Resolve(info.ParameterType));
                            }
                            obj = CreateInstance(model, defindedParams.ToArray());

                        }
                    }
                }
            }
            return obj;
        }


        private object CreateInstance(RegistrationModel model, object[] arg = null)
        {
            object obj = null;
            Type type = model.ObjectType;
            if (model.RegType == Reg_Type.Singleton)
            {
                obj = SingletonCreationService.GetInstance().GetSingleton(type, arg);
            }
            if (model.RegType == Reg_Type.Instance)
            {
                obj = InstanceCreationService.GetNewObject(type, arg);
            }
            return obj;
        } 
    }
}

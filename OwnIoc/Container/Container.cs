using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        //Saved Type and Contructro params name for creating
        private Dictionary<Type, string> _instanceParams = new Dictionary<Type, string>();
        //Type was send in SelfRegisterMethods
        private Type RegistratedInstance = null;
        //Temporary values for setting in created contructor
        private object _setValue = null;


        public void RegisterInstanceType<I, T>() where I : class where T : class
        {
            RegisterType<I, T>(Reg_Type.Instance);
        }

        public void RegisterSingletonType<I, T>() where I : class where T : class
        {
              RegisterType< I, T >(Reg_Type.Singleton);
        }

        public IContainer SelfRegisterInstanceType<I, T>() where I : class where T : class
        {
            this.RegisterInstanceType<I,T>();
            //SetObject witch will be created
            RegistratedInstance = typeof(T);
            return this;
        }

        public IContainer SelfRegisterSingletonType<I, T>() where I : class where T : class
        {
           this.RegisterSingletonType<I,T>();
           //SetObject witch will be created
            RegistratedInstance = typeof (T);
            return this;
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));    
        }

        public IContainer SetContructorParams<T>(string name, object value, Action<T,bool> expression = null) where T : class
        {
            _setValue = value;
            Type typeToCreate = RegistratedInstance;
            ConstructorInfo[] cons = typeToCreate.GetConstructors();
            var contrField = cons.FirstOrDefault(x => x.CustomAttributes != null);
            if (contrField != null)
            {
                ParameterInfo[] _params = contrField.GetParameters();
                var firstParam = _params.FirstOrDefault(x => x.Name == name);
                if (firstParam != null)
                {
                    var attributes = firstParam.Name;
                    if (!string.IsNullOrEmpty(attributes) && attributes == name)
                    {
                        //Add Type and founded contructor param
                      _instanceParams.Add(typeToCreate,attributes);
                    }
                }
            }
            return this;
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
                    var contrField = cons.FirstOrDefault(x => x.CustomAttributes != null);

                    if (contrField == null)
                    {
                        obj = CreateInstance(model);
                    }
                    else
                    {
                        object paramName = null;
                        ParameterInfo [] _params = contrField.GetParameters();
                        if (_instanceParams.ContainsKey(RegistratedInstance))
                        {
                            //Get Contructror parametr Name for setting in creation
                            paramName = _instanceParams[RegistratedInstance];
                        }
                        if (!_params.Any())
                        {
                            obj = CreateInstance(model);
                        }
                        else
                        {

                            List<object> defindedParams = new List<object>();
                            Dictionary<string, object> construct = new Dictionary<string, object>();

                            foreach (var info in _params)
                            {
                                //Add all Params as Key = ContrName(Uniq), Create Resolved Object)
                                construct.Add(info.Name, this.Resolve(info.ParameterType));
                                defindedParams.Add(this.Resolve(info.ParameterType));
                            }
                            //Find ContructorParametrs by Key
                            construct[(string)paramName] = _setValue;
                            var values = construct.Values.ToArray();
                            obj = CreateInstance(model, values);

                        }
                    }
                }
            }
            return obj;
        }

        //Create Instance Accoding type
        private object CreateInstance(RegistrationModel model, object[] arg = null, object [] attributes = null)
        {
            object obj = null;
            Type type = model.ObjectType;
            if (model.RegType == Reg_Type.Singleton)
            {
                obj = SingletonCreationService.GetInstance().GetSingleton(type, arg);
            }
            if (model.RegType == Reg_Type.Instance)
            {
                obj = InstanceCreationService.GetNewObject(type, arg, attributes);
            }
            return obj;
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using OwnIoc.Enumiration;
using OwnIoc.Interfaces;
using OwnIoc.Model;

namespace OwnIoc.Container
{
    public class Container : IContainer
    {
        //Saved Type and Contructro params name for creating
        private readonly Dictionary<Type, string> _instanceParams = new Dictionary<Type, string>();

        private readonly Dictionary<Type, RegistrationModel> _instanceRegistration =
            new Dictionary<Type, RegistrationModel>();

        //Temporary values for setting in created contructor
        private object _setValue;

        //Type was send in SelfRegisterMethods
        private Type RegistratedInstance;


        public void RegisterInstanceType<I, T>() where I : class where T : class
        {
            RegisterType<I, T>(RegType.Instance);
        }

        public void RegisterSingletonType<I, T>() where I : class where T : class
        {
            RegisterType<I, T>(RegType.Singleton);
        }

        public IContainer SelfRegisterInstanceType<I, T>() where I : class where T : class
        {
            RegisterInstanceType<I, T>();
            //SetObject witch will be created
            RegistratedInstance = typeof(T);
            return this;
        }

        public IContainer SelfRegisterSingletonType<I, T>() where I : class where T : class
        {
            RegisterSingletonType<I, T>();
            //SetObject witch will be created
            RegistratedInstance = typeof(T);
            return this;
        }

        public IContainer RegisterAsSelf<T>() where T : class
        {
            RegisterInstanceType<T,T>();
            RegistratedInstance = typeof(T);
            return this;
        }

        public T Resolve<T>()
        {
            return (T) Resolve(typeof(T));
        }

        public IContainer SetContructorParams<T>(string name, object value, Action<T, bool> expression = null)
            where T : class
        {
            _setValue = value;
            var typeToCreate = RegistratedInstance;
            var cons = typeToCreate.GetConstructors();
            var contrField = cons.FirstOrDefault(x => x.CustomAttributes != null);
            if (contrField != null)
            {
                var _params = contrField.GetParameters();
                var firstParam = _params.FirstOrDefault(x => x.Name == name);
                if (firstParam != null)
                {
                    var attributes = firstParam.Name;
                    if (!string.IsNullOrEmpty(attributes) && attributes == name)
                        _instanceParams.Add(typeToCreate, attributes);
                }
            }
            return this;
        }


        private void RegisterType<I, C>(RegType regType)
        {
            if (_instanceRegistration.ContainsKey(typeof(I)))
                _instanceRegistration.Remove(typeof(I));
            _instanceRegistration.Add(typeof(I), new RegistrationModel {RegType = regType, ObjectType = typeof(C)});
        }

        private object Resolve(Type t)
        {
            object obj = null;
            if (_instanceRegistration.ContainsKey(t))
            {
                var model = _instanceRegistration[t];
                if (model != null)
                {
                    var typeToCreate = model.ObjectType;
                    var cons = typeToCreate.GetConstructors();
                    var contrField = cons.FirstOrDefault(x => x.CustomAttributes != null);

                    if (contrField == null)
                    {
                        obj = CreateInstance(model);
                    }
                    else
                    {
                        object paramName = null;
                        var _params = contrField.GetParameters();
                        if (_instanceParams.ContainsKey(RegistratedInstance))
                            paramName = _instanceParams[RegistratedInstance];
                        if (!_params.Any())
                        {
                            obj = CreateInstance(model);
                        }
                        else
                        {
                            var defindedParams = new List<object>();
                            var construct = new Dictionary<string, object>();

                            foreach (var info in _params)
                            {
                                //Add all Params as Key = ContrName(Uniq), Create Resolved Object)
                                construct.Add(info.Name, Resolve(info.ParameterType));
                                defindedParams.Add(Resolve(info.ParameterType));
                            }
                            //Find ContructorParametrs by Key
                            if (!string.IsNullOrEmpty((string) paramName))
                                construct[(string) paramName] = _setValue;
                            var values = construct.Values.ToArray();
                            obj = CreateInstance(model, values);
                        }
                    }
                }
            }
            return obj;
        }

        //Create Instance Accoding type
        private object CreateInstance(RegistrationModel model, object[] arg = null, object[] attributes = null)
        {
            object obj = null;
            var type = model.ObjectType;
            if (model.RegType == RegType.Singleton)
                obj = SingletonCreationService.GetInstance().GetSingleton(type, arg);
            if (model.RegType == RegType.Instance)
                obj = InstanceCreationService.GetNewObject(type, arg, attributes);
            return obj;
        }
    }
}
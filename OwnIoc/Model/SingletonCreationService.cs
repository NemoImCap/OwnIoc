using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnIoc.Model
{
    internal class SingletonCreationService
    {
        static SingletonCreationService instance = null;
        static Dictionary<string, object> _objectsPool = new Dictionary<string, object>();

        static SingletonCreationService()
        {
            instance = new SingletonCreationService();
        }

        private SingletonCreationService()
        { }

        public static SingletonCreationService GetInstance()
        {
            return instance;
        }

        public object GetSingleton(Type t, object[] arguments = null)
        {
            object obj = null;
            try
            {
                if (_objectsPool.ContainsKey(t.Name) == false)
                {
                    obj = InstanceCreationService.GetNewObject(t, arguments);
                    _objectsPool.Add(t.Name, obj);
                }
                else
                {
                    obj = _objectsPool[t.Name];
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            return obj;
        }
    }
}

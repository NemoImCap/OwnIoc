using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnIoc.Model
{
    internal class InstanceCreationService
    {
        private static InstanceCreationService instance = null;

        static InstanceCreationService()
        {
            instance = new InstanceCreationService();
        }

        private InstanceCreationService()
        {
            
        }

        public static InstanceCreationService GetInstance()
        {
            return instance;
        }

        public static object GetNewObject(Type t, object[] arg)
        {
            object obj = null;

            try
            {
                obj = Activator.CreateInstance(t, arg);
            }
            catch (Exception ex)
            {
                
               Console.WriteLine(ex.Message);
            }
            return obj;
        }
    }
}

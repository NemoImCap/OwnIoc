using System;
using System.Linq;

namespace OwnIoc.Model
{
    internal class InstanceCreationService
    {
        private static readonly InstanceCreationService Instance;

        static InstanceCreationService()
        {
            Instance = new InstanceCreationService();
        }

        private InstanceCreationService()
        {
        }

        public static InstanceCreationService GetInstance()
        {
            return Instance;
        }

        public static object GetNewObject(Type t, object[] arg, object[] attributes = null)
        {
            object obj = null;

            try
            {
                obj = attributes != null && attributes.Any()
                    ? Activator.CreateInstance(t, arg, attributes)
                    : Activator.CreateInstance(t, arg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
    }
}
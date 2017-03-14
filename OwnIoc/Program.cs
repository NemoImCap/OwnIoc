using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OwnIoc.Interfaces;

namespace OwnIoc
{
    class Program
    {
        static void Main(string[] args)
        {
            IContainer container = new Container.Container();

            // testing instance type resigtration for class
            container.RegisterInstanceType<ITest1, ClassTest1>();
            var obj1 = container.Resolve<ITest1>();
            obj1.Print();


            // testing singleton registration for class
            container.RegisterSingletonType<ITest2, ClassTest2>();
            ITest2 obj5 = container.Resolve<ITest2>();
            obj5.Print();
            Console.Read();
        }
    }

    interface ITest1
    {
        void Print();
    }

    class ClassTest1 : ITest1
    {
        public string Name { get; set; }


        public ClassTest1(string name)
        {
            Name = name;
        }

        public void Print()
        {
            Console.WriteLine("ClassName: {0}, FIRST CLASS HashCode: {1}", this.GetType().Name, this.GetHashCode());
        }
    }

    interface ITest2
    {
        void Print();
    }

    class ClassTest2 : ITest2
    {
        public void Print()
        {
            Console.WriteLine("ClassName: {0}, SECOND CLASS HashCode: {1}", this.GetType().Name, this.GetHashCode());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OwnIoc.Interfaces;
using OwnIoc.Model;

namespace OwnIoc
{
    class Program
    {
        static void Main(string[] args)
        {
            IContainer container = new Container.Container();

            container.SelfRegisterInstanceType<ITest1, ClassTest1>().SetContructorParams<ClassTest1>("statusName","Teddy");
            //var a = new ResolveObject<ClassTest1>();
            // testing instance type resigtration for class
            //container.RegisterInstanceType<ITest1, ClassTest1>();
            var obj1 = container.Resolve<ITest1>();
            obj1.Print();


            // testing singleton registration for class

            //container.RegisterSingletonType<ITest2, ClassTest2>();
            container.SelfRegisterSingletonType<ITest2, ClassTest2>().SetContructorParams<ClassTest2>("number",34);
            ITest2 obj5 = container.Resolve<ITest2>();

            ITest2 obj3 = container.Resolve<ITest2>();
            obj5.Print();
            obj3.Print();
            Console.Read();
        }
    }

    interface ITest1
    {
        void Print();
    }

    class ClassTest1 : ITest1
    {
        public string StatusName { get; set; }

        public int Age { get; set; }


        public ClassTest1(string statusName, int age)
        {
            StatusName = statusName;
            Age = age;
        }

        public void Print()
        {
            Console.WriteLine("ClassName: {0}, FIRST CLASS HashCode: {1}, {2} , Age {3}", this.GetType().Name, this.GetHashCode(), this.StatusName, this.Age);
        }
    }

    interface ITest2
    {
        void Print();
    }

    class ClassTest2 : ITest2
    {
        public int Number { get; set; }

        public ClassTest2(int number)
        {
            Number = number;
        }

        public void Print()
        {
            Console.WriteLine("ClassName: {0}, SECOND CLASS HashCode: {1}, Nummber {2}", this.GetType().Name, this.GetHashCode(), this.Number);
        }
    }
}

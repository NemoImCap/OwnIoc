using System;
using OwnIoc.Interfaces;
using OwnIoc.Model;

namespace OwnIoc
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IContainer container = new Container.Container();
            container.SelfRegisterInstanceType<ITest1, ClassTest1>()
                .SetContructorParams<ClassTest1>("statusName", "Teddy");
            var obj1 = container.Resolve<ITest1>();
            obj1.Print();


            // testing singleton registration for class
            container.SelfRegisterSingletonType<ITest2, ClassTest2>().SetContructorParams<ClassTest2>("number", 34);
            var obj5 = container.Resolve<ITest2>();
            var obj3 = container.Resolve<ITest2>();
            obj5.Print();
            obj3.Print();


            container.RegisterAsSelf<ContainerAdapter>().SetContructorParams<ContainerAdapter>("name", "Frodo");

            var registerContainerAdapter = container.Resolve<ContainerAdapter>();
            Console.WriteLine("LastObject Hash is " + registerContainerAdapter.GetHashCode());
            Console.Read();
        }
    }

    //Test Classe1
    internal interface ITest1
    {
        void Print();
    }

    internal class ClassTest1 : ITest1
    {
        public ClassTest1(string statusName, int age)
        {
            StatusName = statusName;
            Age = age;
        }

        public string StatusName { get; set; }

        public int Age { get; set; }

        public void Print()
        {
            Console.WriteLine("ClassName: {0}, FIRST CLASS HashCode: {1}, {2} , Age {3}", GetType().Name, GetHashCode(),
                StatusName, Age);
        }
    }

    //Test Classe1
    internal interface ITest2
    {
        void Print();
    }

    internal class ClassTest2 : ITest2
    {
        public ClassTest2(int number)
        {
            Number = number;
        }

        public int Number { get; set; }

        public void Print()
        {
            Console.WriteLine("ClassName: {0}, SECOND CLASS HashCode: {1}, Nummber {2}", GetType().Name, GetHashCode(),
                Number);
        }
    }
}
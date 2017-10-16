using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingConstructorSample
{
    public interface ILog
    {
        void Write(string message);
    }

    public interface IConsole
    {

    }

    public class ConsoleLog : ILog, IConsole
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class EmailLog : ILog
    {
        private const string adminEmail = "admin@systemfeel.com";

        public void Write(string message)
        {
            Console.WriteLine($"Email sent to {adminEmail} : {message}");
        }
    }

    public class Engine
    {
        private ILog _log;
        private int _id;

        public Engine(ILog log)
        {
            _log = log;
            _id = new Random().Next();
        }

        public void Ahead(int power)
        {
            _log.Write($"Engine [{_id}] ahead {power}");
        }
    }

    public class Car
    {
        private Engine _engine;
        private ILog _log;

        public Car(Engine engine)
        {
            _engine = engine;
            _log = new EmailLog();
        }

        public void Go()
        {
            _engine.Ahead(200);
            _log.Write("Car going forward...");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>().As<ILog>();
            builder.RegisterType<Engine>();
            builder.RegisterType<Car>()
                .UsingConstructor(typeof(Engine));

            IContainer container = builder.Build();

            var car = container.Resolve<Car>();
            car.Go();
        }
    }
}

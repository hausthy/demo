using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TestDemo.Services;

namespace TestDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IocServiceTest>();
            serviceCollection.AddTransient<ServiceTestA>();

            var serviceProvider = serviceCollection.BuildServiceProvider();


            using (var scop = serviceProvider.CreateScope())
            {
                var service1 = scop.ServiceProvider.GetRequiredService<IocServiceTest>();

                Console.WriteLine($"service1 HashCode: {service1.GetHashCode()}");
                Console.WriteLine($"service1 Get: {service1.Get()}");
                Console.WriteLine($"service1 GetOrderId: {service1.GetOrderId()}");
                service1.Set(123);
                service1.SetOrder(new Models.Order { OrderId = "1230999" });
                Console.WriteLine($"service1 HashCode: {service1.GetHashCode()}");
                Console.WriteLine($"service1 Get: {service1.Get()}");
                Console.WriteLine($"service1 GetOrderId: {service1.GetOrderId()}");

                var service2 = scop.ServiceProvider.GetRequiredService<IocServiceTest>();

                Console.WriteLine($"service2 HashCode: {service1.GetHashCode()}");
                Console.WriteLine($"service2 Get: {service1.Get()}");
                Console.WriteLine($"service2 GetOrderId: {service1.GetOrderId()}");

                var serviceA = scop.ServiceProvider.GetRequiredService<ServiceTestA>();

                Task.Run(() =>
                {
                    var serviceB = scop.ServiceProvider.GetRequiredService<ServiceTestA>();

                    serviceB.Set(567);
                    service1.SetOrderId("1230999 New");

                    var serviceC = scop.ServiceProvider.GetRequiredService<ServiceTestA>();
                }).Wait();

                var service3 = scop.ServiceProvider.GetRequiredService<IocServiceTest>();

                Console.WriteLine($"service3 HashCode: {service1.GetHashCode()}");
                Console.WriteLine($"service3 Get: {service1.Get()}");
                Console.WriteLine($"service3 GetOrderId: {service1.GetOrderId()}");
                Console.ReadLine();
            }

            using (var scop = serviceProvider.CreateScope())
            {
                var service1 = scop.ServiceProvider.GetRequiredService<IocServiceTest>();

                Console.WriteLine($"service1 HashCode: {service1.GetHashCode()}");
                Console.WriteLine($"service1 Get: {service1.Get()}");
                Console.WriteLine($"service1 GetOrderId: {service1.GetOrderId()}");
                service1.Set(1230);
                Console.WriteLine($"service1 HashCode: {service1.GetHashCode()}");
                Console.WriteLine($"service1 Get: {service1.Get()}");
                Console.WriteLine($"service1 GetOrderId: {service1.GetOrderId()}");

                var service2 = scop.ServiceProvider.GetRequiredService<IocServiceTest>();

                Console.WriteLine($"service2 HashCode: {service1.GetHashCode()}");
                Console.WriteLine($"service2 Get: {service1.Get()}");
                Console.WriteLine($"service2 GetOrderId: {service1.GetOrderId()}");

                var serviceA = scop.ServiceProvider.GetRequiredService<ServiceTestA>();

                Task.Run(() =>
                {
                    var serviceB = scop.ServiceProvider.GetRequiredService<ServiceTestA>();

                    serviceB.Set(5670);

                    var serviceC = scop.ServiceProvider.GetRequiredService<ServiceTestA>();
                }).Wait();

                var service3 = scop.ServiceProvider.GetRequiredService<IocServiceTest>();

                Console.WriteLine($"service3 HashCode: {service1.GetHashCode()}");
                Console.WriteLine($"service3 Get: {service1.Get()}");
                Console.WriteLine($"service3 GetOrderId: {service1.GetOrderId()}");

                Console.ReadLine();
            }
        }
    }
}

//https://www.cnblogs.com/ancupofcoffee/p/4908554.html
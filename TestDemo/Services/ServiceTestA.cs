using System;
using System.Collections.Generic;
using System.Text;

namespace TestDemo.Services
{
    public class ServiceTestA
    {
        private readonly IocServiceTest _iocServiceTest;

        public ServiceTestA(IocServiceTest iocServiceTest)
        {
            _iocServiceTest = iocServiceTest;
            Console.WriteLine($"ServiceTestA HashCode: {iocServiceTest.GetHashCode()}");
            Console.WriteLine($"ServiceTestA Get: {iocServiceTest.Get()}");
        }

        public void Set(int val)
        {
            _iocServiceTest.Set(val);
        }
    }
}

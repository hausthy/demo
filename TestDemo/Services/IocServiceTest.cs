using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TestDemo.Services
{
    public class IocServiceTest
    {
        private static readonly AsyncLocal<int> AsyncLocalUow = new AsyncLocal<int>();

        public int Get()
        {
            return AsyncLocalUow.Value;
        }

        public void Set(int val)
        {
            AsyncLocalUow.Value = val;
        }
    }
}

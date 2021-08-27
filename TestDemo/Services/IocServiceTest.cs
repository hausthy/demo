using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TestDemo.Models;

namespace TestDemo.Services
{
    public class IocServiceTest
    {
        private static readonly AsyncLocal<int> AsyncLocalUow = new AsyncLocal<int>();
        private static readonly AsyncLocal<Order> AsyncLocalOrder = new AsyncLocal<Order>();

        public IocServiceTest()
        {
        }

        public int Get()
        {
            return AsyncLocalUow.Value;
        }

        public void Set(int val)
        {
            AsyncLocalUow.Value = val;
        }

        public string GetOrderId()
        {
            return AsyncLocalOrder.Value?.OrderId;
        }

        public void SetOrder(Order order)
        {
            AsyncLocalOrder.Value = order;
        }

        public void SetOrderId(string orderId)
        {
            AsyncLocalOrder.Value.OrderId = orderId;
        }
    }
}

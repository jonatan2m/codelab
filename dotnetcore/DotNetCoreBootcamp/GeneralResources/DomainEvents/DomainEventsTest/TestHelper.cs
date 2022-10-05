using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace DomainEventsTest
{
    public class TestHelper
    {
        public ServiceProvider ServiceProvider { get; }

        public TestHelper()
        {
            var services = new ServiceCollection();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}

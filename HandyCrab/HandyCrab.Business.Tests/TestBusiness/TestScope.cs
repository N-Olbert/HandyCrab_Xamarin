using System;
using System.Threading;
using HandyCrab.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HandyCrab.Business.Tests.TestBusiness
{
    internal class TestScope : IDisposable
    {
        private static readonly AsyncLocal<TestScope> ContextBackingField = new AsyncLocal<TestScope>();
        private readonly ServiceCollection FactoryServiceCollection = Factory.FTO_GetStandardServiceCollection();

        internal static TestScope Context => ContextBackingField.Value;

        private TestScope()
        {
            Assert.IsNull(ContextBackingField.Value);
            ContextBackingField.Value = this;
        }

        internal static TestScope GetScope() => new TestScope();

        public void ReplaceFactoryInstance(Type type, object instance)
        {
            this.FactoryServiceCollection.Replace(new ServiceDescriptor(type, instance));
            Factory.FTO_TestDataProvider.Value = this.FactoryServiceCollection.BuildServiceProvider();
        }

        public void Dispose()
        {
            Factory.FTO_TestDataProvider.Value = null; // reset
            ContextBackingField.Value = null;
        }
    }
}
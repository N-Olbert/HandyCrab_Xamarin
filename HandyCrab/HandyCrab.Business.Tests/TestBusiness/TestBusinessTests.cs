using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using HandyCrab.Business.Services;
using HandyCrab.Business.Services.BusinessObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]
namespace HandyCrab.Business.Tests.TestBusiness
{
    [TestClass]
    public class TestBusinessTests
    {
        [TestMethod]
        public void ScopeIsolatesFactory()
        {
            //No changes
            Assert.IsInstanceOfType(Factory.Get<IGeolocationService>(), typeof(GeolocationService));
            using (TestScope.GetScope())
            {
                var fake = A.Fake<IGeolocationService>();
                TestScope.Context.ReplaceFactoryInstance(typeof(IGeolocationService), fake);

                //Fake is injected and must be returned
                Assert.AreEqual(fake, Factory.Get<IGeolocationService>());
            }

            //No Changes anymore
            Assert.IsInstanceOfType(Factory.Get<IGeolocationService>(), typeof(GeolocationService));
        }

        [TestMethod]
        public void ScopeIsolatesFactoryInMultithreadedEnvironment()
        {
            //No changes
            Assert.IsInstanceOfType(Factory.Get<IGeolocationService>(), typeof(GeolocationService));

            const int amount = 250;
            var tasks = new Task[amount];
            for (int i = 0; i < amount; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    using (TestScope.GetScope())
                    {
                        var fake = A.Fake<IGeolocationService>();
                        TestScope.Context.ReplaceFactoryInstance(typeof(IGeolocationService), fake);

                        Thread.Yield(); // suggest change

                        //Fake is injected and must be returned
                        Assert.AreEqual(fake, Factory.Get<IGeolocationService>());
                    }
                });
            }

            Task.WaitAll(tasks);

            //No Changes anymore
            Assert.IsInstanceOfType(Factory.Get<IGeolocationService>(), typeof(GeolocationService));
        }
    }
}
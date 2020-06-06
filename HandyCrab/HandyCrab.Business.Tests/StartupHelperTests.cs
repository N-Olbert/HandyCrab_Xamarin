using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using FakeItEasy;
using HandyCrab.Business.Services;
using HandyCrab.Business.Services.BusinessObjects;
using HandyCrab.Business.Tests.TestBusiness;
using HandyCrab.Common;
using HandyCrab.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Essentials;

namespace HandyCrab.Business.Tests
{
    [TestClass]
    public class StartupHelperTests
    {
        [ScopedTestMethod]
        public void StartupHelperRegistersAllViewModelInstances()
        {
            var viewModelInterfaceTypes = typeof(IAboutViewModel)
                                          .Assembly.GetTypes()
                                          .Where(x => x.Name.EndsWith("ViewModel") && x != typeof(IViewModel) && x != typeof(IImagePickerViewModel))
                                          .ToList();
            foreach (var interfaceType in viewModelInterfaceTypes)
            {
                //Types aren't registered yet;
                var method = typeof(ViewModelFactory)
                             .GetMethod(nameof(ViewModelFactory.GetInstance)).MakeGenericMethod(interfaceType);
                var exception = Assert.ThrowsException<TargetInvocationException>(() => method.Invoke(null, null));
                Assert.IsInstanceOfType(exception.InnerException, typeof(InvalidOperationException));
            }

            StartupHelper.Prepare();

            //Replace GeolocationService because it is used by ctor of SearchViewModel
            var fakeGeloactionService = A.Fake<IGeolocationService>();
            TestScope.Context.ReplaceFactoryInstance(typeof(IGeolocationService), fakeGeloactionService);
           
            foreach (var interfaceType in viewModelInterfaceTypes)
            {
                //Types are registered yet!
                var method = typeof(ViewModelFactory)
                             .GetMethod(nameof(ViewModelFactory.GetInstance))
                             .MakeGenericMethod(interfaceType);
                var vm = method.Invoke(null, null);
                
                Assert.IsNotNull(vm);
                Assert.AreEqual(vm.GetType().Name, interfaceType.Name.Substring(1));
            }
        }
    }
}

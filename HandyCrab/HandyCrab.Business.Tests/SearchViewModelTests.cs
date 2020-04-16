using System;
using System.Threading.Tasks;
using FakeItEasy;
using HandyCrab.Business.Services.BusinessObjects;
using HandyCrab.Business.Tests.TestBusiness;
using HandyCrab.Business.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Essentials;

namespace HandyCrab.Business.Tests
{
    [TestClass]
    public class SearchViewModelTests
    {
        [ScopedTestMethod]
        public void SearchViewModelTakesLastKnownLocationOnStartAndUpdatesItLaterToMorePreciseLocation()
        {
            var lastKnownLocation = new Location(40.730610, -73.935242); //New York
            var exactCurrentLocation = new Location(52.520008, 13.404954); //Berlin
            const string NYC = "New York City";
            const string Berlin = "Berlin";

            //Arrange
            var fake = A.Fake<IGeolocationService>();

            //immediately return last known location
            A.CallTo(() => fake.GetLastKnownLocationAsync()).Returns(lastKnownLocation);

            var delayedUpdateFunc = Task.Run(async () =>
            {
                await Task.Delay(1000);
                return exactCurrentLocation;
            });
            A.CallTo(() => fake.GetLocationAsync(A<GeolocationRequest>._)).Returns(delayedUpdateFunc);
            A.CallTo(() => fake.GetPlacemarksAsync(A<Location>._)).Throws<InvalidOperationException>();
            A.CallTo(() => fake.GetPlacemarksAsync(A<Location>.That.Matches(x => x.Longitude == lastKnownLocation.Longitude &&
                                                                                 x.Latitude == lastKnownLocation.Latitude)))
             .Returns(new[] { new Placemark { Locality = NYC, Location = lastKnownLocation} });
            A.CallTo(() => fake.GetPlacemarksAsync(A<Location>.That.Matches(x => x.Longitude == exactCurrentLocation.Longitude &&
                                                                                 x.Latitude == exactCurrentLocation.Latitude)))
             .Returns(new[] { new Placemark { Locality = Berlin, Location = exactCurrentLocation } });

            TestScope.Context.ReplaceFactoryInstance(typeof(IGeolocationService), fake);

            //Act
            var instance = new SearchViewModel();

            //Assert
            Task.Delay(5).Wait();
            Assert.AreEqual(instance.CurrentPlacemark.Locality, NYC);
            Task.Delay(1005).Wait();
            Assert.AreEqual(instance.CurrentPlacemark.Locality, Berlin);
        }
    }
}
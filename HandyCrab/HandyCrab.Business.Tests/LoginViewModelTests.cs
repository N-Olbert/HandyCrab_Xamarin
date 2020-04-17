using System;
using FakeItEasy;
using HandyCrab.Business.Services;
using HandyCrab.Business.ViewModels;
using HandyCrab.Common.Entitys;
using HandyCrab.Business.Tests.TestBusiness;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HandyCrab.Business.Tests
{
    [TestClass]
    public class LoginViewModelTests
    {
        [ScopedTestMethod]
        public void LoginSuccessful()
        {
            //Arrange
            var fake = A.Fake<ILoginClient>();

            A.CallTo(() => fake.LoginAsync(A<Login>._)).Returns(new Failable<User>(new User(), System.Net.HttpStatusCode.OK));

            TestScope.Context.ReplaceFactoryInstance(typeof(ILoginClient), fake);

            var instance = new LoginViewModel();
            instance.UserName = "testUsername";
            instance.Password = "testPassword";

            Assert.IsTrue(instance.LoginCommand.CanExecute(null));
            Assert.IsTrue(instance.IsUserNameValid);
            Assert.IsTrue(instance.IsPasswordValid);

            bool succeeded = false;
            instance.LoginSucceeded += (sender, args) => succeeded = true;
            instance.LoginCommand.Execute(null);

            Assert.IsTrue(succeeded);
        }
        [ScopedTestMethod]
        public void LoginRejected()
        {
            //Arrange
            var fake = A.Fake<ILoginClient>();

            A.CallTo(() => fake.LoginAsync(A<Login>._)).Returns(new Failable<User>(new Exception()));

            TestScope.Context.ReplaceFactoryInstance(typeof(ILoginClient), fake);

            var instance = new LoginViewModel();
            instance.UserName = "testUsername";
            instance.Password = "testPassword";

            Assert.IsTrue(instance.LoginCommand.CanExecute(null));
            Assert.IsTrue(instance.IsUserNameValid);
            Assert.IsTrue(instance.IsPasswordValid);

            bool rejected = false;
            instance.LoginRejected += (sender, args) => rejected = true;
            instance.LoginCommand.Execute(null);

            Assert.IsTrue(rejected);
        }
        [ScopedTestMethod]
        public void CannotTriggerLoginWithoutUsernameOrPassword()
        {
            var instance = new LoginViewModel();

            Assert.IsTrue(!instance.LoginCommand.CanExecute(null));

            instance.UserName = "testUsername";

            Assert.IsTrue(!instance.LoginCommand.CanExecute(null));

            instance.UserName = "";
            instance.Password = "testPassword";

            Assert.IsTrue(!instance.LoginCommand.CanExecute(null));

            instance.UserName = "testUsername";

            Assert.IsTrue(instance.LoginCommand.CanExecute(null));
        }
    }
}

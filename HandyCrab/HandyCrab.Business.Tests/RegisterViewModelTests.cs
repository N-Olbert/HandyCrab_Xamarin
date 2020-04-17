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
    public class RegisterViewModelTests
    {
        [ScopedTestMethod]
        public void RegisterSuccessful()
        {
            //Arrange
            var fake = A.Fake<IRegisterClient>();

            A.CallTo(() => fake.RegisterAsync(A<UserWithPassword>._)).Returns(new Failable<User>(new User(), System.Net.HttpStatusCode.OK));

            TestScope.Context.ReplaceFactoryInstance(typeof(IRegisterClient), fake);

            var instance = new RegisterViewModel();
            instance.UserName = "testUsername";
            instance.Email = "test@email.com";
            instance.Password = "testPassword";

            Assert.IsTrue(instance.LoginCommand.CanExecute(null));
            Assert.IsTrue(instance.IsUserNameValid);
            Assert.IsTrue(instance.IsEmailValid);
            Assert.IsTrue(instance.IsPasswordValid);

            bool succeeded = false;
            instance.RegisterSucceeded += (sender, args) => succeeded = true;
            instance.RegisterCommand.Execute(null);

            Assert.IsTrue(succeeded);
        }

        [ScopedTestMethod]
        public void RegisterRejected()
        {
            //Arrange
            var fake = A.Fake<IRegisterClient>();

            A.CallTo(() => fake.RegisterAsync(A<UserWithPassword>._)).Returns(new Failable<User>(new Exception()));

            TestScope.Context.ReplaceFactoryInstance(typeof(IRegisterClient), fake);

            var instance = new RegisterViewModel();
            instance.UserName = "testUsername";
            instance.Email = "test@email.com";
            instance.Password = "testPassword";

            Assert.IsTrue(instance.LoginCommand.CanExecute(null));
            Assert.IsTrue(instance.IsUserNameValid);
            Assert.IsTrue(instance.IsEmailValid);
            Assert.IsTrue(instance.IsPasswordValid);

            bool rejected = false;
            instance.RegisterRejected += (sender, args) => rejected = true;
            instance.RegisterCommand.Execute(null);

            Assert.IsTrue(rejected);
        }

        [ScopedTestMethod]
        public void CannotTriggerRegisterWithoutUsernameEmailOrPassword()
        {
            var instance = new RegisterViewModel();

            Assert.IsTrue(!instance.RegisterCommand.CanExecute(null));

            instance.UserName = "testUsername";

            Assert.IsTrue(!instance.RegisterCommand.CanExecute(null));

            instance.UserName = "";
            instance.Password = "testPassword";

            Assert.IsTrue(!instance.RegisterCommand.CanExecute(null));

            instance.Password = "";
            instance.Email = "test@email.com";

            Assert.IsTrue(!instance.RegisterCommand.CanExecute(null));

            instance.UserName = "testUsername";
            instance.Password = "testPassword";
            instance.Email = "";

            Assert.IsTrue(!instance.RegisterCommand.CanExecute(null));

            instance.Password = "";
            instance.Email = "test@email.com";

            Assert.IsTrue(!instance.RegisterCommand.CanExecute(null));

            instance.UserName = "";
            instance.Password = "testPassword";

            Assert.IsTrue(!instance.RegisterCommand.CanExecute(null));

            instance.UserName = "testUsername";

            Assert.IsTrue(instance.LoginCommand.CanExecute(null));
        }
    }
}

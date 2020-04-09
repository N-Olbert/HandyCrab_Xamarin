using System;
using HandyCrab.Business.Services.BusinessObjects;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace HandyCrab.Business.Services
{
    internal static class Factory
    {
        private static readonly IServiceProvider Provider = GetProvider();

        [NotNull]
        public static T Get<T>()
        {
            return Provider.GetService<T>();
        }

        private static IServiceProvider GetProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            serviceCollection.AddHttpClient<IRegisterClient, RegisterClient>();
            serviceCollection.AddHttpClient<ILoginClient, LoginClient>();
            serviceCollection.AddSingleton<IImageService, ImageService>();
            serviceCollection.AddSingleton<ISecureStorage, SecureStorage>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
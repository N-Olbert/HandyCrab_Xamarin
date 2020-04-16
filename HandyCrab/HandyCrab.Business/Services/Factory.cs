using System;
using System.Threading;
using HandyCrab.Business.Fundamentals;
using HandyCrab.Business.Services.BusinessObjects;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace HandyCrab.Business.Services
{
    internal static class Factory
    {
        private static readonly IServiceProvider Provider = GetProvider();
        internal static AsyncLocal<IServiceProvider> FTO_TestDataProvider { get; } = new AsyncLocal<IServiceProvider>();

        internal static ServiceCollection FTO_GetStandardServiceCollection()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            serviceCollection.AddHttpClient<IRegisterClient, RegisterClient>();
            serviceCollection.AddHttpClient<ILoginClient, LoginClient>();
            serviceCollection.AddSingleton<IImageService, ImageService>();
            serviceCollection.AddSingleton<ISecureStorage, SecureStorage>();
            serviceCollection.AddSingleton<IGeolocationService, GeolocationService>();
            serviceCollection.AddSingleton<IInternalRuntimeDataStorageService, InternalRuntimeDataStorageService>();
            return serviceCollection;
        }

        [NotNull]
        public static T Get<T>()
        {
            return (FTO_TestDataProvider?.Value ?? Provider).GetService<T>();
        }

        private static IServiceProvider GetProvider()
        {
            return FTO_GetStandardServiceCollection().BuildServiceProvider();
        }
    }
}
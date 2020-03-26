using System;
using System.Collections.Concurrent;
using HandyCrab.Common.Interfaces;
using JetBrains.Annotations;

namespace HandyCrab.Common
{
    public static class ViewModelFactory
    {
        [NotNull]
        private static  readonly ConcurrentDictionary<Type, Func<object>> FactoryMethods = new ConcurrentDictionary<Type, Func<object>>();
        
        public static T GetInstance<T>() where T : class, IViewModel
        {
            if (FactoryMethods.TryGetValue(typeof(T), out var factory))
            {
                return (T) factory?.Invoke();
            }

            throw new InvalidOperationException("No factory found");
        }

        public static void RegisterInstance<TInterface, TImplementation>() 
            where TInterface : class, IViewModel
            where TImplementation : TInterface, new()
        {
            FactoryMethods.TryAdd(typeof(TInterface), () => new TImplementation());
        }
    }
}

using System.Runtime.CompilerServices;
using HandyCrab.Common.Interfaces;

namespace HandyCrab.Common.Entitys
{
    public static class UsernameResolverSingletonHolder
    {
        public static IUserNameResolver Instance { get; private set; }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void SetInstance(IUserNameResolver resolver)
        {
            Instance = resolver;
        }
    }
}
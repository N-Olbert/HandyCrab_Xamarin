using System;
using System.Collections.Concurrent;

namespace HandyCrab.Business.Fundamentals
{
    internal class InternalRuntimeDataStorageService : IInternalRuntimeDataStorageService
    {
        private static readonly ConcurrentDictionary<StorageSlot, object> Storage = new ConcurrentDictionary<StorageSlot, object>();

        public static EventHandler<StorageSlot> StorageValueChanged;

        protected void RaiseStorageValueChanged(StorageSlot key)
        {
            StorageValueChanged?.Invoke(this, key);
        }

        public virtual T GetValue<T>(StorageSlot key)
        {
            if (Storage.TryGetValue(key, out var value))
            {
                return (T) value;
            }

            return default;
        }

        public virtual void StoreValue(StorageSlot key, object value)
        {
            Storage[key] = value;
            StorageValueChanged?.Invoke(this, key);
        }
    }
}
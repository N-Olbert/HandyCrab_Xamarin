namespace HandyCrab.Business.Fundamentals
{
    public interface IInternalRuntimeDataStorageService
    {
        T GetValue<T>(StorageSlot key);

        void StoreValue(StorageSlot key, object value);
    }
}
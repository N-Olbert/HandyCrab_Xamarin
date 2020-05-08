using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace HandyCrab.Common.Entitys
{
    public class Barrier : BarrierBase, IReadOnlyBarrier, INotifyPropertyChanged
    {
        private string userName;
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonIgnore]
        public int DistanceToLocation { get; set; }

        [JsonIgnore]
        public string UserName
        {
            get
            {
                if (this.userName == default)
                {
                    GetUserName();
                }
                
                return this.userName;
            }
        }

        private async void GetUserName()
        {
            //async void is ok 
            try
            {
                var name = await UsernameResolverSingletonHolder.Instance.GetUserNameAsync(UserId);
                SetProperty(ref this.userName, name, nameof(UserName));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected void SetProperty<T>(ref T backingStore, T value, string propertyName)
        {
            if (!EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                backingStore = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace HandyCrab.Business.Services.BusinessObjects
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
                                                                      {
                                                                          NullValueHandling = NullValueHandling.Ignore
                                                                      };
         public static StringContent ToJsonContent(this object obj)
        {
            var myContent = JsonConvert.SerializeObject(obj, Settings);
            return new StringContent(myContent, Encoding.UTF8, "application/json");
        }

        public static T DeserializeJson<T>(this string json)
        {
            try
            {
                return typeof(T) == typeof(object) ? default : JsonConvert.DeserializeObject<T>(json, Settings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
        }
    }
}
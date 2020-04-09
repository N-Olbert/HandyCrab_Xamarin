using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace HandyCrab.Business.Services.BusinessObjects
{
    public static class JsonExtensions
    {
        public static StringContent ToJsonContent(this object obj)
        {
            var myContent = JsonConvert.SerializeObject(obj);
            return new StringContent(myContent, Encoding.UTF8, "application/json");
        }

        public static T DeserializeJson<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
        }
    }
}
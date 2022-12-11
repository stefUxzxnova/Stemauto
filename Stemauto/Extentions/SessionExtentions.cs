using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text.Json;

namespace Stemauto.Extentions
{
    public static class SessionExtentions
    {
        public static void SetObject<T>(this ISession instance, string key, T value)
            where T : class
        {
            if (value == null)
            {
                instance.Remove(key);
                return;
            }
            string jsonData = JsonSerializer.Serialize(value);
            instance.SetString(key, jsonData);
        }

        public static T GetObject<T>(this ISession instance, string key)
            where T : class 
        {
            
            string jsonData = instance.GetString(key);
            if (jsonData == null)
            {
                return null;
            }
           return JsonSerializer.Deserialize<T>(jsonData);
            
        }

    }
}

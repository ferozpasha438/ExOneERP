using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CIN.Application
{
    public class Utility
    {
        public static void Mapper<TS, TD>(object source, object destination)
        {
            if (source != null && destination != null)
            {
                var sourcePropInfo = typeof(TS).GetProperties();
                var destinationPropInfo = typeof(TD).GetProperties();
                var destinationType = destination.GetType();
                var sourceType = source.GetType();
                foreach (var prop in sourcePropInfo)
                {
                    if (destinationPropInfo.All(x => x.Name != prop.Name)) continue;
                    var sourceValue = sourceType.GetProperty(prop.Name).GetValue(source, null);

                    //setting the default values for ModifiedBy Column in All Defination Screens
                    if (prop.Name == "ModifiedByImage" && sourceValue == null)
                    {
                        sourceValue = "user-img.jpg";
                    }
                    if (prop.Name == "ModifiedByName" && sourceValue == null)
                    {
                        sourceValue = "  N/A";
                    }
                    if (prop.Name == "ModifiedDate" && sourceValue == null)
                    {
                        sourceValue = "  N/A";
                    }
                    destinationType.GetProperty(prop.Name).SetValue(destination, sourceValue, null);
                }
            }
        }

        public static bool HasList<T>(System.Collections.Generic.List<T> items) => items.Any();

        /// <summary>
        /// returns true, if Not Credit Pay
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotCreditPay(string value) => value.Trim().ToLower() != "credit";
    }

    public static class ExtensionHelper
    {
        public static void SetCache<T>(this IMemoryCache _cache, string key, T data)
        {
            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromSeconds(36000));
            var jsonData = JsonConvert.SerializeObject(data);
            // Save data in cache.
            _cache.Set(key, jsonData);//, cacheEntryOptions);
        }
        public static T GetCache<T>(this IMemoryCache _cache, string key)
        {
            var jsonData = _cache.Get(key);
            return jsonData is null ? default(T) : JsonConvert.DeserializeObject<T>(jsonData.ToString());
        }

        public static void ClearCache(this IMemoryCache _cache)
        {
            _cache.ClearCache();
        }
    }
}
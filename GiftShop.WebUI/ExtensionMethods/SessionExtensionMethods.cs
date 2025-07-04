using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GiftShop.WebUI.ExtensionMethods
{
    public static class SessionExtensionMethods
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? GetJson<T>(this ISession session, string key) where T : class
        {
            var data = session.GetString(key);
            return data == null ? null : JsonConvert.DeserializeObject<T>(data);
        }
    }
}

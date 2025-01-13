using System.Text.Json;

namespace VrijwilligersWerkApp.Extensions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            if (value == null)
            {
                session.Remove(key);
                return;
            }

            var jsonData = JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            session.SetString(key, jsonData);
        }

        public static T Get<T>(this ISession session, string key)
        {
            var jsonData = session.GetString(key);
            if (string.IsNullOrEmpty(jsonData))
            {
                return default;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(jsonData, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch
            {
                return default;
            }
        }

        public static bool TryGetValue<T>(this ISession session, string key, out T value)
        {
            value = session.Get<T>(key);
            return value != null;
        }
    }
}

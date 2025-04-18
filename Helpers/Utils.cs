using System.Collections.Generic;
using System.Text.Json;

namespace RAZOR_PAGE_KOPYASI.Helpers
{
    public sealed class Utils
    {
        private static readonly Utils _instance = new Utils();
        public static Utils Instance => _instance;
        private Utils() { }

        public string ExportToJson<T>(IEnumerable<T> data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(data, options);
        }
    }
}

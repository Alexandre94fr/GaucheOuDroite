using Newtonsoft.Json;

namespace Shared.Tools
{
    public static class ObjectToStringFormatter
    {
        public static string ObjectToString(object p_result, Formatting p_formating = Formatting.Indented)
        {
            return $"{JsonConvert.SerializeObject(p_result, p_formating)}";
        }
    }
}
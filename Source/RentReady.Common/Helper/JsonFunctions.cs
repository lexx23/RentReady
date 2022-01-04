using Newtonsoft.Json;

namespace RentReady.Common.Helper
{
    public static class JsonFunctions
    {
        public static bool TryParseJson<T>(string data, out T result)
        {
            var success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(data, settings);
            return success;
        }
    }
}
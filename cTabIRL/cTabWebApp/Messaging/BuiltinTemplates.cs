using System.Text.Json;

namespace cTabWebApp.Messaging
{
    public static class BuiltinTemplates
    {
        public static MessageTemplate GetMedevac()
        {
            return JsonSerializer.Deserialize<MessageTemplate>(
                    SharedResource.BuiltinMedevacTemplateJson, 
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
        }
    }
}

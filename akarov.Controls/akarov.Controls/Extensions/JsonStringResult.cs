using akarov.Controls.Utils;
using Newtonsoft.Json;

namespace akarov.Controls.Extensions.Json
{
    public static class JsonStringResult
    {
        public static string Serialize(this object data)
        {
            if (data == null) return "";

            var SerializerSettings = new JsonSerializerSettings();

            SerializerSettings.Converters.Add(new JsonNetDateConverter());


            return JsonConvert.SerializeObject(data, Formatting.Indented, SerializerSettings);
            
        }
        
       

    }
}
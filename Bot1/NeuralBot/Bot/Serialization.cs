using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    public class Serialization
    {
        public static JObject SerializeObject (ISerializable obj)
        {
            JObject jobj = obj.Serialize();
            return new JObject()
            {
                { "$Type", new JValue(obj.GetType().FullName) },
                { "$Data", jobj }
            };
        }

        public static object DeserializeObject (JObject obj)
        {
            // Not robust but shut up.
            Type type = Type.GetType(obj["$Type"].ToObject<string>());
            JObject data = obj["$Data"] as JObject;
            ISerializable serializable = (ISerializable)Activator.CreateInstance(type);
            serializable.Deserialize(data);
            return serializable;
        }

        public static T DeserializeObject<T>(JObject obj) => (T)DeserializeObject(obj);
    }
}

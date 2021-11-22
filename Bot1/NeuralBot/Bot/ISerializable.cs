using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    public interface ISerializable
    {
        JObject Serialize();

        void Deserialize(JObject source);
    }
}

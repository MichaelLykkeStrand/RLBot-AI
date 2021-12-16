using Bot.ANN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Bot.Utilities
{
    public static class DataSetIO
    {
        // This'll use binary serialization over my dead body.
        // Alternatively someone else can do it.

        public static void StoreDataSets(List<DataSet> dataset, string path)
        {
            JToken token = JToken.FromObject(dataset);
            File.WriteAllText(path, token.ToString());
        }

        public static List<DataSet> LoadDataSets (string path)
        {
            string text = File.ReadAllText(path);
            JObject obj = JObject.Parse(text);
            return obj.ToObject<List<DataSet>>();
        }

        public static List<DataSet>[] LoadAllDataSets (string directory)
        {
            string[] files = Directory.GetFiles(directory);
            var results = new List<DataSet>[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                results[i] = LoadDataSets(files[i]);
            }
            return results;
        }
    }
}

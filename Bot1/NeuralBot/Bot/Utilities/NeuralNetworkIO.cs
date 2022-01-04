using Bot.ANN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bot.Utilities
{
    public static class NeuralNetworkIO
    {
        // This'll use binary serialization over my dead body.
        // Alternatively someone else can do it.

        public static void StoreDataSets(List<DataSet> dataset, string path)
        {
            StoreObject(dataset, path);
        }

        public static List<DataSet> LoadDataSets (string path)
        {
            return LoadObject<List<DataSet>>(path);
        }

        public static List<DataSet>[] LoadAllDataSets (string directory)
        {
            return LoadAllObjects<List<DataSet>>(directory);
        }

        public static void StoreBrain (ANN.ANN brain, string path)
        {
            BinarySerialize(brain, path);
        }

        public static ANN.ANN LoadBrain (string path)
        {
            return BinaryDeserialize<ANN.ANN>(path);
        }

        private static void StoreObject (object obj, string path)
        {
            JToken token = JToken.FromObject(obj);
            File.WriteAllText(path, token.ToString(Formatting.None));
        }

        private static void BinarySerialize(object obj, string path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, obj);
            stream.Close();
        }

        private static T BinaryDeserialize<T>(string path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
            object obj = formatter.Deserialize(stream);
            return (T)obj;
        }

        private static T LoadObject<T> (string path)
        {
            string text = File.ReadAllText(path);
            JToken obj = JToken.Parse(text);
            return obj.ToObject<T>();
        }

        private static T[] LoadAllObjects<T> (string directory)
        {
            string[] files = Directory.GetFiles(directory);
            var results = new T[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                results[i] = LoadObject<T>(files[i]);
            }
            return results;
        }
    }
}

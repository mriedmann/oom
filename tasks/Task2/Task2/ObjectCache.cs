using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class ObjectCache
    {
        public static ObjectCache Instance {
            get {
                if (instance == null)
                    instance = new ObjectCache();
                return instance;
            }
        }

        private static ObjectCache instance = null;

        private string cacheDirPath;
        private Dictionary<string, object> store = new Dictionary<string, object>();

        private ObjectCache()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "cache");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            cacheDirPath = path;
        }

        public void Clear()
        {
            if (!Directory.Exists(cacheDirPath))
                Directory.Delete(cacheDirPath);
        }

        public T GetObject<T>(string key)
        {
            if (store.ContainsKey(key))
                return (T)store[key];

            string path = Path.Combine(cacheDirPath, key + ".json");

            if (!File.Exists(path))
                return default(T);

            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (T)serializer.Deserialize(file, typeof(T));
            }
        }

        public void SetObject<T>(string key, T obj)
        {
            string path = Path.Combine(cacheDirPath, key + ".json");


            using (StreamWriter file = new StreamWriter(File.OpenWrite(path)))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, obj, typeof(T));
            }

            store[key] = obj;
        }

        public T GetOrSetObject<T>(string key, Func<T> objSetter)
        {
            var obj = GetObject<T>(key);
            if (obj != null && !obj.Equals(default(T)))
                return obj;

            obj = objSetter();
            SetObject<T>(key, obj);

            return obj;
        }
    }
}

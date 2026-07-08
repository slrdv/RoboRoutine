using Newtonsoft.Json;
using UnityEngine;

namespace RoboRoutine
{
    public sealed class PlayerPrefsStorage<T> : IStorage<T>
    {
        private readonly string _key;

        private T _data;
        private bool _loaded;

        public PlayerPrefsStorage(string key)
        {
            _key = key;
        }

        public void Save(T data)
        {
            string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            PlayerPrefs.SetString(_key, json);
            PlayerPrefs.Save();
            _data = data;
            _loaded = true;
        }

        public T Load()
        {
            if (_loaded) return _data;

            if (!PlayerPrefs.HasKey(_key))
            {
                _loaded = true;
                return default;
            }

            string json = PlayerPrefs.GetString(_key);
            _data = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            _loaded = true;

            return _data;
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(_key);
            _data = default;
            _loaded = false;
        }
    }
}

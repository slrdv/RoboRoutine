using Newtonsoft.Json;
using UnityEngine;

namespace RoboRoutine.Core
{
    public sealed class PlayerPrefsStorage<T> : IStorage<string, T>
    {
        private string _key;

        private T _data;
        private bool _loaded;

        public void Initialize(string key)
        {
            _key = key;
            _data = default;
            _loaded = false;
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

            _loaded = true;

            string json = PlayerPrefs.GetString(_key, null);

            if (json == null || !TryDeserialize(json, out _data))
            {
                _data = default;
                return _data;
            }

            return _data;
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(_key);
            _data = default;
            _loaded = false;
        }

        private bool TryDeserialize(string json, out T data)
        {
            try
            {
                data = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                return true;
            }
            catch (JsonException ex)
            {
                Debug.LogException(ex);
                data = default;

                return false;
            }
        }
    }
}

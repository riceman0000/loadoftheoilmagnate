using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using UniRx;
using UniRx.Async;


namespace OilMagnate
{
    public class SavableBase<T> : ISavable
     where T : SavableBase<T>, new()
    {
        /// <summary>
        /// json text.
        /// </summary>
        static string _jsonText;

        /// <summary>
        /// whether after loading.
        /// </summary>
        bool _isLoaded;

        /// <summary>
        /// Whether during save.
        /// </summary>
        bool _isEditing;

        public async void SaveAsync()
        {
            await UniTask.WaitUntil(() => _isEditing);
            if (_isLoaded && !_isEditing)
            {
                _isEditing = true;
                var path = GetSavePath();
                _jsonText = JsonUtility.ToJson(this);
                File.WriteAllText(path, _jsonText);
#if UNITY_IOS
                // iOSでデータをiCloudにバックアップさせない設定
                UnityEngine.iOS.Device.SetNoBackupFlag(path);
#endif
                _isEditing = false;

                Debug.Log($"saved data");
            }
        }

        public static T Load() // ref T target
        {
            var json = File.Exists(GetSavePath()) ? File.ReadAllText(GetSavePath()) : "";
            var instance = new T();
            if (string.IsNullOrEmpty(json))
            {
                instance._isLoaded = true;
            }
            else
            {
                instance = LoadFromJson(json);
            }

            if (instance != null)
            {
                instance.OnLoad();
            }
            return instance;
        }

        /// <summary>
        /// Load instance from json data.
        /// </summary>
        /// <param name="json">json file.</param>
        /// <returns>An instance of the specified type.</returns>
        static T LoadFromJson(string json)
        {
            var instance = new T();
            try
            {
                instance = JsonUtility.FromJson<T>(json);
                instance._isLoaded = true;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            Debug.Log($"loaded data");
            return instance;
        }

        /// <summary>
        /// Delete any saved data if it exists.
        /// </summary>
        public void Clear()
        {
            var savePath = GetSavePath();
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log($"cleared data");
            }
        }

        /// <summary>
        /// Reset save data.
        /// </summary>
        public void Reset()
        {
            _jsonText = JsonUtility.ToJson(new T());
            JsonUtility.FromJsonOverwrite(_jsonText, this);

            Debug.Log($"reset data");
        }

        /// <summary>
        /// Get the save path.
        /// </summary>
        /// <returns>save path.</returns>
        static string GetSavePath()
        {
            string filePath;
#if UNITY_EDITOR
            filePath = $"{GetSaveKey()}.json";
#else
            filePath =  $"{Application.persistentDataPath}/{GetSaveKey()}.json";
#endif
            return filePath;
        }

        /// <summary>
        /// セーブする名前をクラス名から取得する。
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// !現状だと1種類のクラスに対して一意のセーブしか作れない設計になっている。
        /// </remarks>
        static string GetSaveKey()
        {
            var provider = new SHA1CryptoServiceProvider();
            var hash = provider.ComputeHash(System.Text.Encoding.ASCII.GetBytes(typeof(T).FullName));
            return BitConverter.ToString(hash);
        }

        /// <summary>
        /// Called back when loaded save data.
        /// </summary>
        protected virtual void OnLoad() { }
    }
}
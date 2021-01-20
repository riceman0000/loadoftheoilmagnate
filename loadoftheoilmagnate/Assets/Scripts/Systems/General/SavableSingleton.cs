﻿using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;

namespace OilMagnate
{
    /// <summary>
    /// 前名「SavableSingletonBase」
    /// ローカルストレージにファイルとして、シリアライズしたデータを保存できるシングルトンです（iOSの場合、該当ファイルはiCloudバックアップ対象から除外します）
    /// </summary>
    abstract public class SavableSingleton<T> where T : SavableSingleton<T>, new()
    {
        /// <summary>m_instanceのプロパティ</summary>
        public static T Instance
        {
            get
            {
                if (m_instance != null)
                {
                    return m_instance;
                }
                var json = File.Exists(GetSavePath()) ? File.ReadAllText(GetSavePath()) : "";
                if (string.IsNullOrEmpty(json))
                {
                    m_instance = new T();
                    m_instance.m_isLoaded = true;
                }
                else
                {
                    LoadFromJSON(json);
                }
                Initialize();
                return m_instance;
            }
        }

        static void Initialize()
        {
            if (m_instance != null)
            {
                m_instance.OnInitialize();
            }
        }
        /// <summary>シングルトンインスタンス</summary>
        protected static T m_instance;
        /// <summary>ロードしたかどうかのフラグ</summary>
        bool m_isLoaded;
        /// <summary>セーブ中かどうかのフラグ</summary>
        protected bool m_isSaving;

        /// <summary>string of json</summary>
        static string jsonText;

        protected SavableSingleton() { }

        protected virtual void OnInitialize() { }

        /// <summary>
        /// serializableなクラスをJsonに書き出す
        /// </summary>
        public void Save()
        {
            if (m_isLoaded)
            {
                m_isSaving = true;
                var path = GetSavePath();
                jsonText = JsonUtility.ToJson(this);
                File.WriteAllText(path, jsonText);
#if UNITY_IOS
            // iOSでデータをiCloudにバックアップさせない設定
            UnityEngine.iOS.Device.SetNoBackupFlag(path);
#endif
                m_isSaving = false;
            }
        }


        /// <summary>
        /// Reset this instance.
        /// </summary>
        public void Reset()
        {
            jsonText = JsonUtility.ToJson(new T());
            JsonUtility.FromJsonOverwrite(jsonText, this);
            m_instance = null;
        }

        /// <summary>
        /// Clear this instance.
        /// </summary>
        public void Clear()
        {
            if (File.Exists(GetSavePath()))
            {
                File.Delete(GetSavePath());
            }
            m_instance = null;
        }

        /// <summary>
        /// Loads from json.
        /// </summary>
        /// <param name="json">Json.</param>
        public static void LoadFromJSON(string json)
        {
            try
            {
                m_instance = new T();
                m_instance = JsonUtility.FromJson<T>(json);
                m_instance.m_isLoaded = true;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        /// <summary>
        /// Gets the save path.
        /// </summary>
        /// <returns>The save path.</returns>
        static string GetSavePath()
        {
            //確認しやすい様にエディタではAssetsと同じ階層に保存し、それ以外ではApplication.persistentDataPath以下に保存する様にする
            string filePath;
#if UNITY_EDITOR
            filePath = GetSaveKey() + ".json";
#else
            filePath = Application.persistentDataPath + "/" + GetSaveKey() + ".json";
#endif
            return filePath;
        }

        /// <summary>
        /// Gets the save key.
        /// </summary>
        /// <returns>The save key.</returns>
        static string GetSaveKey()
        {
            var provider = new SHA1CryptoServiceProvider();
            var hash = provider.ComputeHash(System.Text.Encoding.ASCII.GetBytes(typeof(T).FullName));
            return BitConverter.ToString(hash);
        }
    }
}

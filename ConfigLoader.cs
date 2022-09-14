using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//loads game config values from external json file

namespace XStudios
{
    public class ConfigLoader : MonoBehaviour
    {
        Config _config;
        private static ConfigLoader _instance;
        public static ConfigLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<ConfigLoader>();
                }
                return _instance;
            }
        }

        public Config GetConfig()
        {
            return _config;
        }

        void Awake()
        {
            ReadConfigFile();
            Cursor.visible = false;
        }

        public void ReadConfigFile()
        {
            string filePath = Application.dataPath + "/StreamingAssets/config.json";
            StreamReader reader = new StreamReader(filePath);
            string jsonStr = reader.ReadToEnd();
            reader.Close();
            _config = JsonUtility.FromJson<Config>(jsonStr);
        }

        public void SaveConfigToFile()
        {
            string json = JsonUtility.ToJson(_config);
            System.IO.File.WriteAllText(Application.dataPath + "/StreamingAssets/config.json", json);
        }
    }

    [Serializable]
    public class Config
    {
        public float inactivity_time;
        public float game_time;
        public bool show_mouse;
    }
}

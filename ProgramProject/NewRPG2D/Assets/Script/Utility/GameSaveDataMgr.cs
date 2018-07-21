using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Utility
{
    public class GameSaveDataMgr : TSingleton<GameSaveDataMgr>
    {

        private string GetFilePath(string name)
        {
            string path = "";
#if UNITY_EDITOR
            path = string.Format("{0}/Game/", Application.persistentDataPath, name);

#elif UNITY_IPHONE
	   path = string.Format("{0}/Game/{1}", Application.persistentDataPath , name); 
 
#elif UNITY_ANDROID
	    path = string.Format("{0}/Game/{1}",  Application.persistentDataPath, name);
#else
        path = string.Format("{0}/Game/{1}",  Application.persistentDataPath, name);
#endif
            return path;
        }

        public void SaveGameData(string content, string fileName)
        {
            File.WriteAllText(fileName, content);
        }

        public string ReadGameData(string fileName)
        {
            string mPath = GetFilePath(fileName);
            string content = "";
            if (!File.Exists(mPath))
            {
                File.Create(mPath);
            }
            else
            {
                content = File.ReadAllText(mPath);
            }
            return content;
        }
    }
}

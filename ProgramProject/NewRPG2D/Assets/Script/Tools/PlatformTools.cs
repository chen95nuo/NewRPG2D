
//功能：
//创建者: 胡海辉
//创建时间：


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Tools
{
    public static class PlatformTools
    {
        public static string m_FileRealPath
        {
            get { return FileRealPathByPlatform(); }
        }

        public static Vector3 m_TouchPosition
        {
            get { return RealPosByPlatform(); }
        }

        /// <summary>
        /// 不同平台下获取不同的文件路径
        /// </summary>
        /// <returns></returns>
        private static string FileRealPathByPlatform()
        {
            string realPath = "";
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    realPath = Application.dataPath;
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    realPath = Application.streamingAssetsPath;
                    break;
                default:
                    realPath = Application.persistentDataPath;
                    break;

            }
            return realPath;
        }


        private static Vector3  RealPosByPlatform()
        {
            Vector3 pos = Vector3.zero;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                    pos = Input.touches[0].position;
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    pos = Input.mousePosition;
                    break;
                default:
                    pos = Input.mousePosition;
                    break;

            }
            return pos;
        }
       
    }
}

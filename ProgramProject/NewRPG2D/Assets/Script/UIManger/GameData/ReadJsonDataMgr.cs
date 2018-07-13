using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using UnityEngine;
using System.Collections;
using Assets.Script;

public enum BagJsonName
{
    BagEggData,
    BagEquipData,
    BagItemsData,
    BagMenuData,
    BagRoleData
}
public class ReadJsonDataMgr
{
    public static string GetJsonPath(BagJsonName name)
    {
        string path = "";
#if UNITY_EDITOR
        path = string.Format("{0}/Json/{1}.txt", Application.streamingAssetsPath, name);

#elif UNITY_IPHONE
	   path = string.Format("{0}/Raw/Json/{1}.txt", Application.dataPath, name); 
 
#elif UNITY_ANDROID
	    path = string.Format("{0}/Json/{1}.txt", Application.streamingAssetsPath, name);
#else
        path = string.Format("{0}/Json/{1}.txt", Application.streamingAssetsPath, name);
#endif
        return path;
    }

    IEnumerator ReadTxt(BagJsonName name)
    {
        string mPath = GetJsonPath(name);
        string content;
        WWW www = new WWW(mPath);
        yield return www;
        if (www.isDone)
        {
            content = www.text;
            UTF8Encoding encode = new System.Text.UTF8Encoding();
            byte[] binary = encode.GetBytes(content);
            MemoryStream fileMemoryStream = new MemoryStream(binary);

        }
        else
        {

        }
    }
}


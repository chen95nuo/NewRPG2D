using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine;

public class ReadJsonNewMgr : TSingleton<ReadJsonNewMgr>
{
    public Dictionary<int, string> AllJsonDataDic;
    public override void Init()
    {
        base.Init();
        int maxEnum = (int)JsonName.Max;
        AllJsonDataDic = new Dictionary<int, string>(maxEnum);
        for (int i = 0; i < maxEnum; i++)
        {
            GameMain.Instance.StartCoroutine(LoadJson((JsonName)i));
        }

    }



    IEnumerator LoadJson(JsonName name)
    {
        string mPath = ReadJsonDataMgr.GetJsonPath(name);
        string content = "";
        WWW www = new WWW(mPath);
        yield return www;
        if (www.isDone)
        {
            content = www.text;
        }
        string SkipBom = Encoding.UTF8.GetString(www.bytes, 3, www.bytes.Length - 3);
        AllJsonDataDic.Add((int)name, SkipBom);
    }
}

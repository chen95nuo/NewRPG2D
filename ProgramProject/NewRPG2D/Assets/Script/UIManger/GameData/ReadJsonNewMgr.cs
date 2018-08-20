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
        if (GameMain.Instance == null)
        {
            return;
        }
        LoadJsonByMono(GameMain.Instance);
    }

    public void LoadJsonByMono(MonoBehaviour mono)
    {
        int maxEnum = (int)JsonName.Max;
        AllJsonDataDic = new Dictionary<int, string>(maxEnum);
        for (int i = 0; i < maxEnum; i++)
        {
            mono.StartCoroutine(LoadJson((JsonName)i));
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using UnityEngine;
using System.Collections;
using Assets.Script;

public enum JsonName
{
    EggData,
    CardData,
    ItemsData,
    EquipData,
    StoreData,
    RoundData,
    PlayerData,
    ExploreData,
    HatcheryData,
    ExpeditionData,
    ComposedTableData,
    BagEggData,
    BagEquipData,
    BagItemsData,
    BagMenuData,
    BagRoleData,
    BagFurnaceData,
    PlayerRoundData,
    Max,
}
public class ReadJsonDataMgr
{
    public static string GetJsonPath(JsonName name)
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
}


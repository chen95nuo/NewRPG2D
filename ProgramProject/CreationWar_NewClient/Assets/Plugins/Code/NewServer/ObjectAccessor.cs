using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public	static	class	ObjectAccessor  
{
    public static Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();
    public static Dictionary<int, GameObject> aoiObject = new Dictionary<int, GameObject>();

    public static GameObject getPrefab(string prefabName)
    {
        GameObject gameobject = null;
        if (PrefabCache.ContainsKey(prefabName))
        {
            gameobject = PrefabCache[prefabName];
        }
        else
        {
            gameobject = (GameObject)Resources.Load(prefabName, typeof(GameObject));
            PrefabCache.Add(prefabName,gameobject);
        }
        return gameobject;
    }

    public static void addAOIObject(int instanceID, GameObject o)
    {
        /*if(	!aoiObject.ContainsKey(	instanceID	)	)
        {
            aoiObject.Add(instanceID, o);
            
        }*/

        aoiObject[instanceID] = o;
    }

    public static GameObject getAOIObject(int instanceID)
    {
        if(aoiObject.ContainsKey(instanceID))
        {
            GameObject o = aoiObject[instanceID];
            return o;
        }
        return null;
    }

	public static void removeAOIObject(int instanceID)
	{
		if(aoiObject.ContainsKey(instanceID))
		{
			aoiObject.Remove(instanceID);
		}
	}

	public static void clearAOIObject()
	{
		KDebug.WriteLog("=======================clearAOIObject======================");
		aoiObject.Clear();
	}

	/// <summary>
	/// 判断是不是单机副本 - 
	/// </summary>
	public	static	bool	IsSingleCarbon()
	{
		return	MonsterSpawnPointHandler.IsSingleCarbon()	||	Application.loadedLevelName == "Map911";
//		if(	aoiObject.Count == 0 )
//			return	true;
//		return	false;
	}

	/// <summary>
	/// 根据实例ID找到玩家或者技能宠物 -
	/// </summary>
	public	static	GameObject	GetObjectByInstanceID( int	_InstanceID )
	{
		GameObject	go	;
		go = ObjectAccessor.getAOIObject(_InstanceID);			
		if(	go == null)
		{
			go = MonsterHandler.GetInstance().FindMonsterByMonsterID(_InstanceID);			
		}
		return	go;
	}

}

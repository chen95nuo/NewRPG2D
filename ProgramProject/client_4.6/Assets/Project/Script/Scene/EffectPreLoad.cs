using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EffectPreLoad : MonoBehaviour
{
	bool waitForLoad = false;
	public LoadRes loadRes;
	
	// Use this for initialization
	void Start ()
	{
		waitForLoad = false;
	}
	
	public void startWaitForLoad()
	{
		waitForLoad = true;	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(waitForLoad)
		{
			if(loadRes.loadOver)
			{
				for(int i = 0 ; i < PreloadData.dataList.Count;++i)
				{
					string prefabName = PreloadData.dataList[i].path;
					GameObject loadObj = Resources.Load(prefabName) as GameObject;
					
					if(loadObj != null)
					{
						GameObject effectObj = GameObject.Instantiate(loadObj) as GameObject;
						GameObjectUtil.gameObjectAttachToParent(effectObj,gameObject);
						Destroy(effectObj,3f);
					}
				}
				waitForLoad = false;
			}
		}
		
	}
}

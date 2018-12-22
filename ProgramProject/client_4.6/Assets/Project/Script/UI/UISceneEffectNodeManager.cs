using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UISceneEffectNodeManager : MonoBehaviour {
	
	public static UISceneEffectNodeManager mInstance = null;
	
	public List<GameObject> cardInfoEffectPefabList = new List<GameObject>() ;
	
	// intensify effect
	public GameObject intensifySuccessFlyEffectNode;
	public GameObject intensifySuccessEndEffectNode;
	public GameObject intensifyEquipSuccessEffectNode;
	
	GameObject bigCardInfoIconEffectPrefab;
	GameObject smallCardInfoIconEffectPrefab;
	
	void Awake()
	{
		mInstance = this;
	}
	
	public void showChangeCardInfoPageEffect()
	{
		CancelInvoke("removeChangeCardInfoPageEffect");
		if(bigCardInfoIconEffectPrefab == null)
		{
			bigCardInfoIconEffectPrefab = Resources.Load("Prefabs/Effects/UIEffect/InterfaceFrameBgAtlas01_bight_ui01_big") as GameObject;
		}
		if(smallCardInfoIconEffectPrefab == null)
		{
			smallCardInfoIconEffectPrefab = Resources.Load("Prefabs/Effects/UIEffect/InterfaceFrameBgAtlas01_bight_ui01_small") as GameObject;
		}
		
		for(int i = 0 ; i < cardInfoEffectPefabList.Count;++i)
		{
			GameObjectUtil.destroyGameObjectAllChildrens(cardInfoEffectPefabList[i]);
			if(i == 0)
			{
				GameObject bigEObj = GameObject.Instantiate(bigCardInfoIconEffectPrefab) as GameObject;
				GameObjectUtil.setGameObjectLayer(bigEObj,STATE.LAYER_ID_UIEFFECT);
				GameObjectUtil.gameObjectAttachToParent(bigEObj,cardInfoEffectPefabList[i]);
			}
			else
			{
				GameObject smallEObj = GameObject.Instantiate(smallCardInfoIconEffectPrefab) as GameObject;
				GameObjectUtil.setGameObjectLayer(smallEObj,STATE.LAYER_ID_UIEFFECT);
				GameObjectUtil.gameObjectAttachToParent(smallEObj,cardInfoEffectPefabList[i]);
				
			}
		}
		Invoke("removeChangeCardInfoPageEffect",0.6f);
	}
	
	public void removeChangeCardInfoPageEffect()
	{
		for(int i = 0 ; i < cardInfoEffectPefabList.Count;++i)
		{
			GameObjectUtil.destroyGameObjectAllChildrens(cardInfoEffectPefabList[i]);
		}
	}
	
	public void gc()
	{
		bigCardInfoIconEffectPrefab = null;
		smallCardInfoIconEffectPrefab = null;
		Resources.UnloadUnusedAssets();
	}
}

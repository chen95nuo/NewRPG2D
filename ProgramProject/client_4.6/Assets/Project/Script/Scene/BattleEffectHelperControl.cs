using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleEffectHelperControl : MonoBehaviour 
{
	public static BattleEffectHelperControl mInstance ;
	
	public Player targetPlayer;
	public int reNum;
	
	public List<GameObject> effectNodeList;
	
	string reduecEnergyEffectPath = "Prefabs/Effects/jiannu";
	GameObject reduceEnergyEffectPrefab;
	
	string hitNumPath = "Prefabs/Item/HitNum";
	public GameObject hitNumPrefab;
	
	Camera mainCamera;
	Camera NGUICamera;
	
	string sheepEffectPath = "Prefabs/Effects/bianyang";
	GameObject sheepEffectPrefab;
	
	void Awake()
	{
		mInstance = this;
		
	}
	
	public void showEffect(Player p,int num)
	{
		targetPlayer = p;
		reNum = num;
		GameObject effectNode = getEffectNode();
		if(effectNode == null)
			return;
		GameObject effectObj = null;
		if(reduceEnergyEffectPrefab != null)
		{
			effectObj = GameObject.Instantiate(reduceEnergyEffectPrefab) as GameObject;
		}
		else
		{
			reduceEnergyEffectPrefab = Resources.Load(reduecEnergyEffectPath) as GameObject;
			if(reduceEnergyEffectPrefab == null)
			{
				Debug.Log("reduecEnergyEffect not exist !");
			}
			effectObj = GameObject.Instantiate(reduceEnergyEffectPrefab) as GameObject;
		}
		GameObjectUtil.gameObjectAttachToParent(effectObj,effectNode);
		Destroy(effectObj,1.6f);
		Invoke("showReduceNumEffect",1.0f);
	}
	
	void showReduceNumEffect()
	{
		GameObject effectNode = getEffectNode();
		if(effectNode == null)
			return;
		GameObject hitNumObj = null;
		if(hitNumPrefab != null)
		{
			hitNumObj = GameObject.Instantiate(hitNumPrefab) as GameObject;
		}
		else
		{
			hitNumPrefab = Resources.Load(hitNumPath) as GameObject;
			if(hitNumPrefab == null)
			{
				Debug.Log(" hitNumPrefab not exist !");
				return;
			}
			hitNumObj = GameObject.Instantiate(hitNumPrefab) as GameObject;
		}
		hitNumObj.transform.parent = PVESceneControl.mInstance.hitNumParent.transform;
		hitNumObj.transform.localScale = Vector3.one;
		if(mainCamera == null)
		{
			mainCamera = Camera.main;
		}
		if(NGUICamera == null)
		{
			NGUICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		}
		Vector3 worldPos = new Vector3(effectNode.transform.position.x,effectNode.transform.position.y+3,effectNode.transform.position.z);
		Vector2 screenPos = mainCamera.WorldToScreenPoint(worldPos);
		Vector3 curPos = NGUICamera.ScreenToWorldPoint(screenPos);
		hitNumObj.transform.position = curPos;
		HitNumManager numManager = hitNumObj.GetComponent<HitNumManager>();
		numManager.reduceEnergyPos = worldPos;
		numManager.mainCamera = mainCamera;
		numManager.NGUICamera = NGUICamera;
		numManager.createReduceEnergyNum("-" + reNum.ToString());
		
		targetPlayer.removeEnergy(reNum);
		if(targetPlayer.getTeam() == 0)
		{
			PVESceneControl.mInstance.energyManager.energyChange();
		}
	}
	
	public GameObject getEffectNode()
	{
		if(targetPlayer == null)
			return null;
		int team = targetPlayer.getTeam();
		GameObject effetNode = effectNodeList[team];
		return effetNode;
	}
	
	public void createChangeSheepEffect(Vector3 pos)
	{
		GameObject effectObj = null;
		if(effectObj != null)
		{
			effectObj = GameObject.Instantiate(sheepEffectPrefab) as GameObject;
		}
		else
		{
			sheepEffectPrefab = Resources.Load(sheepEffectPath) as GameObject;
			if(sheepEffectPrefab == null)
			{
				Debug.Log(" sheepEffectPrefab not exist !");
				return;
			}
			effectObj = GameObject.Instantiate(sheepEffectPrefab) as GameObject;
		}
		effectObj.transform.position = pos;
		Destroy(effectObj,1.1f);
	}
	
	
	public void gc()
	{
		if(reduceEnergyEffectPrefab != null)
		{
			reduceEnergyEffectPrefab = null;
		}
		
		if(effectNodeList != null)
		{
			effectNodeList.Clear();
		}
		
		if(hitNumPrefab != null)
		{
			hitNumPrefab = null;
		}
		
		if(mainCamera != null)
		{
			mainCamera = null;
		}
		
		if(NGUICamera != null)
		{
			NGUICamera = null;
		}
		
		if(sheepEffectPrefab != null)
		{
			sheepEffectPrefab = null;
		}
		
		GameObject.Destroy(gameObject);		
		mInstance = null;
		
	}
}

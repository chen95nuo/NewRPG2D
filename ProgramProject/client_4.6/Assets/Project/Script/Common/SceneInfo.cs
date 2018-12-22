using System;
using UnityEngine;

public class SceneInfo
{
	
	private static SceneInfo instance;
	
	private GameObject[] positions;
	private GameObject center_0;
	private GameObject center_1;
	private GameObject screen_down;
	private GameObject pos2_head;
	private GameObject myCenter;
	private GameObject enemyCenter;
	
	GameObject skyCallUpStartPosObj;
	GameObject skyCallUpEndPosObj;
	GameObject skyEffectPosObj;
	
	GameObject enemySkyCallUpStartPosObj;
	GameObject enemySkyCallUpEndPosObj;
	GameObject enemySkyEffectPosObj;
	
	public static SceneInfo getInstance()
	{
		if(instance==null)
		{
			instance=new SceneInfo();
		}
		return instance;
	}
	
	private SceneInfo()
	{
		loadPositions();
	}
	
	/**场景进行过一次后再次进入场景,此对象下的所有gameobject均已被销毁,需要重新载入**/
	public void load()
	{
		instance=new SceneInfo();
	}
	
	/// <summary>
	/// 加载12个位置
	/// </summary>
	private void loadPositions()
	{
		GameObject posObj = GameObject.Find("position");
		if(posObj == null)
			return;
		
		positions=new GameObject[12];
		string[] posName={"pos1","pos2","pos3","pos4","pos5","pos6","pos7","pos8","pos9","pos10","pos11","pos12"};
		for(int i=0;i<12;i++)
		{
			positions[i] = GameObjectUtil.findGameObjectByName(posObj,posName[i]);
		}
		center_0= GameObjectUtil.findGameObjectByName(posObj,"center_0");
		center_1= GameObjectUtil.findGameObjectByName(posObj,"center_1");
		screen_down = GameObjectUtil.findGameObjectByName(posObj,"screen_down");
		pos2_head = GameObjectUtil.findGameObjectByName(posObj,"pos2_head");
		myCenter = GameObjectUtil.findGameObjectByName(posObj,"myCenter");
		enemyCenter = GameObjectUtil.findGameObjectByName(posObj,"EnemyCenter");
		
		GameObject unitSkillPosObj = GameObjectUtil.findGameObjectByName(posObj,"UnitSkillPos");
		if(unitSkillPosObj == null)
			return;
		skyCallUpStartPosObj = GameObjectUtil.findGameObjectByName(unitSkillPosObj,"SkyCallUpStartPos");
		skyCallUpEndPosObj = GameObjectUtil.findGameObjectByName(unitSkillPosObj,"SkyCallUpEndPos");
		skyEffectPosObj = GameObjectUtil.findGameObjectByName(unitSkillPosObj,"SkyEffectPos");
		
		enemySkyCallUpStartPosObj = GameObjectUtil.findGameObjectByName(unitSkillPosObj,"EnemySkyCallUpStartPos");
		enemySkyCallUpEndPosObj = GameObjectUtil.findGameObjectByName(unitSkillPosObj,"EnemySkyCallUpEndPos");
		enemySkyEffectPosObj = GameObjectUtil.findGameObjectByName(unitSkillPosObj,"EnemySkyEffectPos");
		
	}
	
	/// <summary>
	/// 获取指定位置
	/// </summary>
	/// <returns>
	/// The position.
	/// </returns>
	/// <param name='index'>
	/// Index.
	/// </param>
	public GameObject getPosition(int index)
	{
		return positions[index];
	}
	
	public GameObject getCenter0()
	{
		return center_0;
	}
	public GameObject getCenter1()
	{
		return center_1;
	}
	
	public GameObject getScreenDown()
	{
		return screen_down;
	}
	
	public GameObject getPos2_head()
	{
		return pos2_head;
	}
	
	public GameObject getMyCenter()
	{
		return myCenter;
	}
	
	public GameObject getEnemyCenter()
	{
		return enemyCenter;
	}
	
	public GameObject getSkyCallUpStartPosObj()
	{
		return skyCallUpStartPosObj;
	}
	
	public GameObject getSkyCallUpEndPosObj()
	{
		return skyCallUpEndPosObj;
	}
	
	public GameObject getSkyEffectPosObj()
	{
		return skyEffectPosObj;
	}
	
	public GameObject getEnemySkyCallUpStartPosObj()
	{
		return enemySkyCallUpStartPosObj;
	}
	
	public GameObject getEnemySkyCallUpEndPosObj()
	{
		return enemySkyCallUpEndPosObj;
	}
	
	public GameObject getEnemySkyEffectPosObj()
	{
		return enemySkyEffectPosObj;
	}
	
	
}


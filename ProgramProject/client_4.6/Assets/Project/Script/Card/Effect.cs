using UnityEngine;
using System.Collections;

/// <summary>
/// 特效
/// </summary>
public class Effect
{
	
	public GameObject effectObj;
	/**特效保持的时间**/
	private float keepTime;
	private float startTime;
	
	/**延时产生的特效**/
	private string effectName;
	private float delayTime;
	private Vector3 srcPos;
	private Vector3 tarPos;
	private Quaternion rotation;
	

	int layer = 0;
	GameObject parent;
	int type ;
	public float getKeepTime()
	{
		return keepTime;
	}
	
	/**延时产生的特效**/
	public Effect(string effectName,float keepTime,float delayTime,Vector3 srcPos,Vector3 tarPos,Quaternion rotation,
		float startTime,int layer = 0, GameObject parent = null, int type = 1)
	{
		this.effectName=effectName;
		this.keepTime=keepTime;
		this.delayTime=delayTime;
		this.srcPos=srcPos;
		this.tarPos=tarPos;
		this.rotation=rotation;
		this.startTime=startTime;
		this.layer = layer;
		this.parent = parent;
		this.type = type;
		if(delayTime <= 0.0f)
		{
			create();
		}
	}
	

	public void create()
	{
		GameObject loadEffect = null;
		if(PVESceneControl.mInstance != null)
		{
			loadEffect = (GameObject)PVESceneControl.mInstance.loadEffects[effectName];
		}
		if(loadEffect==null)
		{
			loadEffect=GameObjectUtil.LoadResourcesPrefabs(effectName,type);
			if(loadEffect == null)
			{
				Debug.Log("error  effect : " + effectName);
			}
			if(PVESceneControl.mInstance != null)
			{
				PVESceneControl.mInstance.loadEffects.Add(effectName,loadEffect);
			}
		}
		effectObj=GameObject.Instantiate(loadEffect) as GameObject;
		effectObj.transform.position=srcPos;
		effectObj.transform.rotation=rotation;
		effectObj.transform.localScale = Vector3.one;
		if(parent != null)
		{
			effectObj.transform.parent = parent.transform;
		}
		GameObjectUtil.setGameObjectLayer(effectObj,layer);
		if(isNeedMove())
		{
			iTween.MoveTo(effectObj,iTween.Hash("position",tarPos,"time",keepTime, "easetype", iTween.EaseType.linear));
		}
	}
	
	public GameObject getEffectObj()
	{
		return effectObj;
	}
	
	public bool isValid(float curTime)
	{
		if(keepTime > 0)
		{
			return curTime-startTime<=keepTime+delayTime;
		}
		else
		{
			return true;
		}
	}
	
	public bool isDelayOver(float curTime)
	{
		return curTime-startTime>=delayTime;
	}
	
	public bool isDelayEffect()
	{
		return delayTime>0;
	}
	
	public Vector3 getTarPos()
	{
		return tarPos;
	}
	
	public bool isNeedMove()
	{
		if(srcPos != tarPos)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public void gc()
	{
		effectObj=null;
		parent=null;
		Resources.UnloadUnusedAssets();
	}
	
}

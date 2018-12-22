using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreakPopWnd : MonoBehaviour
{
	public UILabel textValue;
	public List<GameObject> breakObjList;
	
	int startBreakNum;
	int endBreakNum;
	int curBreakNum;
	GameObject effectPrefab = null;
	
	bool finishShow = false;
	float showEffectTime = 0.4f;
	float curShowEffectTime = 0;
	string cardName = string.Empty;
	
	public bool finishWait = false;
	float waitTime = 0.5f;
	float curWaitTime = 0;
	
	public List<GameObject> effectObjList = new List<GameObject>();
	
	void Awake()
	{
		string path = "Prefabs/Effects/UIEffect/tupo01";
		effectPrefab = Resources.Load(path) as GameObject;
		clear();
		hide();
	}
	
	void Update()
	{
		if(finishWait)
		{
			if(!finishShow)
			{
				if(curBreakNum > endBreakNum)
				{
					finishShow = true;
					clearBreakEffect();
				}
				else
				{
					if(curShowEffectTime >showEffectTime)
					{
						showValue (curBreakNum);
						curBreakNum++;
						showEffect(curBreakNum);
						curShowEffectTime = 0;
					}
					else
					{
						curShowEffectTime += Time.deltaTime;
					}
				}
			}
		}
		else
		{
			if(curWaitTime > waitTime)
			{
				finishWait = true;
				showEffect(curBreakNum);
			}
			else
			{
				curWaitTime += Time.deltaTime;
			}
		}
		
	}
	
	public void show(int startNum,int endNum,string name)
	{
		if(startNum == endNum)
			return;
		clear();
		finishShow = false;
		finishWait = false;
		startBreakNum = startNum;
		endBreakNum = endNum;
		curBreakNum = startBreakNum+1;
		cardName = name;
		showValue(startBreakNum);
		gameObject.SetActive(true);
	}
	
	public void hide()
	{
		clear();
		gameObject.SetActive(false);
		
	}
	
	public void closeWnd()
	{
		if(!finishShow)
		{
			return;
		}
		if(endBreakNum == 5)
		{
			ToastWindow.mInstance.showText(TextsData.getData(151).chinese.Replace("name",cardName));
		}
		hide ();
	}
	
	public void showEffect(int breakNum)
	{
		if(breakNum > 5 || breakNum <1)
			return;
		GameObject obj = breakObjList[breakNum-1];
		GameObject effectObj = GameObject.Instantiate(effectPrefab) as GameObject;
		GameObjectUtil.gameObjectAttachToParent(effectObj,gameObject);
		effectObj.transform.localPosition = new Vector3(obj.transform.localPosition.x,obj.transform.localPosition.y,-50);
		
		GameObjectUtil.setGameObjectLayer(effectObj,STATE.LAYER_ID_NGUI);
		effectObjList.Add (effectObj);
	}
	
	public void showValue(int breakNum)
	{
		if(breakNum > 5 || breakNum <1)
			return;
		int num = breakNum * 10;
		textValue.text = "[ff0000]" + num.ToString() + "%[-]";
		for(int i = 0 ;i < breakNum; ++i)
		{
			breakObjList[i].SetActive(true);
		}
		
	}
	
	public void clearBreakEffect()
	{
		for(int i = 0 ;i < effectObjList.Count;++i)
		{
			GameObject.Destroy(effectObjList[i]);
		}
		effectObjList.Clear();
	}
	
	public void clear()
	{
		for(int i = 0; i < breakObjList.Count;++i)
		{
			breakObjList[i].SetActive(false);
		}
		textValue.text = string.Empty;
		startBreakNum = -1;
		endBreakNum = -1;
		curBreakNum = -1;
		cardName = string.Empty;
		curShowEffectTime = 0;
		curWaitTime = 0;
		
		
	}
}


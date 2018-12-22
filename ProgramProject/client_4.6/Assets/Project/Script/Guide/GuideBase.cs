using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideBase : BWUIPanel
{
	public List<GameObject> stepObjList = new List<GameObject>();
	//public List<GameObject> cardPosList = new List<GameObject>();
	public int runningStep = -1;	
	
	// 防止重复点击发请求,存下来每步骤是否点击过的信息列表 //
	public List<bool> btnClickList = new List<bool>();
	
	public override void show()
	{
		base.show();
		hideAllStep();
	}
	
	public override void hide()
	{
		base.hide();
		hideAllStep();
		//GameHelperCard.mInstance.hideCard();
	}
	
	// 隐藏当前指引所有补数页面 // 
	public virtual void hideAllStep()
	{
		for(int i = 0; i < stepObjList.Count;++i)
		{
			stepObjList[i].SetActive(false);
		}
		//GameHelperCard.mInstance.hideCard();
	}
	
	// 显示当前指引的当前步数界面 //
	public virtual void showStep(int step)
	{
		runningStep = step;
		show();
		stepObjList[step].SetActive(true);
		btnClickList[step] = false;
	}
	
	public void setView3DCardBlackFlag(bool b)
	{
	}
	
	public void setClickBtnCount(int count)
	{
		for(int i = 0 ; i < count; ++i)
		{
			btnClickList.Add(false);
		}
	}
}



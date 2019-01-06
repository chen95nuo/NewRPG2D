using UnityEngine;
using System.Collections.Generic;

public class UIElemTowerSceneDoorPnl3D : MonoBehaviour
{
	public SpriteText doorIdxLabel;
	public UIBox startPnl;
	public GameObject notClear, todayNotClear, todayClear;

	int layer = 0;

	//Start在SetLayer后执行。。。
	//void Start()
	//{
	//    startPnl.Hide(true);
	//}

	public void ResetPassStatus()
	{
		int maxPassLayer = SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.MaxPassLayer;
		int currentLayer = SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentLayer;

		//历史未通关
		if (layer > maxPassLayer)
		{
			notClear.SetActive(true);
			todayNotClear.SetActive(false);
			todayClear.SetActive(false);
		}
		//历史已通关，今日未通关
		else if (layer > currentLayer)
		{
			notClear.SetActive(false);
			todayNotClear.SetActive(true);
			todayClear.SetActive(false);
		}
		//今日已通关
		else
		{
			notClear.SetActive(false);
			todayNotClear.SetActive(false);
			todayClear.SetActive(true);
		}

		if (layer == 0)
		{
			doorIdxLabel.Hide(true);
			startPnl.Hide(false);
		}
		else
		{
			doorIdxLabel.Hide(false);
			startPnl.Hide(true);
		}
	}

	public void SetLayer(int layer)
	{
		this.layer = layer;
		doorIdxLabel.Text = GameUtility.FormatUIString("MelaleucaFloorDoorUIIndex", layer);

		ResetPassStatus();
	}
}

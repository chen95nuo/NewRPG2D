using UnityEngine;
using System;
using System.Collections.Generic;
using KodGames;
using ClientServerCommon;

public class UIDlgTowerExplain : UIModule
{
	public UIScrollList explainList;
	public SpriteText explainLabel;
	public List<AutoSpriteControlBase> BtnRoots;

	private List<MelaleucaFloorConfig.Description> descriptions; 
	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		initBtnData();

		return true;
	}

	public void initBtnData()
	{
		descriptions = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.Descriptions;

		for(int index = 0; index < BtnRoots.Count && index < descriptions.Count; index ++)
		{
			SetSubButton(BtnRoots[index],descriptions[index].DescId);
		}

		SetBtnRoots((int)BtnRoots[0].Data);
	}

	private void SetSubButton(AutoSpriteControlBase button, int data)
	{
		button.Data = data;
	}

	public void SetBtnRoots(int data)
	{
		for (int index = 0; index < BtnRoots.Count; index++ )
		{
			if (index < descriptions.Count)
			{
				BtnRoots[index].controlIsEnabled = !((int)BtnRoots[index].Data == data);
				if (!BtnRoots[index].controlIsEnabled)
					explainLabel.Text = descriptions[index].Content;
			}
			else BtnRoots[index].Hide(true);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSubBtn(UIButton btn)
	{
		int id = (int)btn.Data;		
		SetBtnRoots(id);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackBtn(UIButton btn)
	{
		HideSelf();
	}
}

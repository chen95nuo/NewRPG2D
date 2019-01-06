using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemUnActOrganBeast : MonoBehaviour
{
	public UIElemAssetIcon iconBtn;
	public UIBox beastType;
	public UIButton combineBtn;

	public UIProgressBar fragmentPro;
	public SpriteText beastNameLabel;

	private BeastConfig.BaseInfo beastInfo;
	public BeastConfig.BaseInfo BeastInfo { get { return beastInfo; } }

	public void SetData(BeastConfig.BaseInfo beastInfo)
	{
		this.beastInfo = beastInfo;
		combineBtn.Data = beastInfo.Id;

		int framentCount = 0;
		
		var frament = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(beastInfo.FragmentId);
		if(frament != null)
		{
			framentCount = frament.Amount;
		}

		float prgValue = (float)framentCount / (float)ConfigDatabase.DefaultCfg.BeastConfig.GetBeastBreakthoughtByBreakthoughtNow(beastInfo.MinActiveBreakthought).ActiveCostFragmentCount;
		
		if (prgValue < 1.0f)
		{
			fragmentPro.Value = prgValue;
			combineBtn.Hide(true);
		}
		else
		{
			fragmentPro.Value = 1.0f;
			combineBtn.Hide(false);
		}		
		
		fragmentPro.Text = GameUtility.FormatUIString("UIPnlOrgansBeastTab_CountWithMax", (float)framentCount, ConfigDatabase.DefaultCfg.BeastConfig.GetBeastBreakthoughtByBreakthoughtNow(beastInfo.MinActiveBreakthought).ActiveCostFragmentCount);
		UIElemTemplate.Inst.SetBeastTraitIcon(beastType, beastInfo.BeastType);
		beastType.Hide(false);
		beastNameLabel.Text = beastInfo.BeastName;
		
		//未激活机关兽显示
		iconBtn.SetData(beastInfo.Id, 1, 1, false);
		iconBtn.border.Data = beastInfo.Id;
	}
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlTowerWeekReward : UIModule
{
	public UIScrollList rankScrollList;
	public GameObjectPool rankPool;
	public SpriteText myRankLabel;
	public SpriteText titleLabel;
	
	public UIButton rewardBtn; 
	public UIButton notRewardBtn;

	private int weekRank = 0;
	private bool isGetReward = false;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		
		RequestMgr.Inst.Request(new MelaleucaFloorWeekRewardInfoReq());			

		SysUIEnv.Instance.GetUIModule<UIPnlTowerPoint>().SetSelectedBtn(_UIType.UIPnlTowerWeekReward);

		titleLabel.Text = GameUtility.GetUIString("UIPnlTowerLastWeekReward_Title_Label");

		ClearData();
		StartCoroutine("FillList");

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	public void RequesetRewardSuccess(int weekRank, bool isGetReward)
	{
		this.weekRank = weekRank;
		this.isGetReward = isGetReward;

		Init();
	}

	public void Init()
	{
		myRankLabel.Text = weekRank.ToString();

		//领取按钮初始化
		if (isGetReward)
		{
			rewardBtn.gameObject.SetActive(true);
			notRewardBtn.gameObject.SetActive(false);
		}
		else
		{
			rewardBtn.gameObject.SetActive(false);
			notRewardBtn.gameObject.SetActive(true);

			if (weekRank > 0)
			{
				notRewardBtn.spriteText.Text = GameUtility.GetUIString("UIPnlTowerLastWeekReward_NotReward_Btn_Label");
				notRewardBtn.Data = false;
			}
			else
			{
				myRankLabel.Text = GameUtility.GetUIString("UIPnlTowerRank_Label_WeekReward_NO");
				notRewardBtn.spriteText.Text = GameUtility.GetUIString("UIPnlTowerLastWeekReward_GetReward_Btn_Label");
				notRewardBtn.Data = true;
			}
		}
	}

	public void GetRewardSuccess(KodGames.ClientClass.Reward weekReward)
	{
		rewardBtn.gameObject.SetActive(false);
		
		//领取奖励后按钮状态改变
		notRewardBtn.gameObject.SetActive(true);
		isGetReward = false;

		notRewardBtn.spriteText.Text = GameUtility.GetUIString("UIPnlTowerLastWeekReward_NotReward_Btn_Label");
		notRewardBtn.Data = false;

		string mesReward = string.Empty;
		for (int index = 0; index < weekReward.Consumable.Count; index++)
		{
			mesReward += string.Format(GameUtility.GetUIString("UIPnlTowerLastWeekReward_GetRewardMessage_Label"),
				ItemInfoUtility.GetAssetName(weekReward.Consumable[index].Id), weekReward.Consumable[index].Amount);
		}

		string messageStr = string.Format(GameUtility.GetUIString("UIPnlTowerLastWeekReward_GetRewardTitle_Label")
			, mesReward);

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), messageStr);
	}

	private void ClearData()
	{
		StopCoroutine("FillList");

		for (int index = 0; index < rankScrollList.Count; index++)
			(rankScrollList.GetItem(index).Data as UIElemLastWeekReward).ReleaseRewardItem();	

		rankScrollList.ClearList(false);
		rankScrollList.ScrollPosition = 0f;
	}

	//排名奖励列表
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;
		List<MelaleucaFloorConfig.WeekRewardShow> weekReward = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.WeekRewardShows;

		for (int i = 0; i < weekReward.Count; i++ )
		{
			UIListItemContainer uiContainer = rankPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemLastWeekReward item = uiContainer.GetComponent<UIElemLastWeekReward>();
			uiContainer.data = item;

			item.SetData(weekReward[i]);
			rankScrollList.AddItem(item.gameObject);
		}
	}

	// RewardGet Show
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRewardBtnClick(UIButton btn)
	{
		// RewardGet Show
		RequestMgr.Inst.Request(new MelaleucaFloorGetRewardReq());
	}

	// Click Rreward Icon
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardBtn(UIButton btn)
	{
		// Click Rreward Icon
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgConsumableInfo, assetIcon.AssetId);
	}

	// Click Rreward Button Tips
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickNotRewardBtn(UIButton btn)
	{
		string clickTips  =string.Empty;

		if(!(bool)btn.Data)
			clickTips = GameUtility.GetUIString("UIPnlTowerLastWeekReward_AlreadyGet_Tips_Label");
		else
			clickTips = GameUtility.GetUIString("UIPnlTowerLastWeekReward_NotReward_Tips_Label");
		
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), clickTips);
	}
}
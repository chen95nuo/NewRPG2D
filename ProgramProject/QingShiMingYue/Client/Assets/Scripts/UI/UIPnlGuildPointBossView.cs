using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildPointBossView : UIModule
{
	//标题显示
	public SpriteText leftCountLabel;
	public SpriteText fightBtnLabel;

	private KodGames.ClientClass.StageInfo stageInfo;

	public List<UIElemGuildPointBattleViewItem> avatars;
	public GameObject rewardRoot;

	public UIScrollList rewardList;
	public GameObjectPool rewardPool;
	public GameObjectPool titlePool;
	public UIButton damageBtn;
	public UIButton fightBtn;
	public UIButton noFightBtn;

	private int mapNum;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas[0] is KodGames.ClientClass.StageInfo)
			stageInfo = userDatas[0] as KodGames.ClientClass.StageInfo;

		mapNum = (int)userDatas[1];

		Init();
			
		return true;
	}

	private void Init()
	{
		//Clean Avatars.
		InitAvatars();

		damageBtn.gameObject.SetActive(BossHasBattle());

		if (stageInfo.Status == GuildStageConfig._StageStatus.Complete)
		{
			noFightBtn.Hide(false);
			fightBtn.Hide(true);
			damageBtn.Hide(true);
			rewardRoot.SetActive(true);
			leftCountLabel.Text = "";
		}
		else
		{
			if (SysUIEnv.Instance.GetUIModule<UIPnlGuildPointMain>().FreeChallengeCount > 0)
			{
				leftCountLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointBossView_LeftCount", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, SysUIEnv.Instance.GetUIModule<UIPnlGuildPointMain>().FreeChallengeCount);
				fightBtnLabel.Text = GameUtility.GetUIString("UIGuildPointBossView_FreeBattle");
			}				
			else
			{
				var cost = SysUIEnv.Instance.GetUIModule<UIPnlGuildPointMain>().Costs;
				leftCountLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointBossView_ItemCount", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(cost.Id), GameDefines.textColorWhite, SysUIEnv.Instance.GetUIModule<UIPnlGuildPointMain>().ItemChallengeCount);
				fightBtnLabel.Text = GameUtility.FormatUIString("UIGuildPointBossView_CostBattle", cost.Count, ItemInfoUtility.GetAssetName(cost.Id));
				if (SysUIEnv.Instance.GetUIModule<UIPnlGuildPointMain>().ItemChallengeCount > 0)
					fightBtn.controlIsEnabled = true;
				else
				{
					fightBtnLabel.Text = GameUtility.GetUIString("UIGuildPointBossView_FreeBattle");
					fightBtn.controlIsEnabled = false;
				} 
			}
								
			noFightBtn.Hide(true);
			fightBtn.Hide(false);
			rewardRoot.SetActive(false);
		}		

		//Fill Avatars.
		var positionData = stageInfo.Player.PositionData;

		if (positionData != null)
		{
			for (int index = 0; index < avatars.Count; index++)
			{
				for (int j = 0; j < positionData.Positions[0].AvatarLocations.Count; j++)
				{
					if (positionData.Positions[0].AvatarLocations[j].LocationId == avatars[index].Position)
					{
						avatars[index].SetData(stageInfo.Player.SearchAvatar(positionData.Positions[0].AvatarLocations[j].Guid),
												positionData.Positions[0].AvatarLocations[j].LocationId == positionData.Positions[0].EmployLocationId,
												GetHpByLocationId(positionData.Positions[0].AvatarLocations[j].LocationId));
						break;
					}
				}
			}
		}

		ClearData();
		StartCoroutine("FillRewardList");
	}

	//获取角色血量
	public double GetHpByLocationId(int location)
	{
		for (int i = 0; i < stageInfo.AvatarHps.Count; i++)
			if (stageInfo.AvatarHps[i].locationId == location)
				return stageInfo.AvatarHps[i].leftHP;

		return 0;
	}

	//获取角色血量
	public bool BossHasBattle()
	{
		for (int i = 0; i < stageInfo.AvatarHps.Count; i++)
		{
			if (stageInfo.AvatarHps[i].leftHP < 1)
				return true;
		}
		return false;
	}

	private void ClearData()
	{
		StopCoroutine("FillRewardList");
		rewardList.ClearList(false);
		rewardList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList()
	{
		yield return null;

		if (stageInfo.ExtraShowRewards.Count > 0)
		{
			UIElemGuildPointRewardShowTitleItem item = titlePool.AllocateItem().GetComponent<UIElemGuildPointRewardShowTitleItem>();
			item.SetData(false);

			rewardList.AddItem(item.gameObject);
		}

		foreach (var reward in stageInfo.ExtraShowRewards)
		{
			UIElemDanExtraReward item = rewardPool.AllocateItem().GetComponent<UIElemDanExtraReward>();
			item.SetData(reward);

			rewardList.AddItem(item.gameObject);
		}

		if (stageInfo.ShowRewards.Count > 0)
		{
			UIElemGuildPointRewardShowTitleItem item = titlePool.AllocateItem().GetComponent<UIElemGuildPointRewardShowTitleItem>();
			item.SetData(true);

			rewardList.AddItem(item.gameObject);
		}

		foreach (var reward in stageInfo.ShowRewards)
		{
			UIElemDanExtraReward item = rewardPool.AllocateItem().GetComponent<UIElemDanExtraReward>();
			item.SetData(reward);

			rewardList.AddItem(item.gameObject);
		}

		rewardList.ScrollToItem(0, 0);		
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void InitAvatars()
	{
		for (int index = 0; index < avatars.Count; index++)
			avatars[index].InitData(PlayerDataUtility.GetBattlePosByIndexPos(index));
	}

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	//点击战斗
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickFight(UIButton btn)
	{
		if(SysUIEnv.Instance.GetUIModule<UIPnlGuildPointMain>().FreeChallengeCount > 0)
			RequestMgr.Inst.Request(new GuildStageCombatBossReq(stageInfo.Index, GuildStageConfig._ChallengeType.Free));
		else if(SysUIEnv.Instance.GetUIModule<UIPnlGuildPointMain>().ItemChallengeCount > 0)
			RequestMgr.Inst.Request(new GuildStageCombatBossReq(stageInfo.Index, GuildStageConfig._ChallengeType.Item));
	}

	//点击
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickParticular(UIButton btn)
	{		
		SysUIEnv.Instance.ShowUIModule<UIPnlGuildPointDamageRank>(new int[] { mapNum, stageInfo.Index }, false);
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var showItem = assetIcon.Data as com.kodgames.corgi.protocol.ShowReward;
		GameUtility.ShowAssetInfoUI(showItem, _UILayer.Top);
	}
}

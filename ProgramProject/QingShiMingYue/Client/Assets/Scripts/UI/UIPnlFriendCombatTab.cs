using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlFriendCombatTab : UIModule
{
	//不予修改的公共结构
	public SpriteText passLevel;
	public SpriteText passLevelLast;
	public SpriteText resetCountLabel;
	public SpriteText dieCountLabel;

	//修要修改的公共结构
	public UIBox backBgBox;
	public UIChildLayoutControl childLayout;

	//未通关的时候渲染的东西
	public List<UIElemFriendCombatTabItem> avatars;
	public SpriteText enemyStageLevel;
	public SpriteText enemyPlayerName;
	public UIButton levelRewardButton;
	public UIBox enemyTextBgBox;

	//通关的时候渲染的东西
	public UIBox passActionBgBox;
	public SpriteText passLabel;
	public UIBox passAction01;
	public UIBox passAction02;
	public UIBox passText;

	private int PassBgBoxHeight = 200;
	private int NotPassBgBoxHeight = 248;

	public Transform passButtonT;
	public Transform notPassButtonT;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		InitAvatars();

		return true;
	}

	private UIPnlFriendInfoTab.CombatData Data
	{
		get
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendInfoTab)))
				return SysUIEnv.Instance.GetUIModule<UIPnlFriendInfoTab>().GetInfoCombatData;
			else
				return null;
		}
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		//公共渲染不变的东西
		int reset = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.FriendCampaignResetCount) - Data.AlreadyResetCount;
		resetCountLabel.Text = GameUtility.FormatUIString("UIPnlFriendCombatTab_ResetCount", reset < 0 ? 0 : reset);

		string str = ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetStageNameById(Data.PassStageId);
		if (str.Equals(string.Empty))
			str = GameUtility.GetUIString("UIPnlFriendCombatTab_PassLevel_Zeo");

		passLevel.Text = GameUtility.FormatUIString("UIPnlFriendCombatTab_PassLevel", GameDefines.textColorBtnYellow.ToString(), GameDefines.textColorWhite.ToString(), str);

		str = ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetStageNameById(Data.HistoryMaxDungeonId);
		if (str.Equals(string.Empty))
			str = GameUtility.GetUIString("UIPnlFriendCombatTab_PassLevel_Zeo");

		passLevelLast.Text = GameUtility.FormatUIString("UIPnlFriendCombatTab_PassLevel_Last", GameDefines.textColorBtnYellow.ToString(), GameDefines.textColorWhite.ToString(), str);

		dieCountLabel.Text = GameUtility.FormatUIString("UIPnlFriendCombatTab_DieCountLabel", GetNotDiePlayer(), Data.FriendPositions.Count);

		if (!ConfigDatabase.DefaultCfg.FriendCampaignConfig.JudgeStageIsPassById(Data.PassStageId))
			ShowNotPassUI();
		else
			ShowPassUI();

		return true;
	}

	private void InitAvatars()
	{
		for (int index = 0; index < avatars.Count; index++)
			avatars[index].InitData(PlayerDataUtility.GetBattlePosByIndexPos(index));
	}

	//获取未死亡的角色
	private int GetNotDiePlayer()
	{
		int count = 0;
		for (int index = 0; index < Data.FriendPositions.Count; index++)
		{
			if (Data.FriendPositions[index].TotalLeftHpPercent > 0)
				count++;
		}

		return count;
	}

	//渲染未通关的
	private void ShowNotPassUI()
	{
		//隐藏通关UI控件
		SetPassUI();

		string str = ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetStageNameById(ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetNextStageIdById(Data.PassStageId));
		if (str.Equals(string.Empty))
			str = GameUtility.GetUIString("UIPnlFriendCombatTab_PassLevel_Zeo");

		enemyStageLevel.Text = GameUtility.FormatUIString("UIPnlFriendCombatTab_NexPassLevel", str);

		//Clean Avatars.
		InitAvatars();

		if (Data.RobotInfo.isRobot)
		{
			enemyPlayerName.Text = GameUtility.FormatUIString("UIPnlFriendCombatTab_EnemyNamePosition", Data.RobotInfo.name);

			bool isRecruteAvatar = true;

			for (int index = 0; index < avatars.Count; index++)
			{
				for (int i = 0; i < Data.RobotInfo.robotNpcs.Count; i++)
				{
					if (avatars[index].Position == Data.RobotInfo.robotNpcs[i].battlePosition)
					{
						avatars[index].SetData(Data.RobotInfo.robotNpcs[i], isRecruteAvatar, GetHpByLocationId(Data.RobotInfo.robotNpcs[i].battlePosition));
						isRecruteAvatar = false;
					}
				}
			}
		}
		else
		{
			enemyPlayerName.Text = GameUtility.FormatUIString("UIPnlFriendCombatTab_EnemyNamePosition", Data.EnemyPlayer.Name);
			//Fill Avatars.
			var positionData = Data.EnemyPlayer.PositionData;

			if (positionData != null)
			{
				for (int index = 0; index < avatars.Count; index++)
				{
					for (int j = 0; j < positionData.Positions[0].AvatarLocations.Count; j++)
					{
						if (positionData.Positions[0].AvatarLocations[j].LocationId == avatars[index].Position)
						{
							avatars[index].SetData(Data.EnemyPlayer.SearchAvatar(positionData.Positions[0].AvatarLocations[j].Guid),
													positionData.Positions[0].AvatarLocations[j].LocationId == positionData.Positions[0].EmployLocationId,
													GetHpByLocationId(positionData.Positions[0].AvatarLocations[j].LocationId));
							break;
						}
					}
				}
			}
		}
	}

	//获取角色血量
	public double GetHpByLocationId(int location)
	{
		for (int index = 0; index < Data.EnemyHpInfos.Count; index++)
			if (Data.EnemyHpInfos[index].LocationId == location)
				return Data.EnemyHpInfos[index].LeftHP;

		return 0;
	}

	//渲染通关的
	private void ShowPassUI()
	{
		SetNotPassUI();

		passLabel.Text = GameUtility.GetUIString("UIPnlFriendCombatTab_PassLabel");
	}

	//设置隐藏未通关UI控件
	private void SetNotPassUI()
	{
		enemyStageLevel.Text = string.Empty;
		enemyPlayerName.Text = string.Empty;
		levelRewardButton.Hide(true);
		enemyTextBgBox.Hide(true);

		for (int index = 0; index < this.avatars.Count; index++)
			this.avatars[index].ClearData();

		//this.backBgBox.height = this.PassBgBoxHeight;
		this.backBgBox.SetSize(this.backBgBox.width, this.PassBgBoxHeight);
		this.childLayout.transform.localPosition = this.passButtonT.localPosition;

		SetChildButton(false, true);
	}

	//设置隐藏通关UI控件
	private void SetPassUI()
	{
		levelRewardButton.Hide(false);
		enemyTextBgBox.Hide(false);

		this.passActionBgBox.Hide(true);
		this.passLabel.Text = string.Empty;
		this.passAction01.Hide(true);
		this.passAction02.Hide(true);
		this.passText.Hide(true);

		//this.backBgBox.height = this.NotPassBgBoxHeight;
		this.backBgBox.SetSize(this.backBgBox.width, this.NotPassBgBoxHeight);
		this.childLayout.transform.localPosition = this.notPassButtonT.localPosition;

		SetChildButton(false, false);
	}

	//设置按钮
	private void SetChildButton(params bool[] hideF)
	{
		for (int index = 0; index < Mathf.Min(hideF.Length, childLayout.childLayoutControls.Length); index++)
			childLayout.HideChildObj(childLayout.childLayoutControls[index].gameObject, hideF[index]);
	}

	//点击说明
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickGuide(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlFriendGuide), Data.HistoryMaxDungeonId);
	}

	//点击本关奖励
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickReward(UIButton btn)
	{
		int last = ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetStageIndex(Data.HistoryMaxDungeonId);
		int know = ConfigDatabase.DefaultCfg.FriendCampaignConfig.GetNexStageById(Data.PassStageId);
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgFriendCampaginChackPoint), (know > last ? false : true), Data.PassStageId);
	}

	//点击重置
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickReset(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgFriendCampaignReset), Data.AlreadyResetCount);
	}

	//点击挑战
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickCombat(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlFriendBattle), Data.FriendPositions, Data.AlreadyResetCount, Data.LastFriendPositionId);
	}
}

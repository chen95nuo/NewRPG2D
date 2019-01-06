using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgWolfEnemyExpedition : UIModule
{
	public SpriteText title_Text;//标题
	public SpriteText levelname;//关卡名称
	public SpriteText failNumber;//可失败次数
	public SpriteText powerText;///战力

	//阵容下面人物角色
	public List<UIElemWoldExpeditionBattles> lineUpAvatars;

	private KodGames.ClientClass.WolfInfo wolfInfo;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;


		for (int index = 0; index < lineUpAvatars.Count; index++)
			lineUpAvatars[index].Init(PlayerDataUtility.GetBattlePosByIndexPos(index));

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		wolfInfo = userDatas[2] as KodGames.ClientClass.WolfInfo;

		ShowTips(userDatas[0] as KodGames.ClientClass.Player, (int)userDatas[3]);

		ShowPosition(userDatas[0] as KodGames.ClientClass.Player, userDatas[1] as KodGames.ClientClass.PositionData);

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		for (int index = 0; index < lineUpAvatars.Count; index++)
			lineUpAvatars[index].ClearData();
	}

	//渲染提示
	private void ShowTips(KodGames.ClientClass.Player player, int power)
	{
		title_Text.Text = GameUtility.FormatUIString("UIDlgWolfEnemyExpedition_Sequence", ItemInfoUtility.GetLevelCN(ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetStageSequenceById(wolfInfo.StageId)));
		levelname.Text = GameUtility.FormatUIString("UIDlgWolfEnemyExpedition_LevelName", GameDefines.textColorBtnYellow.ToString(), ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetStageNameById(wolfInfo.StageId),
																							GameDefines.textColorWhite.ToString(), player.Name);
		failNumber.Text = (ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel,
											VipConfig._VipLimitType.WolfSmokeAddCanFaildCount) - wolfInfo.AlreadyFailedTimes).ToString();
		if (power > 0)
			powerText.Text = GameUtility.FormatUIString("UIPnlAvatar_Power", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, PlayerDataUtility.GetPowerString(power));
		else
			powerText.Text = GameUtility.FormatUIString("UIPnlAvatar_NoPower", GameDefines.textColorBtnYellow, GameDefines.textColorWhite);

	}

	//根据阵容ID去渲染阵容内的角色
	private void ShowPosition(KodGames.ClientClass.Player player, KodGames.ClientClass.PositionData position)
	{
		//渲染之前清空一下内容
		ClearData();

		//先去角色身上按照阵容ID去找到该阵容下有多少个角色
		var positionData = position.Positions[0];

		if (positionData != null)
		{
			for (int i = 0; i < lineUpAvatars.Count; i++)
			{
				KodGames.ClientClass.Avatar avatar = null;
				bool isRecruite = false;

				//对比他们之间的位置
				for (int j = 0; j < positionData.AvatarLocations.Count; j++)
				{
					if (positionData.AvatarLocations[j].LocationId == lineUpAvatars[i].Position)
					{
						avatar = player.SearchAvatar(positionData.AvatarLocations[j].Guid);
						isRecruite = positionData.AvatarLocations[j].LocationId == positionData.EmployLocationId;
						break;
					}
				}

				lineUpAvatars[i].SetData(avatar, isRecruite);
			}
		}
	}

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	//点击挑战
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickGain(UIButton btn)
	{

		//点击挑战，判断是否还有挑战次数，没有弹重置面板，有才弹增益面板
		int fail = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel,
											VipConfig._VipLimitType.WolfSmokeAddCanFaildCount) - wolfInfo.AlreadyFailedTimes;
		if (fail > 0)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgWolfGain), wolfInfo.StageId);
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgWolfFailyDeficiency), wolfInfo);

		//不管是什么情况，关闭自己
		HideSelf();
	}
}

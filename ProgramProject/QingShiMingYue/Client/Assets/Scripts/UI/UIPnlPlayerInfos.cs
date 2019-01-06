using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlPlayerInfos : UIModule
{
	private enum UICondition
	{
		Undifined = 0,
		Default = 1,
		PackageAvatar = 2,
		PackageEquip = 3,
		PackageSkill = 4,
		ShopWine = 5,
		ShopMystery = 6,
		Meridian = 7,
		AvatarDiner = 8,
		TowerShopNormal = 9,
		BreakThrough = 10,
		Domineer = 11,
		Equipment = 12,
		WolfShop = 13,
		Adventure = 14,
		Guild = 15,
		GuildApply = 16,
	}

	private class AssetType
	{
		public const int Unknow = 0;
		public const int PlayerGameMoney = 1 << 0;				 // 银币
		public const int PlayerRealMoney = 1 << 1;				 // 元宝
		public const int PlayerStamina = 1 << 2;				 // 体力
		public const int PlayerSoul = 1 << 3;					 // 元神
		public const int PlayerAvatarBreakthrough = 1 << 4;		 // 突破丹
		public const int PlayerChangeMeridian = 1 << 5;			 // 紫晶丹
		public const int PlayerNurseMeridian = 1 << 6;			 // 舒脉丹
		public const int PlayerIron = 1 << 7;					 // 陨铁
		public const int PlayerSpirit = 1 << 8;					 // 精魄
		public const int PlayerTrialStamp = 1 << 9;			 // 试练印
		public const int PlayerDomineer = 1 << 10;				 // 霸气丹
		public const int PlayerMedals = 1 << 11;				 // 军功
		public const int PlayerEnergy = 1 << 12;				 // 精力
		public const int PlayerWineSoul = 1 << 13;				//酒魂
		public const int PlayerEquipBreakthrough = 1 << 14;		// 精炼石
		public const int PlayerGuildMoney = 1 << 15;			//门派粮草
		public const int PlayerAvatarUpDan = 1 << 16;			//升星丹		
		public const int PlayerGuildBossCount = 1 << 17;		//门派禁地奖章

		public static int GetAssetIdByType(int assetType)
		{
			switch (assetType)
			{
				case PlayerGameMoney:
					return IDSeg._SpecialId.GameMoney;
				case PlayerRealMoney:
					return IDSeg._SpecialId.RealMoney;
				case PlayerStamina:
					return IDSeg._SpecialId.Stamina;
				case PlayerSoul:
					return IDSeg._SpecialId.Soul;
				case PlayerIron:
					return IDSeg._SpecialId.Iron;
				case PlayerSpirit:
					return IDSeg._SpecialId.Spirit;
				case PlayerTrialStamp:
					return IDSeg._SpecialId.TrialStamp;
				case PlayerMedals:
					return IDSeg._SpecialId.Medals;
				case PlayerEnergy:
					return IDSeg._SpecialId.Energy;
				case PlayerAvatarBreakthrough:
					return ConfigDatabase.DefaultCfg.ItemConfig.avatarBreakThroughItemId;
				case PlayerChangeMeridian:
					return ConfigDatabase.DefaultCfg.ItemConfig.changeMeridianItemId;
				case PlayerNurseMeridian:
					return ConfigDatabase.DefaultCfg.ItemConfig.nurseMeridianItemId;
				case PlayerDomineer:
					return ConfigDatabase.DefaultCfg.ItemConfig.domineerItemId;
				case PlayerWineSoul:
					return IDSeg._SpecialId.WineSoul;
				case PlayerEquipBreakthrough:
					return ConfigDatabase.DefaultCfg.ItemConfig.equipmentBreakThroughItemId;
				case PlayerAvatarUpDan:
					return ConfigDatabase.DefaultCfg.ItemConfig.avatarUpDan;
				case PlayerGuildMoney:
					return IDSeg._SpecialId.GuildMoney;
				case PlayerGuildBossCount:
					return IDSeg._SpecialId.GuildBossCount;
			}

			return IDSeg.InvalidId;
		}
	}

	[System.Serializable]
	public class PlayerInfoControl
	{
		public AutoSpriteControlBase controlBase;
		public UIElemAssetIcon uiBase;

		public string Text
		{
			get { return uiBase.border.Text; }
			set { uiBase.border.Text = value; }
		}

		private int assetType;
		public int AssetTypes
		{
			get { return assetType; }
			set
			{
				assetType = value;
				assetId = AssetType.GetAssetIdByType(assetType);
				if (assetId != IDSeg.InvalidId)
				{
					controlBase.Data = assetId;
					uiBase.SetData(assetId);
				}
			}
		}

		private object data = null;
		public object Data
		{
			get { return data; }
			set { data = value; }
		}

		private int assetId;
		public int AssetId
		{
			get { return assetId; }
		}

		public bool IsHide
		{
			get { return !controlBase.gameObject.activeInHierarchy; }
		}
	}

	public UIChildLayoutControl layoutCoutrol;
	public List<PlayerInfoControl> playerInfoControls;

	private float delta = 0f;
	private UICondition lastCondition = UICondition.Undifined;
	private Dictionary<UICondition, int> register_dic = new Dictionary<UICondition, int>();

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		// Regist UI with Type.
		register_dic.Add(UICondition.Default, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerStamina);
		register_dic.Add(UICondition.PackageAvatar, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerSoul);
		register_dic.Add(UICondition.PackageEquip, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerIron);
		register_dic.Add(UICondition.PackageSkill, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerStamina);
		register_dic.Add(UICondition.ShopWine, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerWineSoul | AssetType.PlayerSoul);
		register_dic.Add(UICondition.ShopMystery, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerWineSoul | AssetType.PlayerSoul);
		register_dic.Add(UICondition.Meridian, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerChangeMeridian | AssetType.PlayerNurseMeridian);
		register_dic.Add(UICondition.AvatarDiner, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerSpirit);
		register_dic.Add(UICondition.TowerShopNormal, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerTrialStamp);

		if (ConfigDatabase.DefaultCfg.GameConfig.isShowAvatarUpDan)
			register_dic.Add(UICondition.BreakThrough, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerAvatarBreakthrough | AssetType.PlayerAvatarUpDan);
		else
			register_dic.Add(UICondition.BreakThrough, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerAvatarBreakthrough);

		register_dic.Add(UICondition.Domineer, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerDomineer | AssetType.PlayerSpirit);
		register_dic.Add(UICondition.Equipment, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerIron | AssetType.PlayerEquipBreakthrough);
		register_dic.Add(UICondition.WolfShop, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerMedals);
		register_dic.Add(UICondition.Adventure, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerEnergy);
		register_dic.Add(UICondition.Guild, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerGuildBossCount | AssetType.PlayerGuildMoney);
		register_dic.Add(UICondition.GuildApply, AssetType.PlayerGameMoney | AssetType.PlayerRealMoney | AssetType.PlayerStamina | AssetType.PlayerEnergy);

		// Set TabButton Data.
		playerInfoControls[0].AssetTypes = AssetType.PlayerRealMoney;
		playerInfoControls[1].AssetTypes = AssetType.PlayerGameMoney;
		playerInfoControls[2].AssetTypes = AssetType.PlayerTrialStamp;
		playerInfoControls[3].AssetTypes = AssetType.PlayerAvatarBreakthrough;
		playerInfoControls[4].AssetTypes = AssetType.PlayerChangeMeridian;
		playerInfoControls[5].AssetTypes = AssetType.PlayerDomineer;
		playerInfoControls[6].AssetTypes = AssetType.PlayerStamina;
		playerInfoControls[7].AssetTypes = AssetType.PlayerIron;
		playerInfoControls[8].AssetTypes = AssetType.PlayerNurseMeridian;
		playerInfoControls[9].AssetTypes = AssetType.PlayerSoul;
		playerInfoControls[10].AssetTypes = AssetType.PlayerSpirit;
		playerInfoControls[11].AssetTypes = AssetType.PlayerMedals;
		playerInfoControls[12].AssetTypes = AssetType.PlayerEnergy;
		playerInfoControls[13].AssetTypes = AssetType.PlayerWineSoul;
		playerInfoControls[14].AssetTypes = AssetType.PlayerEquipBreakthrough;
		playerInfoControls[15].AssetTypes = AssetType.PlayerGuildMoney;
		playerInfoControls[16].AssetTypes = AssetType.PlayerAvatarUpDan;
		playerInfoControls[17].AssetTypes = AssetType.PlayerGuildBossCount;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		UpdateOnShowItem();
		RefreshPlayInfo();

		return true;
	}

	private void Update()
	{
		UpdateOnShowItem();

		delta += Time.deltaTime;
		if (delta > 1.0f)
		{
			delta = 0f;
			RefreshPlayInfo();
		}
	}

	private void UpdateOnShowItem()
	{
		UICondition currentCondition = UICondition.Default;

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlShopWine))
			currentCondition = UICondition.ShopWine;

		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlShopMystery))
			currentCondition = UICondition.ShopMystery;

		else if ((SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageAvatarTab) && !SysUIEnv.Instance.GetUIModule<UIPnlPackageAvatarTab>().IsOverlayed) ||
			(SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageSell) && SysUIEnv.Instance.GetUIModule<UIPnlPackageSell>().Type == _UIType.UIPnlPackageAvatarTab) ||
			(SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlSelectAvatarList)))
			currentCondition = UICondition.PackageAvatar;

		else if ((SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageEquipTab) && !SysUIEnv.Instance.GetUIModule<UIPnlPackageEquipTab>().IsOverlayed) ||
			(SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageSell) && SysUIEnv.Instance.GetUIModule<UIPnlPackageSell>().Type == _UIType.UIPnlPackageEquipTab) ||
			(SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlSelectEquipmentList)))
			currentCondition = UICondition.PackageEquip;

		else if ((SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageSkillTab) && !SysUIEnv.Instance.GetUIModule<UIPnlPackageSkillTab>().IsOverlayed) ||
			(SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageSell) && SysUIEnv.Instance.GetUIModule<UIPnlPackageSell>().Type == _UIType.UIPnlPackageSkillTab) ||
			(SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlSelectSkillList)))
			currentCondition = UICondition.PackageSkill;

		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAvatarMeridianTab))
			currentCondition = UICondition.Meridian;

		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAvatarDiner))
			currentCondition = UICondition.AvatarDiner;

		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlTowerShop))
			currentCondition = UICondition.TowerShopNormal;

		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAvatarBreakThrough))
			currentCondition = UICondition.BreakThrough;

		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAvatarDomineerTab))
			currentCondition = UICondition.Domineer;

		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlEquipmentRefine))
			currentCondition = UICondition.Equipment;

		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlWolfShop))
			currentCondition = UICondition.WolfShop;

		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAdventureMain))
			currentCondition = UICondition.Adventure;

		else if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlGuildMenuBot)) || SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildPointMain) || SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildPointRankTab))
			currentCondition = UICondition.Guild;

		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildApplyList))
			currentCondition = UICondition.GuildApply;

		if (lastCondition != currentCondition)
		{
			// Set Last Condition.
			lastCondition = currentCondition;

			// Get Regist UI Value.
			int registValue = 0;
			if (register_dic.ContainsKey(currentCondition))
				registValue = register_dic[currentCondition];

			// Show Regist UI.
			for (int index = 0; index < playerInfoControls.Count; index++)
				layoutCoutrol.HideChildObj(playerInfoControls[index].controlBase.gameObject, (registValue & playerInfoControls[index].AssetTypes) == 0);
		}
	}

	private void RefreshPlayInfo()
	{
		for (int index = 0; index < playerInfoControls.Count; index++)
		{
			if (playerInfoControls[index].IsHide)
				continue;

			switch (playerInfoControls[index].AssetTypes)
			{
				case AssetType.PlayerStamina:
					int currentStamina = KodGames.Math.CombineValue(SysLocalDataBase.Inst.LocalPlayer.Stamina.Point.Value, SysLocalDataBase.Inst.LocalPlayer.Stamina.MaxPoint);
					if (playerInfoControls[index].Data == null || (int)playerInfoControls[index].Data != currentStamina)
					{
						playerInfoControls[index].Data = currentStamina;
						playerInfoControls[index].Text = GameUtility.FormatUIString(
							"UIPnlPlayerInfos_CountWithMax",
							SysLocalDataBase.Inst.LocalPlayer.Stamina.Point.Value >= 1000 ? GameUtility.GetUIString("UIPnlPlayerInfos_ADD") : SysLocalDataBase.Inst.LocalPlayer.Stamina.Point.Value.ToString(),
							SysLocalDataBase.Inst.LocalPlayer.Stamina.MaxPoint);
					}
					break;

				case AssetType.PlayerEnergy:

					int currentEnergy = KodGames.Math.CombineValue(SysLocalDataBase.Inst.LocalPlayer.Energy.Point.Value, SysLocalDataBase.Inst.LocalPlayer.Energy.MaxPoint);
					if (playerInfoControls[index].Data == null || (int)playerInfoControls[index].Data != currentEnergy)
					{
						playerInfoControls[index].Data = currentEnergy;
						playerInfoControls[index].Text = GameUtility.FormatUIString(
							"UIPnlPlayerInfos_CountWithMax",
							SysLocalDataBase.Inst.LocalPlayer.Energy.Point.Value >= 1000 ? GameUtility.GetUIString("UIPnlPlayerInfos_ADD") : SysLocalDataBase.Inst.LocalPlayer.Energy.Point.Value.ToString(),
							SysLocalDataBase.Inst.LocalPlayer.Energy.MaxPoint);
					}
					break;

				default:

					int tempCount = ItemInfoUtility.GetGameItemCount(playerInfoControls[index].AssetId);
					if (playerInfoControls[index].Data == null || (int)playerInfoControls[index].Data != tempCount)
					{
						playerInfoControls[index].Data = tempCount;
						playerInfoControls[index].Text = ItemInfoUtility.GetItemCountStr(tempCount);
					}

					break;
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickItem(UIButton btn)
	{
		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(ItemInfoUtility.GetAssetDesc((int)btn.Data), true, true);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}
}
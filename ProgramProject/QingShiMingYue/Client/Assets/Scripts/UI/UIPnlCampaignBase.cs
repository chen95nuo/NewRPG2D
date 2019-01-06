using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlCampaignBase : UIModule
{
	public List<AutoSpriteControlBase> campaignNameBtns;
	public List<UIBox> campaignRewardIcons;

	public class CampaignRecrod
	{
		public int jumpZoneId; // 上次战斗章节id 或者 跳转章节id
		public int jumpDungeonId; // 跳转副本id
		public int dungeonDiff; // 副本难度
		public float dungeonScrollPosition;
		public bool combatBack; // 战斗返回数据

		public CampaignRecrod() : this(IDSeg.InvalidId, IDSeg.InvalidId, _DungeonDifficulity.Unknow, -1) { }

		public CampaignRecrod(int zoneId) : this(zoneId, IDSeg.InvalidId, _DungeonDifficulity.Unknow, -1) { }

		public CampaignRecrod(int zoneId, int jumpDungeonId) : this(zoneId, jumpDungeonId, _DungeonDifficulity.Unknow, -1) { }

		public CampaignRecrod(int zoneId, float dungeonScrollPosition, bool combatBack) : this(zoneId, IDSeg.InvalidId, _DungeonDifficulity.Unknow, dungeonScrollPosition, combatBack) { }

		public CampaignRecrod(int zoneId, int jumpDungeonId, int dungeonDiff) : this(zoneId, jumpDungeonId, dungeonDiff, -1) { }

		public CampaignRecrod(int zoneId, int jumpDungeonId, int dungeonDiff, float dungeonScrollPosition) : this(zoneId, jumpDungeonId, dungeonDiff, dungeonScrollPosition, false) { }

		public CampaignRecrod(int zoneId, int jumpDungeonId, int dungeonDiff, float dungeonScrollPosition, bool combatBack)
		{
			this.jumpZoneId = zoneId;
			this.jumpDungeonId = jumpDungeonId;
			this.dungeonDiff = dungeonDiff;
			this.dungeonScrollPosition = dungeonScrollPosition;
			this.combatBack = combatBack;
		}

		public void ShallowCopy(CampaignRecrod record)
		{
			this.jumpZoneId = record.jumpZoneId;
			this.jumpDungeonId = record.jumpDungeonId;
			this.dungeonDiff = record.dungeonDiff;
			this.dungeonScrollPosition = record.dungeonScrollPosition;
			this.combatBack = record.combatBack;
		}

		public bool ShowDungeonMapUI()
		{
			return jumpDungeonId != IDSeg.InvalidId || dungeonDiff != _DungeonDifficulity.Unknow || combatBack;
		}

		public int GetDungeonDiffType()
		{
			if (jumpDungeonId != 0)
				return ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(jumpDungeonId).DungeonDifficulty;
			else
				return dungeonDiff;
		}
	}

	public void SetCampaignStar(int zoneId)
	{
		CampaignData.ZoneReward normalZoneStar = CampaignData.GetZoneRewardState(zoneId);
		int zoneIndex = CampaignData.GetZoneIndexInCfg(zoneId);
		campaignRewardIcons[zoneIndex].Hide(normalZoneStar == CampaignData.ZoneReward.UnKonw);

		switch (normalZoneStar)
		{
			case CampaignData.ZoneReward.NormalOneReward:
			case CampaignData.ZoneReward.HardOneReward:
			case CampaignData.ZoneReward.NightmareOneReward:
				campaignRewardIcons[zoneIndex].SetState(0);
				break;
			case CampaignData.ZoneReward.NormalTwoReward:
			case CampaignData.ZoneReward.HardTwoReward:
			case CampaignData.ZoneReward.NightmareTwoReward:
				campaignRewardIcons[zoneIndex].SetState(1);
				break;
			case CampaignData.ZoneReward.NormalThreeReward:
			case CampaignData.ZoneReward.HardThreeReward:
			case CampaignData.ZoneReward.NightmareThreeReward:
				campaignRewardIcons[zoneIndex].SetState(2);
				break;
		}
	}
}

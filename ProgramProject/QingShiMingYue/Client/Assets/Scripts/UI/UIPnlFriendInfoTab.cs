using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlFriendInfoTab : UIModule
{
	public List<UIButton> tabButtons;

	private CombatData combatData;
	public CombatData GetInfoCombatData
	{
		get { return combatData; }
	}

	public class CombatData
	{
		private bool isJoin;
		public bool IsJoin { get { return isJoin; } }

		private int passStageId;
		public int PassStageId { get { return passStageId; } }

		private int lastPositionId;
		public int LastPositionId { get { return lastPositionId; } }

		private List<int> lastFriendIds;
		public List<int> LastFriendIds { get { return lastFriendIds; } }

		private int historyMaxDungeonId;
		public int HistoryMaxDungeonId { get { return historyMaxDungeonId; } }

		private int alreadyResetCount;
		public int AlreadyResetCount { get { return alreadyResetCount; } }

		private KodGames.ClientClass.Player enemyPlayer;
		public KodGames.ClientClass.Player EnemyPlayer { get { return enemyPlayer; } }

		private List<KodGames.ClientClass.HpInfo> enemyHpInfos;
		public List<KodGames.ClientClass.HpInfo> EnemyHpInfos { get { return enemyHpInfos; } }

		private List<KodGames.ClientClass.FriendCampaignPosition> friendPositions;
		public List<KodGames.ClientClass.FriendCampaignPosition> FriendPositions { get { return friendPositions; } }

		private int lastFriendPositionId;
		public int LastFriendPositionId { get { return lastFriendPositionId; } }

		private com.kodgames.corgi.protocol.RobotInfo robotInfo;
		public com.kodgames.corgi.protocol.RobotInfo RobotInfo { get { return robotInfo; } }

		public CombatData(bool isJoin, int passStageId, int lastPositionId, List<int> lastFriendIds, int historyMaxDungeonId, int alreadyResetCount,
		KodGames.ClientClass.Player enemyPlayer, List<KodGames.ClientClass.HpInfo> enemyHpInfos, List<KodGames.ClientClass.FriendCampaignPosition> friendPositions, int lastFriendPositionId,
			com.kodgames.corgi.protocol.RobotInfo robotInfo)
		{
			this.isJoin = isJoin;
			this.passStageId = passStageId;
			this.lastPositionId = lastPositionId;
			this.lastFriendIds = lastFriendIds;
			this.historyMaxDungeonId = historyMaxDungeonId;
			this.alreadyResetCount = alreadyResetCount;
			this.enemyPlayer = enemyPlayer;
			this.enemyHpInfos = enemyHpInfos;
			this.friendPositions = friendPositions;
			this.lastFriendPositionId = lastFriendPositionId;
			this.robotInfo = robotInfo;

			for (int index = 0; index < friendPositions.Count; index++)
			{
				if (friendPositions[index].Locations != null && friendPositions[index].Locations.Count > 0)
				{
					//设置阵容数据
					for (int i = 0; i < friendPositions[index].Locations.Count; i++)
					{
						for (int j = 0; j < friendPositions[index].Player.PositionData.Positions[0].AvatarLocations.Count; j++)
						{
							if (friendPositions[index].Player.PositionData.Positions[0].AvatarLocations[j].Guid.Equals(friendPositions[index].Locations[i].Guid))
								friendPositions[index].Player.PositionData.Positions[0].AvatarLocations[j].LocationId = friendPositions[index].Locations[i].LocationId;
						}
					}
				}
			}
		}

	}

	private int UIType;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabButtons[0].Data = _UIType.UIPnlFriendCombatTab;
		tabButtons[1].Data = _UIType.UIPnlFriendCampaginThisWeekRank;
		tabButtons[2].Data = _UIType.UIPnlFriendCampaginLastWeekRank;
		tabButtons[3].Data = _UIType.UIPnlFriendCampaginWeekReward;

		this.UIType = (int)tabButtons[0].Data;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		this.combatData = userDatas[0] as CombatData;

		ChangeTab(_UIType.UIPnlFriendCombatTab);

		return true;
	}

	public override void Overlay()
	{
		base.Overlay();

		//解决第一次进入的时候造成点击战斗时不能关闭子界面的问题
		if (SysUIEnv.Instance.IsUIModuleShown(this.UIType))
			SysUIEnv.Instance.HideUIModule(this.UIType);
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		ChangeTab(this.UIType);
	}

	public void ChangeTab(int type)
	{
		this.UIType = type;

		foreach (var btn in tabButtons)
			btn.controlIsEnabled = ((int)btn.data) != type;

		SysUIEnv.Instance.ShowUIModule(type);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeType(UIButton btn)
	{
		ChangeTab((int)btn.Data);
	}
}

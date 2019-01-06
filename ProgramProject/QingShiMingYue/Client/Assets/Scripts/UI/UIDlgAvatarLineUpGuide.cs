using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgAvatarLineUpGuide : UIModule
{
	public SpriteText guideTitleLabel;
	public SpriteText guideDescLabel;
	public UIScrollList scrollList;
	public GameObjectPool starAppraisePool;
	public GameObjectPool avatarLineUpPool;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		if (userDatas[0] is int)
			InitView((int)userDatas[0]);
		else if (userDatas[0] is KodGames.ClientClass.Player)
			InitView(userDatas[0] as KodGames.ClientClass.Player);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0f;
	}

	private void InitView(int dungeonId)
	{
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);

		// Set Title.
		guideTitleLabel.Text = GameUtility.GetUIString("UIPnlCampaign_Guid_Title");

		// Set Message.
		guideDescLabel.Text = dungeonCfg.guideDesc;

		// Set Appraise Label.
		var uiStarElem = starAppraisePool.AllocateItem().GetComponent<UIElemDungeonGuidStar>();
		uiStarElem.SetData(dungeonCfg);
		scrollList.AddItem(uiStarElem.gameObject);

		// Set Npc.
		Dictionary<int, KodGames.ClientClass.DungeonGuideNpc> dic_npcData = new Dictionary<int, KodGames.ClientClass.DungeonGuideNpc>();
		foreach (var npcData in SysLocalDataBase.Inst.LocalPlayer.CampaignData.GetDungeonGuidNpcsByDungeonId(dungeonId))
		{
			if (npcData == null)
				continue;

			dic_npcData.Add(PlayerDataUtility.GetIndexPosByBattlePos(npcData.BattlePosition), npcData);
		}

		SetNpcView(dic_npcData);
	}

	// 机器人
	private void InitView(KodGames.ClientClass.Player player)
	{
		// Set Title.
		guideTitleLabel.Text = GameUtility.GetUIString("UIDlgAvatarLineUpGuid_Title");

		// Set Message.
		guideDescLabel.Text = GameUtility.GetUIString("UIDlgAvatarLineUpGuid_Message");

		// Set Npc Data.
		Dictionary<int, KodGames.Pair<int, int>> dic_npcData = new Dictionary<int, KodGames.Pair<int, int>>();
		foreach (var location in player.PositionData.GetPositionById(player.PositionData.ActivePositionId).AvatarLocations)
		{
			if (location == null)
				continue;

			var avatar = player.SearchAvatar(location.Guid);
			if (avatar == null)
			{
				Debug.Log("Avatar Not Found : " + location.Guid);
				continue;
			}

			dic_npcData.Add(PlayerDataUtility.GetIndexPosByBattlePos(location.LocationId), new KodGames.Pair<int, int>(avatar.ResourceId, avatar.LevelAttrib.Level));
		}

		SetNpcView(dic_npcData);
	}

	private void SetNpcView(Dictionary<int, KodGames.ClientClass.DungeonGuideNpc> dic_npcData)
	{
		var uiNpcElem = avatarLineUpPool.AllocateItem().GetComponent<UIElemDungeonGuidNpc>();

		for (int index = 0; index < uiNpcElem.npcDatas.Count; index++)
		{
			if (dic_npcData.ContainsKey(index))
				uiNpcElem.npcDatas[index].SetData(dic_npcData[index].AvatarResourceId, dic_npcData[index].Level, dic_npcData[index].TraitType, dic_npcData[index].Name);
			else
				uiNpcElem.npcDatas[index].SetEmpty();
		}

		scrollList.AddItem(uiNpcElem.gameObject);
	}

	private void SetNpcView(Dictionary<int, KodGames.Pair<int, int>> dic_npcData)
	{
		var uiNpcElem = avatarLineUpPool.AllocateItem().GetComponent<UIElemDungeonGuidNpc>();

		for (int index = 0; index < uiNpcElem.npcDatas.Count; index++)
		{
			if (dic_npcData.ContainsKey(index))
				uiNpcElem.npcDatas[index].SetData(dic_npcData[index].first, dic_npcData[index].second);
			else
				uiNpcElem.npcDatas[index].SetEmpty();
		}

		scrollList.AddItem(uiNpcElem.gameObject);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}
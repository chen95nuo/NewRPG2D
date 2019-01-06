using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlGuildPointTalentTab : UIModule
{
	public UIScrollList layerList;
	public GameObjectPool layerListPool;

	public UIScrollList talentList;
	public GameObjectPool talentListPool;
	public UIButton[] tabButtons;

	public SpriteText guildLevelLabel;
	public SpriteText talentCount;
	public SpriteText resetCount;
	public GameObject resetRoot;
	public UIBox listBg;
	public UIElemAssetIcon talentBossBg;

	private List<com.kodgames.corgi.protocol.BossTalent> bossTalents;

	private int defaultType;
	private GuildStageConfig.ValueRange layerRange;
	private List<GuildStageConfig.ValueRange> valueRanges;

	private float defaultBgHeight;
	private float defaultListHeight;
	private float height = 50f;

	private void Awake()
	{
		defaultBgHeight = listBg.height;
		defaultListHeight = talentList.viewableArea.y;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		defaultType = GuildStageConfig._TalentBossType.PassBoss;
		//初始化

		valueRanges = ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.TalentMapNumRanges;
		layerRange = valueRanges[0];

		tabButtons[0].Data = GuildStageConfig._TalentBossType.PassBoss;
		tabButtons[1].Data = GuildStageConfig._TalentBossType.ChallengeBoss;

		resetRoot.SetActive(false);
		var guildRoleCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.RoleId);

		if (guildRoleCfg.Rights.Contains(GuildConfig._RoleRightType.StageTalent))
		{
			resetRoot.gameObject.SetActive(true);
			listBg.SetSize(listBg.width, defaultBgHeight);
			talentList.viewableArea = new Vector2(talentList.viewableArea.x, defaultListHeight);
		}
		else
		{
			listBg.SetSize(listBg.width, defaultBgHeight + height);
			talentList.viewableArea = new Vector2(talentList.viewableArea.x, defaultListHeight + height);
		}		

		ChangeTabBtn();
		SendQueryTalentReq();
	
		return true;
	}

	public void SendQueryTalentReq()
	{
		RequestMgr.Inst.Request(new GuildStageQueryTalentReq(defaultType));
	}

	public void QueryTalentInfoSuccess(int talentPoint, List<com.kodgames.corgi.protocol.BossTalent> bossTalents, int alreadyResetTimes)
	{		
		this.bossTalents = bossTalents;
		
		guildLevelLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointTalentTab_GuildLevel", SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildLevel);
		talentCount.Text = GameUtility.FormatUIString("UIPnlGuildPointTalentTab_GuildTalent", talentPoint);
		resetCount.Text = GameUtility.FormatUIString("UIPnlGuildPointTalentTab_ResetCount", alreadyResetTimes);

		ClearData();
		StartCoroutine("FillBossList");
		StartCoroutine("FillLayerList");
	}

	private void ClearData()
	{
		StopCoroutine("FillBossList");
		talentList.ClearList(false);
		talentList.ScrollPosition = 0f;

		StopCoroutine("FillLayerList");
		layerList.ClearList(false);
		layerList.ScrollPosition = 0f;
	}

	private void ChangeTabBtn()
	{
		if (defaultType == GuildStageConfig._TalentBossType.PassBoss)
			talentBossBg.SetData(ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.UnSearchPassBossIconId);
		if (defaultType == GuildStageConfig._TalentBossType.ChallengeBoss)
			talentBossBg.SetData(ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.UnSearchChallengeBossIconId);

		foreach (UIButton tabBtn in tabButtons)
			tabBtn.controlIsEnabled = ((int)tabBtn.Data) != defaultType;
	}

	private void ChangeLayerBtn()
	{
		for (int index = 0; index < layerList.Count; index++)
		{
			if (layerList.GetItem(index).Data is UIElemGuildPointTalentLayerItem)
			{
				UIElemGuildPointTalentLayerItem item = layerList.GetItem(index).Data as UIElemGuildPointTalentLayerItem;
				if (item.ValueRangeData == layerRange)
					item.SetSelectedStat(true);
				else item.SetSelectedStat(false);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillBossList()
	{
		yield return null;

		for (int i = 0; i < bossTalents.Count; i++)
		{
			GuildStageConfig.GuildTalent talent = ConfigDatabase.DefaultCfg.GuildStageConfig.GetTalentById(bossTalents[i].talentId);
			
			bool inLayer = false;

			foreach (var mapNum in talent.MapNums)
			{
				if (mapNum >= layerRange.Min && mapNum <= layerRange.Max)
					inLayer = true;
			}

			if (inLayer && talent.BossType == defaultType)
			{
				UIElemGuildPointTalentBossItem item = talentListPool.AllocateItem().GetComponent<UIElemGuildPointTalentBossItem>();
				item.SetData(talent, bossTalents[i].level);
				talentList.AddItem(item.gameObject);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillLayerList()
	{
		yield return null;

		// 填充信息
		for (int i = 0; i < valueRanges.Count; i++)
		{
			UIListItemContainer uiContainer = layerListPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemGuildPointTalentLayerItem item = uiContainer.GetComponent<UIElemGuildPointTalentLayerItem>();
			uiContainer.data = item;

			item.SetData(valueRanges[i]);			
			layerList.AddItem(item.gameObject);
		}

		ChangeLayerBtn();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabButtonClick(UIButton btn)
	{
		defaultType = (int)btn.Data;
		SendQueryTalentReq();
		ChangeTabBtn();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnLayerButtonClick(UIButton btn)
	{
		layerRange = btn.Data as GuildStageConfig.ValueRange;

		StopCoroutine("FillBossList");
		talentList.ClearList(false);
		talentList.ScrollPosition = 0f;
		StartCoroutine("FillBossList");

		ChangeLayerBtn();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackBtnClick(UIButton btn)
	{		
		HideSelf();
	}

	//天赋重置
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickResetTalent(UIButton btn)
	{
		RequestMgr.Inst.Request(new GuildStageTalentResetReq());
	}

	//天赋加点
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAddTalent(UIButton btn)
	{
		GuildStageConfig.GuildTalent guildTalent = btn.Data as GuildStageConfig.GuildTalent;
		RequestMgr.Inst.Request(new GuildStageTalentAddReq(defaultType, guildTalent.TalentId));
	}
}


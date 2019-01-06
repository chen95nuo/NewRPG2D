using UnityEngine;
using ClientServerCommon;
using System.Collections.Generic;
using System.Collections;
using KodGames;

public class UIPnlGuildPointMain : UIModule
{
	public UIButton resetBtn;
	public UIButton layerUpBtn;
	public UIButton layerDownBtn;

	public SpriteText myExploreLabel;
	public SpriteText myActionLabel;
	public SpriteText myAdjustCount;
	public SpriteText mapName;
	public SpriteText coreBossLabel;
	public SpriteText challengeBossLabel;
	public SpriteText mapExplore;
	public SpriteText mapPassCondition;

	public SpriteText resetTime;

	public UIElemAssetIcon coreBossIcon;
	public UIElemAssetIcon challengeBossIcon;

	private int initPoint = 0;

	private List<com.kodgames.corgi.protocol.Stage> stages;
	private int explorePoint;
	private int freeChallengeCount;
	public int FreeChallengeCount
	{
		get { return freeChallengeCount; }
	}

	private int itemChallengeCount;
	public int ItemChallengeCount
	{
		get { return itemChallengeCount; }
	}

	private int needPassBossCount;

	private KodGames.ClientClass.Cost costs;
	public KodGames.ClientClass.Cost Costs
	{
		get { return costs; }
	}

	private int playerIndex;

	private int mapNum;
	public int MapNum
	{
		get { return mapNum; }
	}
	//总共BOSS数量
	private int passBossCount;
	private int challengeBossCount;

	//已打BOSS数量
	private int passBossCompleteCount;
	private int challengeBossCompleteCount;

	//地图探索点
	private int unSearchPoint = 0;
	private int allPointCount = 0;

	//使用HashSet便于检索提高效率
	HashSet<int> unKnowPoints = new HashSet<int>();

	private string preName;
	private string roadPreName;
	private long handResetTime;
	private float dealyTime = 0;
	private int handResetStatus;

	private int needGuildLevel;
	private bool isLastMap;
	private int guildLevel;

	public override void OnHide()
	{
		base.OnHide();

		unKnowPoints.Clear();
		GuildSceneData.Instance.MainCamera.enabled = false;
	}

	public override void Overlay()
	{
		base.Overlay();

		GuildSceneData.Instance.MainCamera.enabled = false;
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		GuildSceneData.Instance.MainCamera.enabled = true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		GuildSceneData.Instance.MainCamera.enabled = true;

		if (userDatas.Length >= 10)
		{
			this.stages = userDatas[0] as List<com.kodgames.corgi.protocol.Stage>;
			this.explorePoint = (int)userDatas[1];
			this.freeChallengeCount = (int)userDatas[2];
			this.itemChallengeCount = (int)userDatas[3];
			this.needPassBossCount = (int)userDatas[4];
			this.costs = userDatas[5] as KodGames.ClientClass.Cost;
			this.mapNum = (int)userDatas[6];
			this.playerIndex = (int)userDatas[7];
			this.preName = (string)userDatas[8];
			this.roadPreName = (string)userDatas[9];
			this.handResetTime = (long)userDatas[10];
			this.handResetStatus = (int)userDatas[11];
			this.needGuildLevel = (int)userDatas[12];
			this.isLastMap = (bool)userDatas[13];
			this.guildLevel = (int)userDatas[14];

			SceneInit();
			UIInit();
		}

		UIInit();

		return true;
	}

	private void Update()
	{
		dealyTime +=  Time.deltaTime;
		if (dealyTime > 1f && handResetStatus == GuildStageConfig._HandResetStatus.TimeNotCome)
		{
			dealyTime = 0;
			long time = handResetTime - SysLocalDataBase.Inst.LoginInfo.NowTime;
			if (time > 0)
				resetTime.Text = GameUtility.Time2String(time);
			else
			{
				resetTime.Text = "";
				resetBtn.controlIsEnabled = true;
			}
		}

	}

	public void QueryStageInfoSuccess(List<com.kodgames.corgi.protocol.Stage> stages, int explorePoint, int freeChallengeCount, int itemChallengeCount, int needPassBossCount, KodGames.ClientClass.Cost costs, int mapNum, int index, string preName, string roadPreName)
	{
		this.stages = stages;
		this.explorePoint = explorePoint;
		this.freeChallengeCount = freeChallengeCount;
		this.itemChallengeCount = itemChallengeCount;
		this.needPassBossCount = needPassBossCount;
		this.costs = costs;
		this.playerIndex = index;
		this.preName = preName;
		this.roadPreName = roadPreName;
		this.mapNum = mapNum;

		UIInit();
	}

	private void SceneInit()
	{
		//小地图显示
		for (int i = 0; i < stages.Count; i++)
		{
			if (stages[i].type == GuildStageConfig._StageType.Init)
				initPoint = stages[i].index;
		}

		passBossCount = 0;
		challengeBossCount = 0;

		passBossCompleteCount = 0;
		challengeBossCompleteCount = 0;

		//添加到小地图信息字典
		for (int i = 0; i < stages.Count; i++)
		{
			//查询BOSS
			if (stages[i].type == GuildStageConfig._StageType.PassBoss)
			{
				passBossCount++;
				if (stages[i].status == GuildStageConfig._StageStatus.Complete)
					passBossCompleteCount++;
			}

			if (stages[i].type == GuildStageConfig._StageType.ChallengeBoss)
			{
				challengeBossCount++;
				if (stages[i].status == GuildStageConfig._StageStatus.Complete)
					challengeBossCompleteCount++;
			}

			if (stages[i].type != GuildStageConfig._StageType.Road)
			{
				//用于计算探索率
				if (stages[i].status == GuildStageConfig._StageStatus.UnSearch)
					unSearchPoint++;

				//起始点不计入探索率
				if(stages[i].type != GuildStageConfig._StageType.Init)
					allPointCount++;

				if (stages[i].status == GuildStageConfig._StageStatus.UnSearch)
					unKnowPoints.Add(stages[i].index);
			}
		}
		//创建场景
		GuildSceneData.Instance.CreatePoint(stages, playerIndex, preName, roadPreName, unKnowPoints, mapNum);
	}

	public void UIInit()
	{
		coreBossIcon.SetData(ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.UnSearchPassBossIconId);
		challengeBossIcon.SetData(ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.UnSearchChallengeBossIconId);

		resetBtn.gameObject.SetActive(false);
		var guildRoleCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.RoleId);

		if (guildRoleCfg.Rights.Contains(GuildConfig._RoleRightType.HandResetStage))
		{
			resetBtn.gameObject.SetActive(true);
		}

		//重置时间按钮
		resetTime.Text = "";

		if (handResetStatus != GuildStageConfig._HandResetStatus.CanReset)
			resetBtn.controlIsEnabled = false;
		else
			resetBtn.controlIsEnabled = true;

		bool canDown = false;

		//上下层显示判断
		layerUpBtn.Hide(true);
		if (mapNum > 1)
			layerUpBtn.Hide(false);

		layerDownBtn.Hide(true);
		if (!isLastMap && passBossCompleteCount >= needPassBossCount && guildLevel >= needGuildLevel)
		{
			canDown = true;
			layerDownBtn.Hide(false);
		}

		myExploreLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointMain_Explore", GameDefines.textColorOrange, GameDefines.textColorWhite, explorePoint);
		myActionLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointMain_Action", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, ItemInfoUtility.GetGameItemCount(ConfigDatabase.DefaultCfg.ItemConfig.exploreItem));
		myAdjustCount.Text = GameUtility.FormatUIString("UIPnlGuildPointMain_Adjust", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, freeChallengeCount);
		coreBossLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointMain_CoreBoss", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, passBossCompleteCount, passBossCount);
		challengeBossLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointMain_ChallengeBoss", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, challengeBossCompleteCount, challengeBossCount);
		
		//过关条件
		if (this.isLastMap)
			mapPassCondition.Text = GameUtility.FormatUIString("UIPnlGuildPointMain_PassCondition4", GameDefines.textColorBtnYellow);
		else
		{
			if (needPassBossCount > 0 && needGuildLevel <= 0)
				mapPassCondition.Text = GameUtility.FormatUIString("UIPnlGuildPointMain_PassCondition", GameDefines.textColorBtnYellow, canDown ? GameDefines.textColorWhite : GameDefines.textColorRed, needPassBossCount);
			else if (needPassBossCount <= 0 && needGuildLevel > 0)
				mapPassCondition.Text = GameUtility.FormatUIString("UIPnlGuildPointMain_PassCondition2", GameDefines.textColorBtnYellow, canDown ? GameDefines.textColorWhite : GameDefines.textColorRed, needGuildLevel);
			else if (needPassBossCount > 0 && needGuildLevel > 0)
				mapPassCondition.Text = GameUtility.FormatUIString("UIPnlGuildPointMain_PassCondition3", GameDefines.textColorBtnYellow, canDown ? GameDefines.textColorWhite : GameDefines.textColorRed, needPassBossCount, needGuildLevel);
			else
				mapPassCondition.Text = "";
		}				
		mapName.Text = GameUtility.FormatUIString("UIPnlGuildPointMain_MapName", mapNum);

		int explorePec = (int)((allPointCount - unSearchPoint) * 100/allPointCount);

		mapExplore.Text = GameUtility.FormatUIString("UIPnlGuildPointMain_MapExplore", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, explorePec.ToString());
	}

	public void UpdateMapExplore()
	{
		unSearchPoint = 0;
		allPointCount = 0;
		//添加到小地图信息字典
		for (int i = 0; i < stages.Count; i++)
		{
			if (stages[i].type != GuildStageConfig._StageType.Road)
			{
				//用于计算探索率
				if (stages[i].status == GuildStageConfig._StageStatus.UnSearch)
					unSearchPoint++;

				//起始点不计入探索率
				if (stages[i].type != GuildStageConfig._StageType.Init)
					allPointCount++;
			}
		}
		int explorePec = (int)((allPointCount - unSearchPoint) * 100 / allPointCount);
		mapExplore.Text = GameUtility.FormatUIString("UIPnlGuildPointMain_MapExplore", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, explorePec.ToString());
	}

	//奖励查询
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGuildCheckRewardBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIPnlGuildPointPerson>();
	}

	//玩家列表
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGuildPlayerListBtn(UIButton btn)
	{
		RequestMgr.Inst.Request(new GuildQueryMemberReq(() =>
		{
			SysUIEnv.Instance.ShowUIModule<UIDlgGuildPlayerList>();
			return true;
		}));
	}

	//门派天赋
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTalentBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlGuildPointTalentTab));
	}

	//玩法说明
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGuildHelp(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildGuideDetail, ConfigDatabase.DefaultCfg.GuildConfig.GetMainTypeByGuideType(GuildConfig._GuideType.GuildStage));
		//SysUIEnv.Instance.ShowUIModule(typeof(UIPnlGuildGuide));
	}

	//重置关卡
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickResetBtn(UIButton btn)
	{

		MainMenuItem openBtn = new MainMenuItem();
		openBtn.ControlText = GameUtility.GetUIString("UIPnlGuildPointMain_ResetBtn");
		openBtn.Callback =
			(data) =>
			{
				RequestMgr.Inst.Request(new GuildStageResetReq());
				return true;
			};

		MainMenuItem cancelBtn = new MainMenuItem();
		cancelBtn.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel_Space");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(GameUtility.GetUIString("UIPnlGuildPointMain_ResetMessageTitle"),
					GameUtility.GetUIString("UIPnlGuildPointMain_ResetMessageTips"),
					cancelBtn, openBtn);
		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData, true);
	}

	//门派聊天
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGuildChatBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlChatTab, null, true);
	}

	//关卡排行
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGuildRankBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIPnlGuildPointRankTab>();
	}

	//刷新
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRefreshBtn(UIButton btn)
	{
		RequestMgr.Inst.Request(new QueryGuildStageReq(GuildStageConfig._LayerType.Now));
	}

	//返回
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackBtn(UIButton btn)
	{
		SysGameStateMachine.Instance.EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlGuildTab));	
	}

	//还原
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRrestoreBtn(UIButton btn)
	{
		if (GuildSceneData.Instance.IsMoving || GuildSceneData.Instance.IsCameraMoving)
			return;

		GuildSceneData.Instance.CamereRrestore();
	}

	//小地图
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSmallMapBtn(UIButton btn)
	{
		if (GuildSceneData.Instance.IsMoving || GuildSceneData.Instance.IsCameraMoving || GuildSceneData.Instance.IsCameraFar)
			return;

		GuildSceneData.Instance.CameraAway(initPoint, 5f);
	}

	//上一层
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickLayerUpBtn(UIButton btn)
	{
		if (GuildSceneData.Instance.IsMoving || GuildSceneData.Instance.IsCameraMoving)
			return;

		RequestMgr.Inst.Request(new QueryGuildStageReq(GuildStageConfig._LayerType.Previous));
	}

	//下一层
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickLayerDownBtn(UIButton btn)
	{
		if (GuildSceneData.Instance.IsMoving || GuildSceneData.Instance.IsCameraMoving)
			return;

		RequestMgr.Inst.Request(new QueryGuildStageReq(GuildStageConfig._LayerType.Next));
	}

	//关卡已被重置
	public void StageConnect()
	{
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK");
		okCallback.Callback = (userData) =>
		{
			SysGameStateMachine.Instance.EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlGuildTab));	
			return true;
		};

		string stageMessage = GameUtility.GetUIString("UIPnlGuildPointMain_StageConnecMessage");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(
			GameUtility.GetUIString("UIPnlGuildPointMain_StageConnec"),
			stageMessage,
			false,
			null,
			okCallback);

		SysUIEnv.Instance.HideUIModule<UIDlgMessage>();
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgMessage), _UILayer.TopMost, showData);
	}
}

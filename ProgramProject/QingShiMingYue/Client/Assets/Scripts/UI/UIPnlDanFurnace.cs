using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlDanFurnace : UIModule
{
	//普通宝箱
	public UIScrollList rewardList;
	public GameObjectPool rewardPool;

	//普通消耗
	public UIElemAssetIcon[] normalCost;
	//批量消耗
	public UIElemAssetIcon[] batchCost;

	public SpriteText boxCountLabel;
	public SpriteText todayTimesLabel;
	public SpriteText helpsTipsLabel;

	public GameObject uIFxRoot;
	public GameObject uiLianDanRoot;
	public GameObject danFurnace;

	public SpriteText activityTimeLabel;
	public SpriteText activityTitleLabel;

	public UIBox leftArrow;
	public UIBox rightArrow;

	public UIButton mashBtn;

	//活动显示奖励
	public UIBox showBox;
	public SpriteText alchemyTimesLabel;
	
	private List<com.kodgames.corgi.protocol.BoxReward> boxRewards;
	private List<KodGames.ClientClass.Cost> alchemyCosts;
	private List<KodGames.ClientClass.Cost> batchAlchemyCosts;
	private com.kodgames.corgi.protocol.AlchemyClientIcon alchemyClientIcon;
	private com.kodgames.corgi.protocol.ShowCounter showCounter;
	private long nextRefreshTime;
	private com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo;

	//左右箭头标记
	private int showIndex = 0;
	public int ShowIndex
	{
		get { return showIndex; }
		set { showIndex = value; }
	}

	//今日炼丹次数
	private int todayAlchemyCount = 0;
	public int TodayAlchemyCount
	{
		get { return todayAlchemyCount; }
	}

	public List<int> rewardIndexs;
	
	private float diffHeight = 50f;
	private System.DateTime loginTime;
	private System.DateTime endTime;

	private bool isNeedRefresh;
	private List<GameObject> effects = new List<GameObject>();

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return true;

		this.todayAlchemyCount = (int)userDatas[0];
		this.boxRewards = userDatas[1] as List<com.kodgames.corgi.protocol.BoxReward>;
		this.alchemyCosts = userDatas[2] as List<KodGames.ClientClass.Cost>;
		this.batchAlchemyCosts = userDatas[3] as List<KodGames.ClientClass.Cost>;
		this.alchemyClientIcon = userDatas[4] as com.kodgames.corgi.protocol.AlchemyClientIcon;
		this.showCounter = userDatas[5] as com.kodgames.corgi.protocol.ShowCounter;
		this.nextRefreshTime = (long)userDatas[6];
		this.decomposeInfo = userDatas[7] as com.kodgames.corgi.protocol.DecomposeInfo;

		SysUIEnv.Instance.GetUIModule<UIPnlDanMenuBot>().SetLight(_UIType.UIPnlDanFurnace);

		danFurnace = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.danFurnace));
		if (danFurnace != null)
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(uiLianDanRoot.gameObject, danFurnace);

		Init();

		if (SysUIEnv.Instance.GetUIModule<UIDlgDanMaterialGet>().MaterialReward != null)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanMaterialGet), SysLocalDataBase.CCRewardListToShowReward(SysUIEnv.Instance.GetUIModule<UIDlgDanMaterialGet>().MaterialReward.Reward));		

		if (SysUIEnv.Instance.GetUIModule<UIPnlDanMenuBot>().IsDanGet)		
			StartCoroutine("PlayStarAnimation");		

		return false;
	}

	public override void OnHide()
	{
		GameObject.Destroy(danFurnace.gameObject);
		foreach(var gameObj in effects)
		{
			GameObject.Destroy(gameObj);
		}

		base.OnHide();
	}

	public void UpdateDecomposeInfo(com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo)
	{
		this.decomposeInfo = decomposeInfo;
	}

	private void Update()
	{
		if (ActivityManager.Instance.GetActivity<ActivityAlchemy>()!= null && ActivityManager.Instance.GetActivity<ActivityAlchemy>().IsOpen)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlDanFurnace) && isNeedRefresh)
			{
				RequestMgr.Inst.Request(new QueryAlchemyReq());
				isNeedRefresh = false;
			}
		}
		else
		{
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlDanFurnaceActivity) && isNeedRefresh)
			{
				RequestMgr.Inst.Request(new QueryAlchemyReq());
				isNeedRefresh = false;
			}
		}

		if (rewardIndexs != null || rewardIndexs.Count != 0)
		{
			bool left = false;
			bool right = false;

			foreach (var index in rewardIndexs)
			{
				if (showIndex > index)
					left = true;
				if (showIndex < index)
					right = true;
			}

			SetArrow(left, right);
		}

		loginTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;
		endTime = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(nextRefreshTime);

		if (loginTime > endTime && isNeedRefresh)
		{
			RequestMgr.Inst.Request(new QueryAlchemyReq());
			isNeedRefresh = false;
		}		
	}

	public void UpDateCheck()
	{

	}

	public void OnQueryAlchemySuccess(int todayAlchemyCount, List<com.kodgames.corgi.protocol.BoxReward> boxRewards, List<KodGames.ClientClass.Cost> alchemyCosts, List<KodGames.ClientClass.Cost> batchAlchemyCosts, com.kodgames.corgi.protocol.AlchemyClientIcon alchemyClientIcon, com.kodgames.corgi.protocol.ShowCounter showCounter, long nextRefreshTime, com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo)
	{				
		this.boxRewards = boxRewards;
		this.alchemyCosts = alchemyCosts;
		this.batchAlchemyCosts = batchAlchemyCosts;
		this.todayAlchemyCount = todayAlchemyCount;
		this.alchemyClientIcon = alchemyClientIcon;
		this.showCounter = showCounter;
		this.nextRefreshTime = nextRefreshTime;
		this.decomposeInfo = decomposeInfo;

		Init();
	}

	private void SetArrow(bool left, bool right)
	{
		leftArrow.Hide(!left);
		rightArrow.Hide(!right);
	}

	public void Init()
	{
		isNeedRefresh = true;

		//初始化宝箱箭头显示数据
		showIndex = 0;		
		//隐藏底板
		mashBtn.Hide(true);

		ClearData();
		StartCoroutine("FillPackageList");

		if (activityTimeLabel != null && ActivityManager.Instance.GetActivity<ActivityAlchemy>() != null)
			activityTimeLabel.Text = ActivityManager.Instance.GetActivity<ActivityAlchemy>().GetActivityTime();

		int alreadyGetBoxCount = 0;
		for (int i = 0; i < boxRewards.Count; i++ )
		{
			if (boxRewards[i].hasPicked)
				alreadyGetBoxCount++;
		}

		boxCountLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanFurnace_RewardCount_Label"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, alreadyGetBoxCount, boxRewards.Count);
		todayTimesLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanFurnace_TodayTimes"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, todayAlchemyCount);

		NextCostShow();

		if (helpsTipsLabel != null)
			helpsTipsLabel.Text = alchemyClientIcon.NoActivityText;

		if(showBox != null)
			showBox.gameObject.SetActive(false);

		if (showBox != null && showCounter.rewards.Count > 0)		
		{
			showBox.gameObject.SetActive(true);
			alchemyTimesLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanFurnace_ShowReward_Count_Label"), showCounter.remainCount);
		}	
	}

	private void NextCostShow()
	{
		for (int i = 0; i < normalCost.Length; i++)
		{
			normalCost[i].Hide(true);
			if (i < alchemyCosts.Count)
			{
				normalCost[i].Hide(false);
				normalCost[i].SetData(alchemyCosts[i].Id, alchemyCosts[i].Count);
				normalCost[i].assetNameLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanFurnace_CostName_Label"), ItemInfoUtility.GetAssetName(alchemyCosts[i].Id));
			}
		}

		for (int i = 0; i < batchCost.Length; i++)
		{
			batchCost[i].Hide(true);
			if (i < batchAlchemyCosts.Count)
			{
				batchCost[i].Hide(false);
				batchCost[i].SetData(batchAlchemyCosts[i].Id, batchAlchemyCosts[i].Count);
				batchCost[i].assetNameLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanFurnace_CostName_Label"), ItemInfoUtility.GetAssetName(batchAlchemyCosts[i].Id));
			}
		}
	}

	private void ClearData()
	{
		StopCoroutine("FillPackageList");
		rewardList.ClearList(false);
		rewardList.ScrollPosition = 0f;
	}

	public bool CheckDanPackageCount()
	{
		int packageCount = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.DanMaxCount);

		if(SysLocalDataBase.Inst.LocalPlayer.Dans.Count >= packageCount)
			return false;
		else
			return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator FillPackageList()
	{
		yield return null;

		// 填充宝箱信息
		for (int i = 0; i < boxRewards.Count; i++)
		{
			UIElemAlchemyBox item = rewardPool.AllocateItem().GetComponent<UIElemAlchemyBox>();
			item.SetData(boxRewards[i], i);
			rewardList.AddItem(item.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnAlchemyClick(UIButton btn)
	{
		if (CheckDanPackageCount())
			RequestMgr.Inst.Request(new AlchemyReq(DanConfig._OperateType.Alchemy, alchemyCosts));
		else SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanMaxCount), this.decomposeInfo);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnAlchemyTimesClick(UIButton btn)
	{
		if (CheckDanPackageCount())
			RequestMgr.Inst.Request(new AlchemyReq(DanConfig._OperateType.BatchAlchemy, batchAlchemyCosts));
		else SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanMaxCount), this.decomposeInfo);	
	}

	//点击宝箱
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBoxRewardClick(UIButton btn)
	{
		var boxReward = btn.Data as com.kodgames.corgi.protocol.BoxReward;		

		bool alreadyPicked = false;
		bool okShow = false;
		bool couldPick = false;

		if (boxReward.hasPicked)
			alreadyPicked = true;
		else if (boxReward.alchemyCount <= todayAlchemyCount)
			couldPick = true;
		else
			okShow = true;

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanBoxRewardView), boxReward.rewards, boxReward.randomRewards, okShow, couldPick, alreadyPicked, boxReward);	
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickShowCountClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanShowCountView), showCounter);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickAssetIconClick(UIButton btn)
	{
		UIElemAssetIcon itemIcon = btn.Data as UIElemAssetIcon;
		ClientServerCommon.Reward item = itemIcon.Data as ClientServerCommon.Reward;
		GameUtility.ShowAssetInfoUI(item.id);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnDanHelpInfoClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgDanIntroduce), DanConfig._IntroduceType.DanFurnace);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected IEnumerator PlayStarAnimation()
	{
		SysUIEnv.Instance.GetUIModule<UIPnlDanMenuBot>().IsDanGet = false;

		mashBtn.Hide(false);

		GameObject creatEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.danFlyEffect));
		if (creatEffect != null)
			ObjectUtility.AttachToParentAndResetLocalPosAndRotation(uIFxRoot.gameObject, creatEffect);

		yield return new WaitForSeconds(1.0f);
		AudioManager.Instance.PlaySound(GameDefines.menu_Blade2, 0f);

		Vector3 menuTrans = SysUIEnv.Instance.GetUIModule<UIPnlDanMenuBot>().GetSceneItemPosition(_UIType.UIPnlDanAttic);
		menuTrans.y = menuTrans.y + diffHeight;

		AnimatePosition.Do(creatEffect,
								EZAnimation.ANIM_MODE.FromTo,
								creatEffect.transform.localPosition,
								menuTrans,
								EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Default),
								0.6f,
								0f,
								null,
								 (data) =>
								 {
									 GameObject danGetEffect = ResourceManager.Instance.InstantiateAsset<UnityEngine.GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.danGetEffect));
									 if (danGetEffect != null)
										 ObjectUtility.AttachToParentAndResetLocalPosAndRotation(uIFxRoot.gameObject, danGetEffect);
									 danGetEffect.transform.localPosition = creatEffect.transform.localPosition;

									 GameObject.Destroy(creatEffect.gameObject);
									 effects.Add(danGetEffect);
									 AudioManager.Instance.PlaySound(GameDefines.money, 0f);
									 mashBtn.Hide(true);
								 });
	}
}

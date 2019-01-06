using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;


public class UIPnlAdventureGetReward : UIModule
{
	//delayReward
	public GameObject delayRewardObject;

	public GameObject fixedRewardObject;
	public UIElemAssetIcon delayRewardIcon;
	public UIElemAssetIcon playerIcon;
	public GameObject UIFX_Q_YanShiJianLi_FontFX;
	public UIBox delaySign;

	//fixedReward
	public UIBox closeBtn;

	public GameObject closeEffectObj;
	public Animation openAnim;

	public GameObject openedEffectObj;

	public UIChildLayoutControl childLayout;
	public UIScrollList rewardList;
	public GameObjectPool rewardPool;
	public GameObject UIFX_Q_IconGetItem;


	private float delayTimer = 1f;
	private float fixedTimer = 2f;
	private float openBoxTimer = 1.3f;
	private List<com.kodgames.corgi.protocol.DelayReward> delayRewards;
	private List<Pair<int, int>> fixRewardPackagePars;
	private List<Pair<int, int>> randRewardPackagePars;
	private Animation delayAni;
	private com.kodgames.corgi.protocol.MarvellousProto marvellousProto;

	private int rewardCount = 0;
	private Vector3 delayRewardPoint = Vector3.zero;
	private Vector3 delayRewardScale = Vector3.zero;
	private bool isRotation = false;
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas)) return false;
		marvellousProto = userDatas[0] as com.kodgames.corgi.protocol.MarvellousProto;
		delayRewards = userDatas[1] as List<com.kodgames.corgi.protocol.DelayReward>;
		fixRewardPackagePars = userDatas[2] as List<Pair<int, int>>;
		randRewardPackagePars = userDatas[3] as List<Pair<int, int>>;

		if (delayRewardPoint == Vector3.zero)
			delayRewardPoint = delayRewardIcon.transform.position;
		if (delayRewardScale == Vector3.zero)
			delayRewardScale = delayRewardIcon.transform.localScale;

		ClearData();

		if (delayRewards != null && delayRewards.Count > 0)
			StartCoroutine("EnterDelayReward", delayTimer);
		else
			StartCoroutine("EnterFixedReward", fixedTimer);

		return true;
	}

	private void Update()
	{
		if (AdventureSceneData.Instance.HadDelayReward > 0)
			delaySign.Hide(false);
		else delaySign.Hide(true);

		if (isRotation)
			delayRewardIcon.transform.Rotate(Vector3.forward * Time.deltaTime * 400);
	}

	private void ClearData()
	{
		rewardList.ClearList(false);
		rewardCount = 0;
		delayRewardIcon.transform.position = delayRewardPoint;
		delayRewardIcon.transform.localScale = delayRewardScale;
		delayRewardIcon.transform.rotation = Quaternion.identity;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator EnterDelayReward(float timer)
	{
		InitDelayReward();
		SetDelayRewardData();
		yield return new WaitForSeconds(UIFX_Q_YanShiJianLi_FontFX.animation["UI_LevelUpFont"].length);
		delayAni.gameObject.SetActive(false);
		isRotation = true;
		AnimatePosition.Do(delayRewardIcon.gameObject, EZAnimation.ANIM_MODE.FromTo, delayRewardIcon.transform.position, playerIcon.transform.position,
			AnimatePosition.GetInterpolator(EZAnimation.EASING_TYPE.BackIn), 0.5f, 0f, null, null);


		AnimateScale.Do(delayRewardIcon.gameObject, EZAnimation.ANIM_MODE.FromTo, Vector3.one, Vector3.zero,
			AnimatePosition.GetInterpolator(EZAnimation.EASING_TYPE.BackIn), 0.5f, 0f, null, (data) =>
			{
				isRotation = false;
				DestroyObject(Instantiate(UIFX_Q_IconGetItem, playerIcon.transform.position, Quaternion.identity), 2f);
			});

		yield return new WaitForSeconds(timer);
		if ((fixRewardPackagePars != null && fixRewardPackagePars.Count > 0) || (randRewardPackagePars != null && randRewardPackagePars.Count > 0))
		{
			SetFixedRewardData();
			if (rewardCount > 0)
			{
				InitFixedReward();
				yield return new WaitForSeconds(openBoxTimer);
				openedEffectObj.SetActive(true);
			}
		}
		else
		{
			RequstNextEvent();
		}
	}

	private void RequstNextEvent()
	{
		AdventureSceneData.Instance.GetAdventureTypeByEventId(marvellousProto);
		OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator EnterFixedReward(float timer)
	{
		SetFixedRewardData();
		if (rewardCount > 0)
		{
			InitFixedReward();
			yield return new WaitForSeconds(openBoxTimer);
			openedEffectObj.SetActive(true);
		}
	}

	/// <summary>
	/// 初始化宝箱显示及动画的播放
	/// </summary>
	private void InitDelayReward()
	{
		GameObject delayObject = Instantiate(UIFX_Q_YanShiJianLi_FontFX) as GameObject;
		delayObject.transform.parent = delayRewardObject.transform;
		delayObject.transform.localPosition = Vector3.zero;
		delayAni = delayObject.GetComponent<Animation>();
		delayAni.Play();
		playerIcon.SetData(ItemInfoUtility.GetAvatarFirstIconID());
		ResetUIAndData(true);
	}

	/// <summary>
	/// 设置延时奖励数据
	/// </summary>
	private void SetDelayRewardData()
	{
		if (delayRewards == null || delayRewards.Count == 0) return;
		MarvellousAdventureConfig.RewardEvent rewardEvent = ConfigDatabase.DefaultCfg.MarvellousAdventureConfig.GetEventById(delayRewards[0].eventId) as MarvellousAdventureConfig.RewardEvent;
		delayRewardIcon.SetData(rewardEvent.OriginalIconId);
	}

	/// <summary>
	/// 初始化宝箱显示及动画的播放
	/// </summary>
	private void InitFixedReward()
	{
		ResetUIAndData(false);
		closeBtn.Hide(false);
		openedEffectObj.SetActive(false);

		openAnim.Play();
		//shinningAnim.Play();
	}

	/// <summary>
	/// 设置宝箱数据
	/// </summary>
	private void SetFixedRewardData()
	{
		if ((fixRewardPackagePars != null && fixRewardPackagePars.Count > 0) || (randRewardPackagePars != null && randRewardPackagePars.Count > 0))
		{
			int index = 0;
			foreach (Pair<int, int> fixPair in fixRewardPackagePars) //UIDlgAdventureGetReward_Label_RewardType_Congratulations
			{
				UIListItemContainer itemContainer = rewardPool.AllocateItem().GetComponent<UIListItemContainer>();
				var item = itemContainer.gameObject.GetComponent<UIElemAdventureRewardItem>();
				item.SetData(index, fixPair.first, fixPair.second, GameUtility.GetUIString("UIDlgAdventureGetReward_Label_RewardType_Congratulations"));
				itemContainer.Data = item;
				rewardList.AddItem(itemContainer.gameObject);
				rewardCount++;
				index++;
			}

			foreach (Pair<int, int> randPair in randRewardPackagePars)//UIDlgAdventureGetReward_Label_RewardType_Additional
			{
				UIListItemContainer itemContainer = rewardPool.AllocateItem().GetComponent<UIListItemContainer>();
				var item = itemContainer.gameObject.GetComponent<UIElemAdventureRewardItem>();
				item.SetData(index, randPair.first, randPair.second, GameUtility.GetUIString("UIDlgAdventureGetReward_Label_RewardType_Additional"));
				itemContainer.Data = item;
				rewardList.AddItem(itemContainer.gameObject);
				rewardCount++;
				index++;
			}

			rewardList.ScrollToItem(0, 0);
		}
	}

	//显示延时奖励或者宝箱奖励中的一种
	private void ResetUIAndData(bool isShow)
	{
		delayRewardObject.SetActive(isShow);
		fixedRewardObject.SetActive(!isShow);
	}

	public override void OnHide()
	{
		//SysUIEnv.Instance.CheckUIIsHide<UIPnlAdventureMessage>();
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOKClickButton(UIButton btn)
	{
		foreach (Pair<int, int> pair in fixRewardPackagePars)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UIDlgAdventureGetReward_Label_Reward", pair.second, ItemInfoUtility.GetAssetName(pair.first)), 0f, 0.5f);
		foreach (Pair<int, int> pair in randRewardPackagePars)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UIDlgAdventureGetReward_Label_Reward", pair.second, ItemInfoUtility.GetAssetName(pair.first)), 0f, 0.5f);
		RequstNextEvent();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlAdventureMain));
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		UIElemAssetIcon asset = btn.Data as UIElemAssetIcon;
		if (asset != null)
			GameUtility.ShowAssetInfoUI((int)asset.Data, _UILayer.Top);
	}


}
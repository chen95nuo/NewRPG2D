using UnityEngine;
using System.Collections;
using ClientServerCommon;
using System.Collections.Generic;
using KodGames;

public class UIEffectLottery : UIModule
{
	public GameObject lotteryEffectObj;
	public Animation lotteryEffectAnim;
	public float lotteryEffectDuration;

	public GameObject lotterySuccessObj;
	public UIBtnCamera avatar3D;
	public UIElemAvatarCard avatarImage;
	public UIBox avatarCountry;
	public UIBox avatarProperty;
	public GameObject normalObj;
	public GameObject tenTimesObj;
	public float avatarMdlShowDelay;

	public UIElemTenRewardItem rewardList;
	public Transform chengCardEffectMarker;
	public UIScrollList rewardObjectList;
	public GameObjectPool rewardPool;

	private KodGames.ClientClass.Reward fixReward;
	private KodGames.ClientClass.Reward randomReward;
	private GameObject particle = null;
	private int rawardListIndex = 0;
	private string soundName;
	private List<GameObject> chengParticles = new List<GameObject>();

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		HideTenReward();

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		fixReward = userDatas[0] as KodGames.ClientClass.Reward;
		randomReward = userDatas[1] as KodGames.ClientClass.Reward;

		if (fixReward == null || fixReward.Avatar == null || fixReward.Avatar.Count == 0)
		{
			Debug.LogError("no avatar in the reward.");
			HideSelf();
		}

		if (fixReward.Avatar.Count == 1)
			InitView();
		else
			InitTenView();

		avatarImage.GetComponent<UIButton>().SetControlState(UIButton.CONTROL_STATE.DISABLED);

		return true;
	}

	public override void OnHide()
	{
		StopCoroutine("SetLotterySuccessEffect");

		ClearData();

		base.OnHide();
	}

	private void ClearData()
	{
		StopCoroutine("FillRewardList");
		rewardObjectList.ClearList(false);
		rewardObjectList.ScrollListTo(0f);

		StopCoroutine("SetLotterySuccessEffect");
		StopCoroutine("FillList");

		avatarImage.Clear();

		avatar3D.Data = null;
	}

	private void InitView()
	{
		int resourceId = fixReward.Avatar[0].ResourceId;
		lotteryEffectAnim.GetComponent<AnimationEventHandler>().userEventDelegate = (data1, data2) =>
			{
				switch (data1)
				{
					case "PlayerEffect"://开门发光
						string fxPath = PathUtility.Combine(GameDefines.pfxPath, GameDefines.uiFX_XR);
						FXController fxController = SysFx.Instance.CreateFx(fxPath);
						fxController.Start();
						break;
					case "ShakeCamera"://震屏
						KodGames.Effect.CameraShaker.Shake(avatar3D.gameObject, 2f, 1f, 0.01f);
						break;
				}
			};

		avatarImage.GetComponent<AnimationEventHandler>().userEventDelegate = (data1, data2) =>
			{
				switch (data1)
				{
					case "PlayerEffect"://发光特效

						// 随机奖励在动画期间不可以点击
						SetRandomRewardState(false);

						particle = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.uiFx_Lottery));
						if (particle != null)
							ObjectUtility.AttachToParentAndResetLocalTrans(avatarImage.gameObject, particle);

						break;
					case "SpecialCardEffect"://紫卡和橙色卡的特效

						switch (ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(resourceId).qualityLevel)
						{
							case 4:
								particle = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFX_Q_CardLottery_S4ZiSe));
								break;
							case 5:
								particle = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFX_Q_CardLottery_S5ChengSe));
								break;
						}
						if (particle != null)
							ObjectUtility.AttachToParentAndResetLocalTrans(avatarImage.gameObject, particle);

						avatarImage.GetComponent<UIButton>().SetControlState(UIButton.CONTROL_STATE.NORMAL);

						break;

					case "PlaySound":

						// 随机奖励恢复点击
						SetRandomRewardState(true);

						List<string> voiceList = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(resourceId).voices;
						if (voiceList.Count <= 0)
							return;

						soundName = voiceList[Random.Range(0, voiceList.Count)];

						if (!string.IsNullOrEmpty(soundName))
							AudioManager.Instance.PlayStreamSound(soundName, 0f);

						break;
				}
			};

		StartCoroutine("FillRewardList");
		StartLotteryEffect(resourceId);
	}

	private void InitTenView()
	{
		if (fixReward.Avatar.Count <= 0)
			return;

		lotteryEffectObj.SetActive(false);
		lotterySuccessObj.SetActive(true);

		normalObj.SetActive(false);
		tenTimesObj.SetActive(true);

		ClearData();
		StartCoroutine("FillRewardList");
		StartCoroutine("FillList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList()
	{
		yield return null;

		var rewards = SysLocalDataBase.CCRewardToCSCReward(randomReward);
		foreach (var reward in rewards)
		{
			var itemContainer = rewardPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = itemContainer.GetComponent<UIElemEffectLotteryReward>();

			itemContainer.Data = item;
			item.SetData(reward);
			rewardObjectList.AddItem(itemContainer);
		}
		rewardObjectList.ScrollToItem(0, 0);
	}

	private void StartLotteryEffect(int resourceId)
	{
		lotteryEffectObj.SetActive(true);
		lotterySuccessObj.SetActive(false);
		lotteryEffectAnim.Play();

		StartCoroutine("SetLotterySuccessEffect", resourceId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator SetLotterySuccessEffect(int avatarId)
	{
		yield return new WaitForSeconds(lotteryEffectDuration);
		lotteryEffectObj.SetActive(false);
		lotterySuccessObj.SetActive(true);

		normalObj.SetActive(true);
		tenTimesObj.SetActive(false);

		// Play Animation.
		avatarImage.cardUI.CachedAnimation.Play();

		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarId);
		avatarImage.SetData(avatarId, false, false, null);

		//设置属性
		UIElemTemplate.Inst.SetAvatarTraitIcon(avatarProperty, avatarCfg.traitType);

		//设置国家
		UIElemTemplate.Inst.SetAvatarCountryIcon(avatarCountry, avatarCfg.countryType);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		for (int i = 0; i < Mathf.Min(fixReward.Avatar.Count, rewardList.rewards.Count); i++)
		{
			rewardList.rewards[i].Hide(true);
			rewardList.rewards[i].SetData(fixReward.Avatar[i]);
		}

		yield return null;

		rawardListIndex = 0;
		PlayNextAnimation();
	}

	public void PlayNextAnimation()
	{
		if (rawardListIndex < rewardList.rewards.Count)
		{
			if (rewardList.rewards[rawardListIndex].QualityLevel == 5)//橙卡
			{
				// 随机奖励在动画期间不可以点击
				SetRandomRewardState(false);

				GameObject chengCardEffectObj = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFX_Q_CardLottery_10timeCheng));
				chengParticles.Add(chengCardEffectObj.gameObject);

				ObjectUtility.AttachToParentAndResetLocalTrans(chengCardEffectMarker.gameObject, chengCardEffectObj);

				UIElemTenTimeCardLotteryChengCard tenTimeCardLotteryChengCard = chengCardEffectObj.GetComponent<UIElemTenTimeCardLotteryChengCard>();

				tenTimeCardLotteryChengCard.SetData(rewardList.rewards[rawardListIndex].AvatarData.ResourceId);

				tenTimeCardLotteryChengCard.animEvtHandler.userEventDelegate = (eventName, data) =>
				{
					switch (eventName)
					{
						case "ShrinkStart":
							float duration = GetAnimationLeftTime(tenTimeCardLotteryChengCard.animEvtHandler);

							AnimatePosition.Do(tenTimeCardLotteryChengCard.gameObject, EZAnimation.ANIM_MODE.FromTo,
							CachedTransform.InverseTransformPoint(tenTimeCardLotteryChengCard.transform.position),
							CachedTransform.InverseTransformPoint(rewardList.rewards[rawardListIndex].transform.position),
							EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Default),
							duration,
							0,
							null,
							(ezAnim) =>
							{
								Destroy(chengCardEffectObj);
								rewardList.rewards[rawardListIndex].PlayAnimation(rawardListIndex, rewardList.rewards.Count);
								rawardListIndex++;
							}
							);
							break;

						case "UIFX_Q_CardLottery_S5ChengSe":
							GameObject newEffect = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFX_Q_CardLottery_S5ChengSe));
							chengParticles.Add(newEffect);

							ObjectUtility.AttachToParentAndResetLocalTrans(chengCardEffectMarker.gameObject, newEffect);
							Vector3 localPosi = newEffect.transform.localPosition;
							localPosi.z -= 0.01f;
							newEffect.transform.localPosition = localPosi;
							break;
					}
				};

				return;
			}

			rewardList.rewards[rawardListIndex].PlayAnimation(rawardListIndex, rewardList.rewards.Count);
			rawardListIndex++;
		}
	}

	public bool NextToPlayIsChengCard(int listIndex)
	{
		if (listIndex < rewardList.rewards.Count - 1)
			return rewardList.rewards[listIndex + 1].QualityLevel == 5;

		return false;
	}

	//激活按钮
	public void ActiveRewardButton()
	{
		for (int i = 0; i < rewardList.rewards.Count; i++)
			rewardList.rewards[i].SetBtnControllerState(UIButton.CONTROL_STATE.NORMAL);
	}

	private float GetAnimationLeftTime(AnimationEventHandler handler)
	{
		foreach (AnimationState animState in handler.CachedAnimation)
		{
			if (handler.CachedAnimation.IsPlaying(animState.name))
			{
				return animState.length * (Mathf.Max(0, 1 - animState.normalizedTime));
			}
		}

		Debug.LogError("Can't Find PlayingAnimation in " + handler.gameObject.name);
		return 0;
	}

	public void SetRandomRewardState(bool enable)
	{
		for (int i = 0; i < rewardObjectList.Count; i++)
		{
			var randomRewardItem = rewardObjectList.GetItem(i).Data as UIElemEffectLotteryReward;
			randomRewardItem.EnableButton(enable);
		}
	}

	private void DestoryRewardListParticle()
	{
		int i = 0;
		while (i < rewardList.rewards.Count)
		{
			rewardList.rewards[i].DestroyParticle();
			i++;
		}

		for (i = 0; i < chengParticles.Count; i++)
			Destroy(chengParticles[i]);

		chengParticles.Clear();
	}

	private void StopVoice()
	{
		if (!string.IsNullOrEmpty(soundName))
			AudioManager.Instance.StopSound(soundName);
	}

	private void HideTenReward()
	{
		for (int i = 0; i < rewardList.rewards.Count; i++)
			rewardList.rewards[i].Hide(true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOK(UIButton btn)
	{
		if (particle != null)
			GameObject.Destroy(particle);

		DestoryRewardListParticle();
		HideTenReward();
		StopVoice();
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatarIcon(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIDlgAvatarInfo, fixReward.Avatar[0], false, true, false, false, null);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTenAvatarIcon(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIDlgAvatarInfo, btn.Data as KodGames.ClientClass.Avatar, false, true, false, false, null);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRandomRewardIcon(UIButton btn)
	{
		GameUtility.ShowAssetInfoUI((btn.Data as UIElemAssetIcon).Data as ClientServerCommon.Reward);
	}
}

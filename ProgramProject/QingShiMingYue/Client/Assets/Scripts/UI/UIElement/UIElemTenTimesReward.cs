using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemTenTimesReward : MonoBehaviour
{
	public UIElemAssetIcon assetIcon;
	public UIBox avatarTraitBox;//阴影
	public UIButton btn;

	private int qualityLevel;
	public int QualityLevel
	{
		get
		{
			return qualityLevel;
		}
	}

	KodGames.ClientClass.Avatar avatarData;
	public KodGames.ClientClass.Avatar AvatarData
	{
		get
		{
			return avatarData;
		}
	}


	private List<GameObject> particleList = new List<GameObject>();//用list存储特效，清理的时候就不会漏掉某一个

	public void SetData(KodGames.ClientClass.Avatar avatar)
	{
		//记录品质
		qualityLevel = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).qualityLevel;

		// Set Assistant Data.
		this.avatarData = avatar;
		assetIcon.border.Data = avatar;
		assetIcon.SetData(avatar);
		assetIcon.assetNameLabel.Text = ItemInfoUtility.GetAssetName(avatar.ResourceId);
		assetIcon.Data = avatar;
		AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);

		UIElemTemplate.Inst.SetAvatarTraitIcon(avatarTraitBox, avatarConfig.traitType);
	}

	public void Hide(bool isHide)
	{
		assetIcon.Hide(isHide);
		avatarTraitBox.Hide(isHide);
	}

	public void DestroyParticle()
	{
		foreach (GameObject particle in particleList)
		{
			GameObject.Destroy(particle);
		}

		particleList.Clear();
	}

	public void PlayAnimation(int index, int totalRewardNum)
	{
		//控制button不可用
		SetBtnControllerState(UIButton.CONTROL_STATE.DISABLED);

		assetIcon.GetComponent<AnimationEventHandler>().userEventDelegate = (data1, data2) =>
			{
				switch (data1)
				{
					case "ShowSelf":
						// 随机奖励在动画期间不可以点击
						SysUIEnv.Instance.GetUIModule<UIEffectLottery>().SetRandomRewardState(false);
						Hide(false);
						break;
					case "UIFX_Q_IconLottery_S4ZiSe"://紫卡抽卡特效
						particleList.Add(ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFX_Q_IconLottery_S4ZiSe)));
						ObjectUtility.AttachToParentAndResetLocalTrans(assetIcon.gameObject, particleList[particleList.Count - 1]);
						break;
					case "UIFX_Q_IconLottery_S5ChengSe"://橙卡抽卡特效
						particleList.Add(ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFX_Q_IconLottery_S5ChengSe)));
						ObjectUtility.AttachToParentAndResetLocalTrans(assetIcon.gameObject, particleList[particleList.Count - 1]);
						break;
					case "UIFX_Q_IconShinning_S4ZiSe"://紫卡已抽出特效
						particleList.Add(ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFX_Q_IconShinning_S4ZiSe)));
						ObjectUtility.AttachToParentAndResetLocalTrans(assetIcon.gameObject, particleList[particleList.Count - 1]);
						break;
					case "UIFX_Q_IconShinning_S5ChengSe"://橙卡已抽出特效
						particleList.Add(ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.UIFX_Q_IconShinning_S5ChengSe)));
						ObjectUtility.AttachToParentAndResetLocalTrans(assetIcon.gameObject, particleList[particleList.Count - 1]);
						break;
					case "PlayNextAnimation":
						if (!SysUIEnv.Instance.GetUIModule<UIEffectLottery>().NextToPlayIsChengCard(index))
							SysUIEnv.Instance.GetUIModule<UIEffectLottery>().PlayNextAnimation();
						break;
					case "PlayNextChengAnimation":
						if (SysUIEnv.Instance.GetUIModule<UIEffectLottery>().NextToPlayIsChengCard(index))
							SysUIEnv.Instance.GetUIModule<UIEffectLottery>().PlayNextAnimation();
						break;
					case "ActionRewardButton":
						if (index >= totalRewardNum - 1)
						{
							SysUIEnv.Instance.GetUIModule<UIEffectLottery>().ActiveRewardButton();
							// 随机奖励恢复点击
							SysUIEnv.Instance.GetUIModule<UIEffectLottery>().SetRandomRewardState(true);
						}
						break;
				}
			};

		switch (qualityLevel)
		{
			case 2:
			case 3:
				assetIcon.CachedAnimation.Play("UI_LotteryIconAni_S3LanSe", PlayMode.StopAll);
				break;
			case 4:
				assetIcon.CachedAnimation.Play("UI_LotteryIconAni_S4ZiSe", PlayMode.StopAll);
				break;
			case 5:
				assetIcon.CachedAnimation.Play("UI_LotteryIconAni_S5ChengSe", PlayMode.StopAll);
				break;
		}
	}

	public void SetBtnControllerState(UIButton.CONTROL_STATE state)
	{
		btn.SetControlState(state);
	}
}
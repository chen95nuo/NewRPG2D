using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;
using KodGames;
using com.kodgames.corgi.protocol;

public class UIEffectSchoolOpenBox : UIModule
{
	public UIBox closeBtn;
	public GameObject closeEffectObj;
	public Animation openAnim;
	public Animation shinningAnim;
	public Animation closeAnim;
	public float closeEffectDuring;

	public GameObject openedEffectObj;
	public UIChildLayoutControl childLayout;

	public UIScrollList rewardItemList;
	public GameObjectPool itemPool;

	public UIButton presentedBtn;
	public UIButton getBtn;
	public UIButton backBtn;

	public SpriteText contextLabel;
	private KodGames.ClientClass.StageInfo stageInfo;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		
		if (userDatas != null && userDatas.Length > 0)
		{
			
			stageInfo = userDatas[0] as KodGames.ClientClass.StageInfo;
			SetBtnAction((bool)userDatas[1], (bool)userDatas[2], (bool)userDatas[3]);
			SetUICtrls((bool)userDatas[4]);
		}


		return true;
	}

	private void SetBtnAction(bool isPresented, bool isGet, bool isBack)
	{
		if (ConfigDatabase.DefaultCfg.GuildStageConfig.BaseInfos.BoxSendOpen)
			presentedBtn.Hide(!isPresented);
		else
			presentedBtn.Hide(true);
		
		getBtn.Hide(!isGet);
		backBtn.Hide(!isBack);
		if (isBack)
			contextLabel.Text = GameUtility.FormatUIString("UIEffectSchoolOpenBox_ContextLabel", stageInfo.Name);
		else
			contextLabel.Text = string.Empty;
	}

	public override void Overlay()
	{
		base.Overlay();

		shinningAnim.enabled = false;
	}

	public override void OnHide()
	{
		base.OnHide();
		rewardItemList.ClearList(false);
		shinningAnim.enabled = true;
	}

	private void CloseEffect(bool isClose)
	{
		closeAnim.enabled = !isClose;
		openAnim.enabled = !isClose;
		closeEffectObj.SetActive(!isClose);
		openedEffectObj.SetActive(isClose);
	}

	private void SetUICtrls(bool isPlayEffect)
	{
		CloseEffect(isPlayEffect);
		closeBtn.Hide(false);
		if (!isPlayEffect)
		{
			closeAnim.Play();
			shinningAnim.Play();
			StartCoroutine("BeginOpenEffect", closeEffectDuring);
		}
		else
		{
			StartCoroutine("BeginOpenEffect", 0);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator BeginOpenEffect(float timer)
	{
		yield return new WaitForSeconds(timer);
		closeEffectObj.SetActive(false);
		openedEffectObj.SetActive(true);
		rewardItemList.ClearList(false);
		List<ShowReward> showRewards = stageInfo.ShowRewards;
		for (int i = 0; i < showRewards.Count; i++)
		{
			UIListItemContainer container = itemPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemEffectSchoolOpenBox item = container.gameObject.GetComponent<UIElemEffectSchoolOpenBox>();
			item.SetData(showRewards[i]);
			container.Data = item;
			rewardItemList.AddItem(container);
		}

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnPresentedClick(UIButton btn)
	{
		RequestMgr.Inst.Request(new GuildViewSimpleReq(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildId, (guildInfoSimple) =>
		{
			SysUIEnv.Instance.ShowUIModule<UIPnlGuildPointGiveAway>(guildInfoSimple, stageInfo);
			HideSelf();
			return true;
		}));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGetClick(UIButton btn)
	{

		RequestMgr.Inst.Request(new GuildStageGiveBoxReq(SysLocalDataBase.Inst.LocalPlayer.PlayerId, () =>
		{
			string tipMsg = string.Empty;
			List<ShowReward> showRewards = stageInfo.ShowRewards;
			foreach (ShowReward showReward in showRewards)
				tipMsg += string.Format("{0}x{1},", ItemInfoUtility.GetAssetName(showReward.id), showReward.count);
			if (tipMsg != null && tipMsg.Length > 0)
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIEffectSchoolOpenBox_GetReward", GameDefines.textColorTipsInBlack, tipMsg.Substring(0, tipMsg.Length - 1)));
			RequestMgr.Inst.Request(new QueryGuildStageReq(GuildStageConfig._LayerType.Now));
			HideSelf();
			return true;
		}));

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		HideSelf();
	}


	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId, _UILayer.Top);
	}
}

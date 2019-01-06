using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIEffectOpenBox : UIModule
{
	public UIBox closeBtn;
	public GameObject closeEffectObj;
	public Animation openAnim;
	public Animation shinningAnim;
	public float closeEffectDuring;

	public GameObject openedEffectObj;
	public UIElemAssetIcon rewardIcon;
	public UIElemAssetIcon randomRewardIcon;
	public SpriteText rewardLabel;
	public SpriteText randomLable;
	public UIChildLayoutControl childLayout;

	private int consumableCount;
	private KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		ClearData();

		this.consumableCount = (int)userDatas[0];
		this.costAndRewardAndSync = userDatas[1] as KodGames.ClientClass.CostAndRewardAndSync;

		SetUICtrls();

		return true;
	}

	public override void Overlay()
	{
		base.Overlay();

		shinningAnim.enabled = false;
	}

	public override void OnHide()
	{
		base.OnHide();

		shinningAnim.enabled = true;
	}

	private void ClearData()
	{
		consumableCount = 0;
		costAndRewardAndSync = null;
	}

	private void SetUICtrls()
	{
		closeBtn.Hide(false);
		openedEffectObj.SetActive(false);

		openAnim.Play();
		shinningAnim.Play();
		StartCoroutine("BeginOpenEffect");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator BeginOpenEffect()
	{
		yield return new WaitForSeconds(closeEffectDuring);

		// Hide the child first.
		childLayout.HideChildObj(randomLable.gameObject, true);
		childLayout.HideChildObj(rewardLabel.gameObject, true);

		List<ClientServerCommon.Reward> viewFixReward = SysLocalDataBase.CCRewardToCSCReward(costAndRewardAndSync.ViewFixReward);
		List<ClientServerCommon.Reward> viewRandomReward = SysLocalDataBase.CCRewardToCSCReward(costAndRewardAndSync.ViewRandomReward);

		if (consumableCount > 1)
		{
			UIDlgShopGiftPreview.ShowData showData = new UIDlgShopGiftPreview.ShowData();
			showData.title = GameUtility.GetUIString("UIEffectOpenBox_MultiOpenTitle");

			if (viewFixReward.Count > 0)
				showData.rewardDatas.Add(new UIDlgShopGiftPreview.RewardData(GameUtility.GetUIString("UIEffectOpenBox_FixReward"), viewFixReward));
			if (viewRandomReward.Count > 0)
				showData.rewardDatas.Add(new UIDlgShopGiftPreview.RewardData(GameUtility.GetUIString("UIEffectOpenBox_RandomReward"), viewRandomReward));

			SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgShopGiftPreview, showData);
			HideSelf();
		}
		else
		{
			closeBtn.Hide(true);
			openedEffectObj.SetActive(true);

			// Init View Data.
			rewardIcon.Hide(true);

			if (viewFixReward.Count > 0)
			{
				int fixRewardAssetID = viewFixReward[0].id;
				int fixRewardAssetCount = 1;

				if (IDSeg.ToAssetType(fixRewardAssetID) == IDSeg._AssetType.Item ||
				   IDSeg.ToAssetType(fixRewardAssetID) == IDSeg._AssetType.Special)
					fixRewardAssetCount = viewFixReward[0].count;

				if (IDSeg.ToAssetType(fixRewardAssetID) == IDSeg._AssetType.Avatar
					|| IDSeg.ToAssetType(fixRewardAssetID) == IDSeg._AssetType.Equipment
					|| IDSeg.ToAssetType(fixRewardAssetID) == IDSeg._AssetType.CombatTurn)
				{
					fixRewardAssetCount = 0;
					for (int index = 0; index < viewFixReward.Count; index++)
					{
						if (viewFixReward[index].id == fixRewardAssetID)
							fixRewardAssetCount++;
					}
				}

				if (fixRewardAssetID == IDSeg.InvalidId)
				{
					Debug.Log("FixReward Id is Invalid.");
					yield break;
				}

				rewardIcon.SetData(fixRewardAssetID, fixRewardAssetCount);
				rewardIcon.Hide(false);

				childLayout.HideChildObj(rewardLabel.gameObject, false);
			}

			if (viewRandomReward.Count > 0)
			{
				int randomRewardAssetID = viewRandomReward[0].id;
				int randomRewardAssetCount = 1;

				if (IDSeg.ToAssetType(randomRewardAssetID) == IDSeg._AssetType.Item ||
				   IDSeg.ToAssetType(randomRewardAssetID) == IDSeg._AssetType.Special)
					randomRewardAssetCount = viewRandomReward[0].count;

				if (IDSeg.ToAssetType(randomRewardAssetID) == IDSeg._AssetType.Avatar
					|| IDSeg.ToAssetType(randomRewardAssetID) == IDSeg._AssetType.Equipment
					|| IDSeg.ToAssetType(randomRewardAssetID) == IDSeg._AssetType.CombatTurn)
				{
					randomRewardAssetCount = 0;
					for (int index = 0; index < viewRandomReward.Count; index++)
					{
						if (viewRandomReward[index].id == randomRewardAssetID)
							randomRewardAssetCount++;
					}
				}

				if (randomRewardAssetID == IDSeg.InvalidId)
				{
					Debug.Log("RandomReard Id is InValid.");
					yield break;
				}

				randomRewardIcon.SetData(randomRewardAssetID, randomRewardAssetCount);
				childLayout.HideChildObj(randomLable.gameObject, false);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
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

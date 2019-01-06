using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildPointMonsterView : UIModule
{
	public UIScrollList rewardList;
	public GameObjectPool rewardPool;
	public SpriteText tipsLabel;

	private List<com.kodgames.corgi.protocol.ShowReward> showRewards;
	private KodGames.ClientClass.StageInfo stageInfo;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas)) return false;

		showRewards = userDatas[0] as List<com.kodgames.corgi.protocol.ShowReward>;
		stageInfo = userDatas[1] as KodGames.ClientClass.StageInfo;
		tipsLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointMonsterBattleResult_ViewTips", stageInfo.Player.Name, stageInfo.Name);

		ClearData();
		StartCoroutine("FillRewardList");

		return true;
	}

	private void ClearData()
	{
		StopCoroutine("FillRewardList");
		rewardList.ClearList(false);
		rewardList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList()
	{
		yield return null;

		foreach (var reward in showRewards)
		{
			UIElemDanExtraReward item = rewardPool.AllocateItem().GetComponent<UIElemDanExtraReward>();
			item.SetData(reward);

			rewardList.AddItem(item.gameObject);
		}

		rewardList.ScrollToItem(0, 0);		
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var showItem = assetIcon.Data as com.kodgames.corgi.protocol.ShowReward;
		GameUtility.ShowAssetInfoUI(showItem, _UILayer.Top);
	}
}

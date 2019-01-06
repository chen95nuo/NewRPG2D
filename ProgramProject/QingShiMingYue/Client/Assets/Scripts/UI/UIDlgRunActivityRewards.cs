using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgRunActivityRewards : UIModule
{
	public List<UIElemAssetIcon> icons;
	public List<SpriteText> names;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (false == base.OnShow(layer, userDatas))
			return false;

		SetData(userDatas[0] as KodGames.ClientClass.Reward);

		return true;
	}

	private void SetData(KodGames.ClientClass.Reward reward)
	{
		List<KodGames.Pair<int, int>> rewards = SysLocalDataBase.ConvertIdCountList(reward);

		for (int i = 0; i < icons.Count; i++)
		{
			icons[i].Hide(true);
			names[i].Text = string.Empty;
		}

		if (rewards != null && rewards.Count >= 1)
		{
			for (int index = 0; index < Mathf.Min(rewards.Count, icons.Count); index++)
			{
				icons[index].Hide(false);
				icons[index].SetData(rewards[index].first, rewards[index].second);
				names[index].Text = ItemInfoUtility.GetAssetName(rewards[index].first);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
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

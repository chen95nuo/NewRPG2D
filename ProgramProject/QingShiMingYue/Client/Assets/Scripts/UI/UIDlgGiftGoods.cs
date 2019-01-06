using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgGiftGoods : UIModule
{
	public UIScrollList rewardLists;
	public GameObjectPool rewardPool;

	public SpriteText titleLabel;

	private const int C_COLUMN_COUNT = 3;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (false == base.OnShow(layer, userDatas))
			return false;

		StartCoroutine("FillList", userDatas[0] as KodGames.ClientClass.Reward);
		titleLabel.Text = userDatas[1] as string;

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		rewardLists.ClearList(false);
		rewardLists.ScrollListTo(0);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(KodGames.ClientClass.Reward clcRewards)
	{
		yield return null;

		List<ClientServerCommon.Reward> rewards = SysLocalDataBase.CCRewardToCSCReward(clcRewards);

		int row = rewards.Count % C_COLUMN_COUNT == 0 ? rewards.Count / C_COLUMN_COUNT - 1 : rewards.Count / C_COLUMN_COUNT;
		List<Reward> reward = null;

		for (int index = 0; index <= row; index++)
		{
			if (index < row)
				reward = rewards.GetRange(index * C_COLUMN_COUNT, C_COLUMN_COUNT);
			else
				reward = rewards.GetRange(index * C_COLUMN_COUNT, Math.Min(C_COLUMN_COUNT, rewards.Count - index * C_COLUMN_COUNT));

			UIElemGiftItemRoot item = rewardPool.AllocateItem(false).GetComponent<UIElemGiftItemRoot>();
			item.SetData(reward);
			rewardLists.AddItem(item);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGiftIcon(UIButton btn)
	{
		UIElemAssetIcon item = btn.Data as UIElemAssetIcon;
		Reward reward = item.Data as Reward;
		GameUtility.ShowAssetInfoUI(reward, _UILayer.Top);
	}
}

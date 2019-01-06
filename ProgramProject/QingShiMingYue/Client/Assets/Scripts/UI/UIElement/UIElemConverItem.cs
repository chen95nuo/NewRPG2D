using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemConverItem : MonoBehaviour
{
	public List<UIElemAssetIcon> itemIcons;
	public List<SpriteText> itemNames;

	public void SetData(List<KodGames.Pair<int, int>> rewards)
	{
		//开始，全部隐藏
		for (int index = 0; index < itemIcons.Count; index++)
		{
			itemIcons[index].Hide(true);
			itemNames[index].Text = string.Empty;
			itemNames[index].Hide(true);
		}

		for (int index = 0; index < Math.Min(itemIcons.Count, rewards.Count); index++)
		{
			itemIcons[index].Hide(false);
			itemIcons[index].SetData(rewards[index].first, rewards[index].second);

			itemNames[index].Hide(false);
			itemNames[index].Text = ItemInfoUtility.GetAssetName(rewards[index].first);
		}
	}
}

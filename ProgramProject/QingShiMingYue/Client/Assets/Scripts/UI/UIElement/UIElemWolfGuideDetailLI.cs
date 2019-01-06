using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemWolfGuideDetailLI : MonoBehaviour
{
	public UIBox yiwancheng;
	public List<UIElemAssetIcon> assetIcons;
	public List<SpriteText> assetIconNames;

	public void SetData(List<Reward> rewards, bool pRet)
	{
		yiwancheng.Hide(!pRet);

		for (int index = 0; index < Math.Min(assetIcons.Count, assetIconNames.Count); index++)
		{
			assetIconNames[index].Hide(true);
			assetIcons[index].Hide(true);
		}

		for (int index = 0; index < Math.Min(Math.Min(rewards.Count, assetIcons.Count), assetIconNames.Count); index++)
		{
			assetIcons[index].Hide(false);
			assetIconNames[index].Hide(false);

			assetIconNames[index].Text = ItemInfoUtility.GetAssetName(rewards[index].id);
			assetIcons[index].SetData(rewards[index].id, rewards[index].count);

		}
	}
}

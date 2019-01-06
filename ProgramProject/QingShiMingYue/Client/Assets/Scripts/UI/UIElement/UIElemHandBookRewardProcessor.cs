using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemHandBookRewardProcessor : UIListItemContainerEx
{
	private UIElemHandBookRewardItem uiItem;
	private List<IllustrationConfig.Illustration> illustrationCfgs;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null && uiItem == null)
			{
				uiItem = SubItem.GetComponent<UIElemHandBookRewardItem>();

				if (illustrationCfgs != null)
					SetData(this.illustrationCfgs);
			}
		}
	}

	public override void OnDisabled()
	{
		base.OnDisabled();

		if (Application.isPlaying)
			uiItem = null;
	}

	public void ClearData()
	{
		if (uiItem != null)
			for (int i = 0; i < uiItem.rewards.Length; i++)
				uiItem.rewards[i].ClearData();

		base.OnDisabled();
		uiItem = null;
		illustrationCfgs = null;
	}

	public void SetData(List<IllustrationConfig.Illustration> illustrations)
	{
		this.illustrationCfgs = illustrations;

		if (uiItem != null && illustrationCfgs != null)
		{
			for (int i = 0; i < uiItem.rewards.Length; i++)
				uiItem.rewards[i].Hide(true);

			for (int i = 0; i < illustrations.Count; i++)
				uiItem.rewards[i].SetData(illustrations[i]);
		}
	}

	public void ItemRefresh(int illustrationId)
	{
		if (uiItem == null || illustrationCfgs == null)
			return;

		for (int i = 0; i < Mathf.Min(uiItem.rewards.Length, illustrationCfgs.Count); i++)
		{
			if (illustrationCfgs[i] == null || illustrationCfgs[i].Id != illustrationId)
				continue;

			uiItem.rewards[i].RefreshState();
			break;
		}
	}
}
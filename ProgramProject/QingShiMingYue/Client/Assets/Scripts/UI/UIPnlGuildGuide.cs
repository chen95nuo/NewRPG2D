using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIPnlGuildGuide : UIModule
{
	public UIScrollList guideList;
	public GameObjectPool guidePool;
	public UIButton backBtn;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		StartCoroutine("FillGuideList");

		return true;
	}

	public override void OnHide()
	{
		StopCoroutine("FillGuideList");
		guideList.ClearList(false);
		guideList.ScrollListTo(0);
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillGuideList()
	{
		yield return null;

		for (int index = 0; index < ConfigDatabase.DefaultCfg.GuildConfig.MainTypes.Count; index++)
		{
			UIListItemContainer item = guidePool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemGuildGuideItem guide = item.gameObject.GetComponent<UIElemGuildGuideItem>();
			guide.SetData(ConfigDatabase.DefaultCfg.GuildConfig.MainTypes[index]);
			guideList.AddItem(item);
		}
	}

	#region OnClick

	//点击关闭
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClickBack(UIButton btn)
	{
		if (ItemInfoUtility.IsInMainScene())
		{
			if (SysGameStateMachine.Instance.CurrentState is GameState_GuildPoint)
				SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildPointMain);
			else
				SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildTab);
		}
		else
		{
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildPointMain);
			HideSelf();
		}
	}

	//点击标签
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGuideItemClick(UIButton btn)
	{
		ClientServerCommon.MainType guideItem = btn.Data as ClientServerCommon.MainType;
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildGuideDetail, guideItem);
	}

	#endregion
}

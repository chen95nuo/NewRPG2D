using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlIllusion : UIModule
{
	public UIScrollList cardList;
	public GameObjectPool avatarObjectPool;
	public UIButton closeBtn;

	private List<com.kodgames.corgi.protocol.IllusionAvatar> illusionAvatars = new List<com.kodgames.corgi.protocol.IllusionAvatar>();

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		RequestMgr.Inst.Request(new QueryIllusionReq(OnQueryIllusionRes));

		return true;
	}

	private void OnQueryIllusionRes(bool success)
	{
		SetData(SysLocalDataBase.Inst.LocalPlayer.IllusionData);
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	public override void Overlay()
	{
		base.Overlay();
		ClearData();
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();
		//SetData(SysLocalDataBase.Inst.LocalPlayer.IllusionData);
		//重新刷新界面
		RequestMgr.Inst.Request(new QueryIllusionReq(OnQueryIllusionRes));
	}

	float scrollPosi = 0;
	private void ClearData()
	{
		scrollPosi = cardList.ScrollPosition;
		cardList.ClearList(false);
		cardList.ScrollPosition = 0f;
	}

	//success
	public void SetData(com.kodgames.corgi.protocol.IllusionData illusionData)
	{
		this.illusionAvatars = illusionData.illusionAvatars;
		SysLocalDataBase.Inst.LocalPlayer.CardIds.Clear();
		SysLocalDataBase.Inst.LocalPlayer.CardIds.AddRange(illusionData.historyAvatarIds);
		illusionAvatars.Sort(ComparCard);

		if (this.IsShown)
			StartCoroutine("FillCardList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillCardList()
	{
		yield return null;

		for (int index = 0; index < illusionAvatars.Count; )
		{
			UIListItemContainer container = avatarObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemIllusionItem item = container.GetComponent<UIElemIllusionItem>();
			container.Data = item;
			for (int j = 0; j < item.avatarIcons.Count; ++j)
			{
				if (index < illusionAvatars.Count)
				{
					item.avatarIcons[j].gameObject.SetActive(true);
					item.avatarIcons[j].SetData(illusionAvatars[index++]);
				}
				else
					item.avatarIcons[j].gameObject.SetActive(false);
			}


			cardList.AddItem(item.gameObject);
		}

		yield return null;
		if (scrollPosi < 0)
			scrollPosi = 0;

		cardList.ScrollListTo(scrollPosi);
	}

	private int ComparCard(com.kodgames.corgi.protocol.IllusionAvatar avatar1, com.kodgames.corgi.protocol.IllusionAvatar avatar2)
	{
		return ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar2.recourseId).sortIndex - ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar1.recourseId).sortIndex;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatarIcon(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIPnlAvatarIllusion>((btn.Data as UIElemIllusionAvatarIcon).IllusionAvatar.recourseId);
	}
}

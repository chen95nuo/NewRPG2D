using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgAnnouncement : UIModule
{
	//Announcement list.
	public UIScrollList announcementList;
	//System announcement object pool.
	public GameObjectPool annObjPool;

	//点击按钮后显示的内容
	public UIScrollList annBtnList;
	public GameObjectPool annBtnPool;

	public SpriteText titleLabel;
	public UIElemAssetIcon announcementInfoBtn;

	//按钮控件组
	public UIChildLayoutControl powerBtnControl;
	public UIButton viewBtn;
	public UIButton goBackBtn;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		FillData();

		return true;
	}

	public override void OnHide()
	{
		ClearList();
		ClearBtnList();

		base.OnHide();
	}

	//Clear announcement list.
	private void ClearList()
	{
		StopCoroutine("FillList");
		announcementList.ClearList(false);
		announcementList.ScrollListTo(0f);

		//设置list不可见
		announcementList.gameObject.SetActive(false);

	}

	//清理详细内容的List并且处理详细内容才会显示的几个控件
	private void ClearBtnList()
	{
		StopCoroutine("FillBtnList");

		annBtnList.ClearList(false);
		annBtnList.ScrollListTo(0f);

		annBtnList.gameObject.SetActive(false);

		viewBtn.Hide(true);
		goBackBtn.Hide(true);
		announcementInfoBtn.Hide(true);
		titleLabel.Hide(true);

	}

	//Set announcement list data and fill the announcement list.
	private void FillData()
	{
		// Clear announcement list.
		ClearList();
		ClearBtnList();

		//对公告内容进行排序
		//SysLocalDataBase.Inst.LocalPlayer.NoticeData.Notices.Sort(SortById);

		//Fill the announcement list.
		StartCoroutine("FillList", SysLocalDataBase.Inst.LocalPlayer.NoticeData.Notices);
	}

	private int SortById(com.kodgames.corgi.protocol.Notice noticeX, com.kodgames.corgi.protocol.Notice noticeY)
	{
		return noticeY.id.CompareTo(noticeX.id);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(List<com.kodgames.corgi.protocol.Notice> announcements)
	{
		yield return null;

		//设置list可见
		announcementList.gameObject.SetActive(true);
		foreach (var announcement in announcements)
		{
			UIListItemContainer item = annObjPool.AllocateItem().GetComponent<UIListItemContainer>();

			UIElemAnnouncementTitleItem annTitleItem = item.gameObject.GetComponent<UIElemAnnouncementTitleItem>();
			annTitleItem.SetData(announcement);

			announcementList.AddItem(item);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillBtnList(com.kodgames.corgi.protocol.Notice announcement)
	{
		yield return null;

		annBtnList.gameObject.SetActive(true);
		UIListItemContainer item = annBtnPool.AllocateItem().GetComponent<UIListItemContainer>();
		UIElemAnnouncementItem annTitleItem = item.gameObject.GetComponent<UIElemAnnouncementItem>();
		annTitleItem.SetData(announcement);
		annBtnList.AddItem(item);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAnnOperationBtnClick(UIButton btn)
	{
		HideSelf();

		int gotoUI = (int)((btn.Data) as com.kodgames.corgi.protocol.Notice).gotoUI;

		GameUtility.JumpUIPanel(gotoUI);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAnnBtn(UIButton btn)
	{
		////////////////////////////////////////////
		//***************功能********************//
		//****点击后，设置标题list不可见 *******//
		//********生成详细内容list***** *******//
		////////////////////////////////////////////
		//设置list不可见
		announcementList.gameObject.SetActive(false);
		ClearBtnList();

		goBackBtn.Hide(false);


		var Data = (btn.Data as UIElemAssetIcon).Data as com.kodgames.corgi.protocol.Notice;
		if (Data.gotoUI != _UIType.UnKonw)
		{
			viewBtn.Hide(false);
			viewBtn.Data = Data;
			powerBtnControl.HideChildObj(viewBtn.gameObject, false);
			powerBtnControl.HideChildObj(goBackBtn.gameObject, false);
		}
		else
		{
			viewBtn.Hide(true);
			powerBtnControl.HideChildObj(viewBtn.gameObject, true);
			powerBtnControl.HideChildObj(goBackBtn.gameObject, false);
		}

		announcementInfoBtn.Hide(false);
		titleLabel.Hide(false);

		titleLabel.Text = Data.title;
		announcementInfoBtn.SetData(Data.iconId);

		StartCoroutine("FillBtnList", Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGoBack(UIButton btn)
	{
		//////////////////////////////////////////////////////////////////////////
		//*******点击返回按钮
		//************清空里面的list，设置标题list可见，隐藏详细list里面的内容
		ClearBtnList();

		//设置list可见
		announcementList.gameObject.SetActive(true);
	}
}

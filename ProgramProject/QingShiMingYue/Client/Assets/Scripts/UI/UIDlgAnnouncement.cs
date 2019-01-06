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

	//�����ť����ʾ������
	public UIScrollList annBtnList;
	public GameObjectPool annBtnPool;

	public SpriteText titleLabel;
	public UIElemAssetIcon announcementInfoBtn;

	//��ť�ؼ���
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

		//����list���ɼ�
		announcementList.gameObject.SetActive(false);

	}

	//������ϸ���ݵ�List���Ҵ�����ϸ���ݲŻ���ʾ�ļ����ؼ�
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

		//�Թ������ݽ�������
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

		//����list�ɼ�
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
		//***************����********************//
		//****��������ñ���list���ɼ� *******//
		//********������ϸ����list***** *******//
		////////////////////////////////////////////
		//����list���ɼ�
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
		//*******������ذ�ť
		//************��������list�����ñ���list�ɼ���������ϸlist���������
		ClearBtnList();

		//����list�ɼ�
		announcementList.gameObject.SetActive(true);
	}
}

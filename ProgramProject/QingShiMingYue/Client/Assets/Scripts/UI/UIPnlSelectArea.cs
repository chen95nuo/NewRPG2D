using UnityEngine;
using System.Collections;
using System.Text;
using ClientServerCommon;

public class UIPnlSelectArea : UIModule
{
	//Labels
	public SpriteText playerAccoutLabel;

	public SpriteText areaNumLabel;
	public SpriteText areaNameLabel;
	//public SpriteText areaTypeLabel;

	//Buttons
	public UIButton changeAccoutBtn;

	public UIButton enterGameBg;
	public UIButton selectAreaBg;

	// Area list.
	public UIScrollList areaList;
	public GameObjectPool areaObjectPool;
	public UIBox areaListBg;

	private KodGames.ClientClass.Area selectedArea;
	private const string ENTER_GAME_SOUND = "EnterGame";

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		// Record ShowAreaList. 
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.ShowAreaList);

		ClearList();

		if (SysLocalDataBase.Inst.LoginInfo.ServerAreas == null || SysLocalDataBase.Inst.LoginInfo.ServerAreas.Count <= 0)
			return true;

		// Set the last login name.
		bool areaSetted = false;
		foreach (var area in SysLocalDataBase.Inst.LoginInfo.ServerAreas)
		{
			if (SysLocalDataBase.Inst.LoginInfo.LastAreaId < 0
				&& (area.AreaStatus == _ServerAreaStatus.New
				|| area.AreaStatus == _ServerAreaStatus.Busy
				|| area.AreaStatus == _ServerAreaStatus.Hot))
			{
				SetSelectedArea(area);
				areaSetted = true;
				break;
			}
			else if (SysLocalDataBase.Inst.LoginInfo.LastAreaId == area.AreaID)
			{
				SetSelectedArea(area);
				areaSetted = true;
				break;
			}
		}

		if (areaSetted == false)
			SetSelectedArea(SysLocalDataBase.Inst.LoginInfo.ServerAreas[SysLocalDataBase.Inst.LoginInfo.ServerAreas.Count - 1]);

		RefreshTitle();

		return true;
	}

	public void SetView(bool canSelectArea)
	{
		enterGameBg.gameObject.SetActive(canSelectArea);
		selectAreaBg.gameObject.SetActive(canSelectArea);

		if (areaList.gameObject.activeSelf)
			areaListBg.gameObject.SetActive(false);
	}

	public override void OnHide()
	{
		ClearList();
		base.OnHide();
	}

	private void RefreshTitle()
	{
		if (Platform.Instance.IsGuest)
		{
			playerAccoutLabel.Text = GameUtility.FormatUIString("UIDlgAccontBinding_Label_GuestAccout", SysLocalDataBase.Inst.LoginInfo.AccountId.ToString());
			//changeAccoutBtn.Text = GameUtility.GetUIString("UIPnlSelectArea_Ctrl_BindAccount");
		}
		else
		{
			playerAccoutLabel.Text = GameUtility.FormatUIString("UIPnlSelectArea_Label_WelcomLabel", SysLocalDataBase.Inst.LoginInfo.Account);
			//changeAccoutBtn.Text = GameUtility.GetUIString("UIPnlSelectArea_Ctrl_ChangeAccount");
		}
	}

	private void SetSelectedArea(KodGames.ClientClass.Area area)
	{
		Color labelColor = UIElemAreaListItem.GetColorByStatus(area.AreaStatus);
		areaNumLabel.Text = labelColor.ToString() + GameUtility.FormatUIString("UIDlgAccontBinding_Label_Area", area.ShowAreaId);
		areaNameLabel.Text = labelColor.ToString() + area.AreaName;

		selectedArea = area;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnChangeAccountClick(UIButton btn)
	{
		if (Platform.Instance.IsGuest == false)
		{
			RequestMgr.Inst.Request(new LogoutReq(null));
		}
		else
		{
			SysUIEnv.Instance.ShowUIModule<UIDlgSetAccontBinding>();
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnEnterGameClick(UIButton btn)
	{
		//HideSelf();

		AudioManager.Instance.PlaySound(ENTER_GAME_SOUND, 0);
		// Call the enter game handler
		SysGameStateMachine.Instance.GetCurrentState<GameState_SelectArea>().ConnectIS(selectedArea);
	}

	public void OnConnectISSuccess()
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectAreaClick(UIButton btn)
	{
		enterGameBg.gameObject.SetActive(false);
		selectAreaBg.gameObject.SetActive(false);
		areaListBg.gameObject.SetActive(true);

		//Fill the area scroll list.
		StartCoroutine("InitializeAreaList");
	}

	/// <summary>
	/// Selects area method.
	/// </summary>
	/// <param name='btn'>
	/// Button.
	/// </param>
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void SelectArea(UIButton btn)
	{
		var area = btn.Data as KodGames.ClientClass.Area;

		ClearList();
		enterGameBg.gameObject.SetActive(true);
		selectAreaBg.gameObject.SetActive(true);
		areaListBg.gameObject.SetActive(false);

		SetSelectedArea(area);
	}

	/// <summary>
	/// Initialize the area list.
	/// </summary>
	/// <returns>
	/// The area list.
	/// </returns>
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private System.Collections.IEnumerator InitializeAreaList()
	{
		yield return null;

		//Fill the scroll list
		foreach (KodGames.ClientClass.Area area in SysLocalDataBase.Inst.LoginInfo.ServerAreas)
		{
			UIListItemContainer container = areaObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemAreaListItem item = container.gameObject.GetComponent<UIElemAreaListItem>();
			item.SetData(area, this, "SelectArea");
			//item.areaBtn.Data = area;
			if (area.AreaID == SysLocalDataBase.Inst.LoginInfo.LastAreaId)
				areaList.InsertItem(container, 0);
			else
				areaList.AddItem(container);
		}
	}

	private void ClearList()
	{
		StopCoroutine("InitializeAreaList");
		areaList.ClearList(false);
		areaList.ScrollListTo(0);
		enterGameBg.gameObject.SetActive(true);
		selectAreaBg.gameObject.SetActive(true);
		areaListBg.gameObject.SetActive(false);
	}
}

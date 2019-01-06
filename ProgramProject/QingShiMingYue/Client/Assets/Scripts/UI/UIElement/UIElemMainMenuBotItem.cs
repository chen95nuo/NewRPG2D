using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemMainMenuBotItem : MonoBehaviour
{
	public UIBox selectedLight;
	public UIButton menuButton;
	public GameObject newMessageRoot;

	private int _lnkUI = -1;
	public int LnkUI
	{
		get
		{
			return _lnkUI;
		}
		set
		{
			_lnkUI = value;
			menuButton.Data = _lnkUI;
		}
	}

	// Use this for initialization
	void Start()
	{
		selectedLight.Hide(true);
	}

	private void Update()
	{
		if (SysLocalDataBase.Inst.LocalPlayer == null)
			return;

		if (_lnkUI == _UIType.UIPnlGuildMessage && newMessageRoot != null && SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo != null)
			SetParticleState(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.NewsLeft > 0);

		if (_lnkUI == _UIType.UIPnlChatTab && newMessageRoot != null)
			SetParticleState(SysLocalDataBase.Inst.LocalPlayer.ChatData.UnreadGuildChatCount > 0);
	}

	public void SetSelectedStat(bool selected)
	{
		selectedLight.Hide(!selected);
	}

	public void SetParticleState(bool show)
	{
		if (newMessageRoot != null)
			newMessageRoot.SetActive(show);
	}
}

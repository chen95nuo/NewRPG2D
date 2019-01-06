using UnityEngine;
using System.Collections;
using ClientServerCommon;
using System.Collections.Generic;

public class UIPnlGuildMessageTab : UIModule
{
	public List<UIButton> tabButtons;
	public AutoSpriteControlBase msgLeftRoot;
	public SpriteText msgLeftLabel;

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		tabButtons[0].Data = _UIType.UIPnlGuildMessage;
		tabButtons[1].Data = _UIType.UIPnlGuildChat;

		return true;
	}

	private void Update()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.MsgLeft <= 0)
			msgLeftRoot.Hide(true);
		else
		{
			msgLeftRoot.Hide(false);

			if (!msgLeftLabel.Text.Equals(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.MsgLeft.ToString()))
				msgLeftLabel.Text = SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.MsgLeft.ToString();
		}
	}

	public void SetMsgLeftState(bool show)
	{
		msgLeftRoot.Hide(!show);
	}

	public void ChangeTabButtons(int uiType)
	{
		foreach (UIButton btn in tabButtons)
			btn.controlIsEnabled = ((int)btn.data) != uiType;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabButtonClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule((int)btn.data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackButton(UIButton btn)
	{
		if (ItemInfoUtility.IsInMainScene())
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildTab);
		else
		{
			foreach (var tabBtn in tabButtons)
				if (SysUIEnv.Instance.GetUIModule((int)tabBtn.Data) != null && SysUIEnv.Instance.GetUIModule((int)tabBtn.Data).IsShown)
					SysUIEnv.Instance.HideUIModule((int)tabBtn.Data);
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildPointMain);
			HideSelf();
		}
	}
}

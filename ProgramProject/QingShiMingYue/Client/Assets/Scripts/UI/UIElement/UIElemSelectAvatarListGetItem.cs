using System;
using System.Collections.Generic;

public class UIElemSelectAvatarListGetItem : MonoBehaviour
{
	public UIButton getButton;

	public void SetData(bool isDinerSelect)
	{
		getButton.Text = GameUtility.GetUIString(isDinerSelect ? "UIPnlSelectAvatarList_GoToAvatarDiner" : "UIPnlSelectAvatarList_GoToAvatarShopWine");
		getButton.Data = isDinerSelect;
	}
}
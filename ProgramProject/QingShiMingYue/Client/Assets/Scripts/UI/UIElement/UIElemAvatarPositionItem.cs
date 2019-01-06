using System;
using System.Collections.Generic;

public class UIElemAvatarPositionItem : MonoBehaviour
{
	public AutoSpriteControlBase posItemButton;

	public void SetData(int positionId)
	{
		posItemButton.Text = ItemInfoUtility.GetAssetName(positionId);
		posItemButton.Data = positionId;
	}

	public void SetControllEnable(int positionId)
	{
		posItemButton.controlIsEnabled = (int)posItemButton.Data != positionId;
	}
}

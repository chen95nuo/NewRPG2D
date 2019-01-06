using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemOrganTitleLabel : MonoBehaviour
{
	public SpriteText titleLabel;
	//public SpriteText redBox;

	public void SetData(string msg)
	{
		titleLabel.Text = msg;
		//redBox.SetColor(GameDefines.cardColorGray)
	}
}



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemOrganAttributeTitle : MonoBehaviour
{
	public SpriteText title1;
	public SpriteText title2;
	public SpriteText titleTips1;
	public SpriteText titleTips2;

	public void SetData(string tl1, string tl2, string tips1, string tips2)
	{
		title1.Text = tl1;
		title2.Text = tl2;
		titleTips1.Text = tips1;
		titleTips2.Text = tips2;
	}
}


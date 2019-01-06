using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemShopGiftViewItem : MonoBehaviour
{
	public SpriteText titleText;
	
	public void SetData(string text)
	{
		titleText.Text = text;
	}
}

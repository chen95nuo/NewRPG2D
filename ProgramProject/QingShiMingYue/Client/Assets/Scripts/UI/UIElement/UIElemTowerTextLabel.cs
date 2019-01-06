using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemTowerTextLabel : MonoBehaviour
{
	public SpriteText label;

	public void SetData(string text)
	{
		label.Text = text;
	}
}

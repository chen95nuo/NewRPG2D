using UnityEngine;
using System.Collections;

public class UIElemIllusionArriDescItem : MonoBehaviour
{
	public SpriteText descText;

	public void SetData(string desc)
	{
		descText.Text = desc;
	}
}

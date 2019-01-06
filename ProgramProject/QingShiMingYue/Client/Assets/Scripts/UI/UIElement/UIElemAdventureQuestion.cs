using UnityEngine;
using System.Collections;

public class UIElemAdventureQuestion : MonoBehaviour {

	public SpriteText contextText;

	public void SetData(string context)
	{
		contextText.Text = context;
	}
}

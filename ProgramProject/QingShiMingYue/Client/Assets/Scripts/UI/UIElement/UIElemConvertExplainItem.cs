using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemConvertExplainItem : MonoBehaviour
{
	public SpriteText label;

	public void SetDate(string label)
	{
		if (this.label != null)
			this.label.Text = label;
	}
}

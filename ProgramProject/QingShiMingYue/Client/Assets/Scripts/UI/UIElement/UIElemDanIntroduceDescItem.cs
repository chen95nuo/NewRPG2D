using UnityEngine;
using System.Collections;

public class UIElemDanIntroduceDescItem : MonoBehaviour
{
	public SpriteText messageLabel;

	public void SetData(string message)
	{
		messageLabel.Text = message;
	}
}

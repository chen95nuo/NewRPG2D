using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDanAtticTabItem : MonoBehaviour
{
	public UIButton tabBtn;
	public UIBox newBg;
	public SpriteText countLabel;

	public int count;

	public void SetNewBg()
	{		
		newBg.Hide(count <= 0);
		countLabel.Text = count.ToString();
	}
}
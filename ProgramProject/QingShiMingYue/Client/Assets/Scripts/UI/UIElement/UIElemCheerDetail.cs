using UnityEngine;
using System.Collections;

public class UIElemCheerDetail : MonoBehaviour 
{
	public SpriteText cheerPosLabel;
	public SpriteText cheerAvatarNameLabel;
	public SpriteText benifitLabel;
	
	public void SetData(string cheerPos, string cheerAvatarName, string benifit)
	{
		cheerPosLabel.Text = cheerPos;
		cheerAvatarNameLabel.Text = cheerAvatarName;
		benifitLabel.Text = benifit;
	}
}

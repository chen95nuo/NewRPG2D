using System;
using System.Collections.Generic;

public class UIElemAvatarAttributeItem : MonoBehaviour
{
	public List<SpriteText> attributeLables;

	public void InitView()
	{
		for(int i = 0; i< attributeLables.Count;i++)
			attributeLables[i].Text = string.Empty;
	}
}
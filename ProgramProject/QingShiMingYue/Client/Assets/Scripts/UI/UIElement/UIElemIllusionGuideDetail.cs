﻿using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemIllusionGuideDetail : MonoBehaviour
{
	public SpriteText titleLabel;
	public SpriteText contentLabel;

	public void SetData(ClientServerCommon.SubType subType)
	{
		titleLabel.Text = subType.name;
		contentLabel.Text = subType.desc;
	}

}

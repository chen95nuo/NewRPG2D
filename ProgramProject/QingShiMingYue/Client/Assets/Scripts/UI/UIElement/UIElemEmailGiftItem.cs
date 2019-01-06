using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemEmailGiftItem : MonoBehaviour
{
	public List<UIElemAssetIcon> attachmentBtns;

	public void InitButtonState()
	{
		foreach (var button in attachmentBtns)
			button.Hide(false);
	}
}

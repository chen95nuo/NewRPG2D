using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemAutoColor : MonoBehaviour
{
	public AutoSpriteControlBase spriteRoot;	

	private void Update()
	{
		spriteRoot.SetColor(spriteRoot.color);
	}
}
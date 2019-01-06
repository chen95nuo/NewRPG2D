using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UIAnimationHelper : MonoBehaviour
{
	public Color spriteTextColor;
	public SpriteText spriteText;

	void Start()
	{
		if (spriteText != null)
			spriteTextColor = spriteText.Color;
	}

	void Update()
	{
		if (spriteText == null)
			return;

		if (spriteText.Color.Equals(spriteTextColor))
			return;

		spriteText.SetColor(spriteTextColor);
	}
}
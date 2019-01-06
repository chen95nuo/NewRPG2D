using UnityEngine;
using System.Collections;

public static class UIUtility
{
	public static void SetIconInPixelUnit(AutoSpriteControlBase control, Material material, Rect pixelRect)
	{
		if (control.Started == false)
			control.Start();

		control.SetMaterial(material);
		
		// Convert pixel to UV
		Vector2 start = new Vector2(pixelRect.x, (pixelRect.y + pixelRect.height));
		Vector2 cellSize = new Vector2(pixelRect.width, pixelRect.height);
		start = control.PixelCoordToUVCoord(start);
		cellSize = control.PixelSpaceToUVSpace(cellSize);

		Rect uvRect = new Rect(0, 0, 0, 0);
		uvRect.x = start.x;
		uvRect.y = start.y;
		uvRect.xMax = uvRect.x + cellSize.x;
		uvRect.yMax = uvRect.y + cellSize.y;
		
		SetIcon(control, uvRect);
	}
	
	public static void SetIconInUVUnit(AutoSpriteControlBase control, Material material, Rect uvRect)
	{
		if (control.Started == false)
			control.Start();

		control.SetMaterial(material);
		SetIcon(control, uvRect);
	}

	private static void SetIcon(AutoSpriteControlBase control, Rect uvRect)
	{
		// Set current uv.
		control.SetUVs(uvRect);

		// Update animation.
		if (control.animations != null && control.animations.Length > 0)
		{
			foreach (UVAnimation uvAnim in control.animations)
			{
				SPRITE_FRAME[] frms = new SPRITE_FRAME[uvAnim.GetFrameCount()];

				for (int i = 0; i < uvAnim.GetFrameCount(); i++)
				{
					frms[i] = uvAnim.GetFrame(i);
					frms[i].uvs = uvRect;
				}

				uvAnim.SetAnim(frms);
			}

			bool active = control.gameObject.activeSelf;
			if (!active)
				control.gameObject.SetActive(true);

			control.PlayAnim(0);

			if (!active)
				control.gameObject.SetActive(false);
		}
	}

	public static void CopyIcon(AutoSpriteControlBase baseIcon, AutoSpriteControlBase sourceIcon)
	{
		if (baseIcon.renderer.sharedMaterial != sourceIcon.renderer.sharedMaterial)
			baseIcon.SetMaterial(sourceIcon.renderer.sharedMaterial);
		
		baseIcon.SetUVs(sourceIcon.GetUVs());

		if (baseIcon.animations != null && baseIcon.animations.Length > 0)
		{
			for (int j = 0; j < baseIcon.animations.Length; j++)
			{
				SPRITE_FRAME[] frms = new SPRITE_FRAME[baseIcon.animations[j].GetFrameCount()];
				SPRITE_FRAME[] frmb = new SPRITE_FRAME[sourceIcon.animations[j].GetFrameCount()];

				for (int i = 0; i < baseIcon.animations[j].GetFrameCount(); i++)
				{
					frms[i] = sourceIcon.animations[j].GetFrame(i);
					frmb[i] = baseIcon.animations[j].GetFrame(i);
					frmb[i].uvs = frms[i].uvs;
				}

				baseIcon.animations[j].SetAnim(frmb);
			}

			bool active = baseIcon.gameObject.activeSelf;
			if (!active)
				baseIcon.gameObject.SetActive(true);

			baseIcon.PlayAnim(0);

			if (!active)
		    	baseIcon.gameObject.SetActive(false);
        
		}
	}

	public static void CopyIconTrans(AutoSpriteControlBase baseIcon, AutoSpriteControlBase sourceIcon)
	{
		// Copy transition
		for (int i = 0; i < sourceIcon.Transitions.Length && i < baseIcon.Transitions.Length; ++i)
			if (sourceIcon.Transitions[i] != null && baseIcon.Transitions[i] != null)
				sourceIcon.Transitions[i].CopyTo(baseIcon.Transitions[i]);

		UIButton btn = baseIcon as UIButton;
		btn.SetControlState(UIButton.CONTROL_STATE.OVER, true);
		btn.SetControlState(UIButton.CONTROL_STATE.NORMAL);
	}

	public static float LerpWithoutClamp(float from, float to, float t)
	{
		return from + (to - from) * t;
	}

	public static void UpdateUIText(SpriteText sp, string value)
	{
		if (value != null && sp.Text.Equals(value) == false)
			sp.Text = value;
	}
}
//#define TestMode

using UnityEngine;
using System;
using System.Collections.Generic;

[AddComponentMenu("EZ GUI/Font/GUI Dynamic Font")]
[ExecuteInEditMode]
public class GUIDynamicFont : SpriteDynamicFont
{
	/// <summary>
	/// GUIStyle to define the font
	/// </summary>
	[HideInInspector]
	public GUIStyle guiStyle = new GUIStyle();

	// Our characters:
	protected List<SpriteChar> pendingRenderChars = new List<SpriteChar>();

	// Working variable for generating font texture 
	private GUIContent guiContent = new GUIContent();

	/// <summary>
	/// The default height, in pixels, between lines.
	/// </summary>
	public override float LineHeight
	{
		get { return guiStyle.lineHeight; }
	}

	/// <summary>
	///	The distance, in pixels, from the absolute top
	/// of a line to the baseline:
	/// </summary>
	public override int BaseHeight
	{
		get { return (int)(guiStyle.lineHeight * 2 / 3); }
	}

	/// <summary>
	///	The size (height) of the font, in pixels. This is the height, in pixels of a full-height character.
	/// </summary>
	public override int PixelSize
	{
		get { return guiStyle.fontSize; }
	}

	public override bool ContainsCharacter(char ch)
	{
		if (IsCharacterLoaded(ch))
			return true;

		return guiStyle != null && guiStyle.font != null &&  guiStyle.font.HasCharacter(ch);
	}

	public override SpriteChar AddSpriteChar(char ch)
	{
		// If the character is not in the font
		if (guiStyle == null || guiStyle.font == null || guiStyle.font.HasCharacter(ch) == false)
			return new SpriteChar();

		// Create a new SpriteChar
		guiContent.text = ch.ToString();

		SpriteChar newSpriteChar = new SpriteChar();
		newSpriteChar.id = Convert.ToInt32(ch);
		newSpriteChar.xAdvance = guiStyle.CalcSize(guiContent).x;
		newSpriteChar.waitForRenderData = true;

		// Add it to the pending list to render it to texture in OnGUI callback			
		pendingRenderChars.Add(newSpriteChar);

		return newSpriteChar;
	}

	public virtual void OnGUI()
	{
		// Rendering only can be processed in repaint event. 
		if (Event.current.type == EventType.Repaint)
		{
			foreach (Material mat in fontMaterials)
			{
				RenderTexture rt = (RenderTexture)mat.GetTexture(textureProperty);

				if (rt.IsCreated() == false)
				{
					// Sometimes renter texture will be lost, such as Locking screen. We need to recreate all font texture.
					pendingRenderChars.Clear();
					foreach (var item in charMap)
						pendingRenderChars.Add(item.Value);

					// Rest the start position of the next font
					nextFontTextureIndex = 0;
					nextFontPixel = default(Vector2);

					break;
				}
			}

			if (pendingRenderChars.Count != 0)
			{
				// Grab the texture size
				int texWidth = TexWidth;
				int texHeight = TexHeight;

				// Back current render texture
				RenderTexture oldRT = RenderTexture.active;

				foreach (var spriteChar in pendingRenderChars)
				{
					// Fill GUI content to measure character size
					guiContent.text = Convert.ToChar(spriteChar.id).ToString();

					// Get character size
					Vector2 charSize = guiStyle.CalcSize(guiContent);
					if (charSize.y != guiStyle.lineHeight)
					{
						Debug.LogError("charSize.y != guiStyle.lineHeight");
						continue;
					}
					if (charSize.x > texWidth || charSize.y > texHeight)
					{
						Debug.LogError("charSize is greater than texture size");
						continue;
					}

					// Get render texture
					Debug.Log(guiContent.text + " " + nextFontPixel + " " + charSize);
					if (nextFontPixel.x + charSize.x > texWidth && nextFontPixel.y + guiStyle.lineHeight + charSize.y > texHeight)
					{
						// Step to next font texture
						++nextFontTextureIndex;

						// Rest the start position of the next font
						nextFontPixel = default(Vector2);
					}

					if (nextFontTextureIndex >= fontMaterials.Count)
					{
						//Debug.Log("Creating a new font material");
						//Debug.Log("fontMaterials.Count : " + fontMaterials.Count);
						//Debug.Log("texWidth : " + texWidth + ",texHeight : " + texHeight);
						//Debug.Log("nextFontPixel.x + charSize.x : " + nextFontPixel.x + charSize.x);
						//Debug.Log("nextFontPixel.y + charSize.y : " + nextFontPixel.y + charSize.y);

						// Create a new material to contain the new character.
						//Material newMat = new Material(Shader.Find("Transparent/Vertex Colored"));
						Material newMat = new Material(Shader.Find("GUI/Text Shader"));

						RenderTexture newRT = new RenderTexture(texWidth, texHeight, 0);

						newMat.SetTexture(textureProperty, newRT);
						fontMaterials.Add(newMat);
#if TestMode
						fontTextures.Add(newRT);
#endif
					}

					// Grab the render texture for rendering
					RenderTexture renderTex = (RenderTexture)fontMaterials[nextFontTextureIndex].GetTexture(textureProperty);

					// Calculate new character position on the texture
					Rect rect = default(Rect);
					if (nextFontPixel.x + charSize.x > texWidth)
					{
						nextFontPixel.x = 0;
						nextFontPixel.y += guiStyle.lineHeight;
					}

					rect.x = nextFontPixel.x;
					rect.y = Screen.height - charSize.y - nextFontPixel.y;
					rect.width = charSize.x;
					rect.height = charSize.y;

					// Increase the XY pixel for next character rendering
					nextFontPixel.x += charSize.x;

					// Render to target
					RenderTexture.active = renderTex;
					GL.Viewport(new Rect(0, 0, Screen.width, Screen.height));
					GUI.Label(rect, guiContent, guiStyle);

					// Set SpriteChar data
					spriteChar.page = fontMaterials.Count - 1;
					spriteChar.UVs.x = (rect.x + 1) / texWidth;
					spriteChar.UVs.y = nextFontPixel.y / texHeight;
					spriteChar.UVs.xMax = (rect.x + 1 + charSize.x) / texWidth;
					spriteChar.UVs.yMax = (charSize.y + nextFontPixel.y) / texHeight;

					// Validate SpriteChar
					spriteChar.waitForRenderData = false;

					//Debug.Log("SpriteChar");
					//Debug.Log("ID : " + spriteChar.id + "," + guiContent.text);
					//Debug.Log(guiContent.text + ",UVs : " + spriteChar.UVs);
					//Debug.Log("\txOffset : " + spriteChar.xOffset);
					//Debug.Log("\tyOffset : " + spriteChar.yOffset);
					//Debug.Log("xAdvance : " + spriteChar.xAdvance);
					//Debug.Log("Character Size : " + charSize);
					//Debug.Log("\tpage : " + spriteChar.page);
				}

				// Restore previous render texture
				RenderTexture.active = oldRT;

				// Create pending list
				pendingRenderChars.Clear();

				//Debug.Log("chars map : " + chars.Count);
			}
		}
	}

#if TestMode
	
	public GUIContent testContent = new GUIContent();
	public virtual void Update()
	{
		foreach(char ch in testContent.text)
		{
			GetSpriteChar(ch);
		}

		if (reset)
			Reset();

		if (exportRT)
			ExportRT();
	}
	
	public bool reset = false;
	void Reset()
	{
		charMap.Clear();
		chars.Clear();
		fontMaterials.Clear();
		pendingRenderChars.Clear();

		nextFontTextureIndex = 0;
		nextFontPixel = default(Vector2);

		fontTextures.Clear();
		reset = false;
	}

	public List<RenderTexture> fontTextures = new List<RenderTexture>();
	public bool exportRT = false;
	void ExportRT()
	{
		for (int i = 0; i < fontMaterials.Count; ++i )
		{
			RenderTexture rt = (RenderTexture)fontMaterials[i].GetTexture(textureProperty);

			Texture2D screenShot = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);

			RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
			RenderTexture.active = null;

			byte[] bytes = screenShot.EncodeToPNG();
			string filename = guiStyle.font.fontNames + "_" + i + ".png";
			System.IO.File.WriteAllBytes(filename, bytes);
		}

		exportRT = false;
	}
#endif
}
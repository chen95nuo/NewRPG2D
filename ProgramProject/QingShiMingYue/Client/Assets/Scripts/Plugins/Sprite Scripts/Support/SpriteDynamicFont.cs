//-----------------------------------------------------------------
//  Copyright 2010 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------

//#define TestMode

using UnityEngine;
using System;
using System.Collections.Generic;

/// <remarks>
/// A class that holds information about a font
/// intended for use with SpriteText.
/// </remarks>
public abstract class SpriteDynamicFont : SpriteFontBase
{
	/// <summary>
	/// Property name to retrieve texture in material
	/// </summary>
	protected static readonly string textureProperty = "_MainTex";
	
	/// <summary>
	/// The size of font texture 
	/// </summary>
	public int textureWidth = 256;
	public int textureHeight = 256;
	
	/// <summary>
	/// Material for font rendering
	/// </summary>
	protected List<Material> fontMaterials = new List<Material>();

	// Maps character IDs to that character's
	// index in the array.
	protected Dictionary<char, SpriteChar> charMap = new Dictionary<char, SpriteChar>();

	// Working variable for generating font texture 
	protected int nextFontTextureIndex = 0;
	protected Vector2 nextFontPixel = default(Vector2);

	// The width and height of the font atlas.
	public override int TexWidth 
	{
		get { return textureWidth; } 
	}

	public override int TexHeight 
	{
		get { return textureHeight; } 
	}

	public override int PageCount
	{
		get { return fontMaterials.Count; }
	}

	/// <summary>
	/// Get material count for text rendering
	/// </summary>
	/// <returns>The count of materials</returns>
	public override int GetMaterialCount()
	{
		return fontMaterials.Count;
	}

	/// <summary>
	/// Get material for text rendering
	/// </summary>
	/// <param name="idx">The index of wanted material </param>
	/// <returns>Material</returns>
	public override Material GetMaterial(int idx)
	{
		return fontMaterials[idx];
	}

	public abstract SpriteChar AddSpriteChar(char ch);

	/// <summary>
	/// Returns a reference to the SpriteChar that
	/// corresponds to the specified character ID
	/// (usually the numeric Unicode value).
	/// </summary>
	/// <param name="ch">The numeric value/code of the desired character.
	/// This value can be obtained from a char with Convert.ToInt32().</param>
	/// <returns>Reference to the corresponding SpriteChar that contains information about the character.</returns>
	public override SpriteChar GetSpriteChar(char ch)
	{
		SpriteChar spriteChar = null;
		if (!charMap.TryGetValue(ch, out spriteChar))
		{
			// Character not found in the map, try to create it dynamically
			spriteChar = AddSpriteChar(ch);

			charMap.Add(ch, spriteChar);
		}

		return spriteChar;
	}

	public bool IsCharacterLoaded(char ch)
	{
		return charMap.ContainsKey(ch);
	}

	public virtual void Init()
	{
		// Clear old data
		fontMaterials.Clear();
		charMap.Clear();
		nextFontTextureIndex = 0;
		nextFontPixel = default(Vector2);
	}

	public virtual void FreeMemory()
	{
		fontMaterials.Clear();
		charMap.Clear();
		nextFontTextureIndex = 0;
		nextFontPixel = default(Vector2);
	}
}

// Mirrors the editable settings of a control that affect
public class SpirteDynamicFontMirror
{
	int textureWidth;
	int textureHeight;
	TextureFormat textureFormat;

	// Mirrors the specified control's settings
	public virtual void Mirror(SpriteDynamicFont f)
	{
		textureWidth = f.textureWidth;
		textureHeight = f.textureHeight;
	}

	// Returns true if any of the settings do not match:
	public virtual bool DidChange(SpriteDynamicFont f)
	{
		if (textureWidth != f.textureWidth)
			return true;

		if (textureHeight != f.textureHeight)
			return true;
		
		return false;
	}

	public virtual void Validate(PluginDynamicFont f)
	{

	}
}
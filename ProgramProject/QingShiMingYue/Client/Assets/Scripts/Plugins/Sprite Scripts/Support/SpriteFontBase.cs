//-----------------------------------------------------------------
//  Copyright 2010 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


/// <remarks>
/// A struct that holds important information
/// about a sprite-based character.
/// </remarks>
public class SpriteChar
{
	/// <summary>
	/// The "id" of the character (usually the ASCII value).
	/// </summary>
	public int id;

	/// <summary>
	/// The offset, in pixels, of the char's mesh from its "zero-point".
	/// </summary>
	public float xOffset, yOffset;

	/// <summary>
	/// How far to move, in pixels, from this char to position the next one.
	/// </summary>
	public float xAdvance;

	/// <summary>
	/// Whether or not render data is valid
	/// </summary>
	public bool waitForRenderData;

	/// <summary>
	/// The material index contents this character
	/// </summary>
	public int page;

	/// <summary>
	/// The UV coords of the character.
	/// </summary>
	public Rect UVs;

	/// <summary>
	/// The map of kernings to use for preceding characters.
	/// The key is the previous character, and the value is 
	/// the kerning amount, in pixels.
	/// </summary>
#if UNITY_IPHONE && !(UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9|| UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
	public Hashtable kernings;
#else
	public Dictionary<int, float> kernings;
#endif

#if UNITY_IPHONE && !(UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9|| UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
	public Hashtable origKernings;
#else
	public Dictionary<int, float> origKernings;
#endif

	/// <summary>
	/// Gets the kerning amount given the previous character.
	/// </summary>
	/// <param name="prevChar">The character that precedes this one.</param>
	/// <returns>The kerning amount, in pixels.</returns>
	public float GetKerning(int prevChar)
	{
		if (kernings == null)
			return 0;

		float amount = 0;

#if UNITY_IPHONE && !(UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9|| UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
		if (kernings.ContainsKey(prevChar))
			amount = (float) kernings[prevChar];
#else
		kernings.TryGetValue(prevChar, out amount);
#endif
		return amount;
	}
}

/// <remarks>
/// A class that holds information about a font
/// intended for use with SpriteText.
/// </remarks>
public abstract class SpriteFontBase : MonoBehaviour
{
	/// <summary>
	/// User-defined font name, can not be same with other fonts.
	/// </summary>
	public string fontName = "";

	/// <summary>
	/// The default height, in pixels, between lines.
	/// </summary>
	public abstract float LineHeight
	{ 
		get; 
	}

	/// <summary>
	///	The distance, in pixels, from the absolute top
	/// of a line to the baseline:
	/// </summary>
	public abstract int BaseHeight 
	{ 
		get; 
	}

	/// <summary>
	/// The width of the font atlas
	/// </summary>
	public abstract int TexWidth
	{ 
		get;
	}

	/// <summary>
	/// The height of the font atlas
	/// </summary>
	public abstract int TexHeight 
	{ 
		get;
	}

	/// <summary>
	/// The height of the font atlas
	/// </summary>
	public abstract int PageCount
	{
		get;
	}

	/// <summary>
	///	The size (height) of the font, in pixels.
	/// This is the height, in pixels of a full-height
	/// character.
	/// </summary>
	public abstract int PixelSize 
	{ 
		get; 
	}

	/// <summary>
	/// Get material count for text rendering
	/// </summary>
	/// <returns>The count of materials</returns>
	public abstract int GetMaterialCount();

	/// <summary>
	/// Get material for text rendering
	/// </summary>
	/// <param name="idx">The index of wanted material </param>
	/// <returns>Material</returns>
	public abstract Material GetMaterial(int idx);

	/// <summary>
	/// Returns a reference to the SpriteChar that
	/// corresponds to the specified character ID
	/// (usually the numeric Unicode value).
	/// </summary>
	/// <param name="ch">The numeric value/code of the desired character.
	/// This value can be obtained from a char with Convert.ToInt32().</param>
	/// <returns>Reference to the corresponding SpriteChar that contains information about the character.</returns>
	public abstract SpriteChar GetSpriteChar(char ch);

	/// <summary>
	/// Returns whether the specified character is part
	/// of this font definition.
	/// </summary>
	/// <param name="ch">Character to check.</param>
	/// <returns>True if the character exists in the font definition.  False otherwise.</returns>
	public abstract bool ContainsCharacter(char ch);

	/// <summary>
	/// Gets how wide the specified string
	/// would be, in pixels.
	/// </summary>
	/// <param name="str">The string to measure.</param>
	/// <returns>The width, in pixels, of the string.</returns>
	public float GetWidth(string str)
	{
		SpriteChar chr;
		float width = 0;

		if (str.Length < 1)
			return 0;

		// Get the first character:
		chr = GetSpriteChar(str[0]);

		if (chr != null)
			width = chr.xAdvance;

		for (int i = 1; i < str.Length; ++i)
		{
			chr = GetSpriteChar(str[i]);

			if (chr != null)
				width += chr.xAdvance + chr.GetKerning(str[i - 1]);
		}

		return width;
	}

	/// <summary>
	/// Gets how wide the specified string
	/// would be, in pixels.
	/// </summary>
	/// <param name="str">The string to measure.</param>
	/// <param name="start">The index of the first character of the substring to be measured.</param>
	/// <param name="end">The index of the last character of the substring.</param>
	/// <returns>The width, in pixels, of the string.</returns>
	public float GetWidth(string str, int start, int end)
	{
		SpriteChar chr;
		float width = 0;

		if (start >= str.Length || end < start)
			return 0;

		end = Mathf.Clamp(end, 0, str.Length - 1);

		// Get the first character:
		chr = GetSpriteChar(str[start]);

		if (chr != null)
			width = chr.xAdvance;

		for (int i = start + 1; i <= end; ++i)
		{
			chr = GetSpriteChar(str[i]);

			if (chr != null)
				width += chr.xAdvance + chr.GetKerning(str[i - 1]);
		}

		return width;
	}

	/// <summary>
	/// Gets how wide the specified string
	/// would be, in pixels.
	/// </summary>
	/// <param name="str">The string to measure.</param>
	/// <param name="start">The index of the first character of the substring to be measured.</param>
	/// <param name="end">The index of the last character of the substring.</param>
	/// <returns>The width, in pixels, of the string.</returns>
	public float GetWidth(StringBuilder sb, int start, int end)
	{
		SpriteChar chr;
		float width = 0;

		if (start >= sb.Length || end < start)
			return 0;

		end = Mathf.Clamp(end, 0, sb.Length - 1);

		// Get the first character:
		chr = GetSpriteChar(sb[start]);

		if (chr != null)
			width = chr.xAdvance;

		for (int i = start + 1; i <= end; ++i)
		{
			chr = GetSpriteChar(sb[i]);

			if (chr != null)
				width += chr.xAdvance + chr.GetKerning(sb[i - 1]);
		}

		return width;
	}

	/// <summary>
	/// Gets how wide the specified character
	/// would be, in pixels, when displayed.
	/// </summary>
	/// <param name="prevChar">The character previous to that being measured.</param>
	/// <param name="str">The character to measure.</param>
	/// <returns>The width, in pixels, of the character, as displayed (includes the xAdvance).</returns>
	public float GetWidth(char prevChar, char c)
	{
		SpriteChar chr = GetSpriteChar(c);

		if (chr == null)
			return 0;

		return chr.xAdvance + chr.GetKerning(prevChar);
	}

	/// <summary>
	/// Returns the xAdvance of the specified character.
	/// 0 is returned if the character isn't supported.
	/// </summary>
	/// <param name="c">The character to look up.</param>
	/// <returns>The xAdvance of the character.</returns>
	public float GetAdvance(char c)
	{
		SpriteChar ch = GetSpriteChar(c);

		if (ch == null)
			return 0;
		else
			return ch.xAdvance;
	}

	/// <summary>
	/// Returns a version of the specified string with all
	/// characters removed which are not defined for this font.
	/// </summary>
	/// <param name="str">The string to be stripped of unsupported characters.</param>
	/// <returns>A new string containing only those characters supported by this font.</returns>
	public string RemoveUnsupportedCharacters(string str)
	{
		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < str.Length; ++i)
			if (ContainsCharacter(str[i]) || str[i] == '\n' || str[i] == '\t' ||
				str[i] == '#' || str[i] == '[' || str[i] == ']' || str[i] == '(' || str[i] == ')' || str[i] == ',') // Color tag chars
				sb.Append(str[i]);

		return sb.ToString();
	}
}
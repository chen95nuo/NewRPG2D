//-----------------------------------------------------------------
//  Copyright 2010 Brady Wright and Above and Beyond Software
//	All rights reserved
//-----------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <remarks>
/// This class serves as a scene-wide store of all
/// fonts currently in use.  This is so we can
/// cache the font data once instead of having to
/// read from disk every time we create some text.
/// </remarks>
[AddComponentMenu("EZ GUI/Management/Font Manager")]
[ExecuteInEditMode]
public class FontManager : MonoBehaviour
{
	//----------------------------------------------------------------
	// Singleton code
	//----------------------------------------------------------------
	// s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
	private static FontManager s_Instance = null;

	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	public static FontManager instance
	{
		get
		{
			if (s_Instance == null)
			{
				s_Instance = FindObjectOfType(typeof(FontManager)) as FontManager;

				if (s_Instance == null && Application.isEditor)
					Debug.LogError("Could not locate a FontManager object. You have to have exactly one FontManager in the scene.");
			}

			return s_Instance;
		}
	}

	public static bool Exists()
	{
		return s_Instance != null;
	}
	//----------------------------------------------------------------
	// End Singleton code
	//----------------------------------------------------------------

	public int fontCount;
	public SpriteFontBase defaultFont;

	bool fontRetrived = false;

	// The list of fonts currently loaded.
	Dictionary<string, SpriteFontBase> fonts = new Dictionary<string, SpriteFontBase>();

	void Awake()
	{
		// See if we are a superfluous instance:
		if (s_Instance != null)
		{
			Debug.LogError("You can only have one instance of this singleton object in existence.");
		}
		else
			s_Instance = this;
	}

	void OnApplicationQuit()
	{
		s_Instance = null;
	}

	void UpdateFontStore()
	{
		if (fonts.Count != 0)
		{
			// Remove invalid font, it may be deleted
			List<string> pendingRemoveFonts = new List<string>();
			List<string> pendingRenameFonts = new List<string>();

			foreach (KeyValuePair<string, SpriteFontBase> element in fonts)
			{
				if (element.Value == null)
				{
					pendingRemoveFonts.Add(element.Key);
				}
				else if (element.Value.fontName != element.Key)
				{
					pendingRenameFonts.Add(element.Key);
				}
			}

			foreach (string key in pendingRemoveFonts)
			{
				fonts.Remove(key);
			}

			foreach (string key in pendingRenameFonts)
			{
				SpriteFontBase spriteFont = fonts[key];
				fonts.Remove(key);
				fonts.Add(spriteFont.fontName, spriteFont);
			}
		}

		foreach (SpriteFontBase spriteFont in GetComponentsInChildren<SpriteFontBase>(true))
		{
			if (fonts.ContainsKey(spriteFont.fontName) == false)
			{
				fonts.Add(spriteFont.fontName, spriteFont);
			}
		}
	}

	/// <summary>
	/// Returns the SpriteFontBase object for the
	/// specified definition file.
	/// If no existing object is found, it is
	/// loaded from storage.
	/// </summary>
	/// <param name="fontName">The TextAsset that defines the font.</param>
	/// <returns>A reference to the font definition object.</returns>
	public SpriteFontBase GetFont(string fontName)
	{
		if (fontName == null)
			return null;

		if (fontRetrived == false)
		{
			UpdateFontStore();
			fontRetrived = true;
		}

		if (fonts.ContainsKey(fontName) == false)
			return null;

		SpriteFontBase spriteFont = fonts[fontName];
		if (!Application.isPlaying && spriteFont is SpriteFont)
			((SpriteFont)spriteFont).Load(((SpriteFont)spriteFont).fontDef);

		return spriteFont;
	}

	public SpriteFontBase GetDefaultFont()
	{
		if (fonts.Count == 0)
			return null;

		if (defaultFont != null)
			return defaultFont;

		Dictionary<string, SpriteFontBase>.Enumerator iter = fonts.GetEnumerator();
		iter.MoveNext();
		return iter.Current.Value;
	}

	public string GetDefaultFontName()
	{
		SpriteFontBase defaultFont = GetDefaultFont();
		return defaultFont != null ? defaultFont.fontName : "";
	}

	public void FreeMemory()
	{
		foreach (var font in fonts)
			if (font.Value is SpriteDynamicFont)
				(font.Value as SpriteDynamicFont).FreeMemory();
	}

#if UNITY_EDITOR
	void Update()
	{
		fontCount = fonts.Count;

		UpdateFontStore();
	}

	public string[] GetFontNames()
	{
		List<string> fontNames = new List<string>();

		foreach (KeyValuePair<string, SpriteFontBase> element in fonts)
		{
			fontNames.Add(element.Key);
		}

		return fontNames.ToArray();
	}

	[ContextMenu("FreeDynamicFontTexture")]
	public void FreeDynamicFontTexture()
	{
		foreach (var kvp in fonts)
			if (kvp.Value is SpriteDynamicFont)
				(kvp.Value as SpriteDynamicFont).FreeMemory();
	}
#endif
}
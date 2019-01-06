//#define ENABLE_FORCE_USE_IOS_FONT
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class FontSetting
{
	public bool valid = false;
	public string fileName = "";
	public string familyName = "";
	public string styleName = "";
	public int fontSize = 12;
	public float characterSize = 1.0f;
	public float lineHeightFactor = 1.0f;
	public int baseOffsetY = 0;	
	
	public void Copy(FontSetting f)
	{
		if (f == null)
			return;
		
		fileName = f.fileName;
		familyName = f.familyName;
		styleName = f.styleName;
		fontSize = f.fontSize;
		characterSize = f.characterSize;
		baseOffsetY = f.baseOffsetY;
	}
	
	public bool Equals(FontSetting f)
	{
		return f != null && 
			fileName.Equals(f.fileName, StringComparison.InvariantCultureIgnoreCase) &&
			familyName.Equals(f.familyName, StringComparison.InvariantCultureIgnoreCase) &&
			styleName.Equals(f.styleName, StringComparison.InvariantCultureIgnoreCase) &&	
			fontSize == f.fontSize && 
			characterSize == f.characterSize && 
			baseOffsetY == f.baseOffsetY;
	}
}

[AddComponentMenu("EZ GUI/Font/Plugin Dynamic Font")]
[ExecuteInEditMode]
public class PluginDynamicFont : SpriteDynamicFont
{
	protected PluginDynamicFontMirror mirror;
	
	[HideInInspector]
	public FontSetting iosFont = new FontSetting();	
	[HideInInspector]
	public FontSetting fallbackIosFont = new FontSetting();

	[HideInInspector]
	public FontSetting androidFont = new FontSetting();
	[HideInInspector]
	public FontSetting fallbackAndroidFont = new FontSetting();

	/// <summary>
	/// Color of font body
	/// </summary>
	[HideInInspector]
	public Color fontColor = Color.white;
	
	/// <summary>
	/// Bold style
	/// </summary>
	[HideInInspector]
	public bool bold = false;

	/// <summary>
	/// Italic style
	/// </summary>
	[HideInInspector]
	public bool italic = false;

	/// <summary>
	/// Outline font body
	/// </summary>
	[HideInInspector]
	public bool outline = false;

	/// <summary>
	/// The width of outline
	/// </summary>
	[HideInInspector]
	public int outlineWidth = 1;

	/// <summary>
	/// The color of outline
	/// </summary>
	[HideInInspector]
	public Color outlineColor = Color.black;

	protected int lineHeight;
	protected int baseHeight;

	protected bool started = false;
	protected int fontHandle = FontPlugin.InvalidHandle;
	protected int fallbackFontHandle = FontPlugin.InvalidHandle;
	// PInvoke is very slow, save the unsupported to reduce the query of plugin.
	protected List<char> unsupportedChars = new List<char>();
	
	protected int curLineHeightInAtlas = 0;
	
	// Sprite char to delay apply texture changing 
	protected List<SpriteChar> pendingRenderChars = new List<SpriteChar>();
	// Texture list for delay applying changing.
	protected List<Texture2D> changedFontTextures = new List<Texture2D>();

	public override float LineHeight
	{
#if UNITY_EDITOR && ENABLE_FORCE_USE_IOS_FONT
		get { return lineHeight * iosFont.lineHeightFactor; }
#elif UNITY_IOS
		get { return lineHeight * iosFont.lineHeightFactor; }
#elif UNITY_ANDROID
		get { return lineHeight * androidFont.lineHeightFactor; }
#else
		get { return lineHeight; }
#endif
	}

	public override int BaseHeight
	{
		get { return baseHeight; }
	}

	public override int PixelSize
	{
#if UNITY_EDITOR && ENABLE_FORCE_USE_IOS_FONT
		get { return (int)(iosFont.fontSize / iosFont.characterSize); }
#elif UNITY_IOS
		get { return (int)(iosFont.fontSize / iosFont.characterSize); }
#elif UNITY_ANDROID
		get { return (int)(androidFont.fontSize / androidFont.characterSize); }
#else
		get { return 0; }
#endif
	}

	public void Start()
	{
		if (started)
			return;

		started = true;

		Init();
	}

	public override void Init()
	{
		base.Init();

		FontSetting fontSetting = null;
		FontSetting fallbackFontSetting = null;
#if UNITY_EDITOR && ENABLE_FORCE_USE_IOS_FONT
		fontSetting = iosFont;
		fallbackFontSetting = fallbackIosFont.valid ? fallbackIosFont : null;
#elif UNITY_IOS
		fontSetting = iosFont;
		fallbackFontSetting = fallbackIosFont.valid ? fallbackIosFont : null;
#elif UNITY_ANDROID
		fontSetting = androidFont;
		fallbackFontSetting = fallbackAndroidFont.valid ? fallbackAndroidFont : null;
#endif

		if (fontSetting != null)
		{
#if KOREAN_FONT_SUPPORT && UNITY_ANDROID
			if (FontPlugin.LoadFontFace("NanumGothic.ttf", "NanumGothic", "Regular", fontSetting.fontSize, fontColor, bold, italic, outline, ref fontHandle, ref lineHeight, ref baseHeight) == false
				//Android 5
				&& FontPlugin.LoadFontFace("NotoSansKR-Regular.otf", "Noto Sans KR", "Regular", fontSetting.fontSize, fontColor, bold, italic, outline, ref fontHandle, ref lineHeight, ref baseHeight) == false
				&& FontPlugin.LoadFontFace(fontSetting.fileName, fontSetting.familyName, fontSetting.styleName, fontSetting.fontSize, fontColor, bold, italic, outline, ref fontHandle, ref lineHeight, ref baseHeight) == false)
				return;
#elif KOREAN_FONT_SUPPORT && UNITY_IPHONE
			if (FontPlugin.LoadFontFace(fontSetting.fileName, fontSetting.familyName, fontSetting.styleName, fontSetting.fontSize, fontColor, bold, italic, outline, ref fontHandle, ref lineHeight, ref baseHeight) == false
			    && FontPlugin.LoadFontFace("AppleGothic.otf", "NanumGothic", "Regular", fontSetting.fontSize, fontColor, bold, italic, outline, ref fontHandle, ref lineHeight, ref baseHeight) == false)
				return;
#else
			if (FontPlugin.LoadFontFace(fontSetting.fileName, fontSetting.familyName, fontSetting.styleName, fontSetting.fontSize, fontColor, bold, italic, outline, ref fontHandle, ref lineHeight, ref baseHeight) == false
			//Android 5
			&& FontPlugin.LoadFontFace("NotoSansHans-Regular.otf", "Noto Sans SC", "Regular", fontSetting.fontSize, fontColor, bold, italic, outline, ref fontHandle, ref lineHeight, ref baseHeight) == false)
				return;
#endif

			int fallbackLineHeight = 0;
			int fallbackBaseHeight = 0;
			if (fallbackFontSetting != null &&
				FontPlugin.LoadFontFace(fallbackFontSetting.fileName, fallbackFontSetting.familyName, fallbackFontSetting.styleName, fallbackFontSetting.fontSize, fontColor, bold, italic, outline, ref fallbackFontHandle, ref fallbackLineHeight, ref fallbackBaseHeight) == false)
				return;

			if (outline)
			{
				FontPlugin.SetFontOutline(fontHandle, outlineWidth, outlineColor);
				if (fallbackFontSetting != null)
					FontPlugin.SetFontOutline(fallbackFontHandle, outlineWidth, outlineColor);
			}

			baseHeight -= fontSetting.baseOffsetY;
		}
	}

	public override void FreeMemory ()
	{
		base.FreeMemory ();

		pendingRenderChars.Clear();
		changedFontTextures.Clear();
	}

	public override bool ContainsCharacter(char ch)
	{
		// Make sure font has been initialized
		if (!started)
			Start();
		
		// Check in loaded characters.
		if (IsCharacterLoaded(ch))
			return true;

		// Check in unsupported characters
		if (unsupportedChars.BinarySearch(ch) >= 0)
			return false;

		// Check loaded characters
		if (charMap.ContainsKey(ch))
			return true;

		// Check if the character is supported by the font
		if (FontPlugin.HasCharacter(fontHandle, ch) == false && (fallbackFontHandle == FontPlugin.InvalidHandle || FontPlugin.HasCharacter(fallbackFontHandle, ch) == false))
		{
			unsupportedChars.Add(ch);
			unsupportedChars.Sort();
			return false;
		}
		
		// Force load the character
		GetSpriteChar(ch);
		return true;
	}

	public override SpriteChar AddSpriteChar(char ch)
	{
		if (!started)
			Start();

		SpriteChar spriteChar = new SpriteChar();
		spriteChar.waitForRenderData = true;
		
		// fallback的版本, 通过loadchar不能判断时候支持, 强制判断使用256以下的字符
		if ((fallbackFontHandle != FontPlugin.InvalidHandle && ch  < 256 && FontPlugin.LoadChar(fallbackFontHandle, ch)) || FontPlugin.LoadChar(fontHandle, ch))
		{
			const int charGap = 1;
			const int externEdge = 1;

			// Get glyph data.
			int width = 0;
			int height = 0;
			int xAdvance = 0;
			int xOffset = 0;
			int yOffset = 0;

			Color[] colors = FontPlugin.GetCharPixels(ref width, ref height, ref xAdvance, ref xOffset, ref yOffset);
			if (colors.Length == 0 && ch != ' ' && ch != '\u3000')//空格和全角空格
				Debug.LogError(string.Format("GetCharPixels failed : {0} Unicode: \\u{1}", ch, System.Convert.ToInt32(ch).ToString("X")));
			
			// yOffset returned from plugin is related to base line, convert to the value related to top line.
			yOffset -= baseHeight;
			
			// Extern char edge to avoid the error when sampling
			int spriteWidth = width + externEdge * 2;
			int spriteHeight = height + externEdge * 2;
			
			// Re-calculate line height in atlas
			curLineHeightInAtlas = curLineHeightInAtlas < spriteHeight ? spriteHeight : curLineHeightInAtlas;
			
			// Set char data.
			spriteChar.id = Convert.ToInt32(ch);
			spriteChar.xAdvance = xAdvance;

			if (spriteWidth > textureWidth || spriteHeight > textureHeight)
				Debug.LogError("charSize is greater than texture size.");
			
			// Calculate new character position on new line
			if (nextFontPixel.x + spriteWidth >= textureWidth)
			{
				nextFontPixel.x = 0;
				nextFontPixel.y += curLineHeightInAtlas + charGap;
			}

			// Get render texture
			if (nextFontPixel.y + curLineHeightInAtlas >= textureHeight)
			{
				// Step to next font texture
				++nextFontTextureIndex;

				// Rest the start position of the next font
				nextFontPixel = default(Vector2);
				
				// The height of the new character is line height
				curLineHeightInAtlas = spriteHeight;
			}

			if (nextFontTextureIndex >= fontMaterials.Count)
			{
				// Create a new material to contain the new character.
				Texture2D newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, false);

				// Clear texture to the same as font color
				Color defaultColor = outline == false ? fontColor : outlineColor;
				defaultColor.a = 0;
				for (int y = 0; y < textureHeight; ++y)
				{
					for (int x = 0; x < textureWidth; ++x)
					{
						newTexture.SetPixel(x, y, defaultColor);
					}
				}

				Material newMat = new Material(Shader.Find("Kod/UI/Transparent Color"));
				newMat.hideFlags = HideFlags.DontSave;
				newMat.SetTexture(textureProperty, newTexture);
				fontMaterials.Add(newMat);
			}

			// Grab the render texture for rendering
			Texture2D texture = fontMaterials[nextFontTextureIndex].GetTexture(textureProperty) as Texture2D;

			Rect rect = default(Rect);
			rect.x = nextFontPixel.x;
			rect.y = nextFontPixel.y;
			rect.width = spriteWidth;
			rect.height = spriteHeight;

			// Set pixel data
			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					texture.SetPixel((int)nextFontPixel.x + externEdge + x, 
					                 (int)nextFontPixel.y + externEdge + y, 
					                 colors[y * width + x]);
				}
			}

			// Apply operation cost a lot, so put all apply in the same frame together, and apply the changing in Update()
			if (changedFontTextures.Contains(texture) == false)
				changedFontTextures.Add(texture);
			
			// Increase the XY pixel for next character rendering
			nextFontPixel.x += spriteWidth + charGap;

			// Set SpriteChar data
			spriteChar.page = fontMaterials.Count - 1;
			spriteChar.xOffset = xOffset;
			spriteChar.yOffset = yOffset;
			spriteChar.UVs.x = rect.x / textureWidth;
			spriteChar.UVs.y = rect.y / textureHeight;
			spriteChar.UVs.xMax = rect.xMax / textureWidth;
			spriteChar.UVs.yMax = rect.yMax / textureHeight;
						
			// Validate SpriteChar
			spriteChar.waitForRenderData = false;

			// Add to pending render array for later process
//			pendingRenderChars.Add(spriteChar);
		}

		return spriteChar;
	}

	//---------------------------------------------------
	// Edit-time updating stuff
	//---------------------------------------------------

	// Uses the mirror object to validate and respond
	// to changes in our inspector.
	public virtual void DoMirror()
	{
		// Only run if we're not playing:
		if (Application.isPlaying)
			return;

		if (mirror == null)
		{
			mirror = new PluginDynamicFontMirror();
			mirror.Mirror(this);
		}

		mirror.Validate(this);

		// Compare our mirrored settings to the current settings
		// to see if something was changed:
		if (mirror.DidChange(this))
		{
			Init();
			mirror.Mirror(this);	// Update the mirror
		}
	}

#if (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9) && UNITY_EDITOR
	protected void Update()
	{
		DoMirror();
	}
#endif
	
	private void LateUpdate()
	{
		// Apply changed font texture
		if (changedFontTextures.Count != 0)
		{
			foreach (var texture in changedFontTextures)
				texture.Apply();

			changedFontTextures.Clear();
		}

		// Set pending character ready to render
		if (pendingRenderChars.Count != 0)
		{
			foreach (var spriteChar in pendingRenderChars)
				spriteChar.waitForRenderData = false;
			
			pendingRenderChars.Clear();
		}
	}
}

// Mirrors the editable settings of a control that affect
public class PluginDynamicFontMirror : SpirteDynamicFontMirror
{
	FontSetting fontSetting = new FontSetting();
	Color fontColor;
	bool bold;
	bool italic;
	bool outline;
	int outlineWidth;
	Color outlineColor;

	// Mirrors the specified control's settings
	public override void Mirror(SpriteDynamicFont s)
	{
		PluginDynamicFont f = s as PluginDynamicFont;
		
		base.Mirror(f);
		
		FontSetting fs = null;
		
#if UNITY_EDITOR && ENABLE_FORCE_USE_IOS_FONT
		fs = f.iosFont;
#elif UNITY_IOS
		fs = f.iosFont;
#elif UNITY_ANDROID
		fs = f.androidFont;
#endif
		fontSetting.Copy(fs);
		fontColor = f.fontColor;
		bold = f.bold;
		italic = f.italic;
		outline = f.outline;
		outlineWidth = f.outlineWidth;
		outlineColor = f.outlineColor;
	}

	// Returns true if any of the settings do not match:
	public override bool DidChange(SpriteDynamicFont s)
	{
		PluginDynamicFont f = s as PluginDynamicFont;

		if (base.DidChange(f))
			return true;
#if UNITY_EDITOR && ENABLE_FORCE_USE_IOS_FONT
		if (fontSetting.Equals(f.iosFont) == false)
			return true;
#elif UNITY_IOS
		if (fontSetting.Equals(f.iosFont) == false)
			return true;
#elif UNITY_ANDROID
		if (fontSetting.Equals(f.androidFont) == false)
			return true;
#endif
		if (fontColor != f.fontColor)
			return true;
		if (bold != f.bold)
			return true;
		if (italic != f.italic)
			return true;
		if (outline != f.outline)
			return true;
		if (outlineWidth != f.outlineWidth)
			return true;
		if (outlineColor != f.outlineColor)
			return true;

		return false;
	}

	public override void Validate(PluginDynamicFont f)
	{

	}
}
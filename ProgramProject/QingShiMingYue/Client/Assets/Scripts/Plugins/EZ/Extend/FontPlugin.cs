//#define ENABLE_FORCE_USE_IOS_FONT
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class FontPlugin
{
	public const int InvalidHandle = 0;

	public enum SupportedPlatform
	{
		IOS,
		ANDROID,
	}

	//IOS SystemVersion
	[Serializable]
	public class IOSVersion
	{
		public int major = 0;
		public int minor = 0;
		public int micro = 0;

		public IOSVersion() { }

		public IOSVersion(string versionString)
		{
			Set(versionString);
		}

		// for example versionString=8.0.1 major=8,minor=0,micro=1
		public void Set(string versionString)
		{
			string[] splited = versionString.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			if (splited.Length == 0)
				Debug.LogError("IOSVersion Convert Error.  Version String=" + versionString);

			if (splited.Length > 0)
				Parse(out major, splited[0]);
			if (splited.Length > 1)
				Parse(out minor, splited[1]);
			if (splited.Length > 2)
				Parse(out micro, splited[2]);
		}

		private void Parse(out int val, string toParse)
		{
			if (!int.TryParse(toParse, out val))
			{
				val = 0;
				Debug.LogError("IOSVersion Parse Error. string=" + toParse);
			}
		}

		#region Logic Operations

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (obj is IOSVersion)
			{
				IOSVersion other = obj as IOSVersion;
				return this.major == other.major && this.minor == other.minor && this.micro == other.micro;
			}

			return false;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		public override string ToString()
		{
			return new System.Text.StringBuilder().Append(major).Append('.').Append(minor).Append('.').Append(micro).ToString();
		}

		public static bool operator ==(IOSVersion a, IOSVersion b)
		{
			return a.major == b.major && a.minor == b.minor && a.micro == b.micro;
		}

		public static bool operator !=(IOSVersion a, IOSVersion b)
		{
			return a.major != b.major || a.minor != b.minor || a.micro != b.micro;
		}

		public static bool operator >(IOSVersion a, IOSVersion b)
		{
			if (a.major > b.major)
				return true;

			if(a.major==b.major)
			{
				if (a.minor > b.minor)
					return true;

				if (a.minor == b.minor && a.micro > b.micro)
					return true;
			}

			return false;
		}

		public static bool operator <(IOSVersion a, IOSVersion b)
		{
			if (a.major < b.major)
				return true;

			if (a.major == b.major)
			{
				if (a.minor < b.minor)
					return true;

				if (a.minor == b.minor && a.micro < b.micro)
					return true;
			}

			return false;
		}

		public static bool operator >=(IOSVersion a, IOSVersion b)
		{
			if (a.major > b.major)
				return true;

			if (a.major == b.major)
			{
				if (a.minor > b.minor)
					return true;

				if (a.minor == b.minor && a.micro >= b.micro)
					return true;
			}

			return false;
		}

		public static bool operator <=(IOSVersion a, IOSVersion b)
		{
			if (a.major < b.major)
				return true;

			if (a.major == b.major)
			{
				if (a.minor < b.minor)
					return true;

				if (a.minor == b.minor && a.micro <= b.micro)
					return true;
			}

			return false;
		}

		#endregion
	}

#if UNITY_ANDROID

#if UNITY_EDITOR
#if ENABLE_FORCE_USE_IOS_FONT
	private const string FontsDir = @"Dependencies/Fonts/IOS/";
#else
	private const string FontsDir = @"Dependencies/Fonts/Android/";
#endif
#else // UNITY_EDITOR
	private const string Plugin = "FontPlugin-android";
	private const string FontsDir = @"/system/fonts/";
#endif // UNITY_EDITOR

#elif UNITY_IPHONE

#if UNITY_EDITOR
	private const string FontsDir = @"Dependencies/Fonts/IOS/";
#else // UNITY_EDITOR
	private const string Plugin = "__Internal";
	private const string FontsDir = @"/System/Library/Fonts/Cache/";
	private const string FontsDir_IOS_8_2 = @"/System/Library/Fonts/Core/";
	public static readonly IOSVersion ios_8_2 = new IOSVersion("8.2.0");
	public static readonly IOSVersion iosVersion = GetIOSVersion();
#endif // UNITY_EDITOR

#else

#if !UNITY_EDITOR
	private const string Plugin = "unknown";
#endif // UNITY_EDITOR

	private const string FontsDir = "";
#endif

#if UNITY_EDITOR
	private const string Plugin = "FontPlugin";
	
	public class FontDef
	{
		public string fileName = "";
		public string familyName = "";
		public string styleName = "";
		
		public FontDef(string fileName, string faceName)
		{
			string[] names = faceName.Split('-');
			
			this.fileName = fileName;
			this.familyName = names[0];
			this.styleName = names[1];
		}
	}

	public static Dictionary<SupportedPlatform, Dictionary<string, FontDef>> supportedFontList = new Dictionary<SupportedPlatform, Dictionary<string, FontDef>>();

	public static string GetSupportedFontName(SupportedPlatform platform, string fileName, string familyName, string styleName)
	{
		if (supportedFontList.ContainsKey(platform) == false)
			return "";
		
		string faceName = familyName + "-" + styleName;
		if (supportedFontList[platform].ContainsKey(faceName))
			return faceName;
		else
			return "";
	}

	private static void InitSupportedFontList()
	{
		{
			string[,] fontList = new string[,] { 
				{"_H_AmericanTypewriter.ttc","American Typewriter-Regular,American Typewriter-Bold,American Typewriter-Condensed Bold,American Typewriter-Light,American Typewriter-Condensed,American Typewriter-Condensed Light"}, 
				{"_H_Baskerville.ttc","Baskerville-Regular,Baskerville-Bold,Baskerville-Italic,Baskerville-SemiBold Italic,Baskerville-Bold Italic,Baskerville-SemiBold"}, 
				{"_H_ChalkboardSE.ttc","Chalkboard SE-Light,Chalkboard SE-Regular,Chalkboard SE-Bold"}, 
				{"_H_Cochin.ttc","Cochin-Regular,Cochin-Bold,Cochin-Italic,Cochin-BoldItalic"}, 
				{"_H_Courier.ttc","Courier-Regular,Courier-Bold,Courier-Oblique,Courier-Bold Oblique"}, 
				{"_H_Futura.ttc","Futura-Medium,Futura-Medium Italic,Futura-Condensed Medium,Futura-Condensed ExtraBold"}, 
				{"_H_Helvetica.ttc","Helvetica-Regular,Helvetica-Bold,Helvetica-Oblique,Helvetica-Bold Oblique,Helvetica-Light,Helvetica-Light Oblique,.Helvetica Light-Regular,.Helvetica Light-Oblique"}, 
				{"_H_HelveticaNeue.ttc","Helvetica Neue-Regular,Helvetica Neue-Bold,Helvetica Neue-Italic,Helvetica Neue-Bold Italic,.Helvetica NeueUI-Regular,.Helvetica NeueUI-Bold,.Helvetica NeueUI-Italic,.Helvetica NeueUI-Bold Italic"}, 
				{"_H_HelveticaNeueExtras.ttc","Helvetica Neue-Light,Helvetica Neue-Light Italic,Helvetica Neue-UltraLight,Helvetica Neue-UltraLight Italic,Helvetica Neue-Condensed Black,Helvetica Neue-Condensed Bold,Helvetica Neue-Medium,.Helvetica Neue ATV-Regular"}, 
				{"_H_MarkerFeltThin.ttf","Marker Felt-Thin"}, 
				{"_H_MarkerFeltWide.ttf","Marker Felt-Wide"}, 
				{"_H_Noteworthy.ttc","Noteworthy-Light,Noteworthy-Bold"}, 
				{"_H_Palatino.ttc","Palatino-Regular,Palatino-Italic,Palatino-Bold,Palatino-Bold Italic"}, 
				{"_H_SnellRoundhand.ttc","Snell Roundhand-Regular,Snell Roundhand-Bold,Snell Roundhand-Black"}, 
				{"AppleColorEmoji.ttf","Apple Color Emoji-Regular"}, 
				{"AppleGothic.ttf","AppleGothic-Regular"}, 
				{"Arial.ttf","Arial-Regular"}, 
				{"ArialBold.ttf","Arial-Bold"}, 
				{"ArialBoldItalic.ttf","Arial-Bold Italic"}, 
				{"ArialHB.ttf","Arial Hebrew-Regular"}, 
				{"ArialHBBold.ttf","Arial Hebrew-Bold"}, 
				{"ArialItalic.ttf","Arial-Italic"}, 
				{"ArialRoundedMTBold.ttf","Arial Rounded MT Bold-Regular"}, 
				{"BanglaSangamMN.ttc","Bangla Sangam MN-Regular,Bangla Sangam MN-Bold"}, 
				{"CourierNew.ttf","Courier New-Regular"}, 
				{"CourierNewBold.ttf","Courier New-Bold"}, 
				{"CourierNewBoldItalic.ttf","Courier New-Bold Italic"}, 
				{"CourierNewItalic.ttf","Courier New-Italic"}, 
				{"DB_LCD_Temp-Black.ttf","DB LCD Temp-Black"}, 
				{"DevanagariSangamMN.ttc","Devanagari Sangam MN-Regular,Devanagari Sangam MN-Bold"}, 
				{"Fallback.ttf",".PhoneFallback-Regular"}, 
				{"GeezaPro.ttf","Geeza Pro-Regular"}, 
				{"GeezaProBold.ttf","Geeza Pro-Bold"}, 
				{"Georgia.ttf","Georgia-Regular"}, 
				{"GeorgiaBold.ttf","Georgia-Bold"}, 
				{"GeorgiaBoldItalic.ttf","Georgia-Bold Italic"}, 
				{"GeorgiaItalic.ttf","Georgia-Italic"}, 
				{"GujaratiSangamMN.ttc","Gujarati Sangam MN-Regular,Gujarati Sangam MN-Bold"}, 
				{"GurmukhiMN.ttc","Gurmukhi MN-Regular,Gurmukhi MN-Bold"}, 
				{"HiraginoKakuGothicProNW3.otf","Hiragino Kaku Gothic ProN-W3"}, 
				{"HiraginoKakuGothicProNW6.otf","Hiragino Kaku Gothic ProN-W6"}, 
				{"HKGPW3UI.ttf",".HKGPW3UI-Regular"}, 
				{"Kailasa.ttc","Kailasa-Bold,Kailasa-Regular"}, 
				{"KannadaSangamMN.ttc","Kannada Sangam MN-Regular,Kannada Sangam MN-Bold"}, 
				{"LastResort.ttf",".LastResort-Regular"}, 
				{"LockClock.ttf",".Lock Clock-Light"}, 
				{"MalayalamSangamMN.ttc","Malayalam Sangam MN-Regular,Malayalam Sangam MN-Bold"}, 
				{"OriyaSangamMN.ttc","Oriya Sangam MN-Regular,Oriya Sangam MN-Bold"}, 
				{"PhoneKeyCaps.ttf",".PhoneKeyCaps-Regular"}, 
				{"PhoneKeyCapsTwo.ttf",".PhoneKeyCapsTwo-Regular"}, 
				{"PhonepadTwo.ttf",".PhonepadTwo-Regular"}, 
				{"SinhalaSangamMN.ttc","Sinhala Sangam MN-Regular,Sinhala Sangam MN-Bold"}, 
				{"STHeiti-Light.ttc","Heiti TC-Light,Heiti SC-Light,Heiti K-Light,Heiti J-Light"}, 
				{"STHeiti-Medium.ttc","Heiti TC-Medium,Heiti SC-Medium,Heiti K-Medium,Heiti J-Medium"}, 
				{"TamilSangamMN.ttc","Tamil Sangam MN-Regular,Tamil Sangam MN-Bold"}, 
				{"TeluguSangamMN.ttc","Telugu Sangam MN-Regular,Telugu Sangam MN-Bold"}, 
				{"Thonburi.ttf","Thonburi-Regular"}, 
				{"ThonburiBold.ttf","Thonburi-Bold"}, 
				{"TimesNewRoman.ttf","Times New Roman-Regular"}, 
				{"TimesNewRomanBold.ttf","Times New Roman-Bold"}, 
				{"TimesNewRomanBoldItalic.ttf","Times New Roman-Bold Italic"}, 
				{"TimesNewRomanItalic.ttf","Times New Roman-Italic"}, 
				{"TrebuchetMS.ttf","Trebuchet MS-Regular"}, 
				{"TrebuchetMSBold.ttf","Trebuchet MS-Bold"}, 
				{"TrebuchetMSBoldItalic.ttf","Trebuchet MS-Bold Italic"}, 
				{"TrebuchetMSItalic.ttf","Trebuchet MS-Italic"}, 
				{"Verdana.ttf","Verdana-Regular"}, 
				{"VerdanaBold.ttf","Verdana-Bold"}, 
				{"VerdanaBoldItalic.ttf","Verdana-Bold Italic"}, 
				{"VerdanaItalic.ttf","Verdana-Italic"}, 
				{"Zapfino.ttf","Zapfino-Regular"}, 
			};

			AddFontDict(SupportedPlatform.IOS, fontList);
		}

		{
			string[,] fontList = new string[,] { 
				{"ARDJ-KK.ttf","AR DJ-KK-Regular"}, 
				{"Clockopia.ttf","Clockopia-Regular"}, 
				{"DroidSans.ttf","Droid Sans-Regular"}, 
				{"DroidSansArabic.ttf","Droid Sans Arabic-Regular"}, 
				{"DroidSans-Bold.ttf","Droid Sans-Bold"}, 
				{"DroidSansFallback.ttf","Droid Sans Fallback-Regular"}, 
				{"DroidSansHebrew.ttf","Droid Sans Hebrew-Regular"}, 
				{"DroidSansMono.ttf","Droid Sans Mono-Regular"}, 
				{"DroidSansThai.ttf","Droid Sans Thai-Regular"}, 
				{"DroidSerif-Bold.ttf","Droid Serif-Bold"}, 
				{"DroidSerif-BoldItalic.ttf","Droid Serif-Bold Italic"}, 
				{"DroidSerif-Italic.ttf","Droid Serif-Italic"}, 
				{"DroidSerif-Regular.ttf","Droid Serif-Regular"}, 
				{"gcsh00d-hkscs.ttf","AR CrystalheiHKSCS DB-Regular"}, 
				{"HelveticaNeue_LT_35_Thin.ttf","Helvetica Neue LT-35 Thin"}, 
				{"ucsh00d_c.ttf","AR Crystalhei DB-Regular"} };

			AddFontDict(SupportedPlatform.ANDROID, fontList);		  
		}
	}

	private static void AddFontDict(SupportedPlatform platformType, string[,] fontList)
	{
		Dictionary<string, FontDef> fontDict = new Dictionary<string, FontDef>();
		for (int i = 0; i < fontList.GetLength(0); ++i)
		{
			string fileName = fontList[i, 0];
			string[] faceNames = fontList[i, 1].Split(',');
			for (int j = 0; j < faceNames.Length; ++j)
			{
				fontDict.Add(faceNames[j], new FontDef(fileName, faceNames[j]));
			}
		}

		supportedFontList.Add(platformType, fontDict);
	}
	
	public static void EnumFonsInFontDir()
	{
		string[] files = System.IO.Directory.GetFiles(FontsDir);
		List<string> fileNameList = new List<string>();
		foreach (var file in files)
			fileNameList.Add(System.IO.Path.GetFileName(file));

		PrintFontList(fileNameList);
	}

	private static void PrintFontList(List<string> fontList) 
	{
		string log = "";
		foreach (var file in fontList)
		{
			int handle = 0;
			int lineHeight = 0;
			int baseHeight = 0;

			if (LoadFontFace(file, 0, 12, Color.white, false, false, false, ref handle, ref lineHeight, ref baseHeight))
			{
				string faceNames = "";

				int faceCount = 0;
				GetFontFaceCount(handle, ref faceCount);

				faceNames += GetFaceFamilyName(handle) + "-" + GetFaceStyleName(handle);
				FreeFontFace(handle);

				for (int i = 1; i < faceCount; ++i)
				{
					if (LoadFontFace(file, i, 12, Color.white, false, false, false, ref handle, ref lineHeight, ref baseHeight))
					{
						faceNames += "," + GetFaceFamilyName(handle) + "-" + GetFaceStyleName(handle);
						FreeFontFace(handle);
					}
				}

				log += string.Format("{{\"{0}\",\"{1}\"}}, \n", file, faceNames);
			}
		}

		Debug.Log(log);
	}

	[UnityEditor.MenuItem("Tools/Font/Print Fonts")]
	public static void PrintFonts()
	{
		FontPlugin.EnumFonsInFontDir();
	}
	
#if !UNITY_WEBPLAYER
	[UnityEditor.MenuItem("Tools/Font/FontTest")]
	public static void TestFont()
	{
		const int TextureSize = 128;

		string fileName = "STHeiti-Light.ttc";
		int faceIndex = 1;
		int fontSize = 20;

		int fontHandle = 0;
		int baseHeight = 0;
		int lineHeight = 0;
		if (LoadFontFace(fileName, faceIndex, fontSize, Color.white, false, false, false, ref fontHandle, ref lineHeight, ref baseHeight) == false)
			return;

		int baseDescenderHeight = 10;

		Texture2D texture = new Texture2D(TextureSize, TextureSize);
		for (int y = 0; y < TextureSize; ++y)
		{
			for (int x = 0; x < TextureSize; ++x)
			{
				texture.SetPixel(x,y,Color.gray);
			}			
		}		

		// Draw lines
		for (int x = 0; x < TextureSize; ++x)
		{
			// Ascender
			texture.SetPixel(x, baseDescenderHeight + lineHeight, Color.blue);
			// Baseline
			texture.SetPixel(x, baseDescenderHeight + lineHeight - baseHeight, Color.green);
			// Descender
			texture.SetPixel(x, baseDescenderHeight + 0, Color.red);
		}		

		string testString = "1.abc����";

		int totalxAdvande = 0;
		foreach (var c in testString)
		{
			if (LoadChar(fontHandle, c) == false)
				continue;
			
			int width = 0;
			int height = 0;
			int xAdvance = 0;
			int xOffset = 0;
			int yOffset = 0;
		
			Color[] pixels = GetCharPixels(ref width, ref height, ref xAdvance, ref xOffset, ref yOffset);
			Debug.Log("" + c + " " + height + " " + yOffset);

			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
				{
					Color color = new Color(pixels[y * width + x].a, pixels[y * width + x].a, pixels[y * width + x].a, 1.0f);
					texture.SetPixel(totalxAdvande + x + xOffset, baseDescenderHeight + lineHeight - baseHeight + y + yOffset - height, color);
				}
			}

			totalxAdvande += xAdvance;
		}

		texture.Apply();
		byte[] bytes = texture.EncodeToPNG();
		System.IO.File.WriteAllBytes(@"123.png", bytes);
	}

	[UnityEditor.MenuItem("Tools/Font/SaveSelectTexture")]
	public static void SaveSelectTexture()
	{
		if (UnityEditor.Selection.activeObject == null)
			return;

		Texture2D texture = UnityEditor.Selection.activeObject as Texture2D;
		byte[] bytes = texture.EncodeToPNG();
		System.IO.File.WriteAllBytes(@"FontTexture.png", bytes);
		Debug.Log("Save to FontTexture.png");
	}

	[UnityEditor.MenuItem("Tools/Font/SaveSelectTexture", true)]
	public static bool ValidateSaveSelectTexture()
	{
		return UnityEditor.Selection.activeObject != null && UnityEditor.Selection.activeObject is Texture2D;
	}
#endif
	
#endif

	[DllImport(Plugin)]
	private static extern void UnityCall_Font_SetLogFun([MarshalAs(UnmanagedType.FunctionPtr)] LogCallbackDelegate lgFun);

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_InitFontLib();

	[DllImport(Plugin)]
	private static extern void UnityCall_Font_FreeFontLib();

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_LoadFace(string face, int faceIndex, int bolden, int italic, int outline, ref int fontHandle);

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_LoadFaceWithName(string face, string familyName, string styleName, int bolden, int italic, int outline, ref int fontHandle);

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_GetFaceCount( int ftHdl, ref int count );

	[DllImport(Plugin)]
	private static extern IntPtr UnityCall_Font_GetFaceFamilyName( int ftHdl);

	[DllImport(Plugin)]
	private static extern IntPtr UnityCall_Font_GetFaceStyleName( int ftHdl);

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_FreeFace(int fontHandle);

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_SetCharSize(int fontHandle, int charWidth, int charHeight, int hrzRsl, int vrtRsl);

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_SetPixelSize(int fontHandle, int width, int height);

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_SetCharColor(int fontHandle, byte r, byte g, byte b, byte a);

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_SetCharOutline(int fontHandle, byte r, byte g, byte b, byte a, float width);

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_GetFontHeight(int fontHandle, ref int height);

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_GetFontAscender(int fontHandle, ref int ascender);

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_HasCharacter(int fontHandle, char chr, ref bool tf);

	[DllImport(Plugin)]
	private static extern int UnityCall_Font_LoadChar(int fontHandle, char chr);

	[DllImport(Plugin)]
	private static extern IntPtr UnityCall_Font_GetCharPxlBuf(ref int width, ref int height, ref int xAdvance, ref int advanceY, ref int xOffset, ref int yOffset);

	private static bool initialized = false;
	
	static FontPlugin()
	{
		Initialize();
#if UNITY_EDITOR
		InitSupportedFontList();
#endif
	}
	
	[System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	private delegate void LogCallbackDelegate(int logType, string log);
	
	private void LogCallback(int logType, string log)
	{
		if (logType == 1)
			Debug.LogError(log);
		else
			Debug.Log(log);
	}

	protected static bool Initialize()
	{
		if (initialized)
			return true;

		initialized = true;		

		// Initialize free type
		int result = UnityCall_Font_InitFontLib();
		if (result != 0)
		{
			Debug.LogError(string.Format("InitFontLib failed, error:{0}", result));
			return false;
		}

		return true;
	}

	public static void Free()
	{
		UnityCall_Font_FreeFontLib();
	}

	/// <summary>
	/// Load a face
	/// </summary>
	/// <param name="faceNames">Face names, it will used the first valid font in the array</param>
	/// <param name="size">Font size in pixel unit</param>
	/// <param name="handle">Retrieve the font handle if success</param>
	/// <param name="height">Retrieve the height of the font if success</param>
	/// <param name="baseHeight">Retrieve the height of base line if success</param>
	/// <returns>Return true if success</returns>
	public static bool LoadFontFace(string fileName, int faceIndex, int size, Color fontColor, 
		bool bolden, bool italic, bool outline, ref int handle, ref int lineHeight, ref int baseHeight)
	{
		fileName = FontsDir + fileName;
		//Debug.Log("Load font : " + fileName + "," + faceIndex);
		int result = UnityCall_Font_LoadFace(fileName, faceIndex, bolden ? 1 : 0, italic ? 1 : 0, outline ? 1 : 0, ref handle);
		if (result != 0)
		{
			Debug.LogError(string.Format("UnityCall_Font_LoadFace failed, error:{0}, {1}", result, fileName));
			return false;
		}
		
		return LoadFontFace(handle, size, fontColor, bolden, italic, outline, ref lineHeight, ref baseHeight);
	}
		
	public static bool LoadFontFace(string fileName, string familyName, string styleName, int size, Color fontColor, 
		bool bolden, bool italic, bool outline, ref int handle, ref int lineHeight, ref int baseHeight)
	{
#if UNITY_IPHONE&&!UNITY_EDITOR
		if (iosVersion >= ios_8_2)
			fileName = FontsDir_IOS_8_2 + fileName;
		else			
			fileName = FontsDir + fileName;
#else
		// Load font	
		fileName = FontsDir + fileName;
#endif

		//Debug.Log("Load font : " + fileName + "," + familyName + "," + styleName);
		int result = UnityCall_Font_LoadFaceWithName(fileName, familyName, styleName, bolden ? 1 : 0, italic ? 1 : 0, outline ? 1 : 0, ref handle);
		if (result != 0)
		{
			Debug.LogError(string.Format("UnityCall_Font_LoadFace failed, error:{0}, {1}", result, fileName));
			return false;
		}

		return LoadFontFace(handle, size, fontColor, bolden, italic, outline, ref lineHeight, ref baseHeight);
	}
	
	private static bool LoadFontFace(int handle, int size, Color fontColor, 
		bool bolden, bool italic, bool outline, ref int lineHeight, ref int baseHeight)
	{
//		Debug.Log(GetFaceFamilyName(handle) + "-" + GetFaceStyleName(handle));
		
		// Set font size
		int result = UnityCall_Font_SetPixelSize(handle, 0, size);
		if (result != 0)
		{
			Debug.LogError(string.Format("UnityCall_Font_SetPixelSize failed, error:{0}", result));
			return false;
		}

		// Set font size
		Color32 color32 = fontColor;
		result = UnityCall_Font_SetCharColor(handle, color32.r, color32.g, color32.b, 0);
		if (result != 0)
		{
			Debug.LogError(string.Format("UnityCall_Font_SetCharColor failed, error:{0}", result));
			return false;
		}
		
		// Get font height
		result = UnityCall_Font_GetFontHeight(handle, ref lineHeight);
		if (result != 0)
		{
			Debug.LogError(string.Format("UnityCall_Font_GetFontHeight failed, error:{0}", result));
			return false;
		}

		// Get font base line height
		result = UnityCall_Font_GetFontAscender(handle, ref baseHeight);
		if (result != 0)
		{
			Debug.LogError(string.Format("UnityCall_Font_GetFontAscender failed, error:{0}", result));
			return false;
		}

		return true;
	}

	/// <summary>
	/// Set font outline
	/// </summary>
	/// <param name="handle">Font handle</param>
	/// <param name="outlineWidth">Width of outline</param>
	/// <param name="outlineColor">Color of outle</param>
	/// <returns>Return true if success</returns>
	public static bool SetFontOutline(int handle, int outlineWidth, Color outlineColor)
	{
		Color32 color32 = outlineColor;
		int result = UnityCall_Font_SetCharOutline(handle, color32.r, color32.g, color32.b, 0, outlineWidth);
		if (result != 0)
		{
			Debug.LogError(string.Format("UnityCall_Font_SetCharOutline failed, error:{0}", result));
			return false;
		}

		return true; 
	}

	public static bool GetFontFaceCount(int handle, ref int count)
	{
		int result = UnityCall_Font_GetFaceCount(handle, ref count);
		if (result != 0)
		{
			Debug.LogError(string.Format("GetFontFaceCount failed, error:{0}", result));
			return false;
		}

		return true; 
	}

	public static string GetFaceFamilyName(int handle)
	{
		return Marshal.PtrToStringAnsi(UnityCall_Font_GetFaceFamilyName(handle));
	}

	public static string GetFaceStyleName(int handle)
	{
		return Marshal.PtrToStringAnsi(UnityCall_Font_GetFaceStyleName(handle));
	}

	/// <summary>
	/// Free a font face
	/// </summary>
	/// <param name="handle">The handle of the font</param>
	/// <returns>Return true if success</returns>
	public static bool FreeFontFace(int handle)
	{
		int result = UnityCall_Font_FreeFace(handle);
		if (result != 0)
		{
			Debug.LogError(string.Format("FreeFace failed, error:{0}", result));
			return false;
		}

		return true;
	}

	/// <summary>
	/// Check if a character is valid in the give font
	/// </summary>
	/// <param name="handle">The handle of the font</param>
	/// <param name="chr">The character to be checked</param>
	/// <returns>Return true if success</returns>
	public static bool HasCharacter(int handle, char chr)
	{
		bool tf = false;
		int result = UnityCall_Font_HasCharacter(handle, chr, ref tf);
		if (result != 0)
		{
			Debug.LogError(string.Format("UnityCall_Font_HasCharacter, error:{0}", result));
			return false;
		}

		return tf;
	}

	/// <summary>
	/// Load a character, if success use GetCharPixels the retrieve data
	/// </summary>
	/// <param name="handle">File handle</param>
	/// <param name="chr">Character to be loaded</param>
	/// <param name="bolden">If a bold character</param>
	/// <param name="italic">If a italic character</param>
	/// <param name="outline">If a outline character</param>
	/// <returns>Return true if success</returns>
	public static bool LoadChar(int handle, char chr)
	{
		int result = UnityCall_Font_LoadChar(handle, chr);
		if (result != 0)
		{
			Debug.LogError(string.Format("LoadChar failed, char:{0},{1} error:{2}", chr, System.Convert.ToInt32(chr), result));
			return false;
		}

		return true;
	}

	/// <summary>
	/// Get character data after it has been loaded
	/// </summary>
	/// <returns>Return true if success</returns>
	public static Color[] GetCharPixels(ref int width, ref int height, ref int xAdvance, ref int xOffset, ref int yOffset)
	{
		int advanceY = 0;
		IntPtr pixelsBuf = UnityCall_Font_GetCharPxlBuf(ref width, ref height, ref xAdvance, ref advanceY, ref xOffset, ref yOffset);
		if (pixelsBuf == IntPtr.Zero)
		{
			Debug.LogError("GetCharPxlBuf Error");

			width = 0;
			height = 0;
			xAdvance = 0;
			xOffset = 0;
			yOffset = 0;
			return new Color[0];
		}

		Color[] colors = new Color[width * height];
		for (int i = 0; i < width * height; ++i)
		{
			byte r = (byte)Marshal.ReadByte(pixelsBuf, i * 4 + 0);
			byte g = (byte)Marshal.ReadByte(pixelsBuf, i * 4 + 1);
			byte b = (byte)Marshal.ReadByte(pixelsBuf, i * 4 + 2);
			byte a = (byte)Marshal.ReadByte(pixelsBuf, i * 4 + 3);
			colors[i] = new Color32(r, g, b, a);
		}

		return colors;
	}

#if UNITY_IPHONE
	public static IOSVersion GetIOSVersion()
	{
#if UNITY_EDITOR
		string sysVer = KodGames.ExternalCall.DevicePlugin.GetSystemVersion();
		if (string.IsNullOrEmpty(sysVer))
			sysVer = "0.0.0";
		return new IOSVersion(sysVer);
#else
		return new IOSVersion(KodGames.ExternalCall.DevicePlugin.GetSystemVersion());
#endif
	}
#endif

}

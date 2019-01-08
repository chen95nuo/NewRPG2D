////----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Simple script that lets you localize a UIWidget.
/// </summary>

[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/UI/Localize")]
public class UILocalize : MonoBehaviour
{
	/// <summary>
	/// Localization key.
	/// </summary>
//	void OnLevelWasLoaded(int level) {
//		if (level != 15 && level != 16){
//			Localize();
////			Awake();
////			Start();
//		}
//		
//	}

	public string key;
	public List<string> keys = new List<string>(){""};
	public List<string> Keys {
		set{
			keys = value;
			Localize ();
		}
		get {
			return keys;
		}
	}
	
	string mLanguage;
	bool mStarted = false;

	/// <summary>
	/// This function is called by the Localization manager via a broadcast SendMessage.
	/// </summary>

	void OnLocalize (Localization loc) { if (mLanguage != loc.currentLanguage) Localize(); }

	/// <summary>
	/// Localize the widget on enable, but only if it has been started already.
	/// </summary>

	void OnEnable () { if (mStarted && Localization.instance != null) Localize(); }

	/// <summary>
	/// Localize the widget on start.
	/// </summary>

	void Awake ()
	{
		mStarted = true;
		if (Localization.instance != null) Localize();
	}
	
	void Start(){
		Localize();
//		UIWidget w = GetComponent<UIWidget>();
//		UILabel lbl = w as UILabel;
//		if(lbl != null){
//			lbl.enabled = false;
//			lbl.enabled = true;
//		}
//		gameObject.active = false;
//		gameObject.active = true;
	}
	/// <summary>
	/// Force-localize the widget.
	/// </summary>
	
	private string oldStr;
	public void Localize ()
	{
		Localization loc = Localization.instance;
		UIWidget w = GetComponent<UIWidget>();
		UILabel lbl = w as UILabel;
		UISprite sp = w as UISprite;

		// If no localization key has been specified, use the label's text as the key
		if (string.IsNullOrEmpty(mLanguage) && string.IsNullOrEmpty(key) && lbl != null) key = lbl.text;

		// If we still don't have a key, leave the value as blank
		string val = "";
//		print (lbl);
		if (lbl != null)
		{
			oldStr = lbl.text;
			lbl.text = "";
			for(int i=0; i<keys.Count ; i++){
				val = string.IsNullOrEmpty(keys[i]) ? oldStr : loc.Get(keys[i]);
				// If this is a label used by input, we should localize its default value instead
//				UIInput input = NGUITools.FindInParents<UIInput>(lbl.gameObject);
//				if (input != null && input.label == lbl) input.defaultText = val;
//				else 
					lbl.text += val;
			}
		}else 
		if (sp != null)
		{
			sp.spriteName = val;
			sp.MakePixelPerfect();
		}

		mLanguage = loc.currentLanguage;
	}
}

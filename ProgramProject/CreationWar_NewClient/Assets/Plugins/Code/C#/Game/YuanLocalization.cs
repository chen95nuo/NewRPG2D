using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YuanLocalization : MonoBehaviour {
	
	public TextAsset[] text;
	public string language=string.Empty;
	public Dictionary<string,string> myDic;
	public string[] myStr;
	void Awake()
	{
		language=PlayerPrefs.GetString("Language");
		if(text!=null&&text.Length>0)
		{
			foreach(TextAsset item in text)
			{
				if(item.name==language)
				{
					ByteReader br=new ByteReader(item);
					myDic=br.ReadDictionary ();
					myStr=new string[myDic.Count];
					myDic.Values.CopyTo (myStr,0);
				}
			}
		}
	}

}

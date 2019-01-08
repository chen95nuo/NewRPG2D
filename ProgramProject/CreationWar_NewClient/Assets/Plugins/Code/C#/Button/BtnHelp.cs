using UnityEngine;
using System.Collections;

public class BtnHelp : MonoBehaviour {

	
	public UIToggle myCkb;
	public UILabel title;
	public yuan.YuanMemoryDB.YuanRow yr;
	public UILabel info;
	
	
	public void OnClick()
	{
		myCkb.isChecked=true;
		info.text=yr["HelpInfo"].YuanColumnText;
	}
}

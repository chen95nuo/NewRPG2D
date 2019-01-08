using UnityEngine;
using System.Collections;

public class SpriteForGamble : SpriteForBenefits {

	public UISprite picSelect;
	public UILabel lblIsGet;
	
	private int myLevel;
	public int MyLevel {
		get {
			return this.myLevel;
		}
		set {
			myLevel = value;
			SetLevelPic(myLevel);
		}
	}
	
	private void SetLevelPic(int mLevel)
	{
		switch(mLevel)
		{
			case 1:
			
			picLevel.spriteName="yanse1";
				break;
			case 2:
			picLevel.spriteName="yanse2";
				break;
			case 3:
			picLevel.spriteName="yanse3";
				break;
			case 4:
			picLevel.spriteName="yanse4";
				break;
			case 5:
			picLevel.spriteName="yanse5";
				break;
		}
	}

	void OnClick()
	{
		if(!string.IsNullOrEmpty(this.itemID))
		{
			PanelStatic.StaticIteminfo.SetActiveRecursively (true);
			PanelStatic.StaticIteminfo.transform.localPosition=new Vector3(-0.2875011f,100.1449f,-5.680656f);
			//infoBar.transform.Translate (infoBar.transform.up);
			PanelStatic.StaticIteminfo.SendMessage("SetItemID",this.itemID,SendMessageOptions.DontRequireReceiver);
		}
	}
	
}

public class InfoForGamble
{
    public string picName;
    public string itemID;
}

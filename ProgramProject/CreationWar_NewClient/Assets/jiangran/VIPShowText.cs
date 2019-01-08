using UnityEngine;
using System.Collections;

public class VIPShowText : MonoBehaviour {
	public UILabel VIPText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		ShowText(int.Parse(BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText));
	}

	void ShowText(int VIPNumb){
		int servingMoney = int.Parse(BtnGameManager.yt.Rows[0]["ServingMoney"].YuanColumnText);
		switch(VIPNumb){
		case 0 :
			VIPText.text = string.Format("{0}{1}{2}",StaticLoc.Loc.Get("info1036"),(10-servingMoney).ToString(),StaticLoc.Loc.Get("buttons510"));
				break;
		case 1 :
			VIPText.text = StaticLoc.Loc.Get("info1036")+(50-servingMoney).ToString()+StaticLoc.Loc.Get("info1027");
			break;
		case 2 :
			VIPText.text = StaticLoc.Loc.Get("info1036")+(150-servingMoney).ToString()+StaticLoc.Loc.Get("info1028");
			break;
		case 3 :
			VIPText.text = StaticLoc.Loc.Get("info1036")+(500-servingMoney).ToString()+StaticLoc.Loc.Get("info1029");
			break;
		case 4 :
			VIPText.text = StaticLoc.Loc.Get("info1036")+(1500-servingMoney).ToString()+StaticLoc.Loc.Get("info1030");
			break;
		case 5 :
			VIPText.text = StaticLoc.Loc.Get("info1036")+(2500-servingMoney).ToString()+StaticLoc.Loc.Get("info1031");
			break;
		case 6 :
			VIPText.text = StaticLoc.Loc.Get("info1036")+(5000-servingMoney).ToString()+StaticLoc.Loc.Get("info1032");
			break;
		case 7 :
			VIPText.text = StaticLoc.Loc.Get("info1036")+(10000-servingMoney).ToString()+StaticLoc.Loc.Get("info1033");
			break;
		case 8 :
			VIPText.text = StaticLoc.Loc.Get("info1036")+(20000-servingMoney).ToString()+StaticLoc.Loc.Get("info1034");
			break;
		case 9 :
			VIPText.text = "";
			break;
		}
	}
}

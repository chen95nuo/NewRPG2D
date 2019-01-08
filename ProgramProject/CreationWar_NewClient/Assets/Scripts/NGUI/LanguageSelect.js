#pragma strict

var Loc : Localization;
var MainFont : UIFont;
var FontMaterial : Material[];
var FontImport : TextAsset[];
var bool : boolean = false;
function Start(){
	var str : String = "";
	str = PlayerPrefs.GetString("Language", "CH");
	//print(PlayerPrefs.GetString("Language"));
	SelectChinese();
	if(str == "CH"){
		//SelectChinese();
	}else
	if(str == "CR"){
		//SelectKorea();
	}else
	if(str == "CHT"){
		//SelectChineseT();
	}
	bool = true;
//	Loc.currentLanguage = str;
}

var btnm : BtnManager;
var war : Warnings;
function SelectChinese(){
//	MainFont.material = FontMaterial[0];
//	myBMFontReader.Load(MainFont.bmFont, NGUITools.GetHierarchy(MainFont.gameObject), FontImport[0].bytes);
	Loc.currentLanguage = "CH";
	btnm.SelectBtnSize();
	if(bool)
	war.warningAllEnter.Show(Loc.Get("buttons620") , Loc.Get("info599")); 
//	PlayerPrefs.SetString("Language", "CH");
}

function SelectKorea(){
	MainFont.material = FontMaterial[1];
	myBMFontReader.Load(MainFont.bmFont, NGUITools.GetHierarchy(MainFont.gameObject), FontImport[1].bytes);
	Loc.currentLanguage = "CR";
	btnm.SelectBtnSize();
	if(bool)
	war.warningAllEnter.Show(Loc.Get("buttons620") , Loc.Get("info599")); 
//	PlayerPrefs.SetString("Language", "CR");
}

function SelectChineseT(){
	MainFont.material = FontMaterial[2];
	myBMFontReader.Load(MainFont.bmFont, NGUITools.GetHierarchy(MainFont.gameObject), FontImport[2].bytes);
	Loc.currentLanguage = "CHT";
	btnm.SelectBtnSize();
	if(bool)
	war.warningAllEnter.Show(Loc.Get("buttons620") , Loc.Get("info599")); 
//	PlayerPrefs.SetString("Language", "CR");
}


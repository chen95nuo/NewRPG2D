#pragma strict

function Start () {
}




var btnTraning 		: 	UISprite;//训练
var LblTraning		:   UILabel ; 
var btnCook		: 	UISprite;//烤鱼
var LblCook		:   UILabel ; 
var btnBuild 		: 	UISprite;//强化
var LblBuild		:   UILabel ; 
var btnBattle		: 	UISprite;//影魔
var LblBattle		:   UILabel ; 
var btnSoul 		: 	UISprite;//魔魂
var LblSoul		:   UILabel ; 
var SprBuild		: 	UISprite;//制造
var LblBuildSpr		:   UILabel; 
var btnGem 			: 	UISprite;//宝石
var LblGem		:   UILabel ; 

var btnArenas 		: 	UISprite;//战场
var LblArenas		:   UILabel ; 
var btnMount 	: 	UISprite;//坐骑
var LblMount	:   UILabel ; 

var btnMagic		: 	UISprite;//困魔监狱
var LblMagic		:   UILabel ; 

var btnActivityMe		: 	UISprite;//活动
var LblActivityMe		:   UILabel ; 

var btnBoss		: 	UISprite;//世界BOSS
var LblBoss		:   UILabel ; 



private var Fstr : String = ";";
var RideItemStr : String;
var UseRideIDs : String[];





var OpenColor : Color;
var useColor : Color;


function OnEnable() {
	ShowMyBtn();
}

function ShowMyBtn()
{

		//训练O
		if(AllManage.UICLStatic.canOpenViewAsTaskID("62")){
			btnTraning.color = OpenColor;
			LblTraning.text = "";
		}else{
			btnTraning.color = useColor;
			LblTraning.text = AllManage.AllMge.Loc.Get("info985");
		}
		
		//强化
		if(AllManage.UICLStatic.canOpenViewAsTaskID("121")){
			btnCook.color = OpenColor;
			LblCook.text = "";
		}else{
			btnCook.color = useColor;
			LblCook.text = AllManage.AllMge.Loc.Get("info985");
		}
		
		//强化
		if(AllManage.UICLStatic.canOpenViewAsTaskID("13")){
			btnBuild.color = OpenColor;
			LblBuild.text = "";
		}else{
			btnBuild.color = useColor;
			LblBuild.text = AllManage.AllMge.Loc.Get("info985");
		}
		
	
		//影魔
		if(AllManage.UICLStatic.canOpenViewAsTaskID("527")){
		btnBattle.color = OpenColor;
		LblBattle.text = "";
		}else{
		btnBattle.color = useColor;
		LblBattle.text = AllManage.AllMge.Loc.Get("info985");
		}
		
		//炼魂
		if(AllManage.UICLStatic.canOpenViewAsTaskID("122")){
			btnSoul.color = OpenColor;
			LblSoul.text = "";
		}else{
			btnSoul.color = useColor;
			LblSoul.text = AllManage.AllMge.Loc.Get("info985");
		}
			//制造
		if(AllManage.UICLStatic.canOpenViewAsTaskID("127")){
			SprBuild.color = OpenColor;
			LblBuildSpr.text = "";
		}else{
			SprBuild.color = useColor;
			LblBuildSpr.text = AllManage.AllMge.Loc.Get("info985");
		}
		
		//宝石
		if(AllManage.UICLStatic.canOpenViewAsTaskID("128")){
			btnGem.color = OpenColor;
			LblGem.text = "";
		}else{
			btnGem.color = useColor;
			LblGem.text = AllManage.AllMge.Loc.Get("info985");
		}
		
	
	
		//角斗场
		if(parseInt(AllManage.psStatic.Level)>=10){
		btnArenas.color = OpenColor;
		LblArenas.text = "";
		}else{
		btnArenas.color = useColor;
		LblArenas.text = AllManage.AllMge.Loc.Get("buttons262");
		}
		
				
		//坐骑
		
		RideItemStr = BtnGameManager.yt.Rows[0]["Mounts"].YuanColumnText;
		var i : int = 0;
		UseRideIDs = RideItemStr.Split(Fstr.ToCharArray());
		for(i=0; i<UseRideIDs.length; i++){	
		if(UseRideIDs[i].Length > 4){
		btnMount.color = OpenColor;
		LblMount.text = "";
		break;
		}else{
		btnMount.color = useColor;
		LblMount.text = AllManage.AllMge.Loc.Get("info985");
		}
		}
		
		
			//困魔塔
		if(parseInt(AllManage.psStatic.Level)>=15){
		btnMagic.color = OpenColor;
		LblMagic.text = "";
		}else{
		btnMagic.color = useColor;
		LblMagic.text = AllManage.AllMge.Loc.Get("info1244");
		}
		
			//活动
		if(parseInt(AllManage.psStatic.Level)>=20){
		btnActivityMe.color = OpenColor;
		LblActivityMe.text = "";
		}else{
		btnActivityMe.color = useColor;
		LblActivityMe.text = AllManage.AllMge.Loc.Get("buttons267");
		}
		
				//世界Boss
		if(parseInt(AllManage.psStatic.Level)>=20){
		btnBoss.color = OpenColor;
		LblBoss.text = "";
		}else{
		btnBoss.color = useColor;
		LblBoss.text = AllManage.AllMge.Loc.Get("buttons267");
		}
		
}
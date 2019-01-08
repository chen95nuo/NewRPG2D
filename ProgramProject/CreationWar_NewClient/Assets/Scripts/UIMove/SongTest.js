#pragma strict
class SongTest extends Song{

	function Awake(){
//		print("111111111111111111111111111111Awake");
	}
	
	function Start(){
//		print("111111111111111111111111111111Start");	
	}
}

//	var charB : GameObject;
//	function CharBarTextMoney(strs : String[]){
//		var mGold : int = 0;
//		var mBlood : int = 0;
//		var strGold : String = "";
//		var strBlood : String = "";
//		
//		mGold = (strs[0] == "") ? 0 : parseInt(strs[0]);
//		mBlood = (strs[1] == "") ? 0 : parseInt(strs[0]);
//		
//		if(mGold > 0){
//			strGold = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info554") , mGold ,  AllManage.AllMge.Loc.Get("info335"));
//		}else
//		if(mGold < 0){
//			strGold = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info552") , Mathf.Abs(mGold) ,  AllManage.AllMge.Loc.Get("info335"));
//		}
//		
//		if(strGold != ""){
//			charB.SendMessage("AddText" , strGold , SendMessageOptions.DontRequireReceiver);
//			PanelStatic.StaticSendManager.listBar[0].SendMessage("AddText" , strGold , SendMessageOptions.DontRequireReceiver);
//			PanelStatic.StaticSendManager.listBar[4].SendMessage("AddText" , strGold , SendMessageOptions.DontRequireReceiver);		
//		}
//		
//		if(mBlood > 0){
//			strBlood = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info554") , mGold ,  AllManage.AllMge.Loc.Get("messages053"));
//		}else
//		if(mBlood < 0){
//			strBlood = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info552") , Mathf.Abs(mGold) ,  AllManage.AllMge.Loc.Get("messages053"));		
//		}
//		
//		if(strBlood != ""){
//			charB.SendMessage("AddText" , strBlood , SendMessageOptions.DontRequireReceiver);
//			PanelStatic.StaticSendManager.listBar[0].SendMessage("AddText" , strBlood , SendMessageOptions.DontRequireReceiver);
//			PanelStatic.StaticSendManager.listBar[4].SendMessage("AddText" , strBlood , SendMessageOptions.DontRequireReceiver);		
//		}
//	}

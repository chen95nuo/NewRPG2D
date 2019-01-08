#pragma strict
static var me : GameObject;
//function Awake(){
//	me = gameObject;
//	DontDestroyOnLoad(this.gameObject);
//}

var PS : PlayerStatus;
function Jia(mmm3 : String){
	if(PS == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
		PS = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	} 
		if(mmm3 == "cszz.001"){
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.mmm31 , 0 , 0 , "" , gameObject , "");
//			AllManage.AllMge.UseMoney(0 , -60 , UseMoneyType.mmm31 , gameObject , "");
//				PS.UseMoney(0 , -60);
		}
		if(mmm3 == "cszz.013"){
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.mmm32 , 0 , 0 , "" , gameObject , "");
//			AllManage.AllMge.UseMoney(0 , -200 , UseMoneyType.mmm32 , gameObject , "");
//				PS.UseMoney(0 , -200);
		}
		if(mmm3 == "cszz.003"){
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.mmm33 , 0 , 0 , "" , gameObject , "");
//			AllManage.AllMge.UseMoney(0 , -900 , UseMoneyType.mmm33, gameObject , "");
//				PS.UseMoney(0 , -900);
		}
		if(mmm3 == "cszz.050"){
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.mmm34 , 0 , 0 , "" , gameObject , "");
//			AllManage.AllMge.UseMoney(0 , -3500 , UseMoneyType.mmm34 , gameObject , "");
//				PS.UseMoney(0 , -3500);
		}
}

//var cam : Camera;
//function Update(){
//	if (Input.GetKeyDown (KeyCode.T)){
//		cam.enabled = false;
//	}
//	if (Input.GetKeyDown (KeyCode.Y)){
//		cam.enabled = true;
//	}	
//}

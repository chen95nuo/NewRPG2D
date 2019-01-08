#pragma strict

var btnM : UIButtonMessage;
var str : String = "uicl";
function Awake(){
	btnM = GetComponent(UIButtonMessage);
	if(btnM){
		switch(str){
			case "uicl":
				btnM.target = AllManage.UICLStatic.gameObject;
				break;
			case "yuanBM":
				btnM.target = AllManage.yuanBMStatic.gameObject;
				break; 
			case "mtw":
				btnM.target = AllManage.mtwStatic.gameObject;
				break; 
			case "dungcl":
				btnM.target = AllManage.dungclStatic.gameObject;
				break; 
			case "uiall":
				btnM.target = AllManage.UIALLPCStatic.gameObject;
				break; 
		}
	}
}

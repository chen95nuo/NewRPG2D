#pragma strict

function Start(){
	while(PlayerStatus.MainCharacter == null){
		yield;
	}
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	} 
}

var labelLeft : UILabel;
var labelRight : UILabel;
function showView(){
	obj.SetActiveRecursively(true);
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	} 
	costGold = parseInt( ps.Level ) * parseInt( ps.Level ) * AllManage.InvclStatic.VipYaoPingLevel + 99;
	if(AllManage.InvclStatic.VipYaoPing < AllManage.InvclStatic.maxXuePingNum)
		labelLeft.text = AllManage.AllMge.Loc.Get("messages052") + costGold + AllManage.AllMge.Loc.Get("info564");
	else
		labelLeft.text = AllManage.AllMge.Loc.Get("info567");
		
	if(AllManage.InvclStatic.VipYaoPingLevel < 9)
		labelRight.text = AllManage.AllMge.Loc.Get("messages052") + AllManage.InvclStatic.VipYaoPingLevel * 10 + AllManage.AllMge.Loc.Get("info565");
	else
		labelRight.text = AllManage.AllMge.Loc.Get("info566");
}

function UpdateXuePing(){ 
	if(AllManage.InvclStatic.VipYaoPingLevel < 9)
		AllManage.UICLStatic.UpLevelYaoShui(); 
	else 
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info566"));
		close();
//		labelRight.text = AllManage.AllMge.Loc.Get("info566");
} 

private var ps : PlayerStatus;
private var costGold : int = 0;
function PlusXuePing(){
    if(AllManage.InvclStatic.VipYaoPing < AllManage.InvclStatic.maxXuePingNum) {
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		} 
		costGold = parseInt( ps.Level ) * parseInt( ps.Level ) * AllManage.InvclStatic.VipYaoPingLevel + 99;
		AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.PlusBottle , parseInt( ps.Level ) , AllManage.InvclStatic.VipYaoPingLevel , "" , gameObject , "realPlusXuePing");
//		AllManage.AllMge.UseMoney(parseInt( ps.Level ) * AllManage.InvclStatic.VipYaoPingLevel * 5 + 300 , 0 , UseMoneyType.PlusBottle , gameObject , "realPlusXuePing");
//		if(ps.UseMoney(costGold , 0)){
//			AllManage.UICLStatic.PluseYaoPing(1);
//		}
	}else{
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info567"));		
	}
	close();
}

function realPlusXuePing(){
	AllManage.UICLStatic.PluseYaoPing(1);	
}

var obj : GameObject; 
function close(){
	obj.SetActiveRecursively(false);
}
#pragma strict

var playerS : PlayerStatus;
var fsHP : UISlider;
var fsNU : UIFilledSprite;
var fsEXP : UISlider;
var LabelXueshi : UILabel;
var LabelGold : UILabel;
var LabelTouGuan : UILabel;
var LabelLevel : UILabel;
var Labelhp : UILabel;
var Labelnv : UILabel;
var LabelPower : UILabel;
var LabelPrestige : UILabel;
var LabelPVPPoint : UILabel;

var LabelGoldS : UILabel;
var LabelSoulS : UILabel;
var LabelBloodS : UILabel;
var LabelSoulPowerS : UILabel;
var PlayerInfoOtherLabelCombat : UILabel;
var str4 : String;
var str5 : String;
var str6 : String;

private var float1 : float;
private var float2 : float;
private var float3 : float;
private var float4 : float;
var float5 : float;
var float6 : float;
private var float7 : float;
var str1 : String;
var str2 : String;
var str3 : String;
function Awake(){
	AllManage.PlyUIClStatic = this;
}

function Start(){
	InvokeRepeating("Update1", 0.1, 0.1);  
}

private var ptime : int;
var SpriteGuang : UISprite;
var spritePro : UISprite;
private var strSpritePro : String;
function Update1 () {
	if(playerS != null){
		if(LabelXueshi.text != playerS.Bloodstone){  
			LabelXueshi.text = playerS.Bloodstone;
		}
		if(LabelBloodS != null && LabelBloodS.text != playerS.Bloodstone){  
			LabelBloodS.text = playerS.Bloodstone;
		}
		if(LabelGold.text != playerS.Money){
			LabelGold.text = playerS.Money;		
		}
		strSpritePro = AllManage.InvclStatic.GetProSpriteName();
		if(spritePro.spriteName != strSpritePro){
			spritePro.spriteName = strSpritePro;
		}
		
		float1 = parseInt(playerS.Health);
		float2 = parseInt(playerS.Maxhealth);
		float3 = parseInt(playerS.Mana);
		float4 = parseInt(playerS.Maxmana);
		float5 = playerS.getNowExp();
		float6 = playerS.getNextExp();
		if(fsHP.sliderValue != float1 / float2){
			fsHP.sliderValue = float1 / float2;		
		}
		if(fsHP.sliderValue > 0.4){
			SpriteGuang.enabled = false;
		}else{
			SpriteGuang.enabled = true;			
		}
		if(fsNU.fillAmount != float3 / float4){
			fsNU.fillAmount = float3 / float4;		
		}
		if(float7 != float5 / float6 && Time.time > ptime){
			ptime = Time.time + 1;
			float7 = float5 / float6;
			fsEXP.sliderValue = float7;		
		}
		if(LabelLevel.text != playerS.Level.ToString()){
//			//print(Time.time);
			LabelLevel.text = playerS.Level.ToString();		
		}
		if(LabelPrestige.text != playerS.Prestige){
//			//print(Time.time);
			LabelPrestige.text = playerS.Prestige;		
		}
		if(LabelPVPPoint.text != playerS.PVPPoint){
//			//print(Time.time);
			LabelPVPPoint.text = playerS.PVPPoint;		
		}
		str1 =String.Format("{0}/{1}",playerS.Health, playerS.Maxhealth); 
		if(Labelhp.text != str1){
//			//print(Time.time);
			Labelhp.text = str1;		
		}
		str2 = String.Format("{0}/{1}",playerS.Mana, playerS.Maxmana); 
		if(Labelnv.text != str2){
//			//print(Time.time);
			Labelnv.text =  str2;		
		}
		str3 =  playerS.Power.ToString();
		if(LabelPower.text != str3){
//			//print(Time.time);
			LabelPower.text = str3;		
		}
		str4 = playerS.Money;
		if(LabelGoldS != null){
			if(LabelGoldS.text != str4){
			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add("messages063");
			AllManage.AllMge.Keys.Add(str4 + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelGoldS);
//				LabelGoldS.text = "金币: " +str4;
			}
		}
		
		str5 = playerS.Soul + "";
		if(LabelSoulS != null){
			if(LabelSoulS.text != str5){
			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add("messages094");
			AllManage.AllMge.Keys.Add(str5 + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSoulS);
//				LabelSoulS.text = "灵魂: " +str5;
			}
		}
		
		str5 = playerS.SoulPower + "";
		if(LabelSoulPowerS != null){
			if(LabelSoulPowerS.text != str5){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages095");
			AllManage.AllMge.Keys.Add(str5 + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSoulPowerS);
//				LabelSoulPowerS.text ="魔能: " + str5;
			}
		}
	}
	
	if(PlayerInfoOtherLabelCombat!=null){
	PlayerInfoOtherLabelCombat.text = AllManage.InvclStatic.ComBatLabel.ToString();
	}
}

function GetChange(){

}
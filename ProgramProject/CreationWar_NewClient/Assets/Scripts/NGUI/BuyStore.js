var liang : Transform;
var invpos : Transform[];
var downid : int;
var yes : GameObject;
var Boo1 : boolean = false;
var Boo2 : boolean = false;
var Boo3 : boolean = false;
var Boo4 : boolean = false;
var PS : PlayerStatus;
var productIdentifiers : String[];
private var jia : boolean = false;
function Start(){
//	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
//		PS = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
//	} 
	
//	productIdentifiers = new String["anotherProduct", "tt", "testProduct", "sevenDays", "oneMonthSubsciber" ];
//	StoreCount = PlayerPrefs.GetInt("StoreCount",0);
//	yield WaitForSeconds(1);
//	if(PS != null){
//		Jia();
//	}
}

function Buy1(){
	downid = 0;
	Buy();
}
function Buy2(){
	downid = 1;
	Buy();
}
function Buy3(){
	downid = 2;
	Buy();
}
function Buy4(){
	downid = 3;
	Buy();
}
		
function Buy(){
	var myRandom : int = Random.Range(10000,99999);
	if(downid == 0 ){//&& AllManage.UICLStatic.dicGetResult1){
		print("mai 1");
//		productIdentifiers[1] = "cszz.001";
///InRoom.GetInRoomInstantiate().ssss = 	InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText;
	
	#if UNITY_ANDROID
		#if SDK_UC
			SDKManager.zzsdk_pay(String.Format("{0},{1},{2};{3};{4};{5};{6};{7}",
				PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText,
				AllManage.UICLStatic.dicGetResult1["propTag"],
				AllManage.UICLStatic.dicGetResult1["propPrice"]
			));
		#else 
//	var	 money1:String =	AllManage.UICLStatic.dicGetResult1["propPrice"];
//		var xueshi1:String=AllManage.UICLStatic.dicGetResult1["propXueshi"];
//	var	  playerid1:String=InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText;
//	InRoom.GetInRoomInstantiate().PayTestPC(playerid1,money1,xueshi1);
//			SDKManager.zzsdk_pay(String.Format("{0},{1},{2};{3};{4};{5};{6}",
//				PlayerPrefs.GetString("NumTitleS1" , "Empty"),
//				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
//				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
//				AllManage.UICLStatic.dicGetResult1["propPrice"],
//				AllManage.UICLStatic.dicGetResult1["propTag"],
//				AllManage.UICLStatic.dicGetResult1["propName"],
//				PlayerPrefs.GetString("lblServerNameS1" , "Empty")
//			));
		#endif
//		#else
//			var	 money1:String =	AllManage.UICLStatic.dicGetResult1["propPrice"];
//		var xueshi1:String=AllManage.UICLStatic.dicGetResult1["propXueshi"];
//	var	  playerid1:String=InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText;
//	InRoom.GetInRoomInstantiate().PayTestPC(playerid1,money1,xueshi1);
//			SDKManager.zzsdk_pay(String.Format("{0},{1},{2};{3};{4};{5};{6}",
//				PlayerPrefs.GetString("NumTitleS1" , "Empty"),
//				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
//				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
//				AllManage.UICLStatic.dicGetResult1["propPrice"],
//				AllManage.UICLStatic.dicGetResult1["propTag"],
//				AllManage.UICLStatic.dicGetResult1["propName"],
//				PlayerPrefs.GetString("lblServerNameS1" , "Empty")
//			));
	#elif UNITY_IOS
		#if SDK_ZSYIOS
			StoreKitBinding.requestProductData("cszz.001");
			StoreKitBinding.purchaseProduct("cszz.001",1);			
		#elif SDK_HM
			HMSdkControl.HMSdkpay(System.DateTime.Now.Ticks.ToString()+myRandom.ToString(), 
											  AllManage.UICLStatic.dicGetResult1["propPrice"], 
											  AllManage.UICLStatic.dicGetResult1["propNum"],
				                     		String.Format("{0},{1},{2},{3}",	
									              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
									              InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
									             InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
									             AllManage.UICLStatic.dicGetResult1["propNum"]));	
		#elif SDK_TONGBU
			TBSdkControl.TBSdkpay(System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      AllManage.UICLStatic.dicGetResult1["propPrice"],
				                      String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult1["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
        #elif SDK_JYIOS
        // TODO: API：异步支付（订单号，道具ID，道具名，价格，数量，分区：不超过20个英文或数字的字符串）
        	SdkConector.NdUniPayAsyn( System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
        	 AllManage.UICLStatic.dicGetResult1["propNum"],
        	 AllManage.UICLStatic.dicGetResult1["propName"],
        	 AllManage.UICLStatic.dicGetResult1["propPrice"],
        	  "1",
        	 String.Format("{0},{1},{2},{3}", PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult1["propNum"]));
        #elif SDK_ITOOLS
				ItoolsSdkControl.ItoolSDKpay( AllManage.UICLStatic.dicGetResult1["propName"],
				                              AllManage.UICLStatic.dicGetResult1["propPrice"],
				                           String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult1["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
		#elif SDK_KUAIYONG
				KYSdkControl.KYSDKpay( String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult1["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                       AllManage.UICLStatic.dicGetResult1["propPrice"],
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                       AllManage.UICLStatic.dicGetResult1["propName"]);
     #elif SDK_XY
				XYSDKControl.XYSDKpay(AllManage.UICLStatic.dicGetResult1["propPrice"],
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                     String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult1["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
      #elif SDK_I4
				ASSDKControl.ASSDKpay(AllManage.UICLStatic.dicGetResult1["propPrice"],
										System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      AllManage.UICLStatic.dicGetResult1["propName"],
				                     String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult1["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"));
       #elif SDK_ZSY
				ZSYSDKControl.ZSYSDKpay(String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult1["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                        AllManage.UICLStatic.dicGetResult1["propPrice"],
				                        AllManage.UICLStatic.dicGetResult1["propName"]);
		 #elif SDK_PP
				PPSdkControl.PPSdkpay(AllManage.UICLStatic.dicGetResult1["propPrice"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      AllManage.UICLStatic.dicGetResult1["propName"],
				                      String.Format("{0},{1},{2},{3}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
											  InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
											  InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
											  AllManage.UICLStatic.dicGetResult1["propNum"]),
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"));
      	#endif
	#endif			
		
//			Boo1 = true;
	}
	if(downid == 1){// && AllManage.UICLStatic.dicGetResult2){
		print("mai 2");
		#if UNITY_ANDROID
		#if SDK_UC
			SDKManager.zzsdk_pay(String.Format("{0},{1},{2};{3};{4};{5};{6};{7}",
				PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText,
				AllManage.UICLStatic.dicGetResult2["propTag"],
				AllManage.UICLStatic.dicGetResult2["propPrice"]
			));
		#else
//		var money2:String =AllManage.UICLStatic.dicGetResult2["propPrice"];
//		var xueshi2:String=AllManage.UICLStatic.dicGetResult2["propXueshi"];
//	var  playerid2:String=InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText;
//	InRoom.GetInRoomInstantiate().PayTestPC(playerid2,money2,xueshi2);
//			SDKManager.zzsdk_pay(String.Format("{0},{1},{2};{3};{4};{5};{6}",
//				PlayerPrefs.GetString("NumTitleS1" , "Empty"),
//				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
//				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
//				AllManage.UICLStatic.dicGetResult2["propPrice"],
//				AllManage.UICLStatic.dicGetResult2["propTag"],
//				AllManage.UICLStatic.dicGetResult2["propName"],
//				PlayerPrefs.GetString("lblServerNameS1" , "Empty")
//			));
		#endif	
//		#else
//		var money2:String =AllManage.UICLStatic.dicGetResult2["propPrice"];
//		var xueshi2:String=AllManage.UICLStatic.dicGetResult2["propXueshi"];
//	var  playerid2:String=InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText;
//	InRoom.GetInRoomInstantiate().PayTestPC(playerid2,money2,xueshi2);
//			SDKManager.zzsdk_pay(String.Format("{0},{1},{2};{3};{4};{5};{6}",
//				PlayerPrefs.GetString("NumTitleS1" , "Empty"),
//				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
//				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
//				AllManage.UICLStatic.dicGetResult2["propPrice"],
//				AllManage.UICLStatic.dicGetResult2["propTag"],
//				AllManage.UICLStatic.dicGetResult2["propName"],
//				PlayerPrefs.GetString("lblServerNameS1" , "Empty")
//				));
	#elif UNITY_IOS
		#if SDK_ZSYIOS
			StoreKitBinding.requestProductData("cszz.013");
			StoreKitBinding.purchaseProduct("cszz.013",1);
		#elif SDK_HM
			HMSdkControl.HMSdkpay(System.DateTime.Now.Ticks.ToString()+myRandom.ToString(), 
											  AllManage.UICLStatic.dicGetResult2["propPrice"], 
											  AllManage.UICLStatic.dicGetResult2["propNum"],
				                     		String.Format("{0},{1},{2},{3}",	
									              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
									              InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
									             InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
									             AllManage.UICLStatic.dicGetResult2["propNum"]));	
		#elif SDK_TONGBU
			TBSdkControl.TBSdkpay(System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      AllManage.UICLStatic.dicGetResult2["propPrice"],
				                      String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult2["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
		#elif SDK_JYIOS
        // TODO: API：异步支付（订单号，道具ID，道具名，价格，数量，分区：不超过20个英文或数字的字符串）
        	SdkConector.NdUniPayAsyn( System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
        	 AllManage.UICLStatic.dicGetResult2["propNum"],
        	 AllManage.UICLStatic.dicGetResult2["propName"],
        	 AllManage.UICLStatic.dicGetResult2["propPrice"],
        	  "1",
        	 String.Format("{0},{1},{2},{3}", PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult2["propNum"]));
		#elif SDK_ITOOLS
				ItoolsSdkControl.ItoolSDKpay( AllManage.UICLStatic.dicGetResult2["propName"],
				                              AllManage.UICLStatic.dicGetResult2["propPrice"],
				                          String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult2["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
		#elif SDK_KUAIYONG
				KYSdkControl.KYSDKpay( String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult2["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                       AllManage.UICLStatic.dicGetResult2["propPrice"],
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                       AllManage.UICLStatic.dicGetResult2["propName"]);
       #elif SDK_XY
				XYSDKControl.XYSDKpay(AllManage.UICLStatic.dicGetResult2["propPrice"],
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                     String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult2["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
       #elif SDK_I4
				ASSDKControl.ASSDKpay(AllManage.UICLStatic.dicGetResult2["propPrice"],
										System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      AllManage.UICLStatic.dicGetResult2["propName"],
				                     String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult2["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"));
        #elif SDK_ZSY
				ZSYSDKControl.ZSYSDKpay(String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult2["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                        AllManage.UICLStatic.dicGetResult2["propPrice"],
				                        AllManage.UICLStatic.dicGetResult2["propName"]);
		 #elif SDK_PP
				PPSdkControl.PPSdkpay(AllManage.UICLStatic.dicGetResult2["propPrice"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      AllManage.UICLStatic.dicGetResult2["propName"],
				                      String.Format("{0},{1},{2},{3}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
											  InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
											  InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
											  AllManage.UICLStatic.dicGetResult2["propNum"]),
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"));
      	 #endif
	#endif			
		
//		productIdentifiers[1] = "cszz.013";
//			Boo2 = true;
	}
	if(downid == 2){// && AllManage.UICLStatic.dicGetResult3){
		print("mai 3");
		#if UNITY_ANDROID
		#if SDK_UC
			SDKManager.zzsdk_pay(String.Format("{0},{1},{2};{3};{4};{5};{6};{7}",
				PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText,
				AllManage.UICLStatic.dicGetResult3["propTag"],
				AllManage.UICLStatic.dicGetResult3["propPrice"]
			));
		#else
//	var	money3:String =	AllManage.UICLStatic.dicGetResult3["propPrice"];
//	var	xueshi3:String=AllManage.UICLStatic.dicGetResult3["propXueshi"];
//	 var   playerid3:String=InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText;
//	 InRoom.GetInRoomInstantiate().PayTestPC(playerid3,money3,xueshi3);
//			SDKManager.zzsdk_pay(String.Format("{0},{1},{2};{3};{4};{5};{6}",
//				PlayerPrefs.GetString("NumTitleS1" , "Empty"),
//				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
//				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
//				AllManage.UICLStatic.dicGetResult3["propPrice"],
//				AllManage.UICLStatic.dicGetResult3["propTag"],
//				AllManage.UICLStatic.dicGetResult3["propName"],
//				PlayerPrefs.GetString("lblServerNameS1" , "Empty")
//			));
		#endif
//		#else
//		var	money3:String =	AllManage.UICLStatic.dicGetResult3["propPrice"];
//	var	xueshi3:String=AllManage.UICLStatic.dicGetResult3["propXueshi"];
//	 var   playerid3:String=InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText;
//	 InRoom.GetInRoomInstantiate().PayTestPC(playerid3,money3,xueshi3);
//			SDKManager.zzsdk_pay(String.Format("{0},{1},{2};{3};{4};{5};{6}",
//				PlayerPrefs.GetString("NumTitleS1" , "Empty"),
//				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
//				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
//				AllManage.UICLStatic.dicGetResult3["propPrice"],
//				AllManage.UICLStatic.dicGetResult3["propTag"],
//				AllManage.UICLStatic.dicGetResult3["propName"],
//				PlayerPrefs.GetString("lblServerNameS1" , "Empty")
//			));
	#elif UNITY_IOS
		#if SDK_ZSYIOS
			StoreKitBinding.requestProductData("cszz.003");
			StoreKitBinding.purchaseProduct("cszz.003",1);
		#elif SDK_HM
			HMSdkControl.HMSdkpay(System.DateTime.Now.Ticks.ToString()+myRandom.ToString(), 
											  AllManage.UICLStatic.dicGetResult3["propPrice"], 
											  AllManage.UICLStatic.dicGetResult3["propNum"],
				                     		String.Format("{0},{1},{2},{3}",	
									              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
									              InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
									             InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
									             AllManage.UICLStatic.dicGetResult3["propNum"]));	
		#elif SDK_TONGBU
			TBSdkControl.TBSdkpay(System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                    AllManage.UICLStatic.dicGetResult3["propPrice"],
				                      String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult3["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
		#elif SDK_JYIOS
        // TODO: API：异步支付（订单号，道具ID，道具名，价格，数量，分区：不超过20个英文或数字的字符串）
        	SdkConector.NdUniPayAsyn( System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
        	 AllManage.UICLStatic.dicGetResult3["propNum"],
        	 AllManage.UICLStatic.dicGetResult3["propName"],
        	 AllManage.UICLStatic.dicGetResult3["propPrice"],
        	  "1",
        	 String.Format("{0},{1},{2},{3}", PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult3["propNum"]));
		#elif SDK_ITOOLS
				ItoolsSdkControl.ItoolSDKpay( AllManage.UICLStatic.dicGetResult3["propName"],
				                              AllManage.UICLStatic.dicGetResult3["propPrice"],
				                           String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult3["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
		#elif SDK_KUAIYONG
				KYSdkControl.KYSDKpay( String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult3["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                       AllManage.UICLStatic.dicGetResult3["propPrice"],
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                       AllManage.UICLStatic.dicGetResult3["propName"]);
         #elif SDK_XY
				XYSDKControl.XYSDKpay(AllManage.UICLStatic.dicGetResult3["propPrice"],
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                    String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult3["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
        #elif SDK_I4
				ASSDKControl.ASSDKpay(AllManage.UICLStatic.dicGetResult3["propPrice"],
										System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      AllManage.UICLStatic.dicGetResult3["propName"],
				                     String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult3["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"));
         #elif SDK_ZSY
				ZSYSDKControl.ZSYSDKpay(String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult3["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                        AllManage.UICLStatic.dicGetResult3["propPrice"],
				                        AllManage.UICLStatic.dicGetResult3["propName"]);
		 #elif SDK_PP
					PPSdkControl.PPSdkpay(AllManage.UICLStatic.dicGetResult3["propPrice"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      AllManage.UICLStatic.dicGetResult3["propName"],
				                      String.Format("{0},{1},{2},{3}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
											  InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
											  InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
											  AllManage.UICLStatic.dicGetResult3["propNum"]),
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"));
      	 #endif
	#endif						
//		productIdentifiers[1] = "cszz.003";
//			Boo3 = true;
	}
	if(downid == 3){// && AllManage.UICLStatic.dicGetResult4){
		print("mai 4");
		#if UNITY_ANDROID
		#if SDK_UC
		
			SDKManager.zzsdk_pay(String.Format("{0},{1},{2};{3};{4};{5};{6};{7}",
				PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
				InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText,
				AllManage.UICLStatic.dicGetResult4["propTag"],
				AllManage.UICLStatic.dicGetResult4["propPrice"]
			));
		#else
//		var money:String =	AllManage.UICLStatic.dicGetResult4["propPrice"];
//		var xueshi:String=AllManage.UICLStatic.dicGetResult4["propXueshi"];
//		var  playerid:String=InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText;
//		InRoom.GetInRoomInstantiate().PayTestPC(playerid,money,xueshi);
//		
//			SDKManager.zzsdk_pay(String.Format("{0},{1},{2};{3};{4};{5};{6}",
//				PlayerPrefs.GetString("NumTitleS1" , "Empty"),
//				InventoryControl.yt.Rows[0]["ProID"].YuanColumnText,
//				InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
//				AllManage.UICLStatic.dicGetResult4["propPrice"],
//				AllManage.UICLStatic.dicGetResult4["propTag"],
//				AllManage.UICLStatic.dicGetResult4["propName"],
//				PlayerPrefs.GetString("lblServerNameS1" , "Empty")
//			));
		#endif
//		#else	
		
	
	#elif UNITY_IOS

		#if SDK_ZSYIOS
			StoreKitBinding.requestProductData("cszz.050");
			StoreKitBinding.purchaseProduct("cszz.050",1);
		#elif SDK_HM
			HMSdkControl.HMSdkpay(System.DateTime.Now.Ticks.ToString()+myRandom.ToString(), 
											  AllManage.UICLStatic.dicGetResult4["propPrice"], 
											  AllManage.UICLStatic.dicGetResult4["propNum"],
				                     		String.Format("{0},{1},{2},{3}",	
									              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
									              InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
									             InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
									             AllManage.UICLStatic.dicGetResult4["propNum"]));	
		#elif SDK_TONGBU
			TBSdkControl.TBSdkpay(System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      AllManage.UICLStatic.dicGetResult4["propPrice"],
				                      String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult4["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
		#elif SDK_JYIOS
        // TODO: API：异步支付（订单号，道具ID，道具名，价格，数量，分区：不超过20个英文或数字的字符串）
        	SdkConector.NdUniPayAsyn( System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
        	 AllManage.UICLStatic.dicGetResult4["propNum"],
        	 AllManage.UICLStatic.dicGetResult4["propName"],
        	 AllManage.UICLStatic.dicGetResult4["propPrice"],
        	  "1",
        	 String.Format("{0},{1},{2},{3}", PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult4["propNum"]));
		#elif SDK_ITOOLS
				ItoolsSdkControl.ItoolSDKpay( AllManage.UICLStatic.dicGetResult4["propName"],
				                              AllManage.UICLStatic.dicGetResult4["propPrice"],
				                          String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult4["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
		#elif SDK_KUAIYONG
				KYSdkControl.KYSDKpay( String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult4["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                       AllManage.UICLStatic.dicGetResult4["propPrice"],
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                       AllManage.UICLStatic.dicGetResult4["propName"]);
        #elif SDK_XY
				XYSDKControl.XYSDKpay(AllManage.UICLStatic.dicGetResult4["propPrice"],
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"),
				                    String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult4["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()));
         #elif SDK_I4
				ASSDKControl.ASSDKpay(AllManage.UICLStatic.dicGetResult4["propPrice"],
										System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      AllManage.UICLStatic.dicGetResult4["propName"],
				                     String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult4["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"));
        #elif SDK_ZSY
				ZSYSDKControl.ZSYSDKpay(String.Format("{0},{1},{2},{3},{4}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
												InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
												InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
												AllManage.UICLStatic.dicGetResult4["propNum"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString()),
				                        AllManage.UICLStatic.dicGetResult4["propPrice"],
				                        AllManage.UICLStatic.dicGetResult4["propName"]);
		  #elif SDK_PP
					PPSdkControl.PPSdkpay(AllManage.UICLStatic.dicGetResult4["propPrice"],
								              System.DateTime.Now.Ticks.ToString()+myRandom.ToString(),
				                      AllManage.UICLStatic.dicGetResult4["propName"],
				                      String.Format("{0},{1},{2},{3}",	
								              PlayerPrefs.GetString("NumTitleS1" , "Empty"),
											  InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText,
											  InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText,
											  AllManage.UICLStatic.dicGetResult4["propNum"]),
				                      PlayerPrefs.GetString("NumTitleS1" , "Empty"));
      	#endif
	#endif		
//		productIdentifiers[1] = "cszz.050";

//			Boo4 = true;
	}
//		while(Boo1 || Boo2 || Boo3 || Boo4){
//			Jia();
//			yield WaitForSeconds(2);
//		}
}
//
//var mmm1 : String;
//var mmm2 : String;
//var mmm3 : String;
//var StoreCount : int = 0;
//function Jia(str : String){
//	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
//		PS = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
//	} 
//	var o = StoreKitBinding.getAllSavedTransactions();
//	mmm3 = str;
////	mmm1 = o.Count.ToString();
////	if(o.Count>0){
////		mmm3 = o[o.Count-1].ToString();
//////		mmm3 = mmm2.Substring(26,4);
////	}
////	for(var i=0; i<o.Count; i++){
////		//print(o[i].ToString());
////	}
////	//print(o + " == o");
////	//print(mmm2 + " == mmm2");
////	//print(mmm3 + " == mmm3");
//	if(StoreCount<o.Count){
//		if(mmm3 == "cszz.001"){
//				StoreCount = o.Count;
//				PlayerPrefs.SetInt("StoreCount",StoreCount);
//				PS.UseMoney(0 , -60);
//				Boo1 = false;
//		}
//		if(mmm3 == "cszz.013"){
//				StoreCount = o.Count;
//				PlayerPrefs.SetInt("StoreCount",StoreCount);
//				PS.UseMoney(0 , -200);
//				Boo2 = false;
//		}
//		if(mmm3 == "cszz.003"){
//				StoreCount = o.Count;
//				PlayerPrefs.SetInt("StoreCount",StoreCount);
//				PS.UseMoney(0 , -900);
//				Boo3 = false;
//		}
//		if(mmm3 == "cszz.050"){
//				PS.UseMoney(0 , -3500);
//				StoreCount = o.Count;
//				PlayerPrefs.SetInt("StoreCount",StoreCount);
//				Boo4 = false;
//		}
//	}
//}

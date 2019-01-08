using UnityEngine;
using System.Collections;

/// <summary>
/// 使用血石金币类型
/// </summary>
enum UseMoneyType{
	Sell1 = 0,									//卖出物品1
	WashAttributePoints = 1,				//洗技能点						
	SaveBranch = 2,							//保存转职
	YesBuyCard = 3,							//副本翻牌
	YesDoCD = 4,								//重置铁毡冷却
	Sell = 5,										//卖出物品
	Buy = 6,										//买入物品
	YesOpenCangKu = 7,					//开启仓库格子
	CostGold = 8,								//花费金币
	OpenSpreeItemAsID = 9,				//打开礼包
	YesOpenPackage = 10,				//打开道具包
	YAddGold = 11,							//增加金币
	YAddBlood = 12,							//增加血石
	YesPVP8Times = 13,					//重置8人pvp
	YesPVPTimes = 14,						//重置pvp
	Resurrection2 = 15,						//复活2
	Resurrection3 = 16,						//复活3
	mmm31 = 17,								//内购1
	mmm32 = 18,								//内购2
	mmm33 = 19,								//内购3
	mmm34 = 20,								//内购4
	UpDateOneSkill = 21,					//升级技能
	UpDateOneSoulItemSkill = 22,		//升级魔魂技能
	ButtonBuildSoulAndDigest = 23,	//生产魔魂&灵魂精华
	NumDigestButtonsPlus = 24,			//开启灵魂精华包裹
	PlusBottle = 25,							//增加药水
	ApplyPickup1 = 26,						//拾取1
	ApplyPickup2 = 27,						//拾取2
	TaskDone = 28,							//完成任务
	SpendingReturns11 = 29,				//花费返回
	RaidsStart = 30,							//扫荡开始
	CostRaidsNow = 31,					//立即完成扫荡
	Cost = 32,									//花费
	UpLevelBottle = 33,						//升级药水等级
	PlusBottle1 = 34,							//增加药水1
	FullBottle = 35,							//补满药水
	YesReturn = 36,							//确认返回
	YesManufacture = 37,					//打开制造按钮
	YesSoul = 38,								//打开魔魂按钮
	YesGem = 39,								//打开宝石镶嵌按钮
	YesDuplicate = 40,						//确认副本
	YesResetPVP = 41,						//重置pvp
	yesCook = 42,								//打开烤鱼按钮
	YesBuy = 43,								//确认购买
	Getreward = 44,							//获得奖励
	doneCard = 45							//完成副本
}
/// <summary>
/// 技能血石金币消耗
/// </summary>
public class PersonSkill : Object{
	public int[] gold = new int[3];
	public int[] blood = new int[3];
	public PersonSkill(){

	}
}

public class CostMoney : MonoBehaviour {
	/// <summary>
	/// 使用血石金币方法
	/// </summary>
	/// <param name="useType">使用血石金币类型</param>
	/// <param name="num1">计算公式中需要的变量</param>
	/// <param name="num2">计算公式中需要的变量</param>
	/// <param name="itemID">I装备物品id</param>
	void UseMoney(UseMoneyType useType , int num1 , int num2 , string itemID){
		int cGold = 0;
		int cBlood = 0;
		int[] costMoney;
		switch(useType)
		{
			case UseMoneyType.Sell1 :
				break;
			case UseMoneyType.WashAttributePoints :	
				cBlood = 50;
				break;
			case UseMoneyType.SaveBranch  :
				cBlood = num1 * 10;
				break;
			case UseMoneyType.YesBuyCard :
				break;
			case UseMoneyType.YesDoCD :
				cBlood = 12 - num1;
				break;
			case UseMoneyType.Sell :
				costMoney = GetCostMoney(itemID , num1);
				cGold = costMoney[0] * (-1)  +  costMoney[1] * 500 * (-1);
				break;
			case UseMoneyType.Buy :
				break;
			case UseMoneyType.YesOpenCangKu :
				cBlood = num1 * 10;
				break;
			case UseMoneyType.CostGold :
				cGold = num1;
				break;
			case UseMoneyType.OpenSpreeItemAsID :
				costMoney = GetCostMoneyInTablePacks(itemID);
				cGold = costMoney[0] * (-1);
				cBlood = costMoney[1] * (-1);
				break;
			case UseMoneyType.YesOpenPackage  :
				cBlood = num1 * 10;
				break;
			case UseMoneyType.YAddGold :
				cGold = num1 * (-1);
				break;
			case UseMoneyType.YAddBlood  :
				cBlood = num1 * (-1);
				break;
			case UseMoneyType.YesPVP8Times :
				cBlood = 5;
				break;
			case UseMoneyType.YesPVPTimes  :
				cBlood = 5;
				break;
			case UseMoneyType.Resurrection2  :
				cGold = (num1 / 5 + 2) * 800;
				break;
			case UseMoneyType.Resurrection3 :
				cBlood = (num1 / 5 + 2)*2;
				break;
			case UseMoneyType.mmm31  :
				cBlood = -60;
				break;
			case UseMoneyType.mmm32 :
				cBlood = -200;
				break;
			case UseMoneyType.mmm33 :
				cBlood = -900;
				break;
			case UseMoneyType.mmm34  :
				cBlood = -3500;
				break;
			case UseMoneyType.UpDateOneSkill :
				SetCostPersonSkills();
				cBlood = costPersonSkills[num1].gold[num2];
				cGold =  costPersonSkills[num1].blood[num2];
				break;
			case UseMoneyType.UpDateOneSoulItemSkill :
				cGold = num1 * num2 * 200;
				break;
			case UseMoneyType.ButtonBuildSoulAndDigest :
				SetBuildSDCostMoney();
				cGold = BuildSDCostMoney[num1];
				break;
			case UseMoneyType.NumDigestButtonsPlus :
				cBlood = 5;
				break;
			case UseMoneyType.PlusBottle :
				cGold = num1 * num2 * 5 + 300;
				break;
			case UseMoneyType.ApplyPickup1  :
				break;
			case UseMoneyType.ApplyPickup2 :
				break;
			case UseMoneyType.TaskDone :
				break;
			case UseMoneyType.SpendingReturns11 :
				cBlood = 5;
				break;
			case UseMoneyType.RaidsStart  :
				cBlood = 10;
				break;
			case UseMoneyType.CostRaidsNow :
				cBlood = num1;
				break;
			case UseMoneyType.Cost :
				break;
			case UseMoneyType.UpLevelBottle :
				cBlood = num1 * 10;
				break;
			case UseMoneyType.PlusBottle1  :
				cGold = num1 * num2 * 5 + 300;
				break;
			case UseMoneyType.FullBottle :
				cBlood = num1;
				break;
			case UseMoneyType.YesReturn :
				cBlood = 2;
				break;
			case UseMoneyType.YesManufacture :
				cBlood = 20;
				break;
			case UseMoneyType.YesSoul  :
				cBlood = 30;
				break;
			case UseMoneyType.YesGem :
				cBlood = 50;
				break;
			case UseMoneyType.YesDuplicate :
				cBlood = 50;
				break;
			case UseMoneyType.YesResetPVP :
				cBlood = 50;
				break;
			case UseMoneyType.yesCook :
				cBlood = 2;
				break;
			case UseMoneyType.YesBuy :
				cBlood = 50;
				break;
			case UseMoneyType.Getreward :
				cGold = (num1 * 20 * Random.Range(50,150) / 100) * (-1);
				break;
			case UseMoneyType.doneCard :
				break;
		}
	}
	public int[] BuildSDCostMoney;
	/// <summary>
	/// 配置生产魔魂花费
	/// </summary>
	void SetBuildSDCostMoney(){
		if(BuildSDCostMoney == null){
			BuildSDCostMoney = new int[6];
			BuildSDCostMoney[0] = 0;
			BuildSDCostMoney[1] = 500;
			BuildSDCostMoney[2] = 1000;
			BuildSDCostMoney[3] = 5000;
			BuildSDCostMoney[4] = 7000;
			BuildSDCostMoney[5] = 10000;
		}
	}

	public PersonSkill[] costPersonSkills;
	/// <summary>
	/// 配置学习技能花费
	/// </summary>
	void SetCostPersonSkills(){
		if(costPersonSkills == null){
			costPersonSkills = new PersonSkill[23];
			//			技能0
			costPersonSkills[0].gold[0] = 0;
			costPersonSkills[0].gold[1] = 1000;
			costPersonSkills[0].gold[2] = 10000;
			costPersonSkills[0].blood[0] = 0;
			costPersonSkills[0].blood[1] = 0;
			costPersonSkills[0].blood[2] = 0;
			//			技能1
			costPersonSkills[1].gold[0] = 200;
			costPersonSkills[1].gold[1] = 1000;
			costPersonSkills[1].gold[2] = 10000;
			costPersonSkills[1].blood[0] = 0;
			costPersonSkills[1].blood[1] = 0;
			costPersonSkills[1].blood[2] = 0;
			//			技能2
			costPersonSkills[2].gold[0] = 200;
			costPersonSkills[2].gold[1] = 2000;
			costPersonSkills[2].gold[2] = 10000;
			costPersonSkills[2].blood[0] = 0;
			costPersonSkills[2].blood[1] = 0;
			costPersonSkills[2].blood[2] = 0;
			//			技能3
			costPersonSkills[3].gold[0] = 200;
			costPersonSkills[3].gold[1] = 1000;
			costPersonSkills[3].gold[2] = 10000;
			costPersonSkills[3].blood[0] = 2;
			costPersonSkills[3].blood[1] = 20;
			costPersonSkills[3].blood[2] = 100;
			//			技能4
			costPersonSkills[4].gold[0] = 200;
			costPersonSkills[4].gold[1] = 1000;
			costPersonSkills[4].gold[2] = 10000;
			costPersonSkills[4].blood[0] = 2;
			costPersonSkills[4].blood[1] = 20;
			costPersonSkills[4].blood[2] = 100;
			//			技能5
			costPersonSkills[5].gold[0] = 200;
			costPersonSkills[5].gold[1] = 1000;
			costPersonSkills[5].gold[2] = 10000;
			costPersonSkills[5].blood[0] = 2;
			costPersonSkills[5].blood[1] = 20;
			costPersonSkills[5].blood[2] = 100;
			//			技能6
			costPersonSkills[6].gold[0] = 200;
			costPersonSkills[6].gold[1] = 1000;
			costPersonSkills[6].gold[2] = 10000;
			costPersonSkills[6].blood[0] = 2;
			costPersonSkills[6].blood[1] = 20;
			costPersonSkills[6].blood[2] = 100;
			//			技能7
			costPersonSkills[7].gold[0] = 200;
			costPersonSkills[7].gold[1] = 1000;
			costPersonSkills[7].gold[2] = 10000;
			costPersonSkills[7].blood[0] = 10;
			costPersonSkills[7].blood[1] = 50;
			costPersonSkills[7].blood[2] = 200;
			//			技能8
			costPersonSkills[8].gold[0] = 200;
			costPersonSkills[8].gold[1] = 1000;
			costPersonSkills[8].gold[2] = 10000;
			costPersonSkills[8].blood[0] = 10;
			costPersonSkills[8].blood[1] = 50;
			costPersonSkills[8].blood[2] = 200;
			//			技能9
			costPersonSkills[9].gold[0] = 200;
			costPersonSkills[9].gold[1] = 1000;
			costPersonSkills[9].gold[2] = 10000;
			costPersonSkills[9].blood[0] = 2;
			costPersonSkills[9].blood[1] = 20;
			costPersonSkills[9].blood[2] = 100;
			//			技能10
			costPersonSkills[10].gold[0] = 200;
			costPersonSkills[10].gold[1] = 1000;
			costPersonSkills[10].gold[2] = 10000;
			costPersonSkills[10].blood[0] = 2;
			costPersonSkills[10].blood[1] = 20;
			costPersonSkills[10].blood[2] = 100;
			//			技能11
			costPersonSkills[11].gold[0] = 200;
			costPersonSkills[11].gold[1] = 1000;
			costPersonSkills[11].gold[2] = 10000;
			costPersonSkills[11].blood[0] = 2;
			costPersonSkills[11].blood[1] = 20;
			costPersonSkills[11].blood[2] = 100;
			//			技能12
			costPersonSkills[12].gold[0] = 200;
			costPersonSkills[12].gold[1] = 1000;
			costPersonSkills[12].gold[2] = 10000;
			costPersonSkills[12].blood[0] = 2;
			costPersonSkills[12].blood[1] = 20;
			costPersonSkills[12].blood[2] = 100;
			//			技能13
			costPersonSkills[13].gold[0] = 200;
			costPersonSkills[13].gold[1] = 1000;
			costPersonSkills[13].gold[2] = 10000;
			costPersonSkills[13].blood[0] = 10;
			costPersonSkills[13].blood[1] = 50;
			costPersonSkills[13].blood[2] = 200	;
			//			技能14
			costPersonSkills[14].gold[0] = 200;
			costPersonSkills[14].gold[1] = 1000;
			costPersonSkills[14].gold[2] = 10000;
			costPersonSkills[14].blood[0] = 10;
			costPersonSkills[14].blood[1] = 50;
			costPersonSkills[14].blood[2] = 200;
			//			技能15
			costPersonSkills[15].gold[0] = 200;
			costPersonSkills[15].gold[1] = 1000;
			costPersonSkills[15].gold[2] = 10000;
			costPersonSkills[15].blood[0] = 0;
			costPersonSkills[15].blood[1] = 0;
			costPersonSkills[15].blood[2] = 0;
			//			技能16
			costPersonSkills[16].gold[0] = 200;
			costPersonSkills[16].gold[1] = 1000;
			costPersonSkills[16].gold[2] = 10000;
			costPersonSkills[16].blood[0] = 0;
			costPersonSkills[16].blood[1] = 0;
			costPersonSkills[16].blood[2] = 0;
			//			技能17
			costPersonSkills[17].gold[0] = 200;
			costPersonSkills[17].gold[1] = 1000;
			costPersonSkills[17].gold[2] = 10000;
			costPersonSkills[17].blood[0] = 0;
			costPersonSkills[17].blood[1] = 0;
			costPersonSkills[17].blood[2] = 0;
			//			技能18
			costPersonSkills[18].gold[0] = 200;
			costPersonSkills[18].gold[1] = 1000;
			costPersonSkills[18].gold[2] = 10000;
			costPersonSkills[18].blood[0] = 0;
			costPersonSkills[18].blood[1] = 0;
			costPersonSkills[18].blood[2] = 0;
			//			技能19
			costPersonSkills[19].gold[0] = 200;
			costPersonSkills[19].gold[1] = 1000;
			costPersonSkills[19].gold[2] = 10000;
			costPersonSkills[19].blood[0] = 0;
			costPersonSkills[19].blood[1] = 0;
			costPersonSkills[19].blood[2] = 0;
			//			技能20
			costPersonSkills[20].gold[0] = 200;
			costPersonSkills[20].gold[1] = 1000;
			costPersonSkills[20].gold[2] = 10000;
			costPersonSkills[20].blood[0] = 0;
			costPersonSkills[20].blood[1] = 0;
			costPersonSkills[20].blood[2] = 0;
			//			技能21
			costPersonSkills[21].gold[0] = 200;
			costPersonSkills[21].gold[1] = 1000;
			costPersonSkills[21].gold[2] = 10000;
			costPersonSkills[21].blood[0] = 0;
			costPersonSkills[21].blood[1] = 0;
			costPersonSkills[21].blood[2] = 0;
			//			技能22
			costPersonSkills[22].gold[0] = 200;
			costPersonSkills[22].gold[1] = 1000;
			costPersonSkills[22].gold[2] = 10000;
			costPersonSkills[22].blood[0] = 0;
			costPersonSkills[22].blood[1] = 0;
			costPersonSkills[22].blood[2] = 0;
		}
	}

	/// <summary>
	/// 获取礼包中的血石金币
	/// </summary>
	/// <returns>The cost money in table packs.</returns>
	/// <param name="lastStr">礼包id</param>
	int[] GetCostMoneyInTablePacks(string lastStr){
		int[] money = new int[2];
		money[0] = 0;
		money[1] = 0;
		foreach(yuan.YuanMemoryDB.YuanRow rows in YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytTablePacks.Rows){
			if(rows["id"].YuanColumnText == lastStr){
				money[1] = (rows["BloodStrone"].YuanColumnText != "") ? int.Parse(rows["BloodStrone"].YuanColumnText) : 0;
				money[0] = (rows["Cash"].YuanColumnText != "") ? int.Parse(rows["Cash"].YuanColumnText) : 0;
			}
		}
		return money;
	}

	/// <summary>
	/// 获取物品价值血石金币
	/// </summary>
	/// <returns>The item identifier as string.</returns>
	/// <param name="selectStr">Select string.</param>
	/// <param name="selectID">Select I.</param>
	/// <param name="str">String.</param>
	string GetItemIDAsStr(string selectStr , string selectID , string str){
		foreach(yuan.YuanMemoryDB.YuanRow rows in YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytGameItem.Rows){
			if(rows[selectStr].YuanColumnText == selectID){
				return rows[str].YuanColumnText;
			}
		}
		return "";
	}

	/// <summary>
	/// 获取装备品质
	/// </summary>
	/// <returns>The quality.</returns>
	/// <param name="qua">Qua.</param>
	/// <param name="lv">Lv.</param>
	int GetQuality(int qua , int lv){ 
		if(qua >= 6){
			qua -= 4;
		}
		switch(qua){
			case 1: return lv*2;
			case 2: return lv*3+6;
			case 3: return lv*7+12;
			case 4: return lv*12+24;
			case 5: return lv*15+56;
		}
		return 1;
	}

	float[] itemXiuZheng;
	/// <summary>
	/// 获取装备价值血石金币
	/// </summary>
	/// <returns>The cost money.</returns>
	/// <param name="ItemID">装备id</param>
	/// <param name="proID">角色职业</param>
	int[] GetCostMoney(string ItemID , int proID){
		int[] money = new int[2];
		money[0] = 0;
		money[1] = 0;

		if(itemXiuZheng == null){
			itemXiuZheng = new float[11];
			itemXiuZheng[0] = 1f;
			itemXiuZheng[1] = 1.2f;
			itemXiuZheng[2] = 0.75f;
			itemXiuZheng[3] = 0.75f;
			itemXiuZheng[4] = 1f;
			itemXiuZheng[5] = 0.8f;
			itemXiuZheng[6] = 0.75f;
			itemXiuZheng[7] = 0.7f;
			itemXiuZheng[8] = 0.7f;
			itemXiuZheng[9] = 1.2f;
			itemXiuZheng[10] = 1.2f;
		}

		string useID = "";
		int ItemBlood = 0;
		int level = 0;
		int pstr1 = 0;
		int pItemCask = 0;
		int slotType = 0;

		try{
			int usePid = 0;
			if(ItemID.Substring(0,1) == "x"){
				ItemID = proID + ItemID.Substring(1,ItemID.Length - 1);
			}else
			if(ItemID.Substring(0,1) == "y"){
				ItemID = (proID + 3) + ItemID.Substring(1,ItemID.Length - 1);
			}
			ItemID = ItemID.Replace("-" , "");
			bool isXiaoHao = false;
			if(ItemID.Substring(0,1) != "J"){
				usePid = int.Parse(ItemID.Substring(0,1));	
			}else{
				return money;				
			}
			//	//print(ItemID + " == 1");
			
			if(usePid <= 3){  
				slotType = int.Parse(ItemID.Substring(1,1)) + 9;  
			}else
			if(usePid <= 6){ 
				slotType = int.Parse(ItemID.Substring(1,1));	
			}else
			if(usePid == 9){
				return money;
			}else
			if(usePid == 7){
				return money;
			}else
			{
				isXiaoHao = true;	
			}

			if(isXiaoHao){
				useID = GetItemIDAsStr("ItemID" , ItemID.Substring(0,4) , "LevelID");
				level = (useID != "") ? int.Parse(useID) : 1;
				useID = GetItemIDAsStr("ItemID" , ItemID.Substring(0,4) , "ItemLevel");
				pstr1 = (useID != "") ? int.Parse(useID) : 1;
				useID = GetItemIDAsStr("ItemID" , ItemID.Substring(0,4) , "ItemCash");
				pItemCask = (useID != "") ? int.Parse(useID) : 1;
				useID = GetItemIDAsStr("ItemID" , ItemID.Substring(0,4) , "ItemBlood");
				ItemBlood = (useID != "") ? int.Parse(useID) : 0;

				money[0] = level * 100 * int.Parse(ItemID.Substring(5,2)) * pstr1 * pItemCask;
				if(ItemID.Substring(0 , 2) == "82"){
					money[0] = money[0] / 2;
				}
				if(ItemBlood != 0){
					money[1] = int.Parse(ItemID.Substring(5,2)) * ItemBlood;
				}
			}
			else
			{
				if(int.Parse(ItemID.Substring(4,1)) == 0){
					if(slotType == 11){
						slotType = 10;
					}
					money[0] = (int)(200 + int.Parse(ItemID.Substring(2,2)) *  GetQuality(3 , int.Parse(ItemID.Substring(2,2)))*0.5f + itemXiuZheng[slotType - 1]);
					money[1] = (int)(Mathf.Clamp(int.Parse(ItemID.Substring(2,2)) * GetQuality(3 , int.Parse(ItemID.Substring(2,2))) * 0.02f , 1 , 9999999));
					return money;
				}
				money[0] = (int)(200 + (int.Parse(ItemID.Substring(2,2)) * GetQuality(int.Parse(ItemID.Substring(4,1)) , int.Parse(ItemID.Substring(2,2))) +  int.Parse(ItemID.Substring(15,3)) )*0.5f + itemXiuZheng[slotType - 1]);
				int itemQ = 0;
				itemQ = int.Parse(ItemID.Substring(4,1));
				if(itemQ == 2 || itemQ == 3 || itemQ == 4 || itemQ == 5 || itemQ == 7 || itemQ == 8 || itemQ == 9){
					money[1] = (int)(Mathf.Clamp(int.Parse(ItemID.Substring(2,2)) * GetQuality(int.Parse(ItemID.Substring(4,1)) , int.Parse(ItemID.Substring(2,2))) * 0.004f , 1 , 9999999));
				}
			}
			return money;
		}catch(System.Exception e){
			return money;
		}
	}
}

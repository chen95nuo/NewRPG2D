//using UnityEngine;
//using System.Collections;
//
//public class EquepmentBrokenDown : MonoBehaviour {
//		
//	void BrokenDownAsID(string itemID , bool useBlood){
////		InventoryItem inv = new InventoryItem(); 
////		inv = GetItemInfo(itemID);  //获得装备属性 等级 品质
//		int ranInt = 0;
//		ranInt = Random.Range(0,100);
//		int[] consumableValue;
////		consumableValue = GetBrokenMaterial(inv.itemLevel , inv.itemQuality);
//		int m = 0;
//		int plusBuild = 0;
//		switch (inv.itemQuality)
//		{
//		case 1: m = 0; break;
//		case 2: m = 4; break;
//		case 3: m = 8; break;
//		case 4: m = 12; break;
//		case 5: m = 12; break;
//		}
//		plusBuild = int.Parse(inv.itemBuild) / m;
//
//		consumableValue[0] *= plusBuild;
//		consumableValue[1] *= plusBuild;
//
//		if(useBlood){
//			consumableValue[0] *= 2;
//			consumableValue[1] *= 2;
//		}else
//		if(ranInt > 80){
//			int multiOne = Random.Range(12,23);
//			consumableValue[0] = consumableValue[0] * multiOne / 10;
//			consumableValue[1] = consumableValue[1] * multiOne / 10;
//		}
////		consumableValue = new int[2]; //0精铁,1精金,加到人身上
////		删除装备id为itemID的装备
//	}
//
//	/// <summary>
//	/// 获取消耗精铁，精金数量。
//	/// </summary>
//	/// <returns>精铁，精金数组</returns>
//	/// <param name="level">装备等级</param>
//	/// <param name="quality">装备品质</param>
//	int[] GetBrokenMaterial(int level , int quality){
//		yuan.YuanMemoryDB.YuanRow row = null;
//		int[] consumableValue = new int[2]; //0精铁，1精金
//		if (YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytEquipmentresolve.Rows.TryGetValue(level.ToString(), out row)){
//			switch(quality){
//				case 1:
//					consumableValue[0] = int.Parse(rows["IronWhite"].YuanColumnText);
//					consumableValue[1] = int.Parse(rows["GoldWhite"].YuanColumnText);
//					break;
//				case 1:
//					consumableValue[0] = int.Parse(rows["IronGreen"].YuanColumnText);
//					consumableValue[1] = int.Parse(rows["GoldGreen"].YuanColumnText);
//					break;
//				case 1:
//					consumableValue[0] = int.Parse(rows["IronBlue"].YuanColumnText);
//					consumableValue[1] = int.Parse(rows["GoldBlue"].YuanColumnText);
//					break;
//				case 1:
//					consumableValue[0] = int.Parse(rows["IronPurple"].YuanColumnText);
//					consumableValue[1] = int.Parse(rows["GoldPurple"].YuanColumnText);
//					break;
//				case 1:
//					consumableValue[0] = int.Parse(rows["IronOrange"].YuanColumnText);
//					consumableValue[1] = int.Parse(rows["GoldOrange"].YuanColumnText);
//					break;
//			}
//		}
//		return consumableValue;
//	}
//}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RechargeUiResultJson : ErrorJson {
	
	public int vipLv{get;set;}		//vip等级//
	public int vipCost{get;set;}		//vip当前经验//
	public int sCost{get;set;}		//vip升至下一级的总经验值//
	public int vipMonthType{get;set;}		//是否时月卡玩家，0否1是//
	public int vipMonthDay{get;set;}		//如果是月卡玩家，月卡剩余天数//
	public List<string> ids;				//需要显示描述的充值id//
	public List<string> giftIds;		//可以购买的vip礼包ids//
}

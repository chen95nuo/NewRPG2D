using UnityEngine;
using System.Collections;

public class GiftCodeResultJson : ErrorJson {

	public int gold{get;set;}	//金币//
	public int crystal{get;set;}	//水晶//
	public int runeNum{get;set;}	//符文值//
	public int power{get;set;}	//体力//
	public string card{get;set;}	//格式:cardId-cardNum,cardId-cardNum//
	public string skill{get;set;}	//主动技能:skillId-Num,skillId-Num//
	public string pSkill{get;set;}	//被动技能:pSkillId-Num,pSkillId-Num//
	public string equip{get;set;}	//装备:equipId-Num,equipId-Num//
	public string item{get;set;}	//材料,即道具:itemId-Num,itemId-Num//
}

using System;
using System.Collections;
using UnityEngine;

public class SkillData :PropertyReader
{
	
	public int index{get;set;}
	public int number{get;set;}
	public string name{get;set;}
	public int exptype{get;set;}
	public int type{get;set;}
	public int star{get;set;}
	public int level{get;set;}
	public int exp{get;set;}
	public int atkTarget{get;set;}
	public int element{get;set;}
	public int view{get;set;}
	public float viewNum{get;set;}
	public float atkNumbers{get;set;}
	public string description{get;set;}
	public string upgradetext{get;set;}
	public int sell{get;set;}
	public string icon{get;set;}
	public string icon2{get;set;}
	//==释放音效==//
	public string music{get;set;}
	//==蓄力音效==//
	public string chargemusic{get;set;}
	public int atkAction{get;set;}
	public int position{get;set;}
	public int background{get;set;}
	public int atkCharge{get;set;}
	public string atkChargeSE{get;set;}
	public float atkChargeSETime{get;set;}
	public float atkChargeENDTime{get;set;}
	public int atkChargeSEPositionType{get;set;}
	public int spawn{get;set;}
	public string spawnID{get;set;}
	public float spawnDelayTime{get;set;}
	public float spawnTime{get;set;}
	public float spawnENDTime{get;set;}
	public int spawnPositionType{get;set;}
	public string hurtSE{get;set;}
	//==受伤音效==//
	public string hurtMusic{get;set;}
	public float hurtSETime{get;set;}
	public float hurtENDTime{get;set;}
	public int hurtSEPositionType{get;set;}
	public int defTarget{get;set;}
	public float numberX{get;set;}
	public float numberY{get;set;}
	public float numberZ{get;set;}
	public int defAction{get;set;}
	public string defActionSE{get;set;}
	public float defActionSETime{get;set;}
	public float defActionENDTime{get;set;}
	public int defActionSEPositionType{get;set;}
	public string defTargetSE{get;set;}
	public float defTargetSETime{get;set;}
	public float defTargetENDTime{get;set;}
	public int defTargetSEPositionType{get;set;}
	public string defSE{get;set;}
	public float defSETime{get;set;}
	public float defENDTime{get;set;}
	public int defSEPositionType{get;set;}
	public int healTarget{get;set;}
	public float healNumbers{get;set;}
	public int healAction{get;set;}
	public string healActionSE{get;set;}
	public float healActionSETime{get;set;}
	public float healActionENDTime{get;set;}
	public int healActionSEPositionType{get;set;}
	public int healCharge{get;set;}
	public string healChargeSE{get;set;}
	public float healChargeSETime{get;set;}
	public float healChargeENDTime{get;set;}
	public int healChargeSEPositionType{get;set;}
	public string healedSE{get;set;}
	public float healedSETime{get;set;}
	public float healedENDTime{get;set;}
	public int healedSEPositionType{get;set;}

	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(index,this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss)
	{
		
	}
	
	public static SkillData getData(int skillId)
	{
		foreach(DictionaryEntry de in data)
		{
			if((int)de.Key==skillId)
			{
				return (SkillData)de.Value;
			}
		}
		return null;
	}
	
	public String getElementName()
	{
		int k=0;
		switch(element)
		{
		case 0:
			k=35;
			break;
		case 1:
			k=36;
			break;
		case 2:
			k=37;
			break;
		case 3:
			k=38;
			break;
		case 4:
			k=39;
			break;
		}
		return TextsData.getData(k).chinese;
	}
	
}



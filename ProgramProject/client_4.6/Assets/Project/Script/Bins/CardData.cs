using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardData : PropertyReader {
	
	public int id{get;set;}
	public int number{get;set;}
	public string name{get;set;}
	public int star{get;set;}
	public int maxLevel{get;set;}
	public int race{get;set;}
	public int element{get;set;}
	//==蓄力声音==//
	public string chargeVoice{get;set;}
	//==受伤声音==//
	public string hurtVoice{get;set;}
	public float atk{get;set;}
	public float def{get;set;}
	public float hp{get;set;}
	public int criRate{get;set;}
	public int aviRate{get;set;}
	public int talent{get;set;}
	public int talent2{get;set;}
	public int talent3{get;set;}
    public int talent4 { get; set; }
	public float talentpower{get;set;}
	public int type{get;set;}
	public float castTimeDelay{get;set;}
	public int shadow{get;set;}
	public string atlas{get;set;}
	public string icon{get;set;}
	public int basicskill{get;set;}
	public string cardmodel{get;set;}
	public float modelsize{get;set;}
	public float scalenum{get;set;}
	public float modelposition{get;set;}
	public float modelrotation{get;set;}
	public int sex{get;set;}
	public int sell{get;set;}
	public int exp{get;set;}
	public string description{get;set;}
	public string waytoget{get;set;}

    public int PSSskilltype1 { get; set; }

    public int PSSparameter11 { get; set; }

    public float PSSparameter12 { get; set; }

    public int PSSparameter13 { get; set; }

    public int PSSparameter14 { get; set; }

    public int PSSparameter15 { get; set; }

    public int PPSprobability1 { get; set; }

    public int PSSskilltype2 { get; set; }

    public int PSSparameter21 { get; set; }

    public float PSSparameter22 { get; set; }

    public int PSSparameter23 { get; set; }

    public int PSSparameter24 { get; set; }

    public int PSSparameter25 { get; set; }

    public int PPSprobability2 { get; set; }

    public string PSSicon { get; set; }

    public string PSSeffective { get; set; }

    public string PSSresult { get; set; }

    public string PSSvioce { get; set; }

    public float PSStime { get; set; }

    public string PPSdescription { get; set; }

    public string PPSname { get; set; }

	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id, this);
	}

	public void resetData()
	{
		data.Clear();
	}

	public static CardData getData(int cardId)
	{
		return (CardData)data[cardId];
	}
	
    public void parse(string[] ss)
    {
    }
	
	public static string getAtlas(string icon)
	{
		foreach(DictionaryEntry de in data)
		{
			CardData cd=(CardData)de.Value;
			if(cd.icon.Equals(icon))
			{
				return cd.atlas;
			}
		}
		return null;
	}
	
	public bool isHaveRestrain(STATE.SKILL_TYPE skillType)
	{
	 	if(PSSskilltype1 != 4)
			return false;
		if((STATE.SKILL_TYPE)PSSparameter11 == STATE.SKILL_TYPE.E_All)
			return true;
		if((STATE.SKILL_TYPE)PSSparameter11 == skillType)
		{
			return true;
		}
		return false;
	}
	
	public float getRestrainNum(float demage)
	{
		float r = demage; 
		r = demage* (1 - PSSparameter12);
		return r;
	}
	
	public bool isCanDoForbitAttack()
	{
		if(PSSskilltype2 == 1)
			return true;
		return false;
	}
	
	public int getForbitAttackCountOrReduceEnergy()
	{
		return PSSparameter21;
	}
	
	public float getPSEffectNum()
	{
		//return -10f;
		return PSSparameter22;
	}
	
	public bool isCanReduceEnergy()
	{
		if(PSSskilltype2 == 3)
			return true;
		return false;
	}
	
	public bool isCanBacklashDemage()
	{
		if(PSSskilltype2 == 2)
			return true;
		return false;
	}
	
	
}

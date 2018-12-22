using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PackElementJson
{
	public PackElement pe;
	public int type; // 0 - no tip, 1 - N tip, 2 UP tip
}

public class CGPJson
{
	public List<PackElement> passiveSkills;
}

public class CardGroupResultJson:ErrorJson
{
	//public List<PackElement> cs;
	public List<PackElementJson> cs;
	
	public List<PackElement> ss;
	public List<CGPJson> ps;
	public List<CGEJson> es;
	public int unitId;
	
	public List<string> ics;
	public List<string> iss;
	public List<string> ips;
	public List<string> ies;
	public int unit;			//信和体积提醒，1有新合体技， 0 没有新合体技//
	public CardGroup transformCardGroup()
	{
		CardGroup cg=new CardGroup();
		//units
		cg.unitSkillId=unitId;
		if(cs!=null && ics!=null)
		{
			for(int i=0;i<cs.Count;i++)
			{
//				Debug.Log("************************ " + StringUtil.getInt(ics[i]).ToString());
				cg.cards[StringUtil.getInt(ics[i])]=cs[i].pe;
				cg.cardTips[StringUtil.getInt(ics[i])] = cs[i].type;
			}
		}
		//skill
		if(ss!=null && iss!=null)
		{
			for(int i=0;i<ss.Count;i++)
			{
				cg.skills[StringUtil.getInt(iss[i])]=ss[i];
			}
		}
		//pSkill
		if(ps!=null && ips!=null)
		{
			for(int i=0;i<ps.Count;i++)
			{
				cg.passiveSkills[StringUtil.getInt(ips[i])]=ps[i].passiveSkills;
			}
		}
		//equip
		if(es!=null && ies!=null)
		{
			for(int i=0;i<es.Count;i++)
			{
				cg.equips[StringUtil.getInt(ies[i])]=es[i].equips;
			}
		}
		
		return cg;
	}
}



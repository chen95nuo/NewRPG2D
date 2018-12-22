using UnityEngine;
using System.Collections.Generic;

public class SCPSJson
{
	public List<int> ps;
	public SCPSJson()
	{
		ps = new List<int>();
	}
};

public class SaveCGJson:BasicJson
{
	public int[] ics;//角色卡索引//
	public int[] iss;//主动技能索引//
	
	//public int[] ips;//被动技能索引//
	
	public SCPSJson[] ips;//被动技能索引//

	public SCGEJson[] ies;//装备索引//
	public int us;//合体技id//
	
	public SaveCGJson(CardGroup cg)
	{
		//card//
		ics=new int[6];
		for(int k=0;k<cg.cards.Length;k++)
		{
			PackElement pe=cg.cards[k];
			if(pe==null)
			{
				ics[k]=-1;
			}
			else
			{
				ics[k]=pe.i;
			}
		}
		//skill
		iss=new int[6];
		for(int k=0;k<cg.skills.Length;k++)
		{
			PackElement pe=cg.skills[k];
			if(pe==null)
			{
				iss[k]=-1;
			}
			else
			{
				iss[k]=pe.i;
			}
		}
		//pskill
		ips=new SCPSJson[6];
		for(int k=0;k<cg.passiveSkills.Length;k++)
		{
			List<PackElement> list=cg.passiveSkills[k];
			if(list != null)
			{
				ips[k] = new SCPSJson();
				for(int m = 0; m < list.Count;++m)
				{
					if(list[m] == null)
					{
						ips[k].ps.Add(-1);
					}
					else
					{
						ips[k].ps.Add(list[m].i);
					}
				}
			}
		}
		//equips
		ies=new SCGEJson[6];
		for(int k=0;k<cg.equips.Length;k++)
		{
			List<PackElement> list=cg.equips[k];
			if(list!=null)
			{
				ies[k]=new SCGEJson();
				for(int m=0;m<list.Count;m++)
				{
					ies[k].es.Add(list[m].i);
				}
			}
		}
		//units
		us=cg.unitSkillId;
	}
}

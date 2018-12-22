using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CardGroup : ICloneable
{
	/**长度为6,某个位置没有卡则为null**/
	public PackElement[] cards=new PackElement[6];
	public int[] cardTips = new int[6]; // card tip, 0 - no tip, 1 - N tip , 2 - UP tip
	
	public PackElement[] skills=new PackElement[6];
	public List<PackElement>[] passiveSkills=new List<PackElement>[6]; 
	/*一个卡组界面中对应的每张卡牌*/
	public List<PackElement>[] equips=new List<PackElement>[6];
	//每个卡组所选择的的合体技//
	public int unitSkillId;
	// 0 未修改，1 已修改
	public int changeMark = 0;
	
	public List<string> getEquipInfo(int index)
	{
		if(index<0 || index>=6)
		{
			return null;
		}
		List<PackElement> equip=equips[index];
		List<string> result=new List<string>();
		foreach(PackElement dbe in equip)
		{
			result.Add(dbe.dataId+"-"+dbe.lv);
		}
		return result;
	}
	
	public List<int> getPassiveSkillIDList(int index)
	{
		if(index < 0 || index >= 6)
		{
			return null;
		}
		List<PackElement> psList = passiveSkills[index];
		List<int> result = new List<int>();
		for(int i = 0; i < psList.Count; ++i)
		{
			result.Add(psList[i].dataId);
		}
		return result;
	}
		
    public List<PackElement> GetPc()
    { 
        List<PackElement> pc = new List<PackElement>();
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
            {
                pc.Add(cards[i]);
            }
        }
        return pc;
    }
	/**是否可参战**/
	public int getCardNum()
	{
		int result=0;
		foreach(PackElement c in cards)
		{
			if(c!=null)
			{
				result++;
			}
		}
		return result;
	}
	/**获取本卡组的cardIds**/
	public List<int> getCardIds()
	{
		List<int> result=new List<int>();
		foreach(PackElement c in cards)
		{
			if(c!=null)
			{
				result.Add(c.dataId);
			}
		}
		return result;
	}
	
	public int getIndex(PackElement card)
	{
		for(int i=0;i<cards.Length;i++)
		{
			if(card != null && cards[i]!= null && cards[i].i==card.i)
			{
				return i;
			}
		}
		return -1;
	}
	
	object ICloneable.Clone() 
    { 
        return this.Clone(); 
    } 
    public CardGroup Clone() 
    { 
		CardGroup temp = new CardGroup();
		for(int i = 0; i < this.cards.Length; ++i)
		{
			if(this.cards[i] == null)
				continue;
			temp.cards[i] = this.cards[i].Clone();
		}
		
		for(int i = 0; i < this.cardTips.Length; ++i)
		{
			temp.cardTips[i] = this.cardTips[i];
		}
		
		for(int i = 0; i < this.skills.Length; ++i)
		{
			if(this.skills[i] == null)
				continue;
			temp.skills[i] = this.skills[i].Clone();
		}
		
		for(int i = 0; i < this.passiveSkills.Length;++i)
		{
			if(this.passiveSkills[i] != null)
			{
				if(temp.passiveSkills[i] == null)
				{
					temp.passiveSkills[i] = new List<PackElement>();
					for(int j = 0; j < this.passiveSkills[i].Count; ++j)
					{
						temp.passiveSkills[i].Add(null);
					}
				}
				for(int j = 0; j < this.passiveSkills[i].Count;++j)
				{
					if(this.passiveSkills[i][j] == null)
						continue;
					temp.passiveSkills[i][j] = this.passiveSkills[i][j].Clone();
				}
			}
		}
		
		for(int i = 0; i < this.equips.Length;++i)
		{
			if(this.equips[i] != null)
			{
				if(temp.equips[i] == null)
				{
					temp.equips[i] = new List<PackElement>();
					for(int j = 0; j < this.equips[i].Count; ++j)
					{
						temp.equips[i].Add(null);
					}
				}
				for(int j = 0; j < this.equips[i].Count;++j)
				{
					if(this.equips[i][j] == null)
						continue;
					temp.equips[i][j] = this.equips[i][j].Clone();
				}
			}
		}
		
		temp.unitSkillId = this.unitSkillId;
		temp.changeMark = this.changeMark;


        return temp; 
    }
}

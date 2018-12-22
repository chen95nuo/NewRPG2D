using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UnitSkillData:PropertyReader
{
	
	public int index{get;set;}
	public int number{get;set;}
	public string name{get;set;}
	public int type{get;set;}
	public int cardtype{get;set;}
	public int cardnum{get;set;}
	public int cardid{get;set;}
	public int card1{get;set;}
	public int card2{get;set;}
	public int card3{get;set;}
	public int card4{get;set;}
	public int card5{get;set;}
	public int card6{get;set;}
	public int card7{get;set;}
	public int card8{get;set;}
	public string icon{get;set;}
	public int cost{get;set;}
	public int aim{get;set;}
	public int power{get;set;}
	public string description{get;set;}
	public int effect1{get;set;}
	public int effect2{get;set;}
	public int effect3{get;set;}
	public int probability{get;set;}
	public int lastRND{get;set;}
	public string nameicon{get;set;}
	public string nameeffect{get;set;}
	public string backgroundColor{get;set;}
	public string chargeCameraMove{get;set;}
	public int chargePW{get;set;}
	public string chargeEffect{get;set;}
	//==蓄力音效==//
	public string chargeMusic{get;set;}
	public float chargeEffectTime{get;set;}
	public float chargePWSETime{get;set;}
	public int attackType{get;set;}
	public string castEffect{get;set;}
	public int castPostion{get;set;}
	public string castCameraMove{get;set;}
	public int zoom{get;set;}
	public float castPWSETime{get;set;}
	public string actionCameraMove{get;set;}
	public int actionPositionType{get;set;}
	public float actionPrepareTime{get;set;}
	public string actionSETime{get;set;}
	//==攻击音效==//
	public string actionMusic{get;set;}
	public string music{get;set;}
	public float actionEffectTime{get;set;}
	public float actionPWSETime{get;set;}
	public int screenShake{get;set;}
	public float screenShakeDelay{get;set;}
	public float screenShakeLast{get;set;}
	public string hurtEffect{get;set;}
	//==受伤音效==//
	public string hurtMusic{get;set;}
	public float hurtPWSETime{get;set;}
	
	public int[] cards;

	private static List<UnitSkillData> data=new List<UnitSkillData>();
	/**默认合体技**/
	public static int defaultUnitSkillId1 = 50002;
	public static int defaultUnitSkillId2 = 50012;
	
	public void addData()
	{
		cards=new int[8];
		cards[0]=card1;
		cards[1]=card2;
		cards[2]=card3;
		cards[3]=card4;
		cards[4]=card5;
		cards[5]=card6;
		cards[6]=card7;
		cards[7]=card8;
		data.Add(this);
	}
	public void resetData()
	{
		data.Clear();
	}

    public static List<UnitSkillData> GetSkills()
    { 
        List<UnitSkillData> us = new List<UnitSkillData>();
        foreach(UnitSkillData usld in data)
        {
            us.Add(usld);
        }
        return us;
    }
	public void parse(string[] ss)
	{
		
	}
	
	/**获取所有符合的合体技**/
	public static List<UnitSkillData> getUnitSkills(List<int> cardIds)
	{
		List<UnitSkillData> result=new List<UnitSkillData>();
		foreach(UnitSkillData us in data)
		{
			switch(us.cardtype)
			{
			case 1://人数组合//
				if(cardIds.Count>=us.cardnum)
				{
					result.Add(us);
				}
				break;
			case 2://种族组合//
				if(cardIds.Count>=us.cardnum)
				{
					int num=0;
					foreach(int id in cardIds)
					{
						CardData cd=CardData.getData(id);
						switch(us.cardid)
						{
						case 0:
						{
							num++;
						}break;
						case 5:
						{
							if(cd.race==1)
							{
								num++;
							}
						}break;
						case 6:
						{
							if(cd.race==2)
							{
								num++;
							}
						}break;
						case 7:
						{
							if(cd.race==4)
							{
								num++;
							}
						}break;
						case 8:
						{
							if(cd.race==3)
							{
								num++;
							}
						}break;
						}
					}
					if(num>=us.cardnum)
					{
						result.Add(us);
					}
				}
				break;
			case 3://属性组合//
				if(cardIds.Count>=us.cardnum)
				{
					int num=0;
					foreach(int id in cardIds)
					{
						CardData cd=CardData.getData(id);
						switch(us.cardid)
						{
						case 1:
							if(cd.element==1)
							{
								num++;
							}
							break;
						case 2:
							if(cd.element==2)
							{
								num++;
							}
							break;
						case 3:
							if(cd.element==3)
							{
								num++;
							}
							break;
						case 4:
							if(cd.element==4)
							{
								num++;
							}
							break;
						}
					}
					if(num>=us.cardnum)
					{
						result.Add(us);
					}
				}
				break;
			case 4://特殊组合//
				if(cardIds.Count>=us.cardnum)
				{
					int num=0;
					foreach(int id in us.cards)
					{
						if(cardIds.Contains(id))
						{
							num++;
						}
					}
					if(num>=us.cardnum)
					{
						result.Add(us);
					}
				}
				break;
			case 5://性别组合//
				//TODO
				break;
			}
		}
		return result;
	}
	
	//选取施放这个合体技的牌,死牌也能(battle)/
	public List<Card> selectCards(Player p)
	{
		if(p==null)
		{
			return null;
		}
		Card[] cs=p.getCards();
		List<Card> result=new List<Card>();
		for(int i = 0; i < cs.Length;++i)
		{
			if(cs[i]==null)
			{
				continue;
			}
			result.Add(cs[i]);
		}
		return result;
		
	}
	
	public static UnitSkillData getData(int index)
	{
		foreach(UnitSkillData usd in data)
		{
			if(usd.index==index)
			{
				return usd;
			}
		}
		return null;
	}

	public static List<UnitSkillData> getAllUnitSkillDatas()
	{
		return data;
	}

    //合体技显示界面number>100的不显示//
    public static List<UnitSkillData> getAllUnitSkillDataShow(int nowUniteSkillId)
    {
        bool bNowHaveUnite = false;
        List<UnitSkillData> temp = new List<UnitSkillData>();
        foreach (UnitSkillData s in data)
        {
            if (s.number < 100)
            {
                if (s.index == nowUniteSkillId)
                {
                    bNowHaveUnite = true;
                    temp.Insert(0, s);
                    continue;
                }
                if (GetIsCanUnlock(s.index))
                    temp.Insert(bNowHaveUnite ? 1 : 0, s);
                else
                    temp.Add(s);
            }
        }
        return temp;
    }
	
	public static List<int> getUnitSkillIds(List<int> cardIds)
	{
		List<int> result=new List<int>();
		List<UnitSkillData> units=getUnitSkills(cardIds);
		for(int i=0;i<units.Count;i++)
		{
			result.Add(units[i].index);
		}
		return result;
	}
	
	/**获取一张角色卡可以组成的合体技id**/
	public static List<int> getUnitSkillIds(int cardId)
	{
		CardData cd=CardData.getData(cardId);
		if(cd==null)
		{
			return null;
		}
		List<int> result=new List<int>();
		foreach(UnitSkillData usd in data)
		{
			switch(usd.cardtype)
			{
			case 1:/**1.队伍人数组合**/
				result.Add(usd.index);
				break;
			case 2:/**种族组合**/
				if((usd.cardid==5 && cd.race==1) || (usd.cardid==6 && cd.race==2) || (usd.cardid==7 && cd.race==4) || (usd.cardid==8 && cd.race==3))
				{
					result.Add(usd.index);
				}
				break;
			case 3:/**属性组合**/
				if(usd.cardid==cd.element)
				{
					result.Add(usd.index);
				}
				break;
			case 4:/**特殊组合**/
				for(int i=0;i<usd.cards.Length;i++)
				{
					if(usd.cards[i]==cardId)
					{
						result.Add(usd.index);
						break;
					}
				}
				break;
			case 5:/**性别组合**/
				//TODO
				break;
			}
		}
		return result;
	}
	
	/**根据合体技获取符合条件的卡组里的卡(卡组里的卡不能重复)**/
	public static List<PackElement> getUnitSkillCards(int unitSkillId,CardGroup cg)
	{
		if(cg==null)
		{
			return null;
		}
		PackElement[] dbCards=cg.cards;
		/**按攻击力降序排列**/
		List<PackElement> result=new List<PackElement>();
		UnitSkillData usd=getData(unitSkillId);
		if(usd==null)
		{
			return null;
		}
		switch(usd.cardtype)
		{
		case 1:/**队伍人数**/
			for(int i=0;i<dbCards.Length;i++)
			{
				if(dbCards[i]==null)
				{
					continue;
				}
				float thisAttack=dbCards[i].getSelfAtk();
				for(int k=0;k<result.Count;k++)
				{
					PackElement temp=result[k];
					float tempAttack=temp.getSelfAtk();
					if(thisAttack>tempAttack)
					{
						result.Insert(k,dbCards[i]);
						break;
					}
				}
				if(!result.Contains(dbCards[i]))
				{
					result.Add(dbCards[i]);
				}
			}
			break;
		case 2:/**种族**/
			for(int i=0;i<dbCards.Length;i++)
			{
				if(dbCards[i]==null)
				{
					continue;
				}
				CardData cd=CardData.getData(dbCards[i].dataId);
				if(cd==null)
				{
					continue;
				}
				if((usd.cardid==5 && cd.race==1) || (usd.cardid==6 && cd.race==2) || (usd.cardid==8 && cd.race==3) || (usd.cardid==7 && cd.race==4))
				{
					float thisAttack=dbCards[i].getSelfAtk();
					for(int k=0;k<result.Count;k++)
					{
						PackElement temp=result[k];
						float tempAttack=temp.getSelfAtk();
						if(thisAttack>tempAttack)
						{
							result.Insert(k,dbCards[i]);
							break;
						}
					}
					if(!result.Contains(dbCards[i]))
					{
						result.Add(dbCards[i]);
					}
				}
			}
			break;
		case 3:/**属性**/
			for(int i=0;i<dbCards.Length;i++)
			{
				if(dbCards[i]==null)
				{
					continue;
				}
				CardData cd=CardData.getData(dbCards[i].dataId);
				if(cd==null)
				{
					continue;
				}
				if(usd.cardid==cd.element)
				{
					float thisAttack=dbCards[i].getSelfAtk();
					for(int k=0;k<result.Count;k++)
					{
						PackElement temp=result[k];
						float tempAttack=temp.getSelfAtk();
						if(thisAttack>tempAttack)
						{
							result.Insert(k,dbCards[i]);
							break;
						}
					}
					if(!result.Contains(dbCards[i]))
					{
						result.Add(dbCards[i]);
					}
				}
			}
			break;
		case 4:/**特殊**/
			for(int i=0;i<dbCards.Length;i++)
			{
				if(dbCards[i]==null)
				{
					continue;
				}
				CardData cd=CardData.getData(dbCards[i].dataId);
				if(cd==null)
				{
					continue;
				}
				foreach(int cardId in usd.cards)
				{
					if(cardId==cd.id)
					{
						float thisAttack=dbCards[i].getSelfAtk();
						for(int k=0;k<result.Count;k++)
						{
							PackElement temp=result[k];
							float tempAttack=temp.getSelfAtk();
							if(thisAttack>tempAttack)
							{
								result.Insert(k,dbCards[i]);
								break;
							}
						}
						if(!result.Contains(dbCards[i]))
						{
							result.Add(dbCards[i]);
						}
					}
				}
			}
			break;
		case 5:/**性别**/
			//TODO
			break;
		}
		/**如果人数小于所需人数,返回null**/
		if(result.Count<usd.cardnum)
		{
			return null;
		}
		/**如果人数大于所需人数,选择攻击高的卡**/
		else if(result.Count>usd.cardnum)
		{
			result.RemoveRange(usd.cardnum,result.Count-usd.cardnum);
		}
		/**按照卡组里的卡牌顺序**/
		List<PackElement> final=new List<PackElement>();
		for(int i=0;i<dbCards.Length;i++)
		{
			if(result.Contains(dbCards[i]))
			{
				final.Add(dbCards[i]);
			}
		}
		return final;
	}
	
	/**是否可以激活合体技(背包里的卡可能会重复)**/
	public static bool canActiveUnitSkill(UnitSkillData usd,List<PackElement> cards)
	{
		if(usd==null)
		{
			return false;
		}
		int num=0;
		List<int> temp=new List<int>();
		switch(usd.cardtype)
		{
		case 1:/**队伍人数**/
			foreach(PackElement dbc in cards)
			{
				CardData cd=CardData.getData(dbc.dataId);
				if(cd==null || temp.Contains(cd.id))
				{
					continue;
				}
				temp.Add(cd.id);
				num++;
				if(num>=usd.cardnum)
				{
					break;
				}
			}
			break;
		case 2:/**种族**/
			foreach(PackElement dbc in cards)
			{
				CardData cd=CardData.getData(dbc.dataId);
				if(cd==null || temp.Contains(cd.id))
				{
					continue;
				}
				if((usd.cardid==5 && cd.race==1) || (usd.cardid==6 && cd.race==2) || (usd.cardid==8 && cd.race==3) || (usd.cardid==7 && cd.race==4))
				{
					temp.Add(cd.id);
					num++;
				}
				if(num>=usd.cardnum)
				{
					break;
				}
			}
			break;
		case 3:/**属性**/
			foreach(PackElement dbc in cards)
			{
				CardData cd=CardData.getData(dbc.dataId);
				if(cd==null || temp.Contains(cd.id))
				{
					continue;
				}
				if(usd.cardid==cd.element)
				{
					temp.Add(cd.id);
					num++;
				}
				if(num>=usd.cardnum)
				{
					break;
				}
			}
			break;
		case 4:/**特殊**/
			foreach(PackElement dbc in cards)
			{
				CardData cd=CardData.getData(dbc.dataId);
				if(cd==null || temp.Contains(cd.id))
				{
					continue;
				}
				foreach(int cardId in usd.cards)
				{
					if(cardId==cd.id)
					{
						temp.Add(cd.id);
						num++;
					}
				}
				if(num>=usd.cardnum)
				{
					break;
				}
			}
			break;
		case 5:/**性别**/
			
			break;
		}
		
		if(num<usd.cardnum)
		{
			return false;
		}
		return true;
	}
	
	/**获取排序后的合体技列表**/
	/**返回结果格式:合体技ID-标记,其中标记表示:1使用中,2可使用,3可激活,4不可激活**/
	public static List<string> getSortedUnitSkill(CardGroup cg,List<PackElement> cards)
	{
		if(cg==null)
		{
			return null;
		}
		List<int> temp=new List<int>();
		List<int> mark=new List<int>();
		/**使用中**/
		if(cg.unitSkillId>0)
		{
			temp.Add(cg.unitSkillId);
			mark.Add(1);
		}
		/**可使用**/
		List<int> cardIds=new List<int>();
		foreach(PackElement dbc in cg.cards)
		{
			if(dbc!=null)
			{
				cardIds.Add(dbc.dataId);
			}
		}
		List<UnitSkillData> usefulUnits=getUnitSkills(cardIds);
		foreach(UnitSkillData usd in usefulUnits)
		{
			if(!temp.Contains(usd.index))
			{
				temp.Add(usd.index);
				mark.Add(2);
			}
		}
		/**可激活**/
		foreach(UnitSkillData usd in data)
		{
			if(temp.Contains(usd.index))
			{
				continue;
			}
			/**背包里的卡是否可以激活这个合体技**/
			if(canActiveUnitSkill(usd,cards))
			{
				temp.Add(usd.index);
				mark.Add(3);
			}
		}
		/**不可激活**///**升序排列**//
		List<UnitSkillData> sortList=new List<UnitSkillData>();
		foreach(UnitSkillData usd in data)
		{
			if(!temp.Contains(usd.index))
			{
				sortList.Add(usd);
			}
		}
		List<UnitSkillData> sortList1 = sortList.OrderBy(s=>s.power).ToList<UnitSkillData>();
		for(int i =0;i<sortList1.Count;i++)
		{
			temp.Add(sortList1[i].index);
			mark.Add(4);
		}
		
		List<string> result=new List<string>();
		for(int i=0;i<temp.Count;i++)
		{
			result.Add(temp[i]+"-"+mark[i]+"-null");
		}
		
		return result;
	}
	
	//获得当前合体技列表解锁情况//
	public static List<string> getUnitSkillList(CardGroup cg)
	{
		if(cg==null)
		{
			return null;
		}
		List<int> temp=new List<int>();
		List<int> mark=new List<int>();
		/**使用中**/
		if(cg.unitSkillId>0)
		{
			temp.Add(cg.unitSkillId);
			mark.Add(1);
		}
		/**可使用**/
		List<int> cardIds=new List<int>();
		Dictionary<int,bool> cardinfo = UniteSkillInfo.cardUnlockTable;
		foreach(KeyValuePair<int,bool> ci in cardinfo)
		{
			if(ci.Value)
			{
				cardIds.Add(ci.Key);
			}
		}
		List<UnitSkillData> usefulUnits=getUnitSkills(cardIds);
		foreach(UnitSkillData usd in usefulUnits)
		{
			if(!temp.Contains(usd.index))
			{
				temp.Add(usd.index);
				mark.Add(2);
			}
		}
		/**不可激活**///**升序排列**//
		List<UnitSkillData> sortList=new List<UnitSkillData>();
		foreach(UnitSkillData usd in data)
		{
			if(!temp.Contains(usd.index))
			{
				sortList.Add(usd);
			}
		}
		List<UnitSkillData> sortList1 = sortList.OrderBy(s=>s.power).ToList<UnitSkillData>();
		for(int i =0;i<sortList1.Count;i++)
		{
			temp.Add(sortList1[i].index);
			mark.Add(4);
		}
		
		List<string> result=new List<string>();
		for(int i=0;i<temp.Count;i++)
		{
			result.Add(temp[i]+"-"+mark[i]+"-null");
		}
		
		return result;
	}
	
	public static string getUnitSkillRequireInCardGroupUsedNum(int unitSkillID,CardGroup cg)
	{
		string r = "";
		UnitSkillData usd  = getData(unitSkillID);
		if(usd == null)
			return "0-0";
		int requireNum = usd.cardnum;
		int usedCardNum = 0;
		ArrayList cardIDList = new ArrayList();
		for(int i = 0; i < cg.cards.Length;++i)
		{
			if(cg.cards[i] != null)
			{
				cardIDList.Add(cg.cards[i].dataId);
			}
		}
		// 1 队伍人数，2 种族，3属性，4 特殊，5性别
		switch(usd.cardtype)
		{
		case 1:
			usedCardNum = cardIDList.Count;
			break;
		case 2:
			for(int i = 0; i < cardIDList.Count;++i)
			{
				CardData cd = CardData.getData((int)cardIDList[i]);
				if((usd.cardid==5 && cd.race==1) || (usd.cardid==6 && cd.race==2) || (usd.cardid==8 && cd.race==3) || (usd.cardid==7 && cd.race==4) || usd.cardid == 0)
				{
					usedCardNum++;
				}
			}
			break;
		case 3:
			for(int i = 0; i < cardIDList.Count;++i)
			{
				CardData cd=CardData.getData((int)cardIDList[i]);
				if(usd.cardid == cd.element)
				{
					usedCardNum++;
				}
			}
			break;
		case 4:
			// do nothing
			break;
		case 5:
			// do nothing
			break;
		}
		if(requireNum <= usedCardNum)
		{
			usedCardNum = requireNum;
		}
		r = usedCardNum.ToString()+"-"+requireNum;
		return r;
	}
	// 获取合体技需求的为特殊卡牌组合的信息 
	//返回string的列表
	//每个元素为 卡牌ID－标记  标记(0 已装备,1 未装备)//
	public static List<string> getUnitSkillRequireInCardGroupSpecialCardList(int unitSkillID,CardGroup cg)
	{
		List<string> strList = new List<string>();
		UnitSkillData usd = getData(unitSkillID);
		List<int> inCardGroupIDList = new List<int>();
		for(int i = 0;i < cg.cards.Length;++i)
		{
			if(cg.cards[i] != null)
			{
				inCardGroupIDList.Add(cg.cards[i].dataId);
			}
			
		}
		for(int i = 0; i < usd.cards.Length;++i)
		{
			if(usd.cards[i] > 0 )
			{
				string ss="";
				if(inCardGroupIDList.Contains(usd.cards[i]))
				{
					//已装备//
					ss = usd.cards[i].ToString()+"-"+"0";
				}
				else 
				{
					//未装备//
					ss = usd.cards[i].ToString()+"-" + "1";
				}
				strList.Add(ss);
			}
		}
		return strList;
	}
	
	//返回一个合体技需要的卡牌id的list//
	public static List<int> getUniteSkillAllNeedCardId(int uniteSkillId)
	{
		List<int> needCardIdList = new List<int>();
		UnitSkillData usd = getData(uniteSkillId);
		needCardIdList.Add(usd.card1);
		needCardIdList.Add(usd.card2);
		needCardIdList.Add(usd.card3);
		needCardIdList.Add(usd.card4);
		needCardIdList.Add(usd.card5);
		needCardIdList.Add(usd.card6);
		needCardIdList.Add(usd.card7);
		needCardIdList.Add(usd.card8);
		
		return needCardIdList;
	}

    public static bool GetIsCanUnlock(int uniteSkillId)
    {
        List<int> cardIdList = UnitSkillData.getUniteSkillAllNeedCardId(uniteSkillId);
        for (int i = 0; i < cardIdList.Count; i++)
        {
            if (cardIdList[i] != 0)
            {
                bool mark;
                try
                {
                    mark = UniteSkillInfo.cardUnlockTable[cardIdList[i]];
                }
                catch (KeyNotFoundException)
                {
                    return false;
                }
                if (mark == false)
                {
                    return false;
                }
            }
        }
        return true;
    }
}


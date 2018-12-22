using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Statics
{
	/**atk>=2def**/
	private const float AtkMul=2f;
	/**atk<2def**/
	private const float AtkMul2=4f;
	/**atk**/
	private const float ResumeMul=2f;
	/**回复类型系数**/
	private const float ResumeMul2=1f;

	/// <summary>
	/// 根据技能类型获取目标
	/// </summary>
	/// <returns>
	/// The target indexs.
	/// </returns>
	/// <param name='curCardIndex'>
	/// Current card index.
	/// </param>
	/// <param name='actionCards'>
	/// Action cards.
	/// </param>
	/// <param name='targetCards'>
	/// Target cards.
	/// </param>
	public static List<int> getTargetIndexs(int curCardIndex,Card[] actionCards,Card[] targetCards)
	{
		SkillData skill=actionCards[curCardIndex].getSkill();
		switch(skill.type)
		{
		case 1:
			int scopeType=skill.atkTarget;
			//==天赋:4对特定技能==//
			TalentData td=TalentData.getData(actionCards[curCardIndex].talent1);
			if(td!=null && td.type==4 && td.class1/10==skill.index/10 && (td.effect==6 || td.effect==7 || td.effect==8))
			{
				scopeType=td.effect-5;
			}
			//==第二天赋:4对特定技能==//
			TalentData td2=TalentData.getData(actionCards[curCardIndex].talent2);
			if(td2!=null && td2.type==4 && td2.class1/10==skill.index/10 && (td2.effect==6 || td2.effect==7 || td2.effect==8))
			{
				scopeType=td2.effect-5;
			}
			//==第三天赋:4对特定技能==//
			TalentData td3=TalentData.getData(actionCards[curCardIndex].talent3);
			if(td3!=null && td3.type==4 && td3.class1/10==skill.index/10 && (td3.effect==6 || td3.effect==7 || td3.effect==8))
			{
				scopeType=td3.effect-5;
			}
			return getTargetIndexsBySkillAttackScope(scopeType,curCardIndex,targetCards);
		case 2:
			return getBeTankIndexs(curCardIndex,actionCards);
		case 3:
			return getTargetIndexsBySkillRestoreScope(skill.atkTarget,curCardIndex,actionCards);
		}
		return null;
	}
	
	private static List<int> getTargetIndexsBySkillAttackScope(int scopeType,int curCardIndex,Card[] targetCards)
	{
		List<int> targets=new List<int>();
		switch(scopeType)
		{
		case 0://单体//
			//==前排找目标==//
			int preIndex=curCardIndex;
			if(curCardIndex>=targetCards.Length/2)
			{
				preIndex=curCardIndex-targetCards.Length/2;
			}
			if(targetCards[preIndex]!=null && targetCards[preIndex].getCurHp()>0)
			{
				targets.Add(preIndex);
			}
			else
			{
				int min=targetCards.Length/2;
				int targetIndex=-1;
				for(int i=0;i<targetCards.Length/2;i++)
				{
					if(i!=preIndex && Math.Abs(i-preIndex)<=min && targetCards[i]!=null && targetCards[i].getCurHp()>0)
					{
						if(min==Math.Abs(i-preIndex))
						{
							if(i<targetIndex)
							{
								targetIndex=i;
							}
						}
						else
						{
							min=Math.Abs(i-preIndex);
							targetIndex=i;
						}
					}
				}
				if(targetIndex!=-1)
				{
					targets.Add(targetIndex);
				}
			}
			//==后排找目标==//
			if(targets.Count==0)
			{
				int backIndex=preIndex+targetCards.Length/2;
				if(targetCards[backIndex]!=null && targetCards[backIndex].getCurHp()>0)
				{
					targets.Add(backIndex);
				}
				else
				{
					int min=targetCards.Length/2;
					int targetIndex=-1;
					for(int i=targetCards.Length/2;i<targetCards.Length;i++)
					{
						if(i!=backIndex && Math.Abs(backIndex-i)<=min && targetCards[i]!=null && targetCards[i].getCurHp()>0)
						{
							if(min==Math.Abs(backIndex-i))
							{
								if(i<targetIndex)
								{
									targetIndex=i;
								}
							}
							else
							{
								min=Math.Abs(backIndex-i);
								targetIndex=i;
							}
						}
					}
					if(targetIndex!=-1)
					{
						targets.Add(targetIndex);
					}
				}
			}
			break;
		case 1://竖排//
			//==寻找前排目标==//
			int firstIndex=-1;
			preIndex=curCardIndex;
			if(curCardIndex>=targetCards.Length/2)
			{
				preIndex=curCardIndex-targetCards.Length/2;
			}
			if(targetCards[preIndex]!=null && targetCards[preIndex].getCurHp()>0)
			{
				firstIndex=preIndex;
			}
			else
			{
				int min=targetCards.Length/2;
				for(int i=0;i<targetCards.Length/2;i++)
				{
					if(i!=preIndex && Math.Abs(preIndex-i)<=min && targetCards[i]!=null && targetCards[i].getCurHp()>0)
					{
						if(min==Math.Abs(preIndex-i))
						{
							if(i<firstIndex)
							{
								firstIndex=i;
							}
						}
						else
						{
							min=Math.Abs(preIndex-i);
							firstIndex=i;
						}
					}
				}
			}
			//==寻找后排目标==//
			if(firstIndex>-1)
			{
				targets.Add(firstIndex);
				int temp=firstIndex+targetCards.Length/2;
				if(targetCards[temp]!=null && targetCards[temp].getCurHp()>0)
				{
					targets.Add(temp);
				}
			}
			else
			{
				int backIndex=preIndex+targetCards.Length/2;
				if(targetCards[backIndex]!=null && targetCards[backIndex].getCurHp()>0)
				{
					targets.Add(backIndex);
				}
				else
				{
					int min=targetCards.Length/2;
					int targetIndex=-1;
					for(int i=targetCards.Length/2;i<targetCards.Length;i++)
					{
						if(i!=backIndex && Math.Abs(backIndex-i)<=min && targetCards[i]!=null && targetCards[i].getCurHp()>0)
						{
							if(min==Math.Abs(backIndex-i))
							{
								if(i<targetIndex)
								{
									targetIndex=i;
								}
							}
							else
							{
								min=Math.Abs(backIndex-i);
								targetIndex=i;
							}
						}
					}
					if(targetIndex!=-1)
					{
						targets.Add(targetIndex);
					}
				}
			}
			break;
		case 2://横排//
			for(int i=0;i<targetCards.Length/2;i++)
			{
				if(targetCards[i]!=null && targetCards[i].getCurHp()>0)
				{
					targets.Add(i);
				}
			}
			if(targets.Count==0)
			{
				for(int i=targetCards.Length/2;i<targetCards.Length;i++)
				{
					if(targetCards[i]!=null && targetCards[i].getCurHp()>0)
					{
						targets.Add(i);
					}
				}
			}
			break;
		case 3://全体//
			for(int i=0;i<targetCards.Length;i++)
			{
				if(targetCards[i]!=null && targetCards[i].getCurHp()>0)
				{
					targets.Add(i);
				}
			}
			break;
		}
		return targets;
	}

	private static List<int> getTargetIndexsBySkillRestoreScope(int scopeType,int curCardIndex,Card[] targetCards)
	{
		List<int> targets=new List<int>();
		switch(scopeType)
		{
		case 0://单体//
			float minHpPercent=1f;
			int targetIndex=-1;
			for(int i=0;i<targetCards.Length;i++)
			{
				if(targetCards[i]!=null && targetCards[i].getCurHp()>0 && targetCards[i].getCurHpPercent()<minHpPercent)
				{
					minHpPercent=targetCards[i].getCurHpPercent();
					targetIndex=i;
				}
			}
			if(targetIndex!=-1)
			{
				targets.Add(targetIndex);
			}
			else
			{
				for(int i=0;i<targetCards.Length;i++)
				{
					if(targetCards[i]!=null && targetCards[i].getCurHp()>0)
					{
						targets.Add(i);
						break;
					}
				}
				//targets.Add(UnityEngine.Random.Range(0,targetCards.Length));
			}
			break;
		case 1://竖排//
			targets.Add(curCardIndex);
			int otherIndex=curCardIndex+targetCards.Length/2;
			if(curCardIndex>=targetCards.Length/2)
			{
				otherIndex=curCardIndex-targetCards.Length/2;
			}
			if(targetCards[otherIndex]!=null && targetCards[otherIndex].getCurHp()>0)
			{
				targets.Add(otherIndex);
			}
			break;
		case 2://横排//
			int row=0;
			if(curCardIndex>=targetCards.Length/2)
			{
				row=1;
			}
			for(int i=targetCards.Length/2*row;i<targetCards.Length/2*(row+1);i++)
			{
				if(targetCards[i]!=null && targetCards[i].getCurHp()>0)
				{
					targets.Add(i);
				}
			}
			break;
		case 3://全体//
			for(int i=0;i<targetCards.Length;i++)
			{
				if(targetCards[i]!=null && targetCards[i].getCurHp()>0)
				{
					targets.Add(i);
				}
			}
			break;
		}
		return targets;
	}
		
	public static List<int> getTankIndex(Card[] cards,int attackSkillScope,List<int> targetIndexs)
	{
		List<int> result=new List<int>();
		//攻击范围//
		switch(attackSkillScope)
		{
		case 0://单体攻击//
			//单体攻击目标装备防御技能时,其他防御不发动//
			foreach(int targetIndex in targetIndexs)
			{
				Card c=(Card)cards[targetIndex];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2)
				{
					result.Add(targetIndex);
					return result;
				}
			}
			//单体>竖排>横排>全体//
			//单体//
			int row=0;
			if((int)targetIndexs[0]>=cards.Length/2)
			{
				row=1;
			}
			for(int i=cards.Length/2*row;i<cards.Length/2*(row+1);i++)
			{
				Card c=(Card)cards[i];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==0)
				{
					result.Add(i);
					return result;
				}
			}
			//竖排//
			int otherIndex=(int)targetIndexs[0]+cards.Length/2;
			if(row==1)
			{
				otherIndex=(int)targetIndexs[0]-cards.Length/2;
			}
			Card other=(Card)cards[otherIndex];
			if(other!=null && !other.isDeath() && !other.isForbitAttack() && other.getSkill().type==2 && other.getSkill().defTarget==1)
			{
				result.Add(otherIndex);
				return result;
			}
			/**受伤目标所在的排如果有竖排防御,此竖排防御可为受伤目标抵挡**/
			for(int i=cards.Length/2*row;i<cards.Length/2*(row+1);i++)
			{
				if(i!=(int)targetIndexs[0])
				{
					Card c=(Card)cards[i];
					if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==1)
					{
						result.Add(i);
						return result;
					}
				}
			}
			//横排//
			for(int i=cards.Length/2*row;i<cards.Length/2*(row+1);i++)
			{
				Card c=(Card)cards[i];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==2)
				{
					result.Add(i);
					return result;
				}
			}
			//全体//
			for(int i=0;i<cards.Length;i++)
			{
				Card c=(Card)cards[i];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==3)
				{	
					result.Add(i);
					return result;
				}
			}
			break;
		case 1://竖排攻击//
			//竖排攻击目标装备防御技能时,其他防御不发动//
			foreach(int targetIndex in targetIndexs)
			{
				Card c=(Card)cards[targetIndex];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2)
				{
					result.Add(targetIndex);
					return result;
				}
			}
			//竖排>单体>横排>全体//
			//竖排//
			foreach(int i in targetIndexs)
			{
				Card c=(Card)cards[i];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==1)
				{
					result.Add(i);
					return result;
				}
			}
			/**竖排攻击时,第一个被攻击的目标所在横排的竖排防御T可为这个目标抵挡伤害**/
			int tempIndex=(int)targetIndexs[0];
			int tempRow=0;
			if(tempIndex>=cards.Length/2)
			{
				tempRow=1;
			}
			for(int i=cards.Length/2*tempRow;i<cards.Length/2*(tempRow+1);i++)
			{
				Card c=(Card)cards[i];
				if(i!=tempIndex && c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().defTarget==1)
				{
					result.Add(i);
					return result;
				}
			}
			//单体//
			foreach(int targetIndex in targetIndexs)
			{
				row=0;
				if(targetIndex>=cards.Length/2)
				{
					row=1;
				}
				for(int i=cards.Length/2*row;i<cards.Length/2*(row+1);i++)
				{
					Card c=(Card)cards[i];
					if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==0)
					{
						result.Add(i);
						return result;
					}
				}
			}
			//横排//
			bool mark=false;//是否有前排被攻击的目标//
			foreach(int targetIndex in targetIndexs)
			{
				if(targetIndex<cards.Length/2)
				{
					mark=true;
					for(int i=0;i<cards.Length/2;i++)
					{
						Card c=(Card)cards[i];
						if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==2)
						{
							result.Add(i);
						}
					}
					return result;
				}
			}	
			if(mark)
			{
				for(int i=cards.Length/2;i<cards.Length;i++)
				{
					Card c=(Card)cards[i];
					if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==2)
					{
						result.Add(i);
						return result;
					}
				}
			}
			//全体//
			for(int i=0;i<cards.Length;i++)
			{
				Card c=(Card)cards[i];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==3)
				{
					result.Add(i);
					return result;
				}
			}
			break;
		case 2://横排攻击//
			//横排>全体>竖排//
			//横排//
			row=0;
			if((int)targetIndexs[0]>=cards.Length/2)
			{
				row=1;
			}
			for(int i=cards.Length/2*row;i<cards.Length/2*(row+1);i++)
			{
				Card c=(Card)cards[i];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==2)
				{
					result.Add(i);
					return result;
				}
			}
			//全体//
			for(int i=0;i<cards.Length;i++)
			{
				Card c=(Card)cards[i];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==3)
				{
					result.Add(i);
					return result;
				}
			}
			//竖排//
			if(row==0)
			{
				for(int i=cards.Length/2;i<cards.Length;i++)
				{
					Card c=(Card)cards[i];
					if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==1)
					{
						result.Add(i);
					}
				}
			}
			else
			{
				for(int i=0;i<cards.Length/2;i++)
				{
					Card c=(Card)cards[i];
					if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==1)
					{
						result.Add(i);
					}
				}
			}
			break;
		case 3://全体攻击//
			//全体T>横排T>竖排T//
			//全体//
			for(int i=0;i<cards.Length;i++)
			{
				Card c=(Card)cards[i];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==3)
				{
					result.Add(i);
					return result;
				}
			}
			//横排//
			for(int i=0;i<cards.Length/2;i++)
			{
				Card c=(Card)cards[i];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==2)
				{
					result.Add(i);
					break;
				}
			}
			for(int i=cards.Length/2;i<cards.Length;i++)
			{
				Card c=(Card)cards[i];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==2)
				{
					result.Add(i);
					return result;
				}
			}
			//竖排//
			for(int i=0;i<cards.Length/2;i++)
			{
				Card c=(Card)cards[i];
				Card c2=(Card)cards[i+cards.Length/2];
				if(c!=null && !c.isDeath() && !c.isForbitAttack() && c.getSkill().type==2 && c.getSkill().defTarget==1)
				{
					result.Add(i);
				}
				else if(c2!=null && !c2.isDeath() && !c2.isForbitAttack() && c2.getSkill().type==2 && c2.getSkill().defTarget==1)
				{
					result.Add(i+cards.Length/2);
				}
			}
			break;
		}
		return result;
	}
	
	public static List<int> getBeTankIndexs(int tankIndex,Card[] cards)
	{
		List<int> list=new List<int>();
		list.Add(tankIndex);
		Card tank=cards[tankIndex];
		switch(tank.getSkill().defTarget)
		{
		case 0:
			break;
		case 1://竖排全体//
			int other=tankIndex+cards.Length/2;
			if(tankIndex>=cards.Length/2)
			{
				other=tankIndex-cards.Length/2;
			}
			if(cards[other]!=null && cards[other].getCurHp()>0)
			{
				list.Add(other);
			}
			break;
		case 2://横排全体//
			int start=0;
			int end=cards.Length/2;
			if(tankIndex>=cards.Length/2)
			{
				start=cards.Length/2;
				end=cards.Length;
			}
			for(int i=start;i<end;i++)
			{
				if(i!=tankIndex && cards[i]!=null && cards[i].getCurHp()>0)
				{
					list.Add(i);
				}
			}
			break;
		case 3://全体//
			for(int i=0;i<cards.Length;i++)
			{
				if(i!=tankIndex && cards[i]!=null && cards[i].getCurHp()>0)
				{
					list.Add(i);
				}
			}
			break;
		}
		return list;
	}
	
	public static float getCardSelfMaxHpForUI(int cardId,int level,int breakNum)
	{
		CardData cd=CardData.getData(cardId);
		if(cd==null)
		{
			return 0;
		}
		CardPropertyData cpd=CardPropertyData.getData(level);
		if(cpd==null)
		{
			return 0;
		}
		EvolutionData ed=EvolutionData.getData(cd.star,breakNum);
		float f_hp=0;
		if(ed!=null)
		{
			f_hp=ed.status;
			f_hp/=100;
		}
		return cd.hp/5 * cpd.hp*(1+f_hp);
	}
	
	/**获取卡本身的最大HP**/
	private static float getCardSelfMaxHp(int cardId,int level)
	{
		CardData cd=CardData.getData(cardId);
		if(cd==null)
		{
			return 0;
		}
		CardPropertyData cpd=CardPropertyData.getData(level);
		if(cpd==null)
		{
			return 0;
		}
		return cd.hp/5 * cpd.hp;
	}
	
	//**获取装备数值**//
	public static int getEquipValue(EquipData ed,int equipLevel)
	{
		EquippropertyData equippropertyData=EquippropertyData.getData(ed.type,equipLevel);
		return equippropertyData.starNumbers[ed.star-1];
	}
	
	/**
	 * 获取一张卡的最大HP
	 * lt@2014-2-21 上午10:08:13
	 * @param cardId
	 * @param level
	 * @param breakNum
	 * @param skillId
	 * @param pSkillId
	 * @param equipInfos
	 * @param selfIndex
	 * @param teamerCardIds
	 * @param runeId 符文id
	 * @return
	 */
	private static int getMaxHp(int cardId,int level,int breakNum,int skillId,int skillLevel,List<string> psList,List<string> equipInfos,int talent1,int talent2,int talent3,string runeId,int selfIndex,int[] teamerCardIds,int[] talents1,int[] talents2,int[] talents3,string[] raceAtts)
	{
		//maxHp=(card_hp*(1+b_hp+c_hp+f_hp)+a_hp+d_hp+e_hp+equip_hp+g_hp)//
		//card_hp为卡牌本身的hp；//
 		//equip_hp为装备附加的hp；//
		//a_hp为被动技能的加成系数；//
		//b_hp为卡牌自身天赋的加成系数；//
		//c_hp为队友卡牌天赋的加成系数（所有队友系数相乘后的结果）；//
		//d_hp为卡牌当前装备的主动技能的加成系数；//
		//e_hp为符文系统提供的hp加成系数；//
		//f_hp为突破系统提供的hp加成系数；//
		//g_hp为天赋系统提供的hp加成数值；//
		float card_hp=getCardSelfMaxHp(cardId,level);
		float equip_hp=0;
		if(equipInfos!=null)
		{
			foreach(string equip in equipInfos)
			{
				string[] ss=equip.Split('-');
				EquipData equipData=EquipData.getData(StringUtil.getInt(ss[0]));
				if(equipData == null)
				{
					continue;
				}
				if(equipData.type==3)
				{
					EquippropertyData equippropertyData=EquippropertyData.getData(equipData.type,StringUtil.getInt(ss[1]));
					equip_hp+=equippropertyData.starNumbers[equipData.star-1];
				}
			}
		}
		float a_hp=0;
		for(int i = 0; i < psList.Count;++i)
		{
			PassiveSkillData psd=PassiveSkillData.getData(StringUtil.getInt(psList[i]));
			if(psd!=null && psd.type==3)
			{
				a_hp +=psd.numbers;
			}
		}

		float b_hp=getTalentB_hp(talent1,skillId)+getTalentB_hp(talent2,skillId)+getTalentB_hp(talent3,skillId);
		float c_hp=getTalentC_hp(cardId,selfIndex,teamerCardIds,talents1)+getTalentC_hp(cardId,selfIndex,teamerCardIds,talents2)+getTalentC_hp(cardId,selfIndex,teamerCardIds,talents3);
		float d_hp=0;
		
		//符文属性//
		float e_hp=getRuneValue(runeId, 3);
		CardData cd=CardData.getData(cardId);
		EvolutionData ed=EvolutionData.getData(cd.star,breakNum);
		float f_hp=0;
		if(ed!=null)
		{
			f_hp=ed.status;
			f_hp/=100;
		}
		//种族加成属性//
		float raceAttMul=getRaceAtt(6,raceAtts)/100;
		float raceAttValue=getRaceAtt(3,raceAtts);
		//==天赋加成固定数值==//
		float g_hp=getTalentG_hp(talent1)+getTalentG_hp(talent2)+getTalentG_hp(talent3);
		
		return (int)(card_hp*(1+b_hp+c_hp+f_hp+raceAttMul)+a_hp+d_hp+e_hp+equip_hp+raceAttValue+g_hp);
	}
	
	private static float getTalentB_hp(int talent,int skillId)
	{
		float b_hp=0;
		SkillData sd=SkillData.getData(skillId);
		//==天赋:1对技能类型加成==//
		TalentData td=TalentData.getData(talent);
		if(sd!=null && td!=null && td.type==1 && td.class1==sd.type && td.effect==3)
		{
			b_hp=td.number;
		}
		//==天赋:4对特定技能==//
		if(sd!=null && td!=null && td.type==4 && td.class1/10==sd.index/10 && td.effect==3)
		{
			b_hp=td.number;
		}
		return b_hp;
	}

	private static float getTalentC_hp(int cardId,int selfIndex,int[] teamerCardIds,int[] talents)
	{
		float c_hp=0;
		CardData cd=CardData.getData(cardId);
		if(selfIndex!=-1)
		{
			//==队友天赋加成:5对本方单位==//
			for(int k=0;k<teamerCardIds.Length;k++)
			{
				int cId=teamerCardIds[k];
				if(cId!=0)
				{
					CardData cdTemp=CardData.getData(cId);
					int talent=talents[k];
					if(talent==0)
					{
						continue;
					}
					TalentData td2=TalentData.getData(talent);
					if(td2!=null && (td2.type==5 || td2.type==51) && (td2.effect==3 || td2.effect==7))
					{
						switch(td2.class1)
						{
						case 1://本种族卡牌//
							if(cd.race==cdTemp.race)
							{
								if(td2.number>c_hp)
								{
									c_hp=td2.number;
								}
							}
							break;
						case 2://固定前排//
							if(selfIndex<3)
							{
								if(td2.number>c_hp)
								{
									c_hp=td2.number;
								}
							}
							break;
						case 3://固定后排//
							if(selfIndex>=3)
							{
								if(td2.number>c_hp)
								{
									c_hp=td2.number;
								}
							}
							break;
						case 4://前排对位//
							if(selfIndex+3==k)
							{
								if(td2.number>c_hp)
								{
									c_hp=td2.number;
								}
							}
							break;
						case 5://所有卡牌//
							if(td2.number>c_hp)
							{
								c_hp=td2.number;
							}
							break;
						default://固定卡牌//
							if(td2.class1==cd.id)
							{
								if(td2.number>c_hp)
								{
									c_hp=td2.number;
								}
							}
							break;
						}
					}
				}
			}
		}
		return c_hp;
	}
	
	private static float getTalentG_hp(int talent)
	{
		float g_hp=0;
		//==天赋:8对hp加成固定数值==//
		TalentData td=TalentData.getData(talent);
		if(td!=null && td.type==8 && td.class1==0 && td.effect==3)
		{
			g_hp=td.number;
		}
		return g_hp;
	}
	
	/**
	 * 获取一张卡的最大HP(战斗使用)
	 * lt@2014-2-21 上午10:25:57
	 * @param self
	 * @param teamers
	 * @return
	 */
	public static int getMaxHp(Card self,Card[] teamers)
	{
		int talent1=self.talent1;
		int talent2=self.talent2;
		int talent3=self.talent3;
		int selfIndex=getIndexInArray(self,teamers);
		int[] teamerCardIds=new int[6];
		int[] talents1=new int[6];
		int[] talents2=new int[6];
		int[] talents3=new int[6];
		for(int i=0;i<teamers.Length;i++)
		{
			if(teamers[i]!=null)
			{
				teamerCardIds[i]=teamers[i].getCardData().id;
				talents1[i]=teamers[i].talent1;
				talents2[i]=teamers[i].talent2;
				talents3[i]=teamers[i].talent3;
			}
		}
		return getMaxHp(self.getCardData().id, self.getLevel(), self.breakNum, self.getSkill().index,self.skillLevel, self.getPassiveSkill(), self.getEquipInfos(), talent1, talent2, talent3, self.runeId, selfIndex, teamerCardIds, talents1, talents2, talents3, self.raceAtts);
	}
	
	public static float getCardSelfMaxAtkForUI(int cardId,int level,int breakNum)
	{
		CardData cd=CardData.getData(cardId);
		if(cd==null)
		{
			return 0;
		}
		CardPropertyData cpd=CardPropertyData.getData(level);
		if(cpd==null)
		{
			return 0;
		}
		EvolutionData ed=EvolutionData.getData(cd.star,breakNum);
		float f_hp=0;
		if(ed!=null)
		{
			f_hp=ed.status;
			f_hp/=100;
		}
		return cd.atk/5*cpd.atk*(1+f_hp);
	}
	
	/**获取卡本身的最大atk**/
	private static float getCardSelfMaxAtk(int cardId,int level)
	{
		CardData cd=CardData.getData(cardId);
		if(cd==null)
		{
			return 0;
		}
		CardPropertyData cpd=CardPropertyData.getData(level);
		if(cpd==null)
		{
			return 0;
		}
		return cd.atk/5*cpd.atk;
	}
	
	/**
	 * 获取一张牌的总攻击
	 * lt@2014-2-21 上午10:05:19
	 * @param cardId
	 * @param level
	 * @param skillId
	 * @param pSkillId
	 * @param equipInfos
	 * @param selfIndex
	 * @param teamerCardIds
	 * @param runeId 符文Id
	 * @param target 目标(战斗专用)
	 * @param roundNum 回合数(战斗专用)
	 * @param self 自己(战斗专用)
	 * @return
	 */
	private static float getAllAttack(int cardId,int level,int breakNum,int skillId,int skillLevel,List<string> psList,List<string> equipInfos,int talent1,int talent2,int talent3,string runeId,int selfIndex,int[] teamerCardIds,int[] talents1,int[] talents2,int[] talents3,Card target,int roundNum,Card self,string[] raceAtts)
	{
		//all_atk=(card_atk*(1+b_atk+c_atk+f_atk)+a_atk+d_atk+e_atk+equip_atk+g_atk)//
		//card_atk为卡牌本身的攻击力；//
		//equip_atk为装备附加的攻击力；//
		//a_atk为被动技能的加成系数；//
		//b_atk为卡牌自身天赋的加成系数；//
		//c_atk为队友卡牌天赋的加成系数（所有队友系数相乘后的结果）；//
		//d_atk为卡牌当前装备的主动技能的加成系数；//
		//e_atk为符文系统提供的攻击力加成系数；//
		//f_atk为突破加成系数；//
		//g_atk为天赋系统提供的hp加成数值；//
		float card_atk=getCardSelfMaxAtk(cardId, level);
		float equip_atk=0;
		if(equipInfos!=null)
		{
			foreach(string equip in equipInfos)
			{
				string[] ss=equip.Split('-');
				EquipData equipData=EquipData.getData(StringUtil.getInt(ss[0]));
				if(equipData == null)
				{
					continue;
				}
				if(equipData.type==1)
				{
					EquippropertyData equippropertyData=EquippropertyData.getData(equipData.type,StringUtil.getInt(ss[1]));
					equip_atk+=equippropertyData.starNumbers[equipData.star-1];
				}
			}
		}
		float a_atk=0;
		for(int i = 0;i < psList.Count;++i)
		{
			PassiveSkillData psd=PassiveSkillData.getData(StringUtil.getInt(psList[i]));
			if(psd!=null && psd.type==1)
			{
				a_atk += psd.numbers;
			}
		}

		SkillData sd=SkillData.getData(skillId);
		float b_atk=getTalentB_atk(talent1,sd,target,roundNum,self)+getTalentB_atk(talent2,sd,target,roundNum,self)+getTalentB_atk(talent3,sd,target,roundNum,self);
		float c_atk=getTalentC_atk(cardId,selfIndex,teamerCardIds,talents1)+getTalentC_atk(cardId,selfIndex,teamerCardIds,talents2)+getTalentC_atk(cardId,selfIndex,teamerCardIds,talents3);
		float d_atk=0;
		if(sd!=null && (sd.type==1 || sd.type==3))
		{
			d_atk=SkillPropertyData.getProperty(sd.type, skillLevel, sd.star);
		}
		//符文属性//
		float e_atk=getRuneValue(runeId, 1);
		CardData cd=CardData.getData(cardId);
		EvolutionData ed=EvolutionData.getData(cd.star,breakNum);
		//突破属性//
		float f_atk=0;
		if(ed!=null)
		{
			f_atk=ed.status;
			f_atk/=100;
		}
		//种族加成属性//
		float raceAttMul=getRaceAtt(4,raceAtts)/100;
		float raceAttValue=getRaceAtt(1,raceAtts);
		//==天赋加成固定数值==//
		float g_atk=getTalentG_atk(talent1)+getTalentG_atk(talent2)+getTalentG_atk(talent3);
		
		return card_atk*(1+b_atk+c_atk+f_atk+raceAttMul)+a_atk+d_atk+e_atk+equip_atk+raceAttValue+g_atk;
	}
	
	private static float getTalentB_atk(int talent,SkillData sd,Card target,int roundNum,Card self)
	{
		float b_atk=0;
		//==天赋:1对技能类型加成==//
		TalentData td=TalentData.getData(talent);
		if(sd!=null && td!=null && td.type==1 && td.class1==sd.type && td.effect==1)
		{
			b_atk=td.number;
		}
		//==天赋:2对元素==//
		if(sd!=null && td!=null && td.type==2 && sd.type==1 && td.class1-sd.element==1 && td.effect==1)
		{
			b_atk=td.number;
		}
		//==天赋:4对特定技能==//
		if(sd!=null && td!=null && td.type==4 && td.class1/10==sd.index/10 && td.effect==1)
		{
			b_atk=td.number;
		}
		//==天赋:对敌方卡牌==//
		if(td!=null && td.type==6 && td.class1==4 && target!=null && target.isBoss() && td.effect==6)
		{
			b_atk=td.number;
		}
		//==天赋加成:7特殊==//
		if(target!=null && td!=null && td.type==7 && td.class1==2 && target.getCardData().id==td.effect)
		{
			b_atk=td.number;
		}
		//==天赋加成:7特殊==//
		if(td!=null && td.type==7 && td.class1==7)
		{
			b_atk=td.number*roundNum;
		}
		//==天赋加成:7特殊==//
		if(self!=null && td!=null && td.type==7 && td.class1==11 && self.getCurHp()<=self.getMaxHp()/2)
		{
			b_atk=td.number;
		}
		return b_atk;
	}
	
	private static float getTalentC_atk(int cardId,int selfIndex,int[] teamerCardIds,int[] talents)
	{
		float c_atk=0;
		CardData cd=CardData.getData(cardId);
		if(selfIndex!=-1)
		{
			//==队友天赋加成:5对本方单位==//
			for(int k=0;k<teamerCardIds.Length;k++)
			{
				int cId=teamerCardIds[k];
				if(cId!=0)
				{
					CardData cdTemp=CardData.getData(cId);
					int talent=talents[k];
					if(talent==0)
					{
						continue;
					}
					TalentData td2=TalentData.getData(talent);
					if(td2!=null && (td2.type==5 || td2.type==51) && (td2.effect==1 || td2.effect==7))
					{
						switch(td2.class1)
						{
						case 1://本种族卡牌//
							if(cd.race==cdTemp.race)
							{
								if(td2.number>c_atk)
								{
									c_atk=td2.number;
								}
							}
							break;
						case 2://固定前排//
							if(selfIndex<3)
							{
								if(td2.number>c_atk)
								{
									c_atk=td2.number;
								}
							}
							break;
						case 3://固定后排//
							if(selfIndex>=3)
							{
								if(td2.number>c_atk)
								{
									c_atk=td2.number;
								}
							}
							break;
						case 4://前排对位//
							if(selfIndex+3==k)
							{
								if(td2.number>c_atk)
								{
									c_atk=td2.number;
								}
							}
							break;
						case 5://所有卡牌//
							if(td2.number>c_atk)
							{
								c_atk=td2.number;
							}
							break;
						default://固定卡牌//
							if(td2.class1==cd.id)
							{
								if(td2.number>c_atk)
								{
									c_atk=td2.number;
								}
							}
							break;
						}
					}
				}
			}
		}
		return c_atk;
	}
	
	private static float getTalentG_atk(int talent)
	{
		float g_atk=0;
		//==天赋:8对atk加成固定数值==//
		TalentData td=TalentData.getData(talent);
		if(td!=null && td.type==8 && td.class1==0 && td.effect==1)
		{
			g_atk=td.number;
		}
		return g_atk;
	}
	
	/**
	 * 获取一张牌的总攻击(战斗使用)
	 * lt@2014-2-21 上午10:25:16
	 * @param self
	 * @param teamers
	 * @param target
	 * @param roundNum
	 * @return
	 */
	public static float getAllAttack(Card self,Card[] teamers,Card target,int roundNum)
	{
		int talent1=self.talent1;
		int talent2=self.talent2;
		int talent3=self.talent3;
		int selfIndex=getIndexInArray(self,teamers);
		int[] teamerCardIds=new int[6];
		int[] talents1=new int[6];
		int[] talents2=new int[6];
		int[] talents3=new int[6];
		for(int i=0;i<teamers.Length;i++)
		{
			if(teamers[i]!=null)
			{
				teamerCardIds[i]=teamers[i].getCardData().id;
				talents1[i]=teamers[i].talent1;
				talents2[i]=teamers[i].talent2;
				talents3[i]=teamers[i].talent3;
			}
		}
		return getAllAttack(self.getCardData().id, self.getLevel(), self.breakNum, self.getSkill().index,self.skillLevel, self.getPassiveSkill(), self.getEquipInfos(), talent1, talent2, talent3, self.runeId, selfIndex, teamerCardIds, talents1, talents2, talents3, target, roundNum, self,self.raceAtts);
	}
	
	public static float getCardSelfMaxDefForUI(int cardId,int level,int breakNum)
	{
		CardData cd=CardData.getData(cardId);
		if(cd==null)
		{
			return 0;
		}
		CardPropertyData cpd=CardPropertyData.getData(level);
		if(cpd==null)
		{
			return 0;
		}
		EvolutionData ed=EvolutionData.getData(cd.star,breakNum);
		float f_hp=0;
		if(ed!=null)
		{
			f_hp=ed.status;
			f_hp/=100;
		}
		return cd.def/5*cpd.def*(1+f_hp);
	}
	
	/**获取卡本身的最大def**/
	private static float getCardSelfMaxDef(int cardId,int level)
	{
		CardData cd=CardData.getData(cardId);
		if(cd==null)
		{
			return 0;
		}
		CardPropertyData cpd=CardPropertyData.getData(level);
		if(cpd==null)
		{
			return 0;
		}
		return cd.def/5*cpd.def;
	}
	
	/**获取一张牌的总防御**/
	private static float getAllDef(int cardId,int level,int breakNum,int skillId,int skillLevel,List<string> psList,List<string> equipInfos,string runeId,int talent1,int talent2,int talent3,int selfIndex,int[] teamerCardIds,int[] talents1,int[] talents2,int[] talents3,Card attacker,int roundNum,string[] raceAtts)
	{
		//与攻击力的计算方法类似,被攻击一方总的防御力数值通过以下方法计算：//
		//all_def=(card_def*(1+b_def+c_def+f_def)+a_def+d_def+e_def+equip_def+g_def)//
		//card_def为卡牌本身的防御力；//
		//equip_def为装备附加的防御力；//
		//a_def为被动技能的加成系数；//
		//b_def为卡牌自身天赋的加成系数；//
		//c_def为队友卡牌天赋的加成系数（所有队友系数相乘后的结果）；//
		//d_def为卡牌当前装备的主动技能的加成系数；//
		//e_def为符文系统提供的防御力加成系数；//
		//f_def为突破加成系数；//
		//g_def为天赋系统提供的hp加成数值；//
		float card_def=getCardSelfMaxDef(cardId, level);
		float equip_def=0;
		if(equipInfos!=null)
		{
			foreach(string equip in equipInfos)
			{
				string[] ss=equip.Split('-');
				EquipData equipData=EquipData.getData(StringUtil.getInt(ss[0]));
				if(equipData == null)
				{
					continue;
				}
				if(equipData.type==2)
				{
					EquippropertyData equippropertyData=EquippropertyData.getData(equipData.type,StringUtil.getInt(ss[1]));
					equip_def+=equippropertyData.starNumbers[equipData.star-1];
				}
			}
		}
		float a_def=0;
		int attackSkillElement=0;
		SkillData atkSkill=null;
		if(attacker!=null)
		{
			atkSkill=attacker.getSkill();
			attackSkillElement=atkSkill.element;
		}
		for(int i = 0; i < psList.Count;++i)
		{
			PassiveSkillData psd=PassiveSkillData.getData(StringUtil.getInt(psList[i]));
			if(psd!=null)
			{
				switch(psd.type)
				{
				case 2:
					a_def+=psd.numbers;
					break;
				case 6:
					if(attackSkillElement==1)
					{
						a_def+=psd.numbers;
					}
					break;
				case 7:
					if(attackSkillElement==2)
					{
						a_def+=psd.numbers;
					}
					break;
				case 8:
					if(attackSkillElement==3)
					{
						a_def+=psd.numbers;
					}
					break;
				case 9:
					if(attackSkillElement==4)
					{
						a_def+=psd.numbers;
					}
					break;
				}
			}
		}
		
		SkillData sd=SkillData.getData(skillId);
		float b_def=getTalentB_def(talent1,sd,attackSkillElement,atkSkill,attacker,roundNum)+getTalentB_def(talent2,sd,attackSkillElement,atkSkill,attacker,roundNum)+getTalentB_def(talent3,sd,attackSkillElement,atkSkill,attacker,roundNum);
		float c_def=getTalentC_def(cardId,selfIndex,teamerCardIds,talents1)+getTalentC_def(cardId,selfIndex,teamerCardIds,talents2)+getTalentC_def(cardId,selfIndex,teamerCardIds,talents3);
		float d_def=0;
		if(sd!=null && sd.type==2)
		{
			d_def=SkillPropertyData.getProperty(sd.type, skillLevel, sd.star);
		}
		//符文属性//
		float e_def=getRuneValue(runeId, 2);
		//突破属性//
		CardData cd=CardData.getData(cardId);
		EvolutionData ed=EvolutionData.getData(cd.star,breakNum);
		float f_def=0;
		if(ed!=null)
		{
			f_def=ed.status;
			f_def/=100;
		}
		//种族加成属性//
		float raceAttMul=getRaceAtt(5,raceAtts)/100;
		float raceAttValue=getRaceAtt(2,raceAtts);
		//==天赋加成固定数值==//
		float g_def=getTalentG_def(talent1)+getTalentG_def(talent2)+getTalentG_def(talent3);
		
		return card_def*(1+b_def+c_def+f_def+raceAttMul)+a_def+d_def+e_def+equip_def+raceAttValue+g_def;
	}
	
	private static float getTalentB_def(int talent,SkillData sd,int attackSkillElement,SkillData atkSkill,Card attacker,int roundNum)
	{
		float b_def=0;
		//==天赋:1对技能类型加成==//
		TalentData td=TalentData.getData(talent);
		if(sd!=null && td!=null && td.type==1 && td.class1==sd.type && td.effect==2)
		{
			b_def=td.number;
		}
		//==天赋:2对元素==//
		if(td!=null && td.type==2 && td.class1-attackSkillElement==6 && td.effect==2)
		{
			b_def=td.number;
		}
		//==天赋:4对特定技能==//
		if(sd!=null && td!=null && td.type==4 && td.class1/10==sd.index/10 && td.effect==2)
		{
			b_def=td.number;
		}
		//==天赋加成:7特殊==//
		if(atkSkill!=null && td!=null && td.type==7 && td.class1==1 && atkSkill.atkTarget>0)
		{
			b_def=td.number;
		}
		//==天赋加成:7特殊==//
		if(atkSkill!=null && td!=null && td.type==7 && td.class1==12 && attacker.getCardData().id==td.effect)
		{
			b_def=td.number;
		}
		//==天赋加成:7特殊==//
		if(td!=null && td.type==7 && td.class1==8)
		{
			b_def=td.number*roundNum;
		}
		return b_def;
	}
	
	private static float getTalentC_def(int cardId,int selfIndex,int[] teamerCardIds,int[] talents)
	{
		float c_def=0;
		CardData cd=CardData.getData(cardId);
		if(selfIndex!=-1)
		{
			//==队友天赋加成:5对本方单位==//
			for(int k=0;k<teamerCardIds.Length;k++)
			{
				int cId=teamerCardIds[k];
				if(cId!=0)
				{
					CardData cdTemp=CardData.getData(cId);
					int talent=talents[k];
					if(talent==0)
					{
						continue;
					}
					TalentData td2=TalentData.getData(talent);
					if(td2!=null && (td2.type==5 || td2.type==51) && (td2.effect==2 || td2.effect==7))
					{
						switch(td2.class1)
						{
						case 1://本种族卡牌//
							if(cd.race==cdTemp.race)
							{
								if(td2.number>c_def)
								{
									c_def=td2.number;
								}
							}
							break;
						case 2://固定前排//
							if(selfIndex<3)
							{
								if(td2.number>c_def)
								{
									c_def=td2.number;
								}
							}
							break;
						case 3://固定后排//
							if(selfIndex>=3)
							{
								if(td2.number>c_def)
								{
									c_def=td2.number;
								}
							}
							break;
						case 4://前排对位//
							if(selfIndex+3==k)
							{
								if(td2.number>c_def)
								{
									c_def=td2.number;
								}
							}
							break;
						case 5://所有卡牌//
							if(td2.number>c_def)
							{
								c_def=td2.number;
							}
							break;
						default://固定卡牌//
							if(td2.class1==cd.id)
							{
								if(td2.number>c_def)
								{
									c_def=td2.number;
								}
							}
							break;
						}
					}
				}
			}
		}
		return c_def;
	}
	
	private static float getTalentG_def(int talent)
	{
		float g_def=0;
		//==天赋:8对def加成固定数值==//
		TalentData td=TalentData.getData(talent);
		if(td!=null && td.type==8 && td.class1==0 && td.effect==2)
		{
			g_def=td.number;
		}
		return g_def;
	}
	
	/**
	 * 获取一张牌的总防御(战斗使用)
	 * lt@2014-2-21 上午10:22:51
	 * @param self
	 * @param teamers
	 * @param attacker
	 * @param roundNum
	 * @return
	 */
	public static float getAllDef(Card self,Card[] teamers,Card attacker,int roundNum)
	{
		int talent1=self.talent1;
		int talent2=self.talent2;
		int talent3=self.talent3;
		int selfIndex=getIndexInArray(self,teamers);
		int[] teamerCardIds=new int[6];
		int[] talents1=new int[6];
		int[] talents2=new int[6];
		int[] talents3=new int[6];
		for(int i=0;i<teamers.Length;i++)
		{
			if(teamers[i]!=null)
			{
				teamerCardIds[i]=teamers[i].getCardData().id;
				talents1[i]=teamers[i].talent1;
				talents2[i]=teamers[i].talent2;
				talents3[i]=teamers[i].talent3;
			}
		}
		return getAllDef(self.getCardData().id, self.getLevel(), self.breakNum, self.getSkill().index,self.skillLevel, self.getPassiveSkill(), self.getEquipInfos(), self.runeId, talent1, talent2, talent3, selfIndex, teamerCardIds, talents1, talents2, talents3, attacker, roundNum,self.raceAtts);
	}
	
	//==获取暴击率==//
	private static int getCriRate(int cardId,int skillId,List<string> psList,int talent1,int talent2,int talent3,string runeId,int selfIndex,int[] teamerCardIds,int[] talents1,int[] talents2,int[] talents3,string[] raceAtts)
	{
		float cri=0;
		CardData cd=CardData.getData(cardId);
		if(cd == null)
			return 0;
		for(int i = 0 ; i < psList.Count; ++i)
		{
			PassiveSkillData psd=PassiveSkillData.getData(StringUtil.getInt(psList[i]));
			if(psd!=null && psd.type==4)
			{
				cri+=psd.numbers;
			}
		}
		
		SkillData sd=SkillData.getData(skillId);
		cri+=getTalentB_cri(talent1, sd)+getTalentB_cri(talent2, sd)+getTalentB_cri(talent3, sd);
		cri+=getTalentC_cri(cd,selfIndex,teamerCardIds,talents1)+getTalentC_cri(cd,selfIndex,teamerCardIds,talents2)+getTalentC_cri(cd,selfIndex,teamerCardIds,talents3);
		//符文//
		cri+=getRuneValue(runeId, 4);
		//==卡自带暴击==//
		cri+=cd.criRate;
		//种族加成属性//
		float raceAttValue=getRaceAtt(7,raceAtts);
		cri+=raceAttValue;
		
		cri=cri>100?100:cri;
		return (int)cri;
	}
	
	private static float getTalentB_cri(int talent,SkillData sd)
	{
		float cri=0;
		//==天赋:1对技能类型加成==//
		TalentData td=TalentData.getData(talent);
		if(sd!=null && td!=null && td.type==1 && td.class1==sd.type && td.effect==4)
		{
			cri=td.number;
		}
		//==天赋:4对特定技能==//
		if(sd!=null && td!=null && td.type==4 && td.class1/10==sd.index/10 && td.effect==5)
		{
			cri=td.number;
		}
		return cri;
	}
	
	private static float getTalentC_cri(CardData cd,int selfIndex,int[] teamerCardIds,int[] talents)
	{
		float criAdd=0;
		if(selfIndex!=-1)
		{
			//==队友天赋加成:5对本方单位==//
			for(int k=0;k<teamerCardIds.Length;k++)
			{
				int cId=teamerCardIds[k];
				if(cId!=0)
				{
					CardData cdTemp=CardData.getData(cId);
					int talent=talents[k];
					if(talent==0)
					{
						continue;
					}
					TalentData td2=TalentData.getData(talent);
					if(td2!=null && (td2.type==5 || td2.type==51) && td2.effect==4)
					{
						switch(td2.class1)
						{
						case 1://本种族卡牌//
							if(cd.race==cdTemp.race)
							{
								if(td2.number>criAdd)
								{
									criAdd=td2.number;
								}
							}
							break;
						case 2://固定前排//
							if(selfIndex<3)
							{
								if(td2.number>criAdd)
								{
									criAdd=td2.number;
								}
							}
							break;
						case 3://固定后排//
							if(selfIndex>=3)
							{
								if(td2.number>criAdd)
								{
									criAdd=td2.number;
								}
							}
							break;
						case 4://前排对位//
							if(selfIndex+3==k)
							{
								if(td2.number>criAdd)
								{
									criAdd=td2.number;
								}
							}
							break;
						case 5://所有卡牌//
							if(td2.number>criAdd)
							{
								criAdd=td2.number;
							}
							break;
						default://固定卡牌//
							if(td2.class1==cd.id)
							{
								if(td2.number>criAdd)
								{
									criAdd=td2.number;
								}
							}
							break;
						}
					}
				}
			}
		}
		return criAdd;
	}
	
	//==获取暴击率(战斗使用)==//
	public static int getCriRate(Card self,Card[] teamers)
	{
		int talent1=self.talent1;
		int talent2=self.talent2;
		int talent3=self.talent3;
		
		int selfIndex=getIndexInArray(self,teamers);
		int[] teamerCardIds=new int[6];
		int[] talents1=new int[6];
		int[] talents2=new int[6];
		int[] talents3=new int[6];
		for(int i=0;i<teamers.Length;i++)
		{
			if(teamers[i]!=null)
			{
				teamerCardIds[i]=teamers[i].getCardData().id;
				talents1[i]=teamers[i].talent1;
				talents2[i]=teamers[i].talent2;
				talents3[i]=teamers[i].talent3;
			}
		}
		return getCriRate(self.getCardData().id, self.getSkill().index, self.getPassiveSkill(), talent1, talent2, talent3, self.runeId, selfIndex, teamerCardIds, talents1, talents2, talents3,self.raceAtts);
	}
	
	//==获取闪避率==//
	private static int getAviRate(int cardId,int skillId,List<string> psList,int talent1,int talent2,int talent3,string runeId,int selfIndex,int[] teamerCardIds,int[] talents1,int[] talents2,int[] talents3,string[] raceAtts)
	{
		float avi=0;
		for(int i = 0 ; i < psList.Count;++i)
		{
			PassiveSkillData psd=PassiveSkillData.getData(StringUtil.getInt(psList[i]));
			if(psd!=null && psd.type==5)
			{
				avi+=psd.numbers;
			}
		}
		
		CardData cd=CardData.getData(cardId);
		SkillData sd=SkillData.getData(skillId);
		avi+=getTalentB_avi(talent1, sd)+getTalentB_avi(talent2, sd)+getTalentB_avi(talent3, sd);
		avi+=getTalentC_avi(cd,selfIndex,teamerCardIds,talents1)+getTalentC_avi(cd,selfIndex,teamerCardIds,talents2)+getTalentC_avi(cd,selfIndex,teamerCardIds,talents3);
		//符文//
		avi+=getRuneValue(runeId, 5);
		//==卡自带闪避==//
		avi+=cd.aviRate;
		//种族加成属性//
		float raceAttValue=getRaceAtt(8,raceAtts);
		avi+=raceAttValue;
		
		avi=avi>100?100:avi;
		return (int)avi;
	}
	
	private static float getTalentB_avi(int talent,SkillData sd)
	{
		float avi=0;
		//==天赋:1对技能类型加成==//
		TalentData td=TalentData.getData(talent);
		if(sd!=null && td!=null && td.type==1 && td.class1==sd.type && td.effect==5)
		{
			avi=td.number;
		}
		//==天赋:4对特定技能==//
		if(sd!=null && td!=null && td.type==4 && td.class1/10==sd.index/10 && td.effect==4)
		{
			avi=td.number;
		}
		return avi;
	}
	
	private static float getTalentC_avi(CardData cd,int selfIndex,int[] teamerCardIds,int[] talents)
	{
		float aviAdd=0;
		if(selfIndex!=-1)
		{
			//==队友天赋加成:5对本方单位==//
			for(int k=0;k<teamerCardIds.Length;k++)
			{
				int cId=teamerCardIds[k];
				if(cId!=0)
				{
					CardData cdTemp=CardData.getData(cId);
					int talent=talents[k];
					if(talent==0)
					{
						continue;
					}
					TalentData td2=TalentData.getData(talent);
					if(td2!=null && (td2.type==5 || td2.type==51) && td2.effect==5)
					{
						switch(td2.class1)
						{
						case 1://本种族卡牌//
							if(cd.race==cdTemp.race)
							{
								if(td2.number>aviAdd)
								{
									aviAdd=td2.number;
								}
							}
							break;
						case 2://固定前排//
							if(selfIndex<3)
							{
								if(td2.number>aviAdd)
								{
									aviAdd=td2.number;
								}
							}
							break;
						case 3://固定后排//
							if(selfIndex>=3)
							{
								if(td2.number>aviAdd)
								{
									aviAdd=td2.number;
								}
							}
							break;
						case 4://前排对位//
							if(selfIndex+3==k)
							{
								if(td2.number>aviAdd)
								{
									aviAdd=td2.number;
								}
							}
							break;
						case 5://所有卡牌//
							if(td2.number>aviAdd)
							{
								aviAdd=td2.number;
							}
							break;
						default://固定卡牌//
							if(td2.class1==cd.id)
							{
								if(td2.number>aviAdd)
								{
									aviAdd=td2.number;
								}
							}
							break;
						}
					}
				}
			}
		}
		return aviAdd;
	}
	
	//==获取闪避率(战斗使用)==//
	public static int getAviRate(Card self,Card[] teamers)
	{
		int talent1=self.talent1;
		int talent2=self.talent2;
		int talent3=self.talent3;
		int selfIndex=getIndexInArray(self,teamers);
		int[] teamerCardIds=new int[6];
		int[] talents1=new int[6];
		int[] talents2=new int[6];
		int[] talents3=new int[6];
		for(int i=0;i<teamers.Length;i++)
		{
			if(teamers[i]!=null)
			{
				teamerCardIds[i]=teamers[i].getCardData().id;
				talents1[i]=teamers[i].talent1;
				talents2[i]=teamers[i].talent2;
				talents3[i]=teamers[i].talent3;
			}
		}
		return getAviRate(self.getCardData().id, self.getSkill().index, self.getPassiveSkill(), talent1, talent2, talent3, self.runeId, selfIndex, teamerCardIds, talents1, talents2, talents3,self.raceAtts);
	}
	
	/**
	 * 获取符文属性
	 * lt@2014-2-21 上午11:22:16
	 * @param runeId 当前符文Id
	 * @param property 1atk,2def,3hp,4cri,5avi
	 * @return
	 */
	public static int getRuneValue(string runeId,int property)
	{
		if(string.IsNullOrEmpty(runeId))
		{
			return 0;
		}
		int result=0;
		string[] ss=runeId.Split('-');
		int times=StringUtil.getInt(ss[0]);
		int[] pageNums=new int[Constant.RunePageLength];
		//计算前几轮的数据//
		for(int k=0;k<times;k++)
		{
			result+=RuneTotalData.getValues(k, property);
		}
		//计算当前轮的数据//
		for(int k=0;k<pageNums.Length;k++)
		{
			pageNums[k]=StringUtil.getInt(ss[k+1]);
			int key=times*100+k+1;
			RuneTotalData rdt=RuneTotalData.getData(key);
			if(rdt.proprety==property)
			{
				result+=RuneData.getValues(key, pageNums[k]);
				if(pageNums[k]==RuneData.getGroupDataNum(key))
				{
					result+=rdt.value;
				}
			}
		}
		return result;
	}
	//==获取种族属性:1攻击数值,2防御数值,3生命数值,4攻击百分比,5防御百分比,6生命百分比,7暴击率,8闪避率==//
	private static float getRaceAtt(int attType,string[] raceAtts)
	{
		if(raceAtts==null)
		{
			return 0;
		}
		float result=0;
		foreach(string raceAtt in raceAtts)
		{
			string[] ss=raceAtt.Split('-');
			int type=StringUtil.getInt(ss[0]);
			int num=StringUtil.getInt(ss[1]);
			if(type==attType)
			{
				result+=num;
			}
		}
		return result;
	}
	
	//==计算伤害==//
    public static void calculateHurt_step0(Card attacker, List<Card> targets, List<Card> tanks, int type = 0, UIInterfaceManager uiManager = null)
	{
		int criRate=attacker.getCriRate();
		//==判断闪避暴击==//
		foreach(Card target in targets)
		{
			//==先判断闪避,再判断暴击,对每个目标分别判断==//
			int aviRate=target.getAviRate();
			//==天赋:特殊==//
			TalentData td=TalentData.getData(attacker.talent1);
			if(td!=null && td.type==7 && td.class1==3)
			{
				aviRate=0;
			}
			//==天赋:特殊==//
			TalentData td2=TalentData.getData(attacker.talent2);
			if(td2!=null && td2.type==7 && td2.class1==3)
			{
				aviRate=0;
			}
			//==天赋:特殊==//
			TalentData td3=TalentData.getData(attacker.talent3);
			if(td3!=null && td3.type==7 && td3.class1==3)
			{
				aviRate=0;
			}
			int roll=UnityEngine.Random.Range(0,100);
			int[] result=new int[4];
			if(roll<aviRate)
			{
				result[1]=2;
			}
			else if(roll-aviRate<criRate)
			{
				result[1]=1;
			}
			else
			{
				result[1]=3;
			}
            target.setHurt(result);
            if (type != 0)
            {

            }
		}
		//==坦克的防守范围内有任意一名角色（自身除外）没有闪避掉攻击，都会发动坦克的防御行为。相对的，坦克的防守范围内的全部角色都闪避此次攻击的话，则不会触发坦克的防御行为。==//
		for(int k=tanks.Count-1;k>=0;k--)
		{
			Card tank=(Card)tanks[k];
			bool canRemove=true;
			foreach(Card beTank in tank.beTanks)
			{
				if(beTank!=tank && !beTank.isAvi())
				{
					canRemove=false;
				}
			}
			if(canRemove)
			{
				tanks.Remove(tank);
			}
		}
		//==判断闪避暴击==//
		foreach(Card tank in tanks)
		{
			if(!targets.Contains(tank))
			{
				//==先判断闪避,再判断暴击,对每个目标分别判断==//
				int aviRate=tank.getAviRate();
				//==天赋:特殊==//
				TalentData td=TalentData.getData(attacker.talent1);
				if(td!=null && td.type==7 && td.class1==3)
				{
					aviRate=0;
				}
				//==天赋:特殊==//
				TalentData td2=TalentData.getData(attacker.talent2);
				if(td2!=null && td2.type==7 && td2.class1==3)
				{
					aviRate=0;
				}
				//==天赋:特殊==//
				TalentData td3=TalentData.getData(attacker.talent3);
				if(td3!=null && td3.type==7 && td3.class1==3)
				{
					aviRate=0;
				}
				int roll=UnityEngine.Random.Range(0,100);
				int[] result=new int[4];
				if(roll<aviRate)
				{
					result[1]=2;
				}
				else if(roll-aviRate<criRate)
				{
					result[1]=1;
				}
				else
				{
					result[1]=3;
				}
				tank.setHurt(result);
			}
		}
	}
	
	public static void calculateHurt_step1(int roundNum,Card srcActor,Card target,Player srcPlayer,Player targetPlayer,float tankX,int defenceType,int defenceNum,bool needLog)
	{
		int[] result=target.getHurt();
		if(result[1]==2 && defenceType!=2 && defenceType!=3)
		{
			if(needLog)
			{
				BattleLog.getInstance().log(target.sequence,result);
			}
			return;
		}
		float all_atk=srcActor.getAtk(srcPlayer.getCards(),target,roundNum);
		float all_def=target.getDef(targetPlayer.getCards(),srcActor,roundNum);
		//计算伤害//
		float damage=0;
		if(all_atk>=all_def*2)
		{
			damage=all_atk/AtkMul;
		}
		else
		{
			damage=all_atk*all_atk/AtkMul2/all_def;
		}
		//cri
		if(result[1]==1)
		{
			damage*=Constant.CriMulRate;
		}
		float mul=0;
		switch(srcActor.getSkill().atkTarget)
		{
		case 0:
			mul=1F;
			break;	
		case 1:
			mul=0.7F;
			break;
		case 2:
			mul=0.6F;
			break;
		case 3:
			mul=0.4F;
			break;
		}
		damage*=mul;
		
		//==天赋:对敌方卡牌==//
		float reduce=0;
		TalentData td=TalentData.getData(srcActor.talent1);
		if(td!=null && td.type==6 && td.class1==1 && td.effect==1)
		{
			reduce+=td.number;
		}
		TalentData td2=TalentData.getData(srcActor.talent2);
		if(td2!=null && td2.type==6 && td2.class1==1 && td2.effect==1)
		{
			reduce+=td2.number;
		}
		TalentData td3=TalentData.getData(srcActor.talent3);
		if(td3!=null && td3.type==6 && td3.class1==1 && td3.effect==1)
		{
			reduce+=td3.number;
		}
		bool cancelTank=false;
		if(td!=null && td.type==6 && td.class1==2 && td.effect==2)
		{
			cancelTank=true;
		}
		if(td2!=null && td2.type==6 && td2.class1==2 && td2.effect==2)
		{
			cancelTank=true;
		}
		float hurtAdd=0;
		if(td!=null && td.type==6 && td.class1==3 && ((td.effect==3 && target.getSkill().type==2) || (td.effect==4 && target.getSkill().type==1) || (td.effect==5 && target.getSkill().type==3)))
		{
			hurtAdd+=td.number;
		}
		if(td2!=null && td2.type==6 && td2.class1==3 && ((td2.effect==3 && target.getSkill().type==2) || (td2.effect==4 && target.getSkill().type==1) || (td2.effect==5 && target.getSkill().type==3)))
		{
			hurtAdd+=td2.number;
		}
		damage=damage*(1+hurtAdd);
		
		switch(defenceType)
		{
		case 0://非T//
			float hurtReduce=tankX-reduce;
			hurtReduce=hurtReduce<0?0:hurtReduce;
			if(cancelTank)
			{
				hurtReduce=0;
			}
			damage=damage*(1-hurtReduce);
			target.setDefendBlood(false);
			break;
		case 1://单体攻击,打T//
			//群体攻击,T在目标里并且T只保护自己//
			float number=target.getSkill().numberY;
			hurtReduce=number-reduce;
			hurtReduce=hurtReduce<0?0:hurtReduce;
			
			if(srcActor.getSkill().atkTarget==target.getSkill().defTarget)
			{
				hurtReduce=target.getSkill().numberZ;
			}
			damage=damage*(1-hurtReduce);
			target.setDefendBlood(false);
			break;
		case 2://单体攻击,不是打T//
			//群体攻击,T不在目标里//
			number=target.getSkill().numberY;
			hurtReduce=number-reduce;
			hurtReduce=hurtReduce<0?0:hurtReduce;
			
			if(srcActor.getSkill().atkTarget==target.getSkill().defTarget)
			{
				hurtReduce=target.getSkill().numberZ;
			}
			//T闪避//
			if(result[1]==2)
			{
				result[1]=3;
			}
			damage=damage*target.getSkill().numberX*defenceNum*(1-hurtReduce);
			target.setDefendBlood(true);
			break;
		case 3://群体攻击,T在目标里并且保护其他目标//
			number=target.getSkill().numberY;
			hurtReduce=number-reduce;
			hurtReduce=hurtReduce<0?0:hurtReduce;
			
			if(srcActor.getSkill().atkTarget==target.getSkill().defTarget)
			{
				hurtReduce=target.getSkill().numberZ;
			}
			//T闪避//
			if(result[1]==2)
			{
				result[1]=3;
				damage=damage*(target.getSkill().numberX*defenceNum)*(1-hurtReduce);
			}
			else
			{
				damage=damage*(target.getSkill().numberX*defenceNum+1)*(1-hurtReduce);
			}
			target.setDefendBlood(true);
			break;
		}
		
		/**上下浮动10%,伤害最少为1**/
		damage=damage*UnityEngine.Random.Range(0.97f,1.03f);
		
		if(target.isHaveRestrain((STATE.SKILL_TYPE)srcActor.getSkill().element))
		{
			damage = target.getRestrainNum(damage);
			result[3] = 1;
		}
		
		int temp=(int)damage;
		if(temp<=0)
		{
			temp=1;
		}
		
	
		result[0]=temp;
		target.setCurHp(target.getCurHp()-result[0]);
		
		if(target.isCanBacklashDamage())
		{
			target.setPSEffectType(Card.PSEffectType.E_BacklashDamage);
			target.backlashDamageTarget = srcActor;
		}
		
		//==有card死亡时==//
		if(target.getCurHp()<=0)
		{
			target.setCurHp(0);
			result[2]=1;
			
			//==天赋:3对士气槽,我方成员死亡,场上单位死亡==//
			float energyAddTar=0;
			foreach(Card c in targetPlayer.getCards())
			{
				if(c!=null && c.getCurHp()>0)
				{
					TalentData tdc=TalentData.getData(c.talent1);
					if(tdc!=null && tdc.type==3 && tdc.class1==5 && tdc.effect==2)
					{
						energyAddTar+=tdc.number;
					}
					if(tdc!=null && tdc.type==3 && tdc.class1==7 && tdc.effect==2)
					{
						energyAddTar+=tdc.number;
					}
					TalentData tdc2=TalentData.getData(c.talent2);
					if(tdc2!=null && tdc2.type==3 && tdc2.class1==5 && tdc2.effect==2)
					{
						energyAddTar+=tdc2.number;
					}
					if(tdc2!=null && tdc2.type==3 && tdc2.class1==7 && tdc2.effect==2)
					{
						energyAddTar+=tdc2.number;
					}
					TalentData tdc3=TalentData.getData(c.talent3);
					if(tdc3!=null && tdc3.type==3 && tdc3.class1==5 && tdc3.effect==2)
					{
						energyAddTar+=tdc3.number;
					}
					if(tdc3!=null && tdc3.type==3 && tdc3.class1==7 && tdc3.effect==2)
					{
						energyAddTar+=tdc3.number;
					}
				}
			}
			//==天赋:3对士气槽,敌方成员死亡,场上单位死亡==//
			float energyAdd=0;
			foreach(Card c in srcPlayer.getCards())
			{
				if(c!=null && c.getCurHp()>0)
				{
					TalentData tdc=TalentData.getData(c.talent1);
					if(tdc!=null && tdc.type==3 && tdc.class1==6 && tdc.effect==2)
					{
						energyAdd+=tdc.number;
					}
					if(tdc!=null && tdc.type==3 && tdc.class1==7 && tdc.effect==2)
					{
						energyAdd+=tdc.number;
					}
					TalentData tdc2=TalentData.getData(c.talent2);
					if(tdc2!=null && tdc2.type==3 && tdc2.class1==6 && tdc2.effect==2)
					{
						energyAdd+=tdc2.number;
					}
					if(tdc2!=null && tdc2.type==3 && tdc2.class1==7 && tdc2.effect==2)
					{
						energyAdd+=tdc2.number;
					}
					TalentData tdc3=TalentData.getData(c.talent3);
					if(tdc3!=null && tdc3.type==3 && tdc3.class1==6 && tdc3.effect==2)
					{
						energyAdd+=tdc3.number;
					}
					if(tdc3!=null && tdc3.type==3 && tdc3.class1==7 && tdc3.effect==2)
					{
						energyAdd+=tdc3.number;
					}
				}
			}
			
			switch(srcActor.getSkill().atkTarget)
			{
				case 0:
					srcPlayer.addTempEnergy(EnergyData.getEnergy(6,EnergyData.ScopeSingle)+energyAdd);
					targetPlayer.addTempEnergy(EnergyData.getEnergy(5,EnergyData.ScopeSingle)+energyAddTar);
					break;
				case 1:
					srcPlayer.addTempEnergy(EnergyData.getEnergy(6,EnergyData.ScopeSwing)+energyAdd);
					targetPlayer.addTempEnergy(EnergyData.getEnergy(5,EnergyData.ScopeSwing)+energyAddTar);
					break;
				case 2:
					srcPlayer.addTempEnergy(EnergyData.getEnergy(6,EnergyData.ScopeLine)+energyAdd);
					targetPlayer.addTempEnergy(EnergyData.getEnergy(5,EnergyData.ScopeLine)+energyAddTar);
					break;
				case 3:
					srcPlayer.addTempEnergy(EnergyData.getEnergy(6,EnergyData.ScopeAll)+energyAdd);
					targetPlayer.addTempEnergy(EnergyData.getEnergy(5,EnergyData.ScopeAll)+energyAddTar);
					break;
			}
		}
		target.setHurt(result);
		
		int addHp=0;
		//==天赋:特殊,反击==//
		int beatBackHurt=getBeatBackHurt(target.talent1,result[0])+getBeatBackHurt(target.talent2,result[0])+getBeatBackHurt(target.talent3,result[0]);
		if(beatBackHurt>0)
		{
			int[] backHurt=new int[4];
			backHurt[0]=beatBackHurt;
			backHurt[1]=5;
			addHp+=-backHurt[0];
			srcActor.createHitNumObject(backHurt[0],backHurt[1]);
		}
		int getHp=getVampire(target.talent1,result[0])+getVampire(target.talent2,result[0])+getVampire(target.talent3,result[0]);
		//==天赋:特殊,吸血==//
		if(getHp>0)
		{
			int[] backHurt=new int[4];
			backHurt[0]=getHp;
			backHurt[1]=6;
			addHp+=backHurt[0];
			srcActor.createHitNumObject(backHurt[0],backHurt[1]);
		}
		if(addHp!=0)
		{
			srcActor.setCurHp(srcActor.getCurHp()+addHp);
			if(srcActor.getCurHp()>srcActor.getMaxHp())
			{
				srcActor.setCurHp(srcActor.getMaxHp());
			}
			if(srcActor.getCurHp()<=0)
			{
				int[] hurt2=new int[4];
				hurt2[2]=1;
				srcActor.setCurHp(0);
				srcActor.setHurt(hurt2);
			}
			srcActor.changeBloodBar();
		}
		
		if(needLog)
		{
			BattleLog.getInstance().log(target.sequence,result);
		}
	}
	
	private static int getBeatBackHurt(int talent,int damage)
	{
		int hurt=0;
		//==天赋:特殊,反击==//
		TalentData tdt=TalentData.getData(talent);
		if(tdt!=null && tdt.type==7 && tdt.class1==4)
		{
			hurt=(int)(damage*tdt.number);
		}
		return hurt;
	}
	
	private static int getVampire(int talent,int damage)
	{
		int hp=0;
		//==天赋:特殊,吸血==//
		TalentData td=TalentData.getData(talent);
		if(td!=null && td.type==7 && td.class1==5)
		{
			hp=(int)(damage*td.number);
		}
		return hp;
	}
	
	/// <summary>
	/// Resume the specified srcCard, targetCard and friends.
	/// </summary>
	/// <param name='srcCard'>
	/// Source card.
	/// </param>
	/// <param name='targetCard'>
	/// Target card.
	/// </param>
	/// <param name='friends'>
	/// 为自己方的所有卡牌
	/// </param>
	public static int resume(int roundNum,Card srcCard,Card targetCard,Player srcPlayer,bool needLog)
	{
		float all_atk=srcCard.getAtk(srcPlayer.getCards(),targetCard,roundNum);
		//回复类型系数//
		float mul=0;
		switch(srcCard.getSkill().healTarget)
		{
		case 0:
			mul=1F;
			break;	
		case 1:
			mul=0.7F;
			break;
		case 2:
			mul=0.6F;
			break;
		case 3:
			mul=0.4F;
			break;
		}
		
		float roll=UnityEngine.Random.Range(0.97f,1.03f);
		int[] result=new int[4];
		result[0]=(int)(all_atk/ResumeMul*mul*ResumeMul2*roll);
		result[1]=4;
		targetCard.setCurHp(targetCard.getCurHp()+result[0]);
		if(targetCard.getCurHp()>targetCard.getMaxHp())
		{
			targetCard.setCurHp(targetCard.getMaxHp());
		}
		targetCard.setHurt(result);
		if(needLog)
		{
			BattleLog.getInstance().log(targetCard.sequence,result);
		}
		return result[0];
	}
	
	//*获取合体技的目标*//
	public static List<Card> getUnitSkillTargets(int aim,Player srcP,Player targetP,Player ownP = null)
	{
		List<Card> result=new List<Card>();
		switch(aim)
		{
		case 1://敌方全体//
			foreach(Card c in targetP.getCards())
			{
				if(c!=null && c.getCurHp()>0)
				{
					result.Add(c);
				}
			}
			break;
		case 2://敌方随机目标//
			
			break;
		case 3://我方全体//
			if(ownP != null)
			{
				foreach(Card c in ownP.getCards())
				{
					if(c!=null && c.getCurHp()>0)
					{
						result.Add(c);
					}
				}
			}
			else
			{
				foreach(Card c in srcP.getCards())
				{
					if(c!=null && c.getCurHp()>0)
					{
						result.Add(c);
					}
				}
			}
			
			break;
		case 4://我方随机目标//
			if(ownP != null)
			{
				
			}
			break;
		case 5://我方随机死亡目标//
			if(ownP != null)
			{
				result.Add(ownP.getOneRandomDeadCard());
			}
			else
			{
				result.Add(srcP.getOneRandomDeadCard());
			}
			
			break;
		case 6://敌方血量值最高单位//
			result.Add(targetP.getCurHpMaxCard());
			break;
		case 7://敌方横排目标//
			for(int i=0;i<3;i++)
			{
				Card tc=targetP.getCard(i);
				if(tc!=null && tc.getCurHp()>0)
				{
					result.Add(tc);
				}
			}
			if(result.Count==0)
			{
				for(int i=3;i<6;i++)
				{
					Card tc=targetP.getCard(i);
					if(tc!=null && tc.getCurHp()>0)
					{
						result.Add(tc);
					}
				}
			}
			break;
		}
		return result;
	}
	
	/**合体技伤害**/
	public static void calculateHurtForUnitSkill(int roundNum,List<Card> target,Player srcPlayer,Player targetPlayer,int effect1,int effect2)
	{
		//获取all_atk//
		float all_atk=0;
		int num=0;
		foreach(Card c in srcPlayer.getCards())
		{
			if(c!=null)
			{
				num++;
				all_atk+=c.getAtk(srcPlayer.getCards(),null,roundNum);
			}
		}
		if(num==0)
		{
			return;
		}
		all_atk=all_atk/num;

		//获取受伤目标的all_def//
		foreach(Card c in target)
		{
			//合体技默认元素伤害类型为0//
			float all_def=c.getDef(targetPlayer.getCards(),null,roundNum);
			//最终伤害//
			float roll=UnityEngine.Random.Range(0.97f,1.03f);
			float damage=0;
			if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Battle2_Bounes))
			{
				damage = c.getCurHp();
			}
			else
			{
				if(all_atk>=all_def*2)
				{
					damage=(all_atk/AtkMul*(effect1*0.01f)+effect2)*roll;
				}
				else
				{
					damage=(all_atk*all_atk/all_def/AtkMul2*(effect1*0.01f)+effect2)*roll;
				}
			}
			
			
			//damage=1f;
			
			int[] result=new int[4];
			result[0]=(int)damage;
			result[1]=3;
			c.setCurHp(c.getCurHp()-result[0]);
			if(c.getCurHp()<=0)
			{
				c.setCurHp(0);
				result[2]=1;
			}
			c.setHurt(result);
			//BattleLog.getInstance().log(c.sequence,result);
		}
	}

	
	public static void resumeForUnitSkill(int roundNum,List<Card> target,Player srcPlayer,Player targetPlayer,int effect1,int effect2)
	{
		//获取all_atk//
		float all_atk=0;
		int num=0;
		foreach(Card c in srcPlayer.getCards())
		{
			if(c!=null)
			{
				num++;
				all_atk+=c.getAtk(srcPlayer.getCards(),null,roundNum);
			}
		}
		if(num==0)
		{
			return;
		}
		all_atk=all_atk/num;
		
		// calc resume 
		foreach(Card c in target)
		{
			if(c.getCurHp() <= 0)
			{
				continue;
			}
			//最终回复系数修正//
			float roll=UnityEngine.Random.Range(0.97f,1.03f);
			float resumeValue= (all_atk / ResumeMul * effect1*0.01f + effect2)*roll;
			
			int[] result=new int[4];
			result[0]=(int)resumeValue;
			result[1]=4;
			
			c.setCurHp(c.getCurHp()+result[0]);
			c.setHurt(result);
			//BattleLog.getInstance().log(c.sequence,result);
		}
	}
	
	public static void recoverForUnitSkill(Card c,int recoverHp)
	{
		int[] result=new int[4];
		result[0]=recoverHp;
		result[1]=4;
		c.setCurHp(result[0]);
		c.setHurt(result);
	}
	
	public static int getIndexInArray(PackElement c,PackElement[] cs)
	{
		int index=-1;
		for(int k=0;k<cs.Length;k++)
		{
			if(cs[k]==c)
			{
				index=k;
				break;
			}
		}
		return index;
	}
	
	public static int getIndexInArray(Card c,Card[] cs)
	{
		int index=-1;
		for(int k=0;k<cs.Length;k++)
		{
			if(cs[k]==c)
			{
				index=k;
				break;
			}
		}
		return index;
	}
	
	// get skill value in ui show//
	public static string getSkillValueForUIShow(int skillID,int level)
	{
		string str = string.Empty;
		SkillData sd = SkillData.getData(skillID);
		if(sd == null)
		{
			return str;
		}
		if(sd.exptype == 2)
		{
			str = sd.description;
			return str;
		}
		float skillValue = SkillPropertyData.getProperty(sd.type,level,sd.star);
		
		float mul=0;
		switch(sd.atkTarget)
		{
		case 0:
			mul=1F;
			break;	
		case 1:
			mul=0.7F;
			break;
		case 2:
			mul=0.6F;
			break;
		case 3:
			mul=0.4F;
			break;
		}
		switch(sd.type)
		{
		case 1:
		{
			skillValue*=mul;
		}break;
		case 2:
		{
			// do nothing//
		}break;
		case 3:
		{
			skillValue*=mul;
			skillValue /= 2;
		}break;
		}
		str = sd.upgradetext + (int)skillValue;
		return str;
	}
	
	
	public static string getSkillValueForUIShow02(int skillID,int level)
	{
		string str = string.Empty;
		SkillData sd = SkillData.getData(skillID);
		if(sd == null)
		{
			return str;
		}
		if(sd.exptype == 2)
		{
			return str;
		}
		float skillValue = SkillPropertyData.getProperty(sd.type,level,sd.star);
		
		float mul=0;
		switch(sd.atkTarget)
		{
		case 0:
			mul=1F;
			break;	
		case 1:
			mul=0.7F;
			break;
		case 2:
			mul=0.6F;
			break;
		case 3:
			mul=0.4F;
			break;
		}
		switch(sd.type)
		{
		case 1:
		{
			skillValue*=mul;
		}break;
		case 2:
		{
			// do nothing//
		}break;
		case 3:
		{
			skillValue*=mul;
			skillValue /= 2;
		}break;
		}
		str = ((int)skillValue).ToString();
		return str;
	}
	
	//获取装备增加数值//
	public static string  getEquipValueForUIShow(int equipId, int level)
	{
		EquipData ed = EquipData.getData(equipId);
		int type = ed.type;
		int star = ed.star;
		EquippropertyData epd = EquippropertyData.getData(type, level);
		int equipValue = epd.starNumbers[star - 1];
		string str = equipValue.ToString();
		return str;
	}
	
	//==检验是否达成KO==//
	//==missionId:关卡Id==//
	//==bonus:是否已bonus==//
	//==unitKill:合体技击杀==//
	//==p:玩家==//
	//==round:回合数==//
	public static bool checkKO(int missionId,int bonus,bool unitKill,Player p,int round)
	{
		MissionData md=MissionData.getData(missionId);
		if(md==null)
		{
			return false;
		}
		//判断KO
		bool verifyBonus=false;
		if(md.addtasktype>0 && bonus ==0)
		{
			int condition=md.addtaskid;
			switch(md.addtasktype)
			{
			case 1://使用合体技击杀最后一个敌人//
				verifyBonus=unitKill;
				break;
			case 2://队伍中拥有指定星级卡牌//
				foreach(Card c in p.getCards())
				{
					if(c!=null)
					{
						CardData cd=c.getCardData();
						if(cd!=null && cd.star==condition)
						{
							verifyBonus=true;
							break;
						}
					}
				}
				break;
			case 3://不使用指定星级卡牌//
				verifyBonus=true;
				foreach(Card c in p.getCards())
				{
					if(c!=null)
					{
						CardData cd=c.getCardData();
						if(cd!=null && cd.star==condition)
						{
							verifyBonus=false;
							break;
						}
					}
				}
				break;
			case 4://队伍中拥有指定种族卡牌//
				foreach(Card c in p.getCards())
				{
					if(c!=null)
					{
						CardData cd=c.getCardData();
						if(cd!=null && cd.race==condition)
						{
							verifyBonus=true;
							break;
						}
					}
				}
				break;
			case 5://不使用指定种族卡牌//
				verifyBonus=true;
				foreach(Card c in p.getCards())
				{
					if(c!=null)
					{
						CardData cd=c.getCardData();
						if(cd!=null && cd.race==condition)
						{
							verifyBonus=false;
							break;
						}
					}
				}
				break;
			case 6://队伍中拥有指定卡牌//
				foreach(Card c in p.getCards())
				{
					if(c!=null && c.getCardData().id==condition)
					{
						verifyBonus=true;
						break;
					}
				}
				break;
			case 7://不使用指定卡牌//
				verifyBonus=true;
				foreach(Card c in p.getCards())
				{
					if(c!=null && c.getCardData().id==condition)
					{
						verifyBonus=false;
						break;
					}
				}
				break;
			case 8://队伍中拥有指定类型技能//
				foreach(Card c in p.getCards())
				{
					if(c!=null)
					{
						SkillData sd=c.getSkill();
						if(sd!=null && sd.type==condition)
						{
							verifyBonus=true;
							break;
						}
					}
				}
				break;
			case 9://不使用指定类型技能//
				verifyBonus=true;
				foreach(Card c in p.getCards())
				{
					if(c!=null)
					{
						SkillData sd=c.getSkill();
						if(sd!=null && sd.type==condition)
						{
							verifyBonus=false;
							break;
						}
					}
				}
				break;
			case 10://队伍中拥有指定属性技能//
				foreach(Card c in p.getCards())
				{
					if(c!=null)
					{
						SkillData sd=c.getSkill();
						if(sd!=null && sd.element==condition)
						{
							verifyBonus=true;
							break;
						}
					}
				}
				break;
			case 11://不使用指定属性技能//
				verifyBonus=true;
				foreach(Card c in p.getCards())
				{
					if(c!=null)
					{
						SkillData sd=c.getSkill();
						if(sd!=null && sd.element==condition)
						{
							verifyBonus=false;
							break;
						}
					}
				}
				break;
			case 12://队伍中拥有指定技能//
				foreach(Card c in p.getCards())
				{
					if(c!=null)
					{
						SkillData sd=c.getSkill();
						if(sd!=null && sd.index==condition)
						{
							verifyBonus=true;
							break;
						}
					}
				}
				break;
			case 13://不使用指定技能//
				verifyBonus=true;
				foreach(Card c in p.getCards())
				{
					if(c!=null)
					{
						SkillData sd=c.getSkill();
						if(sd!=null && sd.index==condition)
						{
							verifyBonus=false;
							break;
						}
					}
				}
				break;
			case 14://规定回合数胜利//
				verifyBonus=(round<=condition);
				break;
			case 15://队伍中拥有指定星级技能//
				foreach(Card c in p.getCards())
				{
					if(c!=null)
					{
						SkillData sd=c.getSkill();
						if(sd!=null && sd.star==condition)
						{
							verifyBonus=true;
							break;
						}
					}
				}
				break;
			}
		}
		return verifyBonus;
	}
	
	//判断卡组中是否装备了该卡牌//
	public static bool IsCardInCardGroup(CardGroup cg, int cardId)
	{
		for(int i = 0;cg!=null && i < cg.cards.Length;i++)
		{
			if(cg!= null && cg.cards[i] != null && cardId == cg.cards[i].dataId)
			{
				return true;
			}
		}
		return false;
	}
	
	public static void doForbitAttack(Card actionCard,Player targetPlayer,List<int> targetAliveCardIndexList,int startRound)
	{
		if(targetPlayer == null)
			return;
		if(targetAliveCardIndexList.Count == 0)
			return;
		int fNum = actionCard.getForbitAttackCount();
		//fNum = 3; // TODO zay
		if(fNum == 0)
			return;
		List<int> tempList = new List<int>(); // tempList element is index in targetPlayer cards
		if(fNum >= targetAliveCardIndexList.Count)
		{
			tempList = targetAliveCardIndexList;
		}
		else
		{
			findRandomList(ref targetAliveCardIndexList,ref tempList,fNum);
		}
		
		for(int i = 0; i < tempList.Count;++i)
		{
			int index = tempList[i];
			Card targetCard = targetPlayer.getCard(index);
			if(targetCard == null)
				return;
			BattleEffectHelperControl.mInstance.createChangeSheepEffect(targetCard.body.transform.position);
			targetPlayer.setForbitCardIndex(index);
			targetCard.setPSEffectType(Card.PSEffectType.E_ForbitAttack);
			int effectNum = (int)actionCard.cardData.getPSEffectNum();
			//effectNum = 1;// TODO Zay
			targetCard.setPSEffectNum(effectNum);
			targetCard.setFAStartRound(startRound);
			targetCard.doForbitAttack();
			
		}
		actionCard.playCastPSEffect();
	}
	
	public static void findRandomList(ref List<int> tList,ref List<int> rList,int num)
	{
		if(num == 0)
		{
			return;
		}
		int index = UnityEngine.Random.Range(0,tList.Count);
		rList.Add(tList[index]);
		tList.RemoveAt(index);
		num--;
		findRandomList(ref tList,ref rList,num);
	}
	
	public static void doReduceEnergy(Card actionCard,Player targetPlayer)
	{
		if(targetPlayer == null)
			return;
		int reNum = actionCard.getCardData().getForbitAttackCountOrReduceEnergy();
		//reNum = 20;
		actionCard.playCastPSEffect();
		BattleEffectHelperControl.mInstance.showEffect(targetPlayer,reNum);
	}
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DemoBattleUnitSkillDataInfo
{
	public UnitSkillData usd;
	public int energy;
}

public class Player
{
	private int team;
	//==为防止玩家以修改内存的方式修改怒气,此处使用一个简单的加密算法==//
	private const string suffix="please break";
	private const int suffixLength=12;
	private string energy;
	private int maxEnergy;
	private int tempEnergy;
	private int curActionIndex;
	private Card[] cards;
	
	Card[] sheepCards = new Card[6];
	List<int> forbitCardIndexList = new List<int>();
	List<int> newForbitCardIndexList = new List<int>();
	
	private UnitSkillData[] unitSkills;
	public string[] raceAtts;
	
	private List<DemoBattleUnitSkillDataInfo> demoUnitSkillList;
	
	CardJson[] bornCardJson;
	string bornCardRuneId;
	int bornCardTeam;
	int bornCardCount = 0;
	List<int> bornIndexList = new List<int>();
	//int nowBornIndex = -1;
	
	GameObject effectPrefab;
	
	public List<GameObject> effectObjList= new List<GameObject>();
	
	public Card[] unitSkillShowCards;
	
	public int needReduceEnergyNum = 0;
	
	public int getTeam()
	{
		return team;
	}
	
	public void setUnitSkills(UnitSkillData[] unitSkills)
	{
		this.unitSkills=unitSkills;
	}
	
	public UnitSkillData[] getUnitSkills()
	{
		return unitSkills;
	}
	
	public void setDemoUnitSkills(List<DemoBattleUnitSkillDataInfo> infoList)
	{
		demoUnitSkillList = infoList;
	}
	
	public List<DemoBattleUnitSkillDataInfo> getDemoUnitSkillList()
	{
		return demoUnitSkillList;
	}
	
	public Card[] getCards()
	{
		return cards;
	}
	
	public Card getCard(int index)
	{
		if(index>=cards.Length)
		{
			return null;
		}
		return cards[index];
	}
	
	public List<Card> getLiveCards()
	{
		List<Card> arr = new List<Card>();
		for(int i = 0 ;i < cards.Length; ++i)
		{
			Card c=cards[i];
			if(c==null || c.getCurHp()<=0)
			{
				continue;
			}
			arr.Add(c);
		}
		return arr;
	}
	
	public Card[] getSheepCards()
	{
		return sheepCards;
	}
	
	public Card getSheepCardByIndex(int index)
	{
		if(index>=sheepCards.Length || index <= -1)
		{
			return null;
		}
		return sheepCards[index];
	}
	
	public void setForbitCardIndex(int index)
	{
		if(index>=sheepCards.Length || index <= -1)
		{
			return;
		}
		Card card = cards[index];
		if(card == null || card.isDeath())
			return;
		if(forbitCardIndexList.Contains(index))
		{
			return;
		}
		forbitCardIndexList.Add(index);
		newForbitCardIndexList.Add(index);
	}
	
	public void clearNewForbitCardList()
	{
		newForbitCardIndexList.Clear();
	}
	
	public List<int> getForbitCardIndex()
	{
		return forbitCardIndexList;
	}
	
	public bool isNeedDoForbit(int index)
	{
		if(newForbitCardIndexList.Contains(index))
			return true;
		return false;
	}
	
	public void doForbitAttackEffect(int index)
	{
		if(sheepCards[index] != null)
		{
			return;	
		}
		
		sheepCards[index] = cards[index];
		
		int effectNum = cards[index].getPSEffectNum();
		int startRound = cards[index].getFAStartRound();
		
		int cardId = 53004; // sheep id
		int bn = cards[index].breakNum;
		int level = cards[index].level;
		SkillData sd = cards[index].skill;
		int skillLevel = cards[index].skillLevel;
		List<string> psList = cards[index].passiveSkills;
		List<string> equipInfos = cards[index].equipInfos;
		bool mm = true;//cards[index].missionMonster;
		int atk = cards[index].atk;
		int def = cards[index].def;
		int maxHP = cards[index].maxHp;
		int curHP = cards[index].curHp;
		int bossMark = 0;
		if(cards[index].isBoss())
		{
			bossMark = 1;
		}
		string runeID = cards[index].runeId;
		int critRate = cards[index].getCriRate();
		int aviRate = cards[index].getAviRate();
		int t1 = cards[index].talent1;
		int t2 = cards[index].talent2;
		int t3 = cards[index].talent3;
		string[] raceAtts = cards[index].raceAtts;
		string sheepCardName = cards[index].cardData.name;
		
		CardData cardData=CardData.getData(cardId);
		if(cardData==null)
		{
			return;
		}
		string cardModel = cardData.cardmodel;
		GameObject obj = GameObject.Instantiate(GameObjectUtil.LoadResourcesPrefabs(cardModel,0)) as GameObject;
		obj.AddComponent<Card>();
		GameObject pos=cards[index].gameObject;
		obj.transform.position = pos.transform.position;
		obj.transform.rotation = pos.transform.rotation;
		if(bossMark  == 1)
		{
			obj.transform.localScale = new Vector3(2.5f,2.5f,2.5f);
		}
		else
		{
			obj.transform.localScale = new Vector3(obj.transform.localScale.x * 1.35f,obj.transform.localScale.y * 1.35f, obj.transform.localScale.z * 1.35f);
		}
		sheepCards[index].gameObject.transform.position = new Vector3(1000,1000,0);
		sheepCards[index].bloodBar.SetActive(false);
		sheepCards[index].nameObj.SetActive(false);
		sheepCards[index].gameObject.SetActive(false);
		cards[index] = null;
		Card sheepCard = obj.GetComponent<Card>();
		sheepCard.setData(team, index, cardId, bn, sd.index, skillLevel, psList, equipInfos, level, mm, atk, def, maxHP, bossMark, runeID, critRate, aviRate, t1, t2,t3, raceAtts);
		sheepCard.sheepCardName = sheepCardName;
		sheepCard.setCurHp(curHP);
		sheepCard.isSetInfo = true;
		sheepCard.setPSEffectType(Card.PSEffectType.E_ForbitAttack);
		sheepCard.setPSEffectNum(effectNum);
		sheepCard.setFAStartRound(startRound );
	
		cards[index] = sheepCard;
	}
	
	public void doResumeForbitAttack(int index)
	{
		if(sheepCards[index] == null)
			return;
		Vector3 pos = cards[index].gameObject.transform.position;
		cards[index].gameObject.transform.position = new Vector3(1000,1000,0);
		cards[index].bloodBar.SetActive(false);
		cards[index].nameObj.SetActive(false);
		int curHP = cards[index].curHp;
		PVESceneControl.mInstance.needRemoveSheeps.Add(cards[index]);
		sheepCards[index].gameObject.SetActive(true);
		cards[index] = sheepCards[index];
		
		cards[index].gameObject.transform.position =pos;
		cards[index].setCurHp(curHP);
		cards[index].bloodBar.SetActive(true);
		cards[index].nameObj.SetActive(true);
		cards[index].changeBloodBar();
		cards[index].setPSEffectType(Card.PSEffectType.E_Normal);
		cards[index].setPSEffectNum(0);
		cards[index].setFAStartRound(0 );
	
		sheepCards[index] = null;
		PVESceneControl.mInstance.doRemoveSheep();
	}
	
	public List<Card> getDemoUnitSkillCards()
	{
		List<Card> arr = new List<Card>();
		int count = 0;
		for(int i = 0 ;i < cards.Length; ++i)
		{
			if(count >1)
			{
				break;
			}
			Card c=cards[i];
			if(c==null || c.getCurHp()<=0)
			{
				continue;
			}
			count ++;
			arr.Add(c);
		}
		return arr;
	}
	
	
	public int getCurActionIndex()
	{
		return curActionIndex;
	}
	
	public int addCurActionIndex()
	{
		curActionIndex+=1;
		curActionIndex=curActionIndex%cards.Length;
		return curActionIndex;
	}
	
	public void tempEnergyToEnergy()
	{
		int a=getEnergy();
		a+=tempEnergy;
		a=a>maxEnergy?maxEnergy:a;
		setEnergy(a);
		tempEnergy=0;
	}
	
	public void addTempEnergy(float num)
	{
		if(num<=0)
		{
			return;
		}
		tempEnergy+=(int)num;
	}
	
	public void removeEnergy(int num)
	{
		if(num<=0)
		{
			return;
		}
		int a=getEnergy();
		a-=num;
		setEnergy(a);
	}
	
	private void setEnergy(int num)
	{
		if(num < 0)
			num = 0;
		energy=num+suffix;
	}
	
	public int getEnergy()
	{
		if(string.IsNullOrEmpty(energy))
		{
			return 0;
		}
		int length=energy.Length;
		return StringUtil.getInt(energy.Substring(0,length-suffixLength));
	}
	
	public int getMaxEnergy()
	{
		return maxEnergy;
	}
	
	public void init(CardJson[] datas,int team,string runeId,int maxEnergy,int initEnergy)
	{
		this.team=team;
		this.maxEnergy=maxEnergy;
		this.setEnergy(initEnergy);
		
		this.curActionIndex=-1;
		cards=new Card[6];
		
		sheepCards = new Card[6];
		for(int i=0;i<6;i++)
		{
			CardJson cj=datas[i];
			if(cj==null)
			{
				continue;
			}
			int index=cj.se;
			int cardId=cj.c;
			int level=cj.lv;
			int skillId=cj.s;
			int skillLevel=cj.slv;
			List<string> equipInfos=cj.es;
			CardData cardData=CardData.getData(cardId);
			if(cardData==null)
			{
				continue;
			}
			string cardModel = cardData.cardmodel;
			GameObject obj = GameObject.Instantiate(GameObjectUtil.LoadResourcesPrefabs(cardModel,0)) as GameObject;
			obj.AddComponent<Card>();
			GameObject pos=SceneInfo.getInstance().getPosition(team*6+index);
			obj.transform.position = pos.transform.position;
			obj.transform.rotation = pos.transform.rotation;
			obj.transform.localScale = new Vector3(obj.transform.localScale.x * 1.35f,obj.transform.localScale.y * 1.35f, obj.transform.localScale.z * 1.35f);
			Card card=obj.GetComponent<Card>();
			if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE&&this.team == 0)
			{
				//赋值迷宫单独使用的血量
				if(PlayerInfo.getInstance().curMazeBattleCardHp.Count!=0)
				{
					card.setData(team, index, cardId, cj.b, skillId, skillLevel, cj.ps, equipInfos, level, cj.mm, cj.atk, cj.def, PlayerInfo.getInstance().curMazeBattleCardHp[cardId], cj.bm, runeId, cj.cri, cj.avi, cj.t1, cj.t2, cj.t3, raceAtts);
				}
			}
			else
			{
				card.setData(team, index, cardId, cj.b, skillId, skillLevel, cj.ps, equipInfos, level, cj.mm, cj.atk, cj.def, cj.hp, cj.bm, runeId, cj.cri, cj.avi, cj.t1, cj.t2, cj.t3, raceAtts);
			}
			if(cj.bm == 1)
			{
				obj.transform.localScale = new Vector3(2.5f,2.5f,2.5f);
			}
			card.team=team;
			card.sequence=index;
			cards[index]=card;
		}
		//==设置5项属性,受队友天赋影响==//
		foreach(Card c in cards)
		{
			if(c!=null)
			{
				c.setAttribute(cards);
			}
		}
	}
	
	public void initUnitSkillShowCards(CardJson[] datas,int teamID,int unitSkillID)
	{
		unitSkillShowCards = new Card[6];
		
		UnitSkillData usd = UnitSkillData.getData(unitSkillID);
		if(usd == null)
			return;
		int[] cardIDList = usd.cards;
		int posIndex = 0;
		if(cardIDList == null)
			return;
		for(int i = 0; i < cardIDList.Length; ++i)
		{
			if(cardIDList[i] == 0)
				continue;
			
			int cardId=cardIDList[i];
			CardData cardData=CardData.getData(cardId);
			if(cardData==null)
			{
				continue;
			}
			int index = posIndex;
			posIndex++;
			
			int level=1;
			int skillId=cardData.basicskill;
			int skillLevel=1;
			List<string> psList = new List<string>();
			List<string> equipInfos = new List<string>();
			
			string cardModel = cardData.cardmodel;
			GameObject obj = GameObject.Instantiate(GameObjectUtil.LoadResourcesPrefabs(cardModel,0)) as GameObject;
			obj.AddComponent<Card>();
			GameObject pos=SceneInfo.getInstance().getPosition(teamID*6+index);
			obj.transform.position = pos.transform.position;
			obj.transform.rotation = pos.transform.rotation;
			obj.transform.localScale = new Vector3(obj.transform.localScale.x * 1.35f,obj.transform.localScale.y * 1.35f, obj.transform.localScale.z * 1.35f);
			Card card=obj.GetComponent<Card>();
			
			card.setData(team, index, cardId, 0, skillId, skillLevel, psList, equipInfos, level, false, 1, 1, 1, 0, "", 0, 0, 0,0, 0, raceAtts);
			card.team=teamID;
			card.sequence=index;
			card.hideObj();
			unitSkillShowCards[index]=card;
		}
	}
	
	public bool isDead()
	{
		foreach(Card ca in cards)
		{
			if(ca!=null && ca.getCurHp()>0)
			{
				return false;
			}
		}
		return true;
	}
	
	public bool haveDeadCard()
	{
		foreach(Card c in cards)
		{
			if(c!=null && c.getCurHp()==0)
			{
				return true;
			}
		}
		return false;
	}
	
	public Card getOneLiveCard()
	{
		foreach(Card c in cards)
		{
			if(c!=null && c.getCurHp()>0)
			{
				return c;
			}
		}
		return null;
	}
	
	public Card getOneRandomDeadCard()
	{
		List<Card> list=new List<Card>();
		foreach(Card c in cards)
		{
			if(c!=null && c.getCurHp()==0)
			{
				list.Add(c);
			}
		}
		int roll=Random.Range(0,list.Count);
		return list[roll];
	}
	
	public int getDeadCardNum(){
		int num = 0;
		foreach(Card c in cards)
		{
			if(c!=null && c.getCurHp()==0)
			{
				num ++;
			}
		}
		return num;
	}
	
	//==获取当前血量最大的卡==//
	public Card getCurHpMaxCard()
	{
		Card result=null;
		foreach(Card c in cards)
		{
			if(c!=null)
			{
				if(result==null)
				{
					result=c;
				}
				else
				{
					if(c.getCurHp()>result.getCurHp())
					{
						result=c;
					}
				}
			}
		}
		return result;
	}
	
	public string getUnitIds()
	{
		string ids="";
		for(int i=0;i<unitSkills.Length;i++)
		{
			UnitSkillData usd=unitSkills[i];
			if(usd==null)
			{
				ids+="0&";
			}
			else
			{
				ids+=usd.index+"&";
			}
		}
		ids=ids.Substring(0,ids.Length-1);
		
		return ids;
	}
	
	public string getDemoUnitIds()
	{
		string ids = "";
		for(int i = 0; i < demoUnitSkillList.Count;++i)
		{
			DemoBattleUnitSkillDataInfo info = demoUnitSkillList[i];
			if(info.usd == null)
			{
				ids += "0&";
			}
			else
			{
				ids += info.usd.index;
				ids += "&";
			}
		}
		ids = ids.Substring(0,ids.Length-1);
		return ids;
	}
	
	public void playWinAction()
	{
		for(int i = 0; i < cards.Length;++i)
		{
			Card c = (Card)cards[i];
			if(c != null && c.getCurHp() > 0)
			{
				c.animator.SetBool("idle2win",true);
			}
		}
	}
	
	public void setBornCardParam(CardJson[] cJsons,int team,string runeld,int maxEnergy,int initEnergy)
	{
		this.maxEnergy=maxEnergy;
		this.setEnergy(initEnergy);
		
		bornCardJson = cJsons;
		bornCardTeam = team;
		bornCardRuneId = runeld;
		bornCardCount = 0;
		for(int i = 0; i < cJsons.Length;++i)
		{
			CardJson cj = cJsons[i];
			if(cj != null)
			{
				bornCardCount++;
				bornIndexList.Add(i);
			}
		}
		this.team=bornCardTeam;
		this.curActionIndex=-1;
		cards=new Card[6];
		effectPrefab = Resources.Load("Prefabs/Effects/appear") as GameObject;
	}
	
	public bool checkIsCanGoOnBornCard(int bornIndex)
	{
		if(bornIndex >(bornCardCount-1))
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	
	public void bornEffectByIndex(int bornIndex)
	{
		CardJson cj = bornCardJson[bornIndexList[bornIndex]];
		GameObject posObj =SceneInfo.getInstance().getPosition(bornCardTeam*6+cj.se);
		GameObject effectObj = GameObject.Instantiate(effectPrefab) as GameObject;
		GameObjectUtil.copyTarnsformValue(posObj,effectObj);
		effectObjList.Add(effectObj);
	}
	
	public void bornCardByIndex(int bornIndex)
	{
		CardJson cj = bornCardJson[bornIndexList[bornIndex]];
		int index=cj.se;
		int cardId=cj.c;
		int level=cj.lv;
		int skillId=cj.s;
		int skillLevel=cj.slv;
	
		List<string> equipInfos=cj.es;
		CardData cardData=CardData.getData(cardId);
		if(cardData==null)
		{
			return;
		}
		string cardModel = cardData.cardmodel;

		GameObject obj = GameObject.Instantiate(GameObjectUtil.LoadResourcesPrefabs(cardModel,0)) as GameObject;
		obj.AddComponent<Card>();
		GameObject pos=SceneInfo.getInstance().getPosition(team*6+index);
		obj.transform.position = pos.transform.position;
		obj.transform.rotation = pos.transform.rotation;
		obj.transform.localScale = new Vector3(obj.transform.localScale.x * 1.35f,obj.transform.localScale.y * 1.35f, obj.transform.localScale.z * 1.35f);
		Card card=obj.GetComponent<Card>();
		card.setData(team, index, cardId, cj.b, skillId, skillLevel, cj.ps, equipInfos, level, cj.mm, cj.atk, cj.def, cj.hp, cj.bm, bornCardRuneId, cj.cri, cj.avi, cj.t1, cj.t2, cj.t3, raceAtts);
		if(cj.bm == 1)
		{
			obj.transform.localScale = new Vector3(2.5f,2.5f,2.5f);
		}
		card.team=team;
		card.sequence=index;
		cards[index]=card;

	}
	
	public void setCardAttibute()
	{
		//==设置5项属性,受队友天赋影响==//
		foreach(Card c in cards)
		{
			if(c!=null)
			{
				c.setAttribute(cards);
			}
		}
		
	}
	
	public void clearBornEffect()
	{
		for(int i = 0 ;i < effectObjList.Count;++i)
		{
			GameObject.Destroy(effectObjList[i]);
		}
		effectObjList.Clear();
	}
	
	public void initFriend(CardJson[] datas,string runeId)
	{
		if(datas==null)
		{
			return;
		}
		cards=new Card[6];
		for(int i=0;i<6;i++)
		{
			CardJson cj=datas[i];
			if(cj==null)
			{
				continue;
			}
			int index=cj.se;
			int cardId=cj.c;
			int level=cj.lv;
			int skillId=cj.s;
			int skillLevel=cj.slv;
			List<string> equipInfos=cj.es;
			CardData cardData=CardData.getData(cardId);
			if(cardData==null)
			{
				continue;
			}
			//GameObject obj=new GameObject();
			string cardModel = cardData.cardmodel;
			GameObject obj = GameObject.Instantiate(GameObjectUtil.LoadResourcesPrefabs(cardModel,0)) as GameObject;
			
			GameObject pos=SceneInfo.getInstance().getPosition(index);
			obj.transform.position = pos.transform.position;
			obj.transform.rotation = pos.transform.rotation;
			obj.transform.localScale = new Vector3(obj.transform.localScale.x * 1.35f,obj.transform.localScale.y * 1.35f, obj.transform.localScale.z * 1.35f);
			Card card=obj.AddComponent<Card>();
			card.setData(0, index, cardId, cj.b, skillId, skillLevel, cj.ps, equipInfos, level, cj.mm, cj.atk, cj.def, cj.hp, cj.bm, runeId, cj.cri, cj.avi, cj.t1, cj.t2, cj.t3, raceAtts);
			if(cj.bm == 1)
			{
				obj.transform.localScale = new Vector3(2.5f,2.5f,2.5f);
			}
			card.sequence=index;
			cards[index]=card;
		}
		//==设置5项属性,受队友天赋影响==//
		foreach(Card c in cards)
		{
			if(c!=null)
			{
				c.setAttribute(cards);
			}
		}
	}
	
	public void hideCards()
	{
		if(cards==null)
		{
			return;
		}
		foreach(Card c in cards)
		{
			if(c!=null)
			{
				c.hideObj();
				
			}
		}
	}
	
	public void showCards()
	{
		foreach(Card c in cards)
		{
			if(c!=null && c.getCurHp() > 0)
			{
				c.recoverShowObj();
			}
		}
	}
	
	public void showUnitSkillCards()
	{
		for(int i = 0; i < unitSkillShowCards.Length;++i)
		{
			if(unitSkillShowCards[i] != null)
				unitSkillShowCards[i].recoverShowObj();
		}
	}
	
	public void hideUnitSkillShowCards()
	{
		foreach(Card c in unitSkillShowCards)
		{
			if(c != null)
			{
				c.hideObj();
			}
		}
	}
	
	public void destoryCards()
	{
		hideUnitSkillShowCards();
		for(int i=0;i<6;i++)
		{
			Card c=cards[i];
			if(c==null)
			{
				continue;
			}
			c.bloodBar.SetActive(false);
			c.nameObj.SetActive(false);
			GameObject.DestroyImmediate(c._myTransform.gameObject);
			cards[i]=null;
		}
	}
	
	GameObject friendAppearEffectPrefab;
	GameObject fazhenPrefab;
	
	public void friendStart()
	{
		friendAppearEffectPrefab = Resources.Load("Prefabs/Effects/haoyou_zhaohuan") as GameObject;
		fazhenPrefab= Resources.Load("Prefabs/Effects/haoyou_fazhen") as GameObject;
		for(int i=0;i<cards.Length;i++)
		{
			Card c=cards[i];
			if(c!=null)
			{
				c.hideObj();
			}
			GameObject posObj =SceneInfo.getInstance().getPosition(i);
			GameObject effectObj = GameObject.Instantiate(friendAppearEffectPrefab) as GameObject;
			GameObjectUtil.copyTarnsformValue(posObj,effectObj);
			GameObject.Destroy(effectObj,2f);
		}
		GameObject center=SceneInfo.getInstance().getMyCenter();
		GameObject fazhen=GameObject.Instantiate(fazhenPrefab) as GameObject;
		GameObjectUtil.copyTarnsformValue(center,fazhen);
		GameObject.Destroy(fazhen,2f);
	}
	
	public void friendOver()
	{
		for(int i=0;i<cards.Length;i++)
		{
			Card c=cards[i];
			if(c!=null && c.getCurHp() > 0)
			{
				c.recoverShowObj();
			}
			GameObject posObj =SceneInfo.getInstance().getPosition(i);
			GameObject effectObj = GameObject.Instantiate(friendAppearEffectPrefab) as GameObject;
			GameObjectUtil.copyTarnsformValue(posObj,effectObj);
			GameObject.Destroy(effectObj,2f);
		}
		GameObject center=SceneInfo.getInstance().getMyCenter();
		GameObject fazhen=GameObject.Instantiate(fazhenPrefab) as GameObject;
		GameObjectUtil.copyTarnsformValue(center,fazhen);
		GameObject.Destroy(fazhen,2f);
	}
	
	public void gc()
	{
		if(cards!=null)
		{
			foreach(Card c in cards)
			{
				if(c!=null)
				{
					c.gc();
				}
			}
		}
		unitSkills=null;
		if(demoUnitSkillList!=null)
		{
			demoUnitSkillList.Clear();
			demoUnitSkillList=null;
		}
		bornCardJson=null;
		if(bornIndexList!=null)
		{
			bornIndexList.Clear();
			bornIndexList=null;
		}
		effectPrefab=null;
		friendAppearEffectPrefab=null;
		fazhenPrefab=null;
		//Resources.UnloadUnusedAssets();
	}
	
	public List<Card> getUnitSkillShowCardList()
	{
		List<Card> cardList = new List<Card>();
		for(int i = 0; i < unitSkillShowCards.Length;++i)
		{
			if(unitSkillShowCards[i] != null)
			{
				cardList.Add(unitSkillShowCards[i]);
			}
		}
		return cardList;
	}
	
	public bool isCanResumeAttackCard(int round)
	{
		bool b = false;
		for(int i = 0; i < cards.Length;++i)
		{
			Card card = cards[i];
			if(card != null && !card.isDeath() && card.isForbitAttack())
			{
				if(card.canResumeAttack(round))
				{
					b =  true;
				}
			}
		}
		return b;
	}
	
	public void playReduceEnergyEffect(int num)
	{
		needReduceEnergyNum = num;
		
	}
	
}

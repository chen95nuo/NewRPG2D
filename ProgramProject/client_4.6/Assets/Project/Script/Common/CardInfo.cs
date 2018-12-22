using UnityEngine;
using System.Collections;

public class CardInfo  {
	
	public int formId;				//卡牌在表中的id//
	public float lastMaxExp;		//卡牌升级前的经验值上限//
	public float lastExp;			//卡牌升级前的经验值//
	public float curExp;			//卡牌当前的经验值//
	public int lastLevel;			//卡牌战斗前等级//			
	public int level;				//卡牌战斗后等级//
	public float lastHp;			//卡牌升级前血量//
	public float Hp;				//卡牌升级后血量//
	public float lastAtk;			//卡牌升级前攻击力//
	public float attack;			//卡牌升级后攻击力//
	public float lastDef;			//卡牌升级前防御力//
	public float def;				//卡牌升级后防御力//
	public string cardName;			//卡牌对应的模型在res中的的名字//
	public int star;				//卡片的星级//
	
	public CardInfo(int id, float lastMaxE, float curMaxE, float lastE, float curE, int lastLv,
		int lv, float lastHp, float hp,float lastAtk, float atk, float lastDe, float de){
		this.formId = id;
		this.lastMaxExp = lastMaxE;
		this.lastExp = lastE;
		this.curExp = curE;
		this.lastLevel = lastLv;
		this.level = lv;
		this.lastHp = lastHp;
		this.Hp = hp;
		this.lastAtk = lastAtk;
		this.attack = atk;
		this.lastDef = lastDe;
		this.def = de;	
		this.cardName = CardData.getData(id).cardmodel;
		this.star = CardData.getData(id).star;
		if(lastMaxE <= 0){
			this.lastMaxExp = curMaxE;
		}
	}
	
	
	
	
}

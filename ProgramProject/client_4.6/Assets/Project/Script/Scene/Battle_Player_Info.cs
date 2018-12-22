using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// cuixl 该类用来存储战斗前后玩家（不是卡牌）的信息
/// Battle_ card_ info.
/// </summary>
public class Battle_Player_Info{
	public static Battle_Player_Info instance;
	public int BattleResult = -1; //-1战斗未结束， 2 失败， 1 胜利//
	public ArrayList cardList = new ArrayList();			//保存出战的卡牌的信息//
	public ArrayList battleReward = new ArrayList();		//战斗奖励,包括卡牌，材料，装备等//
	
	public int playerLastLevel;								//人物战斗前等级//
	public int playerCurLevel;								//人物当前等级//
	public float playerLastExp = 600;						//人物战斗前经验值//
	public float playerCurExp = 850;						//人物当前的经验值//
	public float playerLastMaxExp = 800;					//人物战斗前的经验值上限//
	public float playerCurMaxExp = 1000;					//人物升级后的经验值上限//
	public float playerLastPower = 0;						//人物升级后的体力值//
	public float playerCurPower = 0;						// 人物扣除战斗后的体力值//
	public int getCoins;									//战斗获得金币//
	public int WeaponMax ;									//所有装备强化等级提升后的上限//
	public int weaponLevelMax ;								//装备等级上限//
	public bool isUseBonus = false;							//是否是bonus//
	public float bonusAddNum = 3f;							//使用bonus后金币增加的倍数//



    public int hurt;                                        //活动副本战斗造成的总伤害//
	public float scaleExpTime = 2f;
	
	//pvp战斗信息//
	public string pkEnemyName;
	//当前排名//
	public int rank;
    //PK前排名//
    public int rank0;
	//获得的符文值//
	public int runeNum;

    //获得的符文值//
    public int sAward;

	//每次pk奖励的符文值//
	public int runeAward;
	//每次pk奖励的荣誉点//
	public int honorNum;
	//==支援玩家Id及名字==//
	public int helperPlayerId;
	public string helperPlayerName;
	

    //==玩家活动副本获得的金罡心数量==//
    public int diamond;

	public int errorCode = 0;
	
	private Battle_Player_Info(){
	}
	
	public static Battle_Player_Info GetInstance(){
		if(instance == null){
			instance = new Battle_Player_Info();
		}
		
		return instance;
	}
	
	
	public void addCardInfo(CardInfo ca){
		cardList.Add(ca);
	}
	
	public void addBattleDropItems(DropItemInfo drops){
		battleReward.Add(drops);
	}
	
	public void SetResponesData(string json){
		Debug.Log("Battle_Player_Info : json ^^^^^^^^^^^^^^^^^^^^^ == " + json);
		
		if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_NORMAL)
		{
			BattleLogResultJson blrj=JsonMapper.ToObject<BattleLogResultJson>(json);
			errorCode = blrj.errorCode;
			string[] str = blrj.s;
			PlayerInfo.getInstance().SetUnLockData(str);
			if(blrj.r == 1){		//胜利的时候才走//
			
				SetPlayerData(blrj.r, blrj.lv0, blrj.ce0, blrj.me0,
					blrj.lv1, blrj.ce1, blrj.me1, blrj.ds, blrj.ag, StringUtil.getFloat(blrj.gm));
				if(blrj.r==1)
				{
					GetInstance().SetBattleCardsData(blrj.cs0, blrj.cs1);
				}
				else
				{
					GetInstance().SetBattleCardsData(blrj.cs0, blrj.cs1);
				}
			}
			else {
				BattleResult = blrj.r;
			}
			helperPlayerId=blrj.hi;
			helperPlayerName=blrj.hn;
			playerLastPower = blrj.power0;
			playerCurPower = blrj.power1;
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_MAZE)
		{
			MazeBattleLogResultJson mblrj=JsonMapper.ToObject<MazeBattleLogResultJson>(json);
			errorCode = mblrj.errorCode;
			if(mblrj.r == 1){		//胜利的时候才走//
				
				SetPlayerData(mblrj.r, mblrj.lv0, mblrj.ce0, mblrj.me0,
					mblrj.lv1, mblrj.ce1, mblrj.me1, mblrj.ds, mblrj.ag, StringUtil.getFloat(mblrj.gm));
				if(mblrj.r==1)
				{
					GetInstance().SetBattleCardsData(mblrj.cs0, mblrj.cs1);
				}
				else
				{
					GetInstance().SetBattleCardsData(mblrj.cs0, mblrj.cs1);
				}
			}
			else {
				BattleResult = mblrj.r;
			}
			//保存迷宫信息//
			PlayerInfo.getInstance().mazeBattleType = mblrj.type;
			PlayerInfo.getInstance().curPosId = mblrj.state;
			PlayerInfo.getInstance().curMazeId = mblrj.map;
			playerLastPower = mblrj.power0;
			playerCurPower = mblrj.power1;
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_PVP)
		{
			PkBattleLogResultJson pblrj = JsonMapper.ToObject<PkBattleLogResultJson>(json);
			errorCode = pblrj.errorCode;
			BattleResult = pblrj.r;
			rank = pblrj.rank;
            rank0 = pblrj.rank0;
			pkEnemyName = pblrj.name;
			runeNum = pblrj.runeNum;
            sAward = pblrj.sAward;
            runeAward = pblrj.award;
			playerLastPower = pblrj.power0;
			playerCurPower = pblrj.power1;
			honorNum = pblrj.honor;
		}
		else if(PlayerInfo.getInstance().battleType == STATE.BATTLE_TYPE_EVENT)
		{
			EventBattleLogResultJson eblrj=JsonMapper.ToObject<EventBattleLogResultJson>(json);
			errorCode = eblrj.errorCode;

            this.diamond = eblrj.ad;
			if(eblrj.r == 1){		//胜利的时候才走//
				
				SetPlayerData(eblrj.r, eblrj.lv0, eblrj.ce0, eblrj.me0,
					eblrj.lv1, eblrj.ce1, eblrj.me1, eblrj.ds, eblrj.ag, StringUtil.getFloat("1"));
				if(eblrj.r==1)
				{
					GetInstance().SetBattleCardsData(eblrj.cs0, eblrj.cs1);
				}
				else
				{
					GetInstance().SetBattleCardsData(eblrj.cs0, eblrj.cs1);
				}
			}
			else {
				BattleResult = eblrj.r;
			}
			
			PlayerInfo.getInstance().copyId = eblrj.id;
			playerLastPower = eblrj.power0;
			playerCurPower = eblrj.power1;
		}
	}
	
	/**设置人物信息，result 战斗结果， lastLv 战斗前人物的等级， lastExp 战斗前人物的经验值， 
	 * lastMaxExp 战斗前人物经验值上限，curLv 战斗后人物的等级， curExp 战斗后人物的经验值， 
	 * curMaxExp当前的经验值上限， drops 掉落物品， addGold 过关奖励金币  **/
	private void SetPlayerData(int result, int lastLv, float lastExp, float lastMaxExp, int curLv, float curExp, float curMaxExp, List<string> drops, int addGold, float bounesNum)
	{
		this.BattleResult = result;
		this.playerLastLevel = lastLv;
		this.playerLastExp = lastExp;
		this.playerLastMaxExp = lastMaxExp;
		this.playerCurLevel = curLv;
		this.playerCurExp = curExp;
		this.playerCurMaxExp = curMaxExp;
		this.getCoins = addGold;
		this.bonusAddNum = bounesNum;
		battleReward.Clear();
		
		//测试start//
//		playerLastLevel = 16;
//		playerLastExp = 850;
//		playerCurLevel = 17;
//		playerCurExp = 80;
//		playerLastMaxExp = 948;-238.8
//		playerCurMaxExp = 1092;
//		playerCurExp += 100;
//		playerCurLevel += 1;
		//测试end//
		
		
		if(bounesNum > 1){
			isUseBonus = true;
		}
		else {
			isUseBonus = false;
		}
		//Debug.Log("bonusAddNum === " + bonusAddNum + "_" + "isUseBonus ==== " + isUseBonus);
		
		//截取字符串//
//		String[] ss=drops.split("-");
		for(int i = 0;result==1 && i < drops.Count;i++){
			string str = drops[i];
			string[] ss = str.Split('-');
			int type = StringUtil.getInt(ss[0]);
			int itemId = 0;
			int num = 1;
			int level = 1;

            string[] sss = ss[1].Split(',');
			
            switch (type)
			{
			case 1://item
				itemId=StringUtil.getInt(sss[0]);
				num = StringUtil.getInt(sss[1]);
				
				break;
			case 2://equip
                itemId = StringUtil.getInt(sss[0]);
				break;
			case 3://card
				itemId=StringUtil.getInt(ss[1].Split(',')[0]);
				level = StringUtil.getInt(sss[1]);
				break;
			case 4://skill
				itemId=StringUtil.getInt(ss[1].Split(',')[0]);
                
				break;
			case 5://passiveSkill
                itemId = StringUtil.getInt(sss[0]);
				break;
			}
			DropItemInfo dropItem = new DropItemInfo(type, itemId, num, level);
            for (int c = 0; c < num; c++)
            {
                addBattleDropItems(dropItem);
            }
                

		}
	}
	
	/**cards0 表示战斗前卡牌的信息， cards1 表示战斗后卡牌的信息**/
	//角色卡,格式:cardId-level-curExp-maxExp-hp-atk-def//
	private void SetBattleCardsData(List<string> cards0, List<string> cards1){
		//先清空//
		cardList.Clear();
		//Debug.Log("cards0.Length ================= " + cards0.Count);
		for(int i =0 ;i < cards0.Count;i++){
			string str0 = cards0[i];
			string str1 = cards1[i];
			string[] ss0 = str0.Split('-');
			string[] ss1 = str1.Split('-');
			int cardId = StringUtil.getInt(ss0[0]);
			int cardLastLv = StringUtil.getInt(ss0[1]);
			float lastE = StringUtil.getFloat(ss0[2]);
			float lastMaxE = StringUtil.getFloat(ss0[3]);
			float lastHp = StringUtil.getFloat(ss0[4]);
			float lastATK = StringUtil.getFloat(ss0[5]);
			float lastDEF = StringUtil.getFloat(ss0[6]);
			
			int cardCurLv = StringUtil.getInt(ss1[1]);
			float CurE = StringUtil.getFloat(ss1[2]);
			float CurMaxE = StringUtil.getFloat(ss1[3]);
			float CurHp = StringUtil.getFloat(ss1[4]);
			float CurATK = StringUtil.getFloat(ss1[5]);
			float CurDEF = StringUtil.getFloat(ss1[6]);
			//测试start//
//			lastMaxE = 693;
//			CurMaxE += lastMaxE;
//			lastE = 690;
//			CurE = 20;
//			cardLastLv = 5;
//			cardCurLv = 6;
//			CurE += lastMaxE;
//			cardCurLv ++;
			//测试end//
			CardInfo ca = new CardInfo(cardId, lastMaxE, CurMaxE, lastE, CurE, cardLastLv, cardCurLv, lastHp, CurHp, lastATK, CurATK, lastDEF, CurDEF);
			addCardInfo(ca);
		}
	}
	
	public void gc()
	{
		if(cardList!=null)
		{
			cardList.Clear();
		}
		if(battleReward!=null)
		{
			battleReward.Clear();
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfo
{
	private static PlayerInfo instance;
	
	public bool isLogout=false;
	
	/**流量**/
	private int sendByteNum;
	private int receiveByteNum;
	
	public PlayerElement player;//玩家信息//
	/**************************临时数据 start********************************************/
	public BattleResultJson brj;
	/**迷宫战斗的临时数据**/
	public MazeBattleResultJson mbrj;
	//pvp战斗//
	public PkBattleResultJson pbrj;
	//副本战斗//
	public EventBattleResultJson ebrj;
	// demo battle json //
	public NewPlayerBattleResultJson npbrj;
	
	
//	public bool battleOverBackMap = false;
	public int lastMissionId;
	public MapResultJson mrj;
	/**************************临时数据 end**********************************************/
	
	/**cuixl 迷宫相关数据start **/
	//当前战斗的类型1 pve， 2 迷宫中的战斗, 3 pvp, 4 副本(异世界)//
	public int battleType = -1;
	//迷宫编号//
	public int curMazeId;
	//棋子在当前迷宫中的位置//
	public int curPosId;
	//棋子在当前迷宫中上一个位置//
	public int prePosId;
	//迷宫中战斗的类型 1 普通战斗，2 Boss战//
	public int mazeBattleType = -1;
	//今天进入该迷宫的次数//
	public int curMazeNum;
	//是否扣除体力值，0 不扣，1 扣除//
	public int costPowerMark;
	public List<string> curOpenMazeId = new List<string>();
	public string curIntoMaze;
	//pvp掉落物品//
	public List<string> pvpDropList = new List<string> ();
	
	//战斗结束返回界面的类型 1 返回map界面， 2 返回迷宫界面， 3返回扭曲空间界面， 4 返回pvp（竞技场界面） 5 返回副本选关卡界面//
	public int BattleOverBackType;

    public bool isEvent; 
	//返回到活动副本选关界面时，副本的id//
	public int copyId;


    public int hurt;                                        //活动副本战斗造成的总伤害//
    //返回到战斗中的活动界面//
    public bool bBackActivity;
	
	public bool isShowConnectObj;
	
	//当前合体技的个数//
	public int uniteSkillNum = 2;
	
	//当前背景音乐的音量//
	public float musicBgVolume = 1;
	public float soundEffVolume = 1;
	
	//存储各个模块的解锁情况//
	private static Hashtable UnLockDataTabel = new Hashtable();
	//==最后一次请求信息==//
	private BasicJson lastRequestBj;
	private ProcessResponse lastRequestPr;
	//==超时标识==//
	public bool timeout;
    //记录当前玩家是否选择自动战斗模式//
    public bool isAutomaticRelease;
	//记录当前玩家选择的时间流速//
	public float curTimeScale = STATE.SPEED_BATTLE_NORMAL;
	//当前的卡组信息//
	public CardGroup curCardGroup;

    public bool mainIconShow = true;   //true展开[默认] false折叠
	
	public bool gcLogoHadShow = false;
	
	public static bool isFirstLogin = false;
	
	public Dictionary<int,int> curMazeBattleCardHp = new Dictionary<int,int>();//记录当前迷宫当前卡组当前血量//
	
	public Dictionary<int,int> maxMazeBattleCardHp = new Dictionary<int,int>();//记录当前迷宫当前卡组满血量//
	
	public Hashtable MazeBattleHpScale = new Hashtable();
	
	public static PlayerInfo getInstance()
	{
		if(instance==null)
		{
			instance=new PlayerInfo();
		}
		return instance;
	}
	
	private PlayerInfo(){}
	
	/**记录流量信息,type:1发送,2接收**/
	public void recordFlowInfo(int type,string msg)
	{
		if(type==1)
		{
			sendByteNum+=msg.Length;
		}
		else
		{
			receiveByteNum+=msg.Length;
		}
		//Debug.Log("sendByteNum:"+sendByteNum);
		//Debug.Log("receiveByteNum:"+receiveByteNum);
	}
	
	public bool reSendRequest()
	{
		return sendRequest(lastRequestBj,lastRequestPr);
	}
	
	public void CleanRequest()
	{
		lastRequestPr = null;
	}
	
	/**发送请求**/
	public bool sendRequest(BasicJson bj,ProcessResponse pr)
	{
		isShowConnectObj = true;
		if(bj==null || pr==null)
		{
			return false;
		}
		bool result=false;
		bj.playerId=player.id;
		//==记录最后一次请求==//
		lastRequestBj=bj;
		lastRequestPr=pr;
		//==发送请求==//
		if(bj is SaleJson)
		{
			result=RequestSender.getInstance().request(6,JsonMapper.ToJson((SaleJson)bj),true,pr);
		}
		else if(bj is IntensifyJson)
		{
			result=RequestSender.getInstance().request(8,JsonMapper.ToJson((IntensifyJson)bj),true,pr);
		}
		else if(bj is BattleJson)
		{
			result=RequestSender.getInstance().request(7,JsonMapper.ToJson((BattleJson)bj),true,pr);
		}
		else if(bj is BattleLogJson)
		{
			BattleLogJson blj=(BattleLogJson)bj;
			result=RequestSender.getInstance().request(5,JsonMapper.ToJson(blj),true,pr);
		}
		else if(bj is UIJson)
		{
			UIJson uj=(UIJson)bj;
			result=RequestSender.getInstance().request(11,JsonMapper.ToJson(uj),true,pr);
		}
		else if(bj is SaveCGJson)
		{
			SaveCGJson scgj=(SaveCGJson)bj;
			result=RequestSender.getInstance().request(10,JsonMapper.ToJson(scgj),true,pr);
		}
		else if(bj is LotJson)
		{
			LotJson lj=(LotJson)bj;
			result=RequestSender.getInstance().request(13,JsonMapper.ToJson(lj),true,pr);
		}
		else if(bj is ComposeJson)
		{
			ComposeJson lj = (ComposeJson)bj;
			result=RequestSender.getInstance().request(14,JsonMapper.ToJson(lj),true,pr);
		}
		// -- cxl -- 迷宫战斗数据//
		else if(bj is MazeBattleJson){
			MazeBattleJson mbj = (MazeBattleJson)bj;
			result=RequestSender.getInstance().request(15,JsonMapper.ToJson(mbj),true,pr);
		}
		//迷宫中战报信息//
		else if(bj is MazeBattleLogJson)
		{
			MazeBattleLogJson mblj=(MazeBattleLogJson)bj;
			result=RequestSender.getInstance().request(16,JsonMapper.ToJson(mblj),true,pr);
		}
		else if(bj is RuneJson)
		{
			RuneJson rj=(RuneJson)bj;
			result=RequestSender.getInstance().request(17,JsonMapper.ToJson(rj),true,pr);
		}
		//-- cuixl 灵界（冥想） 一键清理--//
		else if(bj is ImaginationClearJson){
			ImaginationClearJson icj = (ImaginationClearJson)bj;
			result=RequestSender.getInstance().request(18,JsonMapper.ToJson(icj),true,pr);
		}
		//-- cuixl 灵界（冥想） 冥想（点击按钮）--//
		else if(bj is ImaginationClickJson){
			ImaginationClickJson iclj = (ImaginationClickJson)bj;
			result=RequestSender.getInstance().request(19,JsonMapper.ToJson(iclj),true,pr);
		}
		//-- cxl 兑换物品--//
		else if(bj is ImaginationComposeJson){
			ImaginationComposeJson icomj = (ImaginationComposeJson)bj;
			result=RequestSender.getInstance().request(20,JsonMapper.ToJson(icomj),true,pr);
		}
		else if(bj is FriendJson)
		{
			FriendJson fj=(FriendJson)bj;
			result=RequestSender.getInstance().request(21,JsonMapper.ToJson(fj),true,pr);
		}
		else if(bj is FriendRemoveJson)
		{
			FriendRemoveJson rfj=(FriendRemoveJson)bj;
			result=RequestSender.getInstance().request(22,JsonMapper.ToJson(rfj),true,pr);
		}
		else if(bj is FriendRefreshJson)
		{
			FriendRefreshJson rfj=(FriendRefreshJson)bj;
			result=RequestSender.getInstance().request(23,JsonMapper.ToJson(rfj),true,pr);
		}
		else if(bj is FriendApplyJson)
		{
			FriendApplyJson faj=(FriendApplyJson)bj;
			result=RequestSender.getInstance().request(24,JsonMapper.ToJson(faj),true,pr);
		}
		else if(bj is FriendMyApplyJson)
		{
			FriendMyApplyJson fmj=(FriendMyApplyJson)bj;
			result=RequestSender.getInstance().request(25,JsonMapper.ToJson(fmj),true,pr);
		}
		else if(bj is FriendCancelJson)
		{
			FriendCancelJson fcj=(FriendCancelJson)bj;
			result=RequestSender.getInstance().request(26,JsonMapper.ToJson(fcj),true,pr);
		}
		else if(bj is FriendProcessApplyJson)
		{
			FriendProcessApplyJson fpj=(FriendProcessApplyJson)bj;
			result=RequestSender.getInstance().request(28,JsonMapper.ToJson(fpj),true,pr);
		}
		else if(bj is FriendSearchJson)
		{
			FriendSearchJson fsj=(FriendSearchJson)bj;
			result=RequestSender.getInstance().request(27,JsonMapper.ToJson(fsj),true,pr);
		}	
		//-- cxl 竞技场界面信息--//
		else if(bj is RankJson)
		{
			RankJson rj=(RankJson)bj;
			result=RequestSender.getInstance().request(29,JsonMapper.ToJson(rj),true,pr);
		}
		//-- cxl 竞技场领取排行奖励--//
		else if(bj is ReceiveAwardJson)
		{
			ReceiveAwardJson raj=(ReceiveAwardJson)bj;
			result=RequestSender.getInstance().request(30,JsonMapper.ToJson(raj),true,pr);
		}
		//-- cxl 竞技场pk请求--//
		else if(bj is PkBattleJson)
		{
			PkBattleJson pbj=(PkBattleJson)bj;
			result=RequestSender.getInstance().request(31,JsonMapper.ToJson(pbj),true,pr);
		}
		//-- cxl pvp战报请求--//
		else if(bj is PkBattleLogJson)
		{
			PkBattleLogJson pblj=(PkBattleLogJson)bj;
			result=RequestSender.getInstance().request(32,JsonMapper.ToJson(pblj),true,pr);
		}
		//-- cxl 活动副本战斗请求--//
		else if(bj is EventBattleJson)
		{
			EventBattleJson ebj=(EventBattleJson)bj;
			result=RequestSender.getInstance().request(34,JsonMapper.ToJson(ebj),true,pr);
		}
		else if(bj is EventBattleLogJson){
			EventBattleLogJson eblj=(EventBattleLogJson)bj;
			result=RequestSender.getInstance().request(35,JsonMapper.ToJson(eblj),true,pr);
		}
		else if(bj is AchieveRewardJson)
		{
			AchieveRewardJson arj=(AchieveRewardJson)bj;
			result=RequestSender.getInstance().request(36,JsonMapper.ToJson(arj),true,pr);
		}
		else if(bj is Star3RewardJson)
		{
			Star3RewardJson s3rj=(Star3RewardJson)bj;
			result=RequestSender.getInstance().request(37,JsonMapper.ToJson(s3rj),true,pr);
		}
		else if(bj is SignJson)
		{
			SignJson sj=(SignJson)bj;
			result=RequestSender.getInstance().request(38,JsonMapper.ToJson(sj),true,pr);
		}
		else if(bj is BreakJson)
		{
			BreakJson sj=(BreakJson)bj;
			result=RequestSender.getInstance().request(39,JsonMapper.ToJson(sj),true,pr);
		}
		else if(bj is EnergyJson)
		{
			EnergyJson ej=(EnergyJson)bj;
			result=RequestSender.getInstance().request(40,JsonMapper.ToJson(ej),true,pr);
		}
		else if(bj is MailJson)
		{
			MailJson mj=(MailJson)bj;
			result=RequestSender.getInstance().request(41,JsonMapper.ToJson(mj),true,pr);
		}
		else if(bj is BuyPowerOrGoldJson)
		{
			BuyPowerOrGoldJson bpgj=(BuyPowerOrGoldJson)bj;
			result=RequestSender.getInstance().request(42,JsonMapper.ToJson(bpgj),true,pr);
		}
		else if(bj is SweepUiJson)			//扫荡界面请求//
		{
			SweepUiJson suj = (SweepUiJson)bj;
			result=RequestSender.getInstance().request(43,JsonMapper.ToJson(suj),true,pr);
		}
		else if(bj is SweepJson)			//开始扫荡//				
		{
			SweepJson sj = (SweepJson)bj;
			result=RequestSender.getInstance().request(44,JsonMapper.ToJson(sj),true,pr);
		}
		else if(bj is PkRankJson)
		{
			PkRankJson prj = (PkRankJson)bj;
			result=RequestSender.getInstance().request(45,JsonMapper.ToJson(prj),true,pr);
		}
		else if(bj is ActivityJson)
		{
			ActivityJson aj = (ActivityJson)bj;
			result = RequestSender.getInstance().request(46,JsonMapper.ToJson(aj),true,pr);
		}
		else if(bj is TaskJson)
		{
			TaskJson tj = (TaskJson)bj;
			result = RequestSender.getInstance().request(47,JsonMapper.ToJson(tj),true,pr);
		}
		else if(bj is ActivityRewardJson)
		{
			ActivityRewardJson actrj = (ActivityRewardJson)bj;
			result = RequestSender.getInstance().request(48,JsonMapper.ToJson(actrj),true,pr);
		}
		else if(bj is GiftCodeJson)
		{
			GiftCodeJson gcj = (GiftCodeJson)bj;
			result = RequestSender.getInstance().request(49,JsonMapper.ToJson(gcj),true,pr);
		}
		else if(bj is BuyFriendJson)
		{
			BuyFriendJson bfj=(BuyFriendJson)bj;
			result=RequestSender.getInstance().request(51,JsonMapper.ToJson(bfj),true,pr);
		}
		else if(bj is BuyBagJson)
		{
			BuyBagJson bfj=(BuyBagJson)bj;
			result=RequestSender.getInstance().request(52,JsonMapper.ToJson(bfj),true,pr);
		}
		else if(bj is ActivityInfoJson)
		{
			ActivityInfoJson aij=(ActivityInfoJson)bj;
			result=RequestSender.getInstance().request(53,JsonMapper.ToJson(aij),true,pr);
		}
		else if(bj is ExchangeJson)
		{
			ExchangeJson erj=(ExchangeJson)bj;
			result=RequestSender.getInstance().request(55,JsonMapper.ToJson(erj),true,pr);
		}
		else if(bj is PkRecordJson)//pk历史记录//
		{
			PkRecordJson pkrj = (PkRecordJson)bj;
			result = RequestSender.getInstance().request(56,JsonMapper.ToJson(pkrj),true,pr);
		}
		else if(bj is PayOrderJson)//获取支付订单号//
		{
			PayOrderJson poj = (PayOrderJson)bj;
			result = RequestSender.getInstance().request(57,JsonMapper.ToJson(poj),true,pr);
		}
		else if(bj is GameBoxJson)		//开宝箱//
		{
			GameBoxJson gbj = (GameBoxJson)bj;
			result = RequestSender.getInstance().request(58,JsonMapper.ToJson(gbj),true,pr);
		}
		else if(bj is AnnounceJson)		//请求跑马灯内容//
		{
			isShowConnectObj = false;
			AnnounceJson anj = (AnnounceJson)bj;
			result = RequestSender.getInstance().request(59,JsonMapper.ToJson(anj),pr);
		}
		else if(bj is ActivityGJson)		//请求活动公告列表//
		{
			ActivityGJson agj = (ActivityGJson)bj;
			result = RequestSender.getInstance().request(62,JsonMapper.ToJson(agj),true,pr);
		}
		else if(bj is ActiveJson)		//请求活动公告列表//
		{
			ActiveJson aj = (ActiveJson)bj;
			result = RequestSender.getInstance().request(63,JsonMapper.ToJson(aj),true,pr);
		}
        else if (bj is GooglePayJson)      //google 支付//
        {
            GooglePayJson aj = (GooglePayJson)bj;
            result = RequestSender.getInstance().requestGooglePay(JsonMapper.ToJson(aj), pr);
        }
        return result;
	}

    //请求非背包时使用//
    public static List<PackElement> getCells(int type, int curPopup1, int curPopup2, int curPopup3, List<PackElement> elements)
    {
        List<PackElement> result = new List<PackElement>();
        if (elements == null || elements.Count == 0)
        {
            return result;
        }
        if (curPopup1 == 0 && curPopup2 == 0 && curPopup3 == 0)
        {
            result.AddRange(elements);
            return result;
        }
        //筛选//
        switch (type)
        {
            case 1:/**角色卡**/
                for (int i = 0; i < elements.Count; i++)
                {
                    PackElement pe = elements[i];
                    CardData cd = CardData.getData(pe.dataId);
                    if (cd == null)
                    {
                        continue;
                    }
                    /**星级**/
                    if (curPopup2 > 0 && cd.star != curPopup2)
                    {
                        continue;
                    }
                    /**种族**/
                    if (curPopup3 > 0 && cd.race != curPopup3)
                    {
                        continue;
                    }
                    /**排序**/
                    switch (curPopup1)
                    {
						case 0:
							if(!result.Contains(pe))
							{
								result.Add(pe);
							}
							break;
                        case 1:/**按获得先后顺序**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (pe.ct.CompareTo(temp.ct) < 0)
                                {
                                    result.Insert(k, pe);
                                    break;
                                }
                            }
                            if (!result.Contains(pe))
                            {
                                result.Add(pe);
                            }
                            break;
                        case 2:/**强化等级从高到低**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (pe.lv > temp.lv)
                                {
                                    result.Insert(k, pe);
                                    break;
                                }
                            }
                            if (!result.Contains(pe))
                            {
                                result.Add(pe);
                            }
                            break;
                        case 3:/**强化等级从低到高**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (pe.lv < temp.lv)
                                {
                                    result.Insert(k, pe);
                                    break;
                                }
                            }
                            if (!result.Contains(pe))
                            {
                                result.Add(pe);
                            }
                            break;
                        case 4:/**攻击力从高到低**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (pe.getSelfAtk() > temp.getSelfAtk())
                                {
                                    result.Insert(k, pe);
                                    break;
                                }
                            }
                            if (!result.Contains(pe))
                            {
                                result.Add(pe);
                            }
                            break;
                        case 5:/**攻击力从低到高**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (pe.getSelfAtk() < temp.getSelfAtk())
                                {
                                    result.Insert(k, pe);
                                    break;
                                }
                            }
                            if (!result.Contains(pe))
                            {
                                result.Add(pe);
                            }
                            break;
                        case 6:/**防御力从高到低**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (pe.getSelfDef() > temp.getSelfDef())
                                {
                                    result.Insert(k, pe);
                                    break;
                                }
                            }
                            if (!result.Contains(pe))
                            {
                                result.Add(pe);
                            }
                            break;
                        case 7:/**防御力从低到高**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (pe.getSelfDef() < temp.getSelfDef())
                                {
                                    result.Insert(k, pe);
                                    break;
                                }
                            }
                            if (!result.Contains(pe))
                            {
                                result.Add(pe);
                            }
                            break;
                        case 8:/**生命值从高到低**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (pe.getSelfHp() > temp.getSelfHp())
                                {
                                    result.Insert(k, pe);
                                    break;
                                }
                            }
                            if (!result.Contains(pe))
                            {
                                result.Add(pe);
                            }
                            break;
                        case 9:/**生命值从低到高**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (pe.getSelfHp() < temp.getSelfHp())
                                {
                                    result.Insert(k, pe);
                                    break;
                                }
                            }
                            if (!result.Contains(pe))
                            {
                                result.Add(pe);
                            }
                            break;
                    }
                }
                break;
            case 2:/**技能卡**/
                for (int i = 0; i < elements.Count; i++)
                {
                    PackElement dbs = elements[i];
                    SkillData sd = SkillData.getData(dbs.dataId);
                    if (sd == null)
                    {
                        continue;
                    }
                    /**星级**/
                    if (curPopup2 > 0 && sd.star != curPopup2)
                    {
                        continue;
                    }
                    /**类型**/
                    if (curPopup3 > 0 && sd.type != curPopup3)
                    {
                        continue;
                    }
                    switch (curPopup1)
                    {
						case 0:
							if(!result.Contains(dbs))
							{
								result.Add(dbs);
							}
							break;
                        case 1:/**按获得先后顺序**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (dbs.ct.CompareTo(temp.ct) < 0)
                                {
                                    result.Insert(k, dbs);
                                    break;
                                }
                            }
                            if (!result.Contains(dbs))
                            {
                                result.Add(dbs);
                            }
                            break;
                        case 2:/**强化等级从高到低**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                SkillData sdTemp = SkillData.getData(temp.dataId);
                                if (sd.level > sdTemp.level)
                                {
                                    result.Insert(k, dbs);
                                    break;
                                }
                            }
                            if (!result.Contains(dbs))
                            {
                                result.Add(dbs);
                            }
                            break;
                        case 3:/**强化等级从低到高**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                SkillData sdTemp = SkillData.getData(temp.dataId);
                                if (sd.level < sdTemp.level)
                                {
                                    result.Insert(k, dbs);
                                    break;
                                }
                            }
                            if (!result.Contains(dbs))
                            {
                                result.Add(dbs);
                            }
                            break;
                        case 4:/**星级从高到低**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                SkillData sdTemp = SkillData.getData(temp.dataId);
                                if (sd.star > sdTemp.star)
                                {
                                    result.Insert(k, dbs);
                                    break;
                                }
                            }
                            if (!result.Contains(dbs))
                            {
                                result.Add(dbs);
                            }
                            break;
                        case 5:/**星级从低到高**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                SkillData sdTemp = SkillData.getData(temp.dataId);
                                if (sd.star < sdTemp.star)
                                {
                                    result.Insert(k, dbs);
                                    break;
                                }
                            }
                            if (!result.Contains(dbs))
                            {
                                result.Add(dbs);
                            }
                            break;
                    }
                }
                break;
            case 8://被动技能//
                for (int i = 0; i < elements.Count; i++)
                {
                    PackElement dbp = elements[i];
                    PassiveSkillData psd = PassiveSkillData.getData(dbp.dataId);
                    if (psd == null)
                    {
                        continue;
                    }
                    /**星级**/
                    if (curPopup2 > 0 && psd.star != curPopup2)
                    {
                        continue;
                    }
                    /**类型**/
                    if (curPopup3 > 0 && curPopup3 != 4)
                    {
                        continue;
                    }
                    switch (curPopup1)
                    {
						case 0:
							if(!result.Contains(dbp))
							{
								result.Add(dbp);
							}
							break;
                        case 1:/**按获得先后顺序**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (dbp.ct.CompareTo(temp.ct) < 0)
                                {
                                    result.Insert(k, dbp);
                                    break;
                                }
                            }
                            if (!result.Contains(dbp))
                            {
                                result.Add(dbp);
                            }
                            break;
                        case 2:/**强化等级从高到低**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                PassiveSkillData psdTemp = PassiveSkillData.getData(temp.dataId);
                                if (psd.level > psdTemp.level)
                                {
                                    result.Insert(k, dbp);
                                    break;
                                }
                            }
                            if (!result.Contains(dbp))
                            {
                                result.Add(dbp);
                            }
                            break;
                        case 3:/**强化等级从低到高**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                PassiveSkillData psdTemp = PassiveSkillData.getData(temp.dataId);
                                if (psd.level < psdTemp.level)
                                {
                                    result.Insert(k, dbp);
                                    break;
                                }
                            }
                            if (!result.Contains(dbp))
                            {
                                result.Add(dbp);
                            }
                            break;
                        case 4:/**星级从高到低**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                PassiveSkillData psdTemp = PassiveSkillData.getData(temp.dataId);
                                if (psd.star > psdTemp.star)
                                {
                                    result.Insert(k, dbp);
                                    break;
                                }
                            }
                            if (!result.Contains(dbp))
                            {
                                result.Add(dbp);
                            }
                            break;
                        case 5:/**星级从低到高**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                PassiveSkillData psdTemp = PassiveSkillData.getData(temp.dataId);
                                if (psd.star < psdTemp.star)
                                {
                                    result.Insert(k, dbp);
                                    break;
                                }
                            }
                            if (!result.Contains(dbp))
                            {
                                result.Add(dbp);
                            }
                            break;
                    }
                }
                break;
            case 3:/**装备卡**/
                for (int i = 0; i < elements.Count; i++)
                {
                    PackElement dbe = elements[i];
                    EquipData ed = EquipData.getData(dbe.dataId);
                    if (ed == null)
                    {
                        continue;
                    }
                    if (curPopup2 > 0 && curPopup2 != ed.star)
                    {
                        continue;
                    }
                    if (curPopup3 > 0 && curPopup3 != ed.type)
                    {
                        continue;
                    }
                    switch (curPopup1)
                    {
						case 0:
							if(!result.Contains(dbe))
							{
								result.Add(dbe);
							}
							break;
                        case 1:/**按获得先后顺序**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (dbe.ct.CompareTo(temp.ct) < 0)
                                {
                                    result.Insert(k, dbe);
                                    break;
                                }
                            }
                            if (!result.Contains(dbe))
                            {
                                result.Add(dbe);
                            }
                            break;
                        case 2:/**强化等级从高到低**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (dbe.lv > temp.lv)
                                {
                                    result.Insert(k, dbe);
                                    break;
                                }
                            }
                            if (!result.Contains(dbe))
                            {
                                result.Add(dbe);
                            }
                            break;
                        case 3:/**强化等级从低到高**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                if (dbe.lv < temp.lv)
                                {
                                    result.Insert(k, dbe);
                                    break;
                                }
                            }
                            if (!result.Contains(dbe))
                            {
                                result.Add(dbe);
                            }
                            break;
                        case 4:/**星级从高到低**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                EquipData edTemp = EquipData.getData(temp.dataId);
                                if (ed.star > edTemp.star)
                                {
                                    result.Insert(k, dbe);
                                    break;
                                }
                            }
                            if (!result.Contains(dbe))
                            {
                                result.Add(dbe);
                            }
                            break;
                        case 5:/**星级从低到高**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                EquipData edTemp = EquipData.getData(temp.dataId);
                                if (ed.star < edTemp.star)
                                {
                                    result.Insert(k, dbe);
                                    break;
                                }
                            }
                            if (!result.Contains(dbe))
                            {
                                result.Add(dbe);
                            }
                            break;
                    }
                }
                break;
            case 4:/**材料卡**/
                for (int i = 0; i < elements.Count; i++)
                {
                    PackElement dbi = elements[i];
                    ItemsData iData = ItemsData.getData(dbi.dataId);
                    if (iData == null)
                    {
                        continue;
                    }
                    /**星级**/
                    if (curPopup2 > 0 && curPopup2 != iData.star)
                    {
                        continue;
                    }
                    /**类型**/
                    if (curPopup3 > 0 && curPopup3 != iData.type)
                    {
                        continue;
                    }
                    switch (curPopup1)
                    {
						case 0:
							if(!result.Contains(dbi))
							{
								result.Add(dbi);
							}
							break;
                        case 1:/**星级从高到低**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                ItemsData iTemp = ItemsData.getData(temp.dataId);
                                if (iData.star > iTemp.star)
                                {
                                    result.Insert(k, dbi);
                                    break;
                                }
                            }
                            if (!result.Contains(dbi))
                            {
                                result.Add(dbi);
                            }
                            break;
                        case 2:/**星级从低到高**/
                            for (int k = 0; k < result.Count; k++)
                            {
                                PackElement temp = result[k];
                                ItemsData iTemp = ItemsData.getData(temp.dataId);
                                if (iData.star < iTemp.star)
                                {
                                    result.Insert(k, dbi);
                                    break;
                                }
                            }
                            if (!result.Contains(dbi))
                            {
                                result.Add(dbi);
                            }
                            break;
                    }
                }
                break;
        }
        return result;
    }
	/**选取Skill卡,type卡类型,curPopup1排序条件,curPopup2筛选条件,curPopup3筛选条件**/
	public static List<PackElement> getGridSkills(int curPopup1,int curPopup2,int curPopup3,List<PackElement> skills)
	{
		List<PackElement> result=new List<PackElement>();
		if(skills!=null && skills.Count>0)
		{
			for(int i=0;i<skills.Count;i++)
			{
				PackElement dbs=skills[i];
				SkillData sd=SkillData.getData(dbs.dataId);
				if(sd==null)
				{
					continue;
				}
				/**星级**/
				if(curPopup2>0 && sd.star!=curPopup2)
				{
					continue;
				}
				/**类型**/
				if(curPopup3>0 && sd.type!=curPopup3)
				{
					continue;
				}
				switch(curPopup1)
				{
				case 1:/**按获得先后顺序**/
					for(int k=0;k<result.Count;k++)
					{
						PackElement temp=result[k];
						if(dbs.ct.CompareTo(temp.ct)<0)
						{
							result.Insert(k,dbs);
							break;
						}
					}
					if(!result.Contains(dbs))
					{
						result.Add(dbs);
					}
					break;
				case 2:/**强化等级从高到低**/
					for(int k=0;k<result.Count;k++)
					{
						PackElement temp=result[k];
						SkillData sdTemp=SkillData.getData(temp.dataId);
						if(sd.level>sdTemp.level)
						{
							result.Insert(k,dbs);
							break;
						}
					}
					if(!result.Contains(dbs))
					{
						result.Add(dbs);
					}
					break;
				case 3:/**强化等级从低到高**/
					for(int k=0;k<result.Count;k++)
					{
						PackElement temp=result[k];
						SkillData sdTemp=SkillData.getData(temp.dataId);
						if(sd.level<sdTemp.level)
						{
							result.Insert(k,dbs);
							break;
						}
					}
					if(!result.Contains(dbs))
					{
						result.Add(dbs);
					}
					break;
				case 4:/**星级从高到低**/
					for(int k=0;k<result.Count;k++)
					{
						PackElement temp=result[k];
						SkillData sdTemp=SkillData.getData(temp.dataId);
						if(sd.star>sdTemp.star)
						{
							result.Insert(k,dbs);
							break;
						}
					}
					if(!result.Contains(dbs))
					{
						result.Add(dbs);
					}
					break;
				case 5:/**星级从低到高**/
					for(int k=0;k<result.Count;k++)
					{
						PackElement temp=result[k];
						SkillData sdTemp=SkillData.getData(temp.dataId);
						if(sd.star<sdTemp.star)
						{
							result.Insert(k,dbs);
							break;
						}
					}
					if(!result.Contains(dbs))
					{
						result.Add(dbs);
					}
					break;
				}
			}
		}
		return result;
	}
	
	/**选取PassiveSkill卡,type卡类型,curPopup1排序条件,curPopup2筛选条件,curPopup3筛选条件**/
	public static List<PackElement> getGridPassiveSkills(int curPopup1,int curPopup2,int curPopup3,List<PackElement> passiveSkills)
	{
		List<PackElement> result=new List<PackElement>();
		if(passiveSkills!=null && passiveSkills.Count>0)
		{
			for(int i=0;i<passiveSkills.Count;i++)
			{
				PackElement dbp=passiveSkills[i];
				PassiveSkillData psd=PassiveSkillData.getData(dbp.dataId);
				if(psd==null)
				{
					continue;
				}
				/**星级**/
				if(curPopup2>0 && psd.star!=curPopup2)
				{
					continue;
				}
				/**类型**/
				if(curPopup3>0 && curPopup3!=4)
				{
					continue;
				}
				switch(curPopup1)
				{
				case 1:/**按获得先后顺序**/
					for(int k=0;k<result.Count;k++)
					{
						PackElement temp=result[k];
						if(dbp.ct.CompareTo(temp.ct)<0)
						{
							result.Insert(k,dbp);
							break;
						}
					}
					if(!result.Contains(dbp))
					{
						result.Add(dbp);
					}
					break;
				case 2:/**强化等级从高到低**/
					for(int k=0;k<result.Count;k++)
					{
						PackElement temp=result[k];
						PassiveSkillData psdTemp=PassiveSkillData.getData(temp.dataId);
						if(psd.level>psdTemp.level)
						{
							result.Insert(k,dbp);
							break;
						}
					}
					if(!result.Contains(dbp))
					{
						result.Add(dbp);
					}
					break;
				case 3:/**强化等级从低到高**/
					for(int k=0;k<result.Count;k++)
					{
						PackElement temp=result[k];
						PassiveSkillData psdTemp=PassiveSkillData.getData(temp.dataId);
						if(psd.level<psdTemp.level)
						{
							result.Insert(k,dbp);
							break;
						}
					}
					if(!result.Contains(dbp))
					{
						result.Add(dbp);
					}
					break;
				case 4:/**星级从高到低**/
					for(int k=0;k<result.Count;k++)
					{
						PackElement temp=result[k];
						PassiveSkillData psdTemp=PassiveSkillData.getData(temp.dataId);
						if(psd.star>psdTemp.star)
						{
							result.Insert(k,dbp);
							break;
						}
					}
					if(!result.Contains(dbp))
					{
						result.Add(dbp);
					}
					break;
				case 5:/**星级从低到高**/
					for(int k=0;k<result.Count;k++)
					{
						PackElement temp=result[k];
						PassiveSkillData psdTemp=PassiveSkillData.getData(temp.dataId);
						if(psd.star<psdTemp.star)
						{
							result.Insert(k,dbp);
							break;
						}
					}
					if(!result.Contains(dbp))
					{
						result.Add(dbp);
					}
					break;
				}
			}
		}
		return result;
	}

	/**只按照number的大小进行排序，number小的放在上面**/
	/**排序:星级降序排列,如果同星级有已装备过的,那么已装备过的显示在上边**/
	private static List<PackElement> getSortedEquipsByType(int type,CardGroup cg,int curCardId,List<PackElement> equips)
	{
		if(equips!=null && cg!=null)
		{
			/**按照number排序**/
			List<PackElement> temp=new List<PackElement>();
			for( int i = 0; i < equips.Count;++i)
			{
				EquipData ed = EquipData.getData(equips[i].dataId);
				if(ed == null)
					continue;
				for(int j=0;j<temp.Count;++j)
				{
					EquipData edTemp=EquipData.getData(temp[j].dataId);
					if(ed.number<edTemp.number)
					{
						temp.Insert(j,equips[i]);
						break;
					}
				}
				if(!temp.Contains(equips[i]))
				{
					temp.Add(equips[i]);
				}
			}
			List<PackElement> used = new List<PackElement>();
			//为已使用的进行排序,并将其放在最前面//
			for(int i = 0; i<temp.Count;)
			{
				PackElement epe = temp[i];
				if(epe == null)
					continue;
				EquipData ed = EquipData.getData(epe.dataId);
				if(ed == null)
					continue;
				int equipIndex=getEquipdedIndex(epe,cg);
				
				if(equipIndex<6 && equipIndex == curCardId)
				{
					for(int j=0;j<used.Count;++j)
					{
						EquipData edTemp=EquipData.getData(used[j].dataId);
						if(ed.number<edTemp.number)
						{
							used.Insert(j,epe);
							break;
						}
					}
					if(!used.Contains(epe))
					{
						used.Add(epe);
					}
					temp.Remove(epe);
				}
				else 
				{
					i++;
				}
			}
			//将已使用的开拍放在list的最前面//
			for(int i =0;i< used.Count;i++)
			{
				temp.Insert(i,used[i]);
			}
			used.Clear();
			
			/**获取指定类型装备**/
			List<PackElement> result=new List<PackElement>();
			bool needFindGuideEquip = false;
			if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Equip) || GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyEquip))
			{
				for(int i = 0; i < temp.Count;++i)
				{
					PackElement equip=temp[i];
					EquipData ed=EquipData.getData(equip.dataId);
					if(ed!=null && ed.type==type)
					{
						if(equip.dataId == 1101)
						{
							result.Add(equip);
							needFindGuideEquip = true;
							break;
						}
					}
				}
			}
		
			for(int i=0;i<temp.Count;i++)
			{
				PackElement equip=temp[i];
				EquipData ed=EquipData.getData(equip.dataId);
				if(ed!=null && ed.type==type)
				{
					if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Equip) || GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_IntensifyEquip))
					{
						if(equip.dataId == 1101 && needFindGuideEquip)
						{
							needFindGuideEquip = false;
							continue;
						}
					}
					result.Add(equip);
				}
			}
			return result;
		}
		return null;
	}
	
	/**获取一件装备装在卡组的第几个位置上,返回0--6,6表示没有装上**/
	private static int getEquipdedIndex(PackElement dbe,CardGroup cg)
	{
		for(int k=0;k<cg.equips.Length;k++)
		{
			List<PackElement> equiped=cg.equips[k];
			if(equiped!=null)
			{
				foreach(PackElement pe in  equiped)
				{
					if(pe.i==dbe.i)
					{
						return k;
					}
				}
			}
		}
		return cg.equips.Length;
	}
	
	/**返回结果格式:装备在链表中的ID-标记-是否使用,其中标记表示:1使用中,2可使用,3可激活,4不可激活**/
	public static List<string> getEquipedStrList(int type, CardGroup cg, int curCardId,List<PackElement> equips)
	{
		List<PackElement> equipList = getSortedEquipsByType(type, cg,curCardId,equips);
			
		List<string> tempStrList = new List<string>();
		for(int i = 0;i < equipList.Count;i++){
			PackElement dbe = equipList[i];
			//获取当前装备在playerInfo中的list的位置//
			int equipIndexInList = equips.IndexOf(dbe);
			
			//获取卡牌在卡组中的位置0-6,6为未使用//
			int equipAddId = getEquipdedIndex(dbe, cg);
			string str;
			if(equipAddId < 6){
				if(equipAddId == curCardId){
					str = equipIndexInList + "-" + 1 + "-null";
				}
				else {
					
					str = equipIndexInList + "-" + 1 + "-" + equipAddId;
				}
			}
			else{
				str = equipIndexInList + "-" + 2 + "-null";
			}
			tempStrList.Add(str);
		}
		return tempStrList;
	}
	
	//获取卡牌列表，并排序，排序规则按照表中的number列的从小到大的顺序开始排//
	private static List<PackElement> getSortedCardsList(CardGroup cg,List<PackElement> cards)
	{
		if(cards!=null && cg!=null)
		{
			/**按照序号排序**/
			List<PackElement> result=new List<PackElement>();
			foreach(PackElement dbc in cards)
			{
				CardData cd=CardData.getData(dbc.dataId);
				if(cd==null)
				{
					continue;
				}
				for(int i=0;i<result.Count;i++)
				{
					CardData cdTemp=CardData.getData(result[i].dataId);
					if(cd.number<cdTemp.number)
					{
						result.Insert(i,dbc);
						break;
					}
					
				}
				if(!result.Contains(dbc))
				{
					result.Add(dbc);
				}
			}
			//为已使用的进行排序,并将其放在最前面//
			List<PackElement> used=new List<PackElement>();
			for(int m = 0; m<result.Count;)
			{
				PackElement card = result[m];
				int cardIndex=getCardAddIndex(card,cg);
				CardData cd=CardData.getData(card.dataId);
				if(cardIndex<6)
				{
					for(int i = 0 ; i < used.Count;++i)
					{
						CardData cdTemp=CardData.getData(used[i].dataId);
						if(cd.number<cdTemp.number)
						{
							used.Insert(i,card);
							break;
						}
					}
					if(!used.Contains(card))
					{
						used.Add(card);
					}
					result.Remove(card);
				}
				else 
				{
					m++;
				}
			}
			//将已使用的开拍放在list的最前面//
			for(int i =0;i< used.Count;i++){
				result.Insert(i,used[i]);
			}
			used.Clear();
			return result;
		}
		return null;
	}
	
	//**去掉大于种族4的卡**//
	public static List<PackElement> getCanFightCards(List<PackElement> cards)
	{
		for(int i=cards.Count-1;i>=0;i--)
		{
			PackElement card=cards[i];
			CardData cd=CardData.getData(card.dataId);
			if(cd!=null && cd.race>4)
			{
				cards.Remove(card);
			}
		}
		return cards;
	}
	
	//去除非正常主动技能的卡牌//
	public static List<PackElement> getCanFightSkill(List<PackElement> skills)
	{
		for(int i = skills.Count - 1;i >=0;i--)
		{
			PackElement pe = skills[i];
			SkillData sd = SkillData.getData(pe.dataId);
			if(sd != null && sd.exptype != 1)
			{
				skills.Remove(pe);
			}
		}
		return skills;
	}
	
	/**获取一件装备装在卡组的第几个位置上,返回0--6,6表示没有装上**/
	private static int getCardAddIndex(PackElement dbc,CardGroup cg)
	{
		for(int k=0;k<cg.cards.Length;k++)
		{
			PackElement cardAdd=cg.cards[k];
			if(cardAdd!=null && cardAdd.i == dbc.i)
			{
				return k;
			}
		}
		return cg.cards.Length;
	}
	
	/**返回结果格式:卡牌在PlayerInfo里的Index-标记,其中标记表示:1使用中,2可使用,3可激活,4不可激活**/
	//curCardIndex 当前卡牌在卡组中的index//
	public static List<string> getCardStrList(CardGroup cg,List<PackElement> cards, int curCardIndex){
		//获取当前背包里的卡牌列表/,并删除经验卡/
		for(int i = 0;i < cards.Count;)
		{
			PackElement pe = cards[i];
			CardData cd = CardData.getData(pe.dataId);
			if(cd.element >= 5)		//经验卡//
			{
				cards.Remove(pe);
			}
			else 
			{
				i++;
			}
		}
		//最后按星级排序//
		List<PackElement> cardList = getSortedCardsList(cg,cards);
		
		List<string> tempStrList = new List<string>();
		for(int i = 0;i < cardList.Count;i++){
			PackElement dbc = cardList[i];
			int cardIndexInList = cards.IndexOf(dbc);
			//获取卡牌在卡组中的位置0-6,6为未使用//
			int cardAddId = getCardAddIndex(dbc, cg);
			string str;
			//int index = getDBCardIndex(dbc);
			if(cardAddId < 6){
				str = cardIndexInList + "-" + 1 + "-" + cardAddId ;
				if(cardAddId == curCardIndex){
					tempStrList.Add(str);
				}
			}
			else{
				str = cardIndexInList + "-" + 2 + "-null";
				tempStrList.Add(str);
			}
		}
		return tempStrList;
	}
	
	//获取主动技能列表//
	/**按照number从小到大的顺序排序**/
	public static List<PackElement> getSortedSkillsList(CardGroup cg,int curCardId,List<PackElement> skills)
	{
		if(skills!=null && cg!=null)
		{
			/**按照number排序**/
			List<PackElement> result=new List<PackElement>();
			foreach(PackElement dbs in skills)
			{
				SkillData sd=SkillData.getData(dbs.dataId);
				if(sd==null)
				{
					continue;
				}
				for(int i=0;i<result.Count;i++)
				{
					SkillData sdTemp=SkillData.getData(result[i].dataId);
					if(sd.number<sdTemp.number)
					{
						result.Insert(i,dbs);
						break;
					}
				}
				if(!result.Contains(dbs))
				{
					result.Add(dbs);
				}
			}
			
			//为已使用的进行排序,并将其放在最前面//
			List<PackElement> used=new List<PackElement>();
			for(int m = 0; m<result.Count;)
			{
				PackElement skill = result[m];
				int skillIndex=getSkilledIndex(skill,cg);
				SkillData sd=SkillData.getData(skill.dataId);
				if(skillIndex<6 && skillIndex == curCardId)
				{
					for(int i=0;i<used.Count;i++)
					{
						SkillData sdTemp=SkillData.getData(used[i].dataId);
						if(sd.number<sdTemp.number)
						{
							used.Insert(i,skill);
							break;
						}
					}
					if(!used.Contains(skill))
					{
						used.Add(skill);
					}
					result.Remove(skill);
				}
				else 
				{
					m++;
				}
			}
			//将已使用的开拍放在list的最前面//
			for(int i =0;i< used.Count;i++)
			{
				result.Insert(i,used[i]);
			}
			used.Clear();
			
			if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Skill))
			{
				PackElement targetPE = null;
				for(int i = 0 ; i < result.Count;i++)
				{
					if(result[i].dataId == 13021)
					{
						targetPE = result[i];
						result.Remove(targetPE);
						break;
					}
				}
				result.Insert(0,targetPE);
			}

			return result;
		}
		return null;
	}
	
	/**获取一个主动技能装在卡组的第几个位置上,返回0--6,6表示没有装上**/
	public static int getSkilledIndex(PackElement dbs,CardGroup cg)
	{
		for(int k=0;k<cg.skills.Length;k++)
		{
			PackElement skilled=cg.skills[k];
			if(skilled!=null && skilled.i == dbs.i)
			{
				return k;
			}
		}
		return cg.skills.Length;
	}
		
	/**返回结果格式:合体技ID-标记,其中标记表示:1使用中,2可使用,3可激活,4不可激活**/
	//curCardId, 当前打开的卡牌的在卡组中的位置//
	public static List<string> getActiveSkillStrList(CardGroup cg, int curCardId,List<PackElement> skills)
	{
		List<PackElement> skillList = getSortedSkillsList(cg,curCardId,skills);
		List<string> tempStrList = new List<string>();
		for(int i = 0;i < skillList.Count;i++)
		{
			PackElement dbs = skillList[i];
			int skillIndexInList = skills.IndexOf(dbs);
			//获取卡牌在卡组中的位置0-6,6为未使用//
			int skillAddId = getSkilledIndex(dbs, cg);
			string str;
			
			if(skillAddId < 6)
			{
				if(skillAddId == curCardId)
				{
					str = skillIndexInList + "-" + 1 + "-null";
				}
				else
				{
					str = skillIndexInList + "-" + 1 + "-" + skillAddId;
				}
			}
			else
			{
				str = skillIndexInList + "-" + 2 + "-null";
			}
			tempStrList.Add(str);
		}
		return tempStrList;
	}
		
	//获取被动技能列表//
	/**排序:星级降序排列,如果同星级有已装备过的,那么已装备过的显示在上边**/
	public static List<PackElement> getSortedPassiveSkillsList(int type,CardGroup cg,int curCardId,List<PackElement> passiveSkills)
	{
		if(passiveSkills!=null && cg!=null)
		{
			/**排序**/
			List<PackElement> result=new List<PackElement>();
			foreach(PackElement dbps in passiveSkills)
			{
				PassiveSkillData psd=PassiveSkillData.getData(dbps.dataId);
				if(psd==null)
				{
					continue;
				}
				for(int i=0;i<result.Count;i++)
				{
					PassiveSkillData psdTemp=PassiveSkillData.getData(result[i].dataId);
					if(psd.number<psdTemp.number)
					{
						result.Insert(i,dbps);
						break;
					}
				}
				if(!result.Contains(dbps))
				{
					result.Add(dbps);
				}
			}
			
			//为已使用的进行排序,并将其放在最前面//
			//List<PackElement> otherUsed=new List<PackElement>();
			List<PackElement> used = new List<PackElement>();
			
			PackElement useTarget = null;
			
			for(int m = 0; m<result.Count;)
			{
				PackElement pSkill = result[m];
				int psIndex = -1;
				int pSkillIndex=getPassiveSkilledIndex(pSkill,cg,ref psIndex);
				PassiveSkillData psd=PassiveSkillData.getData(pSkill.dataId);
				if(pSkillIndex<6 && pSkillIndex == curCardId)
				{
					if(psIndex == (type - 6) )
					{
						useTarget = pSkill;
					}
					else
					{
						for(int i=0;i<used.Count;i++)
						{
							PassiveSkillData psdTemp=PassiveSkillData.getData(used[i].dataId);
							if(psd.number<psdTemp.number)
							{
								used.Insert(i,pSkill);
								break;
							}
						}
						if(!used.Contains(pSkill))
						{
							used.Add(pSkill);
						}
						
					}
					result.Remove(pSkill);
				}
				else
				{
					m++;
				}
			}
			//将已使用的开拍放在list的最前面//
			for(int i =0;i< used.Count;i++)
			{
				result.Insert(i,used[i]);
			}
			
			if(useTarget != null)
			{
				result.Insert(0,useTarget);
			}
			useTarget = null;
			used.Clear();
			
			return result;
		}
		return null;
	}
	
	/**获取一个被动技能装在卡组的第几个位置上,返回0--6,6表示没有装上,psIndex : 3个被动技能的相应的位置**/
	public static int getPassiveSkilledIndex(PackElement dbps,CardGroup cg,ref int  psIndex)
	{
		for(int i=0;i<cg.passiveSkills.Length;i++)
		{
			if(cg.passiveSkills[i] == null)
			{
				cg.passiveSkills[i] = new List<PackElement>();
				for(int j = 0; j < 3; ++j)
				{
					cg.passiveSkills[i].Add(null);
				}
			}
			for(int j = 0; j < cg.passiveSkills[i].Count ; ++j)
			{
				PackElement psd = cg.passiveSkills[i][j];
				if(psd == null)
					continue;
				if(psd.i == dbps.i)
				{
					psIndex = j;
					return i;
				}
			}
		}
		return 6;
	}
	
	/**返回结果格式:合体技ID-标记,其中标记表示:1使用中,2可使用,3可激活,4不可激活**/
	public static List<string> getunActiveSkillStrList(int type,CardGroup cg, int curCardId,List<PackElement> passiveSkills)
	{
		List<PackElement> passiveSkillList = getSortedPassiveSkillsList(type,cg,curCardId,passiveSkills);
		List<string> tempStrList = new List<string>();
		for(int i = 0;i < passiveSkillList.Count;i++)
		{
			PackElement dbps = passiveSkillList[i];
			int passiveSkillIndexInList = passiveSkills.IndexOf(dbps);
			//获取卡牌在卡组中的位置0-6,6为未使用//
			int psIndex = -1;
			int passiveSkillAddId = getPassiveSkilledIndex(dbps, cg,ref psIndex);
			string str;
			if(passiveSkillAddId < 6)
			{
				if(passiveSkillAddId == curCardId)
				{
					if((type-6) == psIndex)
					{
						str = passiveSkillIndexInList + "-" + 1 + "-cur";
					}
					else
					{
						str = passiveSkillIndexInList + "-" + 1 + "-" + passiveSkillAddId.ToString();
					}
					
				}
				else
				{
					str = passiveSkillIndexInList + "-" + 1 + "-" + passiveSkillAddId.ToString();
				}
			}
			else
			{
				str = passiveSkillIndexInList + "-" + 2 + "-null";
			}
			tempStrList.Add(str);
		}
		return tempStrList;
	}
	
	//设置信息 格式： id-mark mark 0 未解锁， 1已解锁//
	public void SetUnLockData(string[] data){
		//先清空一下//
		UnLockDataTabel.Clear();
		//向hashTable中添加数据//
		for(int i = 0;i < data.Length; i++){
			string[] str = data[i].Split('-');
			int id = StringUtil.getInt(str[0]);
			int mark = StringUtil.getInt(str[1]);
			UnLockDataTabel.Add(id, mark);
		}
	}
	
	//获取某一模块的解锁情况//
	public int getUnLockData(int modelId){
        if (UnLockDataTabel.ContainsKey(modelId))
            return (int)UnLockDataTabel[modelId];
        return 1;
	}
	
}

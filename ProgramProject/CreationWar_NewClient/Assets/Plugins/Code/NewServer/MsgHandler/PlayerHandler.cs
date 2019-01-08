using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandler : ServerMonoBehaviour
{
	public GameObject allRes;
	//public MainMenuManage MMManage;
    protected override void OnOperationResponse(ZMNetData mData)
    {
        //base.OnOperationResponse(mData);
        switch ((OpCode)mData.type)
        {
            case OpCode.ACTOR_LOGIN_SERVER:
                {
                    actorLogin(mData);
                }
                break;
            case OpCode.UNIT_REFRESH_SERVER:
                {
					refreshUnit(mData);
//                    StartCoroutine(YieldrefreshUnit(mData));
                }
                break;
            case OpCode.UNIT_MULTI_REFRESH_SERVER:
                {
                    refreshMultiUnit(mData);
                }
                break;
            case OpCode.PLAYER_MOVE_SERVER:
                {
                    serverMove(mData);
                }
                break;
            case OpCode.BROADCAST_USE_SKILL:
                {
                    serverUseSkill(mData);
                }
                break;
            case OpCode.SERVER_NEW_DAY:
                {
                    serverNewDay(mData);
                }
                break;
            //            case OpCode.MAP_PLAYER_COUNT:
            //                {
            //                    mapPlayerCount(mData);
            //                }
            //                break;
            case OpCode.EnterGame:
                {
                    EnterGame(mData);
                }
                break;
            case OpCode.ATTACK_TARGET:
                {
                    attackTarget(mData);
                }
                break;
            case OpCode.HP_CHANGED:
                {
                    hpChanged(mData);
                }
                break;
            case OpCode.SYNC_ACT:
                {
                    syncAct(mData);
                }
                break;
            case OpCode.CLIENT_ADD_TO_MAP:
                {
                    saveMapInstanceID(mData);
                }
                break;
            case OpCode.logoutByGM:
                {
                    logoutByGM(mData);
                }
                break;
			case OpCode.FORCE_MOVE:
				{
					forceMove(mData);
		        }
			break;
            case OpCode.HomeQueue:
            {
                logoutByHome(mData);
            }
            break;
            case OpCode.ChangeForceValue:
            {
                ChangeForceValue(mData);
            }
            break;
            case OpCode.DUEL_GET_PLAYERS:
            {
                duelGetPlayers(mData);
            }
            break;
            case OpCode.DUEL_INVITE:
            { 
                duelInvite(mData);
            }
            break;
            case OpCode.DUEL_INVITE_ERROR:
            {
                duelInviteError(mData);
            }
            break;
            case OpCode.DUEL_INVITE_FEEDBACK:
            {
                duelInviteFeedback(mData);
            }
            break;
            case OpCode.DUEL_RESULT:
            {
                duelResult(mData);
            }
            break;
            case OpCode.DUEL_MAP_LIST:
            {
                duelMapList(mData);
            }
            break;
            case OpCode.DUEL_STATE_CHANGE:
            {
                duelStateChange(mData);
            }
            break;
            case OpCode.TOWER_OPEN:
            {
                towerOpen(mData);
            }
            break;
            case OpCode.CAN_ADD_TO_INSTANCE_MAP:
            {
                addToInstanceMap(mData);
            }
            break;
            case OpCode.TOWER_REWARD:
            {
                towerReward(mData);
            }
            break;
            case OpCode.SMELT:
            {
                smelt(mData);
            }
            break;
            case OpCode.SMELT_GET_NUM:
            {
                smeltGetNum(mData);
            }
            break;
            case OpCode.TOWER_CHALLENGE:
            {
                towerChallenge(mData);
            }
            break;
		}
    }

    private void towerChallenge(ZMNetData mData)
    {
        try
        { 
            int chanllengeResult = mData.readInt();
            if(chanllengeResult == CommonDefine.TOWER_CHALLENGE_FAIL)
            {
                BtnGameManagerBack.my.SwitchToStore();
            }
            else
            {
                int bloodStone = mData.readInt();
                int curTowerNum = mData.readInt();
                int towerLevel = mData.readInt();
                int towerState = mData.readInt();
                int towerTickets = mData.readInt();
                int hasReward = mData.readInt();// 0没有奖励，1表示有奖励
                int bloodstone = mData.readInt();

                BtnGameManager.yt[0]["Bloodstone"].YuanColumnText = bloodstone.ToString();
                PanelStatic.StaticBtnGameManager.invcl.SendMessage("ReInitItem", SendMessageOptions.DontRequireReceiver);

                if (null != TrapTower.instance)
                {
                    TrapTower.instance.SetLastTimeTxt(towerTickets);// 设置剩余挑战次数
                    //TrapTower.instance.SetSkipChallengeTxt(towerNumMax);
                    TrapTower.instance.SetCurrLayerTxt(curTowerNum);
                    TrapTower.instance.SetTowerLvlAndState(towerLevel, towerState);// 副本难度
                    TrapTower.instance.RefreshPanelInfo(hasReward);
                }
                else
                {
                    Debug.LogError("PlayerHandler::towerOpen-------------------TrapTower.instance is null!");
                }

                PanelStatic.StaticWarnings.warningAllTime.Show("", string.Format("{0}{1}{2}", StaticLoc.Loc.Get("tower010"), bloodStone, StaticLoc.Loc.Get("messages053")));// 提示： 扫荡成功，消耗XX血石
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
        finally
        {
            PanelStatic.StaticBtnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
        }
    }

    private void smeltGetNum(ZMNetData mData)
    {
        int curSmeltNum = mData.readInt();
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("RetrunsmeltGetNum" , curSmeltNum , SendMessageOptions.DontRequireReceiver);
		PanelStatic.StaticBtnGameManager.CloseLoading ();
    }

    private void smelt(ZMNetData mData)
    {
		bool success = mData.readBoolean();
		if(success){
			int curSmeltNum = mData.readInt();
			int bloodstone = mData.readInt();
			
			BtnGameManager.yt[0]["Bloodstone"].YuanColumnText = bloodstone.ToString();
			PanelStatic.StaticBtnGameManager.invcl.SendMessage("ReInitItem", SendMessageOptions.DontRequireReceiver);
			
			PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("Retrunsmelt" , curSmeltNum , SendMessageOptions.DontRequireReceiver);
			PanelStatic.StaticBtnGameManager.CloseLoading ();
		}else{
			// faild
		}
    }

    private void towerReward(ZMNetData mData)
    {
        try 
        { 
            //领取奖励结果
            bool rewardResult = mData.readBoolean();

            Debug.Log("towerReward----------------------------" + rewardResult);

            if (rewardResult)
            {
                if (TrapTower.instance)
                {
                    TrapTower.instance.SetCurrLayerTxt(0);
                    //TrapTower.instance.SetLastTimeTxt(0);
                    TrapTower.instance.SetTowerState(0);
                    TrapTower.instance.SetGoldNum("0");
                    TrapTower.instance.SetExpNum("0");
                    TrapTower.instance.SetChallengeBtnTxt();
                    TrapTower.instance.InitItemGrid();
                    TrapTower.instance.InitBtnState();
                    TrapTower.instance.SetSkipChallengeTxt(0);
                    TrapTower.instance.EnableRewardLeaveBtn(false);
                }

                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("meg0152"));
                //if (TrapTower.instance)
                //{
                //    TrapTower.instance.CloseAndLeave();
                //}
            }
            else
            {
                PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("meg0153"));
                //if (TrapTower.instance)
                //{
                //    TrapTower.instance.CloseAndLeave();
                //}
            }

            if (TrapTower.instance)
            {
                TrapTower.instance.CloseAndLeave();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
        finally
        {
            PanelStatic.StaticBtnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
        }
    }

    private void addToInstanceMap(ZMNetData mData)
    {
        int addMapResult = mData.readInt();
        if(addMapResult == CommonDefine.ADD_TO_INSTANCE_MAP_OK)
        {
			int mapID = mData.readInt();
			PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("NowGoSelectChannel" , mapID , SendMessageOptions.DontRequireReceiver);
        }
        else if(addMapResult == CommonDefine.ADD_TO_INSTANCE_MAP_FAIL)
        {
			PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("NowSelectChannel" , false , SendMessageOptions.DontRequireReceiver);
        }
    }

    private void towerOpen(ZMNetData mData)
    {
        try
        {
            int towerLevel = mData.readInt();
            int towerNumMax = mData.readInt();
            //bool isStart = mData.readBoolean();
            int towerNum = mData.readInt();
            Debug.Log("towerOpen------------------" + towerNum);

            //今天是否可以挑战塔
            int towerTickets = mData.readInt();
            //当前刷塔状态
            int towerState = mData.readInt();
            int hasReward = mData.readInt();

            Debug.Log("towerOpen------------------towerLevel:" + towerLevel + "towerNumMax:" + towerNumMax + "towerNum:" + towerNum + "towerTickets:" + towerTickets + "towerState" + towerState);

            if (null != TrapTower.instance)
            {
                TrapTower.instance.SetLastTimeTxt(towerTickets);// 设置剩余挑战次数
                TrapTower.instance.SetSkipChallengeTxt(towerNumMax, hasReward);
                TrapTower.instance.SetCurrLayerTxt(towerNum);
                TrapTower.instance.SetTowerLvlAndState(towerLevel, towerState);// 副本难度
                TrapTower.instance.SetChallengeBtnTxt();
                TrapTower.instance.InitBtnState();
                TrapTower.instance.InitItemGrid();
            }
            else
            {
                Debug.LogError("PlayerHandler::towerOpen-------------------TrapTower.instance is null!");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
        finally
        {
            PanelStatic.StaticBtnGameManager.CloseLoading();//当有转圈时，这里需要打开注释
        }
    }

    private void duelStateChange(ZMNetData mData)
    {
		int instanceid = mData.readInt();
        int currentDuelState = mData.readInt();
		GameObject go  = ObjectAccessor.getAOIObject(instanceid);
		if(go != null){
			go.SendMessage("SetPlayerDuelState" , currentDuelState , SendMessageOptions.DontRequireReceiver);
		}
    }
    
    private void duelMapList(ZMNetData mData)
    {
        int listCount = mData.readInt();
        //instanceid ,canSelect
        Dictionary<int, bool> mapInstanceDic = new Dictionary<int, bool>();
 
        for(int i=0; i<listCount;i++)
        {
            mapInstanceDic.Add(mData.readInt(), mData.readBoolean());
        }
		SelectChannel.SC.ShowSelectChannel(mapInstanceDic);
    }
    //决斗结果
    private void duelResult(ZMNetData mData)
    {
        int result = mData.readInt();
		int duelPlayerID = mData.readInt();
        if(result == CommonDefine.DUEL_RESULT_WIN)
        {
			PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("DuelWin" , duelPlayerID , SendMessageOptions.DontRequireReceiver);
        }
        else if(result == CommonDefine.DUEL_RESULT_LOSE)
        {
			PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("DuelLose" , duelPlayerID , SendMessageOptions.DontRequireReceiver);
        }
    }

	//邀请决斗反馈
	private void duelInviteFeedback(ZMNetData mData)
	{
		int feedbackValue = mData.readInt();
		int instanceID = 0;
		if(feedbackValue == CommonDefine.DUEL_INVITE_FEEDBACK_YES)
		{
			instanceID = mData.readInt();
			PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("StartDuel" , instanceID , SendMessageOptions.DontRequireReceiver);
		}
		else if(feedbackValue == CommonDefine.DUEL_INVITE_FEEDBACK_NO)
		{
			instanceID = mData.readInt();
			PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("RefuseDuel" , SendMessageOptions.DontRequireReceiver);
		}
		else if(feedbackValue == CommonDefine.DUEL_INVITE_FEEDBACK_TIMEOUT)
		{
			
		}
	}

    //邀请决斗异常
    private void duelInviteError(ZMNetData mData)
    {
        string errorMsg = mData.readString();
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("duelInviteError",  errorMsg , SendMessageOptions.DontRequireReceiver);
        //todo show error dialog
    }

    //收到决斗邀请
    private void duelInvite(ZMNetData mData)
    {
        int playerPro = mData.readInt();
        string playerName = mData.readString();
        int playerLevel = mData.readInt();
        int playerForceValue = mData.readInt();

		object[] duel = new object[4]; 
		duel[0] = playerPro;
		duel[1] = playerName;
		duel[2] = playerLevel;
		duel[3] = playerForceValue;
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("DuelInvite",  duel , SendMessageOptions.DontRequireReceiver);
        //todo show dialog with above value
    }

    private void duelGetPlayers(ZMNetData mData)
    {
        int playerSize = mData.readInt();
        int playerPro = 0;
        string playerName = "";
        int playerLevel = 0;
        int playerDuelWinCount = 0;
        int playerDuelState = 0;
        int playerForceValue = 0;
		int instanceID = 0;
		object[,] players = new object[playerSize, 7] ;

        for(int i =0; i< playerSize; i++)
        {
			instanceID = mData.readInt ();
            playerPro = mData.readInt();
            playerName = mData.readString();
            playerLevel = mData.readInt();
            playerDuelWinCount = mData.readInt();
            playerDuelState = mData.readInt();
            playerForceValue = mData.readInt();
			players[i , 0] = playerPro;
			players[i , 1] = playerName;
			players[i , 2] = playerLevel;
			players[i , 3] = playerDuelWinCount;
			players[i , 4] = playerDuelState;
			players[i , 5] = playerForceValue;
			players[i , 6] =  instanceID;
		}

		NearPlayers.NP.ShowNearPlayer(players);
    }

    private void ChangeForceValue(ZMNetData mData)
    {
        int value = mData.readInt();
    }
    private void logoutByHome(ZMNetData mData)
    {
        PanelStatic.StaticBtnGameManager.OffLine();
    }

	private void forceMove(ZMNetData mData)
	{
		int instanceID = mData.readInt ();
		//NGUIDebug.Log("222222222222222222222222222222" + instanceID);
		float positionX = mData.getFloat();
		float positionZ = mData.getFloat();

		int[] info = new int[4];
		info [0] = mData.readInt ();
		info [1] = mData.readInt ();
		info [2] = mData.readInt ();
		info [3] = mData.readInt ();

		object[] objects = new object[2];
		objects[0] = new Vector2(positionX, positionZ);
		objects [1] = info;

//		Debug.Log (positionX, positionZ);

		if(instanceID == PlayerUtil.myID)
		{
			allRes.SendMessage ("ReciveJitui", objects, SendMessageOptions.DontRequireReceiver );
		}
		else
		{
			GameObject go  = ObjectAccessor.getAOIObject(instanceID);
			if(go)
			{
				go.SendMessage ("ReciveJitui", objects, SendMessageOptions.DontRequireReceiver );
			}
		}
	}

	private void logoutByGM(ZMNetData mData)
	{
		//todo  show model window and return to mainmenu,提示已被踢下线
		PanelStatic.StaticBtnGameManager.PlayerOffline();
	}

    private void saveMapInstanceID(ZMNetData mData)
    {
        int mapInstanceID = mData.readInt();
        int mapState = mData.readInt();
        PlayerUtil.mapInstanceID = mapInstanceID;  
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("saveMapInstanceID", mapInstanceID , SendMessageOptions.DontRequireReceiver);
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("SetmapState", mapState , SendMessageOptions.DontRequireReceiver);
    }

    private void attackTarget(ZMNetData mData)
    {
        int srcInstanceID = mData.readInt();
        int targetInstanceID = mData.readInt();
        int damage = mData.readInt();
        int skillID = mData.readInt();
        string effect = mData.readString();
//		Debug.Log("attackTarget ---------------------- " + damage);

		object[] objs = new object[5];
		objs[0] = srcInstanceID;
		objs[1] = targetInstanceID;
		objs[2] = damage;
		objs[3] = skillID;
		objs[4] = effect;

		allRes.SendMessage("ResponseDamage" , objs , SendMessageOptions.DontRequireReceiver);
    }

    private void hpChanged(ZMNetData mData)
    {
        int instanceID = mData.readInt();
        int curHP = mData.readInt();
		if(instanceID == PlayerUtil.myID){
			allRes.SendMessage("AllResSynHealth" , curHP , SendMessageOptions.DontRequireReceiver);
		}else{
			GameObject go  = ObjectAccessor.getAOIObject(instanceID);
			if(go){
				go.SendMessage("SynHealth" , curHP.ToString() , SendMessageOptions.DontRequireReceiver);
			}
		}
    }

    private void EnterGame(ZMNetData mData)
    {
        int isTitle = mData.readInt();
        if (isTitle == 0)
        {
            bool isReOnline = mData.readBoolean();
            string[] strkey = ReadPlayerInfoXML.playerinfo;
            string[] strvalue = mData.getStrings();
            for (int i = 0; i < ReadPlayerInfoXML.playerinfo.Length; i++)
            {
                if (!string.IsNullOrEmpty(strkey[i]))
                {
                    if (BtnGameManager.yt.Rows[0].ContainsKey(strkey[i]))
                    {
                        BtnGameManager.yt.Rows[0][strkey[i]].YuanColumnText = strvalue[i];
                    }
                    else
                    {
                        BtnGameManager.yt.Rows[0].Add(strkey[i], strvalue[i]);
                    }
                }
            }
            //  MainMenuManage.isSetID = true;      
            //        Debug.Log("进入游戏----------------");
            if (isReOnline)
            {
                PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("PGetOutOf", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                StartCoroutine(OnlineRealy());
                // OnlineRealy();
            }
            MainMenuManage.my.isGetSetID = true;
        }
        else {
            //请在这里弹出被封号的板板
            BtnManager.my.EndTimeOut();
			MainMenuManage.my.warnings.warningAllEnter.Show(StaticLoc.Loc.Get ("info358"),StaticLoc.Loc.Get ("info1042"));
	
        }
    }

    private IEnumerator OnlineRealy()
    {
        InRoom.GetInRoomInstantiate().GetServerTime();
        yield return new WaitForSeconds(0.1f);
        InRoom.GetInRoomInstantiate().StartLogonTime();
        yield return new WaitForSeconds(0.2f);
        InRoom.GetInRoomInstantiate().OnlineChests();
        yield return new WaitForSeconds(0.2f);
        InRoom.GetInRoomInstantiate().GetFirstPacks();
        yield return new WaitForSeconds(0.3f);
        InRoom.GetInRoomInstantiate().PlayerFirstRecharge();
        yield return new WaitForSeconds(0.3f);
        InRoom.GetInRoomInstantiate().OnlineActivityInfo();       
    }
//    private void mapPlayerCount(ZMNetData mData)
//    {
//        int playerCount = mData.readInt();
//    }

	private void serverNewDay(ZMNetData mData)
	{
		BtnGameManager.yt.Rows[0]["OnlineChestsTime"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["OpenedChests"].YuanColumnText = "0,0,0,0";
		BtnGameManager.yt.Rows[0]["NumDoubleExp"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["CanSalaries"].YuanColumnText = "1";
		BtnGameManager.yt.Rows[0]["CanRankBenefits"].YuanColumnText = "1";
		BtnGameManager.yt.Rows[0]["EveryDayActivity"].YuanColumnText = "";
		BtnGameManager.yt.Rows[0]["AimCooking"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimFinshMission"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimFinshTaskNum"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimFishing"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimGetLogin"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimHeroMission"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimLogin"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimMakeSoul"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimMining"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimPVP1"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimPVP8"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimPVP24"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimPVPN"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimTeamMission"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimTrain"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["AimUpdateEquip"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["PVPTimes"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["PVP8Times"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["ActivtyTask"].YuanColumnText = "";
		BtnGameManager.yt.Rows[0]["pvp1Num"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["pvp1BeNum"].YuanColumnText = "0";
		BtnGameManager.yt.Rows[0]["pvp1PlayerID"].YuanColumnText = "";
		BtnGameManager.yt.Rows[0]["NumActivityTimes"].YuanColumnText = "";
		BtnGameManager.yt.Rows [0] ["AimGet"].YuanColumnText = "0000";
        //BtnGameManager.yt.Rows[0]["towerNum"].YuanColumnText = "0";
        BtnGameManager.yt.Rows[0]["towerTickets"].YuanColumnText = "1";
        

		int dailyBenefits = 0;
        int.TryParse(BtnGameManager.yt.Rows[0]["DailyBenefits"].YuanColumnText, out dailyBenefits);
		if(dailyBenefits< 7)
		{
			dailyBenefits = dailyBenefits + 1;
		}
		BtnGameManager.yt.Rows[0]["DailyBenefits"].YuanColumnText = dailyBenefits.ToString();

        int autoAITime = 0;
        int.TryParse(BtnGameManager.yt.Rows[0]["AutoAITime"].YuanColumnText, out autoAITime);
		if(autoAITime < 300)
		{
			autoAITime = 300;
		}
        BtnGameManager.yt.Rows[0]["AutoAITime"].YuanColumnText = autoAITime.ToString();

		PanelStatic.StaticBtnGameManager.invcl.SendMessage("ReInitItem", SendMessageOptions.DontRequireReceiver);
	}

    private void serverUseSkill(ZMNetData mData)
    {
        int instanceID = mData.readInt();
        int skillType = mData.readInt();
        int skillID = mData.readInt() ;
//		KDebug.WriteLog(System.DateTime.Now + "======================serverUseSkill==================SkillID==" + skillID + "     ,instanceID" + instanceID);
//        Debug.Log("1111111111111111111111111111111    " + instanceID + "     "  + skillID );
		float dirx = 0;
		float diry = 0;
		float dirz = 0;
        if(skillType == CommonDefine.SKILL_TYPE_HAVE_DIRECT)
        {
            dirx = mData.getFloat();
            diry = mData.getFloat();
            dirz = mData.getFloat();
        }
		int targetID = 0;
		targetID = mData.readInt();
		object[] objs = new object[4];
		objs[0] = skillType;
		objs[1] = skillID;
		objs[2] = new Vector3(dirx , diry , dirz);
		objs[3] = targetID;
		GameObject go  = ObjectAccessor.getAOIObject(instanceID);
		if(skillID <= 100){
			go.SendMessage("ResponseSkill" , objs , SendMessageOptions.DontRequireReceiver);
		}else
		if(skillID > 100){
			go.SendMessage("ResponsePunch" , objs , SendMessageOptions.DontRequireReceiver);
		}
    }

    private void actorLogin(ZMNetData mData)
    {
		int instanceID = mData.readInt();
        //int towerNum = mData.readInt();
//		Debug.Log(instanceID + " ============================actorLogin");
        PlayerUtil.myID = instanceID;
		allRes.SendMessage("SetPlayerInstanceID" , instanceID , SendMessageOptions.DontRequireReceiver);
        //todo set local playerid;
    }

    private void serverMove(ZMNetData mData)
    {

        int instanceID = mData.readInt();
		int count = mData.readInt();
//		KDebug.WriteLog(System.DateTime.Now + "======================RECEIVE MOVE====================" + instanceID + ",count =" + count);
        float movementx = mData.getFloat();
        float movementy = mData.getFloat();
        float movementz = mData.getFloat();

        int positionx = mData.readInt();
		float positiony = mData.getFloat();
        int positionz = mData.readInt();

//		Debug.Log(positiony + " ============================================== positiony");

        GameObject go  = ObjectAccessor.getAOIObject(instanceID);
		int[] pMove = new int[6];
		pMove[0] = (int)movementx;
		pMove[1] = (int)movementy;
		pMove[2] = (int)movementz;
		pMove[3] = positionx;
		pMove[4] = (int)(positiony * 100);
		pMove[5] = positionz;

//		Debug.Log(pMove[4] + " ============================================== pMove[4]");
		if(go !=null)
			go.SendMessage("SynMovement" , pMove , SendMessageOptions.DontRequireReceiver);
        //todo go.tomove
        
    }

    private void refreshMultiUnit(ZMNetData mData)
    {
        int size = mData.readInt();
        //Debug.Log("3333333333333333333333333333333333#" + size);
        for(int i =0; i< size; i++)
        {
//			StartCoroutine( YieldrefreshUnit(mData) );
            if (refreshUnit(mData) == -1)
                break;
        }
    }

    private void syncAct(ZMNetData mData)
    {
        int instanceID = mData.readInt();
        int actType = mData.readInt();
//		if( actType == CommonDefine.ACT_ApplyBuff)
//		{
//			KDebug.WriteLog("收到击退消息。。。。。。。。。。" + System.DateTime.Now + "instance id :" + instanceID);
//		}
        string param = mData.readString();




		object[] objs = new object[3];
		objs[0] = instanceID;
		objs[1] = actType;
		objs[2] = param;
		allRes.SendMessage("ResponseSyncAct" , objs , SendMessageOptions.DontRequireReceiver);

//		GameObject go  = ObjectAccessor.getAOIObject(instanceID);
//		string[] strs;
//		int i = 0;
//		if(go){
//			switch(actType)
//			{
//				case CommonDefine.ACT_JUMP:
//				{
//					strs = param.Split(';');
//				go.SendMessage("SyncDidJump" , strs , SendMessageOptions.DontRequireReceiver);
//				}
//					break;
//			}
//		}
    }

//	IEnumerator YieldrefreshUnit(ZMNetData mData){
//		yield return StartCoroutine(refreshUnit(mData));
//	}

	int refreshUnit(ZMNetData mData)
    {
//		yield return new WaitForEndOfFrame();
        Quaternion rotation = Quaternion.identity;
        bool visible = mData.readBoolean();
        int instanceID = mData.readInt();
		int playerDuelState = mData.readInt();
        bool isHangUp = mData.readBoolean();
        int forceValue = mData.readInt();
		//string isrunning = mData.readString();
        int positionX = mData.readInt();
        float positionY = mData.getFloat() + 0.3f;
        int positionZ = mData.readInt();
		bool isRiding = mData.readBoolean();
		string selectMounts = mData.readString();
		//Debug.Log("==============" + positionX + "," + positionY+","+positionZ + "==================");
		Vector3 position = new Vector3(positionX, positionY, positionZ);
		int playerID = mData.readInt();
        //NGUIDebug.Log("--------------------===============>visble:"+visible);
        if(visible)
        {

			//NGUIDebug.Log("@refreshUnit@\n");
			int aoiCount = ObjectAccessor.aoiObject.Count;
			KDebug.WriteLog("#######################################################" + aoiCount );
			//int settingCount = 10; //todo read setting
			int settingCount = PlayerPrefs.GetInt("HideOtherPlayers", 0) == 1 ? -1 : PlayerPrefs.GetInt("PlayerNum", 5); 
			if (aoiCount > settingCount - 1)
			{
//                KDebug.WriteLog("#######################################################" + aoiCount + "         " + settingCount );
				return -1;
			}else{

	            int proID = mData.readInt();
	            string name = mData.readString();
	            string equipItem = mData.readString();
	            string EquipItemSoul = mData.readString();
	            

	            string prefabName = ""; //= mData.readString();
				int skillBranch = 0;
				string skill = "";
				int proId = 0;
				int playerLevel = 0 ;
				int Stamina = 0 ;
				int Strength = 0;
				int Agility = 0;
				int Intellect = 0;
				int Focus = 0;
	            int hp = 0;
                int MaxHp = 0;
                int MaxMana = 0;
				int rank = 0;

	            string selectTitle = mData.readString();
	            string title = mData.readString();
	            string VIPLevel = mData.readString();
	            string GuildName = mData.readString();
	            string PVPmyTeam = mData.readString();

				string battlefieldLabel = "";
				if (proID == 1)
				{
					prefabName = "ZhanShi";
				}
				else if (proID == 2)
				{
					prefabName = "YouXia";
	            }
	            else if (proID == 3)
	            {
	                prefabName = "FaShi";
	            }

				object[] prefabInit = new object[2];
				prefabInit[0] = prefabName;
				prefabInit[1] = instanceID;
				allRes.SendMessage("PlayerprefabInit" , prefabInit , SendMessageOptions.DontRequireReceiver);

	            GameObject resourceGameObject = ObjectAccessor.getPrefab(prefabName);
	//			GameObject go = (GameObject)GameObject.Instantiate(resourceGameObject, position, rotation);
				GameObject go = PhotonNetwork.InstantiateSceneObject(	prefabName , position, rotation , 0 , null);

	            if (go)
	            {
	               ObjectAccessor.addAOIObject(instanceID, go);
	            }

	            playerLevel = mData.readInt();
	            bool isInTown = mData.readBoolean();
	            if (!isInTown)
	            {
	                hp = mData.readInt();
					skillBranch = mData.readInt();
					skill = mData.readString();
					proId = mData.readInt();
					Stamina = mData.readInt();
					Strength = mData.readInt();
					Agility = mData.readInt();
					Intellect = mData.readInt();
					Focus = mData.readInt();
					battlefieldLabel = mData.readString();
                    MaxHp = mData.readInt();
                    MaxMana = mData.readInt();
					rank = mData.readInt();
	            }
             //   NGUIDebug.Log("=========================================================>isInTown:"+isInTown);

	//			Debug.Log (isInTown);
	//			Debug.Log (skillBranch);
	//			Debug.Log (skill);
	//			Debug.Log (proId);
	//			Debug.Log (playerLevel);
	//			Debug.Log (Stamina);
	//			Debug.Log (Strength);
	//			Debug.Log (Agility);
	//			Debug.Log (Intellect);
	//			Debug.Log (Focus);
				if(go){
					object[] objs = new object[27];
					objs[0] = proID;
					objs[1] = name;
					objs[2] = equipItem;
					objs[3] = EquipItemSoul;
					objs[4] = skillBranch;
					objs[5] = skill;
					objs[6] = playerLevel;
					objs[7] = Stamina;
					objs[8] = Strength;
					objs[9] = Agility;
					objs[10] = Intellect;
					objs[11] = Focus;
					objs[12] = playerID;
					objs[13] = battlefieldLabel;
					objs[14] = hp;

	                objs[15] = selectTitle;
	                objs[16] = title;
	                objs[17] = VIPLevel;
	                objs[18] = GuildName;
	                objs[19] = PVPmyTeam;
					if(isRiding){
						objs[20] = 1;
					}else{
						objs[20] = 0;
					}
					objs[21] = selectMounts;
					objs[22] = MaxHp;
                    objs[23] = MaxMana;
					objs[24] = forceValue;
					objs[25] = rank;
					objs[26] = playerDuelState;
					go.SendMessage("SetBasisValue" , objs , SendMessageOptions.DontRequireReceiver);
					/*if(! isRiding && isrunning != ""){
						go.SendMessage("SyncAnimation" , isrunning , SendMessageOptions.DontRequireReceiver);
					}*/
//					if(isHangUp){
//						go.SendMessage("YieldSitDown" , SendMessageOptions.DontRequireReceiver);
//					}else{
//						go.SendMessage("YieldStandUp" , SendMessageOptions.DontRequireReceiver);
//					}
				}
			}
        }
        else
        {
			GameObject objDes = ObjectAccessor.getAOIObject(instanceID);
			if(objDes != null)
			{
				objDes.SendMessage("ClearPetArray" , SendMessageOptions.DontRequireReceiver);
				Destroy(objDes);
				ObjectAccessor.removeAOIObject(instanceID);
			}
            //todo remove
        }
        return 0;
    }
}

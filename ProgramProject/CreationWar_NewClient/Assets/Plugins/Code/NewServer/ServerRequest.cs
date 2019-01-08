using UnityEngine;
using System.Collections;

public static class ServerRequest {
	public static int count = 0;

    /// <summary>
    /// 改变战力值
    /// </summary>
    /// <param name="Value"></param>
    public static void ChangeForceValue(int Value)
    {
        ZMNetData net = new ZMNetData((int)OpCode.ChangeForceValue);
        net.writeInt(Value);
        ZealmConnector.sendRequest(net);
    }

    public static void requestTowerReward()
    {
        ZMNetData net = new ZMNetData((int)OpCode.TOWER_REWARD);
        ZealmConnector.sendRequest(net);
    }

    public static void requestTowerChallenge()
    {
        ZMNetData net = new ZMNetData((int)OpCode.TOWER_CHALLENGE);
        ZealmConnector.sendRequest(net);
    }

    public static void requestTowerOpen()
    {
        ZMNetData net = new ZMNetData((int)OpCode.TOWER_OPEN);
        ZealmConnector.sendRequest(net);
    }

    public static void requestTowerFailed()
    {
        ZMNetData net = new ZMNetData((int)OpCode.TOWER_FAILED);
        ZealmConnector.sendRequest(net);
    }

    public static void requestTowerFloorFinish(int floorNum)
    {
        ZMNetData net = new ZMNetData((int)OpCode.TOWER_FLOOR_FINISH);
        net.writeInt(floorNum);
        ZealmConnector.sendRequest(net);
    }

	public static void requestSmelt(int state)
	{
        ZMNetData net = new ZMNetData((int)OpCode.SMELT);
		net.writeInt(state);
        ZealmConnector.sendRequest(net);
    }

    public static void requestSmeltGetNum()
    {
        ZMNetData net = new ZMNetData((int)OpCode.SMELT_GET_NUM);
        ZealmConnector.sendRequest(net);
    }

	public static void requestCanAddToInsMap(int InstanceID)
    {
        ZMNetData net = new ZMNetData((int)OpCode.CAN_ADD_TO_INSTANCE_MAP);
		net.writeInt(InstanceID);
        ZealmConnector.sendRequest(net);
    }

    public static void requestChangeMapState(int state)
    {
        ZMNetData net = new ZMNetData((int)OpCode.CHANGE_MAP_STATE);
        net.writeInt(state);
        ZealmConnector.sendRequest(net);
    }

	public static void requestMove(Vector3 Movement , Vector3 position )
	{
        //Debug.Log("===============requestMove================" + Time.time);
        /*if(ObjectAccessor.aoiObject.Count == 0)
        {
            return;
        }*/
		count++;
        ZMNetData zd = new ZMNetData((int)OpCode.MOVE_CLIENT);
		zd.writeInt(count);
        zd.putFloat((float)Movement.x);
        zd.putFloat((float)Movement.y);
        zd.putFloat((float)Movement.z);
		
		zd.writeInt((int)position.x);
		zd.putFloat((float)position.y);
		zd.writeInt((int)position.z);
		
		ZealmConnector.sendRequest(zd);
//		KDebug.WriteLog(System.DateTime.Now + "======================requestMove====================" + PlayerUtil.myID + ",count " + count);
	}

	public static void requestRunning(string state)
	{
		ZMNetData zd = new ZMNetData((int)OpCode.IS_RUNNING);
		zd.writeString(state);
		ZealmConnector.sendRequest(zd);
	}
	public static void requestAddToMap(string mapid, Vector3 position)
    {
		KDebug.Log(Application.loadedLevelName + "  =============== song =====  5 ");
		//	Debug.Log("------------------------------------------------------------------------------------;sdkfj;alsdflk;asjdflkajsdfl;kajsdpf;jw[kdf';asdkf;djg;oiifhsfhofdklhg");
        ZMNetData zd = new ZMNetData((int)OpCode.CLIENT_ADD_TO_MAP);
        zd.writeString(mapid);
		zd.putFloat((float)position.x);
		zd.putFloat((float)position.y);
		zd.putFloat((float)position.z);
        ZealmConnector.sendRequest(zd);
    }

    public static void requestAddToMap(int mapInstanceID, Vector3 position)
    {
  //      Debug.Log("11111111111;sdkfj;alsdflk;asjdflkajsdfl;kajsdpf;jw[kdf';asdkf;djg;oiifhsfhofdklhg");
        ZMNetData zd = new ZMNetData((int)OpCode.CLIENT_ADD_TO_INSTANCE_MAP);
        zd.writeInt(mapInstanceID);
		zd.putFloat((float)position.x);
		zd.putFloat((float)position.y);
		zd.putFloat((float)position.z);
        ZealmConnector.sendRequest(zd);
    }
    
    public static void requestUseSkill(int skillType, int skillid, Vector3 dir , int targetID)
    {
		//KDebug.WriteLog(System.DateTime.Now + "======================requestUseSkill================SkillID====" + skillid + "   ,targetID"+ targetID);
        ZMNetData zd = new ZMNetData((int)OpCode.BROADCAST_USE_SKILL);
		zd.writeInt(PlayerUtil.myID);
        zd.writeInt(skillType);
        zd.writeInt(skillid);
//        Debug.Log("#######requestUseSkill####" + PlayerUtil.myID + "        " + skillType + "    " + skillid);
        if(skillType == CommonDefine.SKILL_TYPE_HAVE_DIRECT)
        {
            zd.putFloat(dir.x);
            zd.putFloat(dir.y);
            zd.putFloat(dir.z);
        }
		zd.writeInt(targetID);
        ZealmConnector.sendRequest(zd);
    }

    public static void requestDamage(int damage, int skillID, int skillLevel, int skillBranch, int targetInstanceID)
    {
        ZMNetData zd = new ZMNetData((int)OpCode.ATTACK_TARGET);
        zd.writeInt(damage);
        zd.writeInt(skillID);
        zd.writeInt(skillLevel);
        zd.writeInt(skillBranch);
        zd.writeInt(targetInstanceID);
        ZealmConnector.sendRequest(zd);
    }
	
    public static void requestDamage(int targetInstanceID,int damage, int skillID, string effect)
    {
        ZMNetData zd = new ZMNetData((int)OpCode.ATTACK_TARGET);
        zd.writeInt(targetInstanceID);
        zd.writeInt(damage);
        zd.writeInt(skillID);
		zd.writeString(effect);
        ZealmConnector.sendRequest(zd);
    }

    public static void attachTarget(int targetInstanceID)
    {
        ZMNetData zd = new ZMNetData((int)OpCode.ATTACK_TARGET);
        zd.writeInt(targetInstanceID);
        ZealmConnector.sendRequest(zd);
    }

    public static void requestResetHp()
    {
        ZMNetData zd = new ZMNetData((int)OpCode.RESET_HP);
        ZealmConnector.sendRequest(zd);
    }

	public static void requestSetMaxHP(int hp , bool isInit , int mana)
    {
        ZMNetData zd = new ZMNetData((int)OpCode.SET_MAX_HP);
        zd.writeBoolean(isInit);
        zd.writeInt(hp);   
		zd.writeInt(mana);   
        ZealmConnector.sendRequest(zd);
    }

	public static void requestSetCurHP(int hp)
	{
		ZMNetData zd = new ZMNetData((int)OpCode.SET_CUR_HP);
		zd.writeInt(hp);   
		ZealmConnector.sendRequest(zd);
	}

    //获取决斗地图列表
    public static void requestDuelMapList()
    {
        ZMNetData zd = new ZMNetData((int)OpCode.DUEL_MAP_LIST);
        ZealmConnector.sendRequest(zd);
    }

    //获取附近玩家
    public static void requestDuelGetPlayers()
    {
        ZMNetData zd = new ZMNetData((int)OpCode.DUEL_GET_PLAYERS);
        ZealmConnector.sendRequest(zd);
    }

	//邀请决斗
	public static void requestDuelInvite(int instanceID)
	{
		ZMNetData zd = new ZMNetData((int)OpCode.DUEL_INVITE);
		zd.writeInt(instanceID);
		ZealmConnector.sendRequest(zd);
	}

	public static void requestAcceptDuel(int i)
	{
		ZMNetData zd = new ZMNetData((int)OpCode.DUEL_INVITE_FEEDBACK);
		zd.writeInt(i);
		ZealmConnector.sendRequest(zd);
	}

    public static void requestHangUp(bool isHangUp)
    {
        ZMNetData zd = new ZMNetData((int)OpCode.IS_HANGUP);
        zd.writeBoolean(isHangUp);
        ZealmConnector.sendRequest(zd);
    }

	public static void requestActivityExit()
	{
		ZMNetData zd = new ZMNetData((int)OpCode.ActivityExit);
		ZealmConnector.sendRequest(zd);
	}

	public static void requestDefneceFinish()
	{
		ZMNetData zd = new ZMNetData((int)OpCode.DefenceFinish);
		ZealmConnector.sendRequest(zd);
	}

	public static void requestRide(bool isRide)
	{
		ZMNetData zd = new ZMNetData((int)OpCode.playerRide);
		zd.writeBoolean(isRide); 
		ZealmConnector.sendRequest(zd);
	}

	public static void requestTtile(string mTitle)
	{
		ZMNetData zd = new ZMNetData((int)OpCode.playerTtile);
		zd.writeString(mTitle); 
		ZealmConnector.sendRequest(zd);
	}

	public static void requestAddHp(int hp)
	{
		ZMNetData zd = new ZMNetData((int)OpCode.ADD_HP);
		zd.writeInt(hp) ;
		ZealmConnector.sendRequest(zd);
	}

	//强制移动类型，角度，距离
	public static void requestForceMove(int instanceID, Vector3 direction, int[] info)
	{
		ZMNetData zd = new ZMNetData((int)OpCode.FORCE_MOVE);
		zd.writeInt (instanceID);
		zd.putFloat(direction.x);
		zd.putFloat (direction.z);
		zd.writeInt (info [0]);
		zd.writeInt (info [1]);
		zd.writeInt (info [2]);
		zd.writeInt (info [3]);
		ZealmConnector.sendRequest(zd);
	}

	public static bool isAddToMap = false;
    public static void syncAct(int instanceID, int actType,string param)
    {
		if (ObjectAccessor.aoiObject.Count == 0 || !isAddToMap)
        {
            return;
        }
		/*if(actType == CommonDefine.ACT_CrossAnimation && param == "idle"){
			Debug.Log("idle =======================");
		}*/
        ZMNetData zd = new ZMNetData((int)OpCode.SYNC_ACT);
        zd.writeInt(instanceID);
        zd.writeInt(actType);
//		if( actType == CommonDefine.ACT_ApplyBuff)
//		{
//			KDebug.WriteLog("========发出击退消息。。。。。。。。。。" + System.DateTime.Now + "instance id :" + instanceID);
//		}
        zd.writeString(param);
        ZealmConnector.sendRequest(zd);
    }
}

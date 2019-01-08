#define	msr_AccumulateAI
using	UnityEngine;
using	System.Collections;
using	System.Collections.Generic;

#if msr_AccumulateAI
public	class AIDecisionData
{
	public	int		MonsterID;
	public	int		DecisionID;
	public	string	DecisionData;	
	public byte isMaxHP;
}
#endif

/// <summary>
/// 怪物生成点对服务器发出的请求 -
/// </summary>
public	static	class MonsterServerRequest
{
	/// <summary>
	/// 通知服务器玩家进入了怪物生成点 -
	/// </summary>
	public	static	void	RequestPlayerEnterMonsterSpawnArea(	int	SpawnPointID,	int	WaveID,	int	MonsterHP,	string	MonsterSpawnData	)
	{	///此处普通 的 生成点，波ID为-1 ，返还也是-1，是Dota模式下怪物生成点的属性WaveID起始为0，返还时+1，且 MonsterSpawnData用“，”分割的最后多一位ATTime用于初始化怪物属性//
		//Debug.Log(	"K______________________RequestPlayerEnterMonsterSpawnArea!!!!!!!!!!!SpawnPointID = " + SpawnPointID );
		//KDebug.WriteLog(	"申请出怪 ： SpawnPointID = " + SpawnPointID + " WaveID = " + WaveID	);
		ZMNetData zd = new ZMNetData((int)OpCode.HitMonsterPoint);
		zd.writeInt(	SpawnPointID	);
		zd.writeInt(	WaveID	);
		zd.writeInt(	MonsterHP	);
		zd.writeString(	MonsterSpawnData	);

		ZealmConnector.sendRequest(zd);
	}
	/// <summary>
	/// 通知副本主bosss挂了 -
	/// </summary>
	public	static	void	RequestBoss2WasDie(	int SpawnPointID	)
	{
		ZMNetData zd = new ZMNetData((int)OpCode.Boss2WasDie);
		zd.writeInt(	SpawnPointID	);
		ZealmConnector.sendRequest(zd);
//		KDebug.Log ("发送boss挂了的包");
	}

	static	string	_AIData;
	/// <summary>
	/// 建议Idle OrderType	=	2-
	/// </summary>
	public	static	void	RequestIdle(	int	MonsterID,	int DecisionID,	int	OrderType,	float	RealNavSpeed,	string	FindWayStr,	float	IdleTimeLength	)
	{
		_AIData	="" + OrderType + ";" + RealNavSpeed + ";" + FindWayStr + ";" + IdleTimeLength;
		RquestCallAI(MonsterID,DecisionID,_AIData);
	}
	public	static	string	IdleAIData(	int	OrderType,	float	RealNavSpeed,	string	FindWayStr,	float	IdleTimeLength	)
	{
		_AIData	=	"" + OrderType + ";" + RealNavSpeed + ";" + FindWayStr + ";" + IdleTimeLength;
		return	_AIData;
	}
	/// <summary>
	/// 建议battle	OrderType	=	3-
	/// </summary>
	public	static	void	RequestBattle(	int	MonsterID,	int DecisionID,	int	OrderType	)
	{
		_AIData	=	""	+	OrderType;
		RquestCallAI(MonsterID,DecisionID,_AIData);
	}
	public	static	string	BattleAIData(	int	OrderType	)
	{
		_AIData	=	""	+	OrderType;
		return	_AIData;
	}
	/// <summary>
	/// Requests the skill j - OrderType	=	5 (没有4,忽略位置同步精度，4进入战斗就不发消息了)-
	/// </summary>
	public	static	void	RequestSkillJ(	int	MonsterID,	int DecisionID,	int	OrderType )
	{
		_AIData	=	""	+	OrderType;
		RquestCallAI(MonsterID,DecisionID,_AIData);
	}
	public	static	string	SkillJAIData(	int	OrderType )
	{
		_AIData	=	""	+	OrderType;
		return	_AIData;
	}
	/// <summary>
	/// 使用技能 -OrderType = 6 -
	/// </summary>
	public	static	void	RequestSkill(	int	MonsterID,	int DecisionID,	int	OrderType, int	SkillID )
	{
		_AIData	=	""	+	OrderType + ";" + SkillID;
		RquestCallAI(MonsterID,DecisionID,_AIData);
	}
	public	static	string	SkillAIData(	int	OrderType, int	SkillID )
	{
		_AIData	=	""	+	OrderType + ";" + SkillID;
		return	_AIData;
	}
	/// <summary>
	/// 射击	OrderType	=	7	-
	/// </summary>
	public	static	void	RequestShoot(	int	MonsterID,	int DecisionID,	int	OrderType )
	{
		_AIData	=	""	+	OrderType ;
		RquestCallAI(MonsterID,DecisionID,_AIData);
	}
	public	static	string	ShootAIData(	int	OrderType )
	{
		_AIData	=	""	+	OrderType ;
		return	_AIData;
	}
	/// <summary>
	/// 近战 OrderType	=	8	-
	/// </summary>
	public	static	void	RequestCloseAttack(	int	MonsterID,	int DecisionID,	int	OrderType )
	{
		_AIData	=	""	+	OrderType ;
		RquestCallAI(MonsterID,DecisionID,_AIData);
	}
	public	static	string	CloseAttackAIData(	int	OrderType )
	{
		_AIData	=	""	+	OrderType ;
		return	_AIData;
	}
	/// <summary>
	/// 撤退 OrderType	=	10	-
	/// </summary>
	public	static	void	RequestFallBack(	int	MonsterID,	int DecisionID,	int	OrderType,byte isMaxHP	)
	{
		#if msr_AccumulateAI
		AIDecisionData	nData	=	new	AIDecisionData();
		nData.MonsterID		=	MonsterID;
		nData.DecisionID	=	DecisionID;
		nData.DecisionData	=	FallBackAIData(OrderType);
		nData.isMaxHP=isMaxHP;
		MonsterHandler.RequestFallBack(	nData	);
		#endif

		#if !msr_AccumulateAI
		_AIData	=	""	+	OrderType ;
		//Debug.Log(	"K________Rquest FallBack!!!!!!!!!!!!MonsterID = " + MonsterID + " Data = " + _AIData );
		ZMNetData zd = new ZMNetData((int)OpCode.DecisionFallBack);
		zd.writeInt(	MonsterID	);
		zd.writeInt(	DecisionID	);
		zd.writeString(	_AIData	);
		ZealmConnector.sendRequest(zd);
		#endif
	}
	public	static	string	FallBackAIData(	int	OrderType	)
	{
		_AIData	=	""	+	OrderType ;
		return	_AIData;
	}

	//	Warning!!!Warning!!!Warning!!!此处顶层分隔符使用“;” - 
	/// <summary>
	/// 发起AI建议	-
	/// </summary>
	static	void	RquestCallAI(	int	MonsterID,	int DecisionID,	string	DecisionData	)
	{
		#if msr_AccumulateAI
		AIDecisionData	nData	=	new	AIDecisionData();
		nData.MonsterID		=	MonsterID;
		nData.DecisionID	=	DecisionID;
		nData.DecisionData	=	DecisionData;
		MonsterHandler.RequestCallAI(	nData	);
		#endif
		#if !msr_AccumulateAI
		//Debug.Log(	"K________RquestCallAI!!!!!!!!!!!MonsterID = " + MonsterID + " Data = " + DecisionData );
		ZMNetData zd = new ZMNetData((int)OpCode.Decision);
		zd.writeInt(	MonsterID	);
		zd.writeInt(	DecisionID	);
		zd.writeString(	DecisionData	);
		ZealmConnector.sendRequest(zd);
		#endif
	}

	/// <summary>
	/// 怪物受伤害 -
	/// </summary>
	public	static	void	MonsterDamaged(	int	MonsterID,	int DamageInfo,	string	OtherData	)
	{
		//Debug.Log(	"K________MonsterDamaged!!!!!!!!!!!MonsterID = " + MonsterID );
		ZMNetData zd = new ZMNetData((int)OpCode.AttackMonster);
		zd.writeInt(	MonsterID	);
		zd.writeInt(	DamageInfo	);
		zd.writeString(	OtherData	);
		ZealmConnector.sendRequest(zd);
	}

	/// <summary>
	/// 怪物加buff的请求 -
	/// </summary>
	public	static	void	MonsterAddHealthBuff( int MonsterID, int BuffStatusID, int BuffID, int BuffValue, float	BuffTime, float	BuffInterval )
	{
		//Debug.Log(	"K________Monster_Add_HealthBuff!!!!缓伤代码在这里!!!!!!!MonsterID = " + MonsterID );
		ZMNetData zd = new ZMNetData((int)OpCode.MonsterHealthBuff);
		zd.writeInt(	MonsterID	);
		zd.writeInt(	BuffStatusID	);
		zd.writeInt(	BuffID	);
		zd.writeInt(	BuffValue	);
		zd.putFloat(	BuffTime	);
		zd.putFloat(	BuffInterval	);
		ZealmConnector.sendRequest(zd);
	}

	/// <summary>
	///	召唤骷髅 -
	/// </summary>
	public	static	void	SummonSkull( int PlayerID, int MonsterMaxHP, string OtherData )
	{
		ZMNetData zd = new ZMNetData((int)OpCode.SummonSkull);
		zd.writeInt(	PlayerID	);
		zd.writeInt(	MonsterMaxHP	);
		zd.writeString(	OtherData	);
		ZealmConnector.sendRequest(zd);
		//Debug.Log("-------------------------------SummonSkull");
	}

	/// <summary>
	/// 移除骷髅
	/// </summary>
	/// <param name="skullID">骷髅ID</param>
	public static  void SkullRemove (int skullID)
	{
		ZMNetData zd = new ZMNetData((int)OpCode.SkullRemove);
		zd.writeInt(	skullID	);
		ZealmConnector.sendRequest(zd);
	}
}

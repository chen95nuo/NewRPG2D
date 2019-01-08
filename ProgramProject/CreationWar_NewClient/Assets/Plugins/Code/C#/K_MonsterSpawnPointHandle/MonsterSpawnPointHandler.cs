#define	msp_CombineCommitSpawn

using UnityEngine;
using System.Collections;

/// <summary>
/// 与怪物生成点同在！控制怪物生成点用于同步刷新怪物 -
/// </summary>
public	class	MonsterSpawnPointHandler	:	ServerMonoBehaviour 
{
	/// <summary>
	/// 单例模式控制场景 -
	/// </summary>
	public	static	MonsterSpawnPointHandler	GetInstance()
	{
		if( InstObj	==	null )
		{
			InstObj		=	new	GameObject( "MonsterSpawnPointHandler" );
			Instance	=	InstObj.AddComponent<MonsterSpawnPointHandler>();
		}
		return	Instance;
	}
	private	static	GameObject					InstObj		=	null;
	private	static	MonsterSpawnPointHandler	Instance	=	null;


	private	SpawnPointNetView[]	SpawnPoints;

	/// <summary>
	/// 记录当前的玩家数量 -
	/// </summary>
	public	int	CurrentPlayerCount	=	1;

	public	static	bool	IsSingleCarbon()
	{
		if(	GetInstance().CurrentPlayerCount>1	)
			return	false;
		return	true;
	}

	protected override void Awake()
	{
		base.Awake();
		SpawnPoints	=	FindObjectsOfType<SpawnPointNetView>();
	}

	/// <summary>
	///	继承查阅感兴趣的信息 -
	/// </summary>
	protected override void OnOperationResponse(	ZMNetData	mData	)
	{
		switch(	(OpCode)	mData.type	)
		{
		case	OpCode.MAP_PLAYER_COUNT:
			PlayerCount(mData);
			break;

		case	OpCode.Boss2WasDie:
			OnBoss2WasDie(mData);
			break;
#if !msp_CombineCommitSpawn
		case	OpCode.HitMonsterPoint:
			HitMonsterPoint( mData );
			break;
#endif
		}
	}

	protected	override	void	OnOperationResponse(	Zealm.OperationResponse	operationResponse	)
	{
		switch(	(OpCode)operationResponse.OperationCode	)
		{
#if msp_CombineCommitSpawn
		case	OpCode.HitMonsterPoint:
			//KDebug.Log("收到出怪消息");
			HitMonsterPoint(operationResponse);
			break;
#endif
		}
	}

	private	void	OnBoss2WasDie(	ZMNetData	mData	)
	{
//		KDebug.Log("收到Boss死了的包");
		int	SPID	=	mData.readInt();
		for( int i = 0; i	< SpawnPoints.Length; i++	)
		{
			if( SpawnPoints[i].CurrentSpawnPointID	==	SPID )
			{
				SpawnPoints[i].SendMessage( "Boss2WasDie" );
				return;
			}
		}
	}

	/// <summary>
	/// 读取玩家数量 -
	/// </summary>
	private void PlayerCount(ZMNetData mData)
	{
		CurrentPlayerCount	=	mData.readInt();
		//SendMessage("SetNumPlayer",);
	}
	/// <summary>
	/// 处理生成怪物消息 -
	/// </summary>
	private void HitMonsterPoint( ZMNetData mData )
	{
		int	SPID	=	mData.readInt();
		//KDebug.WriteLog(	"收到出怪 ： SpawnPointID = " + SPID );
		//KDebug.Log ("K________returned___SpawnPointID  =  " +  SPID  );
		for( int i = 0; i	< SpawnPoints.Length; i++	)
		{
			if( SpawnPoints[i].CurrentSpawnPointID	==	SPID )
			{
				int		WaveID			=	mData.readInt();	//判断是普通还是PVP//
				if(	WaveID	<	0	)
				{		//普通//
					int		MonsterID		=	mData.readInt();
					byte	_MonsterStatus	=	mData.readByte();
					int		_MaxHP			=	mData.readInt();
					string	_Data			=	mData.readString();
					//KDebug.Log("完成");
					string[]	Data	=	new	string[4];
					Data[0]	=	""+MonsterID;
					Data[1]	=	""+_MonsterStatus;
					Data[2]	=	""+_MaxHP;
					Data[3]	=	_Data;

					//Debug.Log (	"K________returned___SpawnPointID  =  " +  SPID	+ " | WaveID = " + WaveID + " | MonsterID = " + MonsterID );
					SpawnPoints[i].SendMessage(	"OnMonsterSpawnPointHitted",	Data,	SendMessageOptions.RequireReceiver	);
					return;
				}
				else 	//PVP//
				{
					int		MonsterID		=	mData.readInt();
					byte	_MonsterStatus	=	mData.readByte();
					int		_MaxHP			=	mData.readInt();
					string	_Data			=	mData.readString();
					string[]	Data	=	new	string[5];
					Data[0]	=	""+MonsterID;
					Data[1]	=	""+_MonsterStatus;
					Data[2]	=	""+_MaxHP;
					Data[3]	=	_Data;
					Data[4]	=	""+WaveID;

//					Debug.Log (	"K____PVPVPVPVPVPVPVPVPVPVPVP____returned___SpawnPointID  =  " +  SPID	+ " | WaveID = " + WaveID + " | MonsterID = " + MonsterID );
					SpawnPoints[i].SendMessage(	"OnCallMonster",	Data,	SendMessageOptions.RequireReceiver	);
					return;
				}

			}
		}
	}
	/// <summary>
	/// 处理生成怪物消息 -
	/// </summary>
	private void HitMonsterPoint(	Zealm.OperationResponse	operationResponse	)
	{
		if(	operationResponse.ReturnCode	== (	(short)yuan.YuanPhoton.ReturnCode.Yes	)	)
		{
			int		SPID	=	(int)operationResponse.Parameters[(short)0];

			//KDebug.WriteLog(	"收到出怪 ： SpawnPointID = " + SPID );
			//KDebug.Log ("K________returned___SpawnPointID  =  " +  SPID  );
			for( int i = 0; i	< SpawnPoints.Length; i++	)
			{
				if( SpawnPoints[i].CurrentSpawnPointID	==	SPID )
				{
					int			WaveID			=	(int)operationResponse.Parameters[(short)1];	//判断是普通还是PVP//

					int[]		MonsterIDs		=	operationResponse.Parameters[(short)2] as int[];
					byte[]		MonsterStatus	=	operationResponse.Parameters[(short)3] as byte[];
					int[]		MonsterMaxHPs	=	operationResponse.Parameters[(short)4] as int[];
					string[]	Datas			=	operationResponse.Parameters[(short)5] as string[];
					if(	WaveID	<	0	)
					{	
						//普通//
						string[]	Data			=	new	string[4];
						for( int k = 0; k < MonsterIDs.Length; k++	)
						{
							Data[0]	=	""+MonsterIDs[k];
							Data[1]	=	""+MonsterStatus[k];
							Data[2]	=	""+MonsterMaxHPs[k];
							Data[3]	=	Datas[k];

							//Debug.Log (	"K________returned___SpawnPointID  =  " +  SPID	+ " | WaveID = " + WaveID + " | MonsterID = " + MonsterIDs[k] );
							SpawnPoints[i].SendMessage(	"OnMonsterSpawnPointHitted",	Data,	SendMessageOptions.RequireReceiver	);
						}
					}
					else 	//PVP//
					{
						string[]	Data	=	new	string[5];
						for( int k = 0; k < MonsterIDs.Length; k++	)
						{
							Data[0]	=	""+MonsterIDs[k];
							Data[1]	=	""+MonsterStatus[k];
							Data[2]	=	""+MonsterMaxHPs[k];
							Data[3]	=	Datas[k];
							Data[4]	=	""+WaveID;

//							Debug.Log (	"K____PVPVPVPVPVPVPVPVPVPVPVP____returned___SpawnPointID  =  " +  SPID	+ " | WaveID = " + WaveID + " | MonsterID = " + MonsterIDs[k] );
							SpawnPoints[i].SendMessage(	"OnCallMonster",	Data,	SendMessageOptions.RequireReceiver	);
						}
					}
					return;
				}
			}
		}
		else
		{
			if(	operationResponse.ReturnCode	== ((short)yuan.YuanPhoton.ReturnCode.Error)	)
			{
				Debug.LogError(operationResponse.DebugMessage);
			}
		}

	}
}

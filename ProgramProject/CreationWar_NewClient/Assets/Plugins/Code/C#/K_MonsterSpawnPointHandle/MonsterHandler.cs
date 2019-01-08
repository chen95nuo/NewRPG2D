#define	msr_AccumulateAI
using	UnityEngine;
using	System.Collections;
using	System.Collections.Generic;

public	class	MonsterHandler	:	ServerMonoBehaviour 
{
	#if msr_AccumulateAI
	private	List<AIDecisionData>	AIList			=	new	List<AIDecisionData>();
	private	List<AIDecisionData>	FallBackList	=	new	List<AIDecisionData>();
	public	static	void	RequestCallAI(	AIDecisionData	newData	)
	{
		GetInstance().AIList.Add( newData );
	}
	public	static	void	RequestFallBack(	AIDecisionData	newData	)
	{
		GetInstance().FallBackList.Add( newData );
	}
	void	Start()
	{
		InvokeRepeating(	"SendRequest",	0.2f, 0.2f );
	}
	void	SendRequest()
	{
		if(	AIList.Count	>	0	)
		{
			Dictionary<short,object> dicUpdate=new Dictionary<short, object>();
			int[]	MDIs	=	new	int[AIList.Count];
			int[]	DIDs	=	new	int[AIList.Count];
			string[]DDatas	=	new	string[AIList.Count];
			for( int i = 0; i < AIList.Count; i++	)
			{
				MDIs[i]		=	AIList[i].MonsterID;
				DIDs[i]		=	AIList[i].DecisionID;
				DDatas[i]	=	AIList[i].DecisionData;
			}
			dicUpdate.Add((short)0, MDIs);
			dicUpdate.Add((short)1, DIDs);
			dicUpdate.Add((short)2, DDatas);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.Decision, dicUpdate);
			ZealmConnector.sendRequest(zn);
			AIList.Clear();
		}
		if(	FallBackList.Count	>	0	)
		{
			Dictionary<short,object> dicUpdate=new Dictionary<short, object>();
			int[]	MDIs	=	new	int[FallBackList.Count];
			int[]	DIDs	=	new	int[FallBackList.Count];
			string[]DDatas	=	new	string[FallBackList.Count];
			byte[] DMaxHP=new byte[FallBackList.Count];
			for( int i = 0; i < FallBackList.Count; i++	)
			{
				MDIs[i]		=	FallBackList[i].MonsterID;
				DIDs[i]		=	FallBackList[i].DecisionID;
				DDatas[i]	=	FallBackList[i].DecisionData;
				DMaxHP[i]=FallBackList[i].isMaxHP;
			}
			dicUpdate.Add((short)0, MDIs);
			dicUpdate.Add((short)1, DIDs);
			dicUpdate.Add((short)2, DDatas);
			dicUpdate.Add ((short)3,DMaxHP);
			ZMNetDataLikePhoton zn = new ZMNetDataLikePhoton((short)OpCode.DecisionFallBack, dicUpdate);
			ZealmConnector.sendRequest(zn);
			FallBackList.Clear();
		}
	}
	#endif
	public	static	MonsterHandler	GetInstance()
	{
		if( InstObj	==	null )
		{
			InstObj		=	new	GameObject( "MonsterHandler" );
			Instance	=	InstObj.AddComponent<MonsterHandler>();
		}
		return	Instance;
	}
	private	static	GameObject		InstObj		=	null;
	private	static	MonsterHandler	Instance	=	null;
	private	List<MonsterNetView>	MonsterNetViewList	=	new	List<MonsterNetView>();
	private	List<MonsterNetView>	PetNetViewList		=	new	List<MonsterNetView>();
	public	static	GameObject	CreateMonster(	GameObject	PrefabName, Vector3 position, Quaternion rotation,	int	_MonsterID	)
	{
//		GameObject		GO	=	PhotonNetwork.InstantiateSceneObject(	PrefabName , position, rotation , 0 , null);
		GameObject		GO	=	GameObject.Instantiate(PrefabName , position , rotation) as GameObject;
		MonsterNetView	MH	=	GO.GetComponent<MonsterNetView>();
		if( MH	==	null )
			MH	=	GO.AddComponent<MonsterNetView>();
		MH.MonsterID	=	_MonsterID;
		GetInstance().MonsterNetViewList.Add(MH);
		return	GO;
	}
	public	GameObject	FindMonsterByMonsterID(	int _MonsterID	)
	{
		for( int i = 0; i < MonsterNetViewList.Count; i++	)
		{
			if(	MonsterNetViewList[i]	!=	null	)
			{
				if( MonsterNetViewList[i].MonsterID	==	_MonsterID )
				{
					return	MonsterNetViewList[i].gameObject;
				}
			}
			else
			{
				MonsterNetViewList.RemoveAt(i);
				i--;
			}
		}
		return FindPetByID( _MonsterID	);
	}
	public	MonsterNetView	GetMonsterNetViewByMonsterID(	int _MonsterID	)
	{
		for( int i = 0; i < MonsterNetViewList.Count; i++	)
		{
			if(	MonsterNetViewList[i]	!=	null	)
			{
				if( MonsterNetViewList[i].MonsterID	==	_MonsterID )
				{
					return	MonsterNetViewList[i];
				}
			}
			else
			{
				MonsterNetViewList.RemoveAt(i);
				i--;
			}
		}
		return FindPetNetViewByID( _MonsterID	);
	}
	protected	override	void	OnOperationResponse(	ZMNetData	mData	)
	{
		switch(	(OpCode)	mData.type	)
		{
            case OpCode.MonsterHateList:
                MonsterHateList(mData);
                break;
            case OpCode.RemoveMosterHate:
                RemoveHate(mData);
                    break;
			case	OpCode.AttackMonster:
				AcceptDamage(mData);
				break;
			case	OpCode.MonsterDie:
				AcceptMonsterDeath(mData);
				break;
			case	OpCode.SummonSkull:
				StartCoroutine( AcceptSummonSkull(mData) );
				break;
		case	OpCode.SkullRemove:
			SkullRemove(mData);
			break;
		}
	}


    /**
     * 怪物仇恨列表
     */
    private void MonsterHateList(ZMNetData mData)
    {
        MonsterID = mData.readInt();//怪物实例ID

        string[] instensids = mData.getStrings();//仇恨列表中的玩家实例ID降序
        string[] monsterHate = mData.getStrings();//仇恨列表中的仇恨值降序，与上面一一对应

//        for(int i = 0;i<instensids.Length;i++)
//        {
//            Debug.Log("ININININ-------------instensids:-" + instensids[i] + "-------------Hate:-" + monsterHate[i]);
//        }



		GameObject go = FindMonsterByMonsterID(MonsterID);
		if(go){
			object[] objs = new object[2];
			objs[0] = instensids;
			objs[1] = monsterHate;
			
			go.SendMessage("ServerRemoveHatreda" , objs , SendMessageOptions.DontRequireReceiver);
		}
    }




	private void SkullRemove(ZMNetData	mData)
	{
		int skullID = mData.readInt();
		GameObject Go = FindMonsterByMonsterID(skullID);
		if(Go){
			Go.SendMessage("ServerReturnDesMe" , SendMessageOptions.DontRequireReceiver);
		}
	}
	
	#if msr_AccumulateAI
	protected	override	void	OnOperationResponse(Zealm.OperationResponse operationResponse)
	{
		switch(	(OpCode)operationResponse.OperationCode	)
		{
		case	OpCode.Decision:
			AcceptDecision(operationResponse);
			break;
		case	OpCode.DecisionFallBack:
			AcceptFallBack(operationResponse);
			break;
		}
	}
	#endif
	int	MonsterID;
	void	AcceptDecision(	ZMNetData	mData	)
	{
		MonsterID		=	mData.readInt();
		//Debug.Log( "K________OnAccetpDecision : " + MonsterID );
		MonsterNetView	m	=	GetMonsterNetViewByMonsterID(MonsterID);
		if(	m	!=	null	)
		{
			string[]	StrData	=	new	string[2];
			StrData[0]	=	"" + mData.readInt();
			StrData[1]	=	mData.readString();
			//Debug.Log( "K________OnAccetpDecision : MonsterID = " + MonsterID + " DID = "+StrData[0] + " ;  DData = " + StrData[1] );
			m.SendMessage( "OnAcceptDecision", StrData );	//发送消息处理指令//
		}
	}
	void	AcceptDecision(	Zealm.OperationResponse	operationResponse	)
	{
		int[]	MonsterIDs	=	operationResponse.Parameters[(short)0] as int[];
		int[]	DecisionIDs	=	operationResponse.Parameters[(short)1] as int[];
		string[]	Datas	=	operationResponse.Parameters[(short)2] as string[];
		for( int i = 0; i<MonsterIDs.Length; i++	)
		{
			MonsterNetView	m	=	GetMonsterNetViewByMonsterID(MonsterIDs[i]);
			if(	m	!=	null	)
			{
				string[]	StrData	=	new	string[2];
				StrData[0]	=	"" + DecisionIDs[i];
				StrData[1]	=	Datas[i];
				//Debug.Log( "K________OnAccetpDecision : MonsterID = " + MonsterID + " DID = "+StrData[0] + " ;  DData = " + StrData[1] );
				m.SendMessage( "OnAcceptDecision", StrData );	//发送消息处理指令//
			}
		}
	}

    void RemoveHate(ZMNetData mData)
    {
        MonsterID = mData.readInt();//怪物实例ID

        string[] instensids = mData.getStrings();//仇恨列表中的玩家实例ID降序
        string[] monsterHate = mData.getStrings();//仇恨列表中的仇恨值降序，与上面一一对应

//        for (int i = 0; i < instensids.Length; i++)
//        {
//            Debug.Log("OUTOUTOUT-------------instensids:-" + instensids[i] + "-------------Hate:-" + monsterHate[i]);
//        }

		GameObject go = FindMonsterByMonsterID(MonsterID);
		if(go){
			object[] objs = new object[2];
			objs[0] = instensids;
			objs[1] = monsterHate;
			
			go.SendMessage("ServerRemoveHatreda" , objs , SendMessageOptions.DontRequireReceiver);
		}
    }

	void	AcceptDamage(	ZMNetData	mData	)
	{
		MonsterID		=	mData.readInt();
		//Debug.Log( "K________OnAcceptDamage : " + MonsterID );
		MonsterNetView	m	=	GetMonsterNetViewByMonsterID(MonsterID);//这句干啥的，不知道，啥注释也没有，我就火大了！CZ
		if(	m!=null	)
		{
			string[]	StrData	=	new	string[2];
			StrData[0]			=	"" + mData.readInt();
			StrData[1]			=	mData.readString();

    //        string[] instensids = mData.getStrings();//仇恨列表中的玩家实例ID降序
    //        string[] monsterHate = mData.getStrings();//仇恨列表中的仇恨值降序，与上面一一对应

            //for (int i = 0; i < instensids.Length; i++)
            //{
            //    NGUIDebug.Log("-------instensids---------"+instensids[i]+"--------Hate"+monsterHate[i]);
            //}


			//Debug.Log( "K________OnAcceptDamage : " + MonsterID + " ; " + StrData[0] + " ; " + StrData[1] );
			m.SendMessage("OnAcceptDamage", StrData);	//发送消息处理指令//

            //object[] objs = new object[2];
            //objs[0] = instensids;
            //objs[1] = monsterHate;
			
            //m.SendMessage("ServerRemoveHatreda" , objs , SendMessageOptions.DontRequireReceiver);
		}
	}

	void	AcceptFallBack(	ZMNetData	mData	)
	{
		MonsterID		=	mData.readInt();
		//Debug.Log( "K__FallBack__FallBack_Fall_Fall_FallBack_OnAcceptFallBack : " + MonsterID );
		MonsterNetView	m	=	GetMonsterNetViewByMonsterID(MonsterID);
		if(	m	!=	null	)
		{
			string[]	StrData	=	new	string[2];
			StrData[0]	=	"" + mData.readInt();
			StrData[1]	=	mData.readString();
			//Debug.Log( "K________OnAcceptFallBack : MonsterID = " + MonsterID + " DID = "+StrData[0] + " ;  DData = " + StrData[1] );
			m.SendMessage( "OnAcceptDecision", StrData );	//发送消息处理指令//
		}
	}
	void	AcceptFallBack(	Zealm.OperationResponse	operationResponse	)
	{
		int[]	MonsterIDs	=	operationResponse.Parameters[0] as int[];
		int[]	DecisionIDs	=	operationResponse.Parameters[1] as int[];
		string[]	Datas	=	operationResponse.Parameters[2] as string[];
		for( int i = 0; i<MonsterIDs.Length; i++	)
		{
			MonsterNetView	m	=	GetMonsterNetViewByMonsterID(MonsterIDs[i]);
			if(	m	!=	null	)
			{
				string[]	StrData	=	new	string[2];
				StrData[0]	=	"" + DecisionIDs[i];
				StrData[1]	=	Datas[i];
				//Debug.Log( "K________OnAccetpDecision : MonsterID = " + MonsterID + " DID = "+StrData[0] + " ;  DData = " + StrData[1] );
				m.SendMessage( "OnAcceptDecision", StrData );	//发送消息处理指令//
			}
		}
	}
	void	AcceptMonsterDeath(	ZMNetData	mData	)
	{
		MonsterID		=	mData.readInt();
		//Debug.Log( "K____________________________________________________________挂了 : " + MonsterID );
		MonsterNetView	m	=	GetMonsterNetViewByMonsterID(	MonsterID	);
		if(	m	!=	null	)
		{
			int[]	RewardData	=	new	int[2];
			RewardData[0]	=	mData.readInt();
			RewardData[1]	=	mData.readInt();
			//Debug.Log( "K________OnAcceptFallBack : MonsterID = " + MonsterID + " DID = "+StrData[0] + " ;  DData = " + StrData[1] );
			m.SendMessage( "OnAcceptDeath", RewardData );	//发送消息处理指令//
		}
	}
	int				_PlayerID;
	MonsterNetView	_PetNetView;
	IEnumerator	AcceptSummonSkull(	ZMNetData	mData	)
	{
		_PlayerID		=	mData.readInt();
		MonsterID		=	mData.readInt();
		int	PetMaxHP	=	mData.readInt();
		string skullInfo = mData.readString();
		int playerInstanceID = mData.readInt();
		int MonsterIDUse = 0;
		MonsterIDUse = MonsterID;
	//	Debug.Log(	"K_____)(——+——+——+——+——+——+——+——+——+——+——返回骷髅ID:"+MonsterID	);

		if(PlayerUtil.isMine(playerInstanceID)){
			_PetNetView		=	FindPetNetViewBySummonerID(	_PlayerID	);

			if(	_PetNetView	!=	null	)
			{
		//		Debug.Log(	"K_____)(——+——+——+——+——+——+——+——+——+——+——返回骷髅ID找到了！ = "	+	MonsterID	);
				_PetNetView.MonsterID	=	MonsterIDUse;
				string	SData	=	""	+	MonsterIDUse;
		//		KDebug.Log(	"...................骷髅同步实例 = "	+	MonsterID,	_PetNetView.transform,	Color.yellow	);
				_PetNetView.SendMessage( "AcceptSummon", SData );
			}
		}
		else
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			GameObject go  = ObjectAccessor.getAOIObject(playerInstanceID);
			if(go){
				go.SendMessage("ReturnCallSkull" , string.Format("{0};{1};{2}" , skullInfo , MonsterIDUse , playerInstanceID) , SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	public	void	RegisterNewSkull(	MonsterNetView	NewSkull	)
	{
		PetNetViewList.Add( NewSkull );
	}

	public	GameObject	FindPetByID( int _MonsterID	)
	{
		MonsterNetView	mnv	=	FindPetNetViewByID( _MonsterID );
		if(	mnv != null	)
			return	mnv.gameObject;
		else
			return	null;
	}

	public	MonsterNetView	FindPetNetViewByID( int _MonsterID	)
	{
		for( int i = 0; i < PetNetViewList.Count; i++	)
		{
			if(	PetNetViewList[i]	!=	null	)
			{
				if( PetNetViewList[i].MonsterID	==	_MonsterID )
				{
					return	PetNetViewList[i];
				}
			}
			else
			{
				PetNetViewList.RemoveAt(i);
				i--;
			}
		}
		return	null;
	}

	public	MonsterNetView	FindPetNetViewBySummonerID( int _SummonerID	)
	{
		for( int i = 0; i < PetNetViewList.Count; i++	)
		{
			if(	PetNetViewList[i]	!=	null	)
			{
				if( PetNetViewList[i].SummonerID	==	_SummonerID	&&	PetNetViewList[i].MonsterID	==	0 )
				{
					return	PetNetViewList[i];
				}
			}
			else
			{
				PetNetViewList.RemoveAt(i);
				i--;
			}
		}
		return	null;
	}

	public	static	void	OnPlayerDead( int	_InstanceID )
	{
		GetInstance()._OnPlayerDead( _InstanceID );
	}

	public	void	_OnPlayerDead( int	_InstanceID )
	{
		for( int i = 0; i < MonsterNetViewList.Count; i++	)
		{
			if(	MonsterNetViewList[i]	!=	null	)
			{
				MonsterNetViewList[i].SendMessage("EnemyDead",_InstanceID );
			}
			else
			{
				MonsterNetViewList.RemoveAt( i );
				i--;
			}
		}
		for( int i = 0; i < PetNetViewList.Count; i++	)
		{
			if(	PetNetViewList[i]	!=	null	)
			{
				PetNetViewList[i].SendMessage("EnemyDead",_InstanceID );
			}
			else
			{
				PetNetViewList.RemoveAt( i );
				i--;
			}
		}
	}
		
}

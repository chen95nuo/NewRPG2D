using UnityEngine;
using System.Collections;

public	class TMonsterHandler	:	ServerMonoBehaviour
{
	/// <summary>
	/// 单例模式控制场景 -
	/// </summary>
	public	static	TMonsterHandler	GetInstance(	GameObject	_TMC	)
	{
		if( InstObj	==	null )
		{
			InstObj		=	new	GameObject( "TMonsterHandler" );
			Instance	=	InstObj.AddComponent<TMonsterHandler>();
		}
		Instance.TMC	=	_TMC;
		return	Instance;
	}
	private	static	GameObject			InstObj		=	null;
	private	static	TMonsterHandler		Instance	=	null;

	public	GameObject		TMC;

	protected	override	void	OnOperationResponse(	Zealm.OperationResponse	operationResponse	)
	{
		switch(	(OpCode)operationResponse.OperationCode	)
		{
			case OpCode.DefenceBattleStartCommit:
				{
					switch (operationResponse.ReturnCode)
					{
						case (short)yuan.YuanPhoton.ReturnCode.Yes:
							{
								long diffTime	=	(long)operationResponse.Parameters[(short)0];
								TMC.SendMessage("OnStart",""+diffTime);
							}
							break;
						case (short)yuan.YuanPhoton.ReturnCode.Error:
							{
								Debug.LogError(StaticLoc.Loc.Get(operationResponse.DebugMessage));
							}
							break;
					}
				}
				break;
			case OpCode.BallCommitDamage:
				{
					switch (operationResponse.ReturnCode)
					{
						case (short)yuan.YuanPhoton.ReturnCode.Yes:
							{
								int damage = (int)operationResponse.Parameters[(short)0];
								TMC.SendMessage("OnAcceptHP",""+damage);
							}
							break;
						case (short)yuan.YuanPhoton.ReturnCode.Error:
							{
								Debug.LogError(StaticLoc.Loc.Get(operationResponse.DebugMessage));
							}
							break;
					}
				}
				break;
		}
	}
}

using	UnityEngine;
using	System.Collections;
using	System.Collections.Generic;

/// <summary>
/// 网络层控制怪物生成点的实例ID -
/// </summary>
public	class	SpawnPointNetView	:	MonoBehaviour 
{
	public	int		CurrentSpawnPointID;
	private	MonsterSpawnPointHandler	SpawnPointHandler;
	void	Awake()
	{
		SpawnPointHandler	=	MonsterSpawnPointHandler.GetInstance();
	}

	/// <summary>
	/// 获取当前的玩家数量 -
	/// </summary>
	public	int	GetNumPlayer()
	{
		return	SpawnPointHandler.CurrentPlayerCount;
	}
	/// <summary>
	///	已经生成了的怪的ID -
	/// </summary>
	private	List<int>	SpawnedPVPMonster	=	new	List<int>();
	/// <summary>
	/// 记录已经生成了的怪物-
	/// </summary>
	public	void	AddMonster(	int	ID	)
	{
		SpawnedPVPMonster.Add(ID);
	}
	/// <summary>
	/// 这个怪是不是已经生成了 -
	/// </summary>
	public	bool	IsSpawned(	int ID	)
	{
		return	SpawnedPVPMonster.Contains( ID );
	}

	/// <summary>
	/// 世界boss场景下已经生成的怪物 -
	/// </summary>
	private	List<GameObject>	SpawnWorldBossMonster	=	new	List<GameObject>();
	/// <summary>
	/// 记录一个已经生成的怪物 -
	/// </summary>
	public	void	AddWorldBossSpawnedMonster( GameObject	MGO )
	{
		SpawnWorldBossMonster.Add( MGO );
	}
	/// <summary>
	/// 获取尚存的怪物数量 -
	/// </summary>
	public	int	GetWorldBossSpawnedMonsterCount()
	{
		for( int i = 0; i < SpawnWorldBossMonster.Count; i++ )
		{
			if( SpawnWorldBossMonster[i] == null )
			{
				SpawnWorldBossMonster.RemoveAt( i );
				i--;
			}
		}
		return	SpawnWorldBossMonster.Count;
	}
}

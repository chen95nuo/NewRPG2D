using UnityEngine;
using System.Collections;

/// <summary>
/// 怪物与Handler控制的接口 -
/// </summary>
public	class	MonsterNetView	:	MonoBehaviour 
{
	public	int	MonsterID	=	0;

	/// <summary>
	/// 召唤者ID - 给召唤兽用的 - 记录召唤这个骷髅的玩家的实例ID，或者另一个怪招雪怪 -
	/// </summary>
	public	int	SummonerID	=	-1;

}

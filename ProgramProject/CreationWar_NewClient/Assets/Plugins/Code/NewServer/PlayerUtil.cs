using	UnityEngine;
using	System.Collections;
using	System.Collections.Generic;
public	static	class PlayerUtil
{
	/// <summary>
	/// 本地的独立实例ID，用于机器人、木桶、陷阱这类本地和临时本地管理的物体获取相对独立 的实例ID -
	/// </summary>
	private	static	int	LocalInstanceID;
    public static int mapInstanceID = -1;
	public	static	int	myID
	{
		set
		{
			  MyID	=	value;
		}
		get
		{	return	MyID;	}
	}
	private	static	int	MyID;

	private	static	List<int>	LocalPlayerObject	=	new	List<int>();
	/// <summary>
	/// 判断是不是自己控制的主角 -	
	/// </summary>
	/// <returns><c>true</c>, if mine was ised, <c>false</c> otherwise.</returns>
	/// <param name="instanceID">Instance I.</param>
	public	static	bool	isMine(int instanceID)
    {
        return	myID	==	instanceID;
    }

	/// <summary>
	/// 注册一个玩家召唤的替身或者召唤兽 ，可用作判断他是不是这台机器控制 -
	/// </summary>
	public	static	void	RegisterNewLocalObject(	int	InstanceID	)
	{
		if(	!LocalPlayerObject.Contains(InstanceID)	)
			LocalPlayerObject.Add(InstanceID);
	}

	public	static	void	UnregisterLocalObject(	int	InstanceID	)
	{
		LocalPlayerObject.Remove(InstanceID);
	}

	/// <summary>
	/// 判断这个怪物是不是隶属于本地管理 -
	/// </summary>
	public	static	bool	IsLocalObject(	int	InstanceID	)
	{
		if(	MyID	==	InstanceID)
			return	true;
		if(	LocalPlayerObject.Contains(	InstanceID	)	)
			return	true;
		if(	999999	==	InstanceID	)	//陷阱的实例ID//
			return	true;
		if(	WorldBossInstance	==	InstanceID	)	//WorldBoss的实例ID//
			return	true;
		return	false;
	}
	/// <summary>
	/// 获取新的本地实例ID -
	/// </summary>
	public	static	int	GetNewLocalInstanceID()	
	{
		LocalInstanceID	--	;
		return	LocalInstanceID;
	}
	/// <summary>
	/// 世界boss实例是本地实例 -
	/// </summary>
	public	static	int	WorldBossInstance	=	int.MinValue;

    /// <summary>
    /// 正在排队的战场的id
    /// </summary>
    public static string battlefieldId = "";
}

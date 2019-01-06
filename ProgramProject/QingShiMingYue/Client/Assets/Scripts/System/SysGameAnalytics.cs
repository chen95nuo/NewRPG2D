using System;
using System.Collections.Generic;
using ClientServerCommon;

/// <summary>
/// 游戏类型
/// </summary>
public class GameRecordType : TypeNameContainer<GameRecordType>
{
	public const int StartGame = 0;					// 开始游戏
	public const int CheckUpdate = 1;				// 检查更新
	public const int CheckUpdateResult = 2;			// 检查更新结果
	public const int StartUpdateConfig = 3;			// 开始更新配置文件
	public const int FinishUpdateConfig = 4;		// 结束更新配置文件
	public const int StartUpdateAsset = 5;			// 开始更新资源
	public const int FinishUpdateAsset = 6;			// 结束更新资源
	public const int StartUpdateApp = 7;			// 开始更新app
	public const int PlatformLogin = 8;				// 开始登陆 平台登陆
	public const int PlatformLoginSuccess = 9;		// 平台登陆成功
	public const int GameLogin = 10;				// 游戏登录
	public const int GameLoginSuccess = 11;			// 游戏登录成功
	public const int ShowAreaList = 12;				// 显示服务器列表
	public const int EnterGame = 13;				// 选区进入
	public const int QueryInitInfo = 14;			// 请求玩家数据
	public const int QueryInitInfoSuccess = 15;	// 请求玩家数据成功
	public const int Tutorial = 16;					// 新手记录
	public const int ShowSelectAvatar = 17;			// 进入选择角色界面
	public const int SetPlayerLevel = 18;			// 记录玩家等级

	public static bool Initialize()
	{
		bool result = false;

		result &= RegisterType("StartGame", StartGame);
		result &= RegisterType("CheckUpdate", CheckUpdate);
		result &= RegisterType("CheckUpdateResult", CheckUpdateResult);
		result &= RegisterType("StartUpdateConfig", StartUpdateConfig);
		result &= RegisterType("FinishUpdateConfig", FinishUpdateConfig);
		result &= RegisterType("StartUpdateAsset", StartUpdateAsset);
		result &= RegisterType("FinishUpdateAsset", FinishUpdateAsset);
		result &= RegisterType("StartUpdateApp", StartUpdateApp);
		result &= RegisterType("PlatformLogin", PlatformLogin);
		result &= RegisterType("PlatformLoginSuccess", PlatformLoginSuccess);
		result &= RegisterType("GameLogin", GameLogin);
		result &= RegisterType("GameLoginSuccess", GameLoginSuccess);
		result &= RegisterType("ShowAreaList", ShowAreaList);
		result &= RegisterType("EnterGame", EnterGame);
		result &= RegisterType("QueryInitInfo", QueryInitInfo);
		result &= RegisterType("QueryInitInfoSuccess", QueryInitInfoSuccess);
		result &= RegisterType("Tutorial", Tutorial);
		result &= RegisterType("ShowSelectAvatar", ShowSelectAvatar);
		result &= RegisterType("SetPlayerLevel", SetPlayerLevel);

		return result;
	}
}

/// <summary>
/// 游戏统计输出接口, 所有需要接收统计的模块都要实现这个接口
/// </summary>
public interface IAnalyticOutput
{
	void RecordGameData(string value);
}

/// <summary>
/// 游戏统计模块, 用于统计游戏内容
/// </summary>
public class SysGameAnalytics : SysModule
{
	public static SysGameAnalytics Instance 
	{ 
		get { return SysModuleManager.Instance.GetSysModule<SysGameAnalytics>(); } 
	}

	private List<IAnalyticOutput> analytics = new List<IAnalyticOutput>();

	public override bool Initialize()
	{
		GameRecordType.Initialize();
		return base.Initialize();
	}

	public void AddAnalytic(IAnalyticOutput analytic)
	{
		if (analytics.Contains(analytic))
			return;

		analytics.Add(analytic);
	}

	public void RemoveAnalytic(IAnalyticOutput analytic)
	{
		analytics.Remove(analytic);
	}

	public void RecordGameData(int recordType, params object[] datas)
	{
		var sb = new System.Text.StringBuilder();
		sb.Append(GameRecordType.GetNameByType(recordType));

		if (datas != null)
		{
			for (int i = 0; i < datas.Length; i++)
			{
				sb.Append("_");
				sb.Append(datas[i].ToString());
			}
		}

		for (int i = 0; i < analytics.Count; i++)
			analytics[i].RecordGameData(sb.ToString());
	}
}

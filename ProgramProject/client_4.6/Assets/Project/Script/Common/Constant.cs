using UnityEngine;
using System.Collections;

public class Constant
{
	public const float CriMulRate=2F;//暴击倍率//
	public const int MaxRound=30;//最大回合数//
	public const int CardNum=6;
	public const int MaxGold=10*10000*10000;/**最大金币数10亿**/
	public const int MaxCrystal=10*10000*10000;/**最大水晶数10亿**/
	public const int RunePageLength=6;
	public const int MaxPower=120;
	public const int AutoRestorePowerTime=5*60;/**自动回复体力间隔(秒)**/
	public const int MaxBreakNum1=2;//==第二天赋需要突破次数==//
	public const int MaxBreakNum2=5;//==第三天赋需要突破次数==//
	public const int InitMaxEnergy=200;/**初始怒气上限**/
	/**客户端操作系统**/
    public const string OS_ANDROID="android";
    public const string OS_IOS="ios";
	public const string OS_PC="pc";
}

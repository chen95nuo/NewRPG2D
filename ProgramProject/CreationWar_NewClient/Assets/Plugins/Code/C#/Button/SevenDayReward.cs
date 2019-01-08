using UnityEngine;
using System.Collections;
using System;

public class SevenDayReward : MonoBehaviour {
	public static SevenDayReward sevenDayReward;
	public BtnDay btnDay;
	public BtnGetReward btnGetReward;

	public PanelDailyBenefits DailyReward;
	private enum SevenDays
	{
		NoDay= -1,
		OneDay = 0,
		TwoDay,
		ThreeDay,
		FourDay,
		FiveDay,
		SixDay,
		SevenDay
		
	}
	
	private SevenDays someDay = SevenDays.NoDay;
	void Awake()
	{
		sevenDayReward = this;
		someDay = (SevenDays)Enum.Parse(typeof(SevenDays), gameObject.name);

        //int haveGotReward = int.Parse(BtnGameManager.yt[0]["CanDailyBenefits"].YuanColumnText);
        //int canRewadDays = int.Parse(BtnGameManager.yt[0]["DailyBenefits"].YuanColumnText);
        //Debug.Log("seven day ============================" + haveGotReward + "------" + canRewadDays);
        //PanelDailyBenefits.panelDailyBenefits.SetBtnState(haveGotReward, canRewadDays);
	}

	void OnEnable()
	{
        PanelDailyBenefits.panelDailyBenefits.SetBtnState(int.Parse(BtnGameManager.yt[0]["CanDailyBenefits"].YuanColumnText), int.Parse(BtnGameManager.yt[0]["DailyBenefits"].YuanColumnText));
	}

	public void ShowItemInfo()
	{
		DailyReward.RewardRelevant((int)someDay);
	}

//	public  void SetBtnState(int haveGotReward, int canRewadDays)
//	{
//		int index = (int)someDay + 1;
//
//		if(index <= haveGotReward)
//		{
//			HaveReward();// 已领取
//			Debug.Log("......................."+index+"........................"+haveGotReward);
//		}
//		else if(index > haveGotReward && index <= canRewadDays)
//		{
//			Unreward();// 可领取但未领取的
//		}
//		else if(index > canRewadDays && index < 8)
//		{
//			DisableReward();// 不可领取
//		}
//	}

	public  void HaveReward()
	{
		btnDay.btnEnter.Disable=true;
		btnGetReward.SwitchBtnStated(BtnStated.End);
		btnDay.SwitchBtnReStated(BtnRewardStated.HaveReward);

	}

	public void Unreward()
	{
		btnGetReward.SwitchBtnStated(BtnStated.Receive);
		btnDay.SwitchBtnReStated(BtnRewardStated.Receive);

	}

	public void DisableReward()
	{
		btnDay.btnEnter.Disable = false;
		btnDay.SwitchBtnReStated(BtnRewardStated.NoReward);

	}
}

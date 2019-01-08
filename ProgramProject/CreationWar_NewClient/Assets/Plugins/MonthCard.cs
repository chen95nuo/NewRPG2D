/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System;
using System.Collections;

public enum CardBtnState
{
    None,
    BuyCard,// 购买月卡
    CanReward,// 能够领取
    CantReward,// 不可领取
}

public class MonthCard : MonoBehaviour {
    public static MonthCard monthCard;
    private UIButton rewardBtn;// 购买月卡和领取奖励按钮
    public UILabel btnLabel;// 按钮上显示的字：购买月卡或者领取福利
    public UILabel rewardLbl;// 用来显示：激活或未激活
    public UILabel timesLbl;// 剩余次数
    private CardBtnState btnState;
    private string isPaycard;
    private string isReceiveCardreward;
    private string activeTime; // 有效期
    private int remainTimes;

    void Awake()
    {
        monthCard = this;
        btnState = CardBtnState.BuyCard; // 初始化变量

        rewardBtn = btnLabel.transform.parent.GetComponent<UIButton>();
	}
	
	void OnEnable()
	{
		isPaycard = BtnGameManager.yt[0]["isPaycard"].YuanColumnText.Trim();//isPaycard每次上线的时候判断这个字段是否为2 如果为2 按钮状态切换为立即生产状态 其余 则是购买原石
        isReceiveCardreward = BtnGameManager.yt[0]["isReceiveCardreward"].YuanColumnText.Trim();//isReceiveCardreward这个字段表示可以生产的次数，如果这个次数小于1，则置为购买原石状态
        activeTime = BtnGameManager.yt[0]["PaycardTime"].YuanColumnText.Trim();// 冷却时间

        if (isPaycard.Equals("2"))// 如果为2 按钮状态切换为立即领取状态,其余则是购买原石状态	
        {
            SetBtnState(CardBtnState.CanReward);

            SetLabelTex(true, activeTime);
        }
        else
        {
            SetBtnState(CardBtnState.BuyCard);

            SetLabelTex(false, null);
        }

        SetTimesTxt(isReceiveCardreward);
    }

    public void SetBtnState(CardBtnState bs)
    {
        btnState = bs;

        if (bs == CardBtnState.BuyCard)
        {
            btnLabel.text = StaticLoc.Loc.Get("meg0127");

            if (!rewardBtn.isEnabled)
            {
                rewardBtn.isEnabled = true;
            }
        }
        else if (bs == CardBtnState.CanReward)
        {
            btnLabel.text = StaticLoc.Loc.Get("meg0128");

            if (!rewardBtn.isEnabled)
            {
                rewardBtn.isEnabled = true;
            }
        }
        else if (bs == CardBtnState.CantReward)// CD中
        {
            btnLabel.text = StaticLoc.Loc.Get("meg0128");

            if (rewardBtn.isEnabled)
            {
                rewardBtn.isEnabled = false;
            }
        }
    }

    public void SetLabelTex(bool isBuy, string time)
    {
        if (isBuy)
        {
            //rewardLbl.text = StaticLoc.Loc.Get("meg0125");
            DateTime dt;
            if (DateTime.TryParse(time, out dt))
            {
                dt.AddMinutes(1.0f);
                int tempMinutes = (dt - InRoom.GetInRoomInstantiate().serverTime).Milliseconds;
                double tempTime = (dt - InRoom.GetInRoomInstantiate().serverTime).TotalHours;

                if (tempMinutes > 0)
                {
                    SetBtnState(CardBtnState.CantReward);

                    if (tempTime > 0 && tempTime < 1.0f)
                    {
                        rewardLbl.text = ((int)(tempTime * 60)).ToString() + StaticLoc.Loc.Get("messages045"); // 显示cd时间，精确到分钟
                    }
                    else
                    {
                        rewardLbl.text = ((int)tempTime).ToString() + StaticLoc.Loc.Get("info988"); // 显示cd时间，精确到小时
                    }
                }
                else
                {
                    rewardLbl.text = StaticLoc.Loc.Get("meg0125");// 可以炼制
                }
            }
            else
            {
                rewardLbl.text = StaticLoc.Loc.Get("meg0125");// 可以炼制
            }
        }
        else
        {
            rewardLbl.text = StaticLoc.Loc.Get("meg0126"); // 未激活
        }
    }

    /// <summary>
    /// 设置剩余次数
    /// </summary>
    public void SetTimesTxt(string times)
    {
        if (string.IsNullOrEmpty(times))
        {
            timesLbl.text = "0";

            SetBtnState(CardBtnState.BuyCard);
        }
        else
        {
            timesLbl.text = times;

            if (times.Trim().Equals("0"))
            {
                SetBtnState(CardBtnState.BuyCard);
                SetLabelTex(false, null);
            }
        }
    }

    /// <summary>
    /// 按钮点击时调用的方法
    /// </summary>
    public void BtnClick()
    {
        if (btnState == CardBtnState.BuyCard)
        {
			InRoom.GetInRoomInstantiate().Rechargecard(TableRead.strPageName);
            //PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("StoreMoveOn", SendMessageOptions.DontRequireReceiver);
        }
        else if (btnState == CardBtnState.CanReward)
        {
            PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().Receivemonthlybenefits());
        }
        else if (btnState == CardBtnState.CantReward)
        {
            PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("meg0124"));
        }
    }
}

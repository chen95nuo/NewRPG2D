using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivityControl : MonoBehaviour {
	public static ActivityControl activityControl;
	public UILabel activityInfoLabel;
	public UILabel activityConditionLabel;
//	public UILabel chargeDaysLabel;
	public UILabel rankingLabel;

    public UILabel lblCondition;
	
	public List<SpriteForBenefits> equipItems;
	
	public yuan.YuanPhoton.ActivityType activityTpye = yuan.YuanPhoton.ActivityType.ActivityLevel;
	
	void Awake()
	{
		activityControl = this;
	}
	
	private YuanRank ytRanking=new YuanRank("ActivityControlRank");
	void OnEnable () {
		
		InRoom.GetInRoomInstantiate().GetActivityInfo(activityTpye);

        // FIXME: 2014.8.14注释掉的，排行榜移植完后需要打开。这里获取某个排行榜，因为会不断转圈，现在需要测试活动，所以暂时注释掉
		// StartCoroutine (PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate ().GetRankOne (yuan.YuanPhoton.RankingType.Exp,BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,ytRanking)));
		
        StartCoroutine (ShowRankingNow());
	}
	
	// Update is called once per frame
	void Update () {
        //if(hideObj1.active) hideObj1.SetActiveRecursively(false);
        //if(hideObj2.active) hideObj2.SetActiveRecursively(false);
	}
	
	/// <summary>
	/// 显示活动相关
	/// </summary>
	public void ShowActivity(string activityInfo, string activityTime,/* string chargeDays,*/ Dictionary<int, string> activityReward)
	{
		if(null != activityInfoLabel)
		{
			//activityInfoLabel.text = activityInfo;
		}
		if(null != activityConditionLabel)
		{
			//activityTimeLabel.text = activityTime;
		}
//		if(null != chargeDaysLabel)
//		{
//			chargeDaysLabel.text = chargeDays;
//		}
		if(null!=activityReward)
		{
			//ShowRewardItems(activityReward);
		}
//		int num=0;
//		foreach(string item in activityReward.Values)
//		{
//			equipItems[num].RefreshIcon(item);
//			num++;
//			if(num>=3)
//			{
//				break;
//			}
//		}
		
		
//		for(int i=0;i<activityReward.Count;i++)
//		{
//			if(i<equipItems.Count)
//			{
//				equipItems[i].itemID = activityReward[i];
//			}
//		}
	}
	
	/// <summary>
	/// 立即充值
	/// </summary>
	public void ChargeNow()
	{
		
	}
	
	/// <summary>
	/// 获取奖励
	/// </summary>
	public void GetReward()
	{
		StartCoroutine ( PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate().GetActivityReward(activityTpye)));
	}
	
	/// <summary>
	/// 显示奖励物品
	/// </summary>
	void ShowRewardItems(Dictionary<int, string> activityReward)
	{
		int ranking = ytRanking.myRank;
		int lvl = int.Parse(BtnGameManager.yt[0]["PlayerLevel"].YuanColumnText);
		string reward=string.Empty;
		if(1 == ranking)
		{
			equipItems[0].RefreshIcon(activityReward[1]);
		}
		else if(2 == ranking)
		{
			equipItems[0].RefreshIcon(activityReward[2]);
		}
		else if(3 == ranking)
		{
			equipItems[0].RefreshIcon(activityReward[3]);
		}
		else if(activityReward.TryGetValue (lvl/10*10,out reward))
		{
			equipItems[0].RefreshIcon(reward);
		}
		else
		{
			equipItems[0].RefreshIcon(activityReward[20]);
		}
	}
	
	
	/// <summary>
	/// 显示玩家当前排名
	/// </summary>
	public IEnumerator ShowRankingNow()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.1f);
			if(!ytRanking.IsUpdate)
			{
				break;
			}
		}
		rankingLabel.text = ytRanking.myRank.ToString ();
	}

    /// <summary>
    /// 显示活动相关
    /// </summary>
    /// <param name="activityInfo">活动内容</param>
    /// <param name="activityCondition">活动条件</param>
    /// <param name="activityReward">活动奖励</param>
    public void ShowActivityInfo(string activityInfo, string activityCondition, string victoryCondition, string[] activityReward)
    {
        if (null != activityInfoLabel)
        {
            activityInfoLabel.text = activityInfo;
        }
        else
        {
            activityInfoLabel.text = "";
        }

        if (null != activityConditionLabel)
        {
            activityConditionLabel.text = activityCondition;
        }
        else
        {
            activityConditionLabel.text = "";
        }

        if (null != lblCondition)
        {
            lblCondition.text = victoryCondition;
        }
        else
        {
            lblCondition.text = "";
        }

        if (null != activityReward && activityReward.Length >= 3)
        {
            if (!string.IsNullOrEmpty(activityReward[0]))
            {
                equipItems[0].RefreshIcon(activityReward[0]);
            }
            if (!string.IsNullOrEmpty(activityReward[1]))
            {
                equipItems[1].RefreshIcon(activityReward[1]);
            }
            if (!string.IsNullOrEmpty(activityReward[2]))
            {
                equipItems[2].RefreshIcon(activityReward[2]);
            } 
        }
        else
        {
            foreach (SpriteForBenefits sfb in equipItems)
            {
                sfb.spriteBenefits = null;
                sfb.lblNum.text = "";
            }
        }
    }
}

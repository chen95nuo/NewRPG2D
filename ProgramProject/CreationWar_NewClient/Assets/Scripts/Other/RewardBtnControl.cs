using UnityEngine;
using System.Collections;

/// <summary>
/// 控制奖励按钮是否闪
/// </summary>
public class RewardBtnControl : MonoBehaviour {
    public UISprite rewardSprite;
	public UISprite picRedPoint;

    IEnumerator Start()
    {

        rewardSprite.enabled = false;

		if(null!=picRedPoint)
		{
			picRedPoint.enabled=false;
		}
        OnEnable();

        yield return new WaitForSeconds(1.0f);

        OnEnable();
    }

    private bool canGetReward;
    void OnEnable()
    {
        canGetReward = CanGetReward();

        //Debug.Log("1-----------------" + BtnGameManager.yt[0]["DailyBenefits"].YuanColumnText + "::::" + BtnGameManager.yt.Rows[0]["CanDailyBenefits"].YuanColumnText);
        //Debug.Log("2---------------------------" + BtnGameManager.yt.Rows[0]["CanSalaries"].YuanColumnText);
        //Debug.Log("3---------------------------" + canGetReward);
        //Debug.Log("4---------------------------" + BtnGameManager.yt.Rows[0]["CanRankBenefits"].YuanColumnText + "-------" + BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText);
        //Debug.Log("5---------------------------" + BtnGameManager.yt.Rows[0]["CanGuildBenefits"].YuanColumnText + "-----" + BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText);

        if (int.Parse(BtnGameManager.yt[0]["DailyBenefits"].YuanColumnText) > int.Parse(BtnGameManager.yt.Rows[0]["CanDailyBenefits"].YuanColumnText)
            || BtnGameManager.yt.Rows[0]["CanSalaries"].YuanColumnText == "1"
            || canGetReward
            //|| (BtnGameManager.yt.Rows[0]["CanRankBenefits"].YuanColumnText == "1" && BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText != "" && BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText != "0")
            //|| (BtnGameManager.yt.Rows[0]["CanGuildBenefits"].YuanColumnText == "1" && BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText != "" && BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText != "0")
            )
        {
            rewardSprite.enabled = true;
			picRedPoint.enabled=true;
        }
        else
        {
            rewardSprite.enabled = false;
			picRedPoint.enabled=false;
        }
    }

    private string[] boxesState;
    private bool CanGetReward()
    {
        boxesState = BtnGameManager.yt.Rows[0]["OpenedChests"].YuanColumnText.Split(',');

        foreach(string str in boxesState)
        {
            if(str.Trim().Equals("1"))
            {
                return true;
            }
        }

        return false;
    }
}

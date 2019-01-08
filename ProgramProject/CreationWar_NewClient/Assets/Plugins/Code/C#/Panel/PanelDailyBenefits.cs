using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelDailyBenefits : MonoBehaviour {
	public static PanelDailyBenefits panelDailyBenefits;

	private Dictionary<int,string> dicDailyBenefits;
	void Awake()
	{

		panelDailyBenefits = this;
		//dicDailyBenefits=((Dictionary<object,object>)YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.DailyBenefits]).DicObjTo<int, string>();     
        dicDailyBenefits = ((Dictionary<object, object>)YuanUnityPhoton.dicBenefitsInfo[(short)yuan.YuanPhoton.BenefitsType.DailyBenefits]).DicObjTo<int, string>();
		
	}

	//七日奖励紫装
	public List<BtnDay> listBtnDay ;
	//每日奖励物品
	public List<ShowReward> listReward;
	//每日奖励下方显示某一天的领取
	public void OnEnable()
	{
		DisableBtnDay ();
		SetBtn ();
		RewardRelevant((int.Parse(BtnGameManager.yt[0]["DailyBenefits"].YuanColumnText) - 1));

		if(int.Parse(BtnGameManager.yt[0]["DailyBenefits"].YuanColumnText)==7)
		{
			return;
		}
		SetPicGround();


	}

	//	DailyBenefits可领取的总天数 DailyBenefits-1当天的
	//CanDailyBenefits已经领取的天数

//	public void ShowGetRewardBtn()
//	{

//		for(int i = 0; i<listBtnDay.Count;i++)
//		{
//			if(listBtnDay[i].btnEnter.picQualit.enabled==true&&listBtnDay[i].btnEnter.picEnabled.enabled==true){
//				listBtnDay[i].btnEnter.picEnabled = BtnGetReward.btnGetReWard.TodayReward ;
//				break;
//			}
//		}
//	}

	public  void SetBtnState(int haveGotReward, int canRewadDays)
	{
		for(int i=0;i<listBtnDay.Count;i++)
		{
			int index = i + 1;
			if(index <= haveGotReward)
			{
				listBtnDay[i].gameObject.GetComponentInChildren<SevenDayReward>().HaveReward();
			}
			else if(index > haveGotReward && index <= canRewadDays)
			{
				listBtnDay[i].gameObject.GetComponentInChildren<SevenDayReward>().Unreward();

			}
			else if(index > canRewadDays && index < 8)
			{
				listBtnDay[i].gameObject.GetComponentInChildren<SevenDayReward>().DisableReward();
			}
		}
	}

    private void DisableBtnDay()
    {
		for(int g = 0; g<listBtnDay.Count; g++)
        {

//            item.btnEnter.Disable = true;
//            item.btnShow.Disable = true ;
			string[] reward = GetSomeDayReward(g,5);
			if (reward[0] == "1")
			{
				string itemID = reward[1];
				int itemCount = int.Parse(reward[2]);
				string str = string.Format("{0},{1}",itemID,itemCount.ToString());
//				Debug.Log("0000000000000000000000000--------------------------"+str);
				object[] parms = new object[4];
				parms[0] = str;
				parms[1] = listBtnDay[g].btnEnter.picEnabled;
				parms[2] = listBtnDay[g].btnEnter.picQualit;
				parms[3] = null;
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("SetItemIconAsID", parms, SendMessageOptions.DontRequireReceiver);
			}

        }
    }

	public bool IsGetReward ;
	public void SetPicGround()
	{

		if(IsGetReward){
			StartCoroutine(SetGetRewardPic(int.Parse(BtnGameManager.yt[0]["DailyBenefits"].YuanColumnText)-1));
		}else{
			StartCoroutine(SetGetRewardPic(int.Parse(BtnGameManager.yt[0]["DailyBenefits"].YuanColumnText)));
		}

	}
	IEnumerator SetGetRewardPic(int g){
		yield return new WaitForSeconds(0.1f);
		string[] reward = GetSomeDayReward(g,5);
		if (reward[0] == "1")
		{
			string itemID = reward[1];
			int itemCount = int.Parse(reward[2]);
			string str = string.Format("{0},{1}",itemID,itemCount.ToString());
			//				Debug.Log("0000000000000000000000000--------------------------"+str);
			object[] parms = new object[4];
			parms[0] = str;
			parms[1] = BtnGetReward.btnGetReWard.TodayReward;
			parms[2] = BtnGetReward.btnGetReWard.GetRewardGround;
			parms[3] = null;
			PanelStatic.StaticBtnGameManager.invcl.SendMessage("SetItemIconAsID", parms, SendMessageOptions.DontRequireReceiver);
		}
	}

    private void SetBtn()
    {
		
        
        	
//            if (BtnGameManager.yt.Rows[0]["CanDailyBenefits"].YuanColumnText.Trim() == "1")
//            {
//                int num = int.Parse(BtnGameManager.yt.Rows[0]["DailyBenefits"].YuanColumnText);
//				if(listBtnDay.Count>=num&&num>0)
//				{
//                	listBtnDay[num - 1].btnEnter.Disable = false;
//                	listBtnDay[num - 1].btnShow.Disable = false;
//				}
//            }
			
//		for(int i=0;i<listBtnDay.Count;i++)
//		{
//			listBtnDay[i].lblGold.text=dicDailyBenefits[i].Split (',')[0];
//			listBtnDay[i].lblBlood.text=dicDailyBenefits[i].Split (',')[1];
//			if(listBtnDay[i].lblGold.text=="0")
//			{
//				listBtnDay[i].tranGold.gameObject.SetActive (false);
//			}
//			else
//			{
//				listBtnDay[i].tranGold.gameObject.SetActive (true);
//			}
//
//			if(listBtnDay[i].lblBlood.text=="0")
//			{
//				listBtnDay[i].tranBlood.gameObject.SetActive (false);
//			}
//			else
//			{
//				listBtnDay[i].tranBlood.gameObject.SetActive (true);
//			}
//		}
//        
    }

    private IEnumerator GetDailyBenefitsBtn(GameObject obj)
    {
        
        BtnDisable disable = obj.GetComponent<BtnDisable>();
        if (disable != null&&!disable.Disable)
        {

            BtnGameManager.yt.Rows[0]["AimGetLogin"].YuanColumnText = (int.Parse(BtnGameManager.yt.Rows[0]["AimGetLogin"].YuanColumnText) + 1).ToString();
            while (BtnGameManager.yt.IsUpdate)
            {
                yield return new WaitForSeconds(0.1f);
            }
            InRoom.GetInRoomInstantiate().UpdateYuanTable("DarkSword2", BtnGameManager.yt,SystemInfo.deviceUniqueIdentifier);
            InRoom.GetInRoomInstantiate().GetDailyBenefits();
            
//			disable.Disable = true;
      
		}
    }

	public void RewardRelevant(int day){
		for(int i = 0;i<listReward.Count;i++){
			string[] reward = GetSomeDayReward(day,i);
			if(null == reward)
			{
				return;
			}

			if (reward[0] == "1")
			{
				string itemID = reward[1];
				string itemCount = reward[2];
				string str = string.Format("{0},{1}",itemID,itemCount);
				object[] parms = new object[4];
				parms[0] = str;
				parms[1] = listReward[i].Reward;
				parms[2] = listReward[i].ItemBox;
				parms[3] = listReward[i].reWardName;
				PanelStatic.StaticBtnGameManager.invcl.SendMessage("SetItemIconAsID", parms, SendMessageOptions.DontRequireReceiver);
				listReward[i].itemID = itemID;
			}
			else if (reward[0] == "2")
			{

				if (reward[1] == "1")
				{
					listReward[i].Reward.spriteName = "Gold";
					listReward[i].itemID = "";
				}
				else if (reward[1] == "2")
				{
					listReward[i].Reward.spriteName = "UIP_Boodstone";
					listReward[i].itemID = "";
				}
				else if (reward[1] == "3")
				{
					listReward[i].Reward.spriteName = "624";
					listReward[i].itemID = "";
					
				}
				
				int num = int.Parse(reward[2]);
				listReward[i].reWardName.text = num.ToString();
			}
		}
	}

	public string[] GetSomeDayReward(int day, int index)
	{
//		for(int k=0;k<dicDailyBenefits.Count-1;k++){
			string rewardsStr = string.Empty;
			if (dicDailyBenefits.TryGetValue(day, out rewardsStr))
			{
				string[] rewards =  rewardsStr.Split(';');	
				return rewards[index].Split(',');
			}
		return null;
//		}
	}

	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FristTimeReward : MonoBehaviour {
	public List<ShowReward> listReward;
    public static string[] rewards;

    public BoxCollider getRewardsBtn;

	public UISlicedSprite ISGetReward;

	public static FristTimeReward fristTimeReward;
	void Start(){
		fristTimeReward = this;
	}

    void OnEnable()
    {
        if(null != rewards)
            FristReward(rewards);

        int servingMoney = int.Parse(BtnGameManager.yt[0]["ServingMoney"].YuanColumnText);
        if (servingMoney > 0)
        {
            EnableGetRewardBtn(true);
			ISGetReward.enabled = true;
        }
        else
        {
            EnableGetRewardBtn(false);
			ISGetReward.enabled = false;
        }
        
    }

	public void FristReward(string[] ItemIds){

        if (null == listReward)
        {
            return;
        }

		for(int i = 0; i<listReward.Count ; i++){
			string[] itemInfo = ItemIds[i].Split(',');
			string itemID = itemInfo[0];
            string itemCount = itemInfo[1].Length > 1 ? itemInfo[1] : string.Format("0{0}", itemInfo[1]);// 物品数量必须是两位或两位以上

            if (itemID.Equals("1")) // 1表示金币 
            {
                listReward[i].Reward.spriteName = "Gold";
				listReward[i].reWardName.text = itemCount;
            }
            else if (itemID.Equals("2")) // 2表示灵魂
            {
                listReward[i].Reward.spriteName = "Suoln";
                listReward[i].reWardName.text = itemCount;
            }
            else
            {
                string str = string.Format("{0},{1}", itemID, itemCount);
                object[] parms = new object[4];
                parms[0] = str;
                parms[1] = listReward[i].Reward;
                parms[2] = listReward[i].ItemBox;
                parms[3] = listReward[i].reWardName;
				listReward[i].itemID = itemID;
//                Debug.Log("--------str:" + str + "---------Reward:" + parms[1] + "---------ItemBox:" + parms[2] + "---------reWardName:" + parms[3]);
                PanelStatic.StaticBtnGameManager.invcl.SendMessage("SetItemIconAsID", parms, SendMessageOptions.DontRequireReceiver);
            }
		}
	}

    /// <summary>
    /// 启用领取奖励按钮
    /// </summary>
    public void EnableGetRewardBtn(bool isEnabled)
    {
        if (null != getRewardsBtn)
        {
            getRewardsBtn.enabled = isEnabled;
        }
    }

    /// <summary>
    /// 点击领取奖励按钮领取奖励
    /// </summary>
    public void GetRewards()
    {
        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().FirstRecharge());
    }
}

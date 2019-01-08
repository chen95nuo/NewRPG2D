using UnityEngine;
using System.Collections;

public enum BtnRewardStated{
	HaveReward,
	Receive,
	NoReward
}
public class BtnDay : MonoBehaviour {

  	public   BtnDisable btnEnter;
  	public  BtnDisable btnShow;
  	public  UISprite ItemReward;
  	public UIButtonMessage message;
	public UILabel lblGold;
	public UILabel lblBlood;
	public Transform tranGold;
	public Transform tranBlood;
	public bool isDay = false;
	public static BtnDay btnDay;


	public static BtnRewardStated btnReWardState = BtnRewardStated.Receive;

	void Awake(){
		btnDay = this;
	}
	public void SwitchBtnReStated(BtnRewardStated mBtnReward){
        switch(mBtnReward){
            case BtnRewardStated.HaveReward:
                {
                    btnEnter.picEnabled.enabled = true;
                    btnEnter.picQualit.enabled = false;
                    btnEnter.Mybutton.enabled = false;
                    btnReWardState = BtnRewardStated.HaveReward;//已领取
					isDay = false;
                }
                break;
		    case BtnRewardStated.Receive:
                {
			        btnEnter.picEnabled.enabled = true;
			        btnEnter.picQualit.enabled = true;
			        btnEnter.Mybutton.enabled = true;
			        btnReWardState = BtnRewardStated.Receive;//可领取
					isDay = true;
                }
			break;
		    case BtnRewardStated.NoReward:
                { 
			        btnEnter.picEnabled.enabled = true;
			        btnEnter.picQualit.enabled = true;
			        btnEnter.Mybutton.enabled = true;
					isDay = false;
            
					btnReWardState = BtnRewardStated.NoReward;//不可领取
		}
			break;
		}

		
	}
}
using UnityEngine;
using System.Collections;

public enum BtnStated{
	Receive,
	End
}
public class BtnGetReward : MonoBehaviour {
	public UISprite TodayReward;
	public UISprite GetRewardGround;
	public UILabel TodayState;
	private UIButton Mybtn;
	private string ShowText;
	public TweenPosition Tween;
	public TweenPosition Tween1;

	public ParticleSystem TheStart;

	public BtnDisable btnDisable;

	public static BtnStated btnState = BtnStated.Receive;
	public static BtnGetReward btnGetReWard;
	public PanelDailyBenefits  DailyReward;

	void Awake(){
		btnGetReWard = this;
		Mybtn = this.GetComponent<UIButton>();

	}
	void OnEnable(){
//		DailyReward.ShowGetRewardBtn();
	}
	public void SwitchBtnStated(BtnStated mbtnState){
		switch(mbtnState){
		case BtnStated.Receive:
				ShowText = StaticLoc.Loc.Get("info324");//领取奖励
//				GetRewardGround.enabled = true;
				TodayReward.enabled = true;
				btnDisable.Disable = false;
				Tween.enabled = true;
				Tween1.enabled = true;
				btnState = BtnStated.Receive;
				if (!TheStart.isPlaying)
				{
				TheStart.Play();
				}
				PanelDailyBenefits.panelDailyBenefits.IsGetReward = true;
				
			
			break;
		case BtnStated.End:
			if(int.Parse(BtnGameManager.yt[0]["DailyBenefits"].YuanColumnText)<7)
			{
				ShowText = StaticLoc.Loc.Get("info1234");//明日可领取
				if (!TheStart.isPlaying)
				{
					TheStart.Play();
				}
			}else{
				ShowText = StaticLoc.Loc.Get("info1235");//领取完毕
				if (!TheStart.isPlaying)
				{
					TheStart.Stop();
				}
			}
//				GetRewardGround.enabled = true;
				TodayReward.enabled = true;
				btnDisable.Disable = true;
				Tween.enabled = false;
				Tween1.enabled = false;
				btnState = BtnStated.End;
				PanelDailyBenefits.panelDailyBenefits.IsGetReward = false;
			break;
		}
		TodayState.text = ShowText;

		if(int.Parse(BtnGameManager.yt[0]["DailyBenefits"].YuanColumnText)==7)
		{
			return;
		}
		PanelDailyBenefits.panelDailyBenefits.SetPicGround();

	}
	public void BtnRewardClick(){
		switch(btnState){
		case BtnStated.Receive:
			PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().GetDailyBenefits());
//			InRoom.GetInRoomInstantiate().GetDailyBenefits();
			break;

		case BtnStated.End:
			SwitchBtnStated(BtnStated.End);
			break;
		}

	}
}

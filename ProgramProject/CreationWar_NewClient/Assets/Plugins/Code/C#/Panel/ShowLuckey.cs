using UnityEngine;
using System.Collections;

public class ShowLuckey : MonoBehaviour {
	public static ShowLuckey  show;
	public UILabel bonuseLable;
	public UILabel NeedBloodLable;
	public UILabel LuckeyTwolab;
	public UILabel LuckeyTenLab;

	public UILabel NowGoldLable;
	public UILabel NeedGold;
	public UILabel LuckeyTwolabGold;
	public UILabel LuckeyTenlabGold;

	public UILabel lblGoldTimeHint;
    public UILabel lblBloodTimeHint;

    public ParticleEmitter PE;
	void Awake(){
		show = this;
	}
	
	void OnEnable(){
		
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().JockPotShowInfo ());

        InvokeRepeating("ShowFreeOneLucky", 0, 1);
	}
	
	//显示当前奖池金额
	public void ShowBonuses(int ShowBons,int ShowNeedB,int poolGold,int needGold){

		int twoluckey = ShowNeedB*2;
		int tenluckey = ShowNeedB*10;

		int twoGold = needGold*2;
		int tenGold = needGold*10;
		if(bonuseLable!=null&&show){
			bonuseLable.text = ShowBons.ToString();
		}
		if(NeedBloodLable!=null&&show){
			NeedBloodLable.text = ShowNeedB.ToString();
		}
		if(LuckeyTwolab!=null&&show){
			LuckeyTwolab.text = twoluckey.ToString();
		}
		if(LuckeyTenLab!=null&&show){
			LuckeyTenLab.text = tenluckey.ToString();
		}


		if(NowGoldLable!=null&&show){
			NowGoldLable.text = poolGold.ToString();
		}
		if(NeedGold!=null&&show){
			NeedGold.text = needGold.ToString();
		}
		if(LuckeyTwolabGold!=null&&show){
			LuckeyTwolabGold.text = twoGold.ToString();
		}
		if(LuckeyTenlabGold!=null&&show){
			LuckeyTenlabGold.text = tenGold.ToString();
		}

	}

	/// <summary>
	/// 显示免费抽奖一次
	/// </summary>
	void ShowFreeOneLucky()
	{


		//金币
		if ((InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(BtnGameManager.yt[0]["PoolGoldFeerTime"].YuanColumnText)).TotalMinutes >= 5||(InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(BtnGameManager.yt[0]["PoolGoldFeerTime"].YuanColumnText)).TotalMinutes < 0)
		{
            lblGoldTimeHint.text = StaticLoc.Loc.Get("info837");
		}
		else
		{
            int remainingTime = 5 * 60 - (int)(InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(BtnGameManager.yt[0]["PoolGoldFeerTime"].YuanColumnText)).TotalSeconds;
            lblGoldTimeHint.text = string.Format("{0}:{1}{2}",StaticLoc.Loc.Get("info836"), remainingTime,StaticLoc.Loc.Get("info838"));
		}

		//血石
		if ((InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(BtnGameManager.yt[0]["PoolBloodFeerTime"].YuanColumnText)).TotalMinutes >= 10||(InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(BtnGameManager.yt[0]["PoolBloodFeerTime"].YuanColumnText)).TotalMinutes < 0)
		{

            lblBloodTimeHint.text = StaticLoc.Loc.Get("info837");
		}
		else
		{
			int remainingTime = 10 * 60 - (int)(InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(BtnGameManager.yt[0]["PoolBloodFeerTime"].YuanColumnText)).TotalSeconds;
            lblBloodTimeHint.text = string.Format("{0}:{1}{2}", StaticLoc.Loc.Get("info836"), remainingTime, StaticLoc.Loc.Get("info838"));
		}
	}
	
	//一连抽
	 void OneLucky(){
		if(show){
			PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().JockPotLottery (1,1));
		}
	}
	void Luckeytwo(){
		if(show){
			PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().JockPotLottery (2,1));
		}
	}
	void Luckeyten(){
		if(show){
            PE.Emit();
			PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().JockPotLottery (10,1));
		}
	}
	//金币抽奖	
	void OneLuckyGold(){
		if(show){
			PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().JockPotLottery (1,0));
		}
	}
	void LuckeytwoGold(){
		if(show){
			PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().JockPotLottery (2,0));
		}
	}
	void LuckeytenGold(){
		if(show){
            PE.Emit();
			PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().JockPotLottery (10,0));
		}
	}
}

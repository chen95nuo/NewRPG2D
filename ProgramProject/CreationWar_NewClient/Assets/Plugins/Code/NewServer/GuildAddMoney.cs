using UnityEngine;
using System.Collections;
using yuan.YuanPhoton;

public class GuildAddMoney : MonoBehaviour {
    public static GuildAddMoney my;
	public UILabel AllContribution;
	public UILabel MyContribution;
	public MoneyType moneytype;
	private string GuildId;
	[HideInInspector]
	public yuan.YuanMemoryDB.YuanTable yt;
	// Use this for initialization
	void Start () {
		GuildId = BtnGameManager.yt[0]["GuildID"].YuanColumnText;
	}

	void Awake()
	{
        my = this;
		yt = new yuan.YuanMemoryDB.YuanTable("GuildAddMoney" + this.name, "");
	}

	void OnEnable(){
		StartCoroutine(ShowText());
	}
	public void GetGuildID(){
		GuildId = BtnGameManager.yt[0]["GuildID"].YuanColumnText;
	}

    public void ShowTextUI()
    {
        MyContribution.text = BtnGameManager.yt.Rows[0]["SurplusContribution"].YuanColumnText;
        AllContribution.text = BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText;
    }

	IEnumerator ShowText(){
		
//		InRoom.GetInRoomInstantiate().GetTableForID(BtnGameManager.yt[0]["GuildID"].YuanColumnText, yuan.YuanPhoton.TableType.GuildInfo, yt);
		while (yt.IsUpdate)
		{
			yield return new WaitForSeconds(0.1f);
		}
        ShowTextUI();
	}
		
		public void ClickMoneyOne(){
			moneytype = MoneyType.Gold;
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().GuildFunds(GuildId,moneytype,1000));
	}
	public void ClickMoneyTwo(){
		moneytype = MoneyType.Gold;
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().GuildFunds (GuildId,moneytype,5000));
	}
	public void ClickMoneyThree(){
		moneytype = MoneyType.Gold;
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().GuildFunds (GuildId,moneytype,10000));
	}
	public void ClickMoneyFour(){
		moneytype = MoneyType.Gold;
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().GuildFunds (GuildId,moneytype,50000));
	}

	public void ClickStoneOne(){
		moneytype = MoneyType.BloodStone;
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().GuildFunds (GuildId,moneytype,10));
	}
	public void ClickStoneTwo(){
		moneytype = MoneyType.BloodStone;
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().GuildFunds (GuildId,moneytype,50));
	}
	public void ClickStoneThree(){
		moneytype = MoneyType.BloodStone;
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().GuildFunds (GuildId,moneytype,100));
	}
	public void ClickStoneFour(){
		moneytype = MoneyType.BloodStone;
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().GuildFunds (GuildId,moneytype,500));
	}

	public void GuildBuildGold(){
		GetGuildID();
		moneytype = MoneyType.Gold;
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().GuildBuild (GuildId,moneytype));
	}
	public void GuildBuildBlood(){
		GetGuildID();
		moneytype = MoneyType.BloodStone;
		PanelStatic.StaticBtnGameManager.RunOpenLoading (()=>InRoom.GetInRoomInstantiate ().GuildBuild (GuildId,moneytype));
	}
}

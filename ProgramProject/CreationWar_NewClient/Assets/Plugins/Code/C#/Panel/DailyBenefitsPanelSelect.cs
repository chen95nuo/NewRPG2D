using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DailyBenefitsPanelSelect : MonoBehaviour {

	public List<GameObject> listPanel;
	
	public BtnSelect btnChest;
//	public BtnSelect btnRank;
	public BtnSelect btnGuild;
	public BtnSelect btnSalaries;
	public BtnSelect btnLogin;
//	public BtnSelect btnPlayerInvite;
	public BtnSelect btnCode;
	public BtnSelect btnMakeGold;
	public BtnSelect btnTopUp;
	public BtnSelect btnBackHome;
	public BtnSelect btnReturnWallet;
	public BtnSelect btnBluntLeve;
	public BtnSelect btnEverydayToUp;
	public BtnSelect btnLeveBag;
	private int showVIP;
	public ParticleEmitter PE;

	private bool isRight = false;

	public bool isOneLeveBag = false;

	public static DailyBenefitsPanelSelect My;

	void Awake(){
		isRight = true;
		My = this ;
		showVIP = PlayerPrefs.GetInt("ShowVIP", 1);
	}
	void OnEnable()
	{
		StartCoroutine (YuanOnEnable ());
		//firstBtn.GetComponent<UIToggle>().isChecked=true;

        if (BtnGameManager.yt.Rows[0]["OpenedChests"].YuanColumnText.Equals("-1,-1,-1,-1"))
        {
            btnChest.btnDisable.picEnabled.enabled = false;
        }
	}

	void Update(){
		if(showVIP==1){
			btnCode.gameObject.SetActive(true);
		}else{
			btnCode.gameObject.SetActive(false);
		}
	}
	
	public IEnumerator YuanOnEnable()
	{
		BtnSelect tempBtn=null;
		if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.RedemptionCodeSwitch)!="1")
		{
			if(btnCode!=null)
			{
//				btnFirst.gameObject.SetActiveRecursively (false);
			}
		}


		// int openNum = int.Parse(BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText == "" ? "0" : BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText) - int.Parse(BtnGameManager.yt.Rows[0]["NumOpenBox"].YuanColumnText == "" ? "0" : BtnGameManager.yt.Rows[0]["NumOpenBox"].YuanColumnText) + 1;

		//if (BtnGameManager.yt.Rows[0]["CanDailyBenefits"].YuanColumnText.Trim() == "1")
		if(isOneLeveBag)
		{
			isOneLeveBag = false;
			tempBtn=btnLeveBag;
		}else
		if(int.Parse(BtnGameManager.yt[0]["DailyBenefits"].YuanColumnText) > int.Parse(BtnGameManager.yt.Rows[0]["CanDailyBenefits"].YuanColumnText))
		{
			tempBtn=btnLogin;
		}
		else  if (BtnGameManager.yt.Rows[0]["CanSalaries"].YuanColumnText == "1")
		{
			tempBtn=btnSalaries;
		}
		//else if(openNum>0)
        else if (!BtnGameManager.yt.Rows[0]["OpenedChests"].YuanColumnText.Equals("-1,-1,-1,-1"))
		{
			tempBtn=btnChest;
		}
//		else if (BtnGameManager.yt.Rows[0]["IsGetFirstVIP"].YuanColumnText == "1")
//		{
//			tempBtn=btnLogin;
//		}
//		else  if (BtnGameManager.yt.Rows[0]["CanRankBenefits"].YuanColumnText == "1"&&BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText != "" && BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText != "0")
//		{
//			tempBtn=btnChest;
//		}
//		else if (BtnGameManager.yt.Rows[0]["CanGuildBenefits"].YuanColumnText == "1"&&BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText != "" && BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText != "0")
//		{
//			tempBtn=btnGuild;
//		}
		else 
		{
            if (btnSalaries.gameObject.activeSelf)
			{
				tempBtn=btnChest;
			}
			else
			{
				tempBtn=btnSalaries;
			}
		}

		yuan.YuanClass.SwitchList (listPanel,true,true);
//		foreach (GameObject item in listPanel)
//		{
//			item.SetActiveRecursively(true);
//		}

		yield return new WaitForFixedUpdate();

		tempBtn.OnClick ();		
		if (BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText == "" || BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText == "0")
		{
			btnLogin.btnDisable.Disable=true;
		}
		if (BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText == "" || BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText == "0")
		{
			if(btnGuild){
			btnGuild.btnDisable.Disable=true;
			}
		}

        if (null != targetBtn)
        {
            targetBtn.OnClick();
            targetBtn = null;
        }
	}

    private BtnSelect targetBtn;
    public void OpenTargetActivity(GameObject btnObj)
    {
        if(null != btnObj)
        {
            targetBtn = btnObj.GetComponent<BtnSelect>();
        }
    }

	public void MakeGoldClick()
	{
		if(PE){
		PE.Emit();
	}
	}
	public void BtnLeveShow()
	{
		if(this.gameObject.activeSelf){
		StartCoroutine (YuanOnEnable ());
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DailyBenefitsType
{
    First=0,
    Guild,
    Login,
    Rank,
    Salaries,
    Chest,
    Inviter,
    Invitees,
	BackMoney,
	ReturnWallet,
	BluntLeve,
	EverydayToUp,
}

public class BtnSelect : MonoBehaviour {

    public UILabel myTitle;
    public string myInfo;
    public string myTime;

    public BtnBenefitsEnter btnBenefitsEnter;
    public DailyBenefitsPanelSelect panelSelect;
    public int num;
    public DailyBenefitsType dailyBenefitsType;
    public BtnDisable btnDisable;
    public UILabel lblTitle;
    public UILabel lblText;
    public UILabel lblTime;
    public UIGrid grid;
    public GameObject invMaker;
    public GameObject spriteForBenefits;
    public List<BenefitsInfo> listBenefitsInfo = new List<BenefitsInfo>();
    public List<SpriteForBenefits> listSprite = new List<SpriteForBenefits>();
    private int numTemp  = 0;
	private UIToggle myToggle;
	
	void Awake()
	{
		//listSprite.Clear();
		myToggle=GetComponent<UIToggle>();
	}
	
    void Start()
    {
		invMaker=PanelStatic.StaticBtnGameManager.InvMake;
        switch (dailyBenefitsType)
        {
            case DailyBenefitsType.Salaries:
//                foreach (BenefitsInfo item in listBenefitsInfo)
//                {
//                //    item.benefitsValue = ((int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.Salaries]* int.Parse(InRoom.isUpdatePlayerLevel?InRoom.playerLevel: BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText)).ToString();
//                    item.benefitsValue = ((int)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.Salaries]* int.Parse(InRoom.isUpdatePlayerLevel?InRoom.playerLevel: BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText)).ToString();
//
//                }
			listBenefitsInfo[0].benefitsValue = ((int)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.Salaries]* int.Parse(InRoom.isUpdatePlayerLevel?InRoom.playerLevel: BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText)).ToString();
			if(listBenefitsInfo.Count>1&&listBenefitsInfo[1].benefitsType==BenefitsType.Gold)
			{
				listBenefitsInfo[1].benefitsValue = ((int)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.Guild] * (BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText == "" ? 0 : int.Parse(BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText))).ToString();
			}
                break;
            case DailyBenefitsType.Rank:
                foreach (BenefitsInfo item in listBenefitsInfo)
                {
                 //   item.benefitsValue = ((int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.Rank] * (BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText==""?0:int.Parse(BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText))).ToString();
                    item.benefitsValue = ((int)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.Rank] * (BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText == "" ? 0 : int.Parse(BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText))).ToString();

                }
                break;
//            case DailyBenefitsType.Guild:
//                foreach (BenefitsInfo item in listBenefitsInfo) 
//                {
//                //    item.benefitsValue = ((int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.Guild] * (BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText==""?0:int.Parse(BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText))).ToString();
//                    item.benefitsValue = ((int)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.Guild] * (BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText == "" ? 0 : int.Parse(BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText))).ToString();
//
//                }
//                break;
        }
    }

    void OnEnable()
    {
        GetFund();
    }
	
	void OnLevelWasLoaded(int id)
    {
        
    }

    void GetFund()
    {
        btnBenefitsEnter.btnDisable.Disable = true;
        switch (dailyBenefitsType)
        {
			case DailyBenefitsType.First:
                if (BtnGameManager.yt.Rows[0]["IsGetFirstVIP"].YuanColumnText == "1")
                {
                    btnBenefitsEnter.btnDisable.Disable = true;
					btnBenefitsEnter.btnDisable.lblText.text=StaticLoc.Loc.Get("info323")+"";
                }
                else
                {
					if(BtnGameManager.yt.Rows[0]["IsFirstVIP"].YuanColumnText == "1")
					{
						  btnBenefitsEnter.btnDisable.Disable = false;
						btnBenefitsEnter.btnDisable.lblText.text=StaticLoc.Loc.Get("info324")+"";
					}
					else
					{
						  btnBenefitsEnter.btnDisable.Disable = false;
						btnBenefitsEnter.btnDisable.lblText.text=StaticLoc.Loc.Get("info325")+"";						
					}
                }
			break;
            case DailyBenefitsType.Salaries:
                if (BtnGameManager.yt.Rows[0]["CanSalaries"].YuanColumnText == "1")
                {
                    btnBenefitsEnter.btnDisable.Disable = false;
                   btnDisable.Disable = false;
					btnBenefitsEnter.btnDisable.lblText.text=StaticLoc.Loc.Get("info324");
                }
                else
                {
                   btnBenefitsEnter.btnDisable.Disable = true;
                    btnDisable.Disable = true;
					btnBenefitsEnter.btnDisable.lblText.text=StaticLoc.Loc.Get("info323")+"";
                }
                //foreach (BenefitsInfo item in listBenefitsInfo)
                //{
         
                //}
                break;
            case DailyBenefitsType.Rank:
			if (BtnGameManager.yt.Rows[0]["CanRankBenefits"].YuanColumnText == "1"&&BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText != "" && BtnGameManager.yt.Rows[0]["Rank"].YuanColumnText != "0")
                {
                    btnBenefitsEnter.btnDisable.Disable = false;
                    btnDisable.Disable = false;
					btnBenefitsEnter.btnDisable.lblText.text=StaticLoc.Loc.Get("info324")+"";
                }
                else
                {
                    btnBenefitsEnter.btnDisable.Disable = true;
                    btnDisable.Disable = true;
					btnBenefitsEnter.btnDisable.lblText.text=StaticLoc.Loc.Get("info323")+"";
                }
                //foreach (BenefitsInfo item in listBenefitsInfo)
                //{
  
                //}
                break;
//            case DailyBenefitsType.Guild:
//			if (BtnGameManager.yt.Rows[0]["CanGuildBenefits"].YuanColumnText == "1"&&BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText != "" && BtnGameManager.yt.Rows[0]["GuildContribution"].YuanColumnText != "0")
//                {
//                    btnBenefitsEnter.btnDisable.Disable = false;
//                    btnDisable.Disable = false;
//				btnBenefitsEnter.btnDisable.lblText.text=StaticLoc.Loc.Get("info324")+"";
//                }
//                else
//                {
//                    btnBenefitsEnter.btnDisable.Disable = true;
//                    btnDisable.Disable = true;
//				btnBenefitsEnter.btnDisable.lblText.text=StaticLoc.Loc.Get("info323")+"";
//                }
//
//                break;
            case DailyBenefitsType.Chest:
                {
                   int openNum = int.Parse(BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText == "" ? "0" : BtnGameManager.yt.Rows[0]["Serving"].YuanColumnText) - int.Parse(BtnGameManager.yt.Rows[0]["NumOpenBox"].YuanColumnText == "" ? "0" : BtnGameManager.yt.Rows[0]["NumOpenBox"].YuanColumnText) + 1;
                   if (openNum > 0)
                   {
                       btnDisable.Disable = false;
                   }
                   else
                   {
                       btnDisable.Disable = true;
                   }
                }
                break;
            case DailyBenefitsType.Login:
                {
                   
                    if (BtnGameManager.yt.Rows[0]["CanDailyBenefits"].YuanColumnText.Trim() == "1")
                    {
//                        btnDisable.Disable = false;
				btnBenefitsEnter.btnDisable.lblText.text=StaticLoc.Loc.Get("info324")+"";
                    }
                    else
                    {
 //                       btnDisable.Disable = false;
				btnBenefitsEnter.btnDisable.lblText.text=StaticLoc.Loc.Get("info323")+"";
                    }
                }
                break;
        }
    }

	private bool isOne = false;
    public void OnClick()
    {

			btnBenefitsEnter.dailyBenefitsType = this.dailyBenefitsType;
			yuan.YuanClass.SwitchList(panelSelect.listPanel, false, true);
	        yuan.YuanClass.SwitchListOnlyOne(panelSelect.listPanel, num, true, true);
			
	        GetFund();

	       
	        numTemp = 0;
	        if (num == 3)
	        {
	            lblTitle.text = myTitle.text;
				lblText.text = string.Format ("{0}\n{1}",StaticLoc.Loc.Get("info830"),StaticLoc.Loc.Get("info829")) ;
	            lblTime.text = myTime;
	            foreach (SpriteForBenefits item in listSprite)
	            {
				if(item){
	                item.gameObject.SetActiveRecursively(false);
				}
				}

	            foreach (BenefitsInfo item in listBenefitsInfo)
	            {

					
			                if (listSprite.Count >numTemp)
			                {
			                    SetBtn(listSprite[numTemp],item);
			                }
			                else
			                {
								if(!isOne&&grid!=null)
								{
				                    SpriteForBenefits tempBtn = ((GameObject)Instantiate(spriteForBenefits)).GetComponent<SpriteForBenefits>();
				                    tempBtn.transform.parent = grid.transform;
				                    tempBtn.transform.localPosition = Vector3.zero;
				                    tempBtn.transform.localScale = new Vector3(1, 1, 1);
				                    SetBtn(tempBtn, item);
				                    listSprite.Add(tempBtn);
									//isOne = true;
								}
							}

	                numTemp++;
	            }
	            //grid.repositionNow = true;
	        }
		if(null!=this.myToggle)
		{
			this.myToggle.value=true;
		}
    }

    private void SetBtn(SpriteForBenefits mBtn, BenefitsInfo mBenefitsInfo)
    {
        
        mBtn.benefitsType = mBenefitsInfo.benefitsType;
        mBtn.benefitsValue = mBenefitsInfo.benefitsValue;
        switch (mBenefitsInfo.benefitsType)
        {
            case BenefitsType.Gold:
                mBtn.lblNum.text = mBenefitsInfo.benefitsValue;
                mBtn.spriteBenefits.spriteName = "Gold";
                break;
            case BenefitsType.BloodStone:
                mBtn.lblNum.text = mBenefitsInfo.benefitsValue;
                mBtn.spriteBenefits.spriteName = "Bloodstone";
                break;
            case BenefitsType.Item:
                string[] strTemp = mBenefitsInfo.benefitsValue.Split(',');
                mBtn.lblNum.text = strTemp[1];

                object[] parms = new object[2];
                parms[0] = mBenefitsInfo.benefitsValue;
                parms[1] = mBtn.spriteBenefits;
                invMaker.SendMessage("SpriteName", parms, SendMessageOptions.DontRequireReceiver);
                break;
        }
        mBtn.gameObject.SetActiveRecursively(true);
    }

}

[System.Serializable]
public class BenefitsInfo
{
    public BenefitsInfo()
    {
 
    }

    public BenefitsType benefitsType;
    public string benefitsValue;
}


public enum BenefitsType
{
    Gold = 0,
    BloodStone,
    Item,
}



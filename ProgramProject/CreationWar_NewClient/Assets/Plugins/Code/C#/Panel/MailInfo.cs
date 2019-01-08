using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MailInfo : MonoBehaviour {

    public UIButton btnDelete;
    public UIButton btnReply;
    public UILabel lblSender;
    public UILabel lblTitle;
    public UILabel lblSendTime;
    public UILabel lblText;
    public UILabel lblGold;
    public UILabel lblBlood;
    public bool isSystemMail;
    public bool isGMMail;
    public UISprite pic;
    public yuan.YuanMemoryDB.YuanRow yr;

    public BtnEvent btnEnter;
    void Awake()
    {
		mailInfo=this;
        btnEnter.BtnClickEvent += this.SendMail;
        mailInfoWirte.txtBloodStone.text = "0";
        mailInfoWirte.txtGold.text = "0";
    }

    void Start()
    {
		warnings=PanelStatic.StaticWarnings;
        btnOpenBagItem.BtnClickEvent += this.OpenBagItem;
        btnOpenPlayerList.BtnClickEvent += this.OpenPlayerList;
    }

    public yuan.YuanMemoryDB.YuanRow Yr
    {
        get { return yr; }
        set
        {
            yr = value;
            SetInfo();
        }
    }

    private void SetInfo()
    {
        lblSender.text = yr["MailSender"].YuanColumnText;
        lblTitle.text = yr["MailTitle"].YuanColumnText;
        lblSendTime.text = yr["SendTime"].YuanColumnText;
        lblText.text = yr["MailText"].YuanColumnText;
        if (yr.ContainsKey("Gold"))
        {
            lblGold.text = yr["Gold"].YuanColumnText;
            lblBlood.text = yr["BloodStone"].YuanColumnText;
        }
    
    }
    
     public IEnumerator OpenGMMail()
    {
//		Debug.Log("123123123");
		yield return new WaitForSeconds(1);
        this.isGMMail = true;
        this.isSystemMail = false;
        mailInfoWirte.txtAddressee.text = StaticLoc.Loc.Get("info326")+"";
        mailInfoWirte.txtTitle.text = "";
        mailInfoWirte.txtText.text = "";
        yuan.YuanClass.SwitchListOnlyOne(menu, 1, true, true);

    }

	void OnEnable()
	{
		ClearBage();
		Invoke("OpenMail",0.3f);
	}
	
	void OpenMail()
	{
		mailInfoWirte.transform.parent.gameObject.SetActiveRecursively(false);
	
	}



    public MailInfoWirte mailInfoWirte;
    public List<GameObject> menu = new List<GameObject>();
    private void WirteMail()
    {
        this.isGMMail = false;
        this.isSystemMail = false;
        yuan.YuanClass.SwitchListOnlyOne(menu, 1, true, true);
        OpenPlayerList(this,null);
    }

    private void WirteMailReply()
    {
        if (yr != null)
        {
            if (isSystemMail)
            {
                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info483"));
            }
            else
            {
                yuan.YuanClass.SwitchListOnlyOne(menu, 1, true, true);
                OpenPlayerList(this, null);
                mailInfoWirte.txtAddressee.text = lblSender.text;
                mailInfoWirte.txtTitle.text = StaticLoc.Loc.Get("info327")+"" + lblTitle.text;
            }
        }
    }

    public GameObject playerList;
    public GameObject bagItem;
    public BtnEvent btnOpenPlayerList;
    public BtnEvent btnOpenBagItem;
    public void OpenPlayerList(object sender,object parm)
    {
        playerList.SetActiveRecursively(true);
        bagItem.SetActiveRecursively(false);
    }

    public void OpenBagItem(object sender,object parm)
    {
        playerList.SetActiveRecursively(false);
        bagItem.SetActiveRecursively(true);
    }

    public GetMail getMail;
    private void DeleteMail()
    {


        if (yr != null&&yr.RowItem.ContainsKey("id"))
        {
			if (!isSystemMail)
            {
				if(string.IsNullOrEmpty(yr["MailTool1"].YuanColumnText)){
					InRoom.GetInRoomInstantiate().MailDelete(yr["id"].YuanColumnText);
					getMail.OpenMail();
				}else if(!string.IsNullOrEmpty(yr["MailTool1"].YuanColumnText)&&(int.Parse(yr["isGetTool1"].YuanColumnText)==1)){

					InRoom.GetInRoomInstantiate().MailDelete(yr["id"].YuanColumnText);
					getMail.OpenMail();

				}else{
					warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info990"));
				}
                
            }
			else
            {
                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info484"));
            }
        }
    }


	
	private float timeGetTool=0;
    private void GetMailToolClick()
    {
        if (yr != null)
        {
			if(Time.time-timeGetTool>=5)
			{
				timeGetTool=Time.time;
	            if (yr.ContainsKey("isPaymentPickup"))
	            {
	                if (yr["isPaymentPickup"].YuanColumnText == "1")
	                {
	                    warnings.warningAllEnterClose.btnEnterEvent.BtnClickEvent += this.GetMailTool;
	                    warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info485"));
	                }
	                else
	                {
	                    GetMailTool(this, null);
	                }
	            }
			}
			else
			{
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info486"));
			}
        }
    }



    private void GetMailTool(object sender,object parm)
    {
        warnings.warningAllEnterClose.btnEnterEvent.BtnClickEvent -= this.GetMailTool;
        warnings.warningAllEnterClose.Close();
		StartCoroutine ( PanelStatic.StaticBtnGameManager.OpenLoading (()=>InRoom.GetInRoomInstantiate().MailGetTool(yr["id"].YuanColumnText)));
        
    }

    private void GetMail()
    {
		//Debug.Log ("111111111111");
        yuan.YuanClass.SwitchListOnlyOne(menu, 0, true, true);
    }

   public Warnings warnings;
	public string invStr;
	public GameObject invBageItem;
    public void SendMail(object sender,object parm)
    {
        if (isGMMail)
        {
            SendGM();
        }
        else
        {

				if((InRoom.isUpdatePlayerLevel?InRoom.playerLevel.Parse (0): BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText.Parse (0))>0)
				{
					if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.MailSwitch)=="1")
					{
			            if (!mailInfoWirte.cbxIsPaymentPickup.isChecked)
			            {
			                //warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
			                //warnings.warningAllEnterClose.btnEnter.functionName = "Send";
			                warnings.warningAllEnterClose.btnEnterEvent.BtnClickEvent += this.Send;
			                warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info487"));
			
			            }
			            else
			            {
			                Send(this, null);
			            }
					}
					else
					{
						warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info645"));
					}
				}
				else
				{
					warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("tips054"));
				}
        }
    }

    public void SendGM()
    {
        if (mailInfoWirte.txtText.text != "" && mailInfoWirte.txtTitle.text != "" && mailInfoWirte.txtAddressee.text != "")
        {
			PanelStatic.StaticBtnGameManager.RunOpenLoading(()=>InRoom.GetInRoomInstantiate().MailSend(mailInfoWirte.txtTitle.text, mailInfoWirte.txtAddressee.text, mailInfoWirte.txtText.text, "", "0", "0", "0",true));
            
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info488"));
        }
    }

    public void Send(object sender,object parm)
    {
//       StartCoroutine (RealSend ());
		RealSend ();
    }
	
//	public IEnumerator RealSend()
	public void RealSend()
	{
		 warnings.warningAllEnterClose.btnEnterEvent.BtnClickEvent -= this.Send;
        warnings.warningAllEnterClose.Close();
		if(Application.internetReachability==NetworkReachability.NotReachable)
		{
			warnings.warningAllEnter.Show (StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info489"));
//			StopCoroutine ("RealSend");
		}
        if (mailInfoWirte.txtText.text != "" && mailInfoWirte.txtTitle.text != "" && mailInfoWirte.txtAddressee.text != "")
        {
            string isPayment = "0";
            if (mailInfoWirte.cbxIsPaymentPickup.isChecked)
            {
                isPayment = "1";
            }
            else
            {
                isPayment = "0";
            }
			int sendGold=0;
			if(int.TryParse (mailInfoWirte.txtGold.value,out sendGold)&&sendGold>=0)
			{
				PanelStatic.StaticBtnGameManager.RunOpenLoading(()=>InRoom.GetInRoomInstantiate().MailSend(mailInfoWirte.txtTitle.text, mailInfoWirte.txtAddressee.text, mailInfoWirte.txtText.text, invStr, isPayment, mailInfoWirte.txtGold.text, mailInfoWirte.txtBloodStone.text, false));
				ClearBage();
			}
			else
			{
				warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info894") );
			}

        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info494") );
        }
	}
	
	public void ClearBage()
	{
		invBageItem.SendMessage ("invClear",SendMessageOptions.DontRequireReceiver);
	}
	
	public GameObject invGetBageItem;
	public void ClearGetBage()
	{
		invGetBageItem.SendMessage ("invClear",SendMessageOptions.DontRequireReceiver);
	}
	
	public static MailInfo mailInfo;

}

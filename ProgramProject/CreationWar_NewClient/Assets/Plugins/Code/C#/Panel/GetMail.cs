using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetMail : MonoBehaviour {



	public static GetMail my;
    public GameObject btnMail;
    public UIGrid grid;
    public GameObject playerFirend;
    private yuan.YuanMemoryDB.YuanTable yt = new yuan.YuanMemoryDB.YuanTable("Mail", "id");
    private yuan.YuanMemoryDB.YuanTable ytSystem = new yuan.YuanMemoryDB.YuanTable("SystemMail", "id");
   
	public Color color;
	void Awake()
	{
		my=this;
	}

    void OnEnable()
    {
        playerFirend.SetActiveRecursively(false);
        Invoke("OpenMail", 0.3f);
    }

    public void OpenMail()
    {
        
        StartCoroutine(GetMailInfo());
        //StartCoroutine(GetSystemMailInfo());
    }
	
    private List<CkbMail> listMail = new List<CkbMail>();
	private CkbMail isSelectMail;
    private List<CkbMail> listSysMail = new List<CkbMail>();
    public MailInfo mailInfo;
    public UILabel lblNull;
    public IEnumerator GetMailInfo()
    {
        //InRoom.GetInRoomInstantiate().GetYuanTable(string.Format("Select * from Mail where MailAddersseeID='{0}' and isDelete!=1",BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText), "DarkSword2", yt);
        InRoom.GetInRoomInstantiate ().GetMyMail (yt);

		while (yt.IsUpdate)
        {
            yield return new WaitForSeconds(0.1f);
        }
        foreach (CkbMail item in listMail)
        {
            Destroy(item.gameObject);
            //item.gameObject.SetActiveRecursively(false);
        }
        listMail.Clear();
        if (yt.Rows.Count > 0)
        {
            lblNull.gameObject.active = false;
        }
        else
        {
            lblNull.gameObject.active = true;
        }
		int num=0;
        foreach (yuan.YuanMemoryDB.YuanRow yr in yt.Rows)
        {
            GameObject obj = (GameObject)Instantiate(btnMail);
            CkbMail tempBtn = obj.GetComponent<CkbMail>();
            tempBtn.lblMailTitle.text = yr["MailTitle"].YuanColumnText;
            tempBtn.lblMailSender.text = yr["MailSender"].YuanColumnText;
            tempBtn.yr = yr;
			if(tempBtn.yr["MailTool1"].YuanColumnText!=""&&tempBtn.yr["isGetTool1"].YuanColumnText!="1")
			{
				object[] parms=new object[2];
				parms[0]=tempBtn.yr["MailTool1"].YuanColumnText;
				parms[1]=tempBtn.pic;
				PanelStatic.StaticBtnGameManager.InvMake.SendMessage("SpriteName", parms, SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				tempBtn.pic.spriteName="dunpai";
				tempBtn.pic.color = color;
			}
            tempBtn.isSystemMail = false;
            tempBtn.myMessage.target = this.gameObject;
            tempBtn.transform.parent = grid.transform;
            tempBtn.transform.localPosition = Vector3.zero;
            tempBtn.transform.localScale = new Vector3(1, 1, 1);
            tempBtn.myMessage.functionName = "MainBtnClick";
            tempBtn.check.group = 9;
            listMail.Add(tempBtn);
			if(num==0)
			{
				tempBtn.myMessage.OnClick();
				MailInfo.mailInfo.Yr=tempBtn.yr;
			}
			num++;

        }
        grid.repositionNow = true;
		//this.RefreshList();
    }

	public void RefreshList()
	{
		UIToggle activeTog= UIToggle.GetActiveToggle (9);
		if(activeTog!=null)
		{
			activeTog.gameObject.SendMessage ("OnClick",SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			mailInfo.yr=null;
			mailInfo.lblBlood.text="";
			mailInfo.lblGold.text="";
			mailInfo.lblSender.text="";
			mailInfo.lblSendTime.text="";
			mailInfo.lblText.text="";
			mailInfo.lblTitle.text="";
			
		}
	}

    private IEnumerator GetSystemMailInfo()
    {
        InRoom.GetInRoomInstantiate().GetYuanTable(string.Format("Select * from SystemMail"), "DarkSword2", ytSystem);
        while (ytSystem.IsUpdate)
        {
            yield return new WaitForSeconds(0.1f);
        }
        foreach (CkbMail item in listSysMail)
        {
            Destroy(item.gameObject);
            //item.gameObject.SetActiveRecursively(false);
        }
        listSysMail.Clear();
        if (ytSystem.Rows.Count > 0)
        {
            lblNull.gameObject.active = false;
        }
        else
        {
            lblNull.gameObject.active = true;
        }
        foreach (yuan.YuanMemoryDB.YuanRow yr in ytSystem.Rows)
        {
            GameObject obj = (GameObject)Instantiate(btnMail);
            CkbMail tempBtn = obj.GetComponent<CkbMail>();
            tempBtn.lblMailTitle.text = yr["MailTitle"].YuanColumnText;
            tempBtn.lblMailSender.text = yr["MailSender"].YuanColumnText;
            tempBtn.isSystemMail = true;
            tempBtn.yr = yr;
			if(tempBtn.yr["MailTool1"].YuanColumnText!=""&&tempBtn.yr["isGetTool1"].YuanColumnText!="1")
			{
				object[] parms=new object[2];
				parms[0]=tempBtn.yr["MailTool1"].YuanColumnText;
				parms[1]=tempBtn.pic;
				PanelStatic.StaticBtnGameManager.InvMake.SendMessage("SpriteName", parms, SendMessageOptions.DontRequireReceiver);
			}
			else
			{

				tempBtn.pic.spriteName="dunpai";
				tempBtn.pic.color = color;
			}
			
            tempBtn.myMessage.target = this.gameObject;
            tempBtn.transform.parent = grid.transform;
            tempBtn.transform.localPosition = Vector3.zero;
            tempBtn.transform.localScale = new Vector3(1, 1, 1);
            tempBtn.myMessage.functionName = "MainBtnClick";
            tempBtn.check.group = 9;
            listSysMail.Add(tempBtn);
        }
        grid.repositionNow = true;
    }

	public GameObject bagItem;
	public UIButton BtnGetClick;
    private void MainBtnClick(GameObject obj)
    {
        CkbMail tempMail = obj.GetComponent<CkbMail>();
        mailInfo.isSystemMail = tempMail.isSystemMail;
        mailInfo.Yr = tempMail.yr;
        if (mailInfo.Yr.ContainsKey("isSystem") && mailInfo.Yr["isSystem"].YuanColumnText == "1")
        {
            mailInfo.isGMMail = true;
        }
        else
        {
            mailInfo.isGMMail = false;
        }
		

        if (mailInfo.Yr.ContainsKey("MailTool1") && mailInfo.Yr["MailTool1"].YuanColumnText!=""&&mailInfo.Yr["isGetTool1"].YuanColumnText!="1")
		{
			bagItem.SendMessage ("GetInvAsID",mailInfo.Yr["MailTool1"].YuanColumnText);
		}
		else
		{
			bagItem.SendMessage ("invClear",SendMessageOptions.DontRequireReceiver);
		}

		if(mailInfo.Yr["isGetTool1"].YuanColumnText=="1"||(mailInfo.Yr["MailTool1"].YuanColumnText==""&&mailInfo.Yr["Gold"].YuanColumnText==""&&mailInfo.Yr["BloodStone"].YuanColumnText==""))
		{
			BtnGetClick.isEnabled = false;
		}else{
			BtnGetClick.isEnabled = true;
		}
    }

 
}

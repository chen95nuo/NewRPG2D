using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BtnSend : MonoBehaviour {
	
	//public CharBar charBar;
	public YuanInput yInput;
    public SendManager sendManager;
    public BtnEvent btnTV;
    public UIToggle ckbTV;
	public GameObject redBack;
	public List<string> listChat=new List<string>();
	public UIPanel myPanel;
	// Use this for initialization

     public void Awake()
	{

		sendManager = PanelStatic.StaticSendManager;
		PanelStatic .StaticBtnGameManager.eventShowOne = this.ShowOne;
		btnTV.SetEvent (this.BtnTVCliek);
	}
	
	void Start () {
		
		warnings=PanelStatic.StaticWarnings;
		redBack.active=false;
		 myPanel=tweenChat.transform.parent.GetComponent<UIPanel>();
	}


	

	
	void OnEnable()
	{
		redBack.active=ckbTV.value;
	}


    public string strAll;
    public string strGuild;
    public string strTeam;
    public string strSomeBody;
    [HideInInspector]
    string[] messgebodys;
    private string sameStr = string.Empty;
    private int numSame;
	
	private float timeout=5;
	private float mytime=0;

	public int timeClick=5;
    void OnClick()
    {
		if(Time.time-mytime<timeClick)
		{
			warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"),StaticLoc.Loc.Get("tips037"));
			return;
		}
		mytime=Time.time;
        if (yInput.Text.Trim() != "")
        {
			foreach(string item in listChat)
			{
				if(yInput.Text.IndexOf (item)!=-1)
				{
					warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"),StaticLoc.Loc.Get("info521"));
					return;
				}
			}
            if (!ckbTV.value)
            {
                if (yInput.Text == sameStr)
                {
                    numSame++;
                }
                else
                {
                    numSame = 0;
                }
                if (numSame >= 3)
                {
                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info522"));
                    return;
                }

//                Debug.Log("----------------------发送消息");

                if (sendManager.barAll.value)
                {
                    string[] messgebodys = new string[1];
                    messgebodys[0] = BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText;
                    InRoom.GetInRoomInstantiate().SendMessage(messgebodys, yuan.YuanPhoton.MessageType.All, RefreshString(yInput.Text), BtnGameManager.yt.Rows[0]["PlayerName"].YuanColumnText);
                }
                else if (sendManager.barGuild.value)
                {
                    //messgebodys = new string[1];
                    InRoom.GetInRoomInstantiate().SendMessage(null, yuan.YuanPhoton.MessageType.Guild, RefreshString(yInput.Text), BtnGameManager.yt.Rows[0]["PlayerName"].YuanColumnText);
                }
                else if (sendManager.barTeam.value)
                {
                    //messgebodys = new string[1];
                    InRoom.GetInRoomInstantiate().SendMessage(null, yuan.YuanPhoton.MessageType.Team, RefreshString(yInput.Text), BtnGameManager.yt.Rows[0]["PlayerName"].YuanColumnText);
                }
                else if (sendManager.barSomeBody.value)
                {
                    //messgebodys = new string[1];
                    //messgebodys[0] = "32";
                    if (strSomeOne != null && strSomeOne[0] != null && strSomeOne[0] != "")
                    {
                        InRoom.GetInRoomInstantiate().SendMessage(strSomeOne, yuan.YuanPhoton.MessageType.Somebody, RefreshString(yInput.Text), BtnGameManager.yt.Rows[0]["PlayerName"].YuanColumnText);
                    }
                }
				else if(sendManager.barSystem.value)
				{
					 warnings.warningAllTime.Show("", StaticLoc.Loc.Get("info744"));
				}

                sameStr = yInput.Text;
            }
            else
            {
                //if (int.Parse(BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText) >= (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.TVMessageBlood])
                //{
                //    InRoom.GetInRoomInstantiate().SendTVMessage(yInput.Text.Trim());
                //    //BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText = (int.Parse(BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText) - (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.TVMessageBlood]).ToString();
                //    PanelStatic.StaticBtnGameManager.invcl.SendMessage("YAddBlood", - (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.TVMessageBlood], SendMessageOptions.DontRequireReceiver);
                //    ckbTV.value = false;
                //    redBack.active = false;
                //}
                //else
                //{
				
                //    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info509"));
                //}
                if (int.Parse(BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText) >= (int)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.TVMessageBlood])
                {
                    InRoom.GetInRoomInstantiate().SendTVMessage(yInput.Text.Trim());
                    //BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText = (int.Parse(BtnGameManager.yt.Rows[0]["Bloodstone"].YuanColumnText) - (int)YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.TVMessageBlood]).ToString();
                    PanelStatic.StaticBtnGameManager.invcl.SendMessage("YAddBlood", -(int)YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.TVMessageBlood], SendMessageOptions.DontRequireReceiver);
                    ckbTV.value = false;
                    redBack.active = false;
                }
                else
                {

                    warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info509"));
                }
            }
            yInput.Text = "";
        }
    }
	
	public string RefreshString(string mText)
	{
		foreach(string item in GetRandomName.shieldedWordStatic)
		{
			mText= mText.Replace(item,"***");
		}
		return mText;
	}
	
    [HideInInspector]
	public string PlayerName;
	public void SetMyName(string name){
		PlayerName = name;
	}

	public void SendTalkSomeBody(string[] listBoyd)
	{
		sendManager.barSomeBody.value=true;
		strSomeOne=listBoyd;
		
	}
	
	public void ShowOne(object mID)
    {
        strSomeOne =(string[]) mID;
        lblPlayerName.text = strSomeOne[1];
        sendManager.OnSomeClick(this.gameObject);
        //myPanel.enabled=true;
        //tweenChat.Play(true);

        //tweenChat.gameObject.SetActiveRecursively(true);
        if (songShulan == null)
        {
            songShulan = GameObject.Find("Anchor - shulan");
        }
        songShulan.SetActive(false);
        this.transform.parent.parent.gameObject.SetActive(true);
		sendManager.barSomeBody.value=true;
//		OnClick ();
		StartCoroutine (OpenSome());
    }

	public IEnumerator OpenSome()
	{
		yield return new WaitForSeconds(1);
		sendManager.barSomeBody.value=true;
		SelectCharBar.my.OnEnable ();
	}

    public TweenPosition tweenChat;
    public UILabel lblPlayerName;
    private string[] strSomeOne = new string[2];
    /// <summary>
    /// 主菜单左边竖排按钮
    /// </summary>
    public GameObject songShulan = null;
    public void ShowOne(object sender,object mID)
    {
        strSomeOne =(string[]) mID;
        lblPlayerName.text = strSomeOne[1];
        sendManager.OnSomeClick(this.gameObject);
        //myPanel.enabled=true;
        //tweenChat.Play(true);
//		Debug.Log ("nnnnnnnnnnnnnnnnnnnnn");
        //tweenChat.gameObject.SetActiveRecursively(true);
        if (songShulan == null)
        {
            songShulan = GameObject.Find("Anchor - shulan");

        }
		if(songShulan!=null)
		{ 
			songShulan.SetActive(false);
		}

        this.transform.parent.parent.gameObject.SetActive(true);
		sendManager.barSomeBody.value=true;
//		OnClick ();
		StartCoroutine (OpenSome());
    }
	


    public BtnGameManager btnGameManger;
    public Warnings warnings;
  
    /// <summary>
    /// 加好友
    /// </summary>
    public void AddFirendBtnClick()
    {
        if (strSomeOne!=null&& strSomeOne[0] != "")
        {
//            btnGameManger.AddFirend(strSomeOne[0]);
			InRoom.GetInRoomInstantiate ().FirendsAddInvitForName (strSomeOne[1]);
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info523"));
          
        }
    }

    /// <summary>
    /// 加黑名单
    /// </summary>
    public void AddBlackBtnClick()
    {
        if (strSomeOne != null && strSomeOne[0] != "")
        {

			PanelStatic.StaticBtnGameManager.BlackFirend(strSomeOne[0]);
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info523"));

        }
    }

    /// <summary>
    /// 移除好友
    /// </summary>
    public void RemoveFirendBtnClick()
    {
//		Debug.Log ("yyyyyyyyyyyyyyyyyyyyyyyy");
        warnings.warningAllEnterClose.Show(StaticLoc.Loc.Get("info358"),StaticLoc.Loc.Get("info510") );
        warnings.warningAllEnterClose.btnEnter.target = this.gameObject;
        warnings.warningAllEnterClose.btnEnter.functionName = "RemoveFirend";
       
    }

    public void RemoveFirend()
    {
		warnings.warningAllEnterClose.gameObject.SetActiveRecursively(false);
        if (strSomeOne != null && strSomeOne[0] != "")
        {
            string strMy = BtnGameManager.yt[0]["FriendsId"].YuanColumnText;
            if (null != strSomeOne[0] && strMy.IndexOf(strSomeOne[0].Trim()) == -1)
            {
                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info511"));
            }
            else
            {
                BtnGameManager.yt[0]["FriendsId"].YuanColumnText = BtnGameManager.yt[0]["FriendsId"].YuanColumnText.Replace(strSomeOne[0] + ";", "");
                warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), StaticLoc.Loc.Get("info512"));
            }
            
        }
        else
        {
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info359"), StaticLoc.Loc.Get("info523"));
        }
         
    }

    public void BtnTVCliek(object sender, object parm)
    {
        if (ckbTV.value)
        {
			
            //warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}{2}",StaticLoc.Loc.Get("info513") , YuanUnityPhoton.GetYuanUnityPhotonInstantiate().dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.TVMessageBlood], StaticLoc.Loc.Get("info514"))  );
            warnings.warningAllEnter.Show(StaticLoc.Loc.Get("info358"), string.Format("{0}{1}{2}", StaticLoc.Loc.Get("info513"), YuanUnityPhoton.dicBenefitsInfo[(byte)yuan.YuanPhoton.BenefitsType.TVMessageBlood], StaticLoc.Loc.Get("info514")));
			redBack.active=true;
        }
		else
		{
			redBack.active=false;
		}
    }
}

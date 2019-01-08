using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SendManager : MonoBehaviour
{

    public UIToggle barAll;
    public UIToggle barMainAll;
    public UIToggle barGuild;
    public UIToggle barTeam;
    public UIToggle barSomeBody;
    public UIToggle barSystem;

    public string strAll;
    public string strGuild;
    public string strTeam;
    public string strSomeBody;
    public List<Dictionary<short, object>> listMessage = new List<Dictionary<short, object>>();
    private yuan.YuanString ys = new yuan.YuanString();

    public delegate void DelegateManager(object[] parm);
    public event DelegateManager eventManagerMainAll;
    public event DelegateManager eventManagerAll;
    public event DelegateManager eventManagerGuild;
    public event DelegateManager eventManagerSomeBody;
    public event DelegateManager eventManagerTeam;
    public event DelegateManager eventManagerSystem;
    // Use this for initialization
	public static SendManager my;

    void Start()
    {
		my=this;
        InRoom.GetInRoomInstantiate().SM = this;
		//OnAllClick(this.gameObject);
    }

    [HideInInspector]
    public bool isSend = false;

    // Update is called once per frame
    void Update()
    {
        //if (isSend)
        //{
        //    this.MySendMessage();
        //    isSend = false;
        //}

        if (listMessage.Count > 0)
        {
            MySendMessage(listMessage[0]);
            listMessage.RemoveAt(0);
        }


    }

	public bool IsHasStart()
	{
		if(PanelStatic.StaticSendManager.eventManagerAll==null||
		   		      PanelStatic.StaticSendManager.eventManagerGuild==null|| 
		   		      PanelStatic.StaticSendManager.eventManagerSomeBody==null||
		   		      PanelStatic.StaticSendManager.eventManagerSystem==null||
		   		      PanelStatic.StaticSendManager.eventManagerTeam==null)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

    public void OnAllClick(GameObject mObj)
    {
		if(barAll!=null){
        barAll.value = true;
			if(SelectCharBar.my!=null)
			{
				SelectCharBar.my.OnEnable ();
			}
//			yuan.YuanClass.SwitchList (listBar,true,true);
//        yuan.YuanClass.SwitchListOnlyOne(listBar, 0, true, true);

		}
		}

    public void OnGuildClick(GameObject mObj)
    {
		if(barGuild!=null){
        barGuild.value = true;
			if(SelectCharBar.my!=null)
			{
				SelectCharBar.my.OnEnable ();
			}
			//			yuan.YuanClass.SwitchList (listBar,true,true);
//			yuan.YuanClass.SwitchListOnlyOne(listBar, 1, true, true);

		}
    }
    public void OnTeamClick(GameObject mObj)
    {
		if(barTeam!=null){
        barTeam.value = true;
			if(SelectCharBar.my!=null)
			{
				SelectCharBar.my.OnEnable ();
			}
			//			yuan.YuanClass.SwitchList (listBar,true,true);
//			yuan.YuanClass.SwitchListOnlyOne(listBar, 2, true, true);
		
		}
    }
    public void OnSomeClick(GameObject mObj)
    {
		if(barSomeBody!=null){
        barSomeBody.value = true;
			if(SelectCharBar.my!=null)
			{
				SelectCharBar.my.OnEnable ();
			}
			//			yuan.YuanClass.SwitchList (listBar,true,true);
//			yuan.YuanClass.SwitchListOnlyOne(listBar, 3, true, true);

		}
		}
    public void OnSystemClick(GameObject mObj)
    {
		if(barSystem!=null){
        barSystem.value = true;
			if(SelectCharBar.my!=null)
			{
				SelectCharBar.my.OnEnable ();
			}
//			yuan.YuanClass.SwitchList (listBar,true,true);
//			yuan.YuanClass.SwitchListOnlyOne(listBar, 4, true, true);
		
    }
	}



    public GameObject[] listBar;
    public void SetBarSwicth()
    {
        if (barAll!=null&&barAll.value)
        {
            yuan.YuanClass.SwitchListOnlyOne(listBar, 0, true, true);
        }
        else if (barGuild!=null&&barGuild.value)
        {
            yuan.YuanClass.SwitchListOnlyOne(listBar, 1, true, true);
        }
        else if (barTeam!=null&&barTeam.value)
        {
            yuan.YuanClass.SwitchListOnlyOne(listBar, 2, true, true);
        }
        else if (barSomeBody!=null&&barSomeBody.value)
        {
            yuan.YuanClass.SwitchListOnlyOne(listBar, 3, true, true);
        }
        else if (barSystem!=null&&barSystem.value)
        {
            yuan.YuanClass.SwitchListOnlyOne(listBar, 4, true, true);
        }
    }

    //[HideInInspector]
    //public yuan.YuanPhoton.MessageType messageType;
    //[HideInInspector]
    //public string messageText;
    //[HideInInspector]
    //public string messageSender;

    public void MySendMessage(Dictionary<short, object> parm)
    {
		SelectCharBar.AddTextList(parm);
		string messageText = (string)parm[(short)yuan.YuanPhoton.ParameterType.MessageText];
		string messageSender = (string)parm[(short)yuan.YuanPhoton.ParameterType.MessageSender];
		string messageSenderID = (string)parm[(short)yuan.YuanPhoton.ParameterType.MessageSenderID];
		yuan.YuanPhoton.MessageType messageType = (yuan.YuanPhoton.MessageType)(short)parm[(short)yuan.YuanPhoton.ParameterType.MessageType];
		SendMessage(messageText,messageSender,messageSenderID,messageType);
    }

	public void AddMessage(string messageText,string messageSender,string messageSenderID,yuan.YuanPhoton.MessageType messageType)
	{
		SendMessage(messageText,messageSender,messageSenderID,messageType);
		Dictionary<short,object> parms=new Dictionary<short, object>();
		parms.Add ((short)yuan.YuanPhoton.ParameterType.MessageText,messageText);
		parms.Add ((short)yuan.YuanPhoton.ParameterType.MessageSender,messageSender);
		parms.Add ((short)yuan.YuanPhoton.ParameterType.MessageSenderID,messageSenderID);
		parms.Add ((short)yuan.YuanPhoton.ParameterType.MessageType,(short)messageType);
		SelectCharBar.AddTextList(parms);
	}

	private void SendMessage(string messageText,string messageSender,string messageSenderID,yuan.YuanPhoton.MessageType messageType)
	{


		if (messageSenderID==""||
		    BtnGameManager.yt.Rows[0]["BlackFriendsId"].YuanColumnText.IndexOf(messageSenderID==null?"":messageSenderID) == -1 )
		{
			object[] parmChat = new object[4];
			//parmChat[0] = messageSender + ":" + messageText;
			parmChat[1] = (object)Color.white;
			parmChat[2] = messageSenderID;
			parmChat[3] = messageSender;

			switch (messageType)
			{
			case yuan.YuanPhoton.MessageType.All:
			{
				//barAll.SendMessage("AddText", messageSender+":"+messageText , SendMessageOptions.DontRequireReceiver);
				//barMainAll.SendMessage("AddText", messageSender+":"+messageText , SendMessageOptions.DontRequireReceiver);
				//Debug.Log("--------------------接收世界消息");
				//barAll.SendMessage("AddTextPlayer", parmChat, SendMessageOptions.DontRequireReceiver);
				//barMainAll.SendMessage("AddTextPlayer", parmChat, SendMessageOptions.DontRequireReceiver);
				parmChat[0]=string.Format ("[ffffff]{0}[-] [ffff00]{1}[-]:{2}",StaticLoc.Loc.Get("info812"),messageSender,messageText);
				if (eventManagerAll != null)
				{
					//Debug.Log ("11111111111111111");
					eventManagerAll(parmChat);
				}
				if (eventManagerMainAll != null)
				{
					//Debug.Log ("22222222222222222");
					eventManagerMainAll(parmChat);
				}
				
			}
				break;
			case yuan.YuanPhoton.MessageType.System:
			{
				messageText=messageText.Replace ("[ffff00]","");
				messageText=messageText.Replace ("[-]","");
				parmChat[0]=string.Format ("[ffff00]{0}[-] [ffff00]{1}[-]:{2}",StaticLoc.Loc.Get("info816"),messageSender,messageText);
				parmChat[1] = (object)new Color(1, 0.9f, 0, 1);
				if (eventManagerAll != null)
				{
					eventManagerAll(parmChat);
				}
				if (eventManagerSystem != null)
				{
					eventManagerSystem(parmChat);
				}
				if (eventManagerMainAll != null)
				{
					eventManagerMainAll(parmChat);
				}
				
			}
				break;
			case yuan.YuanPhoton.MessageType.Guild:
			{
				parmChat[0]=string.Format ("[00ff00]{0}[-] [ffff00]{1}[-]:{2}",StaticLoc.Loc.Get("info813"),messageSender,messageText);
				//barGuild.SendMessage("AddTextPlayer", parmChat, SendMessageOptions.DontRequireReceiver);
				//barMainAll.SendMessage("AddTextPlayer", parmChat, SendMessageOptions.DontRequireReceiver);
				//blue
				parmChat[1] = (object)Color.green;
				if (eventManagerGuild != null)
				{
					eventManagerGuild(parmChat);
				}
				if (eventManagerAll != null)
				{
					//Debug.Log ("11111111111111111");
					eventManagerAll(parmChat);
				}
				if (eventManagerMainAll != null)
				{
					eventManagerMainAll(parmChat);
				}
				
			}
				break;
			case yuan.YuanPhoton.MessageType.Somebody:
			{

				parmChat[0]=string.Format ("[ff00ff]{0}[-] [ffff00]{1}[-]:{2}",StaticLoc.Loc.Get("info815"),messageSender,messageText);
				//						parmChat[1] = (object)Color.cyan;
				parmChat[1] = (object)new Color(1, 0f, 1, 1);
				//barSomeBody.SendMessage("AddTextPlayer", parmChat, SendMessageOptions.DontRequireReceiver);
				//barMainAll.SendMessage("AddTextPlayer", parmChat, SendMessageOptions.DontRequireReceiver);
				if (eventManagerSomeBody != null)
				{
					eventManagerSomeBody(parmChat);
				}
				if (eventManagerAll != null)
				{
					//Debug.Log ("11111111111111111");
					eventManagerAll(parmChat);
				}
				if (eventManagerMainAll != null)
				{
					eventManagerMainAll(parmChat);
				}
			}
				break;
			case yuan.YuanPhoton.MessageType.Team:
			{
				parmChat[0]=string.Format ("[00f0ff]{0}[-] [ffff00]{1}[-]:{2}",StaticLoc.Loc.Get("info814"),messageSender,messageText);
				if (messageSender != BtnGameManager.yt.Rows[0]["PlayerName"].YuanColumnText)
				{
					parmChat[1] = (object)new Color(0, 0.65f, 1, 1);
				}
				else
				{
					parmChat[1] = (object)Color.white;
				}
				//barTeam.SendMessage("AddTextPlayer", parmChat, SendMessageOptions.DontRequireReceiver);
				//barMainAll.SendMessage("AddTextPlayer", parmChat, SendMessageOptions.DontRequireReceiver);
				if (eventManagerTeam != null)
				{
					eventManagerTeam(parmChat);
				}
				if (eventManagerAll != null)
				{
					//Debug.Log ("11111111111111111");
					eventManagerAll(parmChat);
				}
				if (eventManagerMainAll != null)
				{
					eventManagerMainAll(parmChat);
				}
			}
				break;
			}
		}


	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectCharBar : MonoBehaviour {
	public GameObject[] listBar;
	public UIToggle barAll;
	public UIToggle barMainAll;
	public UIToggle barGuild;
	public UIToggle barTeam;
	public UIToggle barSomeBody;
	public UIToggle barSystem;
	private static List<Dictionary<short,object>> listText=new List<Dictionary<short, object>>();

	public static SelectCharBar my;
	void Awake()
	{
		my=this;
	}

	// Use this for initialization
	IEnumerator Start () {
		yield return StartCoroutine( SetBarSwicth ());
		while(!PanelStatic.StaticSendManager.IsHasStart())
		{
			yield return new WaitForEndOfFrame();
		}

		GetTextList();
	}

	public void OnEnable()
	{
		StartCoroutine (Run ());
	}

	public static void AddTextList(Dictionary<short,object> mText)
	{
		if(my==null)
		{
			listText.Add (mText);
		}
	}

	private void GetTextList()
	{
		foreach(Dictionary<short,object> dic in listText)
		{
			string messageText = (string)dic[(short)yuan.YuanPhoton.ParameterType.MessageText];
			string messageSender = (string)dic[(short)yuan.YuanPhoton.ParameterType.MessageSender];
			string messageSenderID = (string)dic[(short)yuan.YuanPhoton.ParameterType.MessageSenderID];
			yuan.YuanPhoton.MessageType messageType = (yuan.YuanPhoton.MessageType)(short)dic[(short)yuan.YuanPhoton.ParameterType.MessageType];

			PanelStatic.StaticSendManager.AddMessage(messageText,messageSender,messageSenderID,messageType);
		}
		listText.Clear();
	}

	public IEnumerator Run()
	{
		yield return new WaitForSeconds(0.5f);
		StartCoroutine( SetBarSwicth ());
	}


	private Transform winSome;
	public IEnumerator SetBarSwicth()
	{
		yuan.YuanClass.SwitchList (listBar,true,true);
		yield return new WaitForEndOfFrame();
		if (barAll.value)
		{
			yuan.YuanClass.SwitchListOnlyOne(listBar, 0, true, true);
			listBar[0].SendMessage ("RefrshInputDisPic",SendMessageOptions.DontRequireReceiver);
		}
		else if (barGuild.value)
		{

			yuan.YuanClass.SwitchListOnlyOne(listBar, 1, true, true);
			listBar[1].SendMessage ("RefrshInputDisPic",SendMessageOptions.DontRequireReceiver);
		}
		else if (barTeam.value)
		{
			yuan.YuanClass.SwitchListOnlyOne(listBar, 2, true, true);
			listBar[2].SendMessage ("RefrshInputDisPic",SendMessageOptions.DontRequireReceiver);
		}
		else if (barSomeBody.value)
		{
			yuan.YuanClass.SwitchListOnlyOne(listBar, 3, true, true);

			if(winSome==null)
			{
				winSome = listBar[3].transform.FindChild ("WindowSome");
			}

			if(winSome!=null)
			{
				winSome.SendMessage ("RefrshInputDisPic",SendMessageOptions.DontRequireReceiver);
			}
		}
		else if (barSystem.value)
		{
			yuan.YuanClass.SwitchListOnlyOne(listBar, 4, true, true);
			listBar[4].SendMessage ("RefrshInputDisPic",SendMessageOptions.DontRequireReceiver);
		}
	}

//	void Update()
//	{
//		OnGUI();
//	}
//	public GameObject ObjMy;
//	public GameObject SonObj;
//	public string  MyStr ;
//
//	void OnGUI (){
//		GUI.Label(new Rect(0,80,500,500), "ran===============================Postion"+ObjMy.transform.localPosition.ToString());
//	
//		GUI.Label(new Rect(0,100,300,300), "ran------------------------------childCount"+SonObj.transform.childCount.ToString());
//
//		GUI.Label(new Rect(0,200,500,500), "ran------------------------------childCount"+SonObj.transform.localPosition.ToString());
//	}

}

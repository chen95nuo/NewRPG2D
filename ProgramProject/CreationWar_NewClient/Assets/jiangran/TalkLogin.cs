using UnityEngine;
using System.Collections;

public class TalkLogin : MonoBehaviour {
	public GameObject ListBarwindowAll;
	public GameObject ListBarwindowGuild;
	public GameObject ListBarwindowSome;
	public GameObject ListBarwindowSystem;
	public GameObject ListBarwindowTeam;
	
	public CharBar windowAll;
	public CharBar windowGuild;
	public CharBar windowSome;
	public CharBar windowSystem;
	public CharBar windowTeam;
	public UIToggle ckbAll;
	public UIToggle ckbGuild;
	public UIToggle ckbSome;
	public UIToggle ckbSystem;
	public UIToggle ckbTeam;
	public BtnSend btnSend;
	public BtnExpression btnExpr;
	public LoadExpression loadExpr;
	
	
	void Awake()
	{

		SetWindow (windowAll);
		SetWindow (windowGuild);
		SetWindow (windowSome);
		SetWindow (windowSystem);
		SetWindow (windowTeam);
		SetStaticSendManager ();
		SetBtnSend ();
		SetBtnExpression ();
		SetLoadExpression ();
		PanelStatic.StaticSendManager.SetBarSwicth ();
		btnSend.Awake ();
	}


	
	void SetLoadExpression()
	{
		loadExpr.input=TalkLoginStatic.my.yuanInput.gameObject;
	}
	
	void SetStaticSendManager()
	{
		
		PanelStatic.StaticSendManager.barAll=ckbAll;
		PanelStatic.StaticSendManager.barGuild=ckbGuild;
		PanelStatic.StaticSendManager.barSomeBody=ckbSome;
		PanelStatic.StaticSendManager.barSystem=ckbSystem;
		PanelStatic.StaticSendManager.barTeam=ckbTeam;
        PanelStatic.StaticBtnGameManager.btnSend = btnSend.gameObject;
		
		PanelStatic.StaticSendManager.listBar[0] = ListBarwindowAll;
		PanelStatic.StaticSendManager.listBar[1] = ListBarwindowGuild;
		PanelStatic.StaticSendManager.listBar[2] = ListBarwindowTeam;                    
		PanelStatic.StaticSendManager.listBar[3] = ListBarwindowSome;
		PanelStatic.StaticSendManager.listBar[4] = ListBarwindowSystem;
		
				
	}
	
	void SetWindow(CharBar mBar)
	{
		mBar.invMaker=PanelStatic.StaticBtnGameManager.InvMake;
		mBar.sendManager=PanelStatic.StaticSendManager;
		mBar.getInput=TalkLoginStatic.my.yuanInput;
		mBar.infoBar=PanelStatic.StaticIteminfo.GetComponent<UIPanel>();

	}
	
	void SetBtnSend()
	{
		btnSend.yInput=TalkLoginStatic.my.yuanInput;
	}
	
	void SetBtnExpression()
	{
		btnExpr.yInput=TalkLoginStatic.my.yuanInput;
	}
}

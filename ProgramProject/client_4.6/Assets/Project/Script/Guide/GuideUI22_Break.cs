using UnityEngine;
using System.Collections;

public class GuideUI22_Break : GuideBase
{
	public static GuideUI22_Break mInstance = null;
	
	public UILabel label0;
	public UILabel label1;
	public UILabel label2;
	public UILabel label3;
    public GuideTeamPos guideTeamPos; 
	//int targetCardID = 14001;
    public Vector3[] buttonPositions = new Vector3[6];

    public GameObject button;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = gameObject;
		init ();
	}
	
	// Use this for initialization
	void Start ()
	{
		close ();
	}
	
	public override void init ()
	{
		base.init();
        label0.text = TextsData.getData(176).chinese;
		label1.text = TextsData.getData(176).chinese;
        label2.text = TextsData.getData(222).chinese;
		label3.text = TextsData.getData(252).chinese;
		setClickBtnCount(4);
        buttonPositions[0] = new Vector3(276f, -154f, 0f);
        buttonPositions[1] = new Vector3(96f, -154f, 0f);
        buttonPositions[2] = new Vector3(-96f, -154f, 0f);
        buttonPositions[3] = new Vector3(276f, -52f, 0f);
        buttonPositions[4] = new Vector3(96f, -52f, 0f);
        buttonPositions[5] = new Vector3(-96f, -52f, 0f);
	}
    public override void showStep(int step)
    {
        base.showStep(step);
        if (step == 1)
        {
            CombinationInterManager combination = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP, "CombinationInterManager") as CombinationInterManager;
            int pos = combination.findCardGroupFirstExistCardPos();
            guideTeamPos.showPosCtrl(pos, TextsData.getData(290).chinese);
        }
    }

    public void SetPosition(int index)
    {
        stepObjList[1].transform.localPosition = buttonPositions[1];
    }
	public void onClickBreakBtn()
	{
		if(btnClickList[0])
		{
			return;
		}
		btnClickList[0] = true;
       
//		MainMenuManager.mInstance.openCardBreakPanel();
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
            main.openCardGroup();
		}
		
	}
  
	public void onClickTargetCard()
	{
		if(btnClickList[1])
		{
			return;
		}
		btnClickList[1] = true;
        //		CardInfoPanelManager.mInstance.openCardList(0);
        UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO);
        CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager") as CardInfoPanelManager;
        cardInfo.curCardBoxId = 2;
        cardInfo.show();
        UISceneStateControl.mInstace.HideObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CARDGROUP);
				
	}
	
	public void onClickConsumeCard()
	{
		if(btnClickList[2])
		{
			return;
		}
		btnClickList[2] = true;
//		CardBreakPanel.mIntance.onSelectConsumeCardItem(CardBreakPanel.mIntance.getGuideTargetIndex());
        CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager") as CardInfoPanelManager;
        cardInfo.ciControl.GetComponent<SpreadManager>().onClickSpread(3);
	}
	
	public void onClickBreak()
	{
		if(btnClickList[3])
		{
			return;
		}
		btnClickList[3] = true;
//		CardBreakPanel.mIntance.doBreak();
        CardInfoPanelManager cardInfo = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_CGINFO, "CardInfoPanelManager") as CardInfoPanelManager;

        cardInfo.BreakClick(0);
	}
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//第3次战斗&好友 //
public class GuideUI_Friend: GuideBase
{
	public static GuideUI_Friend mInstance = null;
	public int step = 0;
	
	public UILabel label0;
	public UILabel label1;
	
	public FriendElement guideFriend = null;
	public int missionID = 110201;
	
	void Awake()
	{
		mInstance = this;
		_MyObj = gameObject;
		init();
	}
	
	// Use this for initialization
	void Start ()
	{
		close();
	}
	
	public override void init()
	{
		base.init();
		label0.text = TextsData.getData(587).chinese;
		label1.text = TextsData.getData(588).chinese;
		setClickBtnCount(2);
	}

	public void onClickFriend1()
	{
		if(btnClickList[0])
		{
			return;	
		}
		btnClickList[0] = true;
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP);
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
			"MissionUI")as MissionUI;
		mission.onClickSelectHelper(guideFriend.pid);
		showStep(1);
	}
	
	public void onClickEnterBattleBtn()
	{
		if(btnClickList[1])
		{
			return;
		}
		btnClickList[1] = true;
		MissionUI mission = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAP,
			"MissionUI")as MissionUI;
		mission.onClickEnterMission();
	}

}

using UnityEngine;
using System.Collections;

public class FriendItem : MonoBehaviour {
	
	public UISprite frame;
	public UISprite icon;
	public UILabel level;
	public UILabel nameLabel;
	public UILabel lastLogin;
	public UISprite powerType;
	
	private float time;
	private bool pressType;
	
	private int unitId;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(pressType)
		{
			time+=Time.deltaTime;
			if(time>=1f)
			{
				pressType=false;
				time=0;
//				FriendUI.mInstance.unitDetail.GetComponent<FriendUnit>().showText(UnitSkillData.getData(unitId).description);
				FriendUI friend = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_FRIEND, "FriendUI") as FriendUI;
				friend.unitDetail.GetComponent<FriendUnit>().showText(UnitSkillData.getData(unitId).description);
			}
		}
	}
	
	public void setData(FriendElement fe)
	{
		unitId=fe.unit;
		//UnitSkillData usd=UnitSkillData.getData(fe.unit);
		icon.spriteName=fe.icon;
		
		//设置图集//
		string iconAtlasName = CardData.getAtlas(fe.icon);
		UIAtlas iconAtlas = LoadAtlasOrFont.LoadAtlasByName(iconAtlasName);
		icon.atlas = iconAtlas;
		
		level.text="Lv. "+fe.level;
		nameLabel.text=fe.name;
		lastLogin.text=TextsData.getData(95).chinese+"："+fe.login;
		//TODO powerType
	}
	
	public void highLight()
	{
		frame.spriteName="exchange_frame_sel";
	}
	
	public void lowLight()
	{
		frame.spriteName="exchange_farme01";
	}
	
	void OnPress(bool isPress)
	{
		pressType=isPress;
		time=0;
	}
	
}

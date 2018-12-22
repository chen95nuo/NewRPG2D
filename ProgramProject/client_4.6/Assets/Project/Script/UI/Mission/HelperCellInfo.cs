using UnityEngine;
using System.Collections;

public class HelperCellInfo : MonoBehaviour {
	
	public GameObject friend;
	public GameObject stranger;
	public UILabel friendValue;
	public UILabel playerName;
	public UILabel playerPower;
	public UISprite playerIconSprite;
	public UISprite unitIconSprite;
	public UILabel unitNameLabel;
	public UILabel unitWaitLabel;
	public GameObject selectBtn;
	public UILabel costLabel;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setData(FriendElement helper,int nextInviteCost,GameObject target,string functionName)
	{
		int addFriendNum=0; 
		if(helper.t==0)
		{
			friend.SetActive(true);
			friend.GetComponent<UILabel>().text=TextsData.getData(139).chinese;
			stranger.SetActive(false);
			addFriendNum=25;
		}
		else
		{
			friend.SetActive(false);
			stranger.SetActive(true);
			stranger.GetComponent<UILabel>().text=TextsData.getData(140).chinese;
			addFriendNum=10;
			costLabel.text=nextInviteCost+"";
		}
		friendValue.text="+"+addFriendNum;
		playerName.text=helper.name;
		playerPower.text=TextsData.getData(203).chinese+helper.bp;
		string playerIcon=helper.icon;
		string atlasName=CardData.getAtlas(playerIcon);
		UIAtlas atlas=LoadAtlasOrFont.LoadHeroAtlasByName(atlasName);
		playerIconSprite.atlas=atlas;
		playerIconSprite.spriteName=playerIcon;
		UnitSkillData usd=UnitSkillData.getData(helper.unit);
		unitIconSprite.spriteName=usd.icon;
		//设置合体技按钮的回调事件//
		HelperUniteIconControl huic = unitIconSprite.GetComponent<HelperUniteIconControl>();
		huic.UniteSkillId = helper.unit;
		
		unitNameLabel.GetComponent<UILabel>().text=usd.name;
		
		int waitRound=FriendenergyData.getNumber(usd.index)-1;
		unitWaitLabel.text=TextsData.getData(460).chinese.Replace("num",waitRound+"");
		
		UIButtonMessage message=selectBtn.GetComponent<UIButtonMessage>();
		message.target=target;
		message.functionName=functionName;
		message.param=helper.pid;
	}
}

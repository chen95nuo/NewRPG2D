using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ACMItem : MonoBehaviour 
{
	public GameObject _MyObj ;
	  
	public int acmID;
	bool isCanFinishFlag;
	
	public GameObject finishBGObj;
	public GameObject unfinishBGObj;
	public UISprite acmIcon;
	public UIAtlas iconAtlas;
	public UILabel acmNameText;
	public UILabel acmDescText;
	public UILabel unFinishText;
	public UIButtonMessage gainRewardBM;

	public List<GameObject> rewardObjList;
	
	// Use this for initialization
	void Awake()
	{
		_MyObj = this.gameObject;
		if(gainRewardBM)
		{
			GameObject achive = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACHIVEMENT);
			if(achive != null)
			{
				gainRewardBM.target = achive;
			}
			gainRewardBM.functionName = "finishACM";
		}
	}
	
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void show(int id,bool isCanFinish)
	{
		AchievementData ad = AchievementData.getData(id);
		if(ad == null)
			return;
		acmID = id;
		_MyObj.SetActive(true);
		acmNameText.text = ad.name;
		acmDescText.text =ad.description;
		acmIcon.atlas = iconAtlas;
		
		acmIcon.spriteName = ad.icon;
		showReward(ad.reward);
		
		isCanFinishFlag = isCanFinish;
		// temp
		if(isCanFinishFlag)
		{
			finishBGObj.SetActive(true);
			unfinishBGObj.SetActive(false);
			gameObject.GetComponent<UIButtonScale>().tweenTarget = transform;
		}
		else
		{
			finishBGObj.SetActive(false);
			unfinishBGObj.SetActive(true);
			showUnFinishInfo();
			gameObject.GetComponent<UIButtonScale>().tweenTarget = transform;
		}
	
	}
	
	public void hide()
	{
		_MyObj.SetActive(false);
	}
	
	public void finishACM()
	{

		AchievementPanel achivement = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACHIVEMENT, 
			"AchievementPanel") as AchievementPanel;
		if(achivement != null)
		{
			achivement.finishACM(acmID);
		}
		
	}
	
	public void showReward(List<string> rewardList)
	{
		for(int i = 0; i < rewardObjList.Count;++i)
		{
			rewardObjList[i].SetActive(false);
		}
		
		for(int i = 0; i < rewardList.Count; ++i)
		{
			if(i > 1)
			{
				return;
			}
			rewardObjList[i].SetActive(true);
			string rewardStr = rewardList[i];
			string[] ss = rewardStr.Split('-');
			int rewardType = StringUtil.getInt(ss[0]);
			string rewardText = StringUtil.getString(ss[1]);
			GameObject rewardObj = rewardObjList[i];
			rewardObj.SetActive(true);
			UISprite rewardIconBG = rewardObj.transform.FindChild("IconBG").GetComponent<UISprite>();
			rewardIconBG.gameObject.SetActive(false);
			UILabel rewardLabel = rewardObj.transform.FindChild("Text").GetComponent<UILabel>();
			rewardLabel.text = string.Empty;
			SimpleCardInfo2 cardInfo = rewardObj.transform.FindChild("CardInfo").GetComponent<SimpleCardInfo2>();
			cardInfo.clear();
			cardInfo.gameObject.SetActive(false);
			
			rewardLabel.text = string.Empty;
			
			//获得achivement组建//
			AchievementPanel achivement = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACHIVEMENT, 
					"AchievementPanel") as AchievementPanel;

			//string iconFrameName = achivement.getIconFrameSpriteName();
			string expSpriteName = achivement.getRewardExpSpriteName();
			string goldSpriteName = achivement.getRewardGoldSpriteName();
			string crystalSpriteName = achivement.getRewardCrystalSpriteName();
			string friendshipSpriteName = achivement.getRewardFriendshipSpriteName();
			
			switch(rewardType)
			{
			case (int)AchievementPanel.RewardType.E_Item:
			{
				string[] tempS = rewardText.Split(',');
				int itemID = StringUtil.getInt(tempS[0]);
				int num = StringUtil.getInt(tempS[1]);
				ItemsData itemData = ItemsData.getData(itemID);
				if(itemData == null)
				{
					rewardObj.SetActive(false);
					continue;
				}
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(itemID,GameHelper.E_CardType.E_Item);
				rewardLabel.text = itemData.name + " x " + num.ToString();
				
				
			}break;
			case (int)AchievementPanel.RewardType.E_Equip:
			{
				string[] tempS = rewardText.Split(',');
				int equipID = StringUtil.getInt(tempS[0]);
				int num = StringUtil.getInt(tempS[1]);
				
				EquipData ed = EquipData.getData(equipID);
				if(ed == null)
				{
					rewardObj.SetActive(false);
					continue;
				}
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(equipID,GameHelper.E_CardType.E_Equip);				
				rewardLabel.text = ed.name + " x " + num.ToString();
			}break;
			case (int)AchievementPanel.RewardType.E_Hero:
			{
				string[] tempS = rewardText.Split(',');
				int heroID = StringUtil.getInt(tempS[0]);
				int num = StringUtil.getInt(tempS[1]);
				
				CardData cd = CardData.getData(heroID);
				if(cd == null)
				{
					rewardObj.SetActive(false);
					continue;
				}
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(heroID,GameHelper.E_CardType.E_Hero);
				rewardLabel.text = cd.name + " x " + num.ToString();
				
			}break;
			case (int)AchievementPanel.RewardType.E_Skill:
			{
				string[] tempS = rewardText.Split(',');
				int skillID = StringUtil.getInt(tempS[0]);
				int num = StringUtil.getInt(tempS[1]);
				
				SkillData sd = SkillData.getData(skillID);
				if(sd == null)
				{
					rewardObj.SetActive(false);
					continue;
				}
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(skillID,GameHelper.E_CardType.E_Skill);				
				rewardLabel.text = sd.name + " x " + num.ToString();
				
			}break;
			case (int)AchievementPanel.RewardType.E_PassiveSkill:
			{
				string[] tempS = rewardText.Split(',');
				int passiveSkillID = StringUtil.getInt(tempS[0]);
				int num = StringUtil.getInt(tempS[1]);
				
				PassiveSkillData psd = PassiveSkillData.getData(passiveSkillID);
				if(psd == null)
				{
					rewardObj.SetActive(false);
					continue;
				}
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(passiveSkillID,GameHelper.E_CardType.E_PassiveSkill);				
				rewardLabel.text = psd.name + " x " + num.ToString();
			}break;
			case (int)AchievementPanel.RewardType.E_Gold:
			{
				rewardIconBG.gameObject.SetActive(true);
				rewardIconBG.atlas = achivement.otherAtlas;
				rewardIconBG.spriteName = goldSpriteName;
				rewardLabel.text = "x " + rewardText;
			}break;
			case (int)AchievementPanel.RewardType.E_Exp:
			{
				rewardIconBG.gameObject.SetActive(true);
				rewardIconBG.atlas = achivement.otherAtlas;
				rewardIconBG.spriteName = expSpriteName;
				rewardLabel.text = "x " + rewardText;
			}break;
			case (int)AchievementPanel.RewardType.E_Crystal:
			{
				rewardIconBG.gameObject.SetActive(true);
				rewardIconBG.atlas = achivement.otherAtlas;
				rewardIconBG.spriteName = crystalSpriteName;
				rewardLabel.text = "x " + rewardText;
			}break;
			case (int)AchievementPanel.RewardType.E_Friendship:
			{
				rewardIconBG.gameObject.SetActive(true);
				rewardIconBG.atlas = achivement.friendshipAtlas;
				rewardIconBG.spriteName = friendshipSpriteName;
				rewardLabel.text = "x " + rewardText;
			}break;
			}
		}
	}
	
	public void showUnFinishInfo()
	{
		AchievementData acmData = AchievementData.getData(acmID);
		if(acmData == null)
			return;
		if(acmData.type == 1)
		{
			int playerLevel = PlayerInfo.getInstance().player.level;
			int requestLevel = StringUtil.getInt(acmData.request);
			unFinishText.text = playerLevel + "/" + requestLevel;
		}
		else
		{
			unFinishText.text = string.Empty;
		}
	}
	
	void OnClick() 
	{

		if(isCanFinishFlag)
		{
			finishACM();
		}
		else
		{
			return;
		}
	}
	
}

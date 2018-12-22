using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementPanel : MonoBehaviour,ProcessResponse
{
//	public static AchievementPanel mInstance = null;
	
	public GameObject tipCtrl;
	public UIPanel scrollListPanel;
	public UIGrid grid;
	
	public enum RewardType
	{
		E_Null = 0,
		E_Item = 1,
		E_Equip = 2,
		E_Hero = 3,
		E_Skill = 4,
		E_PassiveSkill = 5,
		E_Gold = 6,
		E_Exp = 7,
		E_Crystal = 8,
		E_Friendship = 9,
	}
	
	public UIAtlas itemAtlas;
	public UIAtlas equipAtlas;
	public UIAtlas heroAtlas;
	public UIAtlas skillAtlas;
	public UIAtlas passiveAtlas;
	public UIAtlas otherAtlas;
	public UIAtlas iconFrameAtlas;
	public UIAtlas friendshipAtlas;
	string iconFrameName = "head_star_";
	string expSpriteName = "reward_exp";
	string crystalSpriteName = "reward_crystal";
	string goldSpriteName = "reward_gold";
	string friendshipSpriteName = "icon_02";
	
	GameObject acmItemPrefab = null;
	
	public UILabel tipName;
	public List<GameObject> tipRewardList;
	
	public AchieveResultJson arj;
	
	private int requestType;
	private bool receiveData;
	int finishACMID = -1;
	
	public bool isAchievementDialog = false;
	
	
	void Awake()
	{
		init();
	}
	
	void Update()
	{
		if(receiveData)
		{
			receiveData=false;
			switch(requestType)
			{
			case 1:
			{
				if(arj.errorCode == 0)
				{
					refreshList();
					showTipRewardWnd(finishACMID);
					if(GuideManager.getInstance().isRunningGuideID((int)GuideManager.GuideType.E_Achievement))
					{
						GuideUI8_Achievement.mInstance.hideAllStep();
						GuideManager.getInstance().finishGuide((int)GuideManager.GuideType.E_Achievement);
					}
					finishACMID = -1;
					HeadUI.mInstance.requestPlayerInfo();
					
				}
			}break;
			}
		}
	}
	
	public void init()
	{
	}

	public void show()
	{
		tipCtrl.SetActive(false);
        Main3dCameraControl.mInstance.SetBool(true);
		resetScrollList();
		refreshList();
	}
	
	public void hide()
	{
		TalkMainToGetData();
        Main3dCameraControl.mInstance.SetBool(false);
//		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
//		MainMenuManager main = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU,
//			"MainMenuManager") as MainMenuManager;
//		main.SetData(STATE.ENTER_MAINMENU_BACK);
		UISceneStateControl.mInstace.DestoryObj(UISceneStateControl.UI_STATE_TYPE.UI_STATE_ACHIVEMENT);
		gc();
	}
	
	//cxl---通知主界面发请求//
	public void TalkMainToGetData()
	{
		UISceneStateControl.mInstace.ChangeState(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);
		GameObject obj = UISceneStateControl.mInstace.GetObjByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_MAINMENU);;
		if(obj!=null)
		{
			MainMenuManager main = obj.GetComponent<MainMenuManager>();
			if(main!= null && obj.activeSelf)
			{
				main.SendToGetData();
			}
		}
	}
	
	public void gc()
	{
		//==释放资源==//
		acmItemPrefab=null;
		itemAtlas = null;
		equipAtlas = null;
		heroAtlas = null;
		skillAtlas = null;
		passiveAtlas = null;
		otherAtlas = null;
		iconFrameAtlas = null;
		friendshipAtlas = null;

		Resources.UnloadUnusedAssets();
		arj=null;
	}
	
	public void closePanel()
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_BACK);
		
		GameObjectUtil.destroyGameObjectAllChildrens(grid.gameObject);
		gameObject.SetActive(false);
		hide ();
		
	}
	
	public void resetScrollList()
	{
		scrollListPanel.transform.localPosition = Vector3.zero;
		scrollListPanel.clipRange = new Vector4(0,0,715,380);
		grid.repositionNow = true;
	}
	
	//==刷新列表==//
	public void refreshList()
	{
		GameObjectUtil.destroyGameObjectAllChildrens(grid.gameObject);
		List<string> finishAddList = new List<string>();
		List<string> unFinishAddList=new List<string>();
		
		string achieveInfo=arj.ac;
		string[] ss=achieveInfo.Split('-');
		for(int i=0;i<ss.Length;i++)
		{
			string[] temp=ss[i].Split('%');
			int achieveId=StringUtil.getInt(temp[0]);
			AchievementData ad = AchievementData.getData(achieveId);
			if(ad == null)
				continue;
			if(ad.disable == 1)
				continue;
			int type=StringUtil.getInt(temp[1]);
			//==获取之前的成就==//
//			List<int> preIds=AchievementData.getPreAchieve(achieveId);
//			for(int k=0;k<preIds.Count;k++)
//			{
//				string s=preIds[k]+"-"+0;
//				if(!addList.Contains(s))
//				{
//					addList.Add(s);
//				}
//			}
			//==0表示已完成但还未领取奖励,1表示已领取奖励,2表示还未完成==//
			if(type==0)
			{
				string s=achieveId+"-"+1;
				finishAddList.Add(s);
			}
			else if(type==1)
			{
				List<AchievementData> nexts=AchievementData.getNextAchieveMent(achieveId);
				for(int n=0;n<nexts.Count;n++)
				{
					string s=nexts[n].id+"-"+0;
					unFinishAddList.Add(s);
				}
			}
			else if(type==2)
			{
				string s=achieveId+"-"+0;
				unFinishAddList.Add(s);
			}
		}
		//==加载==//
		for(int i=0;i<finishAddList.Count;i++)
		{
			string[] temp=(finishAddList[i]).Split('-');
			int acmID=StringUtil.getInt(temp[0]);
			bool isCanFinish=StringUtil.getInt(temp[1])==1;
			setAndGetACMItem(acmID,isCanFinish);
		}
		
		for(int i=0;i<unFinishAddList.Count;i++)
		{
			string[] temp=(unFinishAddList[i]).Split('-');
			int acmID=StringUtil.getInt(temp[0]);
			bool isCanFinish=StringUtil.getInt(temp[1])==1;
			setAndGetACMItem(acmID,isCanFinish);
		}
		
		grid.repositionNow = true;
	}
	
	public void setAndGetACMItem(int acmID,bool isCanFinish)
	{
		if(acmItemPrefab==null)
		{
			acmItemPrefab = Resources.Load("Prefabs/UI/Achievement/ACMItem") as GameObject;
		}
		GameObject obj = GameObject.Instantiate(acmItemPrefab) as GameObject;
		GameObjectUtil.gameObjectAttachToParent(obj,grid.gameObject);
		ACMItem acmItem = obj.GetComponent<ACMItem>();
		acmItem.show(acmID,isCanFinish);
	}
	
	//==获取成就奖励==//
	public void finishACM(int id)
	{
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		requestType=1;
		finishACMID = id;
		PlayerInfo.getInstance().sendRequest(new AchieveRewardJson(id),this);
	}
	
	
	public void showTipRewardWnd(int acmID)
	{
		string eventName = "achievement";
		Dictionary<string,object> aep = new Dictionary<string, object>();
		aep.Add("palyerId",PlayerPrefs.GetString("username"));
		tipCtrl.SetActive(true);
		AchievementData acmData = AchievementData.getData(acmID);
		if(acmData == null)
			return;
		tipName.text = TextsData.getData(165).chinese + acmData.name;
		List<string> rewardList = acmData.reward;
		
		
		for(int i = 0; i < tipRewardList.Count;++i)
		{
			tipRewardList[i].SetActive(false);
		}
		
		for(int i = 0; i < rewardList.Count; ++i)
		{
			if(i > 1)
			{
				return;
			}
			tipRewardList[i].SetActive(true);
			string rewardStr = rewardList[i];
			string[] ss = rewardStr.Split('-');
			int rewardType = StringUtil.getInt(ss[0]);
			string rewardText = StringUtil.getString(ss[1]);
			GameObject rewardObj = tipRewardList[i];
			rewardObj.SetActive(true);
			UISprite rewardIconBG = rewardObj.transform.FindChild("IconBG").GetComponent<UISprite>();
			rewardIconBG.gameObject.SetActive(false);
			UILabel rewardLabel = rewardObj.transform.FindChild("Text").GetComponent<UILabel>();
			rewardLabel.text = string.Empty;
			SimpleCardInfo2 cardInfo = rewardObj.transform.FindChild("CardInfo").GetComponent<SimpleCardInfo2>();
			cardInfo.clear();
			cardInfo.gameObject.SetActive(false);
			rewardLabel.text = string.Empty;
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
				
				aep.Add("RewardItem"+i,itemID.ToString());
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
				
				aep.Add("RewardEquip"+i,equipID.ToString());
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
				
				aep.Add("RewardHero"+i,heroID.ToString());
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
				
				aep.Add("RewardSkill"+i,skillID.ToString());
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
				
				aep.Add("RewardPassiveSkill"+i,passiveSkillID.ToString());
			}break;
			case (int)AchievementPanel.RewardType.E_Gold:
			{
				rewardIconBG.gameObject.SetActive(true);
				rewardIconBG.atlas = otherAtlas;
				rewardIconBG.spriteName = goldSpriteName;
				rewardLabel.text = "x " + rewardText;
				aep.Add("RewardGold"+i,rewardText);
			}break;
			case (int)AchievementPanel.RewardType.E_Exp:
			{
				rewardIconBG.gameObject.SetActive(true);
				rewardIconBG.atlas = otherAtlas;
				rewardIconBG.spriteName = expSpriteName;
				rewardLabel.text = "x " + rewardText;
				aep.Add("RewardExp"+i,rewardText);
			}break;
			case (int)AchievementPanel.RewardType.E_Crystal:
			{
				rewardIconBG.gameObject.SetActive(true);
				rewardIconBG.atlas = otherAtlas;
				rewardIconBG.spriteName = crystalSpriteName;
				rewardLabel.text = "x " + rewardText;
				aep.Add("RewardCrystal"+i,rewardText);
				if(!TalkingDataManager.isTDPC)
				{
					TDGAVirtualCurrency.OnReward(StringUtil.getInt(rewardText),TextsData.getData(255).chinese);//统计获赠钻石//
				}
			}break;
			case (int)AchievementPanel.RewardType.E_Friendship:
			{
				rewardIconBG.gameObject.SetActive(true);
				rewardIconBG.atlas = friendshipAtlas;
				rewardIconBG.spriteName = friendshipSpriteName;
				rewardLabel.text = "x " + rewardText;
				aep.Add("RewardFriendship"+i,rewardText);
			}break;
			}
		}
		TalkingDataManager.SendTalkingDataEvent(eventName,aep);
	}
	
	public void closeTipWnd()
	{
		
		//播放音效//
		MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);
		
		tipCtrl.SetActive(false);
		
		//TODO
	}
	
	public void receiveResponse(string json)
	{
		if(json!=null)
		{
			//关闭连接界面的动画//
			PlayerInfo.getInstance().isShowConnectObj = false;
			switch(requestType)
			{
			case 1:
			{
				arj=JsonMapper.ToObject<AchieveResultJson>(json);
				
				receiveData=true;
			}break;
			}
		}
	}
	public string getIconFrameSpriteName()
	{
		return iconFrameName;
	}
	public string getRewardExpSpriteName()
	{
		return expSpriteName;
	}
	public string getRewardGoldSpriteName()
	{
		return goldSpriteName;
	}
	public string getRewardCrystalSpriteName()
	{
		return crystalSpriteName;
	}
	public string getRewardFriendshipSpriteName()
	{
		return friendshipSpriteName;
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SweepItemControl : MonoBehaviour {
	
	public UILabel TitleLabel;
	public UILabel GroupExpLabel;
	public UILabel CardExpLabe;
	public UILabel RewardsLabel;
	public List<GameObject> Rewards = new List<GameObject>();
	public GameObject Line;
	public GameObject NoRewardsLabe;
	
	private bool isStartShow;
	float frameCount;
	//0显示title, 1 显示军团经验，和卡牌经验，2 显示获得奖励//
	int step;	
	//轮数//
	int sweepTimes;
	int groupExpNum;
	int cardExpNum;
	List<string> getRews = new List<string>();
	//string itemAtlasName = "ItemCircularIcon";
	//string equpiAtlasName = "EquipCircularIcon";
	//string cardAtlasName = "HeadIcon";
	//string skillAtlasName = "SkillCircularIcom";
	//string passSkillAtlasName = "PassSkillCircularIcon";
	
	//bool startFrameCount = false;
	SweepUIManager sweepUI;
	// Use this for initialization
	void Start () {
		sweepUI = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SWEEP, "SweepUIManager") as SweepUIManager;
	}
	
	// Update is called once per frame
	void Update () {
		if(isStartShow)
		{
			if(frameCount > 0.5f)
			{
				frameCount = 0;
				ChangeState(step);
			}
			frameCount += Time.deltaTime;
		}
	}
	
	public void ChangeState(int state)
	{
		if(sweepUI==null)
		{
			sweepUI = UISceneStateControl.mInstace.GetComponentByType(UISceneStateControl.UI_STATE_TYPE.UI_STATE_SWEEP, "SweepUIManager") as SweepUIManager;	
		}
		switch(state)
		{
		case 0:				//显示轮数//
			ShowTitle();
			step ++;
			break;
		case 1:				//显示经验//
			ShowExp();
			step ++;
			break;
		case 2:				//显示掉落//
			ShowDropRewards(getRews);
			step ++;
			break;
		case 3:				//告诉sweepUiManager显示结束//
//			SweepUIManager.mInstance.SetDrawSweep(false);
			sweepUI.SetDrawSweep(false);
			frameCount = 0;
			isStartShow = false;
			step = 0;
			break;
		}
		
		sweepUI.SetScrollBar();
	}
	
	//sweepTimes 扫荡的轮数, 
	public void SetData(int sweepTimes, SweepCardJson scj)
	{
		
		this.sweepTimes = sweepTimes;
		this.groupExpNum = scj.playerExp;
		this.cardExpNum = scj.cardExp;
		this.getRews = scj.ds;
		this.step = 0;
		this.isStartShow = true;
		ChangeState(step);
	}
	
	//显示轮数//
	public void ShowTitle()
	{	
		string s1 = TextsData.getData(317).chinese;
		string s2 = TextsData.getData(330).chinese;
		TitleLabel.gameObject.SetActive(true);
		TitleLabel.text = s1 + sweepTimes + s2;
		for(int i = 0;i < Rewards.Count;i++)
		{
			GameObject obj = Rewards[i];
			obj.SetActive(false);
		}
	}
	
	//显示经验值//
	public void ShowExp()
	{
		GroupExpLabel.gameObject.SetActive(true);
		CardExpLabe.gameObject.SetActive(true);
		string s1 = TextsData.getData(331).chinese;		//军团经验//
		GroupExpLabel.text = s1 + groupExpNum;
		s1 = TextsData.getData(332).chinese;			//人物经验//
		CardExpLabe.text = s1 + cardExpNum;
		
		
	}
	//显示掉落物品//
	public void ShowDropRewards(List<string> rew)
	{
		RewardsLabel.gameObject.SetActive(true);
		RewardsLabel.text = TextsData.getData(333).chinese;
		if(rew.Count > 0)
		{
			
			for(int i = 0;i< rew.Count;i++)
			{
				GameObject obj = Rewards[i];
				obj.SetActive(true);
//				UIButtonMessage ubm = obj.GetComponent<UIButtonMessage>();
//				ubm.target = gameObject;
//				ubm.functionName = "OnClickRewards";
//				ubm.param = i;
				RewardsItem reItem = obj.GetComponent<RewardsItem>();
				reItem.index = i;
				reItem.rewData = rew[i];
				
				SimpleCardInfo2 cardInfo = obj.GetComponent<SimpleCardInfo2>();
							
				string[] str = rew[i].Split('-');
				int type = StringUtil.getInt(str[0]);
				string[] str2 = str[1].Split(',');
				int id = StringUtil.getInt(str2[0]);
				//if(str2.Length > 1)
				//{
				//	int num = StringUtil.getInt(str2[1]);
				//}
				//UIAtlas atlas = null;
				GameHelper.E_CardType cardType = GameHelper.E_CardType.E_Null;
				switch(type)
				{
				case 1:				//items//
					//ItemsData item = ItemsData.getData(id);
					cardType = GameHelper.E_CardType.E_Item;
					//atlas = LoadAtlasOrFont.LoadAtlasByName(itemAtlasName);
					break;
				case 2:				//equip//
					//EquipData ed = EquipData.getData(id);
					cardType = GameHelper.E_CardType.E_Equip;
					//atlas = LoadAtlasOrFont.LoadAtlasByName(equpiAtlasName);
					break;
				case 3:				//card//
				case 6:			//固定card//
					//CardData cd = CardData .getData(id);
					cardType = GameHelper.E_CardType.E_Hero;
					//atlas = LoadAtlasOrFont.LoadAtlasByName(cardAtlasName);
					break;
				case 4:				//skill//
					//SkillData sd = SkillData.getData(id);
					cardType = GameHelper.E_CardType.E_Skill;
					//atlas = LoadAtlasOrFont.LoadAtlasByName(skillAtlasName);
					break;
				case 5:				//passiveSkill//
					//PassiveSkillData psd = PassiveSkillData.getData(id);
					cardType = GameHelper.E_CardType.E_PassiveSkill;
					//atlas = LoadAtlasOrFont.LoadAtlasByName(passSkillAtlasName);
					break;
				}
				cardInfo.setSimpleCardInfo(id,cardType);
			}
		}
		else
		{
			NoRewardsLabe.SetActive(true);
			string s = TextsData.getData(339).chinese ;
			NoRewardsLabe.GetComponent<UILabel>().text = s;
		}
	}
	
	public bool isEnded()
	{
		if(step > 2)
		{
			return true;
		}
		else 
		{
			return false;
		}
	}
	

	
}

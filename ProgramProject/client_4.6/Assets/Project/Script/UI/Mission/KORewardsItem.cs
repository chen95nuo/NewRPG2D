using UnityEngine;
using System.Collections;

public class KORewardsItem : MonoBehaviour {
	
	public UILabel TaskName;
	public SimpleCardInfo2 RewardsCardIcon;
	public GameObject RewardsGet;
	public UILabel numLabel;
	public UILabel pointNUmLabel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setData(int missionId,int bonus)
	{
		MissionData md=MissionData.getData(missionId);
		if(md==null)
		{
			return;
		}
		//领取过bonus为true,没有bonus为false//
		if(bonus==1)
		{
			RewardsGet.SetActive(true);
		}
		else
		{
			RewardsGet.SetActive(false);
		}
		//==任务名称==//
		int textId=0;
		switch(md.addtasktype)
		{
		case 1:
			textId=427;
			break;
		case 2:
			textId=428;
			break;
		case 3:
			textId=429;
			break;
		case 4:
			textId=430;
			break;
		case 5:
			textId=431;
			break;
		case 6:
			textId=432;
			break;
		case 7:
			textId=433;
			break;
		case 8:
			textId=434;
			break;
		case 9:
			textId=435;
			break;
		case 10:
			textId=436;
			break;
		case 11:
			textId=437;
			break;
		case 12:
			textId=438;
			break;
		case 13:
			textId=439;
			break;
		case 14:
			textId=440;
			break;
		case 15:
			textId=441;
			break;
		}
		string taskname=TextsData.getData(textId).chinese;
		switch(md.showtype)
		{
		case 0://==星级数字,直接替换==//
			taskname=taskname.Replace("n",md.addtaskid+"");
			break;
		case 1://==种族数字,取种族名字替换==//
			string raceName="";
			switch(md.addtaskid)
			{
			case 1:
				raceName=TextsData.getData(5).chinese;
				break;
			case 2:
				raceName=TextsData.getData(6).chinese;
				break;
			case 3:
				raceName=TextsData.getData(8).chinese;
				break;
			case 4:
				raceName=TextsData.getData(7).chinese;
				break;
			}
			taskname=taskname.Replace("n",raceName);
			break;
		case 2://==cardId,取card名字替换==//
			CardData cd=CardData.getData(md.addtaskid);
			taskname=taskname.Replace("n",cd.name);
			break;
		case 3://==skillType,取skillType名字替换==//
			string skillType="";
			switch(md.addtaskid)
			{
			case 1:
				skillType=TextsData.getData(499).chinese;
				break;
			case 2:
				skillType=TextsData.getData(498).chinese;
				break;
			case 3:
				skillType=TextsData.getData(497).chinese;
				break;
			}
			taskname=taskname.Replace("n",skillType);
			break;
		case 4://==skillElement,取skillElement名字替换==//
			string skillElement="";
			switch(md.addtaskid)
			{
			case 1:
				skillElement=TextsData.getData(1).chinese;
				break;
			case 2:
				skillElement=TextsData.getData(2).chinese;
				break;
			case 3:
				skillElement=TextsData.getData(3).chinese;
				break;
			case 4:
				skillElement=TextsData.getData(4).chinese;
				break;
			}
			taskname=taskname.Replace("n",skillElement);
			break;
		case 5://==skillId,取skill名字替换==//
			SkillData sd=SkillData.getData(md.addtaskid);
			taskname=taskname.Replace("n",sd.name);
			break;
		}
		TaskName.text="2."+taskname;
		//==任务奖励==//
		SimpleCardInfo2 sc2=RewardsCardIcon.GetComponent<SimpleCardInfo2>();
		sc2.clear();
		if(!string.IsNullOrEmpty(md.specialaward1))
		{
			string[] ss=md.specialaward1.Split(',');
			int id=StringUtil.getInt(ss[0]);
			int num=StringUtil.getInt(ss[1]);
			switch(md.specialtype1)
			{
			case 1://==item==//
				sc2.setSimpleCardInfo(id,GameHelper.E_CardType.E_Item);
				break;
			case 2://==equip==//
				sc2.setSimpleCardInfo(id,GameHelper.E_CardType.E_Equip);
				break;
			case 3://==card==//
				sc2.setSimpleCardInfo(id,GameHelper.E_CardType.E_Hero);
				break;
			case 4://==skill==//
				sc2.setSimpleCardInfo(id,GameHelper.E_CardType.E_Skill);
				break;
			case 5://==pskill==//
				sc2.setSimpleCardInfo(id,GameHelper.E_CardType.E_PassiveSkill);
				break;
			}
			//==奖励名称==//
			numLabel.text="x "+num;
			//==点击事件==//
			RewardsCardIcon.GetComponent<RewardsItem>().rewData=md.specialtype1+"-"+id+",1";
			// ko point //
			pointNUmLabel.text = md.kopoint.ToString();
		}
	}
}

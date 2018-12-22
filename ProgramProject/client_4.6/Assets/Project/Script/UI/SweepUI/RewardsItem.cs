using UnityEngine;
using System.Collections;

public class RewardsItem : MonoBehaviour {
	
	public int index;
	public string rewData;
	
	RewardsDatasControl rewards;
	//string itemAtlasName = "ItemCircularIcon";
	//string equpiAtlasName = "EquipCircularIcon";
	//string cardAtlasName = "HeadIcon";
	//string skillAtlasName = "SkillCircularIcom";
	//string passSkillAtlasName = "PassSkillCircularIcon";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowDetails()
	{
		string[] str = rewData.Split('-');
		int type = StringUtil.getInt(str[0]);
		string[] str2 = str[1].Split(',');
		int formID = StringUtil.getInt(str2[0]);
		int level= StringUtil.getInt(str2[1]);
		int star = 1;
		//string iconName = "";
		string name = "";
		string des = "";
		int sell = 0;
			
		//UIAtlas atlas = null;
		GameHelper.E_CardType cardType = GameHelper.E_CardType.E_Null;
		
		switch(type)
		{
		case 1:				//items//
			ItemsData item = ItemsData.getData(formID);
			if(item == null)
				return;
			cardType = GameHelper.E_CardType.E_Item;
			star = item.star;
			//iconName = item.icon;
			//atlas = LoadAtlasOrFont.LoadAtlasByName(itemAtlasName);
			name = item.name;
			des = item.discription;
			sell = item.sell;
			break;
		case 2:				//equip//
			EquipData ed = EquipData.getData(formID);
			if(ed == null)
				return;
			cardType = GameHelper.E_CardType.E_Equip;
			star = ed.star;
			//iconName = ed.icon;
			//atlas = LoadAtlasOrFont.LoadAtlasByName(equpiAtlasName);
			name = ed.name;
			des = ed.description;
			sell = ed.sell;
			break;
		case 3:				//card//
			CardData cd = CardData .getData(formID);
			if(cd == null)
				return;
			cardType = GameHelper.E_CardType.E_Hero;
			star = cd.star;
			//iconName = cd.icon;
			//atlas = LoadAtlasOrFont.LoadAtlasByName(cardAtlasName);
			name = cd.name;
			des = cd.description;
			sell = cd.sell;
			break;
		case 4:				//skill//
			SkillData sd = SkillData.getData(formID);
			if(sd == null)
				return;
			cardType = GameHelper.E_CardType.E_Skill;
			star = sd.star;
			//iconName = sd.icon;
			//atlas = LoadAtlasOrFont.LoadAtlasByName(skillAtlasName);
			name = sd.name;
			des = sd.description;
			sell = sd.sell;
			break;
		case 5:				//passiveSkill//
			PassiveSkillData psd = PassiveSkillData.getData(formID);
			if(psd == null)
				return;
			cardType = GameHelper.E_CardType.E_PassiveSkill;
			star = psd.star;
			//iconName = psd.icon;
			//atlas = LoadAtlasOrFont.LoadAtlasByName(passSkillAtlasName);
			name = psd.name;
			des = psd.describe;
			sell = psd.sell;
			break;
		}
		string frameName = "head_star_" + star;
		RewardsDatasControl.mInstance.SetData(formID,cardType,name, frameName, des, level, sell, type);
		Vector3 reV3 = transform.position;
		RewardsDatasControl.mInstance.transform.position = new Vector3(reV3.x + 0.6f, reV3.y + 0.3f, 0f);
	}
	
	public void OnPress(bool isPressed)
	{
		if(isPressed)
		{
			
			ShowDetails();
		}else
		{
			RewardsDatasControl.mInstance.hide ();
		}
	}
	
	
}

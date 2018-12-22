using UnityEngine;
using System.Collections;

public class RechargeItemInfo : MonoBehaviour {
	
	public GameObject helpUnitPanel;
	public int itemType;
	public int itemId;
	public ActivityInfoExchangeElement tempActInfoEElement;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void showHelperUnit(int unitId)
	{
		//1材料，2装备，3英雄卡，4主动技能，5被动技能，6金币，7经验值, 8钻石，9符文，10体力//
		//Show the 1-5 only.
		if(itemType<6)
		{
			helpUnitPanel.SetActive(true);
			
			GetItemInfo(helpUnitPanel);
		}
		
	}
	
	public void hiddleHelperUnit(int unitId)
	{
		helpUnitPanel.SetActive(false);
	}
	
	//Type:
	public void GetItemInfo(GameObject rItem)
	{
		GameObject rewardObj = rItem.transform.FindChild("Rward0").gameObject;
		//UISprite rewardIconBG = rewardObj.transform.FindChild("IconBG").GetComponent<UISprite>();
		UILabel rewardLabel = rewardObj.transform.FindChild("Text").GetComponent<UILabel>();
		
		rewardLabel.text = string.Empty;
		SimpleCardInfo2 cardInfo = rewardObj.transform.FindChild("CardInfo").GetComponent<SimpleCardInfo2>();
		cardInfo.clear();
		cardInfo.gameObject.SetActive(false);
		
		UILabel name = rItem.transform.FindChild("unit-name").GetComponent<UILabel>();
		UILabel des = rItem.transform.FindChild("unit-des").GetComponent<UILabel>();
		UILabel goldNum = rItem.transform.FindChild("goldNumLabel").GetComponent<UILabel>();
		switch(itemType)
		{
			case 1:
			{
				ItemsData itemData = ItemsData.getData(itemId);
				if(itemData == null)
				{
					helpUnitPanel.SetActive(false);
				}
				else
				{
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(itemId,GameHelper.E_CardType.E_Item);
				
					name.text = itemData.name;
					des.text = itemData.discription;
					goldNum.text = itemData.sell.ToString();
				}
				
			}break;
			case 2:
			{
				
				EquipData ed = EquipData.getData(itemId);
				if(ed == null)
				{
					helpUnitPanel.SetActive(false);
				}
				else
				{
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(itemId,GameHelper.E_CardType.E_Equip);	
					
					int eType = ed.type;
				 	int eStar = ed.star;
					//找到装备对应的等级＝1的值//
					int eValue = EquippropertyData.getValue(eType,1,eStar);
				
					string eDesc = ed.description;
					if(eDesc.EndsWith("+"))
					{
						des.text = eDesc + eValue.ToString();
					}
					else
					{
						des.text = ed.description;
					}
					name.text = ed.name;
					goldNum.text = ed.sell.ToString();
				}
				
				
			}break;
			case 3:
			{
				
				CardData cd = CardData.getData(itemId);
				if(cd == null)
				{
					helpUnitPanel.SetActive(false);
				}
				else
				{
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(itemId,GameHelper.E_CardType.E_Hero);
				
					name.text = cd.name;
					des.text = cd.description;
					goldNum.text = cd.sell.ToString();
				}
				
				
			}break;
			case 4:
			{
				
				SkillData sd = SkillData.getData(itemId);
				if(sd == null)
				{
					helpUnitPanel.SetActive(false);
				}
				else
				{
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(itemId,GameHelper.E_CardType.E_Skill);
				
				int sType = sd.type;
				int sStar = sd.star;
				int sValue = SkillPropertyData.getProperty(sType,1,sStar);
				string sDesc = sd.description;
				if(sDesc.EndsWith("+"))
				{
					des.text = sDesc + sValue.ToString();
				}
				else
				{
					des.text = sDesc;
				}
					name.text = sd.name;
					
					goldNum.text = sd.sell.ToString();
				}
				
			}break;
			case 5:
			{
				
				PassiveSkillData psd = PassiveSkillData.getData(itemId);
				if(psd == null)
				{
					helpUnitPanel.SetActive(false);
				}
				else
				{
				cardInfo.gameObject.SetActive(true);
				cardInfo.setSimpleCardInfo(itemId,GameHelper.E_CardType.E_PassiveSkill);
				
					name.text = psd.name;
					des.text = psd.describe;
					goldNum.text = psd.sell.ToString();
				}
				
				
			}break;
			default:
				helpUnitPanel.SetActive(false);
			break;
		}
	}
}

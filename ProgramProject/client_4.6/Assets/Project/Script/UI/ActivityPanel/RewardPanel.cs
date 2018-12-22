using UnityEngine;
using System.Collections;

public class RewardPanel : MonoBehaviour {
	
	public UIAtlas otherAtlas;
    string expSpriteName = "reward_exp";
    string crystalSpriteName = "reward_crystal";
    string goldSpriteName = "reward_gold";
	
	GameObject popParent;
	
	SevenDaysData dayData;
	LevelGiftData levelData;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitRewardPanel(int comeType,int dayNum)
	{
		int goodsTypeCount = 0;
		if(comeType == 1)
		{
			dayData = SevenDaysData.getData(dayNum);
			goodsTypeCount = dayData.goodsType.Count;
		}
		else if(comeType == 2)
		{
			levelData = LevelGiftData.getData(dayNum);
			goodsTypeCount = levelData.goodsType.Count;
		}
		
		transform.FindChild("scroll-bar").GetComponent<UIScrollBar>().value = 0;

        GameObject panel = transform.FindChild("panel").gameObject;
        panel.transform.localPosition = new Vector3(0, -72f, 0);
        panel.GetComponent<UIPanel>().clipRange = new Vector4(0, 72, 300f, 150f);
		popParent = transform.FindChild("panel/pop-info/pop-parent").gameObject;
		
		for(int i=0;i<goodsTypeCount;i++)
		{
			/*
			GameObject rewardGo = Instantiate(GameObjectUtil.LoadResourcesPrefabs("ActivityPanel/ItemIcon", 3)) as GameObject;
			rewardGo.transform.parent = popParent.transform;
            rewardGo.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            rewardGo.name = "ItemIcon" + (1000+i);
			
			int type = 0;
			int num = 0;
			int id = 0;
			if(comeType == 1)
			{
				type = dayData.goodsType[i];
				num = dayData.goodsNum[i];
				id = dayData.goodsIds[i];
			}
			else if(comeType == 2)
			{
				type = levelData.goodsType[i];
				num = levelData.goodsNum[i];
				id = levelData.goodsIds[i];
			}
			
        	rewardGo.GetComponent<ItemIcon>().Init(type, id, num, ItemIcon.ItemUiType.NameDownNum);
			*/
			 GameObject rewardGo = Instantiate(GameObjectUtil.LoadResourcesPrefabs("ActivityPanel/RewardItem", 3)) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(rewardGo, popParent);
			UISprite rewardIconBG = rewardGo.transform.FindChild("IconBG").GetComponent<UISprite>();
			rewardIconBG.gameObject.SetActive(false);
			UILabel rewardLabel = rewardGo.transform.FindChild("Num").GetComponent<UILabel>();
			UILabel rewardName = rewardGo.transform.FindChild("Name").GetComponent<UILabel>();
			
			rewardLabel.text = string.Empty;
			SimpleCardInfo2 sc = rewardGo.transform.FindChild("CardInfo").GetComponent<SimpleCardInfo2>();
			sc.clear();
			sc.gameObject.SetActive(false);
			
			int type = 0;
			int num = 0;
			int id = 0;
			if(comeType == 1)
			{
				type = dayData.goodsType[i];
				num = dayData.goodsNum[i];
				id = dayData.goodsIds[i];
			}
			else if(comeType == 2)
			{
				type = levelData.goodsType[i];
				num = levelData.goodsNum[i];
				id = levelData.goodsIds[i];
			}
			switch(type)
			{
			case 1:
			{
				ItemsData itemData = ItemsData.getData(id);
				if(itemData == null)
				{
					rewardGo.SetActive(false);
					continue;
				}
				sc.gameObject.SetActive(true);
				sc.setSimpleCardInfo(id, GameHelper.E_CardType.E_Item);
				rewardLabel.text = " x " + num.ToString();
				rewardName.text = itemData.name;
			}
				break;
			case 2:
			{
				EquipData ed = EquipData.getData(id);
				if(ed == null)
				{
					rewardGo.SetActive(false);
					continue;
				}
				sc.gameObject.SetActive(true);
				sc.setSimpleCardInfo(id, GameHelper.E_CardType.E_Equip);
				rewardLabel.text = " x " + num.ToString();
				rewardName.text = ed.name;
			}
				break;
			case 3:
			{
				CardData cd = CardData.getData(id);
				if(cd == null)
				{
					rewardGo.SetActive(false);
					continue;
				}
				sc.gameObject.SetActive(true);
				sc.setSimpleCardInfo(id,GameHelper.E_CardType.E_Hero);

				rewardLabel.text = " x " + num.ToString();
				rewardName.text = cd.name;
			}
				break;
			case 4:
			{
				SkillData sd = SkillData.getData(id);
				if(sd == null)
				{
					rewardGo.SetActive(false);
					continue;
				}
				sc.gameObject.SetActive(true);
				sc.setSimpleCardInfo(id,GameHelper.E_CardType.E_Skill);

				rewardLabel.text = " x " + num.ToString();
				rewardName.text = sd.name;
			}
				break;
			case 5:
			{
				PassiveSkillData psd = PassiveSkillData.getData(id);
				if(psd == null)
				{
					rewardGo.SetActive(false);
					continue;
				}
				sc.gameObject.SetActive(true);
				sc.setSimpleCardInfo(id,GameHelper.E_CardType.E_PassiveSkill);
				
				rewardLabel.text = " x " + num.ToString();
				rewardName.text = psd.name;
			}
				break;
			case 6:
				{
					rewardIconBG.gameObject.SetActive(true);
					rewardIconBG.atlas = otherAtlas;
					rewardIconBG.spriteName = goldSpriteName;
					rewardLabel.text = "x " + num.ToString();
					rewardName.gameObject.SetActive(false);
					Vector3 namePos = rewardName.transform.localPosition;
					Vector3 numPos = rewardLabel.transform.localPosition;
					rewardLabel.transform.localPosition = new Vector3(namePos.x,numPos.y,numPos.z);
				}
				break;
			case 7:
			{
				rewardIconBG.gameObject.SetActive(true);
                rewardIconBG.atlas = otherAtlas;
                rewardIconBG.spriteName = expSpriteName;
                rewardLabel.text = "x " + num.ToString();
				rewardName.gameObject.SetActive(false);
				Vector3 namePos = rewardName.transform.localPosition;
				Vector3 numPos = rewardLabel.transform.localPosition;
				rewardLabel.transform.localPosition = new Vector3(namePos.x,numPos.y,numPos.z);
			}
				break;
			case 8:
			{
				Debug.Log("coming 8");
				rewardIconBG.gameObject.SetActive(true);
                rewardIconBG.atlas = otherAtlas;
                rewardIconBG.spriteName = crystalSpriteName;
                rewardLabel.text = "x " + num.ToString();
				rewardName.gameObject.SetActive(false);
				Vector3 namePos = rewardName.transform.localPosition;
				Vector3 numPos = rewardLabel.transform.localPosition;
				rewardLabel.transform.localPosition = new Vector3(namePos.x,numPos.y,numPos.z);
			}
				break;
			case 9:
			{
				sc.gameObject.SetActive(true);
                sc.setSpecialIconInfo(GameHelper.E_CardType.E_Rune);

                rewardLabel.text = "x " + num.ToString();
				rewardName.gameObject.SetActive(false);
				Vector3 namePos = rewardName.transform.localPosition;
				Vector3 numPos = rewardLabel.transform.localPosition;
				rewardLabel.transform.localPosition = new Vector3(namePos.x,numPos.y,numPos.z);
			}
				break;
			case 10:
			{
				sc.gameObject.SetActive(true);
                sc.setSpecialIconInfo(GameHelper.E_CardType.E_Power);

                rewardLabel.text = "x " + num.ToString();
				rewardName.gameObject.SetActive(false);
				Vector3 namePos = rewardName.transform.localPosition;
				Vector3 numPos = rewardLabel.transform.localPosition;
				rewardLabel.transform.localPosition = new Vector3(namePos.x,numPos.y,numPos.z);
			}
				break;
			case 11:
			{
				sc.gameObject.SetActive(true);
                sc.setSpecialIconInfo(GameHelper.E_CardType.E_Friend);

                rewardLabel.text = "x " + num.ToString();
				rewardName.gameObject.SetActive(false);
				Vector3 namePos = rewardName.transform.localPosition;
				Vector3 numPos = rewardLabel.transform.localPosition;
				rewardLabel.transform.localPosition = new Vector3(namePos.x,numPos.y,numPos.z);
			}
				break;
				
			}
		}
		
		popParent.GetComponent<UIGrid>().repositionNow = true;
		
		Resources.UnloadUnusedAssets();
	}
	
	void CloseRewardPanel(int param)
	{
		GameObjectUtil.destroyGameObjectAllChildrens(popParent);
		gameObject.SetActive(false);
	}
}

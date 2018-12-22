using UnityEngine;
using System.Collections;

public class ItemIcon : MonoBehaviour {

    public GameObject objType1;     //parent GameObject of item,equip,card,skill,passive
    public GameObject objType2;     //UISprite of gold,crystal,exp,rune,strength,friendship
    public UISprite texFrame;       //icon frame
    public UISprite texCard;        //card icon
    public UISprite texOther;       //other icon
    public UILabel labelNum;        //item num
    public GameObject objPiece;     //item fragment

    public UILabel labelName;       //item name
    public UILabel labelDesc;       //item description

    //int itemType;
    //int itemId;
    //int itemNum;
    //ItemUiType uiType;

    public enum ItemUiType { Common = 0, NameDown, DescShow, NameDownNum, NullTextShow };

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(int itemType, int itemId, int itemNum = 1, ItemUiType uiType = ItemUiType.Common)
    {
        //this.itemType = itemType;
        //this.itemId = itemId;
        //this.itemNum = itemNum;
        //this.uiType = uiType;

        UIAtlas atlas = null;
        int starNum = 0;
        string iconName = "";
        string name = "";
        string desc = "";
        bool bPiece = false;
        bool bCard = false;
        switch (itemType)
        {
            case 1: //item
                {
                    ItemsData temp = ItemsData.getData(itemId);
                    starNum = temp.star;
                    if (temp.fragment == 0)
                    {
                        iconName = temp.icon;
                        atlas = LoadAtlasOrFont.LoadAtlasByName("ItemCircularIcon");
                    }
                    else
                    {
                        bPiece = true;
                        switch (temp.goodztype)
                        {
                            case 1: iconName = SkillData.getData(temp.goodsid).icon; atlas = LoadAtlasOrFont.LoadAtlasByName("SkillCircularIcom"); break;
                            case 2: iconName = ItemsData.getData(temp.goodsid).icon; atlas = LoadAtlasOrFont.LoadAtlasByName("ItemCircularIcon"); break;
                            case 3: iconName = EquipData.getData(temp.goodsid).icon; atlas = LoadAtlasOrFont.LoadAtlasByName("EquipCircularIcon"); break;
                            case 4:
                                CardData data = CardData.getData(temp.goodsid);
                                iconName = data.icon;
                                atlas = LoadAtlasOrFont.LoadAtlasByName(data.atlas);
                                bCard = true;
                                break;
                            case 5: iconName = PassiveSkillData.getData(temp.goodsid).icon; atlas = LoadAtlasOrFont.LoadAtlasByName("PassSkillCircularIcon"); break;
                        }
                    }
                    name = temp.name;
                    desc = temp.discription;
                }
                break;
            case 2: //equip
                {
                    atlas = LoadAtlasOrFont.LoadAtlasByName("EquipCircularIcon");
                    EquipData temp = EquipData.getData(itemId);
                    starNum = temp.star;
                    iconName = temp.icon;
                    name = temp.name;
                    desc = temp.description + EquippropertyData.getData(temp.type, 1).starNumbers[starNum - 1];
                }
                break;
            case 3: //card
                {
                    CardData temp = CardData.getData(itemId);
                    starNum = temp.star;
                    iconName = temp.icon;
                    atlas = LoadAtlasOrFont.LoadAtlasByName(temp.atlas);
                    name = temp.name;
                    desc = temp.description;
                    bCard = true;
                }
                break;
            case 4: //skill
                {
                    atlas = LoadAtlasOrFont.LoadAtlasByName("SkillCircularIcom");
                    SkillData temp = SkillData.getData(itemId);
                    starNum = temp.star;
                    iconName = temp.icon;
                    name = temp.name;
                    desc = temp.description + (temp.exptype == 1 ? SkillPropertyData.getProperty(temp.type, 1, starNum).ToString() : "");
                }
                break;
            case 5: //passive skill
                {
                    atlas = LoadAtlasOrFont.LoadAtlasByName("PassSkillCircularIcon");
                    PassiveSkillData temp = PassiveSkillData.getData(itemId);
                    starNum = temp.star;
                    iconName = temp.icon;
                    name = temp.name;
                    desc = temp.describe;
                }
                break;
            case 6:
                atlas = LoadAtlasOrFont.LoadAtlasByName("AchievementAtlas");
                iconName = "jb";
                break;
            case 7:
                atlas = LoadAtlasOrFont.LoadAtlasByName("AchievementAtlas");
                iconName = "exp";
                break;
            case 8:
                atlas = LoadAtlasOrFont.LoadAtlasByName("AchievementAtlas");
                iconName = "zs";
                break;
            case 9:
                atlas = LoadAtlasOrFont.LoadAtlasByName("InterfaceAtlas03");
                iconName = "rune";
                break;
            case 10:
                atlas = LoadAtlasOrFont.LoadAtlasByName("InterfaceAtlas03");
                iconName = "energy";
                break;
            case 11:
                atlas = LoadAtlasOrFont.LoadAtlasByName("InterfaceAtlas03");
                iconName = "friends";
                break;
        }
        if (itemType <= 5)
        {
            objType1.SetActive(true);
            objType2.SetActive(false);

            string sName = texFrame.spriteName;
            texFrame.spriteName = sName.Substring(0, sName.Length - 1) + starNum;
            if (bCard)
            {
                texCard.transform.gameObject.SetActive(true);
                texOther.transform.gameObject.SetActive(false);
                texCard.atlas = atlas;
                texCard.spriteName = iconName;
            }
            else
            {
                texCard.transform.gameObject.SetActive(false);
                texOther.transform.gameObject.SetActive(true);
                texOther.atlas = atlas;
                texOther.spriteName = iconName;
            }

            if (bPiece)
                objPiece.SetActive(true);
            else
                objPiece.SetActive(false);
        }
        else
        {
            objType1.SetActive(false);
            objType2.SetActive(true);
            UISprite temp = objType2.GetComponent<UISprite>();
            temp.atlas = atlas;
            temp.spriteName = iconName;
        }

        if (itemNum > 1)
            labelNum.text = "x" + itemNum;

        switch (uiType)
        {
            case ItemUiType.Common:
                break;
            case ItemUiType.NameDown:
                if (labelName != null)
                    labelName.text = name;
                break;
            case ItemUiType.DescShow:
                if (labelDesc != null)
                    labelDesc.text = desc;
                break;
            case ItemUiType.NameDownNum:
                if (labelName != null)
                    labelName.text = name;
                if (itemNum == 1)
                    labelNum.text = "x" + itemNum;
                labelNum.transform.localPosition = new Vector3(85, labelNum.transform.localPosition.y, 0);
                break;
		case ItemUiType.NullTextShow:
			if(labelNum != null)
				labelNum.alpha = 0;
			if(labelName != null)
			{
				labelName.text = name;
				labelName.alpha = 0;
			}
			break;
        }
    }
}

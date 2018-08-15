using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;
using UnityEngine.EventSystems;
using System;

public class UIBagPopUp : MonoBehaviour, IPointerDownHandler
{

    public GameObject topTip;
    public GameObject affixGroup;
    public Text text_equip;
    public Text[] affix;
    private Button[] btn_affix;

    public Text itemName;
    public Text itemDescribe;
    public Button sell;
    public Text text_sell;
    public Button use;
    public Text text_use;

    public UIBagGrid bagGridData;
    private UIBagGrid GridData;

    public Button btn_back;

    private EquipData equipData;

    public RectTransform r1;
    public RectTransform r2;

    private void Awake()
    {
        Image[] images = GetComponentsInChildren<Image>();
        GetSpriteAtlas.insatnce.SetImage(images);

        btn_affix = new Button[4];

        for (int i = 0; i < btn_affix.Length; i++)
        {
            btn_affix[i] = affix[i].GetComponentInParent<Button>();
        }
        btn_back.onClick.AddListener(TTUIPage.ClosePage<UIBagItemMessage>);

        UIEventManager.instance.AddListener<GameObject>(UIEventDefineEnum.UpdateLittleTipEvent, EquipAffixBtn);
        UIEventManager.instance.AddListener<EquipData>(UIEventDefineEnum.UpdateBagItemMessageEvent, ReplaceEquip);
        UIEventManager.instance.AddListener<GameObject>(UIEventDefineEnum.UpdateMessageTipEvent, ChickOBJ);

        use.onClick.AddListener(ChickUse);
        sell.onClick.AddListener(ChickSell);
    }

    private void Update()
    {
        if (r1.sizeDelta != r2.sizeDelta)
        {
            r1.sizeDelta = r2.sizeDelta;
        }

    }

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<GameObject>(UIEventDefineEnum.UpdateLittleTipEvent, EquipAffixBtn);
        UIEventManager.instance.RemoveListener<GameObject>(UIEventDefineEnum.UpdateMessageTipEvent, ChickOBJ);
    }

    public void ReplaceEquip(EquipData data)
    {
        sell.gameObject.SetActive(true);
        text_sell.text = "卸下";
        use.gameObject.SetActive(false);
        updateMessage(data);
    }

    public void updateMessage(ItemData data)
    {
        bagGridData.UpdateItem(data);
        //显示名字，简介，是否使用
        affixGroup.SetActive(false);
        topTip.SetActive(false);
        text_sell.text = "出售";
        text_use.text = "使用";


        switch ((PropType)data.PropType)
        {
            case PropType.Nothing:
                break;
            case PropType.AllOff:
                sell.gameObject.SetActive(false);
                use.gameObject.SetActive(false);
                break;
            case PropType.OnlyUse:
                sell.gameObject.SetActive(false);
                use.gameObject.SetActive(true);
                break;
            case PropType.OnlySell:
                sell.gameObject.SetActive(true);
                use.gameObject.SetActive(false);
                break;
            case PropType.AllOn:
                sell.gameObject.SetActive(true);
                use.gameObject.SetActive(true);
                break;
            default:
                break;
        }

        itemName.text = data.Name.ToString();
        itemDescribe.text = data.Describe;
        Debug.Log("同步");
    }
    public void updateMessage(UIBagGrid information)
    {
        if (information.itemType == ItemType.Prop)
        {
            updateMessage(information.propData);
            GridData = information;
            return;
        }

        use.gameObject.SetActive(false);
        sell.gameObject.SetActive(false);

        updateMessage(information.equipData);
        if (information.gridType == GridType.Use)
        {
            use.gameObject.SetActive(true);
            GridData = information;
        }
        else
        {
            use.gameObject.SetActive(false);
        }
        Debug.Log("同步");
    }

    public void updateMessage(EquipData data)
    {
        bagGridData.UpdateItem(data);
        equipData = data;
        //显示类型，名字，磁条，简介
        affixGroup.SetActive(true);
        topTip.SetActive(true);

        switch (data.EquipType)
        {
            case EquipType.Nothing:
                text_equip.text = "空";
                break;
            case EquipType.Weapon:
                text_equip.text = "武器";
                break;
            case EquipType.Armor:
                text_equip.text = "防具";
                break;
            case EquipType.Necklace:
                text_equip.text = "首饰";
                break;
            default:
                break;
        }
        itemName.text = data.Name;
        itemDescribe.text = data.Describe;
        if (data.Affix_1 == null)
        {
            btn_affix[0].interactable = false;
            affix[0].text = "";
        }
        else
        {
            btn_affix[0].interactable = true;
            affix[0].text = FormatAffix(data.Affix_1);
        }

        if (data.Affix_2 == null)
        {
            btn_affix[1].interactable = false;
            affix[1].text = "";
        }
        else
        {
            btn_affix[1].interactable = true;
            affix[1].text = FormatAffix(data.Affix_2);
        }
        if (data.Affix_3 == null)
        {
            btn_affix[2].interactable = false;
            affix[2].text = "";
        }
        else
        {
            btn_affix[2].interactable = true;
            affix[2].text = FormatAffix(data.Affix_3);
        }
        if (data.Affix_4 == null)
        {
            btn_affix[3].interactable = false;
            affix[3].text = "";
        }
        else
        {
            btn_affix[3].interactable = true;
            affix[3].text = FormatAffix(data.Affix_4);
        }
    }

    public void UseEquip()
    {
        UIEventManager.instance.SendEvent<UIBagGrid>(UIEventDefineEnum.UpdateEquipsEvent, GridData);
    }

    public void EquipAffixBtn(GameObject obj)
    {
        int currentBtn = 0;
        for (int i = 0; i < btn_affix.Length; i++)
        {
            if (btn_affix[i].gameObject == obj)
            {
                currentBtn = i + 1;
            }
        }
        switch (currentBtn)
        {
            case 1:
                string affix_1 = FormatAllAffix(GameEquipData.Instance.QueryEquip(equipData.Id).Affix_1);
                if (affix_1 != "")
                {
                    TTUIPage.ShowPage<UILittleTipPage>();

                    UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, affix_1);
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);
                }
                break;

            case 2:
                string affix_2 = FormatAllAffix(GameEquipData.Instance.QueryEquip(equipData.Id).Affix_2);
                if (affix_2 != "")
                {
                    TTUIPage.ShowPage<UILittleTipPage>();

                    UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, affix_2);
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);
                }

                break;
            case 3:
                string affix_3 = FormatAllAffix(GameEquipData.Instance.QueryEquip(equipData.Id).Affix_3);
                if (affix_3 != "")
                {
                    TTUIPage.ShowPage<UILittleTipPage>();

                    UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, affix_3);
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);
                }

                break;
            case 4:
                string affix_4 = FormatAllAffix(GameEquipData.Instance.QueryEquip(equipData.Id).Affix_4);
                if (affix_4 != "")
                {
                    TTUIPage.ShowPage<UILittleTipPage>();

                    UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, affix_4);
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);
                }

                break;
            default:
                break;
        }
    }

    private void ChickUse()
    {
        if (GridData.itemType == ItemType.Prop)
        {
            Debug.Log("使用 弹出数量选择");
            BagItemData.Instance.ReduceItems(GridData.propData.Id, 1);
            return;
        }
        if (GridData.gridType == GridType.Use)
        {
            UseEquip();
        }
    }

    private void ChickSell()
    {
        if (GridData.itemType == ItemType.Prop)
        {
            Debug.Log("出售 弹出数量选择");
            string st = "是否出售 " + GridData.propData.Name + "*1\n" + "将获得 : " + GridData.propData.SellPrice + "金币";
            TTUIPage.ShowPage<UIMessageTipPage>();
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateMessageTipGoEvent, this.gameObject);
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateMessageTipEvent, st);
            return;
        }
        if (GridData.gridType == GridType.Use)
        {
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateBagItemMessageEvent);
        }
    }

    private void ChickOBJ(GameObject obj)
    {
        Debug.Log("运行了");
        if (obj == this.gameObject)
        {
            GetPlayData.Instance.player[0].GoldCoin += GridData.propData.SellPrice;
            BagItemData.Instance.ReduceItems(GridData.propData.Id, 1);
            updateMessage(GridData.propData);
            TTUIPage.ShowPage<UIPopTipPage>();
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateMessagePopTipEvent, GridData.propData.SellPrice.ToString());
            Debug.Log("运行了");
            TTUIPage.ClosePage<UIBagItemMessage>();
        }
    }

    public static string FormatAllAffix(string affix)
    {
        if (affix == "")
        {
            return "";
        }
        string[] str;
        char[] ch = new char[] { '(', ',', ')' };
        str = affix.Split(ch);
        string newStr = FormatAffix(str[0]) + "+ (" + str[1] + "% ~ " + str[2] + "%)";

        return newStr;
    }

    public static string FormatAffix(string affix)
    {
        if (affix == "")
        {
            return null;
        }
        string[] str;
        str = affix.Split('+');
        EquipAffixName name = (EquipAffixName)Enum.Parse(typeof(EquipAffixName), str[0]);
        string index = "";
        switch (name)
        {
            case EquipAffixName.att:
                index = "攻击";
                break;
            case EquipAffixName.con:
                index = "生命";
                break;
            case EquipAffixName.def:
                index = "防御";
                break;
            case EquipAffixName.dex:
                index = "敏捷";
                break;
            case EquipAffixName.crt:
                index = "暴击率";
                break;
            case EquipAffixName.sbl:
                index = "吸血";
                break;
            case EquipAffixName.stun:
                index = "眩晕";
                break;
            case EquipAffixName.spd:
                index = "攻速";
                break;
            case EquipAffixName.mspd:
                index = "移速";
                break;
            case EquipAffixName.bld:
                index = "流血";
                break;
            case EquipAffixName.cd:
                index = "冷却";
                break;
            case EquipAffixName.weak:
                index = "虚弱";
                break;
            default:
                break;
        }
        if (str.Length > 1)
        {
            return index = index + " + " + str[1];
        }
        else
        {
            return index;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

}

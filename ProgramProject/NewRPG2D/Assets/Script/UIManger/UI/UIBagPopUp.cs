using System;
using System.Collections;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBagPopUp : MonoBehaviour
{

    public RectTransform equip;
    public RectTransform prop;
    public RectTransform egg;

    public Text eggName;

    public Text equipType;
    public Text equipName;
    public Text equipDescribe;
    public Text affix_1;
    private Button btn_affix_1;
    public Text affix_2;
    private Button btn_affix_2;
    public Text affix_3;
    private Button btn_affix_3;
    public Text affix_4;
    private Button btn_affix_4;

    public Text propName;
    public Text propDescribe;
    public Button sell;
    public Button use;
    public Button equipUse;

    public Sprite[] popup;

    private RectTransform rect;

    public Canvas canvas;

    private UIBagGrid bagGridData;

    private void Awake()
    {
        rect = transform.GetComponent<RectTransform>();

        btn_affix_1 = affix_1.GetComponent<Button>();
        btn_affix_1.onClick.AddListener(EquipAffixBtn);
        btn_affix_2 = affix_2.GetComponent<Button>();
        btn_affix_2.onClick.AddListener(EquipAffixBtn);
        btn_affix_3 = affix_3.GetComponent<Button>();
        btn_affix_3.onClick.AddListener(EquipAffixBtn);
        btn_affix_4 = affix_4.GetComponent<Button>();
        btn_affix_4.onClick.AddListener(EquipAffixBtn);
        equipUse.onClick.AddListener(UseEquip);

        egg.gameObject.SetActive(false);
        prop.gameObject.SetActive(false);
        equip.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != sell.gameObject
            && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != use.gameObject
            && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != affix_1.gameObject
            && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != affix_2.gameObject
            && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != affix_3.gameObject
            && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != affix_4.gameObject
            && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != equipUse.gameObject)
        {
            TTUIPage.ClosePage("UIBagItemMessage");
        }
    }
    public void updateMessage(EggData data)
    {
        //显示气泡，根据是否在图鉴中决定名称
        equip.gameObject.SetActive(false);
        prop.gameObject.SetActive(false);
        egg.gameObject.SetActive(true);

        if (data.IsKnown)
        {
            eggName.text = data.Name;
        }
        else
        {
            eggName.text = "不知名的蛋";
        }
    }
    public void updateMessage(ItemData data)
    {
        //显示名字，简介，是否使用
        equip.gameObject.SetActive(false);
        prop.gameObject.SetActive(true);
        egg.gameObject.SetActive(false);

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

        propName.text = data.Name.ToString();
        propDescribe.text = data.Describe;

    }
    public void updateMessage(UIBagGrid information)
    {
        //显示类型，名字，磁条，简介
        equip.gameObject.SetActive(true);
        prop.gameObject.SetActive(false);
        egg.gameObject.SetActive(false);

        switch (information.equipData.EquipType)
        {
            case EquipType.Nothing:
                equipType.text = "空";
                break;
            case EquipType.Weapon:
                equipType.text = "武器";
                break;
            case EquipType.Armor:
                equipType.text = "防具";
                break;
            case EquipType.Necklace:
                equipType.text = "首饰";
                break;
            default:
                break;
        }
        equipName.text = information.equipData.Name;
        equipDescribe.text = information.equipData.Describe;
        if (information.equipData.Affix_1 == null)
        {
            btn_affix_1.interactable = false;
            affix_1.text = "";
        }
        else
        {
            btn_affix_1.interactable = true;
            affix_1.text = FormatAffix(information.equipData.Affix_1);
        }

        if (information.equipData.Affix_2 == null)
        {
            btn_affix_2.interactable = false;
            affix_2.text = "";
        }
        else
        {
            btn_affix_2.interactable = true;
            affix_2.text = FormatAffix(information.equipData.Affix_2);
        }
        if (information.equipData.Affix_3 == null)
        {
            btn_affix_3.interactable = false;
            affix_3.text = "";
        }
        else
        {
            btn_affix_3.interactable = true;
            affix_3.text = FormatAffix(information.equipData.Affix_3);
        }
        if (information.equipData.Affix_4 == null)
        {
            btn_affix_4.interactable = false;
            affix_4.text = "";
        }
        else
        {
            btn_affix_4.interactable = true;
            affix_4.text = FormatAffix(information.equipData.Affix_4);
        }
        if (information.gridType == GridType.Use)
        {
            equipUse.gameObject.SetActive(true);
            bagGridData = information;
        }
        else
        {
            equipUse.gameObject.SetActive(false);
        }
    }

    public void UseEquip()
    {
        UIEventManager.instance.SendEvent<UIBagGrid>(UIEventDefineEnum.UpdateEquipsEvent, bagGridData);
    }

    public void EquipAffixBtn()
    {
        egg.gameObject.SetActive(true);
        egg.pivot = new Vector2(0.5f, 0);
        egg.GetComponent<Image>().sprite = popup[0];
        egg.transform.position = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().transform.position;
        if (EventSystem.current.currentSelectedGameObject == affix_1.gameObject)
            eggName.text = affix_1.text;
        if (EventSystem.current.currentSelectedGameObject == affix_2.gameObject)
            eggName.text = affix_2.text;
        if (EventSystem.current.currentSelectedGameObject == affix_3.gameObject)
            eggName.text = affix_3.text;
        if (EventSystem.current.currentSelectedGameObject == affix_4.gameObject)
            eggName.text = affix_4.text;
    }

    public void PopUpMoveTo(RectTransform rect, ItemType type)
    {
        if (rect.anchoredPosition.x > Screen.width * 0.75f)
        {
            switch (type)
            {
                case ItemType.Nothing:
                    break;
                case ItemType.Egg:
                    egg.pivot = Vector2.right;
                    egg.GetComponent<Image>().sprite = popup[1];
                    egg.anchoredPosition = new Vector2(rect.sizeDelta.x * 0.5f, rect.sizeDelta.y * 0.5f);
                    break;
                case ItemType.Prop:
                    prop.anchoredPosition = new Vector2(-prop.sizeDelta.x * 0.5f, prop.anchoredPosition.y);
                    break;
                case ItemType.Equip:
                    equip.anchoredPosition = new Vector2(-equip.sizeDelta.x * 0.5f, equip.anchoredPosition.y);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case ItemType.Nothing:
                    break;
                case ItemType.Egg:
                    egg.pivot = Vector2.up;
                    egg.GetComponent<Image>().sprite = popup[0];
                    egg.anchoredPosition = new Vector2(-rect.sizeDelta.x * 0.5f, rect.sizeDelta.y * 0.5f + egg.sizeDelta.y);
                    break;
                case ItemType.Prop:
                    prop.anchoredPosition = new Vector2(prop.sizeDelta.x * 0.5f, prop.anchoredPosition.y);
                    break;
                case ItemType.Equip:
                    equip.anchoredPosition = new Vector2(equip.sizeDelta.x * 0.5f, equip.anchoredPosition.y);
                    break;
                default:
                    break;
            }
        }

        this.rect.position = rect.position;

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
        switch (name)
        {
            case EquipAffixName.att:
                return "攻击 + " + str[1];
            case EquipAffixName.con:
                return "生命 + " + str[1];
            case EquipAffixName.def:
                return "防御 + " + str[1];
            case EquipAffixName.dex:
                return "敏捷 + " + str[1];
            case EquipAffixName.crt:
                return "暴击率 + " + str[1];
            case EquipAffixName.sbl:
                return "吸血 + " + str[1];
            case EquipAffixName.stun:
                return "眩晕 + " + str[1];
            case EquipAffixName.spd:
                return "攻速 + " + str[1];
            case EquipAffixName.mspd:
                return "移速 + " + str[1];
            case EquipAffixName.bld:
                return "流血 + " + str[1];
            case EquipAffixName.cd:
                return "冷却 + " + str[1];
            case EquipAffixName.weak:
                return "虚弱 + " + str[1];
            default:
                break;
        }
        return null;
    }

}

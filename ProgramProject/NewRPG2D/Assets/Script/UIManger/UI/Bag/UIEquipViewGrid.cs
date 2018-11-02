using Assets.Script.Battle.BattleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIEquipViewGrid : MonoBehaviour
{

    public Text txt_Name;
    public Text txt_Quality;
    public Text txt_MainNum;
    public Text txt_MainTip;
    public Text txt_OtherNum_1;
    public Text txt_OtherTip_1;
    public Text txt_OtherNum_2;
    public Text txt_OtherTip_2;
    public Text txt_WorkTip;
    public Text txt_NeedLevel;
    public Text txt_Tip_1;

    public Text txt_MainNum_T2;
    public Text txt_MainTip_T2;
    public Image icon_T2;

    public Image image_EquipIcon;
    public Image Image_EquipSprite;
    public Image image_EquipQuality;
    public Image image_FillIcon;


    public GameObject AtrGrid;
    public Transform AtrGridPoint;
    private int index = 0;
    private List<UIAtrGrid> atrGrids = new List<UIAtrGrid>();

    public Button btn_Destroy;
    public Button btn_Load;
    public Button btn_Unload;

    public bool isLoad = true;

    public Sprite[] sp;

    public GameObject InfoType_1;
    public GameObject InfoType_2;

    private HallRoleData currentRoleData;
    private EquipmentRealProperty currentequipData;
    private RectTransform ts;

    public int Index
    {
        get
        {
            if (index >= atrGrids.Count)
            {
                Debug.Log(index + " " + atrGrids.Count);
                InstanceGrid();
            }
            return index;
        }
    }

    private void Awake()
    {
        ts = GetComponent<RectTransform>();

        txt_Tip_1.text = "需求:";

        btn_Destroy.onClick.AddListener(ChickDestroy);
        btn_Load.onClick.AddListener(ChickBtnLoad);
        btn_Unload.onClick.AddListener(ChickBtnUnLoad);

        InstanceGrid();
    }

    public void UpdateInfo(EquipmentRealProperty equipData, int btnType, HallRoleData roleData)
    {
        Debug.Log("运行了");

        currentRoleData = roleData;
        currentequipData = equipData;

        txt_Name.text = equipData.Name;
        txt_Quality.text = LanguageDataMgr.instance.GetString(equipData.QualityType.ToString());

        if (equipData.ProfessionNeed == ProfessionNeedEnum.Fight)
        {
            InfoType_1.SetActive(true);
            InfoType_2.SetActive(false);
            if (equipData.EquipType == EquipTypeEnum.Sword)
            {
                ShowText(true);

                txt_MainTip.text = LanguageDataMgr.instance.GetString(equipData.HurtType.ToString());
                txt_OtherNum_1.text = equipData.RoleProperty[RoleAttribute.MinDamage].ToString("#0") + "-" + equipData.RoleProperty[RoleAttribute.MaxDamage].ToString("#0");
                txt_OtherTip_1.text = "点伤害";
                txt_OtherNum_2.text = "攻速:";
                txt_OtherTip_2.text = equipData.AttackSpeed.ToString();
                txt_MainNum.text = equipData.RoleProperty[RoleAttribute.DPS].ToString("#0");
                string Profession = LanguageDataMgr.instance.GetString(equipData.WeaponProfession + "Profession");
                txt_WorkTip.text = "职业类型: " + "<color=#b8a17f>" + Profession + "</color>";
            }
            else if (equipData.EquipType == EquipTypeEnum.Armor)
            {
                ShowText(false);

                txt_MainNum.text = equipData.RoleProperty[RoleAttribute.HP].ToString("#0");
                txt_MainTip.text = "生命值";
                string Profession = LanguageDataMgr.instance.GetString(equipData.WeaponType.ToString());
                txt_WorkTip.text = "护甲类型: " + "<color=#b8a17f>" + Profession + "</color>";
            }
            else
            {
                InfoType_1.SetActive(false);
                txt_WorkTip.text = "";
            }

            index = 0;
            AtrGridPoint.gameObject.SetActive(false);
            ChickDic(equipData);
            ChickList(equipData.SpecialProperty);
        }
        else
        {
            InfoType_1.SetActive(false);
            InfoType_2.SetActive(true);
            txt_MainNum_T2.text = equipData.RoleProperty[RoleAttribute.DPS].ToString("#0");
            txt_MainTip_T2.text = "资源产量/小时";
            string iconName = ((RoleAttribute)equipData.ProfessionNeed + 1).ToString();
            icon_T2.sprite = GetSpriteAtlas.insatnce.GetIcon(iconName);
            txt_WorkTip.text = "";
            index = 0;
        }

        if (Index == 0)
        {
        }
        else
        {
            AtrGridPoint.gameObject.SetActive(true);
        }
        for (int i = Index; i < atrGrids.Count; i++)
        {
            atrGrids[i].gameObject.SetActive(false);
        }


        txt_NeedLevel.text = equipData.Level.ToString();
        image_EquipIcon.sprite = GetSpriteAtlas.insatnce.GetIcon(equipData.ProfessionNeed.ToString());
        Image_EquipSprite.sprite = GetSpriteAtlas.insatnce.GetIcon(equipData.SpriteName.ToString());
        image_FillIcon.sprite = GetSpriteAtlas.insatnce.GetIcon("Fill_" + equipData.QualityType.ToString());
        image_EquipQuality.sprite = GetSpriteAtlas.insatnce.GetIcon("Quality_" + equipData.QualityType.ToString());

        switch (btnType)
        {
            case 0:
                btn_Destroy.gameObject.SetActive(false);
                btn_Load.gameObject.SetActive(false);
                btn_Unload.gameObject.SetActive(false);
                break;
            case 1:
                btn_Destroy.gameObject.SetActive(true);
                btn_Load.gameObject.SetActive(false);
                btn_Unload.gameObject.SetActive(false);
                break;
            case 2:
                btn_Destroy.gameObject.SetActive(false);
                btn_Load.gameObject.SetActive(false);
                btn_Unload.gameObject.SetActive(true);
                break;
            case 3:
                btn_Destroy.gameObject.SetActive(true);
                btn_Load.gameObject.SetActive(true);
                btn_Unload.gameObject.SetActive(false);
                break;
            default:
                break;
        }

        if (roleData != null)
        {
            txt_Tip_1.text = "需求";
            int roleLevel = roleData.RoleLevel[(int)equipData.ProfessionNeed - 1].Level;
            string text = "";
            string whiteText = "<color=#cccccc>{0}</color> ";
            string redText = "<color=#ee5151>{0}</color> ";
            if (roleLevel >= equipData.Level && roleData != null)
            {
                btn_Load.image.sprite = sp[0];
                txt_Tip_1.text = string.Format(whiteText, txt_Tip_1.text);
                txt_NeedLevel.text = string.Format(whiteText, txt_NeedLevel.text);
                isLoad = true;
            }
            else
            {
                btn_Load.image.sprite = sp[2];
                txt_Tip_1.text = string.Format(redText, txt_Tip_1.text);
                txt_NeedLevel.text = string.Format(redText, txt_NeedLevel.text);
                isLoad = false;
            }
        }
        Invoke("ShowPage", 0.1f);
    }

    private void ShowPage()
    {
        ts.anchoredPosition = Vector3.zero;
    }

    private void ChickDic(EquipmentRealProperty equipData)
    {
        Dictionary<RoleAttribute, float> dic = equipData.RoleProperty;

        //如果是皮甲读DPS
        if (dic[RoleAttribute.MinDamage] > 0 && equipData.EquipType != EquipTypeEnum.Sword && equipData.ProfessionNeed == ProfessionNeedEnum.Fight)
        {
            atrGrids[Index].gameObject.SetActive(true);
            string tip = "伤害 <color=#b8a17f>+" + dic[RoleAttribute.MinDamage].ToString("#0") + "-" + dic[RoleAttribute.MaxDamage].ToString("#0") + "</color>";
            atrGrids[Index].UpdateInfo(RoleAttribute.DPS, tip);
            index++;
        }

        int startPoint = 0;
        //如果是项链类
        if ((int)equipData.EquipType > 1)
        {
            startPoint = (int)RoleAttribute.Nothing;
        }
        else
        {
            startPoint = (int)RoleAttribute.HallShowAtr;
        }
        for (int i = startPoint; i < (int)RoleAttribute.Max; i++)
        {
            if (dic.ContainsKey((RoleAttribute)i) && dic[(RoleAttribute)i] != 0)
            {
                atrGrids[Index].gameObject.SetActive(true);
                string language = LanguageDataMgr.instance.GetString(((RoleAttribute)i).ToString());
                string tip = language + " <color=#b8a17f>+" + dic[(RoleAttribute)i].ToString("#0") + "</color>";
                atrGrids[Index].UpdateInfo((RoleAttribute)i, tip);
                index++;
            }
        }

    }

    private void ChickList(List<SpecialPropertyData> PropertyData)
    {
        for (int i = 0; i < PropertyData.Count; i++)
        {
            atrGrids[Index].gameObject.SetActive(true);
            string st = LanguageDataMgr.instance.GetString(PropertyData[i].SpecialPropertyType.ToString());
            st = string.Format("<color=#ee92ff>特殊效果:</color>\n" + st, "<color=#b8a17f>" + PropertyData[i].param1.ToString("#0.0") + "</color>", "<color=#b8a17f>" + PropertyData[i].param2.ToString("#0.0") + "</color>", "<color=#b8a17f>" + PropertyData[i].param3.ToString("#0.0") + "</color>");
            atrGrids[Index].UpdateInfo(RoleAttribute.Max, st);
            index++;
        }
    }

    private void InstanceGrid()
    {
        GameObject go = Instantiate(AtrGrid, AtrGridPoint) as GameObject;
        UIAtrGrid atr = go.GetComponent<UIAtrGrid>();
        atr.gameObject.SetActive(false);
        atrGrids.Add(atr);
    }

    private void ChickTip()
    {

    }

    private void ChickDestroy()
    {
        object st = "城堡低于4级无法拆分";
        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
    }
    private void ChickBtnLoad()
    {
        if (isLoad)
        {
            Debug.Log("给角色穿装备");
            currentRoleData.AddEquip(currentequipData);
            RefreshBagUI();
        }
        else
        {
            Debug.Log("角色等级不够无法装备");
            object st = "该功能暂未开放";
            UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
        }
    }
    private void ChickBtnUnLoad()
    {
        currentRoleData.UnloadEquip(currentequipData);
        RefreshBagUI();
    }

    private void RefreshBagUI()
    {
        HallEventManager.instance.SendEvent(HallEventDefineEnum.RefreshBagUI);
        UIEquipView.instance.ClosePage();
    }

    private void ShowText(bool isShow)
    {
        txt_OtherNum_1.transform.parent.gameObject.SetActive(isShow);
        txt_OtherNum_2.transform.parent.gameObject.SetActive(isShow);
    }

    public void Close()
    {
        ts.anchoredPosition = Vector2.zero * 2000;
    }
}
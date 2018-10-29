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

    private void Awake()
    {
        txt_Tip_1.text = "需求:";

        btn_Destroy.onClick.AddListener(ChickDestroy);
        btn_Load.onClick.AddListener(ChickBtnLoad);
        btn_Unload.onClick.AddListener(ChickBtnUnLoad);

        InstanceGrid();
    }

    public void UpdateInfo(EquipmentRealProperty equipData, int btnType, HallRoleData roleData)
    {
        currentRoleData = roleData;
        currentequipData = equipData;

        txt_Name.text = equipData.Name;
        txt_Quality.text = equipData.QualityType.ToString();

        if (equipData.ProfessionNeed == ProfessionNeedEnum.Fight)
        {
            InfoType_1.SetActive(true);
            InfoType_2.SetActive(false);
            if (equipData.EquipType == EquipTypeEnum.Sword)
            {
                ShowText(true);

                txt_MainTip.text = equipData.HurtType + "伤害/秒";
                txt_OtherNum_1.text = equipData.RoleProperty[RoleAttribute.MinDamage].ToString("#0") + "-" + equipData.RoleProperty[RoleAttribute.MaxDamage].ToString("#0");
                txt_OtherTip_1.text = "点伤害";
                txt_OtherNum_2.text = "攻速:";
                txt_OtherTip_2.text = equipData.AttackSpeed.ToString();
                txt_MainNum.text = equipData.RoleProperty[RoleAttribute.DPS].ToString("#0");
                txt_WorkTip.text = "职业类型: " + equipData.WeaponProfession.ToString();
            }
            else if (equipData.EquipType == EquipTypeEnum.Armor)
            {
                ShowText(false);

                txt_MainNum.text = equipData.RoleProperty[RoleAttribute.HP].ToString("#0");
                txt_MainTip.text = "生命值";
                txt_WorkTip.text = equipData.WeaponType.ToString();
            }
            else
            {
                InfoType_1.SetActive(false);
                txt_WorkTip.text = "";
            }

            index = 0;
            ChickDic(equipData);
            ChickList(equipData.SpecialProperty);
            if (index == 0)
            {
                AtrGridPoint.gameObject.SetActive(false);
            }
            else
            {
                AtrGridPoint.gameObject.SetActive(true);
            }
            for (int i = index; i < atrGrids.Count; i++)
            {
                atrGrids[i].gameObject.SetActive(false);
            }
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

            //atrGrids[0].UpdateInfo(equipData.)
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
            int roleLevel = roleData.RoleLevel[(int)equipData.ProfessionNeed].Level;
            string text = "";
            string whiteText = "<color=#cccccc>{0}</color> ";
            string redText = "<color=#e6402f>{0}</color> ";
            if (roleLevel >= equipData.Level)
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
    }

    private void ChickDic(EquipmentRealProperty equipData)
    {
        Dictionary<RoleAttribute, float> dic = equipData.RoleProperty;

        //如果是皮甲读DPS
        if (dic[RoleAttribute.MinDamage] > 0 && equipData.EquipType != EquipTypeEnum.Sword)
        {
            atrGrids[index].gameObject.SetActive(true);
            string tip = "伤害:" + dic[RoleAttribute.MinDamage].ToString("#0") + "-" + dic[RoleAttribute.MaxDamage].ToString("#0");
            atrGrids[index].UpdateInfo(RoleAttribute.DPS, tip);
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
            if (index >= atrGrids.Count)
            {
                InstanceGrid();
            }
            if (dic.ContainsKey((RoleAttribute)i) && dic[(RoleAttribute)i] != 0)
            {
                Debug.Log("Show :" + (RoleAttribute)i);
                atrGrids[index].gameObject.SetActive(true);
                string tip = (RoleAttribute)i + " +" + dic[(RoleAttribute)i].ToString("#0");
                atrGrids[index].UpdateInfo((RoleAttribute)i, tip);
                index++;
            }
        }

    }

    private void ChickList(List<SpecialPropertyData> PropertyData)
    {
        for (int i = 0; i < PropertyData.Count; i++)
        {
            atrGrids[index].gameObject.SetActive(true);
            atrGrids[index].UpdateInfo(RoleAttribute.Max, PropertyData[i].SpecialPropertyType.ToString());
            index++;
        }
        if (index >= atrGrids.Count)
        {
            InstanceGrid();
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
}
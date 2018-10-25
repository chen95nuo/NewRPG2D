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
    public Image image_EquipQuality;
    public Image image_FillIcon;


    public GameObject AtrGrid;
    public Transform AtrGridPoint;
    private int index = 0;
    private List<UIAtrGrid> atrGrids;

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

        txt_Name.text = equipData.EquipName;
        txt_Quality.text = equipData.QualityType.ToString();

        if (equipData.ProfessionNeed == ProfessionNeedEnum.Fight)
        {
            InfoType_1.SetActive(true);
            InfoType_2.SetActive(false);
            txt_MainNum.text = equipData.RoleProperty[RoleAttribute.DPS].ToString();
            txt_MainTip.text = equipData.HurtType + "伤害/秒";
            txt_OtherNum_1.text = equipData.RoleProperty[RoleAttribute.DPS].ToString("#0");
            txt_OtherTip_1.text = "点伤害";
            txt_OtherNum_2.text = "攻速:";
            txt_OtherTip_2.text = equipData.AttackSpeed.ToString();
            txt_WorkTip.text = "职业类型: " + equipData.WeaponProfession.ToString();

            index = 0;
            ChickDic(equipData.RoleProperty);
            ChickList(equipData.SpecialProperty);
            if (index == 0)
            {
                AtrGridPoint.gameObject.SetActive(false);
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
            txt_MainNum_T2.text = equipData.RoleProperty[RoleAttribute.DPS].ToString();
            txt_MainTip_T2.text = "资源产量/小时";
            string iconName = ((RoleAttribute)equipData.ProfessionNeed + 1).ToString();
            icon_T2.sprite = GetSpriteAtlas.insatnce.GetIcon(iconName);
            txt_WorkTip.text = "";

            //atrGrids[0].UpdateInfo(equipData.)
        }


        txt_NeedLevel.text = equipData.Level.ToString();
        image_EquipIcon.sprite = GetSpriteAtlas.insatnce.GetIcon(equipData.ProfessionNeed.ToString());
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
                btn_Destroy.gameObject.SetActive(true);
                btn_Load.gameObject.SetActive(true);
                btn_Unload.gameObject.SetActive(false);
                break;
            default:
                break;
        }

        if (btnType == 2)
        {
            int roleLevel = roleData.GetAtrLevel((RoleAttribute)equipData.ProfessionNeed);
            if (roleLevel >= equipData.Level)
            {
                btn_Load.image.sprite = sp[0];
                isLoad = true;
            }
            else
            {
                btn_Load.image.sprite = sp[2];
                isLoad = false;
            }
        }
    }

    private void ChickDic(Dictionary<RoleAttribute, float> dic)
    {
        foreach (var item in dic)
        {
            if (item.Value > 0 && item.Key != RoleAttribute.HurtType)
            {
                atrGrids[index].gameObject.SetActive(true);
                string tip = item.Key.ToString() + " +" + item.Value;
                atrGrids[index].UpdateInfo(item.Key, tip);
            }
            if (index >= atrGrids.Count)
            {
                InstanceGrid();
            }
        }
    }

    private void ChickList(List<SpecialPropertyData> PropertyData)
    {
        for (int i = 0; i < PropertyData.Count; i++)
        {
            atrGrids[index].gameObject.SetActive(true);
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
        UIPanelManager.instance.ShowPage<UIPopUp_2>("城堡低于4级无法拆分");
    }
    private void ChickBtnLoad()
    {
        if (isLoad)
        {
            Debug.Log("给角色穿装备");
            currentRoleData.AddEquip(currentequipData);
        }
        else
        {
            UIPanelManager.instance.ShowPage<UIPopUp_2>("该角色等级不够无法装备");
        }
    }
    private void ChickBtnUnLoad()
    {

    }
}

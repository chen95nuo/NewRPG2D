using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Script.UIManger;
using Assets.Script.Battle;

public class UIRoleInfo : TTUIPage
{

    public GameObject Train;
    public GameObject LoveTip;

    #region GetText
    public Text txt_TrainTime;
    public Text txt_HateLoveTime;

    public Text txt_fight;
    public Text txt_food;
    public Text txt_gold;
    public Text txt_mana;
    public Text txt_wood;
    public Text txt_iron;

    public Text txt_Dps;
    public Text txt_DpsTip;
    public Text txt_CrtType;
    public Text txt_CrtTip;
    public Text txt_PArmor;
    public Text txt_PArmorTip;
    public Text txt_MArmor;
    public Text txt_MArmorTip;
    public Text txt_Dodge;
    public Text txt_DodgeTip;
    public Text txt_Hit;
    public Text txt_HitTip;
    public Text txt_INT;
    public Text txt_INTTip;
    public Text txt_Hp;
    #endregion

    #region Slider
    public Image slider_HP;
    public Image slider_Train;
    public Image slider_Love;
    #endregion

    public Button btn_back;
    #region Bag
    public Button[] btn_AllType;
    public ScrollControl sc;
    #endregion

    private int currentBtnNumb = 0;

    [System.NonSerialized]
    public HallRoleData currentRole;

    #region 角色装备
    public Button[] btn_Equip;
    [SerializeField]
    private Image[] equipIcon;
    [SerializeField]
    private Sprite[] equipBGSp;
    private EquipmentRealProperty[] roleEquips;
    #endregion

    #region 角色皮肤
    //public //这里是两个角色实例
    //这个是用的角色实例的引用
    #endregion


    private void Awake()
    {
        HallEventManager.instance.AddListener<int>(HallEventDefineEnum.ChickRoleTrain, ChickTrainTime);//这个是用来找训练参数的 后期需要优化
        HallEventManager.instance.AddListener<EquipmentRealProperty>(HallEventDefineEnum.ShowEquipInfo, BagChickRoleEquip);

        btn_back.onClick.AddListener(ClosePage);
        for (int i = 0; i < btn_AllType.Length; i++)
        {
            btn_AllType[i].onClick.AddListener(ChickBagType);
        }
        for (int i = 0; i < btn_Equip.Length; i++)
        {
            btn_Equip[i].onClick.AddListener(ChickEquipClick);
        }
        btn_AllType[currentBtnNumb].interactable = false;
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.ChickRoleTrain, ChickTrainTime);
        HallEventManager.instance.RemoveListener<EquipmentRealProperty>(HallEventDefineEnum.ShowEquipInfo, BagChickRoleEquip);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        currentRole = mData as HallRoleData;
        UpdateInfo(currentRole);
        ChickLevelUI(false);
    }

    public void UpdateInfo(HallRoleData data)
    {
        txt_fight.text = data.RoleLevel[0].Level.ToString();
        txt_food.text = data.RoleLevel[1].Level.ToString();
        txt_gold.text = data.RoleLevel[2].Level.ToString();
        txt_mana.text = data.RoleLevel[3].Level.ToString();
        txt_wood.text = data.RoleLevel[4].Level.ToString();
        txt_iron.text = data.RoleLevel[5].Level.ToString();
        ChickLevelUI(true);

        txt_Dps.text = (data.Attack).ToString();
        txt_DpsTip.text = string.Format("{0}伤害/秒", (HurtTypeEnum)data.HurtType);
        ChickAtr(data);//检查属性
        txt_Hp.text = data.NowHp + "/" + data.Health;


        if (data.LoveType == RoleLoveType.boredom)
        {
            LoveTip.SetActive(true);
        }
        else
        {
            LoveTip.SetActive(false);
        }

        if (data.TrainType == RoleTrainType.LevelUp)
        {
            Train.SetActive(true);
        }
        else
        {
            Train.SetActive(false);
        }

        GetRoleEquip(data);
    }

    /// <summary>
    /// 检查等级UI显示数量
    /// </summary>
    /// <param name="isTrue"></param>
    public void ChickLevelUI(bool isTrue)
    {
        PlayerData playerData = GetPlayerData.Instance.GetData();
        if (playerData.MainHallLevel > 4 || isTrue == false)
        {
            txt_mana.transform.parent.gameObject.SetActive(isTrue);
        }
        if (playerData.MainHallLevel >= 6 || isTrue == false)
        {
            txt_wood.transform.parent.gameObject.SetActive(isTrue);
        }
        if (playerData.MainHallLevel >= 9 || isTrue == false)
        {
            txt_iron.transform.parent.gameObject.SetActive(isTrue);
        }
    }


    public void ChickAtr(HallRoleData data)
    {
        if (data.Crt > 0)
        {
            txt_CrtTip.text = data.DPS.ToString();
        }
        else
        {
            txt_CrtTip.transform.parent.gameObject.SetActive(false);
        }
        if (data.PArmor > 0)
        {
            txt_PArmor.text = data.PArmor.ToString();
        }
        else
        {
            txt_PArmor.transform.parent.gameObject.SetActive(false);
        }
        if (data.MArmor > 0)
        {
            txt_MArmor.text = data.MArmor.ToString();
        }
        else
        {
            txt_MArmor.transform.parent.gameObject.SetActive(false);
        }
        if (data.Dodge > 0)
        {
            txt_Dodge.text = data.Dodge.ToString();
        }
        else
        {
            txt_Dodge.transform.parent.gameObject.SetActive(false);
        }
        if (data.HIT > 0)
        {
            txt_Hit.text = data.Dodge.ToString();
        }
        else
        {
            txt_Hit.transform.parent.gameObject.SetActive(false);
        }
        if (data.INT > 0)
        {
            txt_INT.text = data.INT.ToString();
        }
        else
        {
            txt_INT.transform.parent.gameObject.SetActive(false);
        }
    }

    private void ChickBagType()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < btn_AllType.Length; i++)
        {
            if (btn_AllType[i].gameObject == go)
            {
                btn_AllType[currentBtnNumb].interactable = true;
                btn_AllType[i].interactable = false;
                sc.UpdateInfo((BagType)i);
                currentBtnNumb = i;
                return;
            }
        }
        Debug.LogError("没有找到对应按钮");
    }

    private void ChickTrainTime(int index)
    {

        RoleTrainHelper trainRole = HallRoleMgr.instance.FindTrainRole(index);
        if (trainRole.role == currentRole)
        {
            slider_Train.fillAmount = (trainRole.maxTime - trainRole.time) / trainRole.maxTime;
            txt_TrainTime.text = SystemTime.instance.TimeNormalized(trainRole.time);
        }
    }

    private void BagChickRoleEquip(EquipmentRealProperty equipData)
    {
        if (roleEquips[(int)equipData.EquipType] != null)
        {
            UIEquipInfoHelper_1 data = new UIEquipInfoHelper_1(roleEquips[(int)equipData.EquipType], equipData, currentRole);
            UIPanelManager.instance.ShowPage<UIEquipView>(data);
            return;
        }
        UIEquipInfoHelper data_1 = new UIEquipInfoHelper(equipData, currentRole);
        UIPanelManager.instance.ShowPage<UIEquipView>(data_1);
    }

    /// <summary>
    /// 获取角色装备
    /// </summary>
    /// <param name="role"></param>
    private void GetRoleEquip(HallRoleData role)
    {
        roleEquips = new EquipmentRealProperty[5];
        for (int i = 0; i < role.Equip.Length; i++)
        {
            if (role.Equip[i] != 0)
            {
                roleEquips[i] = EquipmentMgr.instance.GetEquipmentByEquipId(role.Equip[i]);
                btn_Equip[i].image.sprite = GetSpriteAtlas.insatnce.GetIcon("Quality_" + roleEquips[i].QualityType.ToString());
                equipIcon[i].sprite = GetSpriteAtlas.insatnce.GetIcon(roleEquips[i].SpriteName);
                //这里顺便更换角色皮肤
            }
            else
            {
                equipIcon[i].sprite = equipBGSp[i];
            }
        }
    }

    private void ChickEquipClick()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < btn_Equip.Length; i++)
        {
            if (go == btn_Equip[i].gameObject && roleEquips[i] != null)
            {
                Debug.Log(string.Format("获取{0}装备", i));
                UIEquipInfoHelper_1 data = new UIEquipInfoHelper_1(roleEquips[i], null, currentRole);
                UIPanelManager.instance.ShowPage<UIEquipView>(data);
            }
        }
    }
}
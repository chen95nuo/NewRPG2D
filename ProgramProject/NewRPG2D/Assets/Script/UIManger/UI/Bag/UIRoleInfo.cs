using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Script.UIManger;
using Assets.Script.Battle;
using System;
using DG.Tweening;

public class UIRoleInfo : TTUIPage
{

    public GameObject Train;
    public GameObject LoveTip;

    #region GetText
    public Text txt_Name;
    public Text txt_TrainTime;
    public Text txt_HateLoveTime;

    public Text[] txt_Level;
    public Button[] btn_Level;

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

    public Image[] sp_Star;
    public Sprite[] starSp;

    #region 角色皮肤
    //public //这里是两个角色实例
    //这个是用的角色实例的引用
    #endregion

    public RectTransform bag;
    public Button btn_Arrow;
    private bool showBag = true;

    public ChangeRoleEquip[] roleAnim;
    private ChangeRoleEquip currentRoleSkil;

    public bool ShowBag
    {
        get
        {
            return showBag;
        }

        set
        {
            bool temp = value;
            if (temp != showBag)
            {
                showBag = value;
                if (showBag)
                {
                    Debug.Log("运行了");
                    bag.DOAnchorPos(new Vector2(-15, 0), 1.0f);
                }
                else
                {
                    Debug.Log("运行了");
                    bag.DOAnchorPos(new Vector2(-815, 0), 1.0f);
                }
            }
        }
    }

    private void Awake()
    {
        HallEventManager.instance.AddListener(HallEventDefineEnum.RefreshBagUI, RefreshUI);//背包有变动 刷新UI
        HallEventManager.instance.AddListener<int>(HallEventDefineEnum.ChickRoleTrain, ChickTrainTime);//这个是用来找训练参数的 后期需要优化
        HallEventManager.instance.AddListener<EquipmentRealProperty>(HallEventDefineEnum.ShowEquipInfo, BagChickRoleEquip);

        btn_back.onClick.AddListener(ClosePage);
        btn_Arrow.onClick.AddListener(ChickArrow);
        for (int i = 0; i < btn_AllType.Length; i++)
        {
            btn_AllType[i].onClick.AddListener(ChickBagType);
        }
        for (int i = 0; i < btn_Equip.Length; i++)
        {
            btn_Equip[i].onClick.AddListener(ChickEquipClick);
        }
        for (int i = 0; i < btn_Level.Length; i++)
        {
            btn_Level[i].onClick.AddListener(ChickLevelTip);
        }
        btn_AllType[currentBtnNumb].interactable = false;
    }

    private void ChickLevelTip()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < btn_Level.Length; i++)
        {
            if (btn_Level[i].gameObject == go)
            {
                string st = LanguageDataMgr.instance.GetString(((TrainType)i) + "Tip");
                RoleInfoTipHelper data = new RoleInfoTipHelper(btn_Level[i].transform, st, true, currentRole.RoleLevel[i].atr);
                UIPanelManager.instance.ShowPage<UIPopUpTips_1>(data);
            }
        }
    }

    private void ChickArrow()
    {
        ShowBag = !ShowBag;
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.RefreshBagUI, RefreshUI);//背包有变动 刷新UI
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
        txt_Name.text = data.Name;
        for (int i = 0; i < txt_Level.Length; i++)
        {
            txt_Level[i].text = data.RoleLevel[i].Level.ToString();
        }
        ChickLevelUI(true);

        txt_Dps.text = (data.Attack).ToString();
        string st = LanguageDataMgr.instance.GetString(((HurtTypeEnum)data.HurtType).ToString());
        txt_DpsTip.text = st;
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
        Debug.Log("星级: " + data.Star);
        for (int i = 0; i < sp_Star.Length; i++)
        {
            if (i <= data.Star - 1)
            {
                sp_Star[i].sprite = starSp[0];
            }
            else
            {
                sp_Star[i].sprite = starSp[1];
            }
        }

        switch (data.sexType)
        {
            case SexTypeEnum.Man:
                currentRoleSkil = roleAnim[0];
                roleAnim[0].gameObject.SetActive(true);
                roleAnim[1].gameObject.SetActive(false);
                break;
            case SexTypeEnum.Woman:
                currentRoleSkil = roleAnim[1];
                roleAnim[0].gameObject.SetActive(false);
                roleAnim[1].gameObject.SetActive(true);
                break;
            default:
                break;
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
            txt_Level[(int)TrainType.Mana].transform.parent.gameObject.SetActive(isTrue);
        }
        if (playerData.MainHallLevel >= 6 || isTrue == false)
        {
            txt_Level[(int)TrainType.Wood].transform.parent.gameObject.SetActive(isTrue);
        }
        if (playerData.MainHallLevel >= 9 || isTrue == false)
        {
            txt_Level[(int)TrainType.Iron].transform.parent.gameObject.SetActive(isTrue);
        }
    }


    public void ChickAtr(HallRoleData data)
    {
        if (data.Crt > 0)
        {
            txt_CrtTip.transform.parent.gameObject.SetActive(true);
            txt_CrtTip.text = data.Attack.ToString("#0");
        }
        else
        {
            txt_CrtTip.transform.parent.gameObject.SetActive(false);
        }
        if (data.PArmor > 0)
        {
            txt_PArmor.transform.parent.gameObject.SetActive(true);
            txt_PArmor.text = data.PArmor.ToString("#0");
        }
        else
        {
            txt_PArmor.transform.parent.gameObject.SetActive(false);
        }
        if (data.MArmor > 0)
        {
            txt_MArmor.transform.parent.gameObject.SetActive(true);
            txt_MArmor.text = data.MArmor.ToString("#0");
        }
        else
        {
            txt_MArmor.transform.parent.gameObject.SetActive(false);
        }
        if (data.Dodge > 0)
        {
            txt_Dodge.transform.parent.gameObject.SetActive(true);
            txt_Dodge.text = data.Dodge.ToString("#0");
        }
        else
        {
            txt_Dodge.transform.parent.gameObject.SetActive(false);
        }
        if (data.HIT > 0)
        {
            txt_Hit.transform.parent.gameObject.SetActive(true);
            txt_Hit.text = data.Dodge.ToString("#0");
        }
        else
        {
            txt_Hit.transform.parent.gameObject.SetActive(false);
        }
        if (data.INT > 0)
        {
            txt_INT.transform.parent.gameObject.SetActive(true);
            txt_INT.text = data.INT.ToString("#0");
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
        if (trainRole.roleID == currentRole.id)
        {
            slider_Train.fillAmount = (trainRole.maxTime - trainRole.time) / trainRole.maxTime;
            txt_TrainTime.text = SystemTime.instance.TimeNormalizedOf(trainRole.time, false);
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
        currentRoleSkil.ChangeOriginalEquip(EquipTypeEnum.Sword);
        currentRoleSkil.ChangeOriginalEquip(EquipTypeEnum.Armor);
        for (int i = 0; i < role.Equip.Length; i++)
        {
            if (role.Equip[i] != 0)
            {
                roleEquips[i] = EquipmentMgr.instance.GetEquipmentByEquipId(role.Equip[i]);
                btn_Equip[i].image.sprite = GetSpriteAtlas.insatnce.GetIcon("Quality_" + roleEquips[i].QualityType.ToString());
                equipIcon[i].sprite = GetSpriteAtlas.insatnce.GetIcon(roleEquips[i].SpriteName);
                //这里顺便更换角色皮肤
                currentRoleSkil.ChangeEquip(roleEquips[i].EquipType, roleEquips[i].EquipName, role.sexType);
            }
            else
            {
                equipIcon[i].sprite = equipBGSp[i];
                btn_Equip[i].image.sprite = GetSpriteAtlas.insatnce.GetIcon("Quality_" + QualityTypeEnum.White);
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

    private void RefreshUI()
    {
        UpdateInfo(currentRole);
        sc.UpdateInfo((BagType)currentBtnNumb);
    }
}

public class RoleInfoTipHelper
{
    public Transform ts;
    public string st;
    public bool needIcon;
    public RoleAttribute atr;
    public RoleInfoTipHelper(Transform ts, string st, bool needIcon = false, RoleAttribute atr = RoleAttribute.Max)
    {
        this.ts = ts;
        this.st = st;
        this.needIcon = needIcon;
        this.atr = atr;
    }
}
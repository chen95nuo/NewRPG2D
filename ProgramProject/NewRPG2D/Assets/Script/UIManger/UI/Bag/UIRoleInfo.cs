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

    private int currentbtnNumb = 0;

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
        HallEventManager.instance.AddListener<EquipmentRealProperty>(HallEventDefineEnum.ShowEquipInfo, ChickShowEquip);
        HallEventManager.instance.AddListener<int>(HallEventDefineEnum.ChickRoleTrain, ChickTrainTime);//这个是用来找训练参数的 后期需要优化

        btn_back.onClick.AddListener(ClosePage);
        for (int i = 0; i < btn_AllType.Length; i++)
        {
            btn_AllType[i].onClick.AddListener(ChickBagType);
        }
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<EquipmentRealProperty>(HallEventDefineEnum.ShowEquipInfo, ChickShowEquip);
        HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.ChickRoleTrain, ChickTrainTime);
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
        txt_fight.text = data.FightLevel.ToString();
        txt_food.text = data.FoodLevel.ToString();
        txt_gold.text = data.GoldLevel.ToString();
        txt_mana.text = data.ManaLevel.ToString();
        txt_wood.text = data.WoodLevel.ToString();
        txt_iron.text = data.IronLevel.ToString();
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
                btn_AllType[currentbtnNumb].interactable = true;
                btn_AllType[i].interactable = false;
                sc.UpdateInfo((BagType)i);
                currentbtnNumb = i;
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

    private void ChickShowEquip(EquipmentRealProperty bagEquipData)
    {
        EquipmentRealProperty roleEquipData = roleEquips[(int)bagEquipData.EquipType];
        if (roleEquipData != null)
        {
            UIEquipInfoHelper data = new UIEquipInfoHelper(roleEquipData, bagEquipData);
            UIPanelManager.instance.ShowPage<UIEquipInfo>(data);
        }
        else
        {
            UIPanelManager.instance.ShowPage<UIEquipInfo>(bagEquipData);
        }
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
                btn_Equip[i].image.sprite = GetSpriteAtlas.insatnce.GetIcon(roleEquips[i].QualityType.ToString());
                equipIcon[i].sprite = GetSpriteAtlas.insatnce.GetIcon(roleEquips[i].SpriteName);
                //这里顺便更换角色皮肤
            }
            else
            {
                equipIcon[i].sprite = equipBGSp[i];
            }
        }
    }
}
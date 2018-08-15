using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using TinyTeam.UI;
using Spine.Unity;
using System;
using DG.Tweening;

public class UIRoleInformation : MonoBehaviour
{
    public Text roleName;
    public Text roleLevel;
    public Text roleExp;
    public Slider roleExpSlider;
    public Text roleHeart;
    //public SkeletonGraphic role;
    public Image roleQuality;
    public Image roleAttribute;
    public Image roleStars;

    public UIRoleEquip[] roleEquip;
    public Sprite[] roleEquipImage;

    public UIRoleAttribute roleHealth;
    public UIRoleAttribute roleAttack;
    public UIRoleAttribute roleAgile;
    public UIRoleAttribute roleDefense;

    public UIRoleSkill role1Skill;
    public UIRoleSkill role2Skill;
    public Text roleSkillLevel;
    public Text roleSkillExp;
    public Slider roleSkillExpSlider;

    private CardData roleData;

    public int currentNumber;

    public Button btn_back;
    public Button btn_Strengthen;
    public Button btn_Innate;
    public Button btn_Potential;
    public Button btn_Gift;
    public Button btn_Disband;

    public UIRoleStrengthen pickUpRoleStrentthen;

    private CardData cardData;
    private SpriteAtlas getImage;

    public SkeletonGraphic Anim_DaBai;
    public SkeletonGraphic Anim_MoWu;

    public Animation anim;

    public CardData RoleData
    {
        get
        {
            return roleData;
        }
    }

    public void Awake()
    {
        getImage = Resources.Load<SpriteAtlas>("UISpriteAtlas/CardImage");

        for (int i = 0; i < roleEquip.Length; i++)
        {
            roleEquip[i].roleEquipOptions.onClick.AddListener(CheckCurrentEquipButton);
        }
        UIEventManager.instance.AddListener<UIBagGrid>(UIEventDefineEnum.UpdateEquipsEvent, updateMessage);
        UIEventManager.instance.AddListener<RoleAtrType>(UIEventDefineEnum.UpdateLittleTipEvent, ShowLittleTip);
        UIEventManager.instance.AddListener(UIEventDefineEnum.UpdateBagItemMessageEvent, RemoveThisEquip);

        Init();


    }

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<UIBagGrid>(UIEventDefineEnum.UpdateEquipsEvent, updateMessage);
        UIEventManager.instance.RemoveListener<RoleAtrType>(UIEventDefineEnum.UpdateExploreTipEvent, ShowLittleTip);
        UIEventManager.instance.RemoveListener(UIEventDefineEnum.UpdateBagItemMessageEvent, RemoveThisEquip);
        UIEventManager.instance.RemoveListener<CardData>(UIEventDefineEnum.UpdateCardMessageEvent, GetCardData);

    }

    private void Init()
    {
        btn_back.GetComponent<Button>().onClick.AddListener(ChickBack);
        btn_Strengthen.GetComponent<Button>().onClick.AddListener(ShowRoleStrengthenPage);
        btn_Innate.GetComponent<Button>().onClick.AddListener(ShowRoleInnatePage);
        btn_Potential.GetComponent<Button>().onClick.AddListener(ShowRolePotentialPage);
        btn_Gift.GetComponent<Button>().onClick.AddListener(ShowRoleGiftPage);
        btn_Disband.GetComponent<Button>().onClick.AddListener(ShowRoleDisbandPage);
        pickUpRoleStrentthen.gameObject.SetActive(false);

        UIEventManager.instance.AddListener<CardData>(UIEventDefineEnum.UpdateCardMessageEvent, GetCardData);
    }

    //动画系统
    private void OnEnable()
    {
        anim.Play("UIRoleMain");
        Image image = transform.Find("BG").GetComponent<Image>();
        image.DOFade(0.5f, 0.4f);
    }

    public void Start()
    {
        Image[] images = transform.GetComponentsInChildren<Image>(true);
        GetSpriteAtlas.insatnce.SetImage(images, getImage);
    }

    /// <summary>
    /// 获取卡牌信息
    /// </summary>
    /// <param name="data"></param>
    private void GetCardData(CardData data)
    {
        cardData = data;
        ShowRoleMessagePage();
    }

    /// <summary>
    /// 更新当前卡信息
    /// </summary>
    public void ShowRoleMessagePage()
    {
        updateMessage(cardData);
    }

    /// <summary>
    /// 显示强化
    /// </summary>
    public void ShowRoleStrengthenPage()
    {
        if (cardData.Fighting)
        {
            TinyTeam.UI.TTUIPage.ShowPage<UIMessageTipPage>();
            UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateMessageTipEvent, "正在探险");
            return;
        }

        pickUpRoleStrentthen.UpdateRole(this);
        anim.Play("UIRoleMain_out");
    }
    /// <summary>
    /// 天赋
    /// </summary>
    private void ShowRoleInnatePage()
    {
        TTUIPage.ShowPage<UIMessageTipPage>();
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateMessageTipEvent, "天赋系统暂未开放");
    }
    /// <summary>
    /// 潜能
    /// </summary>
    private void ShowRolePotentialPage()
    {
        TTUIPage.ShowPage<UIMessageTipPage>();
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateMessageTipEvent, "潜能系统暂未开放");
    }
    /// <summary>
    /// 礼物
    /// </summary>
    private void ShowRoleGiftPage()
    {
        TTUIPage.ShowPage<UIMessageTipPage>();
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateMessageTipEvent, "礼物系统暂未开放");
    }
    /// <summary>
    /// 遣散
    /// </summary>
    private void ShowRoleDisbandPage()
    {
        TTUIPage.ShowPage<UIMessageTipPage>();
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateMessageTipEvent, "遣散系统暂未开放");
    }

    /// <summary>
    /// 判断该位置是否存有装备
    /// </summary>
    public void CheckCurrentEquipButton()
    {
        Debug.Log("判断是否有装备");
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < roleEquip.Length; i++)
        {
            if (roleEquip[i].roleEquipOptions.gameObject == go)
            {
                currentNumber = i;
                Debug.Log(i);
                Debug.Log(currentNumber + "变动了");
                if (RoleData.Equipdata[i].EquipType != EquipType.Nothing)
                {
                    TTUIPage.ShowPage<UIBagItemMessage>();
                    UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateBagItemMessageEvent, RoleData.Equipdata[i]);
                }
                else
                {
                    OpenEquipOpitions();
                }
            }
        }
    }

    /// <summary>
    /// 打开该类型背包菜单
    /// </summary>
    /// <param name="index"></param>
    public void OpenEquipOpitions()
    {
        TTUIPage.ShowPage<UIUseItemBagPage>();
        Debug.Log(currentNumber + "打开了背包");
        if (currentNumber > 2)
        {
            UIEventManager.instance.SendEvent<EquipType>(UIEventDefineEnum.UpdateEquipsEvent, EquipType.Necklace);
        }
        else
        {
            UIEventManager.instance.SendEvent<EquipType>(UIEventDefineEnum.UpdateEquipsEvent, (EquipType)currentNumber + 1);
        }
    }

    /// <summary>
    /// 卸载当前使用的装备
    /// </summary>
    public void RemoveThisEquip()
    {
        BagEquipData.Instance.AddItem(RoleData.Equipdata[currentNumber]);
        RoleData.Equipdata[currentNumber] = new EquipData();
        RoleData.Equipdata[currentNumber].EquipType = EquipType.Nothing;
        updateMessage(RoleData);

        TTUIPage.ClosePage<UIBagItemMessage>();
    }


    public void UpdateMassage()
    {
        if (roleData != null)
        {
            updateMessage(roleData);
        }
    }
    public void updateMessage(UIBagGrid data)
    {
        //获取当前角色在当前武器类型的位置是否有装备 如果有将原装备放回背包，将新的装备放到角色身上
        //如果当前角色装备栏上这件装备不是空的
        if (RoleData.Equipdata[currentNumber].EquipType != EquipType.Nothing)
        {
            EquipData equipData;
            //将原有装备放回背包序列
            equipData = RoleData.Equipdata[(int)data.equipData.EquipType - 1];
            RoleData.Equipdata[currentNumber] = data.equipData;
            BagEquipData.Instance.Remove(data.equipData);
            BagEquipData.Instance.AddItem(equipData);
        }
        else
        {
            RoleData.Equipdata[currentNumber] = data.equipData;
            BagEquipData.Instance.Remove(data.equipData);
        }
        updateMessage(RoleData);
    }
    /// <summary>
    /// 更新卡牌信息
    /// </summary>
    /// <param name="data"></param>
    public void updateMessage(CardData data)
    {
        roleData = data;
        roleName.text = data.Name;
        roleLevel.text = "LV." + data.Level.ToString();
        roleExp.text = data.Exp.ToString() + "/" + GameCardExpData.Instance.GetItem(data.Level).NeedExp;
        roleExpSlider.maxValue = GameCardExpData.Instance.GetItem(data.Level).NeedExp;
        roleExpSlider.value = data.Exp;
        roleHeart.text = data.GoodFeeling.ToString();
        //role.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
        if (data.AnimationName == "Anim_Dabai")
        {
            Anim_MoWu.enabled = false;
            Anim_DaBai.enabled = true;
            //role.AnimationState.SetAnimation(0, "stand", true);
        }
        else if (data.AnimationName == "Anim_Mowu")
        {
            Anim_DaBai.enabled = false;
            Anim_MoWu.enabled = true;
            //role.AnimationState.SetAnimation(0, "stand", true);
        }
        roleQuality.sprite = getImage.GetSprite("roleQuality_" + data.Quality);
        roleAttribute.sprite = IconMgr.Instance.GetIcon("Att_" + data.Attribute);
        roleStars.sprite = IconMgr.Instance.GetIcon("Stars_" + data.Stars);
        for (int i = 0; i < roleEquip.Length; i++)
        {
            if (data.Equipdata[i] != null && data.Equipdata[i].EquipType != EquipType.Nothing)
            {
                Debug.Log("刷新了");
                roleEquip[i].roleEquipQuality.gameObject.SetActive(true);
                roleEquip[i].roleEquipImage.sprite = IconMgr.Instance.GetIcon(data.Equipdata[i].SpriteName);
                roleEquip[i].roleEquipQuality.sprite = IconMgr.Instance.GetIcon("quality_" + data.Equipdata[i].Quality);
            }
            else
            {
                roleEquip[i].roleEquipImage.sprite = roleEquipImage[i];
                roleEquip[i].roleEquipQuality.gameObject.SetActive(false);
            }
        }
        roleHealth.roleValue.text = data.Health.ToString();
        roleHealth.roleScore.text = data.HealthGrow.ToString("#0.0");
        Sprite level = RoleGrade(data.HealthGrow, data.HealthMinGrow, data.HealthMaxGrow);
        roleHealth.roleQualityImage.sprite = level;

        roleAttack.roleValue.text = data.Attack.ToString();
        roleAttack.roleScore.text = data.AttackGrow.ToString("#0.0");
        roleAttack.roleQualityImage.sprite = RoleGrade(data.AttackGrow, data.AttackMinGrow, data.AttackMaxGrow);

        roleAgile.roleValue.text = data.Agile.ToString();
        roleAgile.roleScore.text = data.AgileGrow.ToString("#0.0");
        roleAgile.roleQualityImage.sprite = RoleGrade(data.AgileGrow, data.AgileMinGrow, data.AgileMaxGrow);

        roleDefense.roleValue.text = data.Defense.ToString();
        roleDefense.roleScore.text = data.DefenseGrow.ToString("#0.0");
        roleDefense.roleQualityImage.sprite = RoleGrade(data.DefenseGrow, data.DefenseMinGrow, data.DefenseMaxGrow);
    }

    private void ShowLittleTip(RoleAtrType type)
    {
        string message = "";
        switch (type)
        {
            case RoleAtrType.Nothing:
                break;
            case RoleAtrType.Health:
                message = RoleData.HealthMinGrow.ToString("#0.0") + " ~ " + RoleData.HealthMaxGrow.ToString("#0.0");
                break;
            case RoleAtrType.Attack:
                message = RoleData.AttackMinGrow.ToString("#0.0") + " ~ " + RoleData.AttackMaxGrow.ToString("#0.0");
                break;
            case RoleAtrType.Agile:
                message = RoleData.AgileMinGrow.ToString("#0.0") + " ~ " + RoleData.AgileMaxGrow.ToString("#0.0");
                break;
            case RoleAtrType.Defense:
                message = RoleData.DefenseMinGrow.ToString("#0.0") + " ~ " + RoleData.DefenseMaxGrow.ToString("#0.0");
                break;
            default:
                break;
        }
        if (message != "")
        {
            Debug.Log("Message: " + message);
            //TTUIPage.ShowPage<UILittleTipPage>();
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, message);
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, this.transform);
        }

    }

    public Sprite RoleGrade(float grade, float min, float max)
    {
        float Amin = 0;
        float Amax = 0;
        float Bmin = 0;
        float Bmax = 0;
        float Cmin = 0;
        float Cmax = 0;
        float Smin = 0;
        float Smax = 0;
        Cmin = min;
        Cmax = min + (max - min) / 10.0f * 3.0f;
        Bmin = Cmax + 0.1f;
        Bmax = Cmax + (max - min) / 10.0f * 3.0f;
        Amin = Bmax + 0.1f;
        Amax = Bmax + (max - min) / 10.0f * 3.0f;
        Smin = Amax + 0.1f;
        Smax = Amax + (max - min) / 10.0f * 1.0f;

        if (grade <= Cmax)
        {
            return getImage.GetSprite("Level_1");
        }
        else if (grade >= Bmin && grade <= Bmax)
        {
            return getImage.GetSprite("Level_2");
        }
        else if (grade >= Amin && grade <= Amax)
        {
            return getImage.GetSprite("Level_3");
        }
        else if (grade >= Smin)
        {
            return getImage.GetSprite("Level_4");
        }
        else
        {
            Debug.Log(grade);
            return null;
        }


    }
    public void ChickBack()
    {
        anim.Play("UIRoleMain_out");
        Image image = transform.Find("BG").GetComponent<Image>();
        image.DOFade(0f, 0.2f);

        Invoke("CloseThisPage", .2f);
    }

    public void CloseThisPage()
    {
        TTUIPage.ClosePage<UIRolePage>();
        TTUIPage.ShowPage<UICardHousePage>();
        UIEventManager.instance.SendEvent<bool>(UIEventDefineEnum.UpdateRolesEvent, true);
    }

    [System.Serializable]
    public class UIRoleEquip
    {
        public Image roleEquipImage;
        public Image roleEquipQuality;
        public Button roleEquipOptions;
    }
    [System.Serializable]
    public class UIRoleAttribute
    {
        public Text roleValue;
        public Text roleScore;
        public Image roleQuality;
        public Image roleQualityImage;
    }
    [System.Serializable]
    public class UIRoleSkill
    {
        public Image roleSkillBG;
        public Image roleSkillImage;
        public Text roleSkillLevel;
    }
}



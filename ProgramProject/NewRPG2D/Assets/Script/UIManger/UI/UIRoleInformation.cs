using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIRoleInformation : MonoBehaviour
{
    public Text roleName;
    public Text roleLevel;
    public Text roleExp;
    public Slider roleExpSlider;
    public Text roleHeart;
    public Image role;
    public Image roleQuality;
    public Image roleAttribute;
    public Image roleStars;

    public UIRoleEquip[] roleEquip;

    public UIRoleAttribute roleHealth;
    public UIRoleAttribute roleAttack;
    public UIRoleAttribute roleAgile;
    public UIRoleAttribute roleDefense;

    public UIRoleSkill role1Skill;
    public UIRoleSkill role2Skill;
    public Text roleSkillLevel;
    public Text roleSkillExp;
    public Slider roleSkillExpSlider;

    public CardData roleData;

    public int currentNumber;

    public GameObject buttonType;
    public Button roleEquipRemove;
    public Button roleEquipReplace;

    public void Awake()
    {
        for (int i = 0; i < roleEquip.Length; i++)
        {
            roleEquip[i].roleEquipOptions.onClick.AddListener(CheckCurrentEquipButton);
        }
        roleEquipReplace.onClick.AddListener(OpenEquipOpitions);
        roleEquipRemove.onClick.AddListener(RemoveThisEquip);
        UIEventManager.instance.AddListener<UIBagGrid>(UIEventDefineEnum.UpdateEquipsEvent, updateMessage);
        UIEventManager.instance.AddListener<RoleAtrType>(UIEventDefineEnum.UpdateLittleTipEvent, ShowLittleTip);
    }


    public void Start()
    {

    }

    public void Init()
    {
        buttonType.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != roleEquipRemove.gameObject
                 && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != roleEquipReplace.gameObject)
            {
                buttonType.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 判断该位置是否存有装备
    /// </summary>
    public void CheckCurrentEquipButton()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < roleEquip.Length; i++)
        {
            if (roleEquip[i].roleEquipOptions.gameObject == go)
            {
                currentNumber = i;
                Debug.Log(currentNumber + "变动了");
                if (roleData.Equipdata[i].EquipType != EquipType.Nothing)
                {
                    buttonType.transform.position = roleEquip[i].roleEquipImage.transform.position;
                    buttonType.SetActive(true);
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
        UIEventManager.instance.SendEvent<EquipType>(UIEventDefineEnum.UpdateEquipsEvent, (EquipType)currentNumber + 1);
    }

    /// <summary>
    /// 卸载当前使用的装备
    /// </summary>
    public void RemoveThisEquip()
    {
        BagEquipData.Instance.AddItem(roleData.Equipdata[currentNumber]);
        roleData.Equipdata[currentNumber] = new EquipData();
        roleData.Equipdata[currentNumber].EquipType = EquipType.Nothing;
        updateMessage(roleData);
        buttonType.SetActive(false);
    }

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<UIBagGrid>(UIEventDefineEnum.UpdateEquipsEvent, updateMessage);
        UIEventManager.instance.RemoveListener<RoleAtrType>(UIEventDefineEnum.UpdateExploreTipEvent, ShowLittleTip);
    }
    public void updateMessage(UIBagGrid data)
    {
        //获取当前角色在当前武器类型的位置是否有装备 如果有将原装备放回背包，将新的装备放到角色身上
        //如果当前角色装备栏上这件装备不是空的
        if (roleData.Equipdata[(int)data.equipData.EquipType - 1].EquipType != EquipType.Nothing)
        {
            EquipData equipData;
            //将原有装备放回背包序列
            equipData = roleData.Equipdata[(int)data.equipData.EquipType - 1];
            roleData.Equipdata[(int)data.equipData.EquipType - 1] = data.equipData;
            BagEquipData.Instance.Remove(data.equipData);
            BagEquipData.Instance.AddItem(equipData);
        }
        else
        {
            roleData.Equipdata[(int)data.equipData.EquipType - 1] = data.equipData;
            BagEquipData.Instance.Remove(data.equipData);
        }
        updateMessage(roleData);
    }
    ///更新卡牌信息
    public void updateMessage(CardData data)
    {
        roleData = data;
        roleName.text = data.Name;
        roleLevel.text = "LV." + data.Level.ToString();
        roleExp.text = data.Exp.ToString() + "/" + GameCardExpData.Instance.GetItem(data.Level).NeedExp;
        roleExpSlider.maxValue = GameCardExpData.Instance.GetItem(data.Level).NeedExp;
        roleExpSlider.value = data.Exp;
        roleHeart.text = data.GoodFeeling.ToString();
        role.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
        roleQuality.sprite = IconMgr.Instance.GetIcon("roleQuality_" + data.Quality);
        roleAttribute.sprite = IconMgr.Instance.GetIcon(data.Attribute);
        roleStars.sprite = IconMgr.Instance.GetIcon("Stars_" + data.Stars);
        for (int i = 0; i < roleEquip.Length; i++)
        {
            if (data.Equipdata[i] != null && data.Equipdata[i].EquipType != EquipType.Nothing)
            {
                roleEquip[(int)data.Equipdata[i].EquipType - 1].roleEquipImage.gameObject.SetActive(true);
                roleEquip[(int)data.Equipdata[i].EquipType - 1].roleEquipQuality.gameObject.SetActive(true);
                roleEquip[(int)data.Equipdata[i].EquipType - 1].roleEquipImage.sprite = IconMgr.Instance.GetIcon(data.Equipdata[i].SpriteName);
                roleEquip[(int)data.Equipdata[i].EquipType - 1].roleEquipQuality.sprite = IconMgr.Instance.GetIcon("quality_" + data.Equipdata[i].Quality);
            }
            else
            {
                roleEquip[i].roleEquipImage.gameObject.SetActive(false);
                roleEquip[i].roleEquipQuality.gameObject.SetActive(false);
            }
        }
        roleHealth.roleValue.text = data.Health.ToString();
        roleHealth.roleScore.text = data.HealthGrow.ToString("#0.0");
        string a = RoleGrade(data.HealthGrow, data.HealthMinGrow, data.HealthMaxGrow);
        roleHealth.roleQualityText.text = a;

        roleAttack.roleValue.text = data.Attack.ToString();
        roleAttack.roleScore.text = data.AttackGrow.ToString("#0.0");
        roleAttack.roleQualityText.text = RoleGrade(data.AttackGrow, data.AttackMinGrow, data.AttackMaxGrow);

        roleAgile.roleValue.text = data.Agile.ToString();
        roleAgile.roleScore.text = data.AgileGrow.ToString("#0.0");
        roleAgile.roleQualityText.text = RoleGrade(data.AgileGrow, data.AgileMinGrow, data.AgileMaxGrow);

        roleDefense.roleValue.text = data.Defense.ToString();
        roleDefense.roleScore.text = data.DefenseGrow.ToString("#0.0");
        roleDefense.roleQualityText.text = RoleGrade(data.DefenseGrow, data.DefenseMinGrow, data.DefenseMaxGrow);
    }

    private void ShowLittleTip(RoleAtrType type)
    {
        string message = "";
        switch (type)
        {
            case RoleAtrType.Nothing:
                break;
            case RoleAtrType.Health:
                message = roleData.HealthMinGrow.ToString("#0.0") + " ~ " + roleData.HealthMaxGrow.ToString("#0.0");
                break;
            case RoleAtrType.Attack:
                message = roleData.AttackMinGrow.ToString("#0.0") + " ~ " + roleData.AttackMaxGrow.ToString("#0.0");
                break;
            case RoleAtrType.Agile:
                message = roleData.AgileMinGrow.ToString("#0.0") + " ~ " + roleData.AgileMaxGrow.ToString("#0.0");
                break;
            case RoleAtrType.Defense:
                message = roleData.DefenseMinGrow.ToString("#0.0") + " ~ " + roleData.DefenseMaxGrow.ToString("#0.0");
                break;
            default:
                break;
        }
        TTUIPage.ShowPage<UILittleTipPage>();
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, message);
    }

    public string RoleGrade(float grade, float min, float max)
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

        if (grade >= Cmin && grade <= Cmax)
        {
            return "C";
        }
        else if (grade >= Bmin && grade <= Bmax)
        {
            return "B";
        }
        else if (grade >= Amin && grade <= Amax)
        {
            return "A";
        }
        else if (grade >= Smin && grade <= Smax)
        {
            return "S";
        }
        else
        {
            Debug.Log(grade);
            return null;
        }


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
        public Text roleQualityText;
    }
    [System.Serializable]
    public class UIRoleSkill
    {
        public Image roleSkillBG;
        public Image roleSkillImage;
        public Text roleSkillLevel;
    }
}



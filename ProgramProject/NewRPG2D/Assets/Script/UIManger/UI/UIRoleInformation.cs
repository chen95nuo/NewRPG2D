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

    public GameObject currentBtn;
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
        currentBtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < roleEquip.Length; i++)
        {
            if (roleEquip[i].roleEquipOptions.gameObject == currentBtn)
            {
                if (roleData.Equipdata[i].EquipType != EquipType.Nothing)
                {
                    buttonType.transform.position = roleEquip[i].roleEquipImage.transform.position;
                    buttonType.SetActive(true);
                }
                else
                {
                    OpenEquipOpitions();
                }
                currentNumber = i;
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
        roleLevel.text = data.Level.ToString();
        roleExp.text = data.Exp.ToString();
        roleHeart.text = data.GoodFeeling.ToString();
        role.sprite = Resources.Load<Sprite>("UITexture/Icon/role/" + data.Name);
        roleQuality.sprite = Resources.Load<Sprite>("UITexture/Icon/roleQuality/" + data.Quality);
        roleAttribute.sprite = Resources.Load<Sprite>("UITexture/Icon/attribute/" + data.Attribute);
        roleStars.sprite = Resources.Load<Sprite>("UITexture/Icon/stars/" + data.Stars);
        for (int i = 0; i < roleEquip.Length; i++)
        {
            if (data.Equipdata[i].EquipType != EquipType.Nothing)
            {
                roleEquip[(int)data.Equipdata[i].EquipType - 1].roleEquipImage.gameObject.SetActive(true);
                roleEquip[(int)data.Equipdata[i].EquipType - 1].roleEquipQuality.gameObject.SetActive(true);
                roleEquip[(int)data.Equipdata[i].EquipType - 1].roleEquipImage.sprite = Resources.Load<Sprite>("UITexture/Icon/equip/" + data.Equipdata[i].Id);
                roleEquip[(int)data.Equipdata[i].EquipType - 1].roleEquipQuality.sprite = Resources.Load<Sprite>("UITexture/Icon/quality/" + data.Equipdata[i].Quality);
            }
            else
            {
                roleEquip[i].roleEquipImage.gameObject.SetActive(false);
                roleEquip[i].roleEquipQuality.gameObject.SetActive(false);
            }
        }
        roleHealth.roleValue.text = data.Health.ToString();
        roleHealth.roleScore.text = data.HealthGrow.ToString();
        roleAttack.roleValue.text = data.Attack.ToString();
        roleAttack.roleScore.text = data.AttackGrow.ToString();
        roleAgile.roleValue.text = data.Agile.ToString();
        roleAgile.roleScore.text = data.AgileGrow.ToString();
        roleDefense.roleValue.text = data.Defense.ToString();
        roleDefense.roleScore.text = data.DefenseGrow.ToString();
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
    }
    [System.Serializable]
    public class UIRoleSkill
    {
        public Image roleSkillBG;
        public Image roleSkillImage;
        public Text roleSkillLevel;
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class UIRoleStrengthen : MonoBehaviour
{
    public SkeletonGraphic role;
    public Image roleQuality;
    public Image roleAttribute;
    public Image roleStars;
    public Text roleLevelText;
    public Text roleExpText;
    public Slider roleExpSlider;
    public Text addExpText;
    public Text levelUp;

    public Button confirmed;
    public Button backButton;

    public GameObject RoleMaterial;

    public CardData roleData;

    public CardData[] cardData = new CardData[4];
    public UIRoleStrengtheningMaterial[] roleMaterial;
    public UIRoleInformation roleInformation;

    public GameObject currentButton;
    public Text cardName;

    private bool isRun = false;
    private float currentAddExp = 0;

    private int currentLevel = 0;
    private float currentExp = 0;
    private float maxExp = 0;
    private float currentMaxExp = 0;
    private float currentValue = 0;
    private float startTime = 0;

    public SkeletonGraphic Anim_DaBai;
    public SkeletonGraphic Anim_MoWu;

    private void Awake()
    {
        UIEventManager.instance.AddListener<CardData>(UIEventDefineEnum.UpdateMaterialEvent, UpdateMaterial);

        cardData = new CardData[3];
        roleMaterial = new UIRoleStrengtheningMaterial[3];
        addExpText.text = "0";
        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(RoleMaterial, RoleMaterial.transform.parent.transform) as GameObject;
            go.SetActive(true);
            go.name = RoleMaterial.name + i;
            go.GetComponent<Button>().onClick.AddListener(ChoiceMaterials);
            roleMaterial[i] = go.GetComponent<UIRoleStrengtheningMaterial>();
            roleMaterial[i].parent = GetComponent<UIRoleStrengthen>();
        }
    }
    private void Start()
    {
        confirmed.onClick.AddListener(Confirmed);
        backButton.onClick.AddListener(CloseThisPage);
        RoleMaterial.SetActive(false);
    }
    public void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<CardData>(UIEventDefineEnum.UpdateMaterialEvent, UpdateMaterial);
    }
    public void CloseThisPage()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isRun)
        {
            backButton.interactable = false;

            currentValue = Mathf.Lerp(0, currentAddExp, (Time.time - startTime) * 0.5f);
            float index = (currentValue - currentMaxExp) + currentExp;

            if (index >= maxExp)
            {
                float _index = currentExp;
                currentExp = 0;
                index = 0;
                currentLevel++;
                currentMaxExp += (maxExp - _index);
                levelUp.gameObject.SetActive(true);
                levelUp.rectTransform.DOAnchorPos(Vector2.zero, 1.0f).From();

                maxExp = GameCardExpData.Instance.GetItem(currentLevel).NeedExp;
            }

            roleLevelText.text = "Lv." + currentLevel;
            roleExpSlider.value = index;
            roleExpSlider.maxValue = maxExp;
            addExpText.text = (currentAddExp - currentValue).ToString();
            roleExpText.text = index.ToString("#0") + " / " + maxExp;

            if (currentValue == currentAddExp)
            {
                currentMaxExp = 0;
                currentValue = 0;
                currentAddExp = 0;

                isRun = false;
                levelUp.gameObject.SetActive(false);
                backButton.interactable = true;
                roleInformation.UpdateMassage();
            }
        }
    }

    /// <summary>
    /// 更新主角色
    /// </summary>
    /// <param name="data"></param>
    public void UpdateRole(UIRoleInformation data)
    {
        levelUp.gameObject.SetActive(false);
        Debug.Log("更新主角色");

        roleData = data.RoleData;
        gameObject.SetActive(true);
        //role.sprite = data.role.sprite;
        Debug.Log("强化 ");
        if (data.RoleData.AnimationName == "Anim_Dabai")
        {
            Anim_MoWu.enabled = false;
            Anim_DaBai.enabled = true;
            //role.AnimationState.SetAnimation(0, "stand", true);
        }
        else if (data.RoleData.AnimationName == "Anim_Mowu")
        {
            Anim_DaBai.enabled = false;
            Anim_MoWu.enabled = true;
            //role.AnimationState.SetAnimation(0, "stand", true);
        }

        roleQuality.sprite = data.roleQuality.sprite;
        roleAttribute.sprite = data.roleAttribute.sprite;
        roleStars.sprite = data.roleStars.sprite;
        float maxExp = data.RoleData.maxExp;
        roleLevelText.text = "Lv." + data.RoleData.Level;
        roleExpText.text = data.RoleData.Exp + " / " + maxExp;
        roleExpSlider.maxValue = maxExp;
        roleExpSlider.value = data.RoleData.Exp;
        UpdateMaterial();
        cardName.text = data.RoleData.Name;
    }

    /// <summary>
    /// 初始化刷新材料列表
    /// </summary>
    public void UpdateMaterial()
    {
        confirmed.gameObject.SetActive(false);
        cardData = new CardData[4];
        for (int i = 0; i < roleMaterial.Length; i++)
        {
            roleMaterial[i].UpdateMaterial(cardData[i]);
        }
    }
    /// <summary>
    /// 添加当前材料并更新材料信息
    /// </summary>
    /// <param name="data"></param>
    public void UpdateMaterial(CardData data)
    {
        for (int i = 0; i < roleMaterial.Length; i++)
        {
            if (roleMaterial[i].gameObject == currentButton)
            {
                cardData[i] = data;
                roleMaterial[i].UpdateMaterial(data);
                confirmed.gameObject.SetActive(true);
            }


            if (cardData[i] != null) //计算经验
            {
                currentAddExp += cardData[i].UseAddExp;
                addExpText.text = currentAddExp.ToString();
            }
        }
    }
    /// <summary>
    /// 删除当前材料并更新材料信息
    /// </summary>
    /// <param name="data"></param>
    public void UpdateMaterial(CardData[] data)
    {
        int index = 0;
        for (int i = 0; i < roleMaterial.Length; i++)
        {
            roleMaterial[i].UpdateMaterial(data[i]);
            if (data[i] != null)
                index++;
        }
        if (index <= 0)
        {
            confirmed.gameObject.SetActive(false);
        }
        else
        {
            confirmed.gameObject.SetActive(true);
        }
    }

    public void ChoiceMaterials()
    {
        currentButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        cardData[3] = roleData; //主卡ID为3
        Debug.Log(cardData[3].Name);
        TinyTeam.UI.TTUIPage.ShowPage<UICardHouse>();

        UIEventManager.instance.SendEvent<GridType>(UIEventDefineEnum.UpdateRolesEvent, GridType.Use);
        UIEventManager.instance.SendEvent<CardData[]>(UIEventDefineEnum.UpdateRolesEvent, cardData);
    }

    public void Confirmed()
    {
        isRun = true;
        backButton.interactable = false;
        for (int i = 0; i < roleMaterial.Length; i++)
        {
            if (cardData[i] != null)
            {
                BagRoleData.Instance.Remove(cardData[i]);
            }
        }
        maxExp = roleData.maxExp;
        currentLevel = roleData.Level;
        currentExp = roleData.Exp;
        roleData.AddExp = currentAddExp;
        startTime = Time.time;

        UpdateMaterial();
    }

    public void RemoveMaterial(GameObject go)
    {
        for (int i = 0; i < roleMaterial.Length; i++)
        {
            Debug.Log(roleMaterial[i].gameObject == go);
            if (roleMaterial[i].gameObject == go)
            {
                Debug.Log(i);
                cardData[i] = null;
            }
        }
        UpdateMaterial(cardData);
    }



}

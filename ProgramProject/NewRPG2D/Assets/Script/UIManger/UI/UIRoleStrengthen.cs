using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleStrengthen : MonoBehaviour
{
    public Image role;
    public Image roleQuality;
    public Image roleAttribute;
    public Image roleStars;

    public Button confirmed;

    public GameObject RoleMaterial;

    public CardData roleData;

    public CardData[] cardData;
    public UIRoleStrengtheningMaterial[] roleMaterial;

    public GameObject currentButton;

    public UIRoleMaterialHouse materialHouse;

    private void Awake()
    {
        UIEventManager.instance.AddListener<CardData>(UIEventDefineEnum.UpdateMaterialEvent, UpdateMaterial);

        cardData = new CardData[3];
        roleMaterial = new UIRoleStrengtheningMaterial[3];
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
        confirmed = transform.Find("btn_confirmed").GetComponent<Button>();
        confirmed.onClick.AddListener(Confirmed);
        transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(CloseThisPage);
        RoleMaterial.SetActive(false);
    }
    public void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<CardData>(UIEventDefineEnum.UpdateMaterialEvent, UpdateMaterial);
    }
    public void CloseThisPage()
    {
        gameObject.SetActive(false);
        materialHouse.gameObject.SetActive(false);
    }

    public void UpdateRole(UIRoleInformation data)
    {
        roleData = data.roleData;
        gameObject.SetActive(true);
        role.sprite = data.role.sprite;
        roleQuality.sprite = data.roleQuality.sprite;
        roleAttribute.sprite = data.roleAttribute.sprite;
        roleStars.sprite = data.roleStars.sprite;
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        confirmed.gameObject.SetActive(false);
        cardData = new CardData[3];
        for (int i = 0; i < roleMaterial.Length; i++)
        {
            roleMaterial[i].UpdateMaterial(cardData[i]);
        }
    }
    public void UpdateMaterial(CardData data)
    {
        materialHouse.gameObject.SetActive(false);
        for (int i = 0; i < roleMaterial.Length; i++)
        {
            if (roleMaterial[i].gameObject == currentButton)
            {
                cardData[i] = data;
                roleMaterial[i].UpdateMaterial(data);
                confirmed.gameObject.SetActive(true);
            }
        }
    }
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
        materialHouse.gameObject.SetActive(true);
        materialHouse.UpdateBagItem(roleData, cardData);
    }

    public void Confirmed()
    {

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

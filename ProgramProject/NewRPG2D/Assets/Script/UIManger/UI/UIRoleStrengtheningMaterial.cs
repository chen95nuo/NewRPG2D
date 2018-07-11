using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleStrengtheningMaterial : MonoBehaviour
{
    public Image role;
    public Image roleQuality;
    public Image roleAttribute;
    public Image roleStars;
    public Button back;
    public GameObject roleDisplay;
    public GameObject none;
    public UIRoleStrengthen parent;

    public void Start()
    {
        back.onClick.AddListener(RemoveMaterial);
    }
    public void UpdateMaterial(CardData data)
    {
        if (data == null)
        {
            none.SetActive(true);
            roleDisplay.SetActive(false);
        }
        else
        {
            none.SetActive(false);
            roleDisplay.SetActive(true);
            role.sprite = Resources.Load<Sprite>("UITexture/Icon/role/" + data.Name);
            roleQuality.sprite = Resources.Load<Sprite>("UITexture/Icon/roleQuality/" + data.Quality);
            roleAttribute.sprite = Resources.Load<Sprite>("UITexture/Icon/attribute/" + data.Attribute);
            roleStars.sprite = Resources.Load<Sprite>("UITexture/Icon/stars/" + data.Stars);
        }
    }

    public void RemoveMaterial()
    {
        Debug.Log("返回");
        parent.RemoveMaterial(this.gameObject);
    }

}

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
            role.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
            roleQuality.sprite = IconMgr.Instance.GetIcon("roleQuality_" + data.Quality);
            roleAttribute.sprite = IconMgr.Instance.GetIcon( data.Attribute);
            roleStars.sprite = IconMgr.Instance.GetIcon("Start_" + data.Stars);
        }
    }

    public void RemoveMaterial()
    {
        Debug.Log("返回");
        parent.RemoveMaterial(this.gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFurnacePopUp : MonoBehaviour
{
    public FurnacePopUp[] furnacePops;

    private int iron = 0;
    private int wood = 0;
    private int leatherwear = 0;
    private int cloth = 0;
    private FurnaceData furnaceData;

    private void Awake()
    {
        Restart();
    }

    public void Restart()
    {
        for (int i = 0; i < furnacePops.Length; i++)
        {
            furnacePops[i].MType = ItemMaterialType.Nothing;
            furnacePops[i].PopUp.SetActive(false);
        }
    }

    public void UpdatePopUp(FurnaceData data)
    {
        if (furnaceData != null && furnaceData != data)
        {
            for (int i = 0; i < furnacePops.Length; i++)
            {
                furnacePops[i].MType = ItemMaterialType.Nothing;
                furnacePops[i].PopUp.SetActive(false);
            }
        }

        iron = wood = leatherwear = cloth = 0;
        if (data.PopPoint != null)
        {
            for (int i = 0; i < data.PopPoint.Length; i++)
            {
                furnacePops[data.PopPoint[i].materialPoint].PopUp.SetActive(true);
                furnacePops[data.PopPoint[i].materialPoint].FMaterialImage.sprite = Resources.Load<Sprite>("UITexture/Icon/furnaceMaterial/" + data.PopPoint[i].materialType);
                furnacePops[data.PopPoint[i].materialPoint].fMaterialNumber = data.PopPoint[i].materialNumber;
                furnacePops[data.PopPoint[i].materialPoint].FMaterialNumber.text = "+" + furnacePops[data.PopPoint[i].materialPoint].fMaterialNumber;
            }
            return;
        }
        else
        {
            for (int i = 0; i < data.Material.Length; i++)//炉子里的总材料数
            {
                if (data.Material[i] != null && data.Material[i].ItemType != ItemType.Nothing)
                {
                    for (int j = 0; j < data.Material[i].RawMaterial.Length; j++)//炉子里每个材料可转化的材料
                    {
                        MaterialType(data.Material[i].RawMaterial[j]);
                    }
                }
            }
        }
        furnaceData = data;
    }
    public void MaterialType(RawMaterial rawMaterial)
    {
        switch (rawMaterial.materialType)
        {
            case ItemMaterialType.Nothing:
                break;
            case ItemMaterialType.Iron:
                iron += rawMaterial.number;
                InstancePopUp(ItemMaterialType.Iron, iron);
                break;
            case ItemMaterialType.Wood:
                wood += rawMaterial.number;
                InstancePopUp(ItemMaterialType.Wood, wood);
                break;
            case ItemMaterialType.Leatherwear:
                leatherwear += rawMaterial.number;
                InstancePopUp(ItemMaterialType.Leatherwear, leatherwear);
                break;
            case ItemMaterialType.Cloth:
                cloth += rawMaterial.number;
                InstancePopUp(ItemMaterialType.Cloth, cloth);
                break;
            default:
                break;
        }
    }
    public void InstancePopUp(ItemMaterialType type, int number)
    {
        int index = 0;
        for (int i = 0; i < furnacePops.Length; i++)//查询是否已经有该类型的存在，若已有直接使用
        {
            if (furnacePops[i].MType == type)
            {
                furnacePops[i].fMaterialNumber = number;
                furnacePops[i].FMaterialNumber.text = "+" + furnacePops[i].fMaterialNumber;
            }
            else
                index++;
        }
        if (index >= furnacePops.Length)//没有该类型的存在创建一个
        {
            int roll = 0;
            do
            {
                roll = Random.Range(0, 6);
            } while (furnacePops[roll].MType != ItemMaterialType.Nothing);

            furnacePops[roll].MType = type;
            furnacePops[roll].FMaterialImage.sprite = Resources.Load<Sprite>("UITexture/Icon/furnaceMaterial/" + (int)type);
            furnacePops[roll].fMaterialNumber = number;
            furnacePops[roll].FMaterialNumber.text = "+" + furnacePops[roll].fMaterialNumber;
            furnacePops[roll].PopUp.SetActive(true);
        }
    }
}
[System.Serializable]
public class FurnacePopUp
{
    [SerializeField]
    public GameObject PopUp;
    [SerializeField]
    public Image pop;
    [SerializeField]
    public Image FMaterialImage;
    [SerializeField]
    public Text FMaterialNumber;
    public int fMaterialNumber;

    public ItemMaterialType MType;
}

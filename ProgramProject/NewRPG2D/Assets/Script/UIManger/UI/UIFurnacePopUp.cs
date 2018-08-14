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
    private int stone;//石头
    private int magic;//魔法
    private int diamonds;//钻石
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
        //初始化熔炉
        if (furnaceData != null && furnaceData != data)
        {
            for (int i = 0; i < furnacePops.Length; i++)
            {
                furnacePops[i].MType = ItemMaterialType.Nothing;
                furnacePops[i].PopUp.SetActive(false);
            }
        }
        //初始化参数
        iron = wood = leatherwear = stone = magic = diamonds = 0;

        //检查熔炉是否运行
        if (data.FurnaceType == FurnaceType.Run || data.FurnaceType == FurnaceType.End)
        {
            for (int i = 0; i < data.PopPoint.Length; i++)
            {
                if (data.PopPoint[i].materialType != ItemMaterialType.Nothing)
                {
                    furnacePops[data.PopPoint[i].materialPoint].PopUp.SetActive(true);
                    furnacePops[data.PopPoint[i].materialPoint].FMaterialImage.sprite = IconMgr.Instance.GetIcon("Material_" + data.PopPoint[i].materialType);
                    furnacePops[data.PopPoint[i].materialPoint].fMaterialNumber = data.PopPoint[i].materialNumber;
                    furnacePops[data.PopPoint[i].materialPoint].FMaterialNumber.text = "+" + furnacePops[data.PopPoint[i].materialPoint].fMaterialNumber;
                }
            }
            return;
        }
        //没有运行则动态添加材料
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
            case ItemMaterialType.Magic:
                magic += rawMaterial.number;
                InstancePopUp(ItemMaterialType.Magic, magic);
                break;
            case ItemMaterialType.Diamonds:
                diamonds += rawMaterial.number;
                InstancePopUp(ItemMaterialType.Diamonds, diamonds);
                break;
            case ItemMaterialType.Stone:
                stone += rawMaterial.number;
                InstancePopUp(ItemMaterialType.Stone, stone);
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
            furnacePops[roll].FMaterialImage.sprite = IconMgr.Instance.GetIcon("Material_" + type);
            furnacePops[roll].fMaterialNumber = number;
            furnacePops[roll].FMaterialNumber.text = "+" + furnacePops[roll].fMaterialNumber;
            furnacePops[roll].PopUp.SetActive(true);
        }
    }

    public void SavePopUpPoint(FurnaceData data)
    {
        for (int i = 0; i < data.PopPoint.Length; i++)
        {
            data.PopPoint[i] = new FurnacePopUpMaterial();
            data.PopPoint[i].materialNumber = furnacePops[i].fMaterialNumber;
            data.PopPoint[i].materialPoint = i;
            data.PopPoint[i].materialType = furnacePops[i].MType;
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

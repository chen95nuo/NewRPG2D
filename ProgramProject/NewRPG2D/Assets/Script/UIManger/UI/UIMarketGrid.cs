using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMarketGrid : MonoBehaviour
{

    public BuildingData buildingData;
    [System.NonSerialized]
    public UIMarket market;
    public Text buildName;

    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text txt_Tip_4;

    public Text txt_Gold;
    public Text txt_Mana;
    public Text txt_Wood;
    public Text txt_Iron;

    public Image RoomIcon_1;
    public Image RoomIcon_2;
    public Image RoomIcon_3;
    public Image RoomIcon_4;

    public Text txt_Number;
    public Text txt_NeedTime;
    private Button thisGrid;

    public GameObject lockObj;
    public Image[] lockImages;
    //public Material black;

    private void Awake()
    {
        thisGrid = GetComponent<Button>();
        thisGrid.onClick.AddListener(AddBuilding);
    }

    public void UpdateBuilding(BuildingData data, int[] number, bool isTrue)
    {
        ChickProduce(false);

        txt_Tip_4.text = data.Description;
        buildName.text = data.RoomName.ToString();
        thisGrid.interactable = isTrue;

        Sprite sp = GetSpriteAtlas.insatnce.GetIcon(data.RoomName.ToString());
        switch (data.RoomSize)
        {
            case 1:
                RoomIcon_1.gameObject.SetActive(true);
                RoomIcon_1.sprite = sp;
                break;
            case 3:
                RoomIcon_2.gameObject.SetActive(true);
                RoomIcon_2.sprite = sp;
                break;
            case 6:
                RoomIcon_3.gameObject.SetActive(true);
                RoomIcon_3.sprite = sp;
                break;
            case 9:
                RoomIcon_4.gameObject.SetActive(true);
                RoomIcon_4.sprite = sp;
                break;
            default:
                break;
        }

        if (isTrue == false)
        {
            if (data.NeedLevel == 0) txt_Tip_3.text = "建筑数量已达上限";
            else txt_Tip_3.text = string.Format("需要{0}级国王大厅", data.NeedLevel);
            lockObj.SetActive(true);
            //for (int i = 0; i < lockImages.Length; i++)
            //{
            //    lockImages[i].material = black;
            //}
            return;
        }

        txt_Tip_1.text = "建造时间";
        txt_Tip_2.text = "建筑数量";

        buildingData = data;
        txt_Number.text = number[0] + "/" + number[1];
        txt_NeedTime.text = SystemTime.instance.TimeNormalized(data.NeedTime);

        if (data.NeedGold > 0)
        {
            txt_Gold.gameObject.SetActive(true);
            txt_Gold.text = data.NeedGold.ToString();
        }
        if (data.NeedMana > 0)
        {
            txt_Mana.gameObject.SetActive(true);
            txt_Mana.text = data.NeedMana.ToString();
        }
        if (data.NeedWood > 0)
        {
            txt_Wood.gameObject.SetActive(true);
            txt_Wood.text = data.NeedWood.ToString();
        }
        if (data.NeedIron > 0)
        {
            txt_Iron.gameObject.SetActive(true);
            txt_Iron.text = data.NeedIron.ToString();
        }
    }

    private void AddBuilding()
    {
        Debug.Log(buildingData.RoomName);
        MainCastle.instance.BuildRoomTip(buildingData);
        market.ClosePage();
    }

    private void ChickProduce(bool isTrue)
    {
        txt_Gold.gameObject.SetActive(false);
        txt_Mana.gameObject.SetActive(false);
        txt_Wood.gameObject.SetActive(false);
        txt_Iron.gameObject.SetActive(false);

        RoomIcon_1.gameObject.SetActive(false);
        RoomIcon_2.gameObject.SetActive(false);
        RoomIcon_3.gameObject.SetActive(false);
        RoomIcon_4.gameObject.SetActive(false);

        txt_Tip_1.text = "";
        txt_Tip_2.text = "";
        txt_Tip_3.text = "";
        txt_NeedTime.text = "";
        txt_Number.text = "";

        lockObj.SetActive(false);

        //for (int i = 0; i < lockImages.Length; i++)
        //{
        //    lockImages[i].material = null;
        //}
    }
}

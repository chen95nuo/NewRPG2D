using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Assets.Script.Battle.BattleData;
using Assets.Script.Battle.Equipment;

public class UIBoxInfo : TTUIPage
{
    public Text txt_Name;
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;

    public Image Image_Icon;

    public Button btn_Close;
    public Button btn_Open;

    public GameObject WordBar;
    public List<UIBoxWordBar> barGrid = new List<UIBoxWordBar>();

    public GameObject FixedReward;//固定奖励
    public GameObject PossibleReward;//可能奖励
    public Transform GridPoint;
    public Transform poolPoint;

    public BoxDataHelper realBoxData;

    private void Awake()
    {
        btn_Close.onClick.AddListener(ClosePage);
        btn_Open.onClick.AddListener(OpenBox);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        BoxDataHelper boxdata = mData as BoxDataHelper;
        UpdateInfo(boxdata);
    }

    public void UpdateInfo(BoxDataHelper helperData)
    {
        realBoxData = helperData;
        TreasureBox boxData = TreasureBoxDataMgr.instance.GetXmlDataByItemId<TreasureBox>(helperData.boxId);
        txt_Name.text = boxData.ItemName;
        Sprite sp = GetSpriteAtlas.insatnce.GetIcon(boxData.Icon);
        Image_Icon.sprite = sp;

        int index = 0;
        for (int i = 0; i < boxData.TreasureBoxItems.Length; i++)
        {
            if (boxData.TreasureBoxItems[i].ItemId == 0)
            {
                break;
            }
            else
            {
                if (barGrid.Count <= index)
                {
                    InstanceGrid();
                }
                barGrid[index].transform.SetParent(GridPoint, false);
                barGrid[index].UpdateInfo(boxData.TreasureBoxItems[i]);
                index++;
            }
        }
        if (index > 0)
        {
            FixedReward.transform.SetParent(GridPoint, false);
            FixedReward.transform.SetAsFirstSibling();
        }
        else
        {
            FixedReward.transform.SetParent(poolPoint, false);
            FixedReward.transform.position = Vector3.zero;
        }
        int temp = index;
        for (int i = 0; i < boxData.RandomTreasureBoxItems.Length; i++)
        {
            if (boxData.RandomTreasureBoxItems[i].ItemData.ItemId == 0)
            {
                break;
            }
            else
            {
                if (barGrid.Count <= index)
                {
                    InstanceGrid();
                }
                barGrid[index].transform.SetParent(GridPoint, false);
                barGrid[index].UpdateInfo(boxData.TreasureBoxItems[i]);
                index++;
            }
        }
        if (index > temp)
        {
            PossibleReward.transform.SetParent(GridPoint, false);
            PossibleReward.transform.SetSiblingIndex(temp);
        }
        else
        {
            PossibleReward.transform.SetParent(poolPoint, false);
            PossibleReward.transform.position = Vector3.zero;
        }
        for (int i = index; i < barGrid.Count; i++)
        {
            barGrid[i].transform.parent = poolPoint;
        }
    }

    public void InstanceGrid()
    {
        GameObject go = Instantiate(WordBar, poolPoint) as GameObject;
        UIBoxWordBar data = go.GetComponent<UIBoxWordBar>();
        barGrid.Add(data);
    }

    public void OpenBox()
    {
        Debug.Log("开宝箱");
        int fight = HallRoleMgr.instance.GetAtrMaxRole(RoleAttribute.Fight);
        int life = HallRoleMgr.instance.GetAtrMaxRole(RoleAttribute.Max);
        ItemDataInTreasure data = TreasureBoxMgr.instance.OpenTreasureBox(realBoxData.boxId, fight, life);
        UIPanelManager.instance.ShowPage<UIBoxOpen>();
        UIBoxOpen.instance.ShowAnim(data.PropDataList, data.EquipmentList);
        ChickItemInfo.instance.UseBox(realBoxData.instanceId);
        ClosePage();
    }
}

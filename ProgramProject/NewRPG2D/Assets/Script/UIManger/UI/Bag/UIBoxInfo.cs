using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Assets.Script.Battle.BattleData;

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


    private void Awake()
    {
        btn_Close.onClick.AddListener(ClosePage);
        btn_Open.onClick.AddListener(OpenBox);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        //TreasureBox boxdata = mData as TreasureBox;
        TreasureBox boxdata = TreasureBoxDataMgr.instance.GetXmlDataByItemId<TreasureBox>(1);
        UpdateInfo(boxdata);
    }

    public void UpdateInfo(TreasureBox boxData)
    {
        txt_Name.text = boxData.ItemName;
        //Sprite sp = GetSpriteAtlas.insatnce.GetIcon(boxData.Icon);
        //Image_Icon.sprite = sp;

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
                PropData data = PropDataMgr.instance.GetXmlDataByItemId<PropData>(boxData.TreasureBoxItems[i].ItemId);
                string tip = "";
                if (boxData.TreasureBoxItems[i].ItemMinCount == boxData.TreasureBoxItems[i].ItemMaxCount)
                {
                    tip = data.quality.ToString() + " +" + boxData.TreasureBoxItems[i].ItemMinCount;
                }
                else
                {
                    tip = boxData.TreasureBoxItems[i].ItemMinCount.ToString() + "-" + boxData.TreasureBoxItems[i].ItemMaxCount.ToString();
                }
                barGrid[index].transform.SetParent(GridPoint, false);
                barGrid[index].UpdateInfo(data.SpriteName, tip);
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
                PropData data = PropDataMgr.instance.GetXmlDataByItemId<PropData>(boxData.TreasureBoxItems[i].ItemId);
                string tip = "";
                if (boxData.TreasureBoxItems[i].ItemMinCount == boxData.TreasureBoxItems[i].ItemMaxCount)
                {
                    tip = data.quality.ToString() + " +" + boxData.TreasureBoxItems[i].ItemMinCount;
                }
                else
                {
                    tip = boxData.TreasureBoxItems[i].ItemMinCount.ToString() + "-" + boxData.TreasureBoxItems[i].ItemMaxCount.ToString();
                }
                barGrid[index].transform.SetParent(GridPoint, false);
                barGrid[index].UpdateInfo(data.SpriteName, tip);
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
        //TreasureBox
    }

}

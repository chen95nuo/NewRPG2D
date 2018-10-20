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

    public void UpdateInfo(TreasureBox boxData)
    {
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
                barGrid[index].transform.parent = GridPoint;
                barGrid[index].UpdateInfo(data.SpriteName, tip);
            }
            index++;
        }
        if (index > 0)
        {
            FixedReward.transform.parent = GridPoint;
            FixedReward.transform.SetAsFirstSibling();
        }
        else
        {
            FixedReward.transform.parent = poolPoint;
            FixedReward.transform.position = Vector3.zero;
        }
        int temp = index;
        for (int i = 0; i < boxData.RandomTreasureBoxItems.Length; i++)
        {
            index++;
            if (boxData.RandomTreasureBoxItems[i].ItemData.ItemId == 0)
            {
                break;
            }
        }
        if (index > temp)
        {
            PossibleReward.transform.parent = GridPoint;
            PossibleReward.transform.SetSiblingIndex(temp);
        }
        else
        {
            PossibleReward.transform.parent = poolPoint;
            PossibleReward.transform.position = Vector3.zero;
        }
        for (int i = 0; i < index; i++)
        {

        }

    }

    public void OpenBox()
    {

    }

}

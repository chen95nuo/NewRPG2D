using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDropBagData
{
    private static GameDropBagData instance;
    public static GameDropBagData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.DropBagData];
                instance = JsonUtility.FromJson<GameDropBagData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<DropBagData>();
                }

            }
            return instance;
        }
    }

    public List<DropBagData> items;//库中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DropBagData GetItem(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {

                return items[i];
            }
        }
        return null;
    }

    public CardGainData[] GetCards(CardData[] datas, float addExp, float playerExp)
    {
        CardGainData[] cardGainData = new CardGainData[datas.Length];
        for (int i = 0; i < datas.Length; i++)
        {
            cardGainData[i] = new CardGainData(
                datas[i].Quality,
                datas[i].SpriteName,
                datas[i].Level,
                datas[i].Exp,
                datas[i].maxExp,
                addExp,
                GetPlayData.Instance.player[0].Level,
                GetPlayData.Instance.player[0].Exp,
                playerExp);
            datas[i].AddExp = addExp;
        }
        GetPlayData.Instance.player[0].AddExp = playerExp;
        return cardGainData;
    }

    /// <summary>
    /// 将道具添加到背包并返回奖励信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GainData[] GetGains(int id)
    {
        ExploreData currentExplore = GameExploreData.Instance.GetItem(1);
        DropBagData dropData = GameDropBagData.Instance.GetItem(currentExplore.DroppingBoxId);
        int roll_1 = UnityEngine.Random.Range(0, dropData.MaxDrop + 1); //随机多少个道具
        int index = 0;
        List<GainData> gainData = new List<GainData>();
        for (int i = 0; i < dropData.Items.Length; i++)
        {
            int roll = UnityEngine.Random.Range(0, 100);
            if (roll <= dropData.Items[i].dropOdds)
            {
                index++;
                GainData indexData = new GainData(dropData.Items[i].itemID, dropData.Items[i].itemType, UnityEngine.Random.Range(1, dropData.Items[i].maxNumber), dropData.AddGold);
                //直接添加道具
                switch (indexData.itemtype)
                {
                    case ItemType.Nothing:
                        break;
                    case ItemType.Egg:
                        BagEggData.Instance.AddItems(GameEggData.Instance.GetItem(indexData.itemId)
                            , indexData.itemNumber);
                        break;
                    case ItemType.Prop:
                        BagItemData.Instance.AddItems(GamePropData.Instance.GetItem(indexData.itemId)
                            , indexData.itemNumber);
                        break;
                    case ItemType.Equip:
                        BagEquipData.Instance.AddItem(indexData.itemId);
                        break;
                    case ItemType.Role:
                        break;
                    default:
                        break;
                }
                gainData.Add(indexData);
            }
            if (index >= dropData.MaxDrop)
            {
                break;
            }
        }
        if (gainData.Count <= 0)
        {
            gainData.Add(new GainData(0, 0, 0, dropData.AddGold));
        }
        GainData[] newData = new GainData[gainData.Count];
        for (int i = 0; i < gainData.Count; i++)
        {
            newData[i] = gainData[i];
        }
        GetPlayData.Instance.player[0].GoldCoin += dropData.AddGold;
        return newData;
    }

}

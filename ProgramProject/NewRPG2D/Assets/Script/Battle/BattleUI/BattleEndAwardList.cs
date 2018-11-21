using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Battle.BattleData;
using Assets.Script.Utility.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleEndAwardList : MonoBehaviour
    {
        public BattleEndAwardItem[] items;

        public void SetAwardInfo(AwardItemData[] itemDatas, int[] treasureBoxIds, bool isWin)
        {
            int itemCount = itemDatas.Length;
            int realCount = 0;
            for (int i = 0; i < items.Length; i++)
            {
                items[i].gameObject.CustomSetActive(false);
            }
            if (isWin == false) return;
            for (int i = 0; i < itemCount; i++)
            {
                int count = UnityEngine.Random.Range(itemDatas[i].ItemMinCount, itemDatas[i].ItemMaxCount);
                RealPropData mRealPropData = ChickItemInfo.instance.CreateNewProp(itemDatas[i].ItemId, count);
                if (mRealPropData != null && mRealPropData.propData != null)
                {
                    items[i].gameObject.CustomSetActive(true);
                    items[i].SetBoxInfo(GetSpriteAtlas.insatnce.GetIcon(mRealPropData.propData.SpriteName), count);
                }
            }
            for (int i = itemCount; i < treasureBoxIds.Length + itemCount; i++)
            {
                TreasureBox box = TreasureBoxDataMgr.instance.GetXmlDataByItemId<TreasureBox>(treasureBoxIds[i - itemCount]);
                if (box != null)
                {
                    realCount++;
                    ChickItemInfo.instance.CreateNewBox(box.ItemId);
                    items[i].gameObject.CustomSetActive(true);
                    items[i].SetBoxInfo(GetSpriteAtlas.insatnce.GetIcon(box.SpriteName), 1);
                }
            }

        }
    }
}

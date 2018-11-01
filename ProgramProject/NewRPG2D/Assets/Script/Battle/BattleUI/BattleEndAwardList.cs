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

        public void SetAwardInfo(AwardItemData[] itemDatas, int[] treasureBoxIds)
        {
            int itemCount = itemDatas.Length;
            int realCount = 0;
            for (int i = 0; i < itemCount; i++)
            {
                RealPropData mRealPropData = ChickItemInfo.instance.CreateNewProp(itemDatas[i].ItemId);
                if (mRealPropData != null && mRealPropData.propData != null)
                {
                    realCount++;
                    items[i].SetBoxInfo(GetSpriteAtlas.insatnce.GetIcon(mRealPropData.propData.SpriteName), UnityEngine.Random.Range(itemDatas[i].ItemMinCount, itemDatas[i].ItemMaxCount));
                }
            }
            for (int i = itemCount; i < treasureBoxIds.Length; i++)
            {
                TreasureBox box = TreasureBoxDataMgr.instance.GetXmlDataByItemId<TreasureBox>(treasureBoxIds[i]);
                if (box != null)
                {
                    realCount++;
                    items[i].SetBoxInfo(GetSpriteAtlas.insatnce.GetIcon(box.Icon), 1);
                }
            }
            for (int i = realCount; i < items.Length; i++)
            {
                items[i].gameObject.CustomSetActive(false);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{

    public struct TreasureBoxItemData
    {
        public int ItemId;
        public int ItemCount;
    }

    public struct RandomTreasureBoxItemData
    {
        public TreasureBoxItemData ItemData;
        public float CreateChange;
    }

    public class TreasureBox : ItemBaseData
    {
        public bool DependLevel;
        public string Icon;
        public TreasureBoxItemData[] TreasureBoxItems = new TreasureBoxItemData[5];
        public RandomTreasureBoxItemData[] RandomTreasureBoxItems = new RandomTreasureBoxItemData[3];


        public override XmlName ItemXmlName
        {
            get { return XmlName.TreasureBox; }
        }

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            ItemName = ReadXmlDataMgr.StrParse(node, "Name");
            Description = ReadXmlDataMgr.StrParse(node, "Description");
            Icon = ReadXmlDataMgr.StrParse(node, "Icon");
            DependLevel = ReadXmlDataMgr.IntParse(node, "DependLevel") == 0;
            for (int i = 0; i < TreasureBoxItems.Length; i++)
            {
                GetItemData(node, i + 1, out TreasureBoxItems[i]);
            }

            for (int i = 0; i < RandomTreasureBoxItems.Length; i++)
            {
                int index = i + 1;
                RandomTreasureBoxItems[i].ItemData.ItemId = ReadXmlDataMgr.IntParse(node, "RandomItemId0" + index);
                RandomTreasureBoxItems[i].ItemData.ItemCount = ReadXmlDataMgr.IntParse(node, "RandomItemCount0" + index);
                RandomTreasureBoxItems[i].CreateChange = ReadXmlDataMgr.FloatParse(node, "RandomItemChange0" + index);
            }

            return base.GetXmlDataAttribute(node);
        }

        private void GetItemData(XmlNode node, int index, out TreasureBoxItemData data)
        {
            data.ItemId = ReadXmlDataMgr.IntParse(node, "ItemId0" + index);
            data.ItemCount = ReadXmlDataMgr.IntParse(node, "ItemCount0" + index);
        }
    }
}

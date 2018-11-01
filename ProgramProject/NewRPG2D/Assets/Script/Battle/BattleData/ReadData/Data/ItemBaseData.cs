
using System.Xml;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class ItemBaseData : XmlData
    {
        public int ItemId;
        public string ItemName;
        public string Description;

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            ItemId = ReadXmlDataMgr.IntParse(node, "ID");
            return base.GetXmlDataAttribute(node);
        }

        public override string ToString()
        {
            return "Item_" + ItemName;
        }
    }
}

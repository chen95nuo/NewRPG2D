using Assets.Script.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Assets.Script.Battle.BattleData
{
    public class MapSceneLevelData : ItemBaseData
    {

        public string Name;
        public int Point1RoleId;
        public int Point2RoleId;
        public int Point3RoleId;
        public int Point4RoleId;
        public int Point5RoleId;
        public int CreateEnemy01;
        public int CreateEnemy02;
        public int CreateEnemy03;
        public List<int> VictoryReward;
        public List<int> FailureReward;


        public override XmlName ItemXmlName
        {
            get { return XmlName.MapSceneLevel; }
        }

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            Name = ReadXmlDataMgr.StrParse(node, "Name");
            ReadXmlDataMgr.StrParse(node, "Description");
            CreateEnemy01 = ReadXmlDataMgr.IntParse(node, "CreateEnemy01");
            CreateEnemy02 = ReadXmlDataMgr.IntParse(node, "CreateEnemy02");
            CreateEnemy03 = ReadXmlDataMgr.IntParse(node, "CreateEnemy03");
            return base.GetXmlDataAttribute(node);
        }

        private List<int> ReadItemList(string itemList)
        {
            List<int> itemInts = new List<int>();
            string[] itemStrings = itemList.Split('|');
            for (int i = 0; i < itemStrings.Length; i++)
            {
                int rewardItemId = 0;
                if (Int32.TryParse(itemStrings[i], out rewardItemId))
                {
                    itemInts.Add(rewardItemId);
                }
            }
            return itemInts;
        }
    }
}

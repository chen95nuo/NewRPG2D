using Assets.Script.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Assets.Script.Tools;

namespace Assets.Script.Battle.BattleData
{

    public struct AwardItemData
    {
        public int ItemId;
        public int ItemMinCount, ItemMaxCount;
    }

    public class CreateEnemyData : ItemBaseData
    {

        public string Name;
        public int[] CreateEnemyIds = new int[9];
        public AwardItemData[] AwardItem = new AwardItemData[3];
        public int[] TreasureBoxIds = new int[2];
        public int ChapterID;
        public int Lesson;
        public int NextLessonID;
        public int NeedNum;

        public override XmlName ItemXmlName
        {
            get { return XmlName.CreateEnemyData; }
        }

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            Name = ReadXmlDataMgr.StrParse(node, "Name");
            ReadXmlDataMgr.StrParse(node, "Description");
            for (int i = 0; i < CreateEnemyIds.Length; i++)
            {
                CreateEnemyIds[i] = ReadXmlDataMgr.IntParse(node, string.Format("EnemyPoint{0}RoleId", i + 1));
            }

            for (int i = 0; i < AwardItem.Length; i++)
            {
                AwardItem[i].ItemId = ReadXmlDataMgr.IntParse(node, string.Format("ItemId{0}", (i + 1)));
                StringHelper.instance.GetRange(ReadXmlDataMgr.StrParse(node, "ItemCount" + (i + 1)), out AwardItem[i].ItemMinCount, out AwardItem[i].ItemMaxCount);
            }

            for (int i = 0; i < TreasureBoxIds.Length; i++)
            {
                TreasureBoxIds[i] = ReadXmlDataMgr.IntParse(node, string.Format("TreasureBoxId{0}", i + 1));
            }
            ChapterID = ReadXmlDataMgr.IntParse(node, "Chapter");
            Lesson = ReadXmlDataMgr.IntParse(node, "Lesson");
            NextLessonID = ReadXmlDataMgr.IntParse(node, "NextLessonID");
            NeedNum = ReadXmlDataMgr.IntParse(node, "NeedNum");
            return base.GetXmlDataAttribute(node);
        }
    }
}


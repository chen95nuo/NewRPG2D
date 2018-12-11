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

    public class MapLevelData : ItemBaseCsvData
    {

        public string Name;
        public int[] CreateEnemyIds = new int[9];
        public AwardItemData[] AwardItem = new AwardItemData[3];
        public int[] TreasureBoxIds = new int[2];
        public int ChapterID;
        public int Lesson;
        public int NextLessonID;
        public int NeedNum;

        public override CsvEChartsType ItemCsvName
        {
            get { return CsvEChartsType.CreateEnemyData; }
        }

        public override bool AnalySis(string[] data)
        {
            if (base.AnalySis(data))
            {
                int index = 0;
                Name = StrParse(data, ref index);
                StrParse(data, ref index);
                for (int i = 0; i < CreateEnemyIds.Length; i++)
                {
                    CreateEnemyIds[i] = IntParse(data, ref index);
                }

                for (int i = 0; i < AwardItem.Length; i++)
                {
                    AwardItem[i].ItemId = IntParse(data, ref index);
                    List<float> itemCount = ListParse(data, ref index);
                    if (itemCount != null)
                    {
                        if (itemCount.Count > 0) AwardItem[i].ItemMinCount = (int)itemCount[0];
                        if (itemCount.Count > 1) AwardItem[i].ItemMaxCount = (int)itemCount[1];
                    }
                }

                for (int i = 0; i < TreasureBoxIds.Length; i++)
                {
                    TreasureBoxIds[i] = IntParse(data, ref index);
                }
                ChapterID = IntParse(data, ref index);
                Lesson = IntParse(data, ref index);
                NextLessonID = IntParse(data, ref index);
                NeedNum = IntParse(data, ref index);
                return true;
            }
            return true;
        }
    }
}


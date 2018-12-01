
using System.Collections.Generic;
using System.Xml;
using Assets.Script.Utility;

namespace Assets.Script.Battle.BattleData
{
    public class ItemBaseCsvData : CSVAnalysis
    {
        public int ItemId;
        public string ItemName = "Item";
        public string Description;

        public override bool AnalySis(string[] data)
        {
            if (data.Length == 0)
            {
                return false;
            }

            ItemId = IntParse(data, 0);
            return true;
        }

        public override string ToString()
        {
            return ItemName;
        }

        public static float[] FloatArray(List<float> parse)
        {
            if (parse == null)
            {
                return null;
            }
            float[] f = new float[parse.Count];
            for (int i = 0; i < parse.Count; i++)
            {
                f[i] = parse[i];
                if (f[i] == 0)
                {
                    return null;
                }
            }
            return f;
        }
    }
}


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

        public static float[] FloatArray(string a)
        {
            string[] astr = a.Split(',');
            float[] f = new float[astr.Length];
            for (int i = 0; i < astr.Length; i++)
            {
                f[i] = float.Parse(astr[i]);
                if (f[i] == 0)
                {
                    return null;
                }
            }
            return f;
        }
    }
}

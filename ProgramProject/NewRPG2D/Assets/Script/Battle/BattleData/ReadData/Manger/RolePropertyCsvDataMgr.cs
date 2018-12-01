using Assets.Script.Utility;
using System;
using System.Collections.Generic;

namespace Assets.Script.Battle.BattleData.ReadData
{
    public class RolePropertyCsvDataMgr : ItemCsvDataBaseMgr<RolePropertyCsvDataMgr>
    {
        protected override CsvEChartsType CurrentCsvName
        {
            get { return CsvEChartsType.RolePropertyData; }
        }
    }
}

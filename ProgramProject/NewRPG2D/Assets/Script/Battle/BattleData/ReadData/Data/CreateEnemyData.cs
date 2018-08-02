using Assets.Script.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Assets.Script.Battle.BattleData
{
    public class CreateEnemyData : ItemBaseData
    {

        public string Name;
        public List<CreateEnemyInfo> CreateEnemyInfoList;


        public override XmlName ItemXmlName
        {
            get { return XmlName.MapSceneLevel; }
        }

        public override bool GetXmlDataAttribute(XmlNode node)
        {
            Name = ReadXmlDataMgr.StrParse(node, "Name");
            ReadXmlDataMgr.StrParse(node, "Description");
            CreateEnemyInfo info = GetCreateEnemyInfo(node, 1);
            CreateEnemyInfoList.Add(info);
            info = GetCreateEnemyInfo(node, 2);
            CreateEnemyInfoList.Add(info);
            info = GetCreateEnemyInfo(node, 3);
            CreateEnemyInfoList.Add(info);
            info = GetCreateEnemyInfo(node, 4);
            CreateEnemyInfoList.Add(info);
            info = GetCreateEnemyInfo(node, 5);
            CreateEnemyInfoList.Add(info);

            return base.GetXmlDataAttribute(node);
        }

        private CreateEnemyInfo GetCreateEnemyInfo(XmlNode node, int Index)
        {
            CreateEnemyInfo info = new CreateEnemyInfo();
            info.EnemyPointRoleId = ReadXmlDataMgr.IntParse(node, string.Format("EnemyPoint{0}RoleId", Index));
            info.EnemyCount = ReadXmlDataMgr.IntParse(node, string.Format("EnmeyCount0{0}", Index));
            info.FirstEnemyDelayTime = ReadXmlDataMgr.FloatParse(node, string.Format("FirstEnemyDelayTime0{0}", Index));
            info.IntervalTime = ReadXmlDataMgr.FloatParse(node, string.Format("IntervalTime0{0}", Index));
            return info;
        }
    }
}


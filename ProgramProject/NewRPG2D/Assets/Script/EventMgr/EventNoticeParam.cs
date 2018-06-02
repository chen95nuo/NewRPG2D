
//功能：
//创建者: 胡海辉
//创建时间：


using System;
using UnityEngine;

namespace Assets.Script.EventMgr
{
    public class EventNoticeParam
    {
    }

    [Serializable]
    public class SceneBgm 
    {
      public int audioId;
    }
    /// <summary>
    /// 触发器
    /// </summary>
    public struct TiggerTypeParam
    {
        public TiggerType mType;
        public GameObject obj;
    }


}

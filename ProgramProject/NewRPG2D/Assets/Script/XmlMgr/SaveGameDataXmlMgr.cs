
//功能： 存储游戏数据
//创建者: 胡海辉
//创建时间：


using Assets.Script.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script
{
    public class SaveGameDataXmlMgr : TSingleton<SaveGameDataXmlMgr>,IDisposable
    {

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}

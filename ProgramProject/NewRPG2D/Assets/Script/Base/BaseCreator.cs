
//功能：
//创建者: 胡海辉
//创建时间：


using Assets.Script.Base;
using UnityEngine;

namespace Assets.Script.BallMgr
{
    public abstract class BaseCreator : BaseMonoBehaviour
    {
         [HideInInspector]
        public BaseCreator mCreator;

        public virtual void SetBaseCreator(BaseCreator creator) 
        {
            mCreator = creator;
        }
        public abstract void LogicCollision(BaseCreator creator);
    }
}

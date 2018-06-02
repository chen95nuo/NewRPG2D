
//功能：
//创建者: 胡海辉
//创建时间：


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Base
{
    public class BaseCommpoent
    {
        public BaseMonoBehaviour mMonoCreator;
        public virtual void Init()
        {
            InitComponent();
            InitData();
            InitListener();
        }

        public virtual void InitComponent()
        {
        }

        public virtual void InitData()
        {

        }

        /// <summary>
        /// 初始化监听
        /// </summary>
        public virtual void InitListener()
        {
        }

        public virtual void SetMonoCreator(BaseMonoBehaviour mBaseMonoBehaviour)
        {
            mMonoCreator = mBaseMonoBehaviour;
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public virtual void RemoveListener()
        {
        }

        public virtual void Dispose()
        {
            RemoveListener();
        }

        public virtual void Update(float time) { }
    }
}

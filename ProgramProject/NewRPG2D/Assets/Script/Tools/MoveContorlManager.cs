
//功能： 移动控制
//创建者: 胡海辉
//创建时间：


using UnityEngine;
using System.Collections;
using System;
using Assets.Script.Base;

namespace Assets.Script
{
    public class MoveContorlManager : TSingleton<MoveContorlManager>,IDisposable
    {

        public void Move(Transform trans, float speed)
        {
            trans.position += Vector3.right * speed;
        }

        public void Move(Transform trans, Vector3 dir,float speed)
        {
            trans.position += dir * speed;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}

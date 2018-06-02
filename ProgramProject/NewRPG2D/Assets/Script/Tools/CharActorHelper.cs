
//功能：
//创建者: 胡海辉
//创建时间：


using Assets.Script.Base;
using Assets.Script.EventMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Tools
{
    public class CharActorHelper : TSingleton<CharActorHelper>, IDisposable
    {

        public Camera mainCamera;
        public delegate void DoSomeHandle(EventNoticeParam param);

        public override void Init()
        {
            base.Init();
            mainCamera = Camera.main;
            if (mainCamera == null) { DebugHelper.DebugLogError(" MainCamera is Null"); }
        }


        /// <summary>
        /// 屏幕坐标和世界坐标转化
        /// </summary>
        public Vector3 GetScreenPos(Vector3 pos)
        {
            Vector3 screenPos = Vector3.zero;
            screenPos = mainCamera.WorldToScreenPoint(pos);
            return screenPos;
        }

        /// <summary>
        /// 判断是否在屏幕内
        /// </summary>
        private bool InTheArea(Vector3 worldPos, Vector3 wallWolrdPos)
        {
            Vector2 screenPos = GetScreenPos(worldPos);
            Vector2 wallPos = GetScreenPos(wallWolrdPos);
            if (Mathf.Abs(screenPos.x - wallPos.x) < Screen.width * 0.5f)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 计算两者距离
        /// </summary>
        /// <param name="target1Pos"></param>
        /// <param name="target2Pos"></param>
        /// <returns></returns>
        public float Distance(Vector3 target1Pos, Vector3 target2Pos)
        {
            float dis = 0;
            dis = Vector3.Distance(target1Pos, target2Pos);
            return dis;
        }

        public float Distance(Vector3 target1Pos, float target1Range, Vector3 target2Pos, float target2Range)
        {
            float dis = 0;
            dis = Distance(target1Pos, target2Pos);
            dis = dis - (target1Range + target2Range);

            return dis;
        }

        /// <summary>
        /// 设置物体的显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="bAtive"></param>
        /// <returns></returns>
        public bool SetGameObjActive<T>(T go, bool bAtive) where T : Component
        {
            if (go == null)
            {
                DebugHelper.DebugLogError("T  gameobject is null");
                return false;
            }
            else
            {
                go.gameObject.CustomSetActive(bAtive);
                return true;
            }
        }

        /// <summary>
        /// 获取物体显示的状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool GetGameObjectActive<T>(T go, bool bActive) where T : Component
        {
            if (go == null)
            {
                DebugHelper.DebugLogError("GetGameObjectActive T is null");
                return false;
            }
            else
            {
                return go.gameObject.activeSelf == bActive;
            }
        }


        public string GetPrefabPath(string name, string path = "")
        {
            //string mPath = "";
            if (string.IsNullOrEmpty(path))
                return  string.Format("Prefab/{0}", name);
            else
                return string.Format("Prefab/{0}/{1}", path, name);
        }

        /// <summary>
        /// 延迟做
        /// </summary>
        /// <param name="DelayTime"> <=0 则延迟一帧 </param>
        /// <param name="mDoSomeHandle"></param>
        /// <returns></returns>
        public IEnumerator DelayHandle(float DelayTime, DoSomeHandle mDoSomeHandle, EventNoticeParam param = null)
        {
            if (DelayTime <= 0)
            {
                yield return new  WaitForEndOfFrame();
            }
            else
            {
                yield return new WaitForSeconds(DelayTime);
            }
            mDoSomeHandle(param);
        }

        public override void Dispose()
        {
            base.Dispose();
        }


    }
}

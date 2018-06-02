
//功能：动态加载预制件
//创建者: 胡海辉
//创建时间：


using Assets.Script.Base;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class DynamicPrefabMgr : TSingleton<DynamicPrefabMgr>,IDisposable
    {

        public GameObject GetDynamicPrefabObj(string pathStr, bool bActive = false)
        {
            Transform DynamicPrefabRoot = GameObject.Find("DynamicPrefab").transform;
            Transform trans = GetPrefab<Transform>(pathStr, DynamicPrefabRoot);
            if (trans)
            {
                trans.gameObject.SetActive(bActive);
                return trans.gameObject;
            }
            return null;
        }

        public T GetDynamicPrefab<T>(string pathStr)
        {
            Transform DynamicPrefabRoot = GameObject.Find("DynamicPrefab").transform;
            return GetPrefab<T>(pathStr, DynamicPrefabRoot);
        }

        public T GetPrefab<T>(string pathStr, Transform trans)
        {
            GameObject goPrefab = Resources.Load(pathStr) as GameObject;
            Transform transChild = trans.Find(goPrefab.name);
            if (transChild == null)
            {
                GameObject go = MonoBehaviour.Instantiate<GameObject>(goPrefab);
                transChild = go.transform;
                transChild.parent = trans;
                //transChild.localPosition = Vector3.zero;
                transChild.name = goPrefab.name;
            }
            T fuc = transChild.GetComponent<T>();

            if (fuc == null)
            {
                Debug.LogError("don't find prefab at " + pathStr);
            }
            return fuc;
        }

        public T GetPrefab<T>(string path) 
        {
            GameObject goPrefab = Resources.Load(path) as GameObject;
            T fuc = goPrefab.GetComponent<T>();
            
            if (fuc == null)
            {
                DebugHelper.DebugLogErrorFormat("{0} is not find", path);
            }
            return fuc;
        }

        public T InstantiatePrefab<T>(GameObject obj, Transform parentTrans, Vector3 initPos) 
        {
            GameObject go = MonoBehaviour.Instantiate<GameObject>(obj);
            if (go == null) return default(T);
            go.transform.parent = parentTrans;
            go.transform.localPosition = initPos;
            return go.GetComponent<T>();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}

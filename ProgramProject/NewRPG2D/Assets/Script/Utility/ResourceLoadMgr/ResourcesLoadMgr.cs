using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Utility
{
    public class ResourcesLoadMgr : TSingleton <ResourcesLoadMgr>
    {
        private Dictionary<string, List<GameObject>> GameObjectPoolDic;
        private Transform poolTransform;
        public override void Init()
        {
            base.Init();
            GameObjectPoolDic = new Dictionary<string, List<GameObject>>(10);
            InitPool();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Dispose()
        {
            base.Dispose();
            using (var prefab = GameObjectPoolDic.GetEnumerator())
            {
                if (prefab.MoveNext())
                {
                    DestoryByPrefabName(prefab.Current.Key);
                }
            }
        }

        private void InitPool()
        {
            GameObject poolObj = GameObject.Find("GameObjectPool");
            if (poolObj  == null)
            {
                poolObj = new GameObject("GameObjectPool");
                UnityEngine.Object.DontDestroyOnLoad(poolObj);
            }
            poolTransform = poolObj.transform;
            poolTransform.gameObject.CustomSetActive(false);
        }


        public T LoadResource<T>(string path) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path)) return null;

            return Resources.Load<T>(path);
        }

        public void DestoryByPrefabName(string name)
        {
            List<GameObject> objList;
            if (GameObjectPoolDic.TryGetValue(name, out objList))
            {
                for (int i = 0; i < objList.Count; i++)
                {
                    MonoBehaviour.Destroy(objList[i]);
                }
            }
        }

        public void PushObjIntoPool(string indexName, GameObject obj)
        {
            obj.transform.SetParent(poolTransform, false);
            List<GameObject> objList;
            if (GameObjectPoolDic.TryGetValue(indexName, out objList))
            {
                objList.Add(obj);
            }
            else
            {
                objList = new List<GameObject>(10) { obj };
                GameObjectPoolDic.Add(indexName, objList);
            }
        }

        public GameObject PopObjFromPool(string mPrefabName, string indexName)
        {
            GameObject obj = null;
            List<GameObject> objList;
            if (GameObjectPoolDic.TryGetValue(indexName, out objList))
            {
                if (objList.Count <= 0)
                {
                    obj = Instantiate(mPrefabName, poolTransform);
                }
                else
                {
                    obj = objList[0];
                    objList.RemoveAt(0);
                }
            }
            else
            {
                obj = Instantiate(mPrefabName, poolTransform);
            }
            return obj;
        }


        private GameObject Instantiate(string mPrefabName, Transform mPoolTransform)
        {

            GameObject itemPrefab = Resources.Load<GameObject>(mPrefabName);
            return UnityEngine.Object.Instantiate(itemPrefab, mPoolTransform);
        }

    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class ResourcesLoadSys : BaseSystem<ResourcesLoadSys>
    {
        private Dictionary<string, List<GameObject>> GameObjectPoolDic;
        private Transform poolTransform;
        public override void Initialize()
        {
            GameObjectPoolDic = new Dictionary<string, List<GameObject>>(10);
            InitPool();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void ReDispose()
        {
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
            poolTransform = new GameObject("GameObjectPool").transform;
            UnityEngine.Object.DontDestroyOnLoad(poolTransform);
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

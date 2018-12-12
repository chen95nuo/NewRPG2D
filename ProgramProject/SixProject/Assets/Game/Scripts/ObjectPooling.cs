using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// This script creates the clone of required objects and provide them as required
/// </summary>
namespace MadFireOn
{
    public class ObjectPooling : MonoBehaviour
    {

        public static ObjectPooling instance;

        public GameObject explosionEffect; //ref to explosion effect prefabs
        public GameObject[] spawnedBlock;  //ref to block prefabs

        public int count = 2; //total clones of each object to be spawned

        List<GameObject> SpawnedBlock = new List<GameObject>();    //list to add them
        List<GameObject> ExplosionEffect = new List<GameObject>();

        void Awake()
        {
            MakeInstance();
        }

        void MakeInstance()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Use this for initialization
        void Start()
        {
            //block
            for (int i = 0; i < count; i++)
            {
                //each block is spawn in the array 2 times
                for (int j = 0; j < spawnedBlock.Length; j++)
                {
                    GameObject obj = Instantiate(spawnedBlock[j]);
                    obj.transform.parent = gameObject.transform;
                    obj.SetActive(false);
                    SpawnedBlock.Add(obj);
                }
            }
            //ExplosionEffect
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(explosionEffect);
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                ExplosionEffect.Add(obj);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        //method which is used to call from other scripts to get the clone object
        //block
        public GameObject GetSpawnedBlock()
        {
            //this statement checks for the in active object in the hierarcy and retun it
            for (int i = 0; i < SpawnedBlock.Count; i++)
            {
                if (!SpawnedBlock[i].activeInHierarchy)
                {
                    return SpawnedBlock[i];
                }
            }
            //if the object are less then more are spawned
            GameObject obj = new GameObject();

            for (int j = 0; j < spawnedBlock.Length; j++)
            {
                obj = Instantiate(spawnedBlock[j]);
            }
            obj.SetActive(false);
            SpawnedBlock.Add(obj);
            return obj;

        }

        //ExplosionEffect
        public GameObject GetExplosionEffect()
        {
            for (int i = 0; i < ExplosionEffect.Count; i++)
            {
                if (!ExplosionEffect[i].activeInHierarchy)
                {
                    return ExplosionEffect[i];
                }
            }
            GameObject obj = (GameObject)Instantiate(explosionEffect);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            ExplosionEffect.Add(obj);
            return obj;
        }

    }
}//namespace
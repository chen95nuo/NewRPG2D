using UnityEngine;
using System.Collections;

[System.Serializable]
public class EffectSP {
    public GameObjectPool effectPool;
    public GameObject effectprefab; 
    public int cache  = 0; 

}

public class ObjectGamepool : MonoBehaviour {

    public EffectSP[] EffectP;
	// Use this for initialization

    void Awake()
    {
        for (int i = 0; i < EffectP.Length; i++)
        {
            EffectP[i].effectPool = new GameObjectPool(EffectP[i].effectprefab, EffectP[i].cache, InitializeGameObject, false);
//            EffectP[i].effectPool.PrePopulate(EffectP[i].cache);
        }
    }

    void InitializeGameObject(GameObject mObj)
    {
 
    }

    public void UnspawnEffect(int mNum, GameObject mObj)
    {
        EffectP[mNum].effectPool.Unspawn(mObj);
    }

    public GameObject SpawnEffect(int mNum, Vector3 mPos, Quaternion mRota)
    {
        return EffectP[mNum].effectPool.Spawn(mPos, mRota);
    }



}

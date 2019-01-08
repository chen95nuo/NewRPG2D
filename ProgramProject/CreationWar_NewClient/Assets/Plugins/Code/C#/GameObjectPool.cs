using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPool  {

	private GameObject prefab;
	private List<GameObject> listObj ;
    private List<GameObject> listPool;
	private bool setActiveRecursively;
    public delegate void DelegateFun(GameObject mObj);
	public int minNum=3;
	public int maxNum=10;

    public GameObjectPool(GameObject mPrefab, int initialCapacity, DelegateFun mFunction, bool mSetActiveRecursively)
    {
        this.prefab = mPrefab;
        this.setActiveRecursively = mSetActiveRecursively;
        this.listObj = new List<GameObject>();
        this.listPool = new List<GameObject>();
    }
	
	

    /// <summary>
    /// 复制内存池实例
    /// </summary>
    /// <param name="mPos">位移</param>
    /// <param name="mRota">旋转</param>
    /// <returns></returns>
    public GameObject Spawn(Vector3 mPos, Quaternion mRota)
    {
        GameObject tempObj = null;
        if (prefab != null)
        {
            if (listObj.Count > 0)
            {
                if (listObj[0] != null)
                {
                    tempObj = listObj[0];
                    listObj.RemoveAt(0);
                    
                    listPool.Add(tempObj);
					tempObj.transform.position=mPos;
					tempObj.transform.rotation=mRota;
					tempObj.SetActiveRecursively(true);
                }
                else
                {
                    listObj.RemoveAt(0);
                    tempObj = (GameObject)Object.Instantiate(this.prefab, mPos, mRota);
                    listPool.Add(tempObj);
                }
            }
            else
            {
                tempObj = (GameObject)Object.Instantiate(this.prefab, mPos, mRota);
                listPool.Add(tempObj);
            }
        }
        else
        {
//            throw new System.Exception("要复制的内存池实例为Null");
        }
        return tempObj;
    }

	/// <summary>
	/// 复制内存池实例
	/// </summary>
	/// <param name="mPos">位移</param>
	/// <param name="mRota">旋转</param>
	/// <returns></returns>
	public GameObject NGUISpawn(Vector3 mPos, Quaternion mRota)
	{
		GameObject tempObj = null;
		if (prefab != null)
		{
			if (listObj.Count > 0)
			{
				if (listObj[0] != null)
				{
					tempObj = listObj[0];
					listObj.RemoveAt(0);
					
					listPool.Add(tempObj);
					tempObj.transform.position=mPos;
					tempObj.transform.rotation=mRota;
					tempObj.SetActiveRecursively(true);
				}
				else
				{
					listObj.RemoveAt(0);
					tempObj = NGUITools.AddChild(this.prefab);
					tempObj.transform.position = mPos;
					tempObj.transform.rotation = mRota;
//					tempObj = (GameObject)Object.Instantiate(this.prefab, mPos, mRota);
					listPool.Add(tempObj);
				}
			}
			else
			{
				tempObj = (GameObject)Object.Instantiate(this.prefab, mPos, mRota);
				listPool.Add(tempObj);
			}
		}
		else
		{
			//            throw new System.Exception("要复制的内存池实例为Null");
		}
		return tempObj;
	}

    /// <summary>
    /// 内存池实例回收
    /// </summary>
    /// <param name="mObj">要回收的实例</param>
    /// <returns></returns>
    public bool Unspawn(GameObject mObj)
    {
		if(mObj!=null)
		{
	        if (!listObj.Contains(mObj))
	        {
	            if (listPool.Contains(mObj))
	            {
					listPool.Remove (mObj);
	                mObj.SetActiveRecursively(false);
	                listObj.Add(mObj);
	                return true;
	            }
	            else
	            {
	                return false;
	            }
	        }
		}
        return false;
    }
	
	public void Clear()
	{
		listObj.Clear ();
		listPool.Clear ();
	}
	
	public void ClearNull()
	{
		GameObject[] tempObj=listObj.ToArray ();
		for(int i=0;i<tempObj.Length;i++)
		{
			if(tempObj[i]==null)
			{
				listObj.Remove (tempObj[i]);
			}
		}
		
		GameObject[] tempPool=listPool.ToArray ();
		for(int i=0;i<tempPool.Length;i++)
		{
			if(tempPool[i]==null)
			{
				listPool.Remove (tempPool[i]);
			}
		}
	}



    /// <summary>
    /// 直接复制加入内存池
    /// </summary>
    /// <param name="mNum"></param>
    public void PrePopulate(int mNum)
    {
        if (prefab != null)
        {
            for (int i = 0; i < mNum; i++)
            {
                GameObject obj = (GameObject)Object.Instantiate(prefab);
                listObj.Add(obj);
            }
        }
        else
        {
  //          throw new System.Exception("要复制的内存池实例为Null");
        }
    }
}

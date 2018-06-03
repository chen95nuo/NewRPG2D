
//功能：类的扩展方法，主要针对Unity相关的类（Gameobject, Transform）
//创建者: 胡海辉
//创建时间：


using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Extension
{
    #region gameobject
    public static void CustomSetActive(this GameObject obj, bool bActive)
    {
        if (obj != null && obj.activeSelf != bActive)
        {
            obj.SetActive(bActive);
        }
    }

    public static bool GetActiveState(this GameObject obj, bool bActive)
    {
        if (obj != null)
        {
            return obj.activeInHierarchy == bActive;
        }
        else 
        {
            DebugHelper.LogError(" Extension  GetActiveState obj is null ");
            return false;
        }
    }
    #endregion

    #region transform
    public static void CustomSetActive(this Transform trans, bool bActive)
    {
        if (trans != null )
        {
            trans.gameObject.SetActive(bActive);
        }
    }

    public static T FindChildComponent<T>(this  Transform trans, string childPath, bool forceAdd = false) where T : Component 
    {
        Transform temp = trans.Find(childPath);
        if (temp != null)
        {
            if (temp.GetComponent<T>() == null)
            {
                if (forceAdd) { temp.gameObject.AddComponent<T>(); }
                else
                {
                    DebugHelper.LogErrorFormat(" {0} Component is Null  try forceAdd ", trans.name);
                    return (T)null;
                }
            }
            return temp.GetComponent<T>();
        }
        else 
        {
            DebugHelper.LogErrorFormat(" can't find {0} child {1}  ", trans.name,childPath);
            return (T)null;
        }
    }
    #endregion



}

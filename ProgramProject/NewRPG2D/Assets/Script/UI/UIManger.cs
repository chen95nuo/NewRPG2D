using UnityEngine;
using System.Collections;
using System;
using Assets.Script.EventMgr;
using Assets.Script.Tools;

public class UIManger : MonoBehaviour 
{
    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        InitCompenont();
        InitListener();
        InitData();

    }

    /// <summary>
    /// 初始化组件
    /// </summary>
    private void InitCompenont()
    {
    }

    private void InitData()
    {
      
    }

      /// <summary>
    /// 初始化监听
    /// </summary>
    private void InitListener()
    {
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    private void RemoveListener()
    {
    }

    private void TiggerChangeType(object obj, EventArgs e)
    {
        //TiggerTypeParam param = (TiggerTypeParam)obj;
    
    }
}

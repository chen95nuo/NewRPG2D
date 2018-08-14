//功能： 战斗内事件管理
//创建者: 胡海辉
//创建时间：

using System;
using System.Collections.Generic;
using UnityEngine;


public class EventManager : TSingleton<EventManager>
{
    private List<Delegate>[] listenerTarget;
    public override void Init()
    {
        base.Init();
        listenerTarget = new List<Delegate>[(int)EventDefineEnum.EventMax];
        for (int i = 0; i < (int)EventDefineEnum.EventMax; i++)
        {
            listenerTarget[i] = new List<Delegate>(10);
        }
    }

    public void AddListener(EventDefineEnum eventID, TDelegate eventHadle)
    {
        int index = (int)eventID;
        if (CheckValid(index, eventHadle))
        {
            listenerTarget[index].Add(eventHadle);
        }
    }

    public void RemoveListener(EventDefineEnum eventID, TDelegate eventHadle)
    {
        int index = (int)eventID;
        if (CheckValid(index, eventHadle, true))
        {
            listenerTarget[index].Remove(eventHadle);
        }
    }
    public void SendEvent(EventDefineEnum eventID)
    {
        int index = (int)eventID;
        if (index > listenerTarget.Length)
        {
            DebugHelper.LogError("EventDefine error " + eventID);
            return;
        }
        for (int i = 0; i < listenerTarget[index].Count; i++)
        {
            TDelegate tempDelegate = listenerTarget[index][i] as TDelegate;
            if (tempDelegate != null)
            {
                tempDelegate();
            }
        }
    }

    public void AddListener<TParam>(EventDefineEnum eventID, TDelegate<TParam> eventHadle)
    {
        int index = (int)eventID;
        if (CheckValid(index, eventHadle))
        {
            listenerTarget[index].Add(eventHadle);
        }
    }

    public void RemoveListener<TParam>(EventDefineEnum eventID, TDelegate<TParam> eventHadle)
    {
        int index = (int)eventID;
        if (CheckValid(index, eventHadle, true))
        {
            listenerTarget[index].Remove(eventHadle);
        }
    }

    public void SendEvent<TParam>(EventDefineEnum eventID, TParam param)
    {
        int index = (int) eventID;
        if (index > listenerTarget.Length)
        {
            UnityEngine.Debug.LogError("EventDefine error " + eventID);
            return;
        }

        for (int i = 0; i < listenerTarget[index].Count; i++)
        {
            TDelegate<TParam> tempDelegate = listenerTarget[index][i] as TDelegate<TParam>;
           
            if (tempDelegate != null)
            {
                tempDelegate(param);
            }
        }
    }

    public void RemoveAllListener()
    {
        if (listenerTarget != null)
        {
            for (int i = 0; i < listenerTarget.Length; i++)
            {
                List<Delegate> delegator = listenerTarget[i];
                if (delegator != null)
                {
                    delegator.Clear();
                    delegator = null;
                }
            }
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        RemoveAllListener();
        listenerTarget = null;
    }

    private bool CheckValid(int evt, Delegate handler, bool bRemove = false)
    {
        if (handler == null || handler.Target == null)
        {
            return false;
        }
        if (evt >= listenerTarget.Length || listenerTarget[evt] == null)
        {
            return false;
        }
        if (listenerTarget[evt].Contains(handler))
        {
            if (bRemove == false)
            {
                DebugHelper.LogError(" -------------have same event and same reference------  " + (EventDefineEnum)evt);
            }
        }
        return true;
    }
}

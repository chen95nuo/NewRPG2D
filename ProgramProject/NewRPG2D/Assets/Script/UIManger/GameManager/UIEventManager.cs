using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventManager : TSingleton<UIEventManager>
{
    private List<Delegate>[] listenerTarget;
    public override void Init()
    {
        base.Init();
        listenerTarget = new List<Delegate>[(int)UIEventDefineEnum.EventMax];
        for (int i = 0; i < (int)UIEventDefineEnum.EventMax; i++)
        {
            listenerTarget[i] = new List<Delegate>(10);
        }
    }

    public void AddListener(UIEventDefineEnum eventID, TDelegate eventHadle)
    {
        int index = (int)eventID;
        if (CheckValid(index, eventHadle))
        {
            listenerTarget[index].Add(eventHadle);
        }
    }

    public void RemoveListener(UIEventDefineEnum eventID, TDelegate eventHadle)
    {
        int index = (int)eventID;
        if (CheckValid(index, eventHadle, true))
        {
            listenerTarget[index].Remove(eventHadle);
        }
    }
    public void SendEvent(UIEventDefineEnum eventID)
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

    public void AddListener<TParam>(UIEventDefineEnum eventID, TDelegate<TParam> eventHadle)
    {
        int index = (int)eventID;
        if (CheckValid(index, eventHadle))
        {
            listenerTarget[index].Add(eventHadle);
        }
    }

    public void RemoveListener<TParam>(UIEventDefineEnum eventID, TDelegate<TParam> eventHadle)
    {
        int index = (int)eventID;
        if (CheckValid(index, eventHadle, true))
        {
            listenerTarget[index].Remove(eventHadle);
        }
    }
    public void SendEvent<TParam>(UIEventDefineEnum eventID, TParam param)
    {
        int index = (int)eventID;
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
                DebugHelper.LogError(" -------------have same event and same reference------  " + (UIEventDefineEnum)evt);
            }
        }
        return true;
    }
}

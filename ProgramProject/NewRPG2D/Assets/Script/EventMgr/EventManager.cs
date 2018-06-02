using System;
using System.Collections.Generic;
using UnityEngine;

public class WeakDalegateBase
{
    public WeakReference Target;
    public WeakDalegateBase(WeakReference target)
    {
        Target = target;
    }
}
public class WeakDelegate : WeakDalegateBase
{
    public TDelegate Method;
    public WeakDelegate(WeakReference target, TDelegate method) : base(target)
    {
        Method = method;
    }
}
public class WeakDelegate<T> : WeakDalegateBase
{
    public TDelegate<T> Method;
    public WeakDelegate(WeakReference target, TDelegate<T> method) : base(target)
    {
        Method = method;
    }
}

public class EventManager : TSingleton<EventManager>,IDisposable
{
    private List<WeakDalegateBase>[] listenerTarget;
    public EventManager()
    {
        listenerTarget = new List<WeakDalegateBase>[(int)EventDefineEnum.EventMax];
        for (int i = 0; i < (int)EventDefineEnum.EventMax; i++)
        {
            listenerTarget[i] = new List<WeakDalegateBase>(10);
        }
    }

    public void AddListener(EventDefineEnum eventID, WeakDelegate eventHadle)
    {
        int index = (int)eventID;
        if (CheckValid(index, eventHadle))
        {
            listenerTarget[index].Add(eventHadle);
        }
    }

    public void RemoveListener(EventDefineEnum eventID, WeakDelegate eventHadle)
    {
        int index = (int)eventID;
        if (CheckValid(index, eventHadle, true))
        {
            listenerTarget[index].Remove(eventHadle);
        }
    }
    private List<WeakDalegateBase> removeList = new List<WeakDalegateBase>();
    public void RasieEvent(EventDefineEnum eventID)
    {
        int index = (int)eventID;
        if (index > listenerTarget.Length)
        {
            UnityEngine.Debug.LogError("EventDefine error " + eventID);
            return;
        }

        for (int i = 0; i < listenerTarget[index].Count; i++)
        {
            WeakDelegate tDelegate = listenerTarget[index][i] as WeakDelegate;
            if (tDelegate.Target != null)
            {
                tDelegate.Method();
            }
            else
            {
                removeList.Add(tDelegate);
            }
        }
        foreach (var item in removeList)
        {
            listenerTarget[index].Remove(item);
        }
        removeList.Clear();
    }

    public void AddListener<TParam>(EventDefineEnum eventID, WeakDelegate<TParam> eventHadle)
    {
        int index = (int)eventID;
        if (CheckValid(index, eventHadle))
        {
            listenerTarget[index].Add(eventHadle);
        }
    }

    public void RemoveListener<TParam>(EventDefineEnum eventID, WeakDelegate<TParam> eventHadle)
    {
        int index = (int)eventID;
        if (CheckValid(index, eventHadle, true))
        {
            listenerTarget[index].Remove(eventHadle);
        }
    }
    public void RasieEvent<TParam>(EventDefineEnum eventID, TParam param)
    {
        int index = (int)eventID;
        if (index > listenerTarget.Length)
        {
            UnityEngine.Debug.LogError("EventDefine error " + eventID);
            return;
        }

        for (int i = 0; i < listenerTarget[index].Count; i++)
        {
            WeakDelegate<TParam> tDelegate = listenerTarget[index][i] as WeakDelegate<TParam>;
            if (tDelegate.Target != null)
            {
                tDelegate.Method(param);
            }
            else
            {
                removeList.Add(tDelegate);
            }
        }
        foreach (var item in removeList)
        {
            listenerTarget[index].Remove(item);
        }
        removeList.Clear();
    }

    public void RemoveAllListener()
    {
        if (listenerTarget != null)
        {
            for (int i = 0; i < listenerTarget.Length; i++)
            {
                List<WeakDalegateBase> delegator = listenerTarget[i];
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

    private bool CheckValid(int evt, WeakDalegateBase handler, bool bRemove = false)
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
                if (Application.isPlaying)
                    UnityEngine.Debug.LogError(" -------------have same event and same reference------  " + (EventDefineEnum)evt);
            }
        }
        else
        {
            for (int i = 0; i < listenerTarget[evt].Count; i++)
            {
                if (listenerTarget[evt][i].Target.Target == handler.Target.Target)
                {
                    if (Application.isPlaying)
                        UnityEngine.Debug.LogError(" -------------have same event and same target------  " + handler.Target.Target.ToString());
                    break;
                }
            }
            if (bRemove)
            {
                if (Application.isPlaying)
                    UnityEngine.Debug.LogError(" ----------don't have the event reference------  " + (EventDefineEnum)evt);
                return false;
            }
        }
        return true;
    }
}

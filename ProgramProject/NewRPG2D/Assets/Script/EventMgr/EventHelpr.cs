using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EventHelper
{
}
public enum EventDefineEnum : uint
{

    EventMax,
}

public enum SwitchStatusEnum
{
    OpenStatus,
    CloseStatus,
}

public delegate void TDelegate();
public delegate void TDelegate<T>(T t);

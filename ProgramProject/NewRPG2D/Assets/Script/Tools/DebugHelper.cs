
//功能：
//创建者: 胡海辉
//创建时间：


using UnityEngine;
public class DebugHelper
{
    public static bool bEnableDebug = true;

    public static void DebugLog(string str)
    {
        if (bEnableDebug)
            Debug.Log(str);
    }

    public static void DebugLogFormat(string formatStr, params object[] args)
    {
        if (bEnableDebug)
        {
            Debug.LogFormat(formatStr, args);
        }
    }

    public static void DebugLogError(string str)
    {
        if (bEnableDebug)
            Debug.LogError(str);
    }

    public static void DebugLogErrorFormat(string formatStr, params object[] args)
    {
        if (bEnableDebug)
            Debug.LogErrorFormat(formatStr, args);
    }


    public static void Assert(bool condition, string str)
    {
        if (condition)
        {
            DebugLog(str);
        }
    }

    public static void Assert(bool condition, string formatStr, params object[] args)
    {
        if (condition)
        {
            DebugLogFormat(formatStr, args);
        }
    }

    public static void AssertError(bool condition, string str)
    {
        if (condition)
        {
            DebugLogError(str);
        }
    }

    public static void AssertError(bool condition, string formatStr, params object[] args)
    {
        if (condition)
        {
            DebugLogErrorFormat(formatStr, args);
        }
    }
}

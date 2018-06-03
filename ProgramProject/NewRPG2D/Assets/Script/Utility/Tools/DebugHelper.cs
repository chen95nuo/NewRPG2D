
//功能：Debug 控制
//创建者: 胡海辉
//创建时间：


using UnityEngine;
public class DebugHelper
{
    public static bool bEnableDebug = true;

    public static void Log(string str)
    {
        if (bEnableDebug)
            Debug.Log(str);
    }

    public static void LogFormat(string formatStr, params object[] args)
    {
        if (bEnableDebug)
        {
            Debug.LogFormat(formatStr, args);
        }
    }

    public static void LogError(string str)
    {
        if (bEnableDebug)
            Debug.LogError("Error:" + str);
    }

    public static void LogErrorFormat(string formatStr, params object[] args)
    {
        if (bEnableDebug)
            Debug.LogErrorFormat(formatStr, args);
    }


    public static void Assert(bool condition, string str)
    {
        if (condition)
        {
            Log(str);
        }
    }

    public static void Assert(bool condition, string formatStr, params object[] args)
    {
        if (condition)
        {
           LogFormat(formatStr, args);
        }
    }

    public static void AssertError(bool condition, string str)
    {
        if (condition)
        {
            LogError(str);
        }
    }

    public static void AssertError(bool condition, string formatStr, params object[] args)
    {
        if (condition)
        {
           LogErrorFormat(formatStr, args);
        }
    }
}

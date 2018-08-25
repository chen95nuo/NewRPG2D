using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFormatString
{
    private static UIFormatString instance;

    public static UIFormatString Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIFormatString();
            }
            return instance;
        }
    }

    /// <summary>
    /// 格式化string的数组返回float数组 以逗号区分
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public float[] FormatDataString(string str)
    {
        if (str == null && str == "")
        {
            Debug.LogError("传入数组格式错误" + str);
            return null;
        }
        string[] st;
        st = str.Split(',');
        float[] backNumber = new float[st.Length];
        for (int i = 0; i < st.Length; i++)
        {
            backNumber[i] = System.Convert.ToSingle(st[i]);
        }

        return backNumber;
    }

}

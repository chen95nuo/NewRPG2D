using UnityEngine;
using System.Collections;

public class GooglePayJson
{
    public string mOrderId;
    public string mOriginalJson;
    public string mSignature;
    public string mPackageName;
    public string total_fee;
}

public class GooglePayBackJson
{
    public int code;
    public bool ret;
    public string mOrderId;
    public string mOriginalJson;
    public string mSignature;
    public string mPackageName;
}

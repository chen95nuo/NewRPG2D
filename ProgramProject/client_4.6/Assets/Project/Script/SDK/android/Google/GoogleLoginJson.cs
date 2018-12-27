using UnityEngine;
using System.Collections;

public class GooglePayJson : BasicJson
{
    public string mOrderId;
    public string mOriginalJson;
    public string mSignature;
    public string mPackageName;
    public string total_fee;
    public GooglePayJson(string mOrderId, string mOriginalJson, string mSignature, string mPackageName, string total_fee)
    {
        this.mOrderId = mOrderId;
        this.mOriginalJson = mOriginalJson;
        this.mSignature = mSignature;
        this.mPackageName = mPackageName;
        this.total_fee = total_fee;
    }
}

public class GooglePayBackJson
{
    public int code;
    public bool ret;
    public string mOrderId;
    public string mOriginalJson;
    public string mSignature;
    public string mPackageName;
    public string mItemType;
}

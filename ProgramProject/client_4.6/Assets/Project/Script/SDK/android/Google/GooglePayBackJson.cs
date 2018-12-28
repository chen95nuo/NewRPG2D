using UnityEngine;
using System.Collections;
public class GooglePayBackJson
{
    public int code { get; set; }
    public bool ret { get; set; }
    public string mOrderId { get; set; }
    public string mOriginalJson { get; set; }
    public string mSignature { get; set; }
    public string mPackageName { get; set; }
    public string mItemType { get; set; }
}

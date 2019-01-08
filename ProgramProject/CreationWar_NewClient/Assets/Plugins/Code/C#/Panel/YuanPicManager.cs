using System.IO;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Collection of connection-relevant settings, used internally by PhotonNetwork.ConnectUsingSettings.
/// </summary>
[System.Serializable]
public class YuanPicManager : MonoBehaviour
{
    public List<YuanPic> picSelectBackground;
    public List<YuanPic> picSelectMark;
    public List<YuanPic> picSelectMask;
    public List<YuanPic> picPlayer;
	public List<YuanPic> picExpression;
    
}

[System.Serializable]
public class YuanPic
{
    public UIAtlas atlas = null;
    public string spriteName = null;
}

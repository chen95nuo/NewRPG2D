using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YuanRank {


    private bool isUpdate;
    public bool IsUpdate
    {
        get { return isUpdate; }
        set { isUpdate = value; }
    }
    public string rankName;
    public int myRank;
    public Dictionary<string, int> dicMyRank;
   
    public YuanRank(string mRankName)
    {
        this.rankName = mRankName;
        isUpdate = false;
    }


}

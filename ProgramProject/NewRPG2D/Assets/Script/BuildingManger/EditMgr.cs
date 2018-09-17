using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMgr : CastleMgr
{
    public void UpdateEditCastle(CastleMgr mgr)
    {
        Debug.Log("运行了" + mgr.serverRoom);
        AcceptServerRoom(mgr.serverRoom);
    }
}

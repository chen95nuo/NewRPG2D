using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoleTipGrid : UITipGrid
{
    protected override void ChickEnter()
    {
        HallRoleMgr.instance.LevelComplete(currentData, true);
        base.ChickEnter();
    }
}

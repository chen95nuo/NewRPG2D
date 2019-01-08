/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;

public class ViewSelection : MonoBehaviour 
{
    public GameObject all;
	void OnEnable () 
	{
	    if(Application.loadedLevelName == "Map200")
        {
            all.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(false);
            all.SetActive(true);
        }
	}	

    /// <summary>
    /// 设置自由视角
    /// </summary>
    public void SetFreeView()
    {
        PlayerPrefs.SetInt("LockFov", 1);

        this.gameObject.SetActive(false);
        all.SetActive(true);
        
        Camera.mainCamera.SendMessage("FreeFOV", SendMessageOptions.DontRequireReceiver);

        InRoom.GetInRoomInstantiate().SetViewSelection(1);
		//td 统计视角选择
		//TD_info.panelStatistics (StaticLoc.Loc.Get("tdinfo041"));
    }

    /// <summary>
    /// 设置上帝视角
    /// </summary>
    public void SetGodView()
    {
        PlayerPrefs.SetInt("LockFov", 0);

        this.gameObject.SetActive(false);
        all.SetActive(true);

        Camera.mainCamera.SendMessage("GodFOV", SendMessageOptions.DontRequireReceiver);

        InRoom.GetInRoomInstantiate().SetViewSelection(0);

		//td 统计视角选择
		//TD_info.panelStatistics (StaticLoc.Loc.Get("tdinfo040"));
    }
}

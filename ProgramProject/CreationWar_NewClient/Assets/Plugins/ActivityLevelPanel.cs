/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivityLevelPanel : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
	
	}

    public void BuyBloodStore()
    {
        PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("StoreMoveOn", SendMessageOptions.DontRequireReceiver);
    }
}

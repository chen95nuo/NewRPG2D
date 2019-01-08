/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;

public class AdjustPos : MonoBehaviour {

	void Awake () 
	{
        if (Screen.height * 4 >= Screen.width * 3)
        {
            this.transform.localPosition = new Vector3(250, 300, 0);
        }
        else
        {
            this.transform.localPosition = new Vector3(340, 260, 0);
        }
	}
}

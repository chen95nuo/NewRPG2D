using UnityEngine;
using System.Collections;

public class CkbClick : MonoBehaviour {

    [HideInInspector]
    public CkbToPanel ctp;

	// Use this for initialization
	void Start () {
	
	}

    public void OnClick()
    {
        ctp.CbkClick();
    }
}

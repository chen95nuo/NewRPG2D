using UnityEngine;
using System.Collections;

public class DisableButtons : MonoBehaviour {
    public UIButton[] disableBtns;
    public GameObject grid;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        if (grid != null)
        {
            bool hasActive = false;
            for (int i = 0; i < grid.transform.childCount; i++)
            {
                GameObject go = grid.transform.GetChild(i).gameObject;
                if (go.active)
                {
                    hasActive = true;
                }
            }

            if (hasActive)
            {
                EnableBtns();
            }
            else
            {
                DisableBtns();
            }
        }
	}

    void DisableBtns()
    {
        foreach (UIButton btn in disableBtns)
        {
            btn.isEnabled = false;
        }
    }

    void EnableBtns()
    {
        foreach (UIButton btn in disableBtns)
        {
            btn.isEnabled = true;
        }
    }
}

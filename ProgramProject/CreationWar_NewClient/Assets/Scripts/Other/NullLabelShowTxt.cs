using UnityEngine;
using System.Collections;

public class NullLabelShowTxt : MonoBehaviour {
    public UILabel[] labels;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnEnable()
    {
		InvokeRepeating("ShowNullTxt",0,1);
    }

    void ShowNullTxt()
    {
        foreach(UILabel label in labels)
        {
            if (string.IsNullOrEmpty(label.text))
            {
                label.text = StaticLoc.Loc.Get("info689");
            }
        }
    }
}

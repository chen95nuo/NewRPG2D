using UnityEngine;
using System.Collections;

public class ActTopIcon : MonoBehaviour {

    public UISprite texPic;
    public UISprite texSel;
    public GameObject mark;

    ActivityElement ae;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(ActivityElement activityElement)
    {
        ae = activityElement;

        string sName = texPic.spriteName;
        texPic.spriteName = sName.Substring(0, sName.Length - 1) + ae.type;

        if (activityElement.t == 1)
            setMark(true);
        else
            setMark(false);

        setSel(false);
    }

    public void setMark(bool flag)
    {
        mark.SetActive(flag);
    }

    public void setSel(bool flag)
    {
        texSel.alpha = flag ? 255 : 0;
    }
}

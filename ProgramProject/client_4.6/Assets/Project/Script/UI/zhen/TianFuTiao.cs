using UnityEngine;
using System.Collections;

public class TianFuTiao : MonoBehaviour {
    public UISprite icon;
    public UILabel xlabel, infolabel;
    public float height;
	// Use this for initialization
	void Start () {
	
	}
    public void reset(string icname, string xl, string il,bool active)
    {
        icon.spriteName = icname;
        infolabel.text = xl;
        xlabel.text = il;
        height = xlabel.height*xlabel.gameObject.transform.localScale.x;
        BoxCollider box = gameObject.GetComponent<BoxCollider>();
        box.size = new Vector3(300, height, 0);
        box.center = new Vector3(130, -height/2+20, 0);
       
		if(active)
		{
			//infolabel.color=Color.white;
			//xlabel.color=Color.white;
		}
		else
		{
			infolabel.color=Color.gray;
			xlabel.color=Color.gray;
		}
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

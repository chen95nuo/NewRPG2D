using UnityEngine;
using System.Collections;

public class GetItemNum : MonoBehaviour {

    public UILabel nums;

    public UILabel texts;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



    public void GetNum(int num,string text)
    {
        texts.text = text;
        if (num == 0)
        {

            nums.text = "";
        }
        else
        {
            nums.text = "x  " + num;
        }
    }
}

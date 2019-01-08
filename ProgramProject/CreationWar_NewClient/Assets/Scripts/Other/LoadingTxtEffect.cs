using UnityEngine;
using System.Collections;

public class LoadingTxtEffect : MonoBehaviour {
    private string[] texts = { "Loading.  ", "Loading.. ", "Loading..." };
    private UILabel label;
    private int count = 0;
    public float duration = 0;
    private float tempTime;
	void Start () {
        label = GetComponent<UILabel>();
        label.text = texts[2];
        tempTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        count ++;
        if (Time.time - tempTime > duration)
        {
            label.text = texts[count % texts.Length];
            tempTime = Time.time;
        } 
	}
}

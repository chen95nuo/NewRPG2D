using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {

    public float Speed;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(this.transform.forward, Time.deltaTime * Speed);
	}
}

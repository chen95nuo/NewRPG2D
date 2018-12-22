using UnityEngine;
using System.Collections;

public class ZhenRoot : MonoBehaviour {
    public static ZhenRoot mInstance;
    public Zhen zhen;
    void Awake()
    {
        mInstance = this;
    }
	// Use this for initialization
	void Start () {
		mInstance.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

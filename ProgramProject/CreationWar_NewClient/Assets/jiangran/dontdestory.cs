using UnityEngine;
using System.Collections;

public class dontdestory : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(transform.gameObject);
	
	}
}

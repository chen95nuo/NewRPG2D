using UnityEngine;
using System.Collections;

public class AnnocementPanel : MonoBehaviour {

    public GameObject[] objs;
	void OnEnable () {
        foreach (GameObject go in objs)
        { 
            if(go.active)
            {
                go.SetActiveRecursively(false);
            }
        }
	}
}

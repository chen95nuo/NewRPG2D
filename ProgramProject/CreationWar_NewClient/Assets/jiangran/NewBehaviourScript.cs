using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	public GameObject obj;
	void OnClick(){
		obj.SetActiveRecursively(false);
	}
}

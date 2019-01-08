using UnityEngine;
using System.Collections;

public class StaticLoc : MonoBehaviour {
	public Localization Locs;
	public static Localization Loc;
	void Awake(){
		Loc = Locs;
	}
}

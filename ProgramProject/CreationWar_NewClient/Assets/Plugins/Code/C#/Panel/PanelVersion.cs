using UnityEngine;
using System.Collections;

public class PanelVersion : MonoBehaviour {
	
	public UILabel lblVersion;
	
	// Use this for initialization
	void Start () {
		lblVersion.text="V. "+YuanUnityPhoton.GameVersion.ToString ();
	}

}

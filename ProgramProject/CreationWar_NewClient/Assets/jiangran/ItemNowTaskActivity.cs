using UnityEngine;
using System.Collections;

public class ItemNowTaskActivity : MonoBehaviour {
	public GameObject BtnTaskPanel;
	// Use this for initialization
	
	void OnEnable(){
		TaskAllPanelContorl  tpc = BtnTaskPanel.transform.GetComponent<TaskAllPanelContorl>();
		tpc.MyObjpos();
	}
	
}

using UnityEngine;
using System.Collections;

public class TaskAllPanelContorl : MonoBehaviour {
	public GameObject myPanel;
	public GameObject[] OtherPanel;
	// Use this for initialization
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void MyObjpos(){
		if(OtherPanel!=null){
		for(int i = 0;i<OtherPanel.Length;i++){
//			if(OtherPanel[i].name=="Anchor - Task"){
				OtherPanel[i].SetActiveRecursively(false);
//			}
			Vector3 othLocalpos = OtherPanel[i].transform.localPosition;
			OtherPanel[i].transform.localPosition = new Vector3(othLocalpos.x,3000,othLocalpos.z);
			
		}
		}
		
		
		if(myPanel!=null){
//		if(myPanel.name=="Anchor - Task"){
			myPanel.SetActiveRecursively(true);
//		}
	
		Vector3 localPos = myPanel.transform.localPosition;
		myPanel.transform.localPosition = new Vector3(localPos.x,0,localPos.z);
		}
		
	
	
}

    void DisableActivityPanel()
    {
        PanelStatic.StaticWarnings.warningAllTime.Show("", StaticLoc.Loc.Get("info778"));
    }
}
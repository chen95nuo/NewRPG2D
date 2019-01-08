using UnityEngine;
using System.Collections;

public class HideObjOnEnable : MonoBehaviour {
    public GameObject targetObj;
	void OnEnable () 
	{
        StateSwitch();
	}
	
	void StateSwitch()
	{
		if(targetObj == null) return;
		
		if(this.transform.childCount > 0)
        {
			bool isAllActive = false;
			for(int i=0;i<transform.childCount;i++){
				
				isAllActive |= transform.GetChild(i).gameObject.active;
			}
			
			if(isAllActive)
				targetObj.active = false;
			else
				targetObj.active = true;
        }
		else
		{
			targetObj.active = true;
		}
	}
	
	void Update()
	{
		if(gameObject.active)
			StateSwitch();
	}
}

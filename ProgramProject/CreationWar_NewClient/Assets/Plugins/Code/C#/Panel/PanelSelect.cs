using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelSelect : MonoBehaviour {
    public List<GameObject> listPanel;
    
	public BtnSelect firstBtn;
	
	public GameObject btnCode;
	
    void OnEnable()
	{
		StartCoroutine (YuanOnEnable ());
		//firstBtn.GetComponent<UIToggle>().isChecked=true;
	}
	
	public IEnumerator YuanOnEnable()
	{
		if(InRoom.GetInRoomInstantiate ().GetServerSwitchString (yuan.YuanPhoton.BenefitsType.RedemptionCodeSwitch)!="1")
		{
			if(btnCode!=null)
			{
				btnCode.SetActiveRecursively (false);
			}
		}		
		yuan.YuanClass.SwitchList (listPanel,true,true);
		yield return new WaitForFixedUpdate();
		firstBtn.OnClick ();		
	}
}

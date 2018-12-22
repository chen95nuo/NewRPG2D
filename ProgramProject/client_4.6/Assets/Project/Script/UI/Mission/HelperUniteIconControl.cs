using UnityEngine;
using System.Collections;

public class HelperUniteIconControl : MonoBehaviour {
	
	public int UniteSkillId;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public void ShowDetails()
	{
		ShowUniteInfoManager.mInstance.setData(UniteSkillId);
		Vector3 reV3 = transform.position;
		ShowUniteInfoManager.mInstance.transform.position = new Vector3(reV3.x + 0.8f, reV3.y + 0.4f, 0f);
	}
	
	
	public void OnPress(bool isPressed)
	{
		if(isPressed)
		{
			
			ShowDetails();
		}else
		{
			 ShowUniteInfoManager.mInstance.hide ();
		}
	}
	
	
	
}

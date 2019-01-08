using UnityEngine;
using System.Collections;

public class CreatTeamBtn : MonoBehaviour {
	public UIToggle UT;
	public BoxCollider BxOther;
	public bool disable = false;
	public static CreatTeamBtn creatTeamBtn;
	// Use this for initialization
	void Start () {
		creatTeamBtn = this;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public bool Disable
	{
		get { return disable; }
		set
		{
			disable = value;
			BtnStateClick(disable);
		}
	}
	void BtnStateClick(bool IsClick)
	{
		if(UT){
		if(IsClick){
			UT.value = true;
			BxOther.enabled = false;
			}else{
			BxOther.enabled = true;
			}
		}
	}
}

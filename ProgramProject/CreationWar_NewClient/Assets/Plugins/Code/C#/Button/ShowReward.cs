using UnityEngine;
using System.Collections;

public class ShowReward : MonoBehaviour {
	public UISprite Reward;
	public UISprite ItemBox;
	public UILabel 	reWardName;

	public string itemID;
	public ParticleSystem Part;

	void OnEnable(){
		if(Part){
		Part.Play();
		}
	}

	void OnClick()
	{
//		if (!canClickIcon) return;
		
		if(itemID!="")
		{
			PanelStatic.StaticIteminfo.SetActiveRecursively(true);
			PanelStatic.StaticIteminfo.transform.position = new Vector3(transform.position.x,100,transform.position.z);
			PanelStatic.StaticIteminfo.SendMessage("SetItemID", itemID, SendMessageOptions.DontRequireReceiver);
			}
		}
}

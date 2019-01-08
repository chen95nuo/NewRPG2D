using UnityEngine;
using System.Collections;

public class InviteFriends : MonoBehaviour {
	public Warnings warnings;
	// Use this for initialization
	void Start () {
		warnings=PanelStatic.StaticWarnings;
	}


	public void ShowInviteFriend(){


		if(MonsterSpawnPointHandler.GetInstance().CurrentPlayerCount<=2){
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("ShowInvite",SendMessageOptions.DontRequireReceiver);
		}else{
			warnings.warningAllTime.Show("",StaticLoc.Loc.Get("info968"));
		}
	}
}

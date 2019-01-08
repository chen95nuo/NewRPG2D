using UnityEngine;
using System.Collections;

public class BtnNearPlayers : MonoBehaviour {
	public UISprite PlayerPro;
	public UILabel PlayerName;
	public UILabel PlayerLeve;
	public UILabel PlayerDeulWinCount;
	public UILabel PlayerForce;
	public UISprite btnDuelState;

	public int DuelState = 0;
	public int instanceID = 0;
	public void SetDuelState(int state , int id)
	{
		instanceID = id;
		DuelState = state;
		if(DuelState == 0)
			btnDuelState.spriteName = "UIH_Minor_Button_N";
		else
			btnDuelState.spriteName = "UIH_Minor_Button_O";
	}

	public void RequestDuel(){
		if(DuelState == 0){
			ServerRequest.requestDuelInvite(instanceID);
		}
	}

}

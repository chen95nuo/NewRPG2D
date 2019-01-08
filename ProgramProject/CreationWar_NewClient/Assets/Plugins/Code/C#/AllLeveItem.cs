using UnityEngine;
using System.Collections;

public class AllLeveItem : MonoBehaviour {

	public BtnDisable[] LeveMe;

	public int MyBtnLeve;
	public UIGrid uiGrid;
	public void ClickBtn()
	{
		InRoom.GetInRoomInstantiate().GetLevelPack(MyBtnLeve);
	}

}

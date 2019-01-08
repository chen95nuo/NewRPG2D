using UnityEngine;
using System.Collections;

public class RefreshOnePVPList : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


    public void GetList()
    {
        InRoom.GetInRoomInstantiate().GetPVETeamList();
    }

    public void GetLegionOneList()
    {
        InRoom.GetInRoomInstantiate().GetLegionOneList();
    }

   


}

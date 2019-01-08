using UnityEngine;
using System.Collections;

public class OpenPVPCreate : MonoBehaviour {

    public int level;
    public UIButton btnCreate;
    public UIButton btnAdd;

    void OnEnable()
    {
        if ((InRoom.isUpdatePlayerLevel?InRoom.playerLevel.Parse (0):int.Parse(BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText)) >= level)
        {
            btnCreate.isEnabled = true;
            btnAdd.isEnabled = true;
        }
        else
        {
            btnCreate.isEnabled = false;
            btnAdd.isEnabled = false;
        }
    }
	

}

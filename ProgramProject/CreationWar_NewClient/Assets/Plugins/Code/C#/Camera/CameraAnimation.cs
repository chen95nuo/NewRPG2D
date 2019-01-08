using UnityEngine;
using System.Collections;

public class CameraAnimation : MonoBehaviour {
    public BtnManager btnManager;
	// Use this for initialization
	void Start () {
	
	}
	


    void AnimStart()
    {
        yuan.YuanClass.SwitchList(btnManager.listMenu, false, true);
    }

    void ToSelectPlayer()
    {
        btnManager.PlayerStartIn();
    }

    void ToNewPlayer()
    {
        btnManager.PlayerCreatIn();
    }
}

using UnityEngine;
using System.Collections;

public class SetTempTeam : MonoBehaviour {

    public GameObject setTempTeam;
    public BtnGameManager btnGameManager;
    void OnEnable()
    {
        setTempTeam.SetActive(BtnGameManager.IsInTempTeam);

        PanelStatic.StaticBtnGameManager.GetTempTeam();
    }
}

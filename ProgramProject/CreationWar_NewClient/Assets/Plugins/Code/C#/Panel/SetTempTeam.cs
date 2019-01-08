using UnityEngine;
using System.Collections;

public class SetTempTeam : MonoBehaviour {

    public GameObject setTempTeam;
    public BtnGameManager btnGameManager;
    void OnEnable()
    {
        setTempTeam.SetActiveRecursively(BtnGameManager.IsInTempTeam);

        PanelStatic.StaticBtnGameManager.GetTempTeam();
    }
}

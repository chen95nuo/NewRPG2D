using UnityEngine;
using System.Collections;

public class BtnRankingType : MonoBehaviour {
    public PanelRanking panelRanking;
    void OnClick()
    {
        StartCoroutine(panelRanking.SetRanking());
    }
}

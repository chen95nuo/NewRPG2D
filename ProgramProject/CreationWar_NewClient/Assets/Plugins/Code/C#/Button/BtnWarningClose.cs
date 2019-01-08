using UnityEngine;
using System.Collections;

public class BtnWarningClose : MonoBehaviour {

    void OnClick()
    {
        this.transform.parent.gameObject.SetActiveRecursively(false);
    }

}

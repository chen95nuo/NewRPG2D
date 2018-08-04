using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRemoveAllListener : MonoBehaviour {

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveAllListener();
    }
}

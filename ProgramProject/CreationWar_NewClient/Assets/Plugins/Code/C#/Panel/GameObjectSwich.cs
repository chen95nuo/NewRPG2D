using UnityEngine;
using System.Collections;

public class GameObjectSwich : MonoBehaviour {

    public GameObject targe;
    public bool isChild;
    public bool isStartSwich;

    void Start()
    {
        if (isChild)
        {
            targe.SetActiveRecursively(isStartSwich);
        }
        else
        {
            targe.active = isStartSwich;
        }
    }

    void OnClick()
    {
        if (isChild)
        {
            targe.SetActiveRecursively(!targe.active);
        }
        else
        {
            targe.active = targe.active ? false : true;
        }
    }

}

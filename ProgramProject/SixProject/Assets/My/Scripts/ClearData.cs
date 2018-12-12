using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearData : MonoBehaviour {

    public void Cleardata()
    {
        PlayerPrefs.DeleteAll();//删除所有存档
    }
}

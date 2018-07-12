using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class GameMain : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        TTUIPage.ShowPage<UIMainPage>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveAllListener();
    }

}

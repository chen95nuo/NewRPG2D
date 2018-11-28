using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;

public class LoginScene : MonoBehaviour
{

    private void Awake()
    {
        UIPanelManager.instance.ShowPage<UILogin>();
    }
}

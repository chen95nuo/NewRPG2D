using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class GameMain : MonoBehaviour
{
    public static GameMain Instance = null;

    public GameObject MapCanvas;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        //读取存档的信息;
        ReadJsonNewMgr.CreateInstance();
        GoFightMgr.CreateInstance();

        GameObject go = Instantiate(MapCanvas) as GameObject;
        go.SetActive(true);
    }

    // Use this for initialization
    void Start()
    {
        TTUIPage.ShowPage<UIMainPage>();
        if (GoFightMgr.instance.isRound)
        {
            TinyTeam.UI.TTUIPage.ShowPage<UIRoundPage>();
            GoFightMgr.instance.isRound = false;
        }
        //DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveAllListener();
        //ReadJsonNewMgr.DestroyInstance();
        //GoFightMgr.DestroyInstance();
    }

}

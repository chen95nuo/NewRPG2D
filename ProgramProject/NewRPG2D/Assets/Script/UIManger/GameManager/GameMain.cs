using Assets.Script.UIManger;
using Assets.Script.Utility;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    public static GameMain Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Init();


    }

    public void Init()
    {
        UIPanelManager.instance.ShowPage<UILoading>();
        Invoke("test", 0.5f);
    }

    private void test()
    {
        Invoke("test_1", 0.5f);
        Invoke("test_2", 1.0f);
    }

    private void test_1()
    {
        ChickPlayerInfo.instance.ChickBuilding();
    }

    private void test_2()
    {
        LocalServer.instance.StartInit();
    }

}

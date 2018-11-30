using Assets.Script.Timer;
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
        Invoke("test", 1f);
    }

    private void test()
    {

    }
}

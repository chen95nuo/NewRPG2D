using UnityEngine;
using System.Collections;

public class ChangeGuildPanel : MonoBehaviour {
    public GameObject guilds;
    public GameObject myGuild;
    private AutoSetFields asf;

    void Start()
    {
        asf = GetComponent<AutoSetFields>();
        if (null == asf)
        {
            Debug.LogError("ChangeGuildPanel :: Start() - AutoSetFields component is not!");
        }
    }

    /// <summary>
    /// 切换到公会目录面板
    /// </summary>
    public void SwitchToGuildsPanel()
    {
        guilds.SetActive(true);
        myGuild.SetActive(false);
    }

    /// <summary>
    /// 切换到我的公会面板
    /// </summary>
    void SwitchToMyGuildPanel()
    {
        guilds.SetActive(false);
        myGuild.SetActive(true);
    }

    /// <summary>
    /// 隐藏我的公会面板
    /// </summary>
    void HideMyGuildsPanel() 
    {
        myGuild.SetActive(false);
    }
}

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
    /// �л�������Ŀ¼���
    /// </summary>
    public void SwitchToGuildsPanel()
    {
        guilds.SetActiveRecursively(true);
        myGuild.SetActiveRecursively(false);
    }

    /// <summary>
    /// �л����ҵĹ������
    /// </summary>
    void SwitchToMyGuildPanel()
    {
        guilds.SetActiveRecursively(false);
        myGuild.SetActiveRecursively(true);
    }

    /// <summary>
    /// �����ҵĹ������
    /// </summary>
    void HideMyGuildsPanel() 
    {
        myGuild.SetActiveRecursively(false);
    }
}

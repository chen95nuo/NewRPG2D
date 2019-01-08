/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;

public class UserLicensingAgreement : MonoBehaviour 
{
    public GameObject announcementPanelObj;
    public UILabel licensingContent;
    public GameObject[] objs;
    private string gameVersion;

    void Awake()
    {
        //PlayerPrefs.SetString("GameVersion", "1.2.6");
        gameVersion = YuanUnityPhoton.GameVersion.ToString();
        if (gameVersion.Equals(PlayerPrefs.GetString("GameVersion", "NoVersion")))
        {
            gameObject.SetActive(false);
        }
        else
        {
            announcementPanelObj.transform.localScale = Vector3.zero;

            gameObject.transform.localScale = Vector3.one;
        }
    }

    void OnEnable()
    {
        foreach (GameObject go in objs)
        {
            if (go.activeSelf)
            {
                go.SetActive(false);
            }
        }
    }

	void Start () 
	{
        TextAsset ta = Resources.Load("UserLicensingAgreement") as TextAsset;
        //Debug.Log(ta.text);
        licensingContent.text = ta.text;
	}

    void AgreeAndClosePanel()
    {
        this.gameObject.SetActive(false);
        announcementPanelObj.SetActive(true);
        announcementPanelObj.transform.localScale = Vector3.one;
        PlayerPrefs.SetString("GameVersion", gameVersion);
    }
}

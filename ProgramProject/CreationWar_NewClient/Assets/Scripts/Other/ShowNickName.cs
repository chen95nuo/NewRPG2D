using UnityEngine;
using System.Collections;

public class ShowNickName : MonoBehaviour {

	UIPopupList playerTitle;

	void Awake()
	{
        playerTitle = GetComponent<UIPopupList>();

        //EventDelegate.Callback call = () => PanelStatic.StaticBtnGameManager.CmbSelectPlayerTitle(playerTitle.gameObject, null);

        //playerTitle.onChange.Add(new EventDelegate(() => PanelStatic.StaticBtnGameManager.CmbSelectPlayerTitle(playerTitle.gameObject, null)));
       
	}

    void Start()
    {	
    }

    void OnEnable()
    {
        PanelStatic.StaticBtnGameManager.GetPlayerTitle(playerTitle);
        playerTitle.value = BtnGameManager.yt[0]["SelectTitle"].YuanColumnText;
    }

    public void ChangeTitle()
    {
        PanelStatic.StaticBtnGameManager.CmbSelectPlayerTitle(playerTitle.gameObject, null);
    }
}

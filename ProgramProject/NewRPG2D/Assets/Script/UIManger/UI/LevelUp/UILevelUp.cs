using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public abstract class UILevelUp : TTUIPage
{
    public Text txt_Name;
    public Text txt_Level;
    public Button btn_back;
    public Button btn_BgBack;
    public GameObject levelUpHelper;
    private UILevelUpHelper btnLevel;

    private void Awake()
    {
        btn_back.onClick.AddListener(ClosePage);
        btn_BgBack.onClick.AddListener(ClosePage);

        if (btnLevel == null)
        {
            GameObject go = Instantiate(levelUpHelper, transform) as GameObject;
            btnLevel = go.GetComponent<UILevelUpHelper>();
        }

        btnLevel.btn_LevelUp.onClick.AddListener(ClosePage);
        btnLevel.btn_NowUp.onClick.AddListener(ClosePage);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        UpdateInfo(mData as RoomMgr);
    }

    private void UpdateInfo(RoomMgr data)
    {
        txt_Name.text = LanguageDataMgr.instance.GetRoomName(data.BuildingData.RoomName.ToString());
        txt_Level.text = data.BuildingData.Level.ToString();
        btnLevel.Init();
        btnLevel.UpdateUIfo(data);
        Init(data);
    }

    protected virtual void Init(RoomMgr data) { }

    protected virtual void ChickClose()
    {
        System.Type type = GetType();
        UIPanelManager.instance.ClosePage(type);
    }

}

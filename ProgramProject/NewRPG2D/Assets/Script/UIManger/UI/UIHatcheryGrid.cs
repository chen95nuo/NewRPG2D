using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHatcheryGrid : MonoBehaviour
{

    public Button btn_hatchery;
    public Button btn_speed;
    public Button btn_complete;
    public Text timeText;
    public Button btn_UnLock;

    private HatcheryData hatchery;

    public Sprite gridSprite;

    private void Awake()
    {
        btn_complete.onClick.AddListener(ChickComplete);
    }
    private void Update()
    {
        if (hatchery != null && hatchery.HatcheryType == HatcheryType.Run)
        {
            SystemTime.insatnce.TimeNormalized(hatchery.NeedTime, timeText);
            if (hatchery.NeedTime <= 0)
            {
                SystemTime.insatnce.TimeNormalized(0, timeText);
                hatchery.HatcheryType = HatcheryType.End;
                HatcheryEnd();
            }
        }
    }

    public void UpdateGrid()
    {
        if (hatchery != null && hatchery.HatcheryType == HatcheryType.Open)
        {
            btn_UnLock.gameObject.SetActive(false);
            btn_speed.gameObject.SetActive(false);
            btn_complete.gameObject.SetActive(false);
            timeText.gameObject.SetActive(false);
            btn_hatchery.interactable = true;
        }
        else
        {
            btn_UnLock.gameObject.SetActive(true);
            btn_speed.gameObject.SetActive(false);
            btn_complete.gameObject.SetActive(false);
            timeText.gameObject.SetActive(false);
            btn_hatchery.interactable = false;
        }
    }
    public void UpdateGrid(HatcheryData data)
    {
        hatchery = data;
        btn_UnLock.gameObject.SetActive(false);
        btn_speed.gameObject.SetActive(false);
        btn_complete.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        btn_hatchery.interactable = true;
    }
    /// <summary>
    /// 孵化开始
    /// </summary>
    /// <param name="data"></param>
    public void UpdateGrid(EggData data)
    {
        data.ItemNumber--;
        hatchery.Eggdata = data;
        hatchery.HatcheryType = HatcheryType.Run;
        hatchery.EndTime = SystemTime.insatnce.GetTime().AddSeconds(data.HatchingTime).ToFileTime();
        btn_hatchery.interactable = false;
        btn_hatchery.image.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
        timeText.gameObject.SetActive(true);
        btn_speed.gameObject.SetActive(true);
    }
    /// <summary>
    /// 孵化完成
    /// </summary>
    private void HatcheryEnd()
    {
        timeText.gameObject.SetActive(false);
        btn_speed.gameObject.SetActive(false);
        btn_complete.gameObject.SetActive(true);
    }

    /// <summary>
    /// 点击孵化完成 弹出奖励菜单
    /// </summary>
    public void ChickComplete()
    {
        TinyTeam.UI.TTUIPage.ShowPage<UIRoleTipPage>();
        CardData data = GameCardData.Instance.GetItem(hatchery.Eggdata.Id);
        UIEventManager.instance.SendEvent<CardData>(UIEventDefineEnum.UpdateRoleTipEvent, data);
        btn_hatchery.image.sprite = gridSprite;
        hatchery.HatcheryType = HatcheryType.Open;
        UpdateGrid();
    }

}

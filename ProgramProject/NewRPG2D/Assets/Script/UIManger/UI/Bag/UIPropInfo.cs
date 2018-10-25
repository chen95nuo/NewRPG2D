using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPropInfo : TTUIPage
{
    public Button btn_Close;

    public Text txt_Name;
    public Text txt_Quality;
    public Text txt_Message;
    public Text txt_Tip_1;
    public Text txt_GetType;

    public Image image_Icon;
    public Image image_IconBG;
    public Image image_GetIcon;

    public override void Show(object mData)
    {
        base.Show(mData);
        PropData data = mData as PropData;
        UpdateInfo(data);
    }

    private void Awake()
    {
        btn_Close.onClick.AddListener(ClosePage);
        txt_Tip_1.text = "获取方式";
    }

    public void UpdateInfo(PropData data)
    {
        txt_Name.text = data.ItemName;
        txt_Quality.text = data.quality.ToString();
        txt_Message.text = data.des;

        image_Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.SpriteName);
        image_IconBG.sprite = GetSpriteAtlas.insatnce.GetIcon("Quality_" + data.quality.ToString());
        image_GetIcon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.getAccess.ToString());
    }
}

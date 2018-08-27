using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITalk : MonoBehaviour
{

    public Image iconImage;
    public Text name;
    public Text describe;
    public Button btn_Next;
    public Sprite[] Icons;

    private UITalkType type;

    private void Awake()
    {
        UIEventManager.instance.AddListener<UITalkType>(UIEventDefineEnum.UpdateTalkEvent, UpdateTalkPage);

        btn_Next.onClick.AddListener(GoNextPage);
    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<UITalkType>(UIEventDefineEnum.UpdateTalkEvent, UpdateTalkPage);
    }

    private void UpdateTalkPage(UITalkType type)
    {
        this.type = type;
        switch (type)
        {
            case UITalkType.Nothing:
                break;
            case UITalkType.Furnace:
                //iconImage.sprite = Icons[0];
                name.text = "炼金术士";
                describe.text = "想要增加魔物实力嘛？来找我吧!";
                break;
            case UITalkType.Store:
                //iconImage.sprite = Icons[1];
                name.text = "商店";
                describe.text = "挑一个你喜欢的东西吧!";
                break;
            case UITalkType.Explore:
                //iconImage.sprite = Icons[2];
                name.text = "探险家";
                describe.text = "你想去哪里?";
                break;
            case UITalkType.EggStore:
                //iconImage.sprite = Icons[3];
                name.text = "扭蛋";
                describe.text = "试试运气吧!";
                break;
            default:
                break;
        }
    }

    private void GoNextPage()
    {
        TinyTeam.UI.TTUIPage.ClosePage<UITalkPage>();
        switch (type)
        {
            case UITalkType.Nothing:
                break;
            case UITalkType.Furnace:
                TinyTeam.UI.TTUIPage.ShowPage<UIFurnacePage>();
                break;
            case UITalkType.Store:
                TinyTeam.UI.TTUIPage.ShowPage<UIStorePage>();
                break;
            case UITalkType.Explore:
                TinyTeam.UI.TTUIPage.ShowPage<UIExplorePage>();
                break;
            case UITalkType.EggStore:
                TinyTeam.UI.TTUIPage.ShowPage<UIEggStore>();
                break;
            default:
                break;
        }
    }

}

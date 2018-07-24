using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UITipPage : TTUIPage
{
    private Text tipText;
    private Text mainText;
    private Button trueButton;
    private Button falseButton;
    private TipType tipType;

    public UITipPage() : base(UIType.Normal, UIMode.NeedBack, UICollider.None)
    {
        uiPath = "UIPrefab/UITip";
    }
    public override void Awake(GameObject go)
    {
        Init();

        UIEventManager.instance.AddListener<int>(UIEventDefineEnum.UpdateExploreTipEvent, ExploreEvent);
    }

    private void Init()
    {
        tipText = this.transform.Find("tip").GetComponent<Text>();
        mainText = this.transform.Find("mainTip").GetComponent<Text>();
        trueButton = this.transform.Find("true").GetComponent<Button>();
        falseButton = this.transform.Find("false").GetComponent<Button>();
        trueButton.onClick.AddListener(IsTrue);
        falseButton.onClick.AddListener(IsFalse);
    }

    public void ExploreEvent(int price)
    {
        tipType = TipType.Explore;
        ShowTip(price);
    }

    public void ShowTip(int price)
    {
        switch (tipType)
        {
            case TipType.Nothing:
                tipText.text = "error!";
                mainText.text = "error!";
                break;
            case TipType.Explore:
                tipText.text = "提示";
                mainText.text = $"确定使用{price}金币开启新的队伍吗？";
                break;
            default:
                break;
        }
    }

    public void IsTrue()
    {
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateExploreTipEvent, true);
        ClosePage<UITipPage>();
    }
    public void IsFalse()
    {
        ClosePage<UITipPage>();
    }
}

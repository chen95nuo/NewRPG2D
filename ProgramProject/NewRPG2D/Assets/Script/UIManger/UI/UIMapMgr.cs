/*每次鼠标抬起进行动画判定，鼠标按下则进行拖动，在动画完成之前无法进行拖动*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using TinyTeam.UI;

public class UIMapMgr : MonoBehaviour
{

    public ScrollRect m_ScrollRect;
    public float m_Value;
    public Transform m_content;
    private float mTargetValue;
    private bool mNeedMove = false;
    private const float MOVE_SPEED = 0.1F;
    public float SMOOTH_TIME = 0.2F;
    private float mMoveSpeed = 0;
    private float imageNumber = 0;
    private float currentTime = 0;
    public int pageNumber = 0;
    public Ease style;


    public Button btn_MapType;//地图选择
    public Button btn_Round;//关卡选择
    public Button btn_Email;//邮箱
    public Button btn_Furnace;//熔炉
    public Button btn_EggStore;//扭蛋机
    public Button btn_Store;//超市
    public Button btn_Reward;//任务板
    public Button btn_Explore;//探险
    private Vector2 first;
    private Vector2 second;

    private void Awake()
    {
        Input.multiTouchEnabled = false;//关闭多点触控

        imageNumber = 1.0f / (m_content.childCount - 1.0f);

        btn_MapType.onClick.AddListener(ShowMapType);
        btn_Round.onClick.AddListener(ShowRound);
        btn_Email.onClick.AddListener(ShowEmail);
        btn_Furnace.onClick.AddListener(ShowFurnace);
        btn_EggStore.onClick.AddListener(ShowEggStore);
        btn_Store.onClick.AddListener(ShowStore);
        btn_Reward.onClick.AddListener(ShowReward);
        btn_Explore.onClick.AddListener(ShowExplore);

        m_ScrollRect.horizontalNormalizedPosition = 0;
    }

    private void Start()
    {
        mTargetValue = 0;
        mNeedMove = true;
    }

    private void ShowMapType() { TTUIPage.ShowPage<UIMapTypePage>(); }
    private void ShowRound() { TTUIPage.ShowPage<UIRoundPage>(); }
    private void ShowEmail() { TTUIPage.ShowPage<UIEmailPage>(); }
    private void ShowFurnace() { ShowFurnacePage(); }
    private void ShowEggStore() { ShowEggStorePage(); }
    private void ShowStore() { ShowStorePage(); }
    private void ShowReward() { TTUIPage.ShowPage<UIRewardPage>(); }
    private void ShowExplore() { ShowExplorePage(); }



    public void OnPointerUp()
    {
        m_Value = m_ScrollRect.horizontalNormalizedPosition;

        // 判断当前位于哪个区间，设置自动滑动至的位置
        if (m_Value <= 1 && mTargetValue + 0.03f < m_Value)
        {
            //下一页
            pageNumber++;
        }
        else if (m_Value > 0 && mTargetValue - 0.03f > m_Value)
        {
            //上一页
            pageNumber--;
        }
        else
        {
        }

        mTargetValueNormalized(pageNumber);

    }

    private void mTargetValueNormalized(int number)
    {
        Mathf.Clamp(number, 0, 4);
        switch (number)
        {
            case 0: mTargetValue = 0; break;
            case 1: mTargetValue = 0.33f; break;
            case 2: mTargetValue = 0.67f; break;
            case 3: mTargetValue = 1; break;
            default:
                break;
        }
        m_ScrollRect.DOHorizontalNormalizedPos(mTargetValue, SMOOTH_TIME);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            OnPointerUp();
        }
    }

    private void ShowFurnacePage()
    {
        TTUIPage.ShowPage<UITalkPage>();
        UIEventManager.instance.SendEvent<UITalkType>(UIEventDefineEnum.UpdateTalkEvent, UITalkType.Furnace);
    }

    private void ShowStorePage()
    {
        TTUIPage.ShowPage<UITalkPage>();
        UIEventManager.instance.SendEvent<UITalkType>(UIEventDefineEnum.UpdateTalkEvent, UITalkType.Store);
    }

    private void ShowExplorePage()
    {
        TTUIPage.ShowPage<UITalkPage>();
        UIEventManager.instance.SendEvent<UITalkType>(UIEventDefineEnum.UpdateTalkEvent, UITalkType.Explore);
    }

    private void ShowEggStorePage()
    {
        TTUIPage.ShowPage<UITalkPage>();
        UIEventManager.instance.SendEvent<UITalkType>(UIEventDefineEnum.UpdateTalkEvent, UITalkType.EggStore);
    }
}
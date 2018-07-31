using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using TinyTeam.UI;

public class UIMapMgr : MonoBehaviour
{

    public Scrollbar m_Scrollbar;
    public ScrollRect m_ScrollRect;
    public Transform m_content;
    private float mTargetValue;
    private bool mNeedMove = false;
    private const float MOVE_SPEED = 1.5F;
    private const float SMOOTH_TIME = 0.2F;
    private float mMoveSpeed = 0f;
    private float imageNumber = 0;


    public Button btn_MapType;//地图选择
    public Button btn_Round;//关卡选择
    public Button btn_Email;//邮箱
    public Button btn_Furnace;//熔炉
    public Button btn_EggStore;//扭蛋机
    public Button btn_Store;//超市
    public Button btn_Reward;//任务板
    public Button btn_Explore;//探险

    private void Awake()
    {
        imageNumber = 1.0f / (m_content.childCount - 1.0f);

        btn_MapType.onClick.AddListener(ShowMapType);
        btn_Round.onClick.AddListener(ShowRound);
        btn_Email.onClick.AddListener(ShowEmail);
        btn_Furnace.onClick.AddListener(ShowFurnace);
        btn_EggStore.onClick.AddListener(ShowEggStore);
        btn_Store.onClick.AddListener(ShowStore);
        btn_Reward.onClick.AddListener(ShowReward);
        btn_Explore.onClick.AddListener(ShowExplore);
    }

    private void Start()
    {
        mTargetValue = 0;
        mNeedMove = true;
        mMoveSpeed = 0;
    }

    private void ShowMapType() { }
    private void ShowRound() { TTUIPage.ShowPage<UIRoundPage>(); }
    private void ShowEmail() { TTUIPage.ShowPage<UIEmailPage>(); }
    private void ShowFurnace() { TTUIPage.ShowPage<UIFurnacePage>(); }
    private void ShowEggStore() { TTUIPage.ShowPage<UIEggStore>(); }
    private void ShowStore() { TTUIPage.ShowPage<UIStorePage>(); }
    private void ShowReward() { TTUIPage.ShowPage<UIRewardPage>(); }
    private void ShowExplore() { TTUIPage.ShowPage<UIExplorePage>(); }



    public void OnPointerUp()
    {
        //// 判断当前位于哪个区间，设置自动滑动至的位置
        if (m_Scrollbar.value <= 1 && mTargetValue + 0.03f < m_Scrollbar.value)
        {
            mTargetValue += imageNumber;
        }
        else if (m_Scrollbar.value > 0 && mTargetValue - 0.03f > m_Scrollbar.value)
        {
            mTargetValue -= imageNumber;
        }
        else
        {

        }
        mNeedMove = true;
        mMoveSpeed = 0;
    }

    void Update()
    {

        if (Input.GetMouseButtonUp(0) && mNeedMove == false)
        {
            OnPointerUp();
        }

        if (mNeedMove)
        {
            if (Mathf.Abs(m_Scrollbar.value - mTargetValue) < 0.001f)
            {
                m_Scrollbar.value = mTargetValue;
                mNeedMove = false;
                return;
            }
            m_Scrollbar.value = Mathf.SmoothDamp(m_Scrollbar.value, mTargetValue, ref mMoveSpeed, SMOOTH_TIME);
        }
    }
}
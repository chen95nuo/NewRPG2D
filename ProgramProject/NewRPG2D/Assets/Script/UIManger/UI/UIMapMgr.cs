using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Plugins.Options;

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

    private void Awake()
    {
        imageNumber = 1.0f / (m_content.childCount - 1.0f);
    }

    private void Start()
    {
        mTargetValue = 0;
        mNeedMove = true;
        mMoveSpeed = 0;
    }

    public void OnPointerUp()
    {
        //// 判断当前位于哪个区间，设置自动滑动至的位置
        if (mTargetValue + 0.03f < m_Scrollbar.value)
        {
            mTargetValue += imageNumber;
        }
        else if (mTargetValue - 0.03f > m_Scrollbar.value)
        {
            mTargetValue -= imageNumber;
        }

        mNeedMove = true;
        mMoveSpeed = 0;
    }

    void Update()
    {
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
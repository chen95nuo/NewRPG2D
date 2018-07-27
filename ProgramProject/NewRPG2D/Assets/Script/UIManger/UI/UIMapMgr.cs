using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMapMgr : MonoBehaviour
{

    public Scrollbar m_Scrollbar;
    public ScrollRect m_ScrollRect;
    public Transform m_content;

    private float mTargetValue;

    private bool mNeedMove = false;

    private const float MOVE_SPEED = 1F;

    private const float SMOOTH_TIME = 0.2F;

    private float mMoveSpeed = 0f;

    private float imageNumber = 0;

    private void Awake()
    {
        imageNumber = 1.0f / (m_content.childCount - 1.0f);
    }

    public void OnPointerUp()
    {// 1. 0.03 - 0.30 2. 0.36 - 0.63 3. 0.69-0.96
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
            if (Mathf.Abs(m_Scrollbar.value - mTargetValue) < 0.01f)
            {
                m_Scrollbar.value = mTargetValue;
                mNeedMove = false;
                return;
            }
            m_Scrollbar.value = Mathf.SmoothDamp(m_Scrollbar.value, mTargetValue, ref mMoveSpeed, SMOOTH_TIME);
        }
    }

}
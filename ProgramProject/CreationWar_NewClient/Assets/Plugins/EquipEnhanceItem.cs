/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;

public class EquipEnhanceItem : MonoBehaviour 
{
    public float delayTime = 2.0f;
    public float speed = 2.0f;
    public float moveDistance = 1.0f;

    public UISprite stenghenSprite;
    public UISprite resultSprite;
	
	void OnEnable()
    {
        //StartCoroutine(DelayHide());
    }

    IEnumerator DelayHide(bool isSuccess)
    {
        yield return new WaitForSeconds(1.0f);
        float totalTime = 0;
        float alpha = 0;
        Vector3 endPos = Vector3.zero;
        if (isSuccess)
        {
            endPos = this.transform.localPosition + Vector3.up * moveDistance;
        }
        else
        {
            endPos = this.transform.localPosition - Vector3.up * moveDistance;
        }

        while(totalTime <= delayTime)
        {
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, endPos, Time.deltaTime);
            alpha = resultSprite.alpha;
            alpha = Mathf.Lerp(alpha, 0, Time.deltaTime * 0.8f);
            stenghenSprite.alpha = alpha;
            resultSprite.alpha = alpha;

            totalTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(false);
        stenghenSprite.alpha = 1;
        resultSprite.alpha = 1;
    }

    /// <summary>
    /// 改变显示文字，成功或失败
    /// </summary>
    /// <param name="isSuccess">是否成功</param>
    public void ChangeSprite(bool isSuccess)
    {
        if (isSuccess)
        {
            resultSprite.spriteName = "qh_win";
        }
        else
        {
            resultSprite.spriteName = "qh_loss";
        }

        StartCoroutine(DelayHide(isSuccess));
    }

    public void SetAlpha()
    {
        stenghenSprite.alpha = 1;
        resultSprite.alpha = 1;
    }
}

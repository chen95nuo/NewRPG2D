using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnimTools
{

    private static UIAnimTools instance;

    public static UIAnimTools Instance
    {

        get
        {
            if (instance == null)
            {
                instance = new UIAnimTools();
            }
            return instance;
        }
    }

    /// <summary>
    /// 动画播放
    /// </summary>
    /// <param name="anim">动画</param>
    /// <param name="name">动画名称</param>
    /// <param name="run">是否倒放 f是正</param>
    public void PlayAnim(Animation anim, string name, bool run)
    {
        Debug.Log(name);
        anim.gameObject.SetActive(true);
        Debug.Log(name);
        if (run)
        {
            anim[name].speed = -1;
            anim[name].time = anim[name].length;
            anim.Play(name);
        }
        else
        {
            anim[name].speed = 1;
            anim.Play(name);
        }
    }
    public void PlayAnim(Animation anim, string name)
    {
        anim.gameObject.SetActive(true);
        anim[name].speed = 1;
        anim.Play(name);
    }

    /// <summary>
    /// 播放背景动画
    /// </summary>
    /// <param name="image"></param>
    /// <param name="run">是否倒放 f是开</param>
    public void GetBG(Image image, bool run)
    {
        if (run)
        {
            image.DOFade(0f, 2f);
        }
        else
        {
            Debug.Log("运行了");
            image.DOFade(0.5f, 2f);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="run"></param>
    /// <param name="time">时间 f是开</param>
    public void GetBG(Image image, bool run, float time)
    {
        if (run)
        {
            image.DOFade(0f, time);
        }
        else
        {
            image.DOFade(0.5f, time);
        }
    }

    public string ItemNameColor(int level, string name)
    {
        string color = "";
        switch (level)
        {
            case 1:
                color = "A59791";
                break;
            case 2:
                color = "DA9C60";
                break;
            case 3:
                color = "AAD3D6";
                break;
            case 4:
                color = "FAC83E";
                break;
            default:
                break;
        }

        return "<color=#" + color + ">" + name + "</color>";
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class tst : MonoBehaviour
{
    public Image icon;

    private bool isRun = false;
    private Tweener tween;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRun = !isRun;
            GridAnim(isRun);
        }
    }

    public void GridAnim(bool isRun)
    {
        if (isRun)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(Vector3.one * 1.2f, 0.5f));
            sequence.AppendCallback(TweenCallBack);
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(Vector3.one, 0.5f));
            sequence.AppendCallback(TweenCallBack);
        }
    }

    public void TweenCallBack()
    {
        if (isRun)
        {
            tween = transform.DOShakePosition(0.2f, new Vector3(10, 0, 0), 5, 0, true).SetLoops(-1, LoopType.Incremental);
            tween.SetAutoKill(false);
        }
        else
        {
            tween.Kill(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UILevelUpTipGrid : MonoBehaviour
{
    public Text txt_time;
    public Transform ts;
    private Canvas canvas;

    private void Awake()
    {
        HallEventManager.instance.AddListener(HallEventDefineEnum.CameraMove, UpdatePos);
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.CameraMove, UpdatePos);
    }

    public void GetInfo(Transform transform, Canvas canvas)
    {
        ts = transform;
        this.canvas = canvas;
        UpdatePos();
    }

    public void UpdatePos()
    {
        if (transform.position.z >= 100)
        {
            return;
        }
        Vector2 pos;
        RectTransform rt = canvas.transform as RectTransform;
        Vector3 v3 = Camera.main.WorldToScreenPoint(ts.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, v3, canvas.worldCamera, out pos);
        RectTransform rect = transform.transform as RectTransform;
        rect.anchoredPosition = pos;
    }

    public void UpdateTime(int time)
    {
        SystemTime.instance.TimeNormalized(time, txt_time);
    }
}

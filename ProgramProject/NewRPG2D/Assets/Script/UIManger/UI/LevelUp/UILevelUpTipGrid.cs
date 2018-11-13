using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UILevelUpTipGrid : MonoBehaviour
{
    public Text txt_time;
    [System.NonSerialized]
    public Transform ts;
    private Canvas canvas;
    public Image slider;
    private Vector3 point;
    public Image icon;
    public Text txt_Tip;

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
        point = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        this.canvas = canvas;
        UpdatePos();
    }

    public void UpdatePos()
    {
        if (transform.position.z >= 100 || MapControl.instance.type == CastleType.edit)
        {
            return;
        }
        Vector2 pos;
        RectTransform rt = canvas.transform as RectTransform;
        Vector3 v3 = Camera.main.WorldToScreenPoint(point);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, v3, canvas.worldCamera, out pos);
        RectTransform rect = transform.transform as RectTransform;
        rect.anchoredPosition = pos;
    }

    public void UpdateTime(LevelUPHelper data)
    {
        if (MapControl.instance.type == CastleType.edit)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        slider.fillAmount = (float)(data.allTime - data.needTime) / (float)data.allTime;
        txt_time.text = SystemTime.instance.TimeNormalizedOf(data.needTime, false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TinyTeam.UI;

public class UILittleTip : MonoBehaviour
{
    private Canvas canvas;
    private RectTransform rt;
    private Text text;
    private bool isRun = false;
    private GameObject go;
    private bool isClose = false;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    private void Awake()
    {
        canvas = TTUIRoot.Instance.GetComponent<Canvas>();
        rt = GetComponent<RectTransform>();
        text = transform.GetComponentInChildren<Text>();

        UIEventManager.instance.AddListener<string>(UIEventDefineEnum.UpdateLittleTipEvent, UpdateMessage);
        UIEventManager.instance.AddListener<bool>(UIEventDefineEnum.UpdateLittleTipEvent, CloseMessage);
        xMin = -(Screen.width / 2.0f);
        xMax = Screen.width / 2.0f;
        yMin = -(Screen.height / 2.0f);
        yMax = Screen.height / 2.0f;
    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<string>(UIEventDefineEnum.UpdateLittleTipEvent, UpdateMessage);
        UIEventManager.instance.RemoveListener<bool>(UIEventDefineEnum.UpdateLittleTipEvent, CloseMessage);
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            isRun = true;
            if (isClose == false)
            {
                TTUIPage.ClosePage<UILittleTipPage>();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isRun = false;
        }
        if (isRun)
        {
            Vector2 _pos = Vector2.one;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                        Input.mousePosition, canvas.worldCamera, out _pos);
            float index_X = rt.sizeDelta.x / 2;
            float index_Y = rt.sizeDelta.y / 2;

            _pos = new Vector2(Mathf.Clamp(_pos.x, xMin + index_X, xMax - index_X), Mathf.Clamp(_pos.y, yMin + index_Y, yMax - index_Y));
            rt.anchoredPosition = _pos;

        }
    }

    private void OnEnable()
    {
        isRun = true;
    }

    private void UpdateMessage(string message)
    {
        text.text = message;
        isClose = true;
    }

    private void CloseMessage(bool isClose)
    {
        this.isClose = isClose;
    }
}

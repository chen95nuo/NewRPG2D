using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSpeedControl : MonoBehaviour
{
    public InputField txt_moveSpeed;
    public InputField txt_scaleSpeed;
    public Slider move;
    public Slider scale;
    public Text txt_move;
    public Text txt_scale;
    public Text txt_Tip;

    private void Awake()
    {
        move.onValueChanged.AddListener(moveSlider);
        scale.onValueChanged.AddListener(scaleSlder);
        txt_moveSpeed.onValueChanged.AddListener(MoveTxt);
        txt_scaleSpeed.onValueChanged.AddListener(ScaleTxt);
    }

    private void ScaleTxt(string arg0)
    {
        scale.value = float.Parse(arg0);
    }

    private void MoveTxt(string arg0)
    {
        move.value = float.Parse(arg0);
    }

    private void scaleSlder(float arg0)
    {
        txt_scaleSpeed.text = arg0.ToString();
        CameraControl.instance.scaleSpeed = scale.value;
    }

    private void moveSlider(float arg0)
    {
        txt_moveSpeed.text = arg0.ToString();
        CameraControl.instance.MoveSpeed = move.value;
    }

    // Update is called once per frame
    void Update()
    {
        txt_move.text = "移动速度: " + CameraControl.instance.MoveSpeed.ToString("#0.0");
        txt_scale.text = "缩放速度: " + CameraControl.instance.scaleSpeed.ToString("#0.0");

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
#elif UNITY_ANDROID||UNITY_IPHONE
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
            {
                txt_Tip.text = "点击到UI了";
            }
            else
            {
                txt_Tip.text = "没点击到UI";
            }
        }

    }
}

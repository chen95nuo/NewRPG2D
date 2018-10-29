using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIChickHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    public delegate void ClickDelegate();

    private float nowTime = 0;
    private float holdTime = .5f;
    private bool isDown = false;

    public ClickDelegate click;
    // Update is called once per frame
    void Update()
    {
        if (isDown)
        {
            nowTime += Time.deltaTime;
            if (nowTime >= holdTime)
            {
                click();
                isDown = false;
                nowTime = 0;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isDown = false;
    }
}

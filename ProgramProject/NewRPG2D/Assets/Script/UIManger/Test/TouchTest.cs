using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchTest : MonoBehaviour
{

    public Text text_1;
    public Text text_2;
    public Text text_3;

    private Touch oldTouch1;
    private Touch oldTouch2;
    public float zMin = 5.4f;
    public float zMax = 28f;
    public float scaleSpeed = 1f;
    private Camera m_Camera;

    bool isMove = false;

    Vector2 m_screenpos = new Vector2();
    Vector3 oldPosition;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        oldPosition = Camera.main.transform.position;
    }

    void Update()
    {
        if (Input.touchCount != 0)
        {
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Debug.Log("Began");
                    m_screenpos = Input.touches[0].position;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Camera.main.transform.Translate(new Vector3(-Input.touches[0].deltaPosition.x * Time.deltaTime * 0.5f, -Input.touches[0].deltaPosition.y * Time.deltaTime * 0.5f, 0));

                    text_1.text = Input.GetTouch(0).rawPosition.x.ToString("#0.0") + Input.GetTouch(0).rawPosition.y.ToString("#0.0");
                    text_2.text = Input.GetTouch(0).deltaPosition.x.ToString("#0.0") + Input.GetTouch(0).deltaPosition.y.ToString("#0.0");
                    text_3.text = Input.GetTouch(0).position.x.ToString("#0.0") + Input.GetTouch(0).position.y.ToString("#0.0");
                    Debug.Log("Moved");
                    isMove = true;
                }
                if (Input.GetTouch(0).phase == TouchPhase.Stationary && isMove == false)
                {
                    Debug.Log("Stationary");
                }
                if (Input.GetTouch(0).phase == TouchPhase.Ended && isMove == false)
                {
                    Debug.Log("Ended");
                }
                if (Input.GetTouch(0).phase == TouchPhase.Canceled)
                {
                    Debug.Log("Canceled");
                }
            }
            if (Input.touchCount > 1)
            {

                // 记录两个手指的位置  
                Vector2 finger1 = new Vector2();
                Vector2 finger2 = new Vector2();

                // 记录两个手指的移动  
                Vector2 mov1 = new Vector2();
                Vector2 mov2 = new Vector2();

                for (int i = 0; i < 2; i++)
                {
                    Touch touch = Input.touches[i];

                    if (touch.phase == TouchPhase.Ended)
                        break;

                    if (touch.phase == TouchPhase.Moved)
                    {
                        isMove = true;
                        float mov = 0;
                        if (i == 0)
                        {
                            finger1 = touch.position;
                            mov1 = touch.deltaPosition;

                        }
                        else
                        {
                            finger2 = touch.position;
                            mov2 = touch.deltaPosition;

                            if (finger1.x > finger2.x)
                            {
                                mov = mov1.x;
                            }
                            else
                            {
                                mov = mov2.x;
                            }

                            if (finger1.y > finger2.y)
                            {
                                mov += mov1.y;
                            }
                            else
                            {
                                mov += mov2.y;
                            }

                            Camera.main.orthographicSize = mov * Time.deltaTime * 0.1f;
                            Camera.main.orthographicSize = (Mathf.Clamp(Camera.main.orthographicSize, zMin, zMax));
                        }
                    }
                }

                //if (Input.GetTouch(1).phase == TouchPhase.Began)
                //{
                //    Debug.Log("Began1");
                //}
                //if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                //{
                //    Debug.Log("Moved1");
                //    isMove = true;

                //}
                //if (Input.GetTouch(1).phase == TouchPhase.Stationary && isMove == false)
                //{
                //    Debug.Log("Stationary1");
                //}
                //if (Input.GetTouch(1).phase == TouchPhase.Ended && isMove == false)
                //{
                //    Debug.Log("Ended1");
                //}
                //if (Input.GetTouch(1).phase == TouchPhase.Canceled)
                //{
                //    Debug.Log("Canceled1");
                //}
            }
        }
        else
        {
            isMove = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Assets.Script.Utility;
using UnityEngine;

public class CameraTest : MonoBehaviour
{

    public Camera MCamera;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            CaptureScreenMgr.instance.CaptureCamera(MCamera, new Rect(Vector2.zero, new Vector2(100, 100)), 101);

        }
    }
}

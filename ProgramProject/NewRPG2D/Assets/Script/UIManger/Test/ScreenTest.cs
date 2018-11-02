using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTest : MonoBehaviour
{

    public int baseWidth = 1920;
    public int baseHeight = 1080;
    public Transform ts;

    private void Awake()
    {
        ts = transform;
    }
    private void Start()
    {
        float size = Camera.main.orthographicSize;
        float scale_x = Screen.width * 1f / baseWidth * 1f;
        float scale_y = Screen.height * 1f / baseHeight * 1f;
        ts.localScale = new Vector3(scale_x, scale_y, 1);
        //Camera.main.orthographicSize = size * scale_y;
    }
}

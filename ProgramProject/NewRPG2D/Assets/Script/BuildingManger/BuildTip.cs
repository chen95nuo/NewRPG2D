/*
 * 建造提示框，需要获取该空位信息，空位起点信息，房间大小
 * 用于提示房屋建造
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTip : MonoBehaviour
{

    public EmptyPoint emptyPoint;//空位信息
    public int roomSize;//房间大小

    public SpriteRenderer sr;
    public BoxCollider bc;

    private float high = 10.8f;
    private float width = 4.77f;

    // Use this for initialization
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider>();
    }

    public void UpdateTip(EmptyPoint point, int size, Vector2 startPoint)
    {
        emptyPoint = point;
        roomSize = size;
        Vector2 tsSize = new Vector2(width * size, high);
        sr.size = tsSize;
        bc.size = tsSize;
        transform.position = new Vector2(startPoint.x + (width * (point.startPoint.x + (size * 0.5f - 0.5f))), startPoint.y + (high * point.startPoint.y));
    }
}

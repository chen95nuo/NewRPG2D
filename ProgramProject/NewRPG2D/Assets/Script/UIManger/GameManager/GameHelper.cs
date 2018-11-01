using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelper : TSingleton<GameHelper>
{


    public Vector2 GetPoint(Canvas canvas, Vector3 point)
    {
        Vector2 pos;
        RectTransform rt = canvas.transform as RectTransform;
        Vector3 v3 = Camera.main.WorldToScreenPoint(point);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, v3, canvas.worldCamera, out pos);
        return pos;
    }
}

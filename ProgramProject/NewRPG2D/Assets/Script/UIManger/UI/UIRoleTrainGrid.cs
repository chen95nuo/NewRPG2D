using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleTrainGrid : MonoBehaviour
{
    private Button btn_Enter;
    public bool IsUse;//是否正在使用

    public Image image_Icon;
    private Transform ts;
    private Canvas canvas;
    [System.NonSerialized]
    public HallRole role;

    private void Awake()
    {
        HallEventManager.instance.AddListener(HallEventDefineEnum.CameraMove, UpdatePos);
        btn_Enter = GetComponent<Button>();
        btn_Enter.onClick.AddListener(ChickEnter);
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.CameraMove, UpdatePos);
    }

    /// <summary>
    /// 启动这个Icon
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="canvas"></param>
    /// <param name="role"></param>
    public void GetInfo(Transform transform, Canvas canvas, HallRole role)
    {
        this.role = role;
        IsUse = true;
        ts = transform;
        this.canvas = canvas;
        UpdatePos();
    }

    /// <summary>
    /// 关掉这个Icon
    /// </summary>
    public void RemoveInfo()
    {
        IsUse = false;
        transform.position = Vector3.up * 1000;
        role = null;
        ts = null;
    }

    public void UpdatePos()
    {
        if (IsUse == false || transform.position.z >= 100 || MapControl.instance.type == CastleType.edit)
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

    private void ChickEnter()
    {
        HallRoleMgr.instance.LevelComplete(role.RoleData);
        RemoveInfo();
    }




}

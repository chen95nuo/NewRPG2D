using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleTrainGrid : MonoBehaviour
{
    private Button btn_Enter;
    public bool IsUse;//是否正在使用

    public Image image_Icon;
    public Transform ts;
    private Canvas canvas;
    private TrainType trainType;
    private HallRole role;

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

    public void GetInfo(Transform transform, Canvas canvas, TrainType type, HallRole role)
    {
        this.trainType = type;
        this.role = role;
        IsUse = true;
        ts = transform;
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
        Vector3 v3 = Camera.main.WorldToScreenPoint(ts.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, v3, canvas.worldCamera, out pos);
        RectTransform rect = transform.transform as RectTransform;
        rect.anchoredPosition = pos;
    }

    private void ChickEnter()
    {
        IsUse = false;
        transform.position = Vector3.up * 1000;
        string s_Atr = trainType.ToString();
        RoleAttribute atr = (RoleAttribute)System.Enum.Parse(typeof(RoleAttribute), s_Atr);
        Debug.Log("升级技能为" + atr);
        role.RoleData.LevelUp(atr);
    }


}

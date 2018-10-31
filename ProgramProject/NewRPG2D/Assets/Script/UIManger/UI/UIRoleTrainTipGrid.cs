using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleTrainTipGrid : MonoBehaviour
{
    public Text txt_Time;
    public Image slider;

    private Canvas canvas;
    private HallRole role;


    private void Awake()
    {
        HallEventManager.instance.AddListener(HallEventDefineEnum.CameraMove, ChickCameraMove);
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.CameraMove, ChickCameraMove);
    }

    public void UpdateInfo(RoleTrainHelper trainData, Canvas canvas)
    {
        this.canvas = canvas;
        if (role == null || role.roleId != trainData.roleID)
        {
            HallRoleData data = HallRoleMgr.instance.GetRoleData(trainData.roleID);
            HallRole role = HallRoleMgr.instance.GetRole(data);
            this.role = role;
        }
        txt_Time.text = SystemTime.instance.TimeNormalizedOfSecond(trainData.time);
        slider.fillAmount = (float)(trainData.maxTime - trainData.time) / (float)trainData.maxTime;
        ChickCameraMove();
    }

    public void ChickCameraMove()
    {
        Vector2 pos;
        RectTransform rt = canvas.transform as RectTransform;
        Vector3 v3 = Camera.main.WorldToScreenPoint(role.TipPoint.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, v3, canvas.worldCamera, out pos);
        RectTransform rect = transform.transform as RectTransform;
        rect.anchoredPosition = pos;
    }
}

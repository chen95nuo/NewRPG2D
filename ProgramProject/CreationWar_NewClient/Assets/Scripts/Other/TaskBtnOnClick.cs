using UnityEngine;
using System.Collections;

public class TaskBtnOnClick : MonoBehaviour {
    public GameObject msgReceverObj;

    /// <summary>
    /// ���������ťʱֱ����ת���������
    /// </summary>
    void OnClick()
    {
        if (null != msgReceverObj)
        {
            msgReceverObj.SendMessage("MainTweenMove",SendMessageOptions.RequireReceiver);
            msgReceverObj.SendMessage("TaskMoveOn", SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogError("TaskBtnOnClick :: OnClick() - msgReceverObj is null!");
        }
    }
}

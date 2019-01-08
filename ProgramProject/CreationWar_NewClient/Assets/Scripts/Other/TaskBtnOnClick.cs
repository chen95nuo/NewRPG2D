using UnityEngine;
using System.Collections;

public class TaskBtnOnClick : MonoBehaviour {
    public GameObject msgReceverObj;

    /// <summary>
    /// 当点击任务按钮时直接跳转到任务面板
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

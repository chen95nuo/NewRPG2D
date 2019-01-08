using UnityEngine;
using System.Collections;

public enum PanelStaticType
{
    StaticBtnGameManager,
    StaticBtnGameManagerBack,
    StaticYuanPicManger,
    StaticWarnings,
    StaticTVMessage,
    StaticSendManager,
	StaticInvCL,
}

public class AutoAddBtnMessage : MonoBehaviour {

    public UIButtonMessage myMessage;
    public PanelStaticType panelStaticType;


    void Start()
    {
        SetField();
    }

    /// <summary>
    /// 设置变量
    /// </summary>
    public  void SetField()
    {
        System.Type staticType = typeof(PanelStatic);
        System.Reflection.FieldInfo[] temp = staticType.GetFields();
        foreach (System.Reflection.FieldInfo item in temp)
        {
            if (item.Name == this.panelStaticType.ToString())
            {
                myMessage.target = ((MonoBehaviour)item.GetValue(PanelStatic.panelStatic)).gameObject;
                break;
            }
        }
    }

}

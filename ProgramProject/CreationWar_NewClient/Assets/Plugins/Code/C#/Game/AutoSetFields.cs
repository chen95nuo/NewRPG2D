using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
[System.Serializable]
public class YuanStatic
{
    public string fieldName;
    public GameObject fieldObj;
}


public class AutoSetFields : MonoBehaviour {

    /// <summary>
    /// 
    /// </summary>
    public PanelStaticType panelStaticType;

    /// <summary>
    /// 变量集合
    /// </summary>
    public List<YuanStatic> listFields = new List<YuanStatic>();

	void Start () {
		try
		{
        SetFields();
		}
		catch(System.Exception ex)
		{
			Debug.Log (string.Format ("GameObjectName:{0},{1}",this.gameObject.name,ex.ToString ()));
		}
	}



    /// <summary>
    /// 设置变量
    /// </summary>
    public void SetFields()
    {
        System.Type staticType = typeof(PanelStatic);
        System.Reflection.FieldInfo[] temp = staticType.GetFields();
        foreach (System.Reflection.FieldInfo item in temp)
        {
            if (item.Name == this.panelStaticType.ToString())
            {

				
                System.Reflection.FieldInfo tempField;
                
                   foreach (YuanStatic itemStatic in listFields)
                   {
                       tempField = item.FieldType.GetField(itemStatic.fieldName,System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic);
                       if (tempField != null)
                       {

                           object tempCom=itemStatic.fieldObj;
                           if (tempField.FieldType != typeof(GameObject))
                           {
                               tempCom = itemStatic.fieldObj.GetComponent(tempField.FieldType);
                           }
                           if(tempCom!=null)
                           {
                              tempField.SetValue(item.GetValue(PanelStatic.panelStatic),tempCom);
                           }
                       }
                   }
                break;
            }
        }
    }
}

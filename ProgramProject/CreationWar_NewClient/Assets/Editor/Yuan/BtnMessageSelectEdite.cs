using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(BtnMessageSelect))]
public class BtnMessageSelectEdite : Editor
{

    BtnMessageSelect btnMessageSelect;
    void OnEnable()
    {
        btnMessageSelect = (BtnMessageSelect)this.target;
    }

    public GameObject NeedFindObj;

    public PanelStatic panelStatc;
    
    public List<UIButtonMessage> listObj = new List<UIButtonMessage>();

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("要选择的物体:");
        NeedFindObj = (GameObject)EditorGUILayout.ObjectField(NeedFindObj, typeof(GameObject));
        EditorGUILayout.EndHorizontal();
        if (NeedFindObj != null)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("开始寻找"))
            {
               
                UIButtonMessage[] temp = (UIButtonMessage[]) Resources.FindObjectsOfTypeAll(typeof(UIButtonMessage));
                listObj.Clear();
                foreach (UIButtonMessage item in temp)
                {
                    if (item.target == NeedFindObj)
                    {
                        listObj.Add(item);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            if (listObj.Count > 0)
            {
                

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("找到符合条件的物体:{0}个",listObj.Count));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("开始记忆组件"))
                {
                    AutoAddBtnMessage tempAuto;
                    foreach (UIButtonMessage item in listObj)
                    {
                        tempAuto=item.GetComponent<AutoAddBtnMessage>();
                        if (tempAuto == null)
                        {
                            tempAuto = item.gameObject.AddComponent<AutoAddBtnMessage>();
                        }
                        tempAuto.panelStaticType = btnMessageSelect.panelStaticType;
                        tempAuto.myMessage = item;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        SceneView.RepaintAll();
    }
}

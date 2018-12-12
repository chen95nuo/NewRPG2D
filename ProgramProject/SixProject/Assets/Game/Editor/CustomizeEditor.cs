using UnityEngine;
using System.Collections;
using UnityEditor;

namespace MadFireOn
{

    [CustomEditor(typeof(GameManager))]
    public class CustomizeEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GameManager myTarget = (GameManager)target;

            if (GUILayout.Button("Reset All"))
            {
                myTarget.ResetGameManager();
            }
            
        }

    }
}//namespace MadFireOn

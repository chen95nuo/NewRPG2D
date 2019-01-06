using UnityEngine;

[ExecuteInEditMode]
public class paramTest : MonoBehaviour
{
	public Transform traceTarget;
	void OnGUI()
	{
		if (traceTarget == null) return;
		GUI.color = Color.red;
		GUILayout.Label("distance" + Vector3.Distance(traceTarget.position, transform.position).ToString());
		GUILayout.Label("Local Rotataion:" + CachedTransform.localRotation.ToString());
	}
}
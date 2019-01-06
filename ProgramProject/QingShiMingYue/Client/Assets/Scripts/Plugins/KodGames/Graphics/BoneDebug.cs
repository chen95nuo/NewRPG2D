using UnityEngine;
using System.Collections;

namespace KodGames.Graphics
{
	[ExecuteInEditMode]
	public class BoneDebug : MonoBehaviour
	{
		void drawbone(Transform t)
		{
			foreach (Transform child in t)
			{
				float len = 0.05f;
				Vector3 loxalX = new Vector3(len, 0, 0);
				Vector3 loxalY = new Vector3(0, len, 0);
				Vector3 loxalZ = new Vector3(0, 0, len);
				loxalX = child.rotation * loxalX;
				loxalY = child.rotation * loxalY;
				loxalZ = child.rotation * loxalZ;
				UnityEngine.Debug.DrawLine(t.position * 0.1f + child.position * 0.9f, t.position * 0.9f + child.position * 0.1f, Color.white);
				UnityEngine.Debug.DrawLine(child.position, child.position + loxalX, Color.red);
				UnityEngine.Debug.DrawLine(child.position, child.position + loxalY, Color.green);
				UnityEngine.Debug.DrawLine(child.position, child.position + loxalZ, Color.blue);
				drawbone(child);
			}
		}

		void Update()
		{
			drawbone(transform);
		}
	}
}

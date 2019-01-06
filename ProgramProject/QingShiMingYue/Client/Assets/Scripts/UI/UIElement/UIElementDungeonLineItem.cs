using UnityEngine;
using System;

public class UIElementDungeonLineItem : MonoBehaviour
{
	public AutoSpriteControlBase lineButton; 
	private float originalLineButtonWidth;

	public void Awake()
	{
		originalLineButtonWidth = lineButton.width;
	}

	public void SetLinePositionAndRotation(Vector3 mapIcon1Vec3, Vector3 mapIcon2Vec3, float horizontalSpacing, float verticalSpacing)
	{
#if UNITY_EDITOR
		Awake();
#endif
		float xDistance = Math.Abs(mapIcon1Vec3.x - mapIcon2Vec3.x);
		float yDistance = Math.Abs(mapIcon1Vec3.y - mapIcon2Vec3.y);
		float distance = (float)Math.Sqrt(Math.Pow(xDistance, 2.0) + Math.Pow(yDistance, 2.0));
		float angleDirection = mapIcon1Vec3.y < mapIcon2Vec3.y ? 1 : -1;

		float x = ((mapIcon1Vec3.x < mapIcon2Vec3.x) ? mapIcon1Vec3.x : mapIcon2Vec3.x) + xDistance / 2;
		float y = ((mapIcon1Vec3.y < mapIcon2Vec3.y) ? mapIcon1Vec3.y : mapIcon2Vec3.y) + yDistance / 2;

		this.transform.localPosition = new Vector3(x, y, -0.001f);
		this.transform.localScale = Vector3.one;
		this.transform.localEulerAngles = new Vector3(0f, 0f, angleDirection * Vector3.Angle(Vector3.right, mapIcon2Vec3 - mapIcon1Vec3));

		lineButton.width = ((int)Math.Ceiling((distance - horizontalSpacing ) / originalLineButtonWidth)) * originalLineButtonWidth;
	}
}
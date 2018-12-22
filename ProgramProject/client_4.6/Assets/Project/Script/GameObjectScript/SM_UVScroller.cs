using UnityEngine;
using System.Collections;

public class SM_UVScroller : MonoBehaviour 
{
	public int targetMaterialSlot = 0;
	public float speedY = 0.5f;
	public float speedX = 0.0f;
	private float timeWentX = 0;
	private float timeWentY = 0;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		timeWentX += Time.deltaTime*speedX;
		timeWentY += Time.deltaTime*speedY;
		this.gameObject.renderer.materials[targetMaterialSlot].SetTextureOffset ("_MainTex", new Vector2(timeWentX, timeWentY));
	}
}



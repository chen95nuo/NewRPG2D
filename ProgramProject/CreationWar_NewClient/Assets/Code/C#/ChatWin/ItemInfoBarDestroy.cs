using UnityEngine;
using System.Collections;

public class ItemInfoBarDestroy : MonoBehaviour {
	
	public Camera mCamera;
	public UIPanel panel;
	// Use this for initialization
	void Start () {
		transform.parent.position=new Vector3(0,-999999,0);
	}
	
	// Update is called once per frame
	void Update () {
		DestroyBar ();
	}
	
	private RaycastHit rayHit;
	private void DestroyBar()
	{
		if(Input.GetMouseButtonDown (0))
		{
			//Debug.Log ("11111111111");
			//if(Physics.Raycast (mCamera.ViewportPointToRay(Input.mousePosition),out rayHit))
			//{
			//	Debug.Log ("2222222222222");
			//	if(rayHit.collider!=this.gameObject.collider)
			//	{
			//		Debug.Log ("33333333");
			//		transform.parent.position=new Vector3(0,-999999,0);
			//	}
			//}
			//else
			//{
			//	transform.parent.position=new Vector3(0,-999999,0);
			//}
			
			transform.parent.position=new Vector3(0,-999999,0);
			
			transform.parent.gameObject.SetActiveRecursively (false);
			panel.enabled=false;
		}
	}
}

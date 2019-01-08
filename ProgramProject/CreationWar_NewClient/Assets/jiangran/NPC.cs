using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {
	//主摄像机对象
	private Camera camera;
	//NPC名称
	private string name = "11111111111";
	//NPC模型高度
	float npcHeight;

	// Use this for initialization
	void Start () {
		//得到摄像机对象
		camera = Camera.main;
		//得到模型原始高度
		float size_y = collider.bounds.size.y;
		//得到模型缩放比例
		float scal_y = transform.localScale.y;
		//它们的乘积就是高度
		npcHeight = (size_y *scal_y) ;
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI(){
		Vector3 worldPosition = new Vector3 (transform.position.x , transform.position.y + npcHeight,transform.position.z);
		//根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
		Vector2 position = camera.WorldToScreenPoint (worldPosition);
		//得到真实NPC头顶的2D坐标
		position = new Vector2 (position.x, Screen.height - position.y);

		//计算NPC名称的宽高
		Vector2 nameSize = GUI.skin.label.CalcSize (new GUIContent(name));
		//设置显示颜色为黄色
		GUI.color  = Color.yellow;

		if(position.x>0){
		//绘制NPC名称
		GUI.Label(new Rect(position.x - (nameSize.x/2),position.y + nameSize.y ,nameSize.x,nameSize.y), name);
		
	}
	}
}

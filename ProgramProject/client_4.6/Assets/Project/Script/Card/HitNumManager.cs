using UnityEngine;
using System.Collections;

public class HitNumManager : MonoBehaviour {
	
	public float showTime = 0.5f;

	public UILabel hitNum;
	
	public GameObject posObject;
	public GameObject desObject;
	public Camera NGUICamera;
	public Camera mainCamera;
	
	public UIFont fontNum1;
	public UIFont fontNum2;
	
	public Vector3 reduceEnergyPos;
	
	
	//type = 3表示显示伤害值。type = 4表示回血值
	public void CreateHitNum(string num, int type)
	{
		Vector3 targetPos = Vector3.zero;
		//获取数字要移动的目标位置//
		if(posObject != null && NGUICamera!= null && mainCamera!= null)
		{
			//Card card = posObject.GetComponent<Card>();
			Vector3 worldPos = desObject.transform.position;
			Vector2 screenPos =  mainCamera.WorldToScreenPoint(worldPos);
			targetPos = NGUICamera.ScreenToWorldPoint(screenPos);
		}
		
		iTween.MoveTo(gameObject,iTween.Hash("position",targetPos,"time",showTime,"delay",0.3f));
		hitNum.text += num;
		if(type == 3)
		{
			hitNum.bitmapFont = fontNum1;
		}
		else if(type ==4)
		{
			hitNum.bitmapFont = fontNum2;
		}
		
		Destroy(gameObject,1f);
	}
	
	public void createReduceEnergyNum(string num)
	{
		Vector3 targetPos = Vector3.zero;
		if(NGUICamera != null && mainCamera != null)
		{
			Vector3 worldPos = new Vector3(reduceEnergyPos.x,reduceEnergyPos.y+2,reduceEnergyPos.z);
			Vector2 screenPos =  mainCamera.WorldToScreenPoint(worldPos);
			targetPos = NGUICamera.ScreenToWorldPoint(screenPos);
			
			hitNum.text += num;
			iTween.MoveTo(gameObject,iTween.Hash("position",targetPos,"time",showTime,"delay",0.3f));
			hitNum.bitmapFont = fontNum1;
			hitNum.color = new Color(1.0f,0.5f,0.0f,1.0f);
		}
		Destroy(gameObject,1f);
	}
}

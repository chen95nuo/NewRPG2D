using UnityEngine;
using System.Collections;

public class CallUp : MonoBehaviour 
{
	GameObject _MyObj = null;
	public bool needScale = false;
	public float  scaleTime = 2f;
	private float scaleNum = 1.75f;
	
	public Vector3 startMovePos;
	public Vector3 endMovePos;
	
	void Awake()
	{
		_MyObj = this.gameObject;
	}

	// Use this for initialization
	void Start ()
	{
		if(needScale)
		{
			iTween.ScaleTo(_MyObj, new Vector3(scaleNum, scaleNum, scaleNum),scaleTime);
		}
		else
		{
			_MyObj.transform.localScale = new Vector3(scaleNum,scaleNum,scaleNum);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	// callup move
	public void movePos(Vector3 startPos,Vector3 endPos,float time,iTween.EaseType easeType = iTween.EaseType.linear)
	{
		_MyObj.transform.position = startPos;
		iTween.MoveTo(_MyObj,iTween.Hash("position",endPos,"time",time,"easetype",easeType,"oncomplete","moveEnd","oncompletetarget",_MyObj));
	}
	

}

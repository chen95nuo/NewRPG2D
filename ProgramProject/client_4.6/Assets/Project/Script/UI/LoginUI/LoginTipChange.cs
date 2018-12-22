using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoginTipChange : MonoBehaviour 
{
	public UILabel processText;
	public List<UILabel> labelList;
	int listCount;
	int curIndex = 0;
	// Use this for initialization
	void Start ()
	{
		listCount = labelList.Count;
		curIndex = Random.Range(0,listCount);
		processText.text = labelList[curIndex].text;
		Invoke("change",5);
	}
	
	public void change()
	{
		if(gameObject.activeSelf)
		{
			curIndex++;
			if(curIndex >= listCount)
			{
				curIndex = 0;
			}
			processText.text = labelList[curIndex].text;
			Invoke("change",5);
		}
	}

}

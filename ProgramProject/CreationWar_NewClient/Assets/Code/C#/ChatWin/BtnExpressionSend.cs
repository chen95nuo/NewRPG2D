using UnityEngine;
using System.Collections;

public class BtnExpressionSend : MonoBehaviour {
	//[HideInInspector]
	public YuanInput input;
	//[HideInInspector]
	public string str;
	
	void Start()
	{
		if(input==null)
		{
			input=TalkLoginStatic.my.yuanInput;
		}
	}
	
	void OnClick()
	{
			if(input&&str!="")
			{
				input.Text=input.Text+str;
				//input.SendMessage("AddText",str);
			}
	}
	

}

using UnityEngine;
using System.Collections;

public class AllSpriteControl : MonoBehaviour {
	public UISprite MailSprite;
	public UISprite picRedPoint;

	public static AllSpriteControl allSC;
	public static bool isHasNewMail=false;
	// Use this for initialization
	void Start () {

		allSC = this;
		if(isHasNewMail)
		{
			ShowMailSprite();
		}
	}

	public void ClickMail(){
		MailSprite.enabled = false;
		picRedPoint.enabled=false;
		isHasNewMail=false;
	}

	public void ShowMailSprite()
	{
		MailSprite.enabled = true;
		picRedPoint.enabled=true;
		isHasNewMail=true;
	}
}

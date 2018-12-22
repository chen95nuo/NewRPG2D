using UnityEngine;
using System.Collections.Generic;

public class MissionCellInfo : MonoBehaviour {
	
	public GameObject newMark;
	public UISprite mapIcon;
	public UILabel nameLabel;
	public GameObject[] stars;
	public GameObject effect;
	public GameObject ko;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void showData(MissionData md,int starNum,GameObject target,string functionName,int newMark0,int bonus,int times)
	{
		string atlasName=CardData.getAtlas(md.bossicon);
		mapIcon.atlas=LoadAtlasOrFont.LoadHeroAtlasByName(atlasName);
		mapIcon.spriteName=md.bossicon;
		nameLabel.text=md.name;
       
		if(md.isLastMissionInZone())
		{
			nameLabel.color=new Color(255f,100f,0,255f);
		}
		else
		{
			nameLabel.color=Color.white;
		}
       
		switch(starNum)
		{
		case 0:
			stars[0].GetComponent<UISprite>().spriteName="map_star_2";
			stars[1].GetComponent<UISprite>().spriteName="map_star_2";
			stars[2].GetComponent<UISprite>().spriteName="map_star_2";
			break;
		case 1:
			stars[0].GetComponent<UISprite>().spriteName="map_star";
			stars[1].GetComponent<UISprite>().spriteName="map_star_2";
			stars[2].GetComponent<UISprite>().spriteName="map_star_2";
			break;
		case 2:
			stars[0].GetComponent<UISprite>().spriteName="map_star";
			stars[1].GetComponent<UISprite>().spriteName="map_star";
			stars[2].GetComponent<UISprite>().spriteName="map_star_2";
			break;
		case 3:
			stars[0].GetComponent<UISprite>().spriteName="map_star";
			stars[1].GetComponent<UISprite>().spriteName="map_star";
			stars[2].GetComponent<UISprite>().spriteName="map_star";
			break;
		}
		if(bonus==1)
		{
			newMark.SetActive(false);
		}
		else
		{
			newMark.SetActive(newMark0==1);
		}
		effect.SetActive(false);
		
		int canEnter=1;
		if(newMark0==2)
		{
			canEnter=0;
		}
		
		if(newMark0==2)
		{
			mapIcon.color=Color.grey;// new Color(86,79,79,255);
		}
		else
		{
			mapIcon.color=Color.white;
		}
		
		UIButtonMessage3 msg=GetComponent<UIButtonMessage3>();
		msg.target=target;
		msg.functionName=functionName;
		msg.stringParam=md.id+"-"+starNum+"-"+bonus+"-"+times+"-"+canEnter;
		
		if(md.addtasktype>0)
		{
			ko.SetActive(true);
			if(bonus==0)
			{
				ko.GetComponent<UISprite>().spriteName="ko2";
			}
			else
			{
				ko.GetComponent<UISprite>().spriteName="KO";
			}
		}
		else
		{
			ko.SetActive(false);
		}
	}
	
}

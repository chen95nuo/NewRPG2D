using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefreshMapItemStart : MonoBehaviour {
	

	public enum DuplicateMapType
	{
		Normal,
		Elite,
		Dungeon,
	}
	
	private Dictionary<string, ShowMapLeftStart> dicBtns = new Dictionary<string, ShowMapLeftStart>();
	private string duplicateString = string.Empty;
	public DuplicateMapType duplicateMapType = DuplicateMapType.Normal;

	public void Difficulty(int Dif)
	{
		if(Dif==0){
			duplicateMapType = DuplicateMapType.Normal;
		}
		if(Dif==1)
		{
			duplicateMapType = DuplicateMapType.Elite;

		}
		if(Dif==2){
			duplicateMapType = DuplicateMapType.Dungeon;
		}

	}
	// Use this for initialization
	void Start () {
		
	}
	
	void OnEnable()
	{
		if(null!=listJS)
		{
			GetBtns(listJS);
		}
	}
		
	private string[] listInfo = new string[2];
	private string duplicateID = string.Empty;
	private int duplicateStarts = 0;
	private List<GameObject> listJS;

	public void RefreshBtns(List<GameObject> list)
	{
		listJS=list;
		GetBtns (list);
	}

	public void RefreshBtns()
	{
		if(listJS!=null)
		{
			GetBtns (listJS);
		}
	}

	private string level="1";
	private void GetBtns(List<GameObject> list)
	{
		switch(duplicateMapType)
		{
		case DuplicateMapType.Normal:
			duplicateString = "DuplicateEvaNormal";
			level="1";
			break;
		case DuplicateMapType.Elite:
			duplicateString = "DuplicateEvaElite";
			level="5";
			break;
		case DuplicateMapType.Dungeon:
			duplicateString = "DuplicateEvaDungeon";
			level="2";
			break;
		}
		string[] listDuplicate=BtnGameManager.yt.Rows[0][duplicateString].YuanColumnText.Split(';');
		string strDuplicateName = string.Empty;
		string strDuplicateLevel = string.Empty;

		ShowMapLeftStart star;
		foreach(GameObject obj in list)
		{
			star = obj.GetComponent<ShowMapLeftStart>();
			star.NumStars=0;
			if(null!=star)
			{
				foreach (string item in listDuplicate)
				{
					if (item != "")
					{
						
						listInfo = item.Split(',')	;
						duplicateID = listInfo[0]	;
						if(duplicateMapType == DuplicateMapType.Elite)
						{
							for(int i=1;i<=3;i++)
							{
								if(duplicateID==star.mapID.Substring (0,2)+i.ToString ()+level)
								{
									duplicateStarts =int.Parse( listInfo[1]);
									star.NumStars=duplicateStarts;
									
									break;
								}
							}
						}
						else
						{
							if(duplicateID==star.mapID+level)
							{
								duplicateStarts =int.Parse( listInfo[1]);
								star.NumStars=duplicateStarts;

								break;
							}
						}
					}
				}
			}
		}


	}
	

}

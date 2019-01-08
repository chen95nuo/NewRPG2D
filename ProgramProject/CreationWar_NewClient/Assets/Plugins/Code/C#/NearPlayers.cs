using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NearPlayers : MonoBehaviour {

	public static NearPlayers NP;
	public GameObject   ObjBtnNear;
	public UIGrid grid;

	private List<BtnNearPlayers> listBtnNear = new List<BtnNearPlayers>();
	// Use this for initialization
	void Start () {
		NP = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		ServerRequest.requestDuelGetPlayers();
	}

	public void ShowNearPlayer(object[,] p){

			for(int i =0; i< p.GetLength(0); i++)
			{
			int num = 0;
//			if (listBtnNear.Count > num)
//			{
//				listBtnNear[num].gameObject.SetActiveRecursively(true);
//			}else{
			if(i>=grid.transform.childCount){
				GameObject obj = (GameObject)Instantiate(ObjBtnNear);
				BtnNearPlayers  tempBtn = obj.GetComponent<BtnNearPlayers>();
				tempBtn.transform.parent = grid.transform;
				tempBtn.transform.localPosition = Vector3.zero;
				tempBtn.transform.localScale = new Vector3(1, 1, 1);
				listBtnNear.Add(tempBtn);
				

				grid.repositionNow = true;
				}

				if((int)p[i , 0]==1){
				listBtnNear[i].PlayerPro.spriteName = "head-zhanshi";
				}else if((int)p[i , 0]==2){
				listBtnNear[i].PlayerPro.spriteName = "head-youxia";
				}else if((int)p[i , 0]==3){
				listBtnNear[i].PlayerPro.spriteName = "head-fashi";
				}
				listBtnNear[i].PlayerName.text =  (string)p[i , 1];
				listBtnNear[i].PlayerLeve.text =  ((int)p[i , 2]).ToString();
				listBtnNear[i].PlayerDeulWinCount.text = ((int)p[i , 3]).ToString();
				//			players[i , 4] = playerDuelState;
				listBtnNear[i].PlayerForce.text = ((int)p[i , 5]).ToString();
				listBtnNear[i].SetDuelState((int)p[i , 4] , (int)p[i , 6]);
//			}
		}

		
	}
}

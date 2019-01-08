using UnityEngine;
using System.Collections;

public class ShowMapLeftStart : MonoBehaviour {
		
		public UISprite[] listStars;

		public UISprite[] listBackGround;
		public string mapID;
		
		private int numStars;
		
		public int NumStars
		{
			get { return numStars; }
			set 
			{
				numStars = value ;
				SetStar(numStars);
			}
		}
		
		void OnEnable()
		{
			SetStar(this.numStars);
		}

		public void SetMapID(string mMapID)
		{
		  this.mapID=mMapID;
		}
		
		private void SetStar(int mNumStars)
		{
			int num = mNumStars >> 1;
			int half = mNumStars % 2;
			int tempNum = 0;
			foreach (UISprite item in listStars)
			{
				if (tempNum > (num - 1))
				{
				item.spriteName = "start0";
				}
				else
				{
					item.spriteName = "Start - new";
				}
				tempNum++;
			}
			if (half != 0&&num<tempNum)
			{
			listStars[num].spriteName = "start0";
			}
		}



	void Update(){
		if(mapID=="121"||mapID=="131"||mapID=="151"||mapID=="111"){
			foreach (UISprite item in listStars)
			{
				item.spriteName = "";
			}
			if(null!=listBackGround){
			foreach (UISprite itemBG in listBackGround)
			{
				itemBG.spriteName = "";
			}
			}
		}
	}
}

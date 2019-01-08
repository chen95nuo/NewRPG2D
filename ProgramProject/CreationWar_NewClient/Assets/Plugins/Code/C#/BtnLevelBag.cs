using UnityEngine;
using System.Collections;

public class BtnLevelBag : MonoBehaviour {

	public UILabel lblShowText;
	public UISprite IsNowShowSpr;
	public void BtnClick()
	{
		PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage("Show9", SendMessageOptions.DontRequireReceiver);
		DailyBenefitsPanelSelect.My.isOneLeveBag = true;
		DailyBenefitsPanelSelect.My.BtnLeveShow();
	}

	void Start()
	{
		InvokeRepeating("ShowText",0,1f);
	}
	public void ShowText(){
//		Debug.Log("ran============================================");
		int Leve = int.Parse(BtnGameManager.yt.Rows[0]["PlayerLevel"].YuanColumnText);
		
		string[] HasLeve =  BtnGameManager.yt.Rows[0]["hasLevePacks"].YuanColumnText.Split (';');


	
				if(Leve<=5){
				for(int i=0;i<HasLeve.Length;i++)
				{

				if(HasLeve.Length>1&&string.IsNullOrEmpty(HasLeve[i])){
					return;
				}
				if(!string.IsNullOrEmpty(HasLeve[i])){
				if(int.Parse(HasLeve[i])==5){
					IsNowShowSpr.enabled = false;
					lblShowText.text = StaticLoc.Loc.Get("info1257");
				}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1256");
				}
				}else{
					if(Leve<5){
					IsNowShowSpr.enabled = false;
					lblShowText.text = StaticLoc.Loc.Get("info1256");
					}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1256");
					}
				}
			}
				}else if(10>=Leve&&Leve>5)
				{
			for(int i=0;i<HasLeve.Length;i++)
			{
				if(HasLeve.Length>1&&string.IsNullOrEmpty(HasLeve[i])){
					return;
				}
				if(!string.IsNullOrEmpty(HasLeve[i])){
				if(int.Parse(HasLeve[i])==10){
					IsNowShowSpr.enabled = false;
					lblShowText.text = StaticLoc.Loc.Get("info1257");
				}else{
						if(int.Parse(HasLeve[i])==5){
							IsNowShowSpr.enabled = false;
							lblShowText.text = StaticLoc.Loc.Get("info1257");
						}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1256");
						}
				}
				}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1256");
				}
			}
				}
				else if(15>=Leve&&Leve>10)
				{
			for(int i=0;i<HasLeve.Length;i++)
			{
				if(HasLeve.Length>1&&string.IsNullOrEmpty(HasLeve[i])){
					return;
				}
				if(!string.IsNullOrEmpty(HasLeve[i])){
				if(int.Parse(HasLeve[i])==15){
					IsNowShowSpr.enabled = false;
					lblShowText.text = StaticLoc.Loc.Get("info1258");
				}else{
						if(int.Parse(HasLeve[i])==10){
							IsNowShowSpr.enabled = false;
							lblShowText.text = StaticLoc.Loc.Get("info1258");
						}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1257");
						}
				}
				}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1257");
				}
			}
				}
				else if(20>=Leve&&Leve>15)
				{
			for(int i=0;i<HasLeve.Length;i++)
			{
				if(HasLeve.Length>1&&string.IsNullOrEmpty(HasLeve[i])){
					return;
				}
				if(!string.IsNullOrEmpty(HasLeve[i])){
				if(int.Parse(HasLeve[i])==20){
					IsNowShowSpr.enabled = false;
					lblShowText.text = StaticLoc.Loc.Get("info1259");
				}else{
						if(int.Parse(HasLeve[i])==15){
							IsNowShowSpr.enabled = false;
							lblShowText.text = StaticLoc.Loc.Get("info1259");
						}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1258");
						}
				}
				}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1258");
				}
				}
				}
				else if(25>=Leve&&Leve>20)
				{
			for(int i=0;i<HasLeve.Length;i++)
			{
				if(HasLeve.Length>1&&string.IsNullOrEmpty(HasLeve[i])){
					return;
				}
				if(!string.IsNullOrEmpty(HasLeve[i])){
				if(int.Parse(HasLeve[i])==25){
					IsNowShowSpr.enabled = false;
					lblShowText.text = StaticLoc.Loc.Get("info1260");
				}else{
						if(int.Parse(HasLeve[i])==20){
							IsNowShowSpr.enabled = false;
							lblShowText.text = StaticLoc.Loc.Get("info1260");
						}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1259");
						}
				}
				}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1259");
				}
			}
				}
				else if(30>=Leve&&Leve>25)
				{
			for(int i=0;i<HasLeve.Length;i++)
			{
				if(HasLeve.Length>1&&string.IsNullOrEmpty(HasLeve[i])){
					return;
				}
				if(!string.IsNullOrEmpty(HasLeve[i])){
				if(int.Parse(HasLeve[i])==30){
					IsNowShowSpr.enabled = false;
					lblShowText.text = StaticLoc.Loc.Get("info1261");
				}else{
					if(int.Parse(HasLeve[i])==25){
						IsNowShowSpr.enabled = false;
						lblShowText.text = StaticLoc.Loc.Get("info1261");
						}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1260");
						}
				}
				}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1260");
				}

			}
				}
				else if(40>=Leve&&Leve>30)
				{
			for(int i=0;i<HasLeve.Length;i++)
			{
				if(HasLeve.Length>1&&string.IsNullOrEmpty(HasLeve[i])){
					return;
				}
				if(!string.IsNullOrEmpty(HasLeve[i])){
				if(int.Parse(HasLeve[i])==40){
					IsNowShowSpr.enabled = false;
					lblShowText.text = StaticLoc.Loc.Get("info1262");
				}else{
						if(int.Parse(HasLeve[i])==30){
							IsNowShowSpr.enabled = false;
							lblShowText.text = StaticLoc.Loc.Get("info1262");
						}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1261");
						}
				}
				}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1261");
				}

			}
				}
				else if(50>=Leve&&Leve>40)
				{
			for(int i=0;i<HasLeve.Length;i++)
			{
				if(HasLeve.Length>1&&string.IsNullOrEmpty(HasLeve[i])){
					return;
				}
				if(!string.IsNullOrEmpty(HasLeve[i])){
				if(int.Parse(HasLeve[i])==50){
					IsNowShowSpr.enabled = false;
					lblShowText.text = StaticLoc.Loc.Get("info1263");
				}else{
						if(int.Parse(HasLeve[i])==40){
							IsNowShowSpr.enabled = false;
							lblShowText.text = StaticLoc.Loc.Get("info1263");
						}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1262");
						}
				}
				}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1262");
				}
			}
				}
				else if(50<Leve&&Leve<60)
				{
			for(int i=0;i<HasLeve.Length;i++)
			{
				if(HasLeve.Length>1&&string.IsNullOrEmpty(HasLeve[i])){
					return;
				}
				if(!string.IsNullOrEmpty(HasLeve[i])){
				if(int.Parse(HasLeve[i])==50){
					IsNowShowSpr.enabled = false;
					lblShowText.text = StaticLoc.Loc.Get("info1264");
				}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1264");
				}
				}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1263");
				}
				
			}
				}else if(Leve>=60){
			for(int i=0;i<HasLeve.Length;i++)
			{
				if(HasLeve.Length>1&&string.IsNullOrEmpty(HasLeve[i])){
					return;
				}
				if(!string.IsNullOrEmpty(HasLeve[i])){
				if(int.Parse(HasLeve[i])==60){
					IsNowShowSpr.enabled = false;
					lblShowText.text = StaticLoc.Loc.Get("info1264");
				}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1264");
				}
				}else{
					IsNowShowSpr.enabled = true;
					lblShowText.text = StaticLoc.Loc.Get("info1264");
				}
			}
				}
			}


	
}

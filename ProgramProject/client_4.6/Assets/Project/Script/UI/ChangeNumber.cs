using UnityEngine;
using System.Collections;

public class ChangeNumber : MonoBehaviour {
	//实现数字变换//
	public static ChangeNumber mInstance;
	
	UILabel changeLabel;					//需要变换的label//
	float lastNum;							//变化之前的数字//
	float curNum;							//变化之后的数字
	float needChangeNum;					//需要变化的数字//
	float curChangeNum;
	float remainChangeNum;
	float curChangeTime;
	float remainChangeTime;
	float needChangeTime;
	float runningTime;
	bool isRunning;
	float curShowNum;
	float totalNum;
	int curType;
	void Awake(){
		mInstance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	
	
	/**type 表示当前显示的是那种，0 是普通的（即每帧都更新）， 1 是需要点击才更新的**/
	public void SetData(UILabel label, float lastN, float curN, float time, float totalN = -1, int type = 0){
		changeLabel = label;
		lastNum = lastN;
		curNum = curN;
		needChangeNum = curNum - lastNum;
		remainChangeNum = needChangeNum;
		totalNum = totalN;
		curType = type;
		
		remainChangeTime = time;
		curShowNum = lastNum;
	}
	
	public void StartChange(){
		
		curChangeNum = needChangeNum;
		
		remainChangeNum -= curChangeNum;
		
		runningTime = 0.0f;
		isRunning = true;
	}
	
	public void StopChange(){
		isRunning = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(curType == 0){
			ChangeNum();
		}
		else if(curType == 1){
			
		}
	}
	
	public void ChangeNum(){
		if(isRunning)
		{
			if(runningTime <= remainChangeTime)
			{
				curShowNum = (int)(lastNum + needChangeNum*runningTime/remainChangeTime);
				if(changeLabel != null){
					if(totalNum > -1){
						changeLabel.text = curShowNum +"/" + totalNum ;
					}
					else {
						
						changeLabel.text = curShowNum +"" ;
					}
				}
				runningTime+= Time.deltaTime;
			}
			else
			{
				curShowNum = (int)(lastNum + needChangeNum);
				if(changeLabel != null){
					
					if(totalNum > -1){
						changeLabel.text = curShowNum +"/" + totalNum ;
					}
					else {
						
						changeLabel.text = curShowNum +"" ;
					}
				}
				isRunning = false;
				
			}	
		}
	}
	
	
	public float GetShowNum(){
		return curShowNum;
	}
	
	public bool GetAnimIsRunning(){
		return isRunning;
	}
	
}

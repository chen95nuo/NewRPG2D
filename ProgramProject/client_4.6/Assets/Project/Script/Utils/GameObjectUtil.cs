using UnityEngine;
using System;
using System.Collections;


// mark - xuyan //
public class GameObjectUtil 
{
	private static float fixedTime = 0.02f; 
	
	
	public static GameObject findGameObjectByName(GameObject obj,string name)
	{
		if(obj == null)
			return null;
		GameObject findObj = null;
		if(obj.name == name)
		{
			findObj = obj;
			return findObj;
		}
		
		for(int i = 0; i < obj.transform.childCount; ++i)
		{
			findObj = findGameObjectByName(obj.transform.GetChild(i).gameObject,name);
			if(findObj != null)
			{
				return findObj;
			}
		}
		return null;
	}
	
	public static void setGameObjectLayer(GameObject obj,int layer)
	{
		if(obj == null)
			return;
		obj.layer = layer;
		for(int i = 0; i < obj.transform.childCount; ++i)
		{
			setGameObjectLayer(obj.transform.GetChild(i).gameObject,layer);
		}
	}
	
	public static void setGameObjectRenderState(GameObject obj,bool b)
	{
		if(obj == null)
			return;
		obj.renderer.enabled = b;
		for(int i = 0; i < obj.transform.childCount;++i)
		{
			setGameObjectRenderState(obj.transform.GetChild(i).gameObject,b);		
		}
	}
	
	public static Vector3 localPosToWorldPosForGameObject(GameObject obj,Vector3 pos)
	{
		Vector3 worldPos = pos;
		if(obj.transform.parent != null)
		{
			worldPos.x = obj.transform.parent.localPosition.x + worldPos.x*obj.transform.parent.localScale.x;
			worldPos.y = obj.transform.parent.localPosition.y + worldPos.y*obj.transform.parent.localScale.y;
			worldPos.z = obj.transform.parent.localPosition.z + worldPos.z*obj.transform.parent.localScale.z;
			worldPos = localPosToWorldPosForGameObject(obj.transform.parent.gameObject,worldPos);
		}
		return worldPos;
	}
	
	//mark start -- cxl//
	public static Vector3 WorldPosToLocalPosForGameObject(Camera mainCamera,Camera NGUICamera, Vector3 pos)
	{
		
//		Vector3 worldPos = new Vector3(pos.x, pos.y, pos.z);
		Vector2 screenPos = mainCamera.WorldToScreenPoint(pos);
		Vector3 loaclPos = NGUICamera.ScreenToWorldPoint(screenPos);
		
		return loaclPos;
	}
	//mark end -- cxl//
	
	public static void destroyGameObjectAllChildrens(GameObject obj)
	{
		if(obj == null)
		{
			return;
		}
		if(obj.transform.childCount == 0)
		{
			return;
		}
		int count = obj.transform.childCount;
		for(int i = 0; i < count;++i)
		{
			GameObject child = obj.transform.GetChild(0).gameObject;
			if(child != null)
			{
				GameObject.DestroyImmediate(child);
			}
		}
	}
	
	public static void gameObjectAttachToParent(GameObject obj,GameObject parentObj,bool useSelfScale = false)
	{
		float selfScaleX = obj.transform.localScale.x;
		float selfScaleY = obj.transform.localScale.y;
		float selfScaleZ = obj.transform.localScale.z;
		obj.transform.parent = parentObj.transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		if(!useSelfScale)
		{
			obj.transform.localScale = new Vector3(1,1,1);
		}
		else
		{
			obj.transform.localScale = new Vector3(selfScaleX,selfScaleY,selfScaleZ);
		}
		obj.layer = parentObj.layer;
	}
	
	//cuixl -- mark//
	/// <summary>
	/// Loads the resources prefabs.
	/// </summary>
	/// <returns>
	/// The resources prefabs.
	/// </returns>
	/// <param name='name'>
	/// Name.
	/// </param>
	/// <param name='type'>
	/// Type. 0 card, 1 effect. 2 item. 3 ui 4 ui中的bounes, -1 表示是整个路径
	/// </param>
	public static GameObject LoadResourcesPrefabs(string name, int type)
	{
		GameObject prefab = null;
		string path = "";
		if(type == -1){
			path = name ;
		}
		if(type == 0){
			path = "Prefabs/Cards/";
		}
		if(type == 1){
			path = "Prefabs/Effects/";
		}
		else if(type == 2){
			path = "Prefabs/Item/";
		}
		else if(type == 3){
			path = "Prefabs/UI/";
		}
		else if(type == 4){
			path = "Prefabs/UI/BattleBounesPanel/";
		}
		else if(type == 5)
		{
			path = "Prefabs/Effects/CallUpEffect/";
		}
		else if(type == 6)
		{
			path = "Prefabs/";
		}
//		Debug.Log("path : " + path + name);
		if(type > -1){
			prefab = (GameObject) Resources.Load(path + name);
		}
		else {
			
			prefab = (GameObject) Resources.Load(path);
		}
		if(prefab == null)
		{
			Debug.Log("effect name:" + name);
			Time.timeScale = 0;
		}
		return prefab;
	}
	
	/**获取当前时间(毫秒数)**/
	public static long getCurTime()
	{
		return DateTime.Now.Ticks/10000;
	}
	
	/**封装淡入淡出方法 -- cxl --**/
	//methodName 动画结束后回调的方法//
	public static void PlayTweenAlpha(GameObject go, float alphaFrom, float alphaTo, string methodName, float time){
		TweenAlpha tween = go.GetComponent<TweenAlpha>();
		tween.from = alphaFrom;
		tween.to = alphaTo;
		if(tween.onFinished!=null && tween.onFinished.Count>0)
		{
			tween.onFinished[0].methodName = methodName;
		}
		TweenAlpha.Begin<TweenAlpha>(go, time);
		 
	}
	
	
	//物体从屏幕外移动到起始点//
	public static void PlayerMoveAndScaleAnim(GameObject go, float changeTime, iTween.EaseType moveType, 
		GameObject callBackObj = null, string methodName = "", bool isPlayScale = true, float y = -2000, float z = -2000){
		Vector3 startPos = go.transform.position;
		go.transform.localPosition = new Vector3(startPos.x, startPos.y, z);
		//播放由大到小的效果//
		if(isPlayScale){
			Vector3 startScale = go.transform.localScale;
			//Debug.Log("startScale ==== " + startScale);
			go.transform.localScale = new Vector3(50, 50, 1);
			iTween.ScaleTo(go,iTween.Hash("scale",startScale,"time",changeTime, "easetype",moveType));
		}
		//移动//
		if(callBackObj != null){
			
			iTween.MoveTo(go,iTween.Hash("position",startPos,"time",changeTime, "oncomplete", methodName, 
				"oncompletetarget", callBackObj, "easetype",iTween.EaseType.linear));
		}
		else {
			iTween.MoveTo(go,iTween.Hash("position",startPos,"time",changeTime,"easetype",iTween.EaseType.linear));
		}
	}
	
	//封装itween的move方法//
	public static void PlayMoveToAnim(GameObject go, Vector3 desPos, float time, GameObject callBackObj, string callBackMethod, float delayTime = 0.0f, UnityEngine.Object callBackParam = null){
		if(callBackParam != null){
			iTween.MoveTo(go,iTween.Hash("position",desPos,"time",time, "oncomplete", callBackMethod, 
				"oncompletetarget", callBackObj, "oncompleteparams", callBackParam, "delay", delayTime, "easetype", iTween.EaseType.linear));
		}
		else {
			iTween.MoveTo(go,iTween.Hash("position",desPos,"time",time, "oncomplete", callBackMethod, 
				"oncompletetarget", callBackObj , "delay", delayTime, "easetype", iTween.EaseType.linear));
		}
	}
	
	public static void LoadLevelByName(string levelName)
	{
		CleanSceneData(levelName);
		if(UISceneEffectNodeManager.mInstance != null)
		{
			UISceneEffectNodeManager.mInstance.gc();
		}
		Resources.UnloadUnusedAssets();
		GC.Collect();
		ChangeSceneLoadManager.mInstance.setData(levelName);
		
		AddSpeed(1);
	}
	
	public static void CleanSceneData(string levelName)
	{
		//清楚其他两个场景中的静态变量//
		if(levelName==STATE.GAME_SCENE_NAME_LOADING)
		{
			CleanUISceneData();
			CleanGameSceneData();
		}
		//清楚game和loading场景中遗留的静态变量//
		else if(levelName==STATE.GAME_SCENE_NAME_UI)
		{
			//清除Loading场景的变量//
			CleanLoadingSceneData();
			//清楚game场景数据//
			CleanGameSceneData();
		}
		//ui和loading场景的静态变量//
		else if(levelName==STATE.GAME_SCENE_NAME_GAME)
		{
			CleanUISceneData();
			CleanLoadingSceneData();
		}
		Resources.UnloadUnusedAssets();
	}
	
	public static void CleanLoadingSceneData()
	{
		
		
		if(LoadRes.RES_DIR != null)
		{
			LoadRes.RES_DIR = null;
		}
		
		if(LoginUI_new.mInstance != null)
		{
			LoginUI_new.mInstance.CleanData();
			LoginUI_new.mInstance = null;
		}
		
		if(NoticeUIManager.mInstance != null)
		{
			NoticeUIManager.mInstance.CleanData();
			NoticeUIManager.mInstance = null;
		}
		if(ToastWindow.mInstance != null)
		{
			GameObject.Destroy(ToastWindow.mInstance.gameObject);
			ToastWindow.mInstance = null;
		}
	}
	
	
	//清除game场景数据//
	public static void CleanGameSceneData()
	{
		if(PVESceneControl.mInstance!= null)
		{
			PVESceneControl.mInstance.gc();
			PVESceneControl.mInstance = null;
		}
		if(BattleBounesPanel.mInstance!= null)
		{
			BattleBounesPanel.mInstance.gc();
//			BattleBounesPanel.mInstance = null;
			
		}
		if(BattleGuidePointUnitSkill.mInstance!= null)
		{
			BattleGuidePointUnitSkill.mInstance.gc();
//			BattleGuidePointUnitSkill.mInstance = null;
		}
		
		if(DialogPanel.mInstance!= null)
		{
			DialogPanel.mInstance.gc();
			GameObject.Destroy(DialogPanel.mInstance.gameObject);
//			DialogPanel.mInstance = null;
		}
		if(NewBattleUnitePanel.mInstance!= null)
		{
			NewBattleUnitePanel.mInstance.gc();
//			NewBattleUnitePanel.mInstance = null;
		}
		if(ResultTipManager.mInstance!= null)
		{
			ResultTipManager.mInstance.gc();
//			ResultTipManager.mInstance = null;
		}
		if(RewardsDatasControl.mInstance!= null)
		{
			RewardsDatasControl.mInstance.gc();
//			RewardsDatasControl.mInstance = null;
		}
		if(UIInterfaceManager.mInstance!= null)
		{
			UIInterfaceManager.mInstance.gc();
//			UIInterfaceManager.mInstance = null;
		}
		if(StartFightPanel.mInstance!= null)
		{
			StartFightPanel.mInstance.gc();
//			StartFightPanel.mInstance = null;
		}
		
		if(BattleEffectHelperControl.mInstance != null)
		{
			BattleEffectHelperControl.mInstance.gc();
		}
		if(ToastWarnUI.mInstance!= null)
		{
			
			ToastWarnUI.mInstance = null;
		}
		if(ToastWindow.mInstance!= null)
		{
			ToastWindow.mInstance = null;
		}
		
	}
	
	public static void CleanUISceneData()
	{
		if(Main3dCameraControl.mInstance!=null)
		{
			Main3dCameraControl.mInstance = null;
		}
		if(ShowBuyTipControl.mInstance!=null)
		{
			
			ShowBuyTipControl.mInstance = null;
		}
		if(UISceneStateControl.mInstace!=null)
		{
			UISceneStateControl.mInstace = null;
		}
		if(BlackBgUI.mInstance!=null)
		{
			BlackBgUI.mInstance = null;
		}
		
		if(RequestUnlockManager.mInstance !=null)
		{
			RequestUnlockManager.mInstance = null;
		}
		if(RewardsDatasControl.mInstance !=null)
		{
			RewardsDatasControl.mInstance = null;
		}
		if(BuyTipManager.mInstance !=null)
		{
			BuyTipManager.mInstance = null;
		}
		if(HeadUI.mInstance !=null)
		{
			HeadUI.mInstance = null;
		}
		if(UIJumpTipManager.mInstance !=null)
		{
			UIJumpTipManager.mInstance = null;
		}
		if(ToastWarnUI.mInstance !=null)
		{
			ToastWarnUI.mInstance = null;
		}
		if( ToastWindow .mInstance !=null)
		{
			ToastWindow.mInstance = null;
		}
		if(UISceneDialogPanel.mInstance != null)
		{
			UISceneDialogPanel.mInstance = null;
		}
		
		if(ZhenRoot.mInstance!=null)
		{
			ZhenRoot.mInstance.zhen.gc();
			ZhenRoot.mInstance.zhen = null;
			ZhenRoot.mInstance = null;
		}
		
		if(UISceneStateControl.mInstace != null)
		{
			UISceneStateControl.mInstace.CleanData();
			UISceneStateControl.mInstace = null;
		}
		
	}
	
	//修改时间流速//
	public static void AddSpeed(float scaleNum){
		Time.timeScale = scaleNum;
		Time.fixedDeltaTime = fixedTime * scaleNum;
	}
	
	// cpoy sourceObj transform's position rotation and localScale to targetObj
	public static void copyTarnsformValue(GameObject sourceObj,GameObject targetObj)
	{
		if(sourceObj == null || targetObj == null)
		{
			return;
		}
		targetObj.transform.position = sourceObj.transform.position;
		targetObj.transform.rotation = sourceObj.transform.rotation;
		targetObj.transform.localScale = sourceObj.transform.localScale;
	}
	
	//隐藏刀光//
	public static void HiddenLight(GameObject cardObj){
		//DaoGuangController daoGuang = cardObj.GetComponent<DaoGuangController>();
		//if(daoGuang != null && daoGuang.trail != null){
		//	daoGuang.trail.gameObject.SetActive(false);
		//}
	}
	
	public static void showCardEffect(GameObject cardObj)
	{
		CardEffectControl cec = cardObj.GetComponent<CardEffectControl>();
		if(cec != null)
		{
			cec.showEffect();
		}
	}
	
	public static void hideCardEffect(GameObject cardObj)
	{
		CardEffectControl cec = cardObj.GetComponent<CardEffectControl>();
		if(cec != null)
		{
			cec.hideEffect();
		}
	}
	
	public static void showFog(Color c)
	{
		RenderSettings.fog = true;
		RenderSettings.fogColor = c;
		RenderSettings.fogDensity = 0.02f;
		RenderSettings.fogMode = FogMode.Linear;
		RenderSettings.fogStartDistance = 35;
		RenderSettings.fogEndDistance = 100;
	}
	
	public static void showDarkFog()
	{
		RenderSettings.fog = true;
		RenderSettings.fogColor = new Color(0,0,0,1);
		RenderSettings.fogDensity = 0.02f;
		RenderSettings.fogMode = FogMode.Linear;
		RenderSettings.fogStartDistance = 15;
		RenderSettings.fogEndDistance = 60;
	}
	
	public static void playForwardUITweener(UITweener uiTweener)
	{
		uiTweener.enabled = true;
		uiTweener.tweenFactor = 0;
		uiTweener.PlayForward();
	}
	
	public static void playReverseUITweener(UITweener uiTweener)
	{
		uiTweener.enabled = false;
		uiTweener.tweenFactor = 1;
		uiTweener.PlayReverse();
	}
	
}
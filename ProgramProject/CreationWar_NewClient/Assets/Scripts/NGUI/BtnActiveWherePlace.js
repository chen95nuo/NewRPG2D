#pragma strict

var activeMapType : MapType;
function Start () {
	if(activeMapType == MapType.zhucheng){
		if(UIControl.mapType == MapType.zhucheng){
//			gameObject.active = true;
		}else{
			this.transform.localPosition.y = -190;
		}
	}
	
	if(activeMapType == MapType.fuben){
		if(UIControl.mapType == MapType.zhucheng){
//			gameObject.active = false;
			this.transform.localPosition.y = -190;
		}else{
			
//			gameObject.active = true;
		}
	}
}

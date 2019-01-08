#pragma strict

function Awake () {

	var hh :float = Screen.height;
	var ww :float = Screen.width;
	var hw :float = hh /ww;
if(hw>=0.75||Application.loadedLevelName == "Loading 1")
camera.orthographicSize = camera.orthographicSize*1.5*Screen.height/Screen.width;  
if(hw>=0.75&&Application.loadedLevelName == "Login-2")
camera.orthographicSize = camera.orthographicSize*1.6*Screen.height/Screen.width; 
}


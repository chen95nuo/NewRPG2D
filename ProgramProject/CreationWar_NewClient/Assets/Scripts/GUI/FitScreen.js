private var gui : GUITexture;								

function Awake()
{
	gui = GetComponent( GUITexture );	
//if(Screen.width<=1024){
gui.pixelInset = Rect(gui.pixelInset.x *Screen.width*0.002083 , gui.pixelInset.y *Screen.width*0.002083, gui.pixelInset.width*Screen.width*0.002083, gui.pixelInset.height*Screen.width*0.002083);
//}

//if(Screen.height==768){
//gui.pixelInset = Rect(gui.pixelInset.x *Screen.height*0.003125 , gui.pixelInset.y *Screen.height*0.003125, gui.pixelInset.width, gui.pixelInset.height);	
//}
}

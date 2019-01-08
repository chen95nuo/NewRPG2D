#pragma strict

var MySprite : UISprite;
function Start () {

}

function OnEnable(){
MySprite.spriteName = AllManage.mtwStatic.nowRightSprite.spriteName;
}
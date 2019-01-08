#pragma strict

function show0(){
	AllManage.UIALLPCStatic.show0();
}

function JianCeShan(){
	AllManage.InvclStatic.JianCeShan();
}

var PEA : PanelEverydayAim;
function Awake(){
	if(PEA){
		PEA.invCL = AllManage.InvclStatic.gameObject;
		PEA.invMaker = AllResources.InvmakerStatic.gameObject;		
	}
}
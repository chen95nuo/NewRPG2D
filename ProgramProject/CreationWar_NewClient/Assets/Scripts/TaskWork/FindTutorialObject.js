#pragma strict


var buttons : GameObject[];
var tutorIDNums : int[];
var tutorStepNums : int[];
function Awake(){
	for(var i=0; i<buttons.length; i++){
		AllManage.jiaochengCLStatic.allJiaoCheng[ tutorIDNums[i] ].obj[ tutorStepNums[i] ] = buttons[ i ];
	}
}

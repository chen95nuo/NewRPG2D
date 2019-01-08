#pragma strict

function OnEnable (){
//InvokeRepeating("ShowLable",0,1);
}
var bagiteam : BagItem;
var LabelName : UILabel;
var LabelAckStr : UILabel;
var LabelAckNumb : UILabel;
var LabeltAmorStr : UILabel;
var LabeltAmorNumb : UILabel;
var LabelAllStr : UILabel;
var LabelAllNumb : UILabel;
function ShowLable(){
    if(null == LabelName)
    {
        return;
    }

	LabelName.text = "";
	LabelAckStr.text = "";
	LabelAckNumb.text = "";
	LabeltAmorStr.text = "";
	LabeltAmorNumb.text = "";
	LabelAllStr.text = "";
	LabelAllNumb.text = "";
	
	LabelName.text = bagiteam.inv.itemName;
	LabelAckStr.text = bagiteam.inv.ATatkStr;
	LabelAckNumb.text = bagiteam.inv.ATatk + "";
	LabeltAmorStr.text = bagiteam.inv.AtarmorStr;
	LabeltAmorNumb.text = bagiteam.inv.ATarmor + "";
	LabelAllStr.text = bagiteam.inv.ATzongfenStr;
	LabelAllNumb.text = bagiteam.inv.ATzongfen + "";
	

}
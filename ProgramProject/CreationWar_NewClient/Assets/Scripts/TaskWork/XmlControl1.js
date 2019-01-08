import System;
//import System.Collections;
import System.Xml;
import System.Xml.Serialization;
import System.IO;
import System.Text;
import System.Net;
import System.Net.Sockets;
//import System.Threading;


// Anything we want to store in the XML file, we define it here
class DemoData
{
	var id : int = 0;
    var x : float = 0;
    var y : float = 0;
    var z : float = 0;
    var name : String = "";
    var level : int = 0;
    var ship : int = 0;
    var fan : int = 0;
    var shouxian : int = 0;
    var exp : int = 0;
    var paoStr : String = "000000";
    var vs1 : String = "";
    var vs2 : String = "";
	var vs3 : String = "";
	var vs4 : String = "";
	var vs5 : String = "";
	var thisVs : String = "";
    var go : String = "";
}
// UserData is our custom class that holds our defined objects we want to store in XML format
 class UserData
 {
    // We have to define a default instance of the structure
   public var _iUser : DemoData = new DemoData();
    // Default constructor doesn't really do anything at the moment
   function UserData() { }
}

private var _Save : Rect;
private var _Load : Rect;
private var _SaveMSG : Rect;
private var _LoadMSG : Rect;
private var _send : Rect;
private var _FileLocation : String;
private var _FileName : String = "SaveData.xml";
var _Player : GameObject;
var _PlayerName : String = "Joe Schmoe";
var myData : UserData;
private var _data : String;
private var VPosition : Vector3;

// When the EGO is instansiated the Start will trigger
// so we setup our initial values for our local members
//function Start () {
function Awake () {
      // We setup our rectangles for our messages
      _Save=new Rect(10,80,100,20);
      _Load=new Rect(10,100,100,20);
      _SaveMSG=new Rect(10,120,200,40);
      _LoadMSG=new Rect(10,140,200,40);
       _send = new Rect(10,160,200,40);
      // Where we want to save and load to and from
      _FileLocation=Application.dataPath;
      
          
      // we need soemthing to store the information into
      myData=new UserData();
   }
function Start(){
	
//		_FileLocation=Application.dataPath.Substring(0,Application.dataPath.Length-5);
//		_FileLocation=_FileLocation.Substring(0,_FileLocation.LastIndexOf('/'));
//		_FileLocation=_FileLocation+"/Documents";
	myData._iUser.paoStr = "";
	for(var i=1; i<7; i++){
		var str = "daoju" + i;
		myData._iUser.paoStr += PlayerPrefs.GetInt(str,0);
	}
	myData._iUser.level = PlayerPrefs.GetInt("LV",1);
	myData._iUser.ship=PlayerPrefs.GetInt("ship");
    myData._iUser.shouxian=PlayerPrefs.GetInt("shouxian");
    myData._iUser.fan=PlayerPrefs.GetInt("fan");
    myData._iUser.exp = PlayerPrefs.GetInt("exp",1);
}

function Update () {}
   
function OnGUI()
{   
   // ***************************************************
   // Loading The Player...
   // **************************************************       
   if (GUI.Button(_Load,"Load")) {
      
      GUI.Label(_LoadMSG,"Loading from: "+_FileLocation);
      // Load our UserData into myData
      LoadXML();
      if(_data.ToString() != "")
      {
         // notice how I use a reference to type (UserData) here, you need this
         // so that the returned object is converted into the correct type
         //myData = (UserData)DeserializeObject(_data);
         myData = DeserializeObject(_data);
         // set the players position to the data we loaded
         VPosition=new Vector3(myData._iUser.x,myData._iUser.y,myData._iUser.z);             
         _Player.transform.position=VPosition;
         // just a way to show that we loaded in ok
         Debug.Log(myData._iUser.name);
      }
   }
   
   // ***************************************************
   // Saving The Player...
   // **************************************************   
   if (GUI.Button(_Save,"Save")) {
            
      GUI.Label(_SaveMSG,"Saving to: "+_FileLocation);
      //Debug.Log("SaveLoadXML: sanity check:"+ _Player.transform.position.x);

      myData._iUser.x = _Player.transform.position.x;
      myData._iUser.y = _Player.transform.position.y;
      myData._iUser.z = _Player.transform.position.z;
      myData._iUser.name = _PlayerName;   
      // Time to creat our XML!
      _data = SerializeObject(myData);
      // This is the final resulting XML from the serialization process
      CreateXML();
      Debug.Log(_data);
   }
}
   
function UTF8ByteArrayToString(characters : byte[] )
{     
   var encoding : UTF8Encoding  = new UTF8Encoding();
   var constructedString : String  = encoding.GetString(characters);
   return (constructedString);
}

function StringToUTF8ByteArray(pXmlString : String)
{
   var encoding : UTF8Encoding  = new UTF8Encoding();
   var byteArray : byte[]  = encoding.GetBytes(pXmlString);
   return byteArray;
}
   
function SerializeObject(pObject : Object)
{
   var XmlizedString : String  = null;
   var memoryStream : MemoryStream  = new MemoryStream();
   var xs : XmlSerializer = new XmlSerializer(typeof(UserData));
   var xmlTextWriter : XmlTextWriter  = new XmlTextWriter(memoryStream, Encoding.UTF8);
   xs.Serialize(xmlTextWriter, pObject);
   memoryStream = xmlTextWriter.BaseStream; // (MemoryStream)
   XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
   return XmlizedString;
}

function DeserializeObject(pXmlizedString : String)   
{
   var xs : XmlSerializer  = new XmlSerializer(typeof(UserData));
   var memoryStream : MemoryStream  = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
   var xmlTextWriter : XmlTextWriter  = new XmlTextWriter(memoryStream, Encoding.UTF8);
   return xs.Deserialize(memoryStream);
}

function CreateXML()
{
   var writer : StreamWriter;
   var t : FileInfo = new FileInfo(_FileLocation+"/"+ _FileName);
   if(!t.Exists)
   {
      writer = t.CreateText();
   }
   else
   {
      t.Delete();
      writer = t.CreateText();
   }
   writer.Write(_data);
   writer.Close();
//   Debug.Log("File written.");
}
   
function LoadXML()
{
   var r : StreamReader = File.OpenText(_FileLocation+"/"+ _FileName);
   var _info : String = r.ReadToEnd();
   r.Close();
   _data=_info;
   cango = true;
   MyClient(_data);
//   Debug.Log("File Read");
}

function saveXml(Name : String , str : String){
      myData._iUser.name = Name;   
     myData._iUser.go = str;
      _data = SerializeObject(myData);
      CreateXML();
}

function saveXmlVs(Name : String , str : String){
	myData._iUser.name = Name;   
	myData._iUser.go = "saveVS";
	myData._iUser.thisVs = str;
	_data = SerializeObject(myData);
	CreateXML();
}

function insertMe(){
     LoadXML();
}

private var MyTcpClient : TcpClient;
private var Ip : IPAddress;	
private static var cango : boolean = true;

function MyClient(str : String){
	while(cango){
        try{
			MyTcpClient = new TcpClient();
			Ip=IPAddress.Parse("127.0.0.1");
 			MyTcpClient.Connect("173.208.211.186" , 5423);
            var stm : NetworkStream ;
			stm=MyTcpClient.GetStream();
            var AS : ASCIIEncoding = new ASCIIEncoding();			
            var  b : byte[] = AS.GetBytes(str);	
            stm.Write(b, 0, b.Length);
            var bb : byte[] = new byte[5000000];
            var k : int = stm.Read(bb, 0,5000000);
            var returnStr : String = "";
           for (var i = 0; i < k; i++)
				returnStr+=Convert.ToChar(bb[i]);
			if(returnStr == ""){
			}else{
				returnStr = returnStr.Substring(1,returnStr.length-1);
				var myData : UserData = new UserData();
				myData = DeserializeObject(returnStr);
				if(myData._iUser.go == "canVS"){	
					Application.LoadLevel("Game VS");
				}else
				if(myData._iUser.go == "getVS"){
				}
			}
 			cango=false;
           MyTcpClient.Close();
        }
        catch (e : Exception){
			cango = true;
			MyTcpClient.Close();
       }
		yield;
	}
}


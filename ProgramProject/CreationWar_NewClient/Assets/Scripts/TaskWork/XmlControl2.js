	
class XmlControl2 extends MonoBehaviour{
//		_FileLocation=Application.dataPath.Substring(0,Application.dataPath.Length-5);
//		_FileLocation=_FileLocation.Substring(0,_FileLocation.LastIndexOf('/'));
//		_FileLocation=_FileLocation+"/Documents";

	class TaskData {
  		  // We have to define a default instance of the structure
  		 public var _iUser : MainTask[];
  		  // Default constructor doesn't really do anything at the moment
  		 function TaskData() { }
	}
	
	class PlayerStatusData {
  		  // We have to define a default instance of the structure
  		 public var _iUser : PlayerStatusData;
  		  // Default constructor doesn't really do anything at the moment
  		 public function PlayerStatusData() { }
	}
	
	
	
//	function XmlSaveAsName(name : String , typeObj : Object){
//		switch(name){
//			case "MainTask" :
//				xsl = new XmlSerializer(typeof(TaskData));	
//				_data = SerializeObject(typeObj , xsl); 
//		 		CreateXML("TaskData.xml");
//				break;
//			case "MainPlayerStatus" : 
//				xsl = new XmlSerializer(typeof(PlayerStatusData));	
//				_data = SerializeObject(typeObj , xsl); 
//		 		CreateXML("PlayerStatusData.xml");
//				break;
//				
//		}
//	}
//	
//	function XmlLoadAsName(name : String , typeObj : Object){
//		switch(name){ 
//			case "MainTask" : 
//				LoadXML("TaskData.xml");
//				xsl = new XmlSerializer(typeof(TaskData));	
//				break;
//			case "MainPlayerStatus" : 
//				LoadXML("PlayerStatusData.xml");
//				xsl = new XmlSerializer(typeof(PlayerStatusData));	
//				break; 
//		} 
//		return  DeserializeObject(_data , xsl);  
//	}


	   
//	   // Here we serialize our UserData object of myData
//	   //string SerializeObject(object pObject)
//	function SerializeObject(pObject : Object ,xs : XmlSerializer)
//	{
//	   var XmlizedString : String  = null;
//	   var memoryStream : MemoryStream  = new MemoryStream(); 
//	  // var xs : XmlSerializer = new XmlSerializer(typeof(xmlType));
//	   var xmlTextWriter : XmlTextWriter  = new XmlTextWriter(memoryStream, Encoding.UTF8);
//	   xs.Serialize(xmlTextWriter, pObject);
//	   memoryStream = xmlTextWriter.BaseStream; // (MemoryStream)
//	   XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
//	   return XmlizedString;
//	}
//	   // Here we deserialize it back into its original form
//	   //object DeserializeObject(string pXmlizedString)
//	function DeserializeObject(pXmlizedString : String , xs : XmlSerializer)   
//	{
//	   //var xs : XmlSerializer  = new XmlSerializer(typeof(UserData));  
//	   var memoryStream : MemoryStream  = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
//	   var xmlTextWriter : XmlTextWriter  = new XmlTextWriter(memoryStream, Encoding.UTF8);
//	   return xs.Deserialize(memoryStream);
//	}
//	   // Finally our save and load methods for the file itself
//	function CreateXML(FileName : String)
//	{
//	   var writer : StreamWriter;
//	   //FileInfo t = new FileInfo(_FileLocation+"\\"+ _FileName);
//	   var t : FileInfo = new FileInfo(_FileLocation+"/"+ FileName);
//	      t.Delete();
//	   if(!t.Exists)
//	   {
//	      writer = t.CreateText();
//	   }
//	   else
//	   {
//	      t.Delete();
//	      writer = t.CreateText();
//	   }
//	   writer.Write(_data);
//	   writer.Close();
//	   Debug.Log("File written.");
//	} 
//	function LoadXML(FileName : String)
//	{
//	   //StreamReader r = File.OpenText(_FileLocation+"\\"+ _FileName);
//	   var r : StreamReader = File.OpenText(_FileLocation+"/"+ FileName);
//	   var _info : String = r.ReadToEnd();  
////	   xmlDoc.LoadXml(_FileLocation+"/"+ FileName);
//	   r.Close();
//	   _data=_info;
//	   Debug.Log("File Read");
//	}
	
}
 
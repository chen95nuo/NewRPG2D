using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;

public class wirtelog : MonoBehaviour {
	
	public Warnings warnings;
	
	    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }

	public string errorText=string.Empty;
	public void Register()
	{
		
//		Debug.Log (Application.persistentDataPath);
		if (System.IO.File.Exists(string.Format (@"{0}/bod2log.txt",Application.persistentDataPath)))
		{
			FileStream mFile=new FileStream(string.Format (@"{0}/bod2log.txt",Application.persistentDataPath),FileMode.Open);
			Byte[] mBytes=new Byte[(int)mFile.Length];
			mFile.Read (mBytes,0,(int)mFile.Length);
			mFile.Close ();
			mFile.Dispose ();
			ASCIIEncoding encoding=new ASCIIEncoding();
			errorText=encoding.GetString (mBytes);
			DeleteFile(Application.persistentDataPath,"bod2log.txt");

			
			if(errorText.Length>30)
			{
				OnBtnEnter ();
//				warnings.warningAllEnterClose.btnEnter.target=this.gameObject;
//				warnings.warningAllEnterClose.btnEnter.functionName="OnBtnEnter";
//				//warnings.warningAllEnterClose.btnExit.target=this.gameObject;
//				//warnings.warningAllEnterClose.btnEnter.functionName="OnBtnExit";
//				warnings.warningAllEnterClose.Show ("错误报告","系统检测到您上次退出游戏之前有一些错误报告，是否上传？");
			}
		}
		
    	CreateFile(Application.persistentDataPath,"bod2log.txt","BoD2Report");
		Application.RegisterLogCallback(ProcessExceptionReport);
	}
	
	public void OnBtnEnter()
	{
		errorText=errorText.Replace ("'","''");
		if(Application.platform!=RuntimePlatform.OSXEditor&&Application.platform!=RuntimePlatform.WindowsEditor)
		{
			YuanUnityPhoton.GetYuanUnityPhotonInstantiate ().SendError (Application.platform.ToString (),"","",errorText);
		}
		warnings.warningAllEnterClose.Close ();
	}
	

	
	private void ProcessExceptionReport(string condition, string stackTrace, LogType type){
		if(type == LogType.Exception){
		CreateFile(Application.persistentDataPath,"bod2log.txt",condition);	
		CreateFile(Application.persistentDataPath,"bod2log.txt",stackTrace);
		}	
	}
	
  void CreateFile(string path,string name,string info)
  {
      try
      {
          StreamWriter swa;
          FileInfo tt = new FileInfo(path + "//" + name);
          if (!tt.Exists)
              swa = tt.CreateText();
          else
              swa = tt.AppendText();
          swa.WriteLine(info);
          swa.Close();
          swa.Dispose();
      }
      catch (Exception ex)
      {
          Debug.LogWarning(ex.ToString());
      }
 } 

	void DeleteFile(string path,string name)
	   {
	        File.Delete(path+"//"+ name);	 
	   }	

}



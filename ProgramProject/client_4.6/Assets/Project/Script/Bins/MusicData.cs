using UnityEngine;
using System.Collections;

public class MusicData : PropertyReader {
	
	public int id{get;set;}
	public int type{get;set;}
	public string scene{get;set;}
	public string music{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(id,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static MusicData getData(int id){
		return (MusicData)data[id];
	}
	
	public static MusicData getIdBySceneName(string sc){
		MusicData music = null;
		foreach(DictionaryEntry  obj in data){
			MusicData md = (MusicData)obj.Value;
			if(md.scene == sc){
				music = md;
				break;
			}
		}
		return music;
	}
	
	public static MusicData getDataByType(int t){
		MusicData music = null;
		foreach(DictionaryEntry  obj in data){
			MusicData md = (MusicData)obj.Value;
			if(md.type == t){
				music = md;
				break;
			}
		}
		return music;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TaskData : PropertyReader {

	//==任务ID号==//
	public int taskId{get;set;}
	//==地图编号==//
	public int map_number{get;set;}
	//==触发条件类型==//
	public int ci_begin{get;set;}
	//==触发id==//
	public int ci_Id{get;set;}
	//==触发头像==//
	public string ci_model{get;set;}
	//==任务文本==//
	public string acceptWord{get;set;}
	
	public float yPos{get;set;}
	public float zoom{get;set;}
	
	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(taskId,this);
	}
	
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static TaskData getData(int taskId)
	{
		return (TaskData)data[taskId];
	}
	
	public static List<int> getTaskIDArray(int mapID,int triggerType)
	{
		List<int> idList = new List<int>();
		foreach(DictionaryEntry de in data)
		{
			TaskData td = (TaskData)de.Value;
			if(td.map_number == mapID && td.ci_begin == triggerType)
			{
				idList.Add(td.taskId);
			}
		}
		if(idList.Count > 0)
		{
			idList.Sort();
		}
		
		return idList;
	}
	
	public static List<int> getGuideTaskIDArray(int guideID)
	{
		List<int> idList = new List<int>();
		foreach(DictionaryEntry de in data)
		{
			TaskData td = (TaskData)de.Value;
			if(td.map_number == guideID)
			{
				idList.Add(td.taskId);
			}
		}
		if(idList.Count > 0)
		{
			idList.Sort();
		}
		return idList;
	}
}


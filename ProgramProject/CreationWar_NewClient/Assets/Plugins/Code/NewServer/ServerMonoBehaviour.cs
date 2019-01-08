using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerMonoBehaviour : MonoBehaviour {

	private object myLock=new object();

	protected virtual void Awake()
	{
		NetDataManager.DataHandle+=this.MyOnOperationResponse;
		NetDataManager.DataHandlePhoton+=this.MyOnOperationResponse;
	}

	protected virtual void OnDestroy()
	{
		NetDataManager.DataHandle-=this.OnOperationResponse;
	}

	private List<System.Action> listAction=new List<System.Action>();
	private  void MyOnOperationResponse(ZMNetData mData)
	{
		lock(myLock)
		{
			listAction.Add (()=>this.OnOperationResponse (mData));
		}
	}

	private void MyOnOperationResponse(Zealm.OperationResponse operationResponse)
	{
		lock(myLock)
		{
			listAction.Add (()=>this.OnOperationResponse (operationResponse));
		}
	}

	protected virtual void OnOperationResponse(Zealm.OperationResponse operationResponse)
	{

	}


	protected virtual void OnOperationResponse(ZMNetData mData)
	{

	}


	protected int maxNum=200;
	void Update()
	{
		lock(myLock)
		{
//			if(listAction.Count>0)
//			{
//				System.Action mAction=listAction[0];
//				listAction.RemoveAt (0);
//				mAction();
//			}
			
			//Debug.Log ("----------------------------------listActionCount:"+listAction.Count);
			if(listAction.Count<=maxNum)
			{
				foreach(System.Action action in listAction)
				{
					try
					{
						action();
					}
					catch(System.Exception ex)
					{
						Debug.Log(ex.ToString());
					}
				}
				listAction.Clear();
			}
			else if(listAction.Count>maxNum)
			{
				for(int i=0;i<maxNum;i++)
				{
					try
					{
						listAction[i]();
					}
					catch(System.Exception ex)
					{
						Debug.Log(ex.ToString());
					}
				}
				listAction.RemoveRange(0,maxNum);
			}
		}
	}

}

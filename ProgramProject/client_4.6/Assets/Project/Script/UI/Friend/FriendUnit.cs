using UnityEngine;
using System.Collections;

public class FriendUnit : MonoBehaviour {
	
	public UILabel label;
	
	private GameObject _myGo;
	
	void Awake()
	{
		_myGo=gameObject;
		_myGo.SetActive(false);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void showText(string msg)
	{
		_myGo.SetActive(true);
		label.text=msg;
	}
	
	public void closeToast()
	{
		_myGo.SetActive(false);
	}
	
}

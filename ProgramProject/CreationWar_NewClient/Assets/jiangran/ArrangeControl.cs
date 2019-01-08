using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrangeControl : MonoBehaviour {
	public GameObject ArrangeMe;
	public Transform parentArrange;
	private List<AuctionItem> ListAuctionItem = new List<AuctionItem>();
	// Use this for initialization
	void Start () {
	
	}

	Dictionary<string, GameObject> arrangeItems = new Dictionary<string, GameObject>();
	public void AddArrangeMeItem(string id , string name , string time){
		GameObject ArrangeObjItem = null;
		if(!arrangeItems.TryGetValue(id, out ArrangeObjItem)){
			ArrangeObjItem = (GameObject)Instantiate(ArrangeMe);
			arrangeItems.Add(id,ArrangeObjItem);
		}
		AuctionItem btnItem = ArrangeObjItem.GetComponent<AuctionItem>();
		btnItem.transform.parent = parentArrange;
		btnItem.transform.localPosition = Vector3.zero;
		btnItem.transform.localScale = Vector3.one;
		btnItem.lblTime.text = time ;
		ListAuctionItem.Add(btnItem);
	}
}
